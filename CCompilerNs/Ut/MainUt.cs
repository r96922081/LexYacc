namespace CCompilerNs
{
    public class MainUt : BaseUt
    {
        public void Ut1()
        {
            string src = @"
int main()
{
	return 77;
}
";
            AsmEmitter.SetOutputFile("test.s");

            Program program = (Program)cc.Parse(src);
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 77);
        }

        public void Ut2()
        {
            string src = @"
int main()
{
	return 1 + 5 - 2;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 4);
        }

        public void Ut3()
        {
            string src = @"
int main()
{
	return 1 - 7 + 11 - 20 - 44 + 89;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 30);
        }

        public static void RunAllUt()
        {
            MainUt mainUt = new MainUt();

            mainUt.Ut1();
            mainUt.Ut2();
            mainUt.Ut3();
        }

        public static void Ut()
        {
            RunAllUt();
        }
    }
}
