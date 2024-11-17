using System.Text;

namespace LexYaccNs
{
    public class LexYaccCodeGen
    {
        public static void GenCode(string lexFile, string yaccFile, string outputFolder, string name)
        {
            string outputFile = Path.Combine(outputFolder, name + ".cs");

            StringBuilder sb = new StringBuilder();

            // LexYacc Gen
            sb.AppendLine("//LexYacc Gen");
            sb.AppendLine("public class " + name);
            sb.AppendLine("{");
            sb.AppendLine("    public static object Parse(string input)");
            sb.AppendLine("    {");
            // pairNs.LexActions.ruleInput, pairNs.YaccActions.ruleInput, 
            sb.AppendLine("        return " + name + "Ns.LexYaccNs.LexYacc.Parse(input, " + name + "Ns.LexActions.ruleInput, " + name + "Ns.YaccActions.ruleInput, " +
                name + "Ns.LexActions.CallAction, " + name + "Ns.YaccActions.CallAction);");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            File.WriteAllText(outputFile, sb.ToString());
            sb.AppendLine("");

            // Yacc Gen
            List<LexTokenDef> lexTokenDef = YaccCodeGen.GenCode(File.ReadAllText(yaccFile), name, outputFolder, true);

            // Lex Gen
            LexCodeGen.GenCode(File.ReadAllText(lexFile), name, lexTokenDef, outputFolder, true);

            // Src files Gen
            File.AppendAllText(outputFile, "//Src files Gen");
            string[] allFiles = Directory.GetFiles("../../../LexYaccNs", "*", SearchOption.AllDirectories);
            foreach (string file in allFiles)
            {
                if (file.EndsWith(".cs"))
                    AppendFile(outputFile, file, name + "Ns");
            }

            allFiles = Directory.GetFiles("../../../RegexNs", "*", SearchOption.AllDirectories);
            foreach (string file in allFiles)
            {
                if (file.EndsWith(".cs"))
                    AppendFile(outputFile, file, name + "Ns");
            }
        }

        private static void AppendFile(string outputFile, string file, string ns)
        {
            File.AppendAllText(outputFile, "\nnamespace " + ns + "{\n\n");
            File.AppendAllText(outputFile, File.ReadAllText(file));
            File.AppendAllText(outputFile, "\n}\n");
        }
    }
}
