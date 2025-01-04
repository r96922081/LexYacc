namespace CCompilerNs
{
    public class Program : AstNode
    {
        public Program() : base("Program")
        {

        }

        public override void EmitCurrentAsm()
        {
            string asm = @"
.data

.text
";
            Emit(asm);
        }

        public override void EmitAsm()
        {
            base.EmitAsm();
            Emit("");
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
            localMap.Add(l.name, l);

            localSize += a.type.size;

            l.stackOffset = -localSize;
            a.stackOffset = -localSize;
        }

        public override void EmitAsm()
        {
            Context.funDecl = this;

            string asm = string.Format(@"
#FunDecl =>
.global {0}
{1}:
push %rbp
mov %rsp, %rbp

add ${2}, %rsp
", functionName, functionName, -localSize);

            Emit(asm);
            foreach (Statement s in statements)
            {
                s.EmitAsm();
            }

            /*int alignStackSize = localSize % 16 == 0 ? 0 : 16 - localSize % 16;
            if (alignStackSize != 0)
                Emit(string.Format("add ${0}, %rsp", -alignStackSize));*/

            asm = string.Format(@"
leave
ret
#<= FunDecl
");
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

            string leave = @"
leave
ret";
            Emit(leave);
            Emit("#<= ReturnStatement\n");
        }
    }

    public class AssignmentStatement : Statement
    {
        public string name;
        public Expression value;

        public AssignmentStatement() : base("AssignmentStatement")
        {

        }

        public override void EmitAsm()
        {
            Emit("#AssignmentStatement =>");

            value.EmitAsm();
            LocalVariable l = Context.funDecl.localMap[name];

            Emit("pop %rax");
            Emit(string.Format("mov %rax, {0}(%rbp)", l.stackOffset));
            Emit("#<= AssignmentStatement\n");
        }
    }

    public class DeclareStatement : Statement
    {
        public VariableType type;
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
            Emit("#<= DeclareStatement\n");
        }
    }

    public class FunctionCallStatement : Statement
    {
        public string name;
        public List<Expression> parameters;

        public FunctionCallStatement() : base("FunctionCallStatement")
        {

        }

        public override void EmitAsm()
        {
            Emit("#FunctionCallStatement =>");

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

            /*int alignStackSize = localSize % 16 == 0 ? 0 : 16 - localSize % 16;
            if (alignStackSize != 0)
                Emit(string.Format("add ${0}, %rsp # align stack 16-byte boundary", -alignStackSize));*/

            Emit(string.Format("call {0}", name));

            if (parameters != null)
            {
                // Clear parameters
                for (int i = 0; i < parameters.Count; i++)
                {
                    Emit(string.Format("pop %rbx # clear parameter on stack"));
                }
            }

            /*if (alignStackSize != 0)
                Emit(string.Format("add ${0}, %rsp", alignStackSize));*/

            Emit("#<= FunctionCallStatement\n");
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
        public FunctionCallStatement? functionCall = null;

        public Expression() : base("Expression")
        {

        }

        public override void EmitAsm()
        {
            // case mulExpression: INT_VALUE
            if (intValue != null)
            {
                Emit(string.Format("mov ${0}, %rax", intValue));
                Emit(string.Format("push %rax\n"));
            }
            // case mulExpression: ID
            else if (variableName != null)
            {
                LocalVariable local = null;

                if (Context.funDecl.localMap.ContainsKey(variableName))
                    local = Context.funDecl.localMap[variableName];
                else if (Context.funDecl.paramMap.ContainsKey(variableName))
                    local = Context.funDecl.paramMap[variableName];
                else
                    throw new Exception("unknown variable " + variableName);

                Emit(string.Format("mov {0}(%rbp), %rax", local.stackOffset));
                Emit(string.Format("push %rax\n"));
            }
            // case mulExpression: functionCall
            else if (functionCall != null)
            {
                functionCall.EmitAsm();
                Emit(string.Format("push %rax\n"));
            }
            else
            {
                if (rhs == null)
                {
                    lhs.EmitAsm();
                    Emit(string.Format("pop %rax"));
                    Emit(string.Format("push %rax\n"));
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
                        Emit(string.Format("push %rax\n"));
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

                    Emit(string.Format("push %rax\n"));
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
}
