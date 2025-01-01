namespace CCompilerNs
{
    public class AsmEmitter
    {
        public static bool outputToConsole = true;
        public static string outputFilePath = null;

        public static void Emit(string asm)
        {
            Console.WriteLine(asm);
            File.AppendAllText(outputFilePath, asm + "\n");
        }

        public static void SetOutputFile(string filePath)
        {
            outputFilePath = filePath;
            File.WriteAllText(outputFilePath, "");
        }

    }
}
