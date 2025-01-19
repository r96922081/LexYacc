namespace CCompilerNs
{
    public class CC
    {
        public class Variable
        {
            public string name;
            public int position;

            public Variable(string name, int position)
            {
                this.name = name;
                this.position = position;
            }
        }

        private static void MainAsm(List<Variable> locals)
        {
            // 15 * 6 - 7 / 2 = 41

            // 15
            //mov $10, %rax
            AsmEmitter.Emit(string.Format("mov ${0}, %rax\n", 15));

            // mov $6, %rbx
            // mul %rbx
            AsmEmitter.Emit(string.Format("mov ${0}, %rbx", 6));
            AsmEmitter.Emit(string.Format("mul %rbx\n"));

            // mov $7, %rbx
            // sub %rbx, %rax
            AsmEmitter.Emit(string.Format("mov ${0}, %rbx", 7));
            AsmEmitter.Emit(string.Format("sub %rbx, %rax\n"));

            // mov $2, %rbx
            // div %rbx
            AsmEmitter.Emit(string.Format("mov ${0}, %rbx", 2));
            AsmEmitter.Emit(string.Format("div %rbx\n"));
        }

        public static void Parse()
        {







            Variable v1 = new Variable("a", -8);
            Variable v2 = new Variable("b", -16);

            List<Variable> locals = new List<Variable>();
            locals.Add(v1);
            locals.Add(v2);

            //MainAsm(locals);


        }
    }
}
