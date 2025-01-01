namespace CCompilerNs
{
    public class CC
    {
        public class LocalVariable
        {
            public string name;
            public int position;

            public LocalVariable(string name, int position)
            {
                this.name = name;
                this.position = position;
            }
        }

        private static void MainAsm(List<LocalVariable> locals)
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







            LocalVariable v1 = new LocalVariable("a", -8);
            LocalVariable v2 = new LocalVariable("b", -16);

            List<LocalVariable> locals = new List<LocalVariable>();
            locals.Add(v1);
            locals.Add(v2);

            //MainAsm(locals);


        }
    }
}
