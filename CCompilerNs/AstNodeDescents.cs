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

    public enum VariableType
    {
        void_type,
        int_type
    }

    public class LocalVariable
    {
        public string name;
        public int stackAddress = 0;
    }

    public class FunDecl : AstNode
    {
        public VariableType returnType;
        public string functionName;
        public List<Statement> statements;

        public Dictionary<string, LocalVariable> locals = new Dictionary<string, LocalVariable>();
        public int localSize = 0;

        public FunDecl() : base("FunDecl")
        {

        }

        public void AddLocalVariable(DeclareStatement a)
        {
            LocalVariable l = new LocalVariable();
            l.name = a.name;
            locals.Add(l.name, l);


            if (a.type == VariableType.int_type)
            {
                localSize += 8;
            }

            l.stackAddress = localSize;
            a.stackAddress = localSize;
        }

        public override void EmitAsm()
        {
            Context.locals = locals;

            string asm = string.Format(@"
#FunDecl =>
.global {0}
{1}:
push % rbp
mov % rsp, % rbp

add ${2}, %rsp
", functionName, functionName, -localSize);

            Emit(asm);
            foreach (Statement s in statements)
                s.EmitAsm();
            Context.locals = null;

            asm = @"
leave
ret
#<= FunDecl
";
            Emit(asm);
        }
    }

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
            LocalVariable l = Context.locals[name];

            Emit("pop %rax");
            Emit(string.Format("mov %rax, -{0}(%rbp)", l.stackAddress));
            Emit("#<= AssignmentStatement\n");
        }
    }

    public class DeclareStatement : Statement
    {
        public VariableType type;
        public string name;
        public Expression value;
        public int stackAddress;

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

            Emit(string.Format("mov %rax, -{0}(%rbp)", stackAddress));
            Emit("#<= DeclareStatement\n");
        }
    }



    public class Expression : AstNode
    {
        public Expression lhs = null;
        public string? op = null;
        public Expression rhs = null;
        public int? intValue = null;
        public string? variableName = null;
        public string? functionName = null;

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
                LocalVariable local = Context.locals[variableName];
                Emit(string.Format("mov -{0}(%rbp), %rax", local.stackAddress));
                Emit(string.Format("push %rax\n"));
            }
            // case mulExpression: functionCall
            else if (functionName != null)
            {
                Emit(string.Format("call {0}", functionName));
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
                        Emit(string.Format("mov $0, % rdx"));
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
            else if (functionName != null)
            {
                s = "Expression(" + functionName + ")";
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
