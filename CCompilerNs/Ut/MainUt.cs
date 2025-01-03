﻿namespace CCompilerNs
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

        public void Ut7()
        {
            string src = @"
int main()
{
    int a = 1 + 2;
	return a;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 3);
        }

        public void Ut8()
        {
            string src = @"
int main()
{
    int a = 1;
    int b = 2;
    int c = 3;
	return (a + b * c - 1) / c;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 2);
        }

        public void Ut9()
        {
            string src = @"
int main()
{
    int a = 2;
    int b = 3;
    int c = 6;
    int d = 2;
    int e;
    int f;
    e = 5;
	return a - (b - 11) / a + e * c - 7 * 2 + 9;
}
";
            // replace e with 5 then it will pass
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 31);
        }

        public void Ut10()
        {
            string src = @"
int main()
{
    int a = 2;
    int b = 3;
    int c = 6;
    int d = 2;
    int e;
    int f;
    e = a + d + 1;
    int g;
    g = 110;
    g = 11;
    int h = c + 1 + a - d;
	return a - (b - g) / a + e * c - h * a + (b + a * b);
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

        public void Ut11()
        {
            string src = @"
int f1()
{
    return 1;
}

int f2()
{
    return 2;
}

int f3()
{
    return 3;
}

int main()
{
    return f1() + f2() * f3() + 10;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 17);
        }

        public static void RunAllUt()
        {
            MainUt mainUt = new MainUt();

            mainUt.Ut1();
            mainUt.Ut2();
            mainUt.Ut3();
            mainUt.Ut4();
            mainUt.Ut5();
            //mainUt.Ut6();
            mainUt.Ut7();
            mainUt.Ut8();
            mainUt.Ut9();
            mainUt.Ut10();
            mainUt.Ut11();
        }

        public static void Ut()
        {
            RunAllUt();
        }
    }
}
