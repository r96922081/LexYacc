namespace CCompilerNs
{
    public class Program : AstNode
    {
        public Program() : base("Program")
        {

        }

        public override void EmitCurrentAsm()
        {
            string asm = @".data
.text";
            Emit(asm);
        }

        public override void EmitAsm()
        {
            base.EmitAsm();
        }
    }

    public class FunDecl : AstNode
    {
        public VariableType returnType;
        public string functionName;
        public Dictionary<string, LocalVariable> paramMap = new Dictionary<string, LocalVariable>();
        public List<Statement> statements;

        public Dictionary<string, LocalVariable> localMap = new Dictionary<string, LocalVariable>();
        public int localSize = 0;

        public FunDecl() : base("FunDecl")
        {

        }

        public void AddParamVariable(LocalVariable p)
        {
            paramMap.Add(p.name, p);
            p.stackOffset = 8 + paramMap.Count * 8; // skip the first 8 byte of return address in stack
        }

        public void AddLocalVariable(DeclareStatement a)
        {
            LocalVariable l = new LocalVariable();
            l.name = a.name;
            l.type = a.type;
            l.arraySize = a.arraySize;
            localMap.Add(l.name, l);

            int count = 1;

            foreach (int size in a.arraySize)
                count *= size;

            localSize += a.type.size * count;

            l.stackOffset = -localSize;
            a.stackOffset = -localSize;
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

            LocalVariable l = Context.funDecl.localMap[name];

            if (arrayIndex.Count == 0)
            {
                value.EmitAsm();
                Emit("pop %rax");  // pop value
                Emit(string.Format("mov %rax, {0}(%rbp)", l.stackOffset));
            }
            // a[2][3] = xxx;
            else
            {
                value.EmitAsm();
                Util.SaveArrayIndexAddressToRbx(l, arrayIndex);

                Emit("pop %rax"); // value to %rax
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

            int localSize = Context.funDecl.localSize;

            if (parameters != null)
            {
                // Caller push in reserve order, callee pop in order
                for (int i = parameters.Count - 1; i >= 0; i--)
                {
                    parameters[i].EmitAsm();
                    Emit(string.Format("pop %rax"));
                    Emit(string.Format("push %rax # push parameter onto stack"));
                    localSize += 8;
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
                LocalVariable local = null;

                if (Context.funDecl.localMap.ContainsKey(variableName))
                    local = Context.funDecl.localMap[variableName];
                else if (Context.funDecl.paramMap.ContainsKey(variableName))
                    local = Context.funDecl.paramMap[variableName];
                else
                    throw new Exception("unknown variable " + variableName);

                int stackOffset = local.stackOffset;
                Emit(string.Format("mov {0}(%rbp), %rax", stackOffset));
                Emit(string.Format("push %rax"));
            }
            // case mulExpression: ID arrayIndex
            else if (variableName != null && arrayIndex.Count > 0)
            {
                LocalVariable local = null;

                if (Context.funDecl.localMap.ContainsKey(variableName))
                    local = Context.funDecl.localMap[variableName];
                else if (Context.funDecl.paramMap.ContainsKey(variableName))
                    local = Context.funDecl.paramMap[variableName];
                else
                    throw new Exception("unknown variable " + variableName);

                Util.SaveArrayIndexAddressToRbx(local, arrayIndex);

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
                        Emit(string.Format("mov $0, %rdx"));
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
}
