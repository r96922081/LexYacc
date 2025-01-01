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

.global main

main:
push %rbp
mov %rsp, %rbp

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

    public class FunDecl : AstNode
    {
        public VariableType returnType;
        public string functionName;

        public FunDecl() : base("FunDecl")
        {

        }

        public override void EmitCurrentAsm()
        {
            // mov $-24, %rsp # reserve 3 variable
            //AsmEmitter.Emit(string.Format("mov ${0}, %rsp # reserve {1} variable\n", locals.Count * -8, locals.Count));
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
        public AddExpression returnValue;

        public ReturnStatement() : base("ReturnStatement")
        {

        }

        public override void EmitAsm()
        {
            EmitChildrenAsm();

            string leave = @"
leave
ret
";
            Emit(leave);
        }
    }

    public class AddExpression : AstNode
    {
        public AddExpression lhs = null;
        public string? op = null;
        public AddExpression rhs = null;
        public int? intValue = null;

        public AddExpression() : base("AddExpression")
        {

        }

        public override void EmitAsm()
        {
            if (intValue != null)
            {
                Emit(string.Format("mov ${0}, %rax", intValue));
                Emit(string.Format("push %rax"));
            }
            else
            {
                lhs.EmitAsm();
                rhs.EmitAsm();

                Emit(string.Format("pop %rbx"));
                Emit(string.Format("pop %rax"));

                if (op == "+")
                    Emit(string.Format("add %rbx, %rax\n"));
                else if (op == "-")
                    Emit(string.Format("sub %rbx, %rax\n"));

                Emit(string.Format("push %rax"));
            }
        }

        public override string ToString()
        {
            if (op != null)
            {
                s = "AddExpression(" + op + ")";
            }
            else
            {
                s = "AddExpression(" + intValue + ")";
            }

            return base.ToString();
        }
    }
}
