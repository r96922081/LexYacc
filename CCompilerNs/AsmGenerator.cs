namespace CCompilerNs
{
    public class AsmGenerator
    {
        public string s = "";
        public static string outputFilePath = null;

        public virtual void EmitAsm()
        {

        }

        protected void Emit(string asm)
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
        public FunctionDeclare functionDeclare;
        public Stack<LoopStatement> loopStatementStack = new Stack<LoopStatement>();
        public Dictionary<string, string> stringLiteral = new Dictionary<string, string>();
        public Dictionary<string, StructDef> structDefs = new Dictionary<string, StructDef>();
        public Dictionary<string, Variable> gv = new Dictionary<string, Variable>();
        private List<FunctionDeclare> functionDecls = new List<FunctionDeclare>();

        public List<GlobalDeclare> globalDeclares = new List<GlobalDeclare>();
        private List<GlobalVariable> uninitedGv = new List<GlobalVariable>();
        private List<GlobalVariable> initedGv = new List<GlobalVariable>();

        private bool inited = false;

        private void DistributeTopLevels()
        {
            foreach (GlobalDeclare g in globalDeclares)
            {
                if (g is StructDef)
                {
                    StructDef structDef = g as StructDef;
                    Gv.program.structDefs.Add(structDef.name, structDef);
                }
                else if (g is FunctionDeclare)
                    functionDecls.Add((FunctionDeclare)g);
                else if (g is GlobalVariable)
                {
                    GlobalVariable gv2 = g as GlobalVariable;

                    if (gv2.int_value == null)
                    {
                        uninitedGv.Add(gv2);
                        Variable v = new Variable();
                        v.name = gv2.name;
                        v.typeInfo = gv2.typeInfo;
                        v.scope = VariableScopeEnum.global;
                        gv.Add(v.name, v);
                    }
                    else
                    {
                        initedGv.Add(gv2);
                        Variable v = new Variable();
                        v.name = gv2.name;
                        v.typeInfo = gv2.typeInfo;
                        v.scope = VariableScopeEnum.global;
                        gv.Add(v.name, v);
                    }
                }
            }
        }

        private void SetStructInfo()
        {
            foreach (StructDef s in structDefs.Values)
            {
                int offset = 0;
                foreach (StructField f in s.fields)
                {
                    f.offset = offset;
                    if (f.typeInfo.typeEnum == VariableTypeEnum.struct_type)
                    {
                        StructDef subStruct = Gv.program.structDefs[f.typeInfo.typeName];
                        f.typeInfo.SetSize(subStruct.size);
                    }

                    int count = 1;
                    for (int i = 0; i < f.typeInfo.arraySize.Count; i++)
                        count *= f.typeInfo.arraySize[i];

                    offset += f.typeInfo.GetSize() * count;

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
            foreach (var kvp in Gv.stringLiteral)
                Gv.program.stringLiteral.Add(kvp.Key, kvp.Value);

            Gv.stringLiteral.Clear();
            Gv.sn = 0;

            DistributeTopLevels();
            SetStructInfo();
            SetLocalStackOffset();
        }

        private void SetLocalStackOffset()
        {
            foreach (FunctionDeclare f in functionDecls)
                f.SetLocalStackOffset();
        }

        public override void EmitAsm()
        {
            Emit("\n#=================================================#\n");
            Emit("#" + Util.GetCallStack());
            Init();

            if (uninitedGv.Count != 0)
            {
                Emit(".bss\n");
                foreach (GlobalVariable gv in uninitedGv)
                    gv.EmitAsm();
                Emit("");
            }

            if (initedGv.Count != 0 || Gv.program.stringLiteral.Count != 0)
            {
                Emit(".data\n");
                foreach (GlobalVariable gv in initedGv)
                    gv.EmitAsm();
                foreach (string text in Gv.program.stringLiteral.Keys)
                    Emit(string.Format("{0}:  .asciz \"{1}\"", Gv.program.stringLiteral[text], text));
                Emit("");
            }

            Emit(".text\n");
            foreach (FunctionDeclare f in functionDecls)
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

                    int typeSize = -1;

                    if (typeInfo.typeEnum == VariableTypeEnum.struct_type)
                        typeSize = Util.GetStructDef(typeInfo.typeName).size;
                    else
                        typeSize = typeInfo.GetSize();


                    Emit(string.Format(".lcomm {0}, {1}", name, typeSize * count));
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
                param6
                param5
                shadow space 32 bytes
                return address
new %rbp     -> old %rbp (to set as %rsp after ret)
new %rbp - 8 -> local1
new %rbp - 16-> local2

                Callee's review:
                put %rcx, %rdx, %r8, %r9 in shadow space

                stack:
                param6
                param5
                param4
                param3
                param2
                param1
                return address
new %rbp     -> old %rbp (to set as %rsp after ret)
new %rbp - 8 -> local1
new %rbp - 16-> local2

             */

            int returnAddress = 8;

            if (functionName == "main")
                returnAddress = 0;
            else
                returnAddress = 8;

            for (int i = 0; i < paramsInOrder.Count; i++)
            {
                Variable p = paramsInOrder[i];
                p.stackOffset = returnAddress + (i + 1) * 8;
            }

            foreach (Variable l in localsInOrder)
            {
                int count = 1;

                if (l.typeInfo.arraySize.Count == 0)
                {
                    if (l.typeInfo.typeEnum == VariableTypeEnum.struct_type)
                    {
                        localSize += l.typeInfo.GetSize();
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

                    int size = count * l.typeInfo.GetSize();
                    size = (size + 7) / 8 * 8;
                    localSize += size;
                }

                l.stackOffset = -localSize;
                localDeclareMap[l.name].stackOffset = l.stackOffset;
            }
        }

        private void CopyFirst4ParamsToShadowSpace()
        {
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
        }

        private int GetStructSizeInParam()
        {
            int size = 0;
            foreach (var p in paramsInOrder)
            {
                if (p.typeInfo.typeEnum == VariableTypeEnum.struct_type)
                    size += p.typeInfo.GetSize();
            }

            return size;
        }

        private void CopyParamStruct(int oldLocalSize)
        {
            foreach (var p in paramsInOrder)
            {
                if (p.typeInfo.typeEnum != VariableTypeEnum.struct_type)
                    continue;

                int size = p.typeInfo.GetSize();
                int oldStackOffset = p.stackOffset;
                p.stackOffset = -(oldLocalSize + size);
                int oldAddress = p.stackOffset;

                /*  
                    # copy memory
                    mov $0x0000000000001000, %rsi  # Source address (64-bit)
                    mov $0x0000000000002000, %rdi  # Destination address (64-bit)
                    mov $24, %rcx                  # Number of bytes to copy
                    cld                            # clear flag, %rsi, %rdi increments while copy
                    rep movsb                      # Repeat move byte (RCX times)
                 */

                Emit(string.Format("mov {0}(%rbp), %rax", oldStackOffset));
                Emit(string.Format("mov %rax, %rsi"));
                Emit(string.Format("lea {0}(%rbp), %rax", p.stackOffset));
                Emit(string.Format("mov %rax, %rdi"));
                Emit(string.Format("mov ${0}, %rcx", size));
                Emit(string.Format("cld"));
                Emit(string.Format("rep movsb"));

                oldLocalSize += size;
            }
        }

        public override void EmitAsm()
        {
            Gv.program.functionDeclare = this;

            int oldLocalSize = localSize;
            localSize += GetStructSizeInParam();

            string asm = string.Format(@"#FunctionDeclare =>
.global {0}
{1}:
push %rbp
mov %rsp, %rbp
add ${2}, %rsp", functionName, functionName, -localSize);

            Emit(asm);

            CopyFirst4ParamsToShadowSpace();
            CopyParamStruct(oldLocalSize);

            foreach (Statement s in statements)
            {
                s.EmitAsm();
            }

            asm = string.Format(@"leave
ret
#<= FunctionDeclare");
            Emit(asm);

            Gv.program.functionDeclare = this;
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
        public BooleanExpressions booleanExpressions;
        public List<Statement> statements = new List<Statement>();
        public string matchLabel;

        public void EmitCmpAsm()
        {
            matchLabel = "branch_" + (Gv.sn++);

            // else case. no lhs, rhs
            if (booleanExpressions == null)
            {
                Emit("jmp " + matchLabel);
            }
            else
            {
                booleanExpressions.jmpLabel = matchLabel;
                booleanExpressions.jmpCondition = BooleanExpressions.JmpCondition.Match;
                booleanExpressions.EmitAsm();
            }

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
            Util.PushVariableAddress(variableId);
            Emit("pop %rbx");
            Emit("pop %rax");

            Variable variable = Util.GetVariableFrom_Local_Param_Global(variableId.name[0]);
            if (variableId.arrayIndexList[0].Count != 0 && variable.typeInfo.GetSize() == 1)
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

        /*
                stack:
                param6
                param5
                shadow space 32 bytes
                return address
new %rbp     -> old %rbp (to set as %rsp after ret)
new %rbp - 8 -> local1
new %rbp - 16-> local2
        
         */

        public override void EmitAsm()
        {
            Emit("#FunctionCallExpression =>");

            // 16-byte align seemed unnecessary, and it may has bug in my implementation
            // because some test case failed after uncommenting it
            /*
            Emit(string.Format("mov %rsp, %rax # stack 16-byte align, before calling function"));
            Emit(string.Format("andq $0xF, %rax"));
            string alignedLabel = string.Format("aligned_{0}", (Gv.sn++));
            if (parameters == null || parameters.Count % 2 == 0)
                Emit(string.Format("jz {0}", alignedLabel));
            else
                Emit(string.Format("jnz {0}", alignedLabel));
            Emit(string.Format("pushq $0"));
            Emit(string.Format("{0}:", alignedLabel));*/


            if (parameters != null)
            {
                // Additional arguments beyond the first four are passed on the stack, push in reserve order
                // to follow Win64 function calling conventions
                for (int i = parameters.Count - 1; i >= 4; i--)
                {
                    parameters[i].EmitAsm();
                    Emit(string.Format("pop %rax"));
                    Emit(string.Format("push %rax # push parameter onto stack"));
                }

                // The first four arguments (integer or pointer types) are passed in:  %rcx, %rdx, %r8, %r9
                // to follow Win64 function calling conventions
                if (parameters.Count >= 1)
                    parameters[0].EmitAsm();
                if (parameters.Count >= 2)
                    parameters[1].EmitAsm();
                if (parameters.Count >= 3)
                    parameters[2].EmitAsm();
                if (parameters.Count >= 4)
                    parameters[3].EmitAsm();

                if (parameters.Count >= 4)
                    Emit(string.Format("pop %r9"));
                if (parameters.Count >= 3)
                    Emit(string.Format("pop %r8"));
                if (parameters.Count >= 2)
                    Emit(string.Format("pop %rdx"));
                if (parameters.Count >= 1)
                    Emit(string.Format("pop %rcx"));
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

            /*
            Emit(string.Format("mov %rsp, %rax # restore stack 16-byte align fix, after calling function"));
            Emit(string.Format("andq $0xF, %rax"));
            alignedLabel = string.Format("aligned_{0}", (Gv.sn++));
            if (parameters == null || parameters.Count % 2 == 0)
                Emit(string.Format("jz {0}", alignedLabel));
            else
                Emit(string.Format("jnz {0}", alignedLabel));
            Emit(string.Format("pop %rax"));
            Emit(string.Format("{0}:", alignedLabel));*/


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
        public string? stringLiternal = null;
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
            else if (stringLiternal != null)
            {
                Emit(string.Format("lea {0}(%rip), %rax", Gv.program.stringLiteral[stringLiternal]));
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
            Variable variable = Util.GetVariableFrom_Local_Param_Global(variableId.name[0]);
            VariableAddressOrValue addrOrValue = Util.PushVariableAddress(variableId);
            Emit("pop %rbx");

            if (addrOrValue == VariableAddressOrValue.Address)
                Emit(string.Format("mov %rbx, %rax"));
            else if (variableId.arrayIndexList[0].Count != 0 && variable.typeInfo.GetSize() == 1)
                Emit(string.Format("movzbq (%rbx), %rax")); // case a[1][2]
            else
                Emit(string.Format("mov (%rbx), %rax")); // case local, or array with element size = 8

            Emit(string.Format("push %rax"));
        }
    }

    public class BooleanExpressions : Expression
    {
        public List<BooleanExpression> booleanExpressions = new List<BooleanExpression>();
        public List<string> ops = new List<string>();

        private List<List<BooleanExpression>> andGroups = new List<List<BooleanExpression>>();

        public string jmpLabel;
        public JmpCondition jmpCondition = JmpCondition.None;

        public enum JmpCondition
        {
            Match,
            NotMatch,
            None
        }

        private void SetAndGroups()
        {
            List<BooleanExpression> andGroup = new List<BooleanExpression>();
            andGroup.Add(booleanExpressions[0]);

            for (int i = 0; i < ops.Count; i++)
            {
                if (ops[i] == "&&")
                    andGroup.Add(booleanExpressions[i + 1]);
                else
                {
                    andGroups.Add(andGroup);
                    andGroup = new List<BooleanExpression>();
                    andGroup.Add(booleanExpressions[i + 1]);
                }
            }

            andGroups.Add(andGroup);
        }

        public override void EmitAsm()
        {
            SetAndGroups();
            string booleanFalseLabel = "boolean_false_label_" + (Gv.sn++);
            string booleanTrueLabel = "boolean_true_label_" + (Gv.sn++);
            string booleanNoJmpLabel = "boolean_no_jmp_label_" + (Gv.sn++);

            foreach (List<BooleanExpression> andGroup in andGroups)
            {
                string groupFalseLabel = "group_false_label_" + (Gv.sn++);
                string groupTrueLabel = "group_true_label_" + (Gv.sn++);

                foreach (BooleanExpression b in andGroup)
                {
                    b.lhs.EmitAsm();
                    b.rhs.EmitAsm();

                    Emit("pop %rbx");
                    Emit("pop %rax");

                    Emit("cmp %rbx, %rax");

                    if (b.op == "==")
                        Emit("jne " + groupFalseLabel);
                    else if (b.op == "!=")
                        Emit("je " + groupFalseLabel);
                    else if (b.op == ">")
                        Emit("jle " + groupFalseLabel);
                    else if (b.op == "<")
                        Emit("jge " + groupFalseLabel);
                    else if (b.op == "<=")
                        Emit("jg " + groupFalseLabel);
                    else if (b.op == ">=")
                        Emit("jl " + groupFalseLabel);
                }
                Emit(groupTrueLabel + ":");
                Emit("jmp " + booleanTrueLabel);
                Emit(groupFalseLabel + ":");
            }

            Emit(booleanFalseLabel + ":");
            if (jmpCondition == JmpCondition.NotMatch)
                Emit("jmp " + jmpLabel);
            Emit("jmp " + booleanNoJmpLabel);

            Emit(booleanTrueLabel + ":");
            if (jmpCondition == JmpCondition.Match)
                Emit("jmp " + jmpLabel);
            Emit(booleanNoJmpLabel + ":");
        }
    }

    public class LoopStatement : Statement
    {
        public string loopEndLabel;
        public string updaterLabel;
    }

    public class ForLoopStatement : LoopStatement
    {
        public AssignmentStatement initializer;
        public BooleanExpressions booleanExpressions;
        public AssignmentStatement updater;

        public List<Statement> statements = new List<Statement>();

        public string loopStartLabel;

        public override void EmitAsm()
        {
            Emit("#ForLoopStatement =>");
            loopStartLabel = "loop_start_" + (Gv.sn++);
            loopEndLabel = "loop_end_" + (Gv.sn++);
            updaterLabel = "updater_" + (Gv.sn++);

            initializer.EmitAsm();
            Emit(loopStartLabel + ":");

            booleanExpressions.jmpLabel = loopEndLabel;
            booleanExpressions.jmpCondition = BooleanExpressions.JmpCondition.NotMatch;
            booleanExpressions.EmitAsm();

            Gv.program.loopStatementStack.Push(this);
            foreach (Statement s in statements)
                s.EmitAsm();
            Gv.program.loopStatementStack.Pop();

            Emit(updaterLabel + ":");
            updater.EmitAsm();
            Emit("jmp " + loopStartLabel);
            Emit(loopEndLabel + ":");

            Emit("#<= ForLoopStatement");
        }
    }

    public class WhileLoopStatement : LoopStatement
    {
        public BooleanExpressions booleanExpressions;

        public List<Statement> statements = new List<Statement>();

        public string loopStartLabel;

        public override void EmitAsm()
        {

            Emit("#WhileLoopStatement =>");
            loopStartLabel = "loop_start_" + (Gv.sn++);
            loopEndLabel = "loop_end_" + (Gv.sn++);
            updaterLabel = "updater_" + (Gv.sn++);

            Emit(loopStartLabel + ":");

            booleanExpressions.jmpLabel = loopEndLabel;
            booleanExpressions.jmpCondition = BooleanExpressions.JmpCondition.NotMatch;
            booleanExpressions.EmitAsm();

            Gv.program.loopStatementStack.Push(this);
            foreach (Statement s in statements)
                s.EmitAsm();
            Gv.program.loopStatementStack.Pop();

            // continue will jmp to updater label
            Emit(updaterLabel + ":");
            Emit("jmp " + loopStartLabel);
            Emit(loopEndLabel + ":");

            Emit("#<= WhileLoopStatement");
        }
    }



    public class BreakStatement : Statement
    {
        public override void EmitAsm()
        {
            Emit("jmp " + Gv.program.loopStatementStack.Peek().loopEndLabel);
        }
    }

    public class ContinueStatement : Statement
    {
        public override void EmitAsm()
        {
            Emit("jmp " + Gv.program.loopStatementStack.Peek().updaterLabel);
        }
    }

    public class EmptyStatement : Statement
    {
        public override void EmitAsm()
        {
        }
    }
}
