//Lex Gen 
namespace UtLex1Ns
{


    using LexYaccNs;
    public class LexActions
    {
        public static object value = null;

        public static Dictionary<string, Func<string, object>> actions = new Dictionary<string, Func<string, object>>();

        public static Dictionary<int, string> tokenDict = new Dictionary<int, string>
        {
            { 256, "NUMBER"},
        };

        public static int NUMBER = 256;

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

        public static string ruleInput = @"
%{
%}

%%
(\-)?[0-9]+ { 
        value = int.Parse(yytext);
        return NUMBER;
}
""+""  {return '+';}
""-""  {return '-';}
""*""  {return '*';}
""/""  {return '/';}
[ 	
]+   {}
%%";


        public static void Init()
        {
            if (actions.Count != 0)
                return;
            actions.Add("LexRule0", LexAction0);
            actions.Add("LexRule1", LexAction1);
            actions.Add("LexRule2", LexAction2);
            actions.Add("LexRule3", LexAction3);
            actions.Add("LexRule4", LexAction4);
            actions.Add("LexRule5", LexAction5);
        }
        public static object LexAction0(string yytext)
        {
            value = null;

            // user-defined action
            value = int.Parse(yytext);
            return NUMBER;

            return 0;
        }
        public static object LexAction1(string yytext)
        {
            value = null;

            // user-defined action
            return '+';

            return 0;
        }
        public static object LexAction2(string yytext)
        {
            value = null;

            // user-defined action
            return '-';

            return 0;
        }
        public static object LexAction3(string yytext)
        {
            value = null;

            // user-defined action
            return '*';

            return 0;
        }
        public static object LexAction4(string yytext)
        {
            value = null;

            // user-defined action
            return '/';

            return 0;
        }
        public static object LexAction5(string yytext)
        {
            value = null;

            return 0;
        }
    }
}


