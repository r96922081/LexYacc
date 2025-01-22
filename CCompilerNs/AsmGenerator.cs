namespace CCompilerNs
{
    public class AsmGenerator
    {
        public string s = "";
        public static string outputFilePath = null;

        public AsmGenerator()
        {

        }

        public AsmGenerator(string s)
        {

        }

        public virtual void EmitAsm()
        {

        }

        public void Emit(string asm)
        {
            EmitToChannel(asm);
        }

        public static void EmitToChannel(string asm)
        {
            Console.WriteLine(asm);
            if (outputFilePath != null)
                File.AppendAllText(outputFilePath, asm + "\n");
        }

        public static void SetOutputFile(string filePath)
        {
            outputFilePath = filePath;
            File.WriteAllText(outputFilePath, "");
        }
    }

    public class GlobalDeclare : AsmGenerator
    {
        public GlobalDeclare()
        {

        }
    }



    public class Program : AsmGenerator
    {
        public List<GlobalDeclare> globalDeclares = new List<GlobalDeclare>();

        private List<GlobalVariable> uninitedGv = new List<GlobalVariable>();
        private List<GlobalVariable> initedGv = new List<GlobalVariable>();
        private List<FunctionDeclare> funDecls = new List<FunctionDeclare>();
        private List<StructDef> structDefs = new List<StructDef>();
        private bool inited = false;

        private void DistributeTopLevels()
        {
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

            Gv.context.Clear();
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
            Emit("\n#=================================================#\n");
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
    }

    public class GlobalVariable : GlobalDeclare
    {
        public VariableTypeInfo typeInfo;
        public string name;
        public int? int_value;

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
            /*
                Caller's review:
                First 4 param: %rcx, %rdx, %r8, %r9 
            
                stack:
                param5
                param6
                shadow space 32 bytes
                return address
new %rbp     -> old %rbp (to set as %rsp after ret)
new %rbp - 8 -> local1
new %rbp - 16-> local2

                Callee's review:
                put %rcx, %rdx, %r8, %r9 in shadow space

                stack:
                aram5
                param6
                param1
                param2
                param3
                param4
                return address
new %rbp     -> old %rbp (to set as %rsp after ret)
new %rbp - 8 -> local1
new %rbp - 16-> local2

             */

            int shadowSpace = 32;
            int returnAddress = 8;

            if (functionName == "main")
                returnAddress = 0;
            else
                returnAddress = 8;

            if (paramsInOrder.Count >= 1)
            {
                Variable p = paramsInOrder[0];
                p.stackOffset = returnAddress + 32;
            }

            if (paramsInOrder.Count >= 2)
            {
                Variable p = paramsInOrder[1];
                p.stackOffset = returnAddress + 24;
            }

            if (paramsInOrder.Count >= 3)
            {
                Variable p = paramsInOrder[2];
                p.stackOffset = returnAddress + 16;
            }

            if (paramsInOrder.Count >= 4)
            {
                Variable p = paramsInOrder[3];
                p.stackOffset = returnAddress + 8;
            }

            for (int i = 4; i < paramsInOrder.Count; i++)
            {
                Variable p = paramsInOrder[i];
                p.stackOffset = returnAddress + shadowSpace + (i + 1) * 8;
            }

            foreach (Variable l in localsInOrder)
            {
                int count = 1;

                if (l.typeInfo.arraySize.Count == 0)
                {
                    if (l.typeInfo.typeEnum == VariableTypeEnum.struct_type)
                    {
                        l.typeInfo.UpdateStructInfo();
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

            // copy first 4 param to shadow space
            if (paramsInOrder.Count >= 1)
            {
                Variable p = paramsInOrder[0];
                Emit(string.Format("lea {0}(%rbp), %rax", p.stackOffset));
                Emit(string.Format("mov %rcx, (%rax)"));
            }
            if (paramsInOrder.Count >= 2)
            {
                Variable p = paramsInOrder[1];
                Emit(string.Format("lea {0}(%rbp), %rax", p.stackOffset));
                Emit(string.Format("mov %rdx, (%rax)"));
            }
            if (paramsInOrder.Count >= 3)
            {
                Variable p = paramsInOrder[2];
                Emit(string.Format("lea {0}(%rbp), %rax", p.stackOffset));
                Emit(string.Format("mov %r8, (%rax)"));
            }
            if (paramsInOrder.Count >= 4)
            {
                Variable p = paramsInOrder[3];
                Emit(string.Format("lea {0}(%rbp), %rax", p.stackOffset));
                Emit(string.Format("mov %r9, (%rax)"));
            }

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
    public class Statement : AsmGenerator
    {
    }

    public class CompoundIfStatement : Statement
    {
        public IfStatement ifStatement;
        public List<IfStatement> elseIfStatements;
        public IfStatement elseStatement;

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

        public override void EmitAsm()
        {
            Emit("#AssignmentStatement =>");

            value.EmitAsm();
            Util.SaveVariableAddressToRbx(variableId);
            Emit("pop %rax");

            Variable variable = Util.GetVariableFrom_Local_Param_Global(variableId.name[0]);
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

        public override void EmitAsm()
        {
            Emit("#FunctionCallExpression =>");

            if (parameters != null)
            {
                // The first four arguments (integer or pointer types) are passed in:  %rcx, %rdx, %r8, %r9
                // to follow Win64 function calling conventions
                if (parameters.Count >= 1)
                {
                    parameters[0].EmitAsm();
                    Emit(string.Format("pop %rcx"));
                }

                if (parameters.Count >= 2)
                {
                    parameters[1].EmitAsm();
                    Emit(string.Format("pop %rdx"));
                }

                if (parameters.Count >= 3)
                {
                    parameters[2].EmitAsm();
                    Emit(string.Format("pop %r8"));
                }

                if (parameters.Count >= 4)
                {
                    parameters[3].EmitAsm();
                    Emit(string.Format("pop %r9"));
                }

                // Additional arguments beyond the first four are passed on the stack, push in reserve order
                // to follow Win64 function calling conventions
                for (int i = parameters.Count - 1; i >= 4; i--)
                {
                    parameters[i].EmitAsm();
                    Emit(string.Format("pop %rax"));
                    Emit(string.Format("push %rax # push parameter onto stack"));
                }
            }

            Emit(string.Format("add $-32, %rsp")); // 32 byte shadow space to follow Win64 function calling conventions
            Emit(string.Format("call {0}", name));
            Emit(string.Format("add $32, %rsp"));

            if (parameters != null)
            {
                // Clear parameters
                for (int i = 4; i < parameters.Count; i++)
                {
                    Emit(string.Format("pop %rbx # clear parameter on stack")); // caller cleaning up the stack to follow Win64 function calling conventions
                }
            }

            Emit("#<= FunctionCallExpression");
        }
    }

    // save result in stack
    public class Expression : AsmGenerator
    {
        public Expression lhs = null;
        public string? op = null;
        public Expression rhs = null;
        public int? intValue = null;
        public FunctionCallExpression? functionCall = null;

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
    }

    // save result in stack
    public class VariableIdExpression : Expression
    {
        public VariableId variableId;

        public override void EmitAsm()
        {
            Util.SaveVariableAddressToRbx(variableId);

            Variable variable = Util.GetVariableFrom_Local_Param_Global(variableId.name[0]);
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
        public override void EmitAsm()
        {
            Emit("jmp " + Gv.context.forLoopStatementStack.Peek().loopEndLabel);
        }
    }

    public class ContinueStatement : Statement
    {
        public override void EmitAsm()
        {
            Emit("jmp " + Gv.context.forLoopStatementStack.Peek().updaterLabel);
        }
    }

    public class EmptyStatement : Statement
    {
        public override void EmitAsm()
        {
        }
    }
}
