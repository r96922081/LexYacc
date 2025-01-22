using RegexNs;

namespace LexYaccNs
{
    public class LexRuleReader
    {
        public static Section SplitSecction(string input, bool singleQuoteAsString)
        {
            Section s = new Section();

            int definitionStart = input.IndexOf("%{");
            if (definitionStart != -1)
            {
                int definitionEnd = input.IndexOf("%}");
                s.definitionSection = input.Substring(definitionStart + 2, definitionEnd - definitionStart - 2).Trim();
                input = input.Substring(definitionEnd + 2);

                int ruleStart = input.IndexOf("%%");
                s.typeSection = input.Substring(0, ruleStart).Trim();
                input = input.Substring(ruleStart + 2);

                int ruleEnd = input.IndexOf("%%");
                s.ruleSection = input.Substring(0, ruleEnd).Trim();
            }
            else
            {
                // special format for ut that has only rule section
                s.ruleSection = input.Trim();
            }

            return s;
        }

        public static void Parse(string input, out Section sections, out List<LexRule> rules)
        {
            rules = new List<LexRule>();

            sections = SplitSecction(input, false);
            string ruleSectionString = sections.ruleSection.Trim();

            while (ruleSectionString.Length > 0)
            {
                int leftBracket = ruleSectionString.IndexOf(" {");
                if (leftBracket == -1)
                    leftBracket = ruleSectionString.IndexOf("\t{");

                string regex = ruleSectionString.Substring(0, leftBracket + 1).Trim();

                // the case } in action:
                // "}"  { return '}'; }
                ruleSectionString = ruleSectionString.Substring(leftBracket + 2);
                int rightBracket = LexYaccUtil.FindCharNotInLiteral(ruleSectionString, '}', true);
                string action = LexYaccUtil.RemoveHeadAndTailEmptyLine(ruleSectionString.Substring(0, rightBracket));

                ruleSectionString = ruleSectionString.Substring(rightBracket + 1).Trim();

                regex = regex.Replace("\\/", "/");

                if (regex.StartsWith("\""))
                    rules.Add(new LexRule(regex.Substring(1, regex.Length - 2), "LexRule" + rules.Count, action));
                else
                {
                    regex = regex.Replace("\\\"", "\"");
                    rules.Add(new LexRule(Regex.Compile(regex), "LexRule" + rules.Count, action));
                }
            }
        }
    }
}
