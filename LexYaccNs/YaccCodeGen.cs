using System.Text;

namespace LexYaccNs
{
    public class YaccCodeGen
    {
        public static List<LexTokenDef> GenCode(string input, string name, string outputFolder, bool append)
        {
            Section sections;
            List<YaccRule> productionRules;
            List<LexTokenDef> lexTokenDef;
            Dictionary<string, string> ruleNonterminalType;

            YaccRuleReader.Parse(input, out sections, out productionRules, out lexTokenDef, out ruleNonterminalType);

            string code = GenCode(input, sections, productionRules, lexTokenDef, ruleNonterminalType, name + "Ns");
            if (append)
                File.AppendAllText(Path.Combine(outputFolder, name + ".cs"), code);
            else
                File.WriteAllText(Path.Combine(outputFolder, name + ".cs"), code);

            return lexTokenDef;
        }

        public static string GenCode(string ruleInput, Section sections, List<YaccRule> productionRules, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType, string namespaceStr)
        {
            string indent1 = "    ";
            string indent2 = "        ";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("//Yacc Gen ");
            sb.AppendLine("namespace " + namespaceStr);
            sb.AppendLine("{");
            sb.AppendLine();

            sb.AppendLine(sections.definitionSection);
            sb.AppendLine();

            sb.AppendLine("public class YaccActions{");
            sb.AppendLine();
            sb.AppendLine(indent1 + "public static Dictionary<string, Func<Dictionary<int, object>, object>> actions = new Dictionary<string, Func<Dictionary<int, object>, object>>();");
            sb.AppendLine();
            sb.AppendLine(indent1 + "public static string ruleInput = @\"" + ruleInput.Replace("\"", "\"\"") + "\";\n");
            sb.AppendLine();
            sb.AppendLine(indent1 + "public static object CallAction(string functionName, Dictionary<int, object> param)");
            sb.AppendLine(indent1 + "{");
            sb.AppendLine(indent1 + "    Init();");
            sb.AppendLine(indent1 + "    if (!actions.ContainsKey(functionName))");
            sb.AppendLine(indent1 + "        return null;");
            sb.AppendLine(indent1 + "    return actions[functionName](param);");
            sb.AppendLine(indent1 + "}");
            sb.AppendLine();
            sb.AppendLine(indent1 + "public static void Init()");
            sb.AppendLine(indent1 + "{");
            sb.AppendLine(indent2 + "if (actions.Count != 0)");
            sb.AppendLine(indent2 + "    return;");
            sb.AppendLine();

            foreach (YaccRule rule in productionRules)
            {
                foreach (Production p in rule.productions)
                {
                    if (p.action == null)
                        continue;
                    string functionName = string.Format("Rule_{0}_Producton_{1}", p.lhs.name, p.index);
                    sb.AppendLine(indent2 + string.Format("actions.Add(\"{0}\", {1});", functionName, functionName));
                }
            }
            sb.AppendLine(indent1 + "}");

            foreach (YaccRule rule in productionRules)
            {
                foreach (Production p in rule.productions)
                {
                    if (p.action != null)
                    {
                        sb.AppendLine();
                        sb.AppendLine(indent1 + string.Format("public static object Rule_{0}_Producton_{1}(Dictionary<int, object> objects) {{ ", p.lhs.name, p.index));

                        string type = ruleNonterminalType[rule.lhs.name];

                        sb.Append(indent2 + string.Format("{0} {1}", type, "_0 = new " + type));
                        if (type == "string" || type == "String")
                            sb.AppendLine("(\"\");");
                        else
                            sb.AppendLine("();");

                        if (p.IsEmptyProduction())
                        {

                        }
                        else
                        {
                            if (p.type == ProductionType.LeftRecursiveSecond)
                                sb.AppendLine(indent2 + string.Format("{0} {1} =({2})objects[1]", type, "_1", type) + ";");

                            for (int i = 0; i < p.symbols.Count - (p.type == ProductionType.Plain ? 0 : 1); i++)
                            {
                                Symbol symbol = p.symbols[i];
                                string typeName = "";


                                if (symbol is Terminal)
                                {
                                    Terminal t = (Terminal)symbol;
                                    if (t.type == TerminalType.CONSTANT_CHAR || t.type == TerminalType.EMPTY)
                                        continue;

                                    typeName = "";
                                    foreach (LexTokenDef l in lexTokenDef)
                                    {
                                        if (l.name == t.tokenName)
                                            typeName = l.type;
                                    }
                                }
                                else
                                {
                                    Nonterminal nt = (Nonterminal)symbol;
                                    typeName = ruleNonterminalType[nt.name];
                                }

                                int shift = p.type == ProductionType.LeftRecursiveSecond ? 1 : 0;
                                sb.AppendLine(indent2 + string.Format("{0} {1} = ({2})objects[{3}]", typeName, "_" + (i + 1 + shift).ToString(), typeName, i + 1 + shift) + ";");

                            }
                        }

                        sb.AppendLine();

                        if (p.action.Trim().Length != 0)
                        {
                            sb.AppendLine(indent2 + "// user-defined action");
                            sb.AppendLine(LexYaccUtil.FixGenCodeIndention(p.action, indent2));
                        }

                        sb.AppendLine(indent2 + "return _0;");
                        sb.AppendLine(indent1 + "}");
                    }
                }
            }

            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine();

            return sb.ToString();
        }
    }
}
