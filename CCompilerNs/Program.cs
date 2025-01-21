namespace CCompilerNs
{
    public class ProgramBase
    {
        public string s = "";

        public ProgramBase()
        {

        }

        public ProgramBase(string s)
        {

        }

        public virtual void EmitAsm()
        {

        }

        public void Emit(string asm)
        {
            AsmEmitter.Emit(asm);
        }
    }

    public class GlobalDeclare : ProgramBase
    {
        public GlobalDeclare()
        {

        }

        public GlobalDeclare(string s) : base(s)
        {

        }
    }



    public class Program : ProgramBase
    {
        public List<GlobalDeclare> globalDeclares = new List<GlobalDeclare>();

        private List<GlobalVariable> uninitedGv = new List<GlobalVariable>();
        private List<GlobalVariable> initedGv = new List<GlobalVariable>();
        private List<FunctionDeclare> funDecls = new List<FunctionDeclare>();
        private List<StructDef> structDefs = new List<StructDef>();
        private bool inited = false;


        public Program() : base("Program")
        {

        }

        private void DistributeTopLevels()
        {
            Gv.context.gv.Clear();

            foreach (GlobalDeclare g in globalDeclares)
            {
                if (g is StructDef)
                {
                    StructDef structDef = g as StructDef;
                    structDefs.Add(structDef);
                    Gv.context.structDefs.Add(structDef.name, structDef);
                }
                else if (g is FunctionDeclare)
                    funDecls.Add((FunctionDeclare)g);
                else if (g is GlobalVariable)
                {
                    GlobalVariable gv = g as GlobalVariable;

                    if (gv.int_value == null)
                    {
                        uninitedGv.Add(gv);
                        Variable v = new Variable();
                        v.name = gv.name;
                        v.typeInfo = gv.typeInfo;
                        v.scope = VariableScopeEnum.global;
                        v.typeInfo.arraySize.AddRange(gv.typeInfo.arraySize);
                        Gv.context.gv.Add(v.name, v);
                    }
                    else
                    {
                        initedGv.Add(gv);
                        Variable v = new Variable();
                        v.name = gv.name;
                        v.typeInfo = gv.typeInfo;
                        v.scope = VariableScopeEnum.global;
                        v.typeInfo.arraySize.AddRange(gv.typeInfo.arraySize);
                        Gv.context.gv.Add(v.name, v);
                    }
                }
            }
        }

        private void SetStructInfo()
        {
            foreach (StructDef s in structDefs)
            {
                int offset = 0;
                foreach (StructField f in s.fields)
                {
                    f.offset = offset;
                    if (f.typeInfo.typeEnum == VariableTypeEnum.struct_type)
                    {
                        StructDef subStruct = Gv.context.structDefs[f.typeInfo.typeName];
                        f.typeInfo.size = subStruct.size;
                    }

                    offset += f.typeInfo.size;

                    // align 8 bytes
                    offset = (offset + 7) / 8 * 8;
                }
                s.size = offset;
            }
        }

        private void Init()
        {
            if (inited)
                return;

            inited = true;

            Gv.program = this;
            DistributeTopLevels();
            SetStructInfo();
            SetLocalStackOffset();
        }

        private void SetLocalStackOffset()
        {
            foreach (FunctionDeclare f in funDecls)
                f.SetLocalStackOffset();
        }

        public override void EmitAsm()
        {
            Init();

            if (uninitedGv.Count != 0)
            {
                Emit(".bss");
                foreach (GlobalVariable gv in uninitedGv)
                    gv.EmitAsm();
                Emit("");
            }

            if (initedGv.Count != 0)
            {
                Emit(".data\n");
                foreach (GlobalVariable gv in initedGv)
                    gv.EmitAsm();
                Emit("");
            }

            Emit(".text\n");
            foreach (FunctionDeclare f in funDecls)
                f.EmitAsm();
            Emit("");
        }

        public override string ToString()
        {
            Init();

            return base.ToString();
        }
    }

    public class GlobalVariable : GlobalDeclare
    {
        public VariableTypeInfo typeInfo;
        public string name;
        public int? int_value;

        public GlobalVariable() : base("GlobalVariable")
        {

        }

        public override void EmitAsm()
        {
            if (int_value == null)
            {
                if (typeInfo.arraySize.Count == 0)
                    Emit(string.Format(".lcomm {0}, 8", name));
                else
                {
                    int count = 1;
                    foreach (int arraySize in typeInfo.arraySize)
                        count *= arraySize;
                    Emit(string.Format(".lcomm {0}, {1}", name, count * typeInfo.size));
                }
            }
            else
                Emit(string.Format("{0}: .quad {1}", name, int_value));
        }
    }


    public class FunctionDeclare : GlobalDeclare
    {
        public VariableTypeInfo returnTypeInfo;
        public string functionName;

        public List<Variable> paramsInOrder = new List<Variable>();
        public Dictionary<string, Variable> paramMap = new Dictionary<string, Variable>();
        public List<Statement> statements;

        public List<Variable> localsInOrder = new List<Variable>();
        public Dictionary<string, Variable> localMap = new Dictionary<string, Variable>();
        public Dictionary<string, DeclareStatement> localDeclareMap = new Dictionary<string, DeclareStatement>();
        public int localSize = 0;

        public FunctionDeclare() : base("FunctionDeclare")
        {

        }

        public void AddParamVariable(Variable p)
        {
            p.scope = VariableScopeEnum.param;
            paramMap.Add(p.name, p);
            paramsInOrder.Add(p);
        }

        public void AddLocalVariable(DeclareStatement a)
        {
            Variable l = new Variable();
            l.name = a.name;
            l.typeInfo = a.typeInfo;
            l.typeInfo.arraySize = a.typeInfo.arraySize;
            l.scope = VariableScopeEnum.local;

            localMap.Add(l.name, l);
            localDeclareMap.Add(l.name, a);
            localsInOrder.Add(l);
        }

        public void SetLocalStackOffset()
        {
            if (functionName != "main")
                localSize += 8;
            /*
            stack:
            param1
            param2
            %rbp->  return address
            local1
            local2
             */
            for (int i = 0; i < paramsInOrder.Count; i++)
            {
                Variable p = paramsInOrder[i];
                p.stackOffset = 8 + (i + 1) * 8;
            }

            foreach (Variable l in localsInOrder)
            {
                int count = 1;

                if (l.typeInfo.arraySize.Count == 0)
                {
                    if (l.typeInfo.typeEnum == VariableTypeEnum.struct_type)
                    {
                        l.typeInfo.UpdateStructSize();
                        localSize += l.typeInfo.size;
                    }
                    else
                    {
                        // set non-array, no struct local variable size to 8 bytes, even for char type
                        localSize += 8;
                    }
                }
                else
                {
                    foreach (int arraySize in l.typeInfo.arraySize)
                        count *= arraySize;

                    int size = count * l.typeInfo.size;
                    size = (size + 7) / 8 * 8;
                    localSize += size;
                }

                l.stackOffset = -localSize;
                localDeclareMap[l.name].stackOffset = l.stackOffset;
            }
        }

        public override void EmitAsm()
        {
            Gv.context.functionDeclare = this;

            string asm = string.Format(@"#FunctionDeclare =>
.global {0}
{1}:
push %rbp
mov %rsp, %rbp
add ${2}, %rsp", functionName, functionName, -localSize);

            Emit(asm);
            foreach (Statement s in statements)
            {
                s.EmitAsm();
            }

            asm = string.Format(@"leave
ret
#<= FunctionDeclare");
            Emit(asm);

            Gv.context.functionDeclare = this;
        }
    }

    // Statement clear all result
    public class Statement : ProgramBase
    {
        public Statement() : base("Statement")
        {

        }

        public Statement(string s) : base(s)
        {

        }
    }

    public class CompoundIfStatement : Statement
    {
        public IfStatement ifStatement;
        public List<IfStatement> elseIfStatements;
        public IfStatement elseStatement;

        public CompoundIfStatement() : base("CompoundIfStatement")
        {

        }

        public override void EmitAsm()
        {
            ifStatement.EmitCmpAsm();

            if (elseIfStatements != null)
            {
                foreach (IfStatement s in elseIfStatements)
                    s.EmitCmpAsm();
            }

            if (elseStatement != null)
                elseStatement.EmitCmpAsm();


            string endCompoundIfLabel = "branch_compound_if_end_" + +(Gv.sn++);

            Emit("jmp " + endCompoundIfLabel);

            ifStatement.EmitSubstatementsAsm(endCompoundIfLabel);

            if (elseIfStatements != null)
            {
                foreach (IfStatement s in elseIfStatements)
                    s.EmitSubstatementsAsm(endCompoundIfLabel);
            }

            if (elseStatement != null)
                elseStatement.EmitSubstatementsAsm(endCompoundIfLabel);


            Emit(endCompoundIfLabel + ":");
        }
    }

    public class IfStatement : Statement
    {
        public Expression lhs;
        public string op;
        public Expression rhs;
        public List<Statement> statements = new List<Statement>();
        public string matchLabel;

        public IfStatement() : base("IfStatement")
        {

        }

        public void EmitCmpAsm()
        {
            matchLabel = "branch_" + (Gv.sn++);

            // else case. no lhs, rhs
            if (op == null || op.Length == 0)
            {

            }
            else
            {
                // expression saves result in stack
                lhs.EmitAsm();
                rhs.EmitAsm();

                Emit("pop %rbx");
                Emit("pop %rax");

                Emit("cmp %rbx, %rax");
            }

            if (op == "==")
                Emit("je " + matchLabel);
            else if (op == "!=")
                Emit("jne " + matchLabel);
            else if (op == ">")
                Emit("jg " + matchLabel);
            else if (op == "<")
                Emit("jl " + matchLabel);
            else if (op == "<=")
                Emit("jle " + matchLabel);
            else if (op == ">=")
                Emit("jge " + matchLabel);
            else
                Emit("jmp " + matchLabel);
        }

        public void EmitSubstatementsAsm(string endCompoundIfLabel)
        {
            Emit(matchLabel + ":");
            foreach (Statement s in statements)
                s.EmitAsm();
            Emit("jmp " + endCompoundIfLabel);
        }
    }

    public class ReturnStatement : Statement
    {
        public Expression returnValue;

        public ReturnStatement() : base("ReturnStatement")
        {

        }

        public override void EmitAsm()
        {
            Emit("#ReturnStatement =>");
            if (returnValue != null)
            {
                returnValue.EmitAsm();
                Emit("pop %rax");
            }

            string leave = @"leave
ret";
            Emit(leave);
            Emit("#<= ReturnStatement");
        }
    }

    public class AssignmentStatement : Statement
    {
        public VariableId variableId;
        public Expression value;

        public AssignmentStatement() : base("AssignmentStatement")
        {

        }

        public override void EmitAsm()
        {
            Emit("#AssignmentStatement =>");

            Variable variable = Util.GetVariableFrom_Local_Param_Global(variableId.name[0]);

            value.EmitAsm();
            Util.SaveVariableAddressToRbx(variable, variableId);
            Emit("pop %rax");

            if (variableId.arrayIndexList[0].Count != 0 && variable.typeInfo.size == 1)
                Emit("mov %al, (%rbx)");
            else
                Emit("mov %rax, (%rbx)");

            Emit("#<= AssignmentStatement");
        }
    }

    public class DeclareStatement : Statement
    {
        public VariableTypeInfo typeInfo;
        public string name;
        public Expression value;
        public int stackOffset;

        public DeclareStatement() : base("DeclareStatement")
        {

        }

        public override void EmitAsm()
        {
            Emit("#DeclareStatement =>");
            if (value != null)
            {
                value.EmitAsm();
                Emit("pop %rax");
            }

            Emit(string.Format("mov %rax, {0}(%rbp)", stackOffset));
            Emit("#<= DeclareStatement");
        }
    }

    public class FunctionCallExpression : Statement
    {
        public string name;
        public List<Expression> parameters;

        public FunctionCallExpression() : base("FunctionCallExpression")
        {

        }

        public override void EmitAsm()
        {
            Emit("#FunctionCallExpression =>");

            if (parameters != null)
            {
                // Caller push in reserve order, callee pop in order
                for (int i = parameters.Count - 1; i >= 0; i--)
                {
                    parameters[i].EmitAsm();
                    Emit(string.Format("pop %rax"));
                    Emit(string.Format("push %rax # push parameter onto stack"));
                }
            }

            Emit(string.Format("call {0}", name));

            if (parameters != null)
            {
                // Clear parameters
                for (int i = 0; i < parameters.Count; i++)
                {
                    Emit(string.Format("pop %rbx # clear parameter on stack"));
                }
            }

            Emit("#<= FunctionCallExpression");
        }
    }

    // save result in stack
    public class Expression : ProgramBase
    {
        public Expression lhs = null;
        public string? op = null;
        public Expression rhs = null;
        public int? intValue = null;
        public FunctionCallExpression? functionCall = null;

        public Expression() : base("Expression")
        {

        }

        public Expression(string s) : base(s)
        {

        }

        public override void EmitAsm()
        {
            // case mulExpression: INT_VALUE
            if (intValue != null)
            {
                Emit(string.Format("mov ${0}, %rax", intValue));
                Emit(string.Format("push %rax"));
            }
            // case mulExpression: functionCall
            else if (functionCall != null)
            {
                functionCall.EmitAsm();
                Emit(string.Format("push %rax"));
            }
            else
            {
                if (rhs == null)
                {
                    lhs.EmitAsm();
                    Emit(string.Format("pop %rax"));
                    Emit(string.Format("push %rax"));
                }
                else
                {
                    lhs.EmitAsm();

                    // case addExpression: addExpression '+' mulExpression
                    if (rhs is Expression)
                    {
                        rhs.EmitAsm();
                    }
                    // case mulExpression: mulExpression '*' INT_VALUE
                    else
                    {
                        Emit(string.Format("mov ${0}, %rax", intValue));
                        Emit(string.Format("push %rax"));
                    }

                    Emit(string.Format("pop %rbx"));
                    Emit(string.Format("pop %rax"));

                    if (op == "+")
                        Emit(string.Format("add %rbx, %rax"));
                    else if (op == "-")
                        Emit(string.Format("sub %rbx, %rax"));
                    else if (op == "*")
                        Emit(string.Format("mul %rbx"));
                    else if (op == "/")
                    {
                        Emit(string.Format("movq $0, %rdx"));
                        Emit(string.Format("div %rbx"));
                    }

                    Emit(string.Format("push %rax"));
                }
            }
        }

        public override string ToString()
        {
            // case mulExpression: INT_VALUE
            if (intValue != null)
            {
                s = "Expression(" + intValue + ")";
            }
            // case mulExpression: functionCall
            else if (functionCall != null)
            {
                s = "Expression(" + functionCall.name + "())";
            }
            else
            {
                // case mulExpression: '(' addExpression ')'
                if (rhs == null)
                {
                    s = "Expression";
                }
                // case addExpression: addExpression '+' mulExpression
                else
                {
                    s = "Expression(" + op + ")";
                }
            }

            return base.ToString();
        }
    }

    // save result in stack
    public class VariableIdExpression : Expression
    {
        public VariableId variableId;

        public VariableIdExpression() : base("VariableIdExpression")
        {
        }

        public override void EmitAsm()
        {
            Variable variable = Util.GetVariableFrom_Local_Param_Global(variableId.name[0]);

            Util.SaveVariableAddressToRbx(variable, variableId);

            if (variableId.arrayIndexList[0].Count != 0 && variable.typeInfo.size == 1)
                Emit(string.Format("movzbq (%rbx), %rax")); // case a[1][2]
            else if (variableId.arrayIndexList[0].Count == 0 && variable.typeInfo.arraySize.Count != 0)
                Emit(string.Format("mov %rbx, %rax")); // case a[1][2], but pass a as parameter
            else
                Emit(string.Format("mov (%rbx), %rax")); // case local, or array with element size = 8

            Emit(string.Format("push %rax"));
        }
    }

    public class ForLoopStatement : Statement
    {
        public AssignmentStatement initializer;
        public Expression conditionLhs;
        public string conditionOp;
        public Expression conditionrhs;
        public AssignmentStatement updater;

        public List<Statement> statements = new List<Statement>();

        public string loopStartLabel;
        public string loopEndLabel;
        public string updaterLabel;

        public ForLoopStatement() : base("ForLoopStatement")
        {

        }

        public override void EmitAsm()
        {
            Emit("#ForLoopStatement =>");
            loopStartLabel = "loop_start_" + (Gv.sn++);
            loopEndLabel = "loop_end_" + (Gv.sn++);
            updaterLabel = "updater_" + (Gv.sn++);

            initializer.EmitAsm();
            Emit("push %rax");
            Emit(loopStartLabel + ":");

            // check condition
            conditionLhs.EmitAsm();
            conditionrhs.EmitAsm();

            Emit("pop %rbx");
            Emit("pop %rax");

            Emit("cmp %rbx, %rax");

            if (conditionOp == "==")
                Emit("jne " + loopEndLabel);
            else if (conditionOp == "!=")
                Emit("je " + loopEndLabel);
            else if (conditionOp == ">")
                Emit("jle " + loopEndLabel);
            else if (conditionOp == "<")
                Emit("jge " + loopEndLabel);
            else if (conditionOp == "<=")
                Emit("jg " + loopEndLabel);
            else if (conditionOp == ">=")
                Emit("jl " + loopEndLabel);

            Gv.context.forLoopStatementStack.Push(this);
            foreach (Statement s in statements)
                s.EmitAsm();
            Gv.context.forLoopStatementStack.Pop();

            Emit(updaterLabel + ":");
            updater.EmitAsm();
            Emit("jmp " + loopStartLabel);
            Emit(loopEndLabel + ":");

            Emit("#<= ForLoopStatement");
        }
    }

    public class BreakStatement : Statement
    {
        public BreakStatement() : base("BreakStatement")
        {

        }

        public override void EmitAsm()
        {
            Emit("jmp " + Gv.context.forLoopStatementStack.Peek().loopEndLabel);
        }
    }

    public class ContinueStatement : Statement
    {
        public ContinueStatement() : base("ContinueStatement")
        {

        }

        public override void EmitAsm()
        {
            Emit("jmp " + Gv.context.forLoopStatementStack.Peek().updaterLabel);
        }
    }

    public class EmptyStatement : Statement
    {
        public EmptyStatement() : base("EmptyStatement")
        {

        }

        public override void EmitAsm()
        {
        }
    }

    public class StructDef : GlobalDeclare
    {
        public string name;
        public List<StructField> fields = new List<StructField>();
        public int size;
    };

    public class StructField : ProgramBase
    {
        public VariableTypeInfo typeInfo;
        public string name;
        public int offset;
    };

    // a[1].b.c[2][3].d
    public class VariableId : ProgramBase
    {
        public List<string> name = new List<string>();
        public List<List<Expression>> arrayIndexList = new List<List<Expression>>();
    }
}
