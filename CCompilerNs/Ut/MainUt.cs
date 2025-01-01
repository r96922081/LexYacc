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

        public void Ut4()
        {
            string src = @"
int main()
{
	return 2 * 3 - 8 / 4;
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

        public void Ut5()
        {
            string src = @"
int main()
{
	return 2 - (3 - 11) / 2 + 5 * 6 - 7 * 2 + 9;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 31);
        }

        public void Ut6()
        {
            string src = @"
int main()
{
	return (10 + 20) * (30 − 5);
}
";

            // return 1 * (2-1);
            //return ((10 + 20) * (30−5))/ (15 + 5)−(25−10);
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 22);
        }



        public static void RunAllUt()
        {
            MainUt mainUt = new MainUt();

            mainUt.Ut1();
            mainUt.Ut2();
            mainUt.Ut3();
            mainUt.Ut4();
            mainUt.Ut5();
            mainUt.Ut6();
        }

        public static void Ut()
        {
            RunAllUt();
        }
    }
}
