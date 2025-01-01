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

        public static void RunAllUt()
        {
            MainUt mainUt = new MainUt();

            mainUt.Ut1();
        }

        public static void Ut()
        {
            RunAllUt();
        }
    }
}
