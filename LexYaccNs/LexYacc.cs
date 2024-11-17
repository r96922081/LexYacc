namespace LexYaccNs
{

    public class LexYacc
    {
        public static object Parse(string input, string lexRule, string yaccRule, Lex.CallActionDelegate lexCallActionDelegate, Yacc.CallActionDelegate yaccActionDelegate)
        {
            List<Terminal> tokens = Lex.Parse(input, lexRule, lexCallActionDelegate);

            Yacc yacc = new Yacc(yaccRule);
            for (int i = 0; i < tokens.Count; i++)
                yacc.Feed(tokens[i]);
            yacc.EndFeeding();

            if (yacc.result != FeedResult.Accept)
                return "syntax error";

            return yacc.startDFA.CallAction(yaccActionDelegate);
        }
    }

}