using RegexNs;

namespace LexYaccNs
{
    public class LexRule
    {
        public LexRule(NFA nfa, string ruleName, string action)
        {
            this.nfa = nfa;
            this.ruleName = ruleName;
            this.action = action;
        }

        public LexRule(string plainText, string ruleName, string action)
        {
            this.plainText = plainText;
            this.ruleName = ruleName;
            this.action = action;
        }

        public string ruleName;
        public NFA nfa;
        public string plainText;
        public string action;

        public string yytext;
    }
}
