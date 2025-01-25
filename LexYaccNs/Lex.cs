namespace LexYaccNs
{
    using RegexNs;
    using System.Collections.Generic;

    public class Lex
    {
        public delegate void CallActionDelegate(List<Terminal> tokens, LexRule rule);

        public static List<Terminal> Parse(string input, string ruleInput, CallActionDelegate actionFunction)
        {
            List<Terminal> tokens = new List<Terminal>();

            List<LexRule> rules;
            Section section;
            LexRuleReader.Parse(ruleInput, out section, out rules);

            int start = 0;

            // find the longest match rule first.
            // If there are multiple rules match the same length, selec the first one
            while (start < input.Length)
            {
                LexRule matchedRule = null;

                for (int i = 0; i < rules.Count; i++)
                {
                    LexRule rule = rules[i];
                    rule.yytext = "";

                    if (rule.plainText != null)
                    {
                        if (input.Substring(start).StartsWith(rule.plainText))
                        {
                            rule.yytext = rule.plainText;
                            if (matchedRule == null)
                                matchedRule = rule;
                            else
                            {
                                if (rule.yytext.Length > matchedRule.yytext.Length)
                                    matchedRule = rule;
                            }
                        }
                    }
                    else
                    {
                        int prevAccept = -1;
                        RecognizeParam param = rule.nfa.CreateRecognizeParam();

                        for (int j = 0; start + j < input.Length; j++)
                        {
                            RecognizeResult result = rule.nfa.StepRecognize(input[start + j], param);
                            if (result == RecognizeResult.AliveAndAccept)
                                prevAccept = j;
                            else if (result == RecognizeResult.EndAndReject)
                                break;
                        }

                        if (prevAccept != -1)
                        {
                            rule.yytext = input.Substring(start, prevAccept + 1);

                            if (matchedRule == null)
                                matchedRule = rule;
                            else
                            {
                                if (rule.yytext.Length > matchedRule.yytext.Length)
                                    matchedRule = rule;
                            }
                        }
                    }
                }

                if (matchedRule == null)
                {
                    Console.WriteLine("Error starts at: " + input.Substring(start));
                    throw new Exception("Syntax Error, at pos " + start);
                }
                else
                {
                    actionFunction(tokens, matchedRule);
                    start += matchedRule.yytext.Length;
                }
            }

            return tokens;
        }
    }
}