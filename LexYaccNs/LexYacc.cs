namespace LexYaccNs
{

    public class LexYacc
    {
        public static object Parse(string input, string lexRule, string yaccRule, Lex.CallActionDelegate lexCallActionDelegate, Yacc.CallActionDelegate yaccActionDelegate)
        {
            List<Terminal> tokens = Lex.Parse(input, lexRule, lexCallActionDelegate);
            List<Symbol> symbols = new List<Symbol>();
            symbols.AddRange(tokens);

            Yacc yacc = new Yacc(yaccRule);
            bool result = yacc.Feed(symbols);

            if (!result)
                return "syntax error";

            return yacc.route.startDFA.CallAction(yaccActionDelegate);
        }
    }

}