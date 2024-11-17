using System.Text;

namespace LexYaccNs
{
    public class LexCodeGen
    {
        public static string classDef = @"
    using LexYaccNs;
    public class LexActions
    {
        public static object value = null;

        public static Dictionary<string, Func<string, object>> actions = new Dictionary<string, Func<string, object>>();
";

        public static string callActionFunctionDef = @"
        public static void CallAction(List<Terminal> tokens, LexRule rule)
        {
            Init();
            object ret = actions[rule.ruleName](rule.yytext);
            if (ret is int && (int)ret == 0)
            {

            }
            else if (ret is char)
            {
                tokens.Add(Terminal.BuildConstCharTerminal((char)ret));
            }
            else
            {
                tokens.Add(Terminal.BuildToken(tokenDict[(int)ret], LexActions.value));
            }
        }
";

        public static string GenCode(string input, string namespaceStr, List<LexTokenDef> lexTokenDef)
        {
            List<LexRule> rules;
            Section section;
            LexRuleReader.Parse(input, out section, out rules);

            StringBuilder sb = new StringBuilder();
            string indent1 = "    ";
            string indent2 = "        ";
            string indent3 = "            ";

            sb.AppendLine("//Lex Gen ");
            sb.AppendLine("namespace " + namespaceStr);
            sb.AppendLine("{");
            sb.AppendLine(section.definitionSection);
            sb.AppendLine(classDef);

            sb.AppendLine(indent2 + "public static Dictionary<int, string> tokenDict = new Dictionary<int, string>");
            sb.AppendLine(indent2 + "{");
            foreach (LexTokenDef lexToken in lexTokenDef)
                sb.AppendLine(indent3 + "{ " + lexToken.index + ", \"" + lexToken.name + "\"},");
            sb.AppendLine(indent2 + "};");
            sb.AppendLine();

            foreach (LexTokenDef lexToken in lexTokenDef)
                sb.AppendLine(indent2 + "public static int " + lexToken.name + " = " + lexToken.index + ";");

            sb.AppendLine(callActionFunctionDef);

            sb.AppendLine(indent2 + "public static string ruleInput = @\"" + input.Replace("\"", "\"\"") + "\";\n");
            sb.AppendLine();
            sb.AppendLine(indent2 + "public static void Init()");
            sb.AppendLine(indent2 + "{");
            sb.AppendLine(indent2 + "    if (actions.Count != 0)");
            sb.AppendLine(indent2 + "        return;");

            for (int i = 0; i < rules.Count; i++)
            {
                LexRule rule = rules[i];
                sb.AppendLine(indent3 + "actions.Add(\"LexRule" + i + "\", LexAction" + i + ");");
            }
            sb.AppendLine(indent2 + "}");


            for (int i = 0; i < rules.Count; i++)
            {
                LexRule rule = rules[i];
                sb.AppendLine(indent2 + "public static object LexAction" + i + "(string yytext)");
                sb.AppendLine(indent2 + "{");
                sb.AppendLine(indent2 + "    value = null;");
                sb.AppendLine();

                if (rule.action.Trim().Length != 0)
                {
                    sb.AppendLine(indent3 + "// user-defined action");
                    sb.AppendLine(LexYaccUtil.FixGenCodeIndention(rule.action, indent3));
                }

                sb.AppendLine(indent2 + "    return 0;");
                sb.AppendLine(indent2 + "}");
            }

            sb.AppendLine(indent1 + "}");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine();

            Console.Write(sb.ToString());

            return sb.ToString();
        }

        public static void GenCode(string ruleSection, string name, List<LexTokenDef> lexTokenDef, string outputFolder, bool append)
        {
            string code = GenCode(ruleSection, name + "Ns", lexTokenDef);
            if (append)
                File.AppendAllText(Path.Combine(outputFolder, name + ".cs"), code);
            else
                File.WriteAllText(Path.Combine(outputFolder, name + ".cs"), code);
        }
    }
}
