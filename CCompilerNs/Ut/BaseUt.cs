using System.Diagnostics;

namespace CCompilerNs
{
    public class BaseUt
    {
        public void Check(bool b)
        {
            if (!b)
                Trace.Assert(false);
        }

        public int CompileAndRun(string asmPath, string exePath)
        {
            Process gcc = new Process();
            gcc.StartInfo.FileName = "gcc.exe";
            gcc.StartInfo.Arguments = "-no-pie -o " + exePath + " " + asmPath;

            gcc.Start();
            gcc.WaitForExit();
            if (gcc.ExitCode != 0)
                throw new Exception("Compile failed");

            Process exe = new Process();
            exe.StartInfo.FileName = exePath;
            exe.Start();
            exe.WaitForExit();
            return exe.ExitCode;
        }

        public Tuple<int, string> CompileAndRun2(string asmPath, string exePath)
        {
            Process gcc = new Process();
            gcc.StartInfo.FileName = "gcc.exe";
            gcc.StartInfo.Arguments = "-no-pie -o " + exePath + " " + asmPath;

            gcc.Start();
            gcc.WaitForExit();
            if (gcc.ExitCode != 0)
                throw new Exception("Compile failed");

            Process exe = new Process();
            exe.StartInfo.FileName = exePath;
            exe.StartInfo.RedirectStandardOutput = true;
            exe.Start();
            exe.WaitForExit();
            string output = exe.StandardOutput.ReadToEnd();


            return Tuple.Create(exe.ExitCode, output);
        }
    }
}
