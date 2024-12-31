using System.Diagnostics;

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

        private static void EmitAsm(string asm)
        {
            Console.WriteLine(asm);
        }

        private static void EmitPrologue()
        {
            string prologue = @"
.data

.text

.global main

main:
push %rbp
mov %rsp, %rbp

";
            EmitAsm(prologue);
        }

        private static void EmitEpilogue()
        {
            string epilogue = @"

leave
ret
";
            EmitAsm(epilogue);
        }

        private static void ReserveStack(List<LocalVariable> locals)
        {
            // mov $-24, %rsp # reserve 3 variable
            EmitAsm(string.Format("mov ${0}, %rsp # reserve {1} variable\n", locals.Count * -8, locals.Count));
        }

        private static void MainAsm(List<LocalVariable> locals)
        {
            // 15 * 6 - 7 / 2 = 41

            // 15
            //mov $10, %rax
            EmitAsm(string.Format("mov ${0}, %rax\n", 15));

            // mov $6, %rbx
            // mul %rbx
            EmitAsm(string.Format("mov ${0}, %rbx", 6));
            EmitAsm(string.Format("mul %rbx\n"));

            // mov $7, %rbx
            // sub %rbx, %rax
            EmitAsm(string.Format("mov ${0}, %rbx", 7));
            EmitAsm(string.Format("sub %rbx, %rax\n"));

            // mov $2, %rbx
            // div %rbx
            EmitAsm(string.Format("mov ${0}, %rbx", 2));
            EmitAsm(string.Format("div %rbx\n"));
        }

        private static void CompileAndRun()
        {
            Process p = new Process();
            p.StartInfo.FileName = "gcc.exe";
            p.StartInfo.Arguments = "-no-pie -o D:/test.exe D:/test.s";

            p.Start();
            p.WaitForExit();
            int x = p.ExitCode;

            Process p2 = new Process();
            p2.StartInfo.FileName = "D:\\test.exe";
            p2.Start();
            p2.WaitForExit();
            x = p2.ExitCode;
        }

        public static void Parse()
        {



            string src = @"
int main()
{
	return 0;
}
";
            AstNode ret = (AstNode)cc.Parse(src);
            ret.Print();


            LocalVariable v1 = new LocalVariable("a", -8);
            LocalVariable v2 = new LocalVariable("b", -16);

            List<LocalVariable> locals = new List<LocalVariable>();
            locals.Add(v1);
            locals.Add(v2);

            EmitPrologue();
            ReserveStack(locals);
            MainAsm(locals);
            EmitEpilogue();
        }
    }
}
