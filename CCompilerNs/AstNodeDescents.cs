namespace CCompilerNs
{
    public class Program : AstNode
    {
        public List<TopLevel> topLevels = new List<TopLevel>();

        private List<GlobalVariable> uninitedGv = new List<GlobalVariable>();
        private List<GlobalVariable> initedGv = new List<GlobalVariable>();
        private List<FunDecl> funDecls = new List<FunDecl>();
        private List<StructDef> structDefs = new List<StructDef>();
        private bool inited = false;


        public Program() : base("Program")
        {

        }

        private void DistributeTopLevels()
        {
            Context.gv.Clear();

            foreach (TopLevel t in topLevels)
            {
                if (t.comment)
                    continue;

                if (t.structDef != null)
                {
                    structDefs.Add(t.structDef);
                    Context.structDefs.Add(t.structDef.name, t.structDef);
                }
                else if (t.funDecl != null)
                    funDecls.Add(t.funDecl);
                else if (t.gv.int_value == null)
                {
                    uninitedGv.Add(t.gv);
                    Variable v = new Variable();
                    v.name = t.gv.name;
                    v.type = t.gv.type;
                    v.scope = VariableScopeEnum.global;
                    v.arraySize.AddRange(t.gv.arraySize);
                    Context.gv.Add(v.name, v);
                }
                else
                {
                    initedGv.Add(t.gv);
                    Variable v = new Variable();
                    v.name = t.gv.name;
                    v.type = t.gv.type;
                    v.scope = VariableScopeEnum.global;
                    v.arraySize.AddRange(t.gv.arraySize);
                    Context.gv.Add(v.name, v);
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
                    if (f.type.type == VariableTypeEnum.struct_type)
                    {
                        StructDef subStruct = Context.structDefs[f.type.typeName];
                        f.type.size = subStruct.size;
                    }

                    offset += f.type.size;

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

            DistributeTopLevels();
            SetStructInfo();
            SetLocalStackOffset();
        }

        private void SetLocalStackOffset()
        {
            foreach (FunDecl f in funDecls)
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
            foreach (FunDecl f in funDecls)
                f.EmitAsm();
            Emit("");
        }

        public override string ToString()
        {
            Init();

            if (childrenForPrint.Count == 0)
                childrenForPrint.AddRange(topLevels);

            return base.ToString();
        }
    }

    public class TopLevel : AstNode
    {
        public FunDecl funDecl;
        public GlobalVariable gv;
        public StructDef structDef;
        public bool comment = false;

        public TopLevel() : base("TopLevel")
        {

        }

        public override void EmitAsm()
        {
            if (funDecl != null)
                funDecl.EmitAsm();
            else if (gv != null)
                gv.EmitAsm();
        }

        public override string ToString()
        {
            if (childrenForPrint.Count == 0)
            {
                if (funDecl != null)
                    childrenForPrint.Add(funDecl);
                else if (gv != null)
                    childrenForPrint.Add(gv);
            }

            return base.ToString();
        }
    }

    public class GlobalVariable : AstNode
    {
        public VariableType type;
        public string name;
        public int? int_value;
        public List<int> arraySize = new List<int>();

        public GlobalVariable() : base("GlobalVariable")
        {

        }

        public override void EmitAsm()
        {
            if (int_value == null)
            {
                if (arraySize.Count == 0)
                    Emit(string.Format(".lcomm {0}, 8", name));
                else
                {
                    int count = 1;
                    foreach (int arraySize in arraySize)
                        count *= arraySize;
                    Emit(string.Format(".lcomm {0}, {1}", name, count * type.size));
                }
            }
            else
                Emit(string.Format("{0}: .quad {1}", name, int_value));
        }
    }


    public class FunDecl : AstNode
    {
        public VariableType returnType;
        public string functionName;

        public List<Variable> paramsInOrder = new List<Variable>();
        public Dictionary<string, Variable> paramMap = new Dictionary<string, Variable>();
        public List<Statement> statements;

        public List<Variable> localsInOrder = new List<Variable>();
        public Dictionary<string, Variable> localMap = new Dictionary<string, Variable>();
        public Dictionary<string, DeclareStatement> localDeclareMap = new Dictionary<string, DeclareStatement>();
        public int localSize = 0;

        public FunDecl() : base("FunDecl")
        {

        }

        public void AddParamVariable(Variable p)
        {
            paramMap.Add(p.name, p);
            paramsInOrder.Add(p);
        }

        public void AddLocalVariable(DeclareStatement a)
        {
            Variable l = new Variable();
            l.name = a.name;
            l.type = a.type;
            l.arraySize = a.arraySize;
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

                if (l.arraySize.Count == 0)
                {
                    localSize += 8 * count;
                }
                else
                {
                    foreach (int arraySize in l.arraySize)
                        count *= arraySize;

                    int size = count * l.type.size;
                    size = (size + 7) / 8 * 8;
                    localSize += size;
                }

                l.stackOffset = -localSize;
                localDeclareMap[l.name].stackOffset = l.stackOffset;
            }
        }

        public override void EmitAsm()
        {
            Context.funDecl = this;

            string asm = string.Format(@"#FunDecl =>
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
#<= FunDecl");
            Emit(asm);

            Context.funDecl = this;
        }
    }

    // Statement clear all result
    public class Statement : AstNode
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
        public string name;
        public Expression value;
        public List<Expression> arrayIndex = new List<Expression>();

        public AssignmentStatement() : base("AssignmentStatement")
        {

        }

        public override void EmitAsm()
        {
            Emit("#AssignmentStatement =>");

            Variable l = null;
            Variable p = null;
            Variable gv = null;

            Variable v = null;

            if (Context.funDecl.localMap.ContainsKey(name))
            {
                l = Context.funDecl.localMap[name];
                v = l;
            }
            else if (Context.funDecl.paramMap.ContainsKey(name))
            {
                p = Context.funDecl.paramMap[name];
                v = p;
            }
            else if (Context.gv.ContainsKey(name))
            {
                gv = Context.gv[name];
                v = gv;
            }

            if (arrayIndex.Count == 0)
            {
                value.EmitAsm();
                Emit("pop %rax");  // pop value

                if (v.stackOffset != -1)
                    Emit(string.Format("mov %rax, {0}(%rbp)", v.stackOffset));
                else
                    Emit(string.Format("mov %rax, {0}(%rip)", name));
            }
            // a[2][3] = xxx;
            else
            {
                value.EmitAsm();
                Util.SaveArrayIndexAddressToRbx(l, p, gv, arrayIndex);

                Emit("pop %rax"); // value to %rax                

                if (v.type.size == 1)
                    Emit("mov %al, (%rbx)");
                else
                    Emit("mov %rax, (%rbx)");
            }

            Emit("#<= AssignmentStatement");
        }
    }

    public class DeclareStatement : Statement
    {
        public VariableType type;
        public string name;
        public Expression value;
        public int stackOffset;
        public List<int> arraySize = new List<int>();

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
    public class Expression : AstNode
    {
        public Expression lhs = null;
        public string? op = null;
        public Expression rhs = null;
        public int? intValue = null;
        public string? variableName = null;
        public List<Expression> arrayIndex = new List<Expression>();
        public FunctionCallExpression? functionCall = null;

        public Expression() : base("Expression")
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
            // case mulExpression: ID
            else if (variableName != null && arrayIndex.Count == 0)
            {
                Variable local = null;
                Variable param = null;
                Variable gv = null;

                Variable variable = null;


                if (Context.funDecl.localMap.ContainsKey(variableName))
                {
                    local = Context.funDecl.localMap[variableName];
                    variable = local;
                }
                else if (Context.funDecl.paramMap.ContainsKey(variableName))
                {
                    param = Context.funDecl.paramMap[variableName];
                    variable = param;
                }
                else if (Context.gv.ContainsKey(variableName))
                {
                    gv = Context.gv[variableName];
                    variable = gv;
                }

                // If ID is array, then push address of array
                if (variable.arraySize.Count != 0)
                {
                    if (local != null)
                    {
                        Emit(string.Format("mov %rbp, %rax"));
                        Emit(string.Format("add ${0}, %rax", local.stackOffset));
                        Emit(string.Format("push %rax"));
                    }
                    else if (param != null)
                    {
                        Emit(string.Format("mov %rbp, %rax"));
                        Emit(string.Format("add ${0}, %rax", param.stackOffset));
                        Emit(string.Format("mov (%rax), %rax"));
                        Emit(string.Format("push %rax"));
                    }
                    else if (gv != null)
                    {
                        Emit(string.Format("lea {0}(%rip), %rax", variableName));
                        Emit(string.Format("push %rax"));
                    }
                }
                else
                {
                    if (variable.stackOffset != -1)
                        // local variable
                        Emit(string.Format("mov {0}(%rbp), %rax", variable.stackOffset));
                    else
                        // global variable
                        Emit(string.Format("mov {0}(%rip), %rax", variableName));

                    Emit(string.Format("push %rax"));
                }
            }
            // case mulExpression: ID arrayIndex
            else if (variableName != null && arrayIndex.Count > 0)
            {
                Variable local = null;
                Variable param = null;
                Variable globalVariable = null;

                Variable variable = null;

                if (Context.funDecl.localMap.ContainsKey(variableName))
                {
                    local = Context.funDecl.localMap[variableName];
                    variable = local;
                }
                else if (Context.funDecl.paramMap.ContainsKey(variableName))
                {
                    param = Context.funDecl.paramMap[variableName];
                    variable = param;
                }
                else if (Context.gv.ContainsKey(variableName))
                {
                    globalVariable = Context.gv[variableName];
                    variable = globalVariable;
                }

                Util.SaveArrayIndexAddressToRbx(local, param, globalVariable, arrayIndex);

                Emit(string.Format("movq $0, %rax"));

                if (variable.type.size == 1)
                    Emit(string.Format("movzbq (%rbx), %rax"));
                else
                    Emit(string.Format("mov (%rbx), %rax"));

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
            // case mulExpression: ID
            else if (variableName != null)
            {
                s = "Expression(" + variableName + ")";
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

            Context.forLoopStatementStack.Push(this);
            foreach (Statement s in statements)
                s.EmitAsm();
            Context.forLoopStatementStack.Pop();

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
            Emit("jmp " + Context.forLoopStatementStack.Peek().loopEndLabel);
        }
    }

    public class ContinueStatement : Statement
    {
        public ContinueStatement() : base("ContinueStatement")
        {

        }

        public override void EmitAsm()
        {
            Emit("jmp " + Context.forLoopStatementStack.Peek().updaterLabel);
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

    public class StructDef : AstNode
    {
        public string name;
        public List<StructField> fields = new List<StructField>();
        public int size;
    };

    public class StructField : AstNode
    {
        public VariableType type;
        public List<int> arraySize = new List<int>();
        public string name;
        public int offset;
    };
}
