using System.Text;

namespace CCompilerNs
{
    public class Compiler
    {
        public static void GenerateAsm(string src, string outputFile)
        {
            AsmGenerator.SetOutputFile(outputFile);
            src = RemoveComment(src);

            object ret = cc.Parse(src);
            Program program = (Program)ret;

            program.EmitAsm();
        }

        private static string RemoveComment(string sourceCode)
        {
            StringBuilder result = new StringBuilder();
            int length = sourceCode.Length;
            bool inBlockComment = false;
            bool inLineComment = false;
            bool inString = false;

            for (int i = 0; i < length; i++)
            {
                if (!inBlockComment && !inLineComment)
                {
                    if (!inString && sourceCode[i] == '"')
                    {
                        inString = true;
                    }
                    else if (inString && sourceCode[i] == '"' && (i == 0 || sourceCode[i - 1] != '\\'))
                    {
                        inString = false;
                    }
                }

                if (!inString && !inLineComment && i + 1 < length && sourceCode[i] == '/' && sourceCode[i + 1] == '*')
                {
                    inBlockComment = true;
                    i++;
                    continue;
                }

                if (inBlockComment && i + 1 < length && sourceCode[i] == '*' && sourceCode[i + 1] == '/')
                {
                    inBlockComment = false;
                    i++;
                    continue;
                }

                if (!inString && !inBlockComment && i + 1 < length && sourceCode[i] == '/' && sourceCode[i + 1] == '/')
                {
                    inLineComment = true;
                    i++;
                    continue;
                }

                if (inLineComment && sourceCode[i] == '\n')
                {
                    inLineComment = false;
                    result.Append(sourceCode[i]);
                    continue;
                }

                if (!inBlockComment && !inLineComment)
                {
                    result.Append(sourceCode[i]);
                }
            }

            return result.ToString();
        }
    }
}
