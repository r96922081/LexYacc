using System.Diagnostics;
using System.Text;

namespace CCompilerNs
{
    public class BaseUt
    {
        public void Check(bool b)
        {
            if (!b)
                Trace.Assert(false);
        }

        public int CompileAsmAndRun(string asmPath, string exePath)
        {
            Process gcc = new Process();
            gcc.StartInfo.FileName = "gcc.exe";
            gcc.StartInfo.Arguments = "-no-pie -o " + exePath + " " + asmPath;

            gcc.Start();
            gcc.WaitForExit();
            if (gcc.ExitCode != 0)
                throw new Exception("Compile failed");

            Process exe = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };

            exe.OutputDataReceived += (sender, args) => { };
            exe.ErrorDataReceived += (sender, args) => { };

            exe.Start();
            exe.BeginOutputReadLine();
            exe.BeginErrorReadLine();
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

            Process exe = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            StringBuilder outputBuilder = new StringBuilder();

            exe.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    outputBuilder.AppendLine(args.Data);
                }
            };

            exe.Start();
            exe.BeginOutputReadLine();
            exe.WaitForExit();

            string output = outputBuilder.ToString();

            Console.Write(output);

            return Tuple.Create(exe.ExitCode, output);
        }
    }
}
