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
        public VariableType returnType;
        public object returnValue;

        public ReturnStatement() : base("ReturnStatement")
        {

        }

        public override void EmitCurrentAsm()
        {
            if (returnType == VariableType.int_type)
            {
                Emit(string.Format("mov ${0}, % rax", (int)returnValue));
            }


            string leave = @"
leave
ret
";
            Emit(leave);
        }
    }
}
