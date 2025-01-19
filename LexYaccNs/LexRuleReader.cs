using RegexNs;

namespace LexYaccNs
{
    public class LexRuleReader
    {
        public static void Parse(string input, out Section sections, out List<LexRule> rules)
        {
            rules = new List<LexRule>();

            sections = YaccRuleReader.SplitSecction(input, false);
            string ruleSectionString = sections.ruleSection.Trim();

            while (ruleSectionString.Length > 0)
            {
                int leftBracket = LexYaccUtil.FindCharNotInLiteral(ruleSectionString, '{', false);
                string regex = ruleSectionString.Substring(0, leftBracket).Trim();

                // the case } in action:
                // "}"  { return '}'; }
                ruleSectionString = ruleSectionString.Substring(leftBracket + 1);
                int rightBracket = LexYaccUtil.FindCharNotInLiteral(ruleSectionString, '}', true);
                string action = LexYaccUtil.RemoveHeadAndTailEmptyLine(ruleSectionString.Substring(0, rightBracket));

                ruleSectionString = ruleSectionString.Substring(rightBracket + 1).Trim();

                regex = regex.Replace("\\/", "/");

                if (regex.StartsWith("\""))
                    rules.Add(new LexRule(regex.Substring(1, regex.Length - 2), "LexRule" + rules.Count, action));
                else
                    rules.Add(new LexRule(Regex.Compile(regex), "LexRule" + rules.Count, action));
            }
        }
    }
}
