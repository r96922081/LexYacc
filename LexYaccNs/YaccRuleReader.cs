using System.Text;

namespace LexYaccNs
{
    public class Section
    {
        public string definitionSection;
        public string typeSection;
        public string ruleSection;
    }

    public class TypeSectionParser
    {
        // %token <intVal> CONSTANT
        // %type <astVal> program declList decl funDecl typeSpec returnStmt funName param paramList id constant
        public static Tuple<List<LexTokenDef>, Dictionary<string, string>> Parse(string input)
        {
            List<LexTokenDef> lexTokenDef = new List<LexTokenDef>();
            int tokenIndex = 256;
            Dictionary<string, string> ruleNonterminalType = new Dictionary<string, string>();



            if (input != null)
            {
                using (StringReader reader = new StringReader(input))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line.Length == 0)
                            continue;

                        bool isToken = false;
                        if (line.StartsWith("%token"))
                        {
                            isToken = true;
                        }
                        else if (line.StartsWith("%type"))
                        {
                            isToken = false;
                        }
                        else
                        {
                            throw new Exception("Syntax error");
                        }


                        int start = line.IndexOf('<');
                        int end = line.IndexOf('>');

                        // the case <List<List<string>>>
                        for (; line[end + 1] == '>'; end++)
                            ;

                        string type = line.Substring(start + 1, end - start - 1);
                        line = line.Substring(end + 1);

                        string[] symbols = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string symbol in symbols)
                        {
                            if (isToken)
                            {
                                LexTokenDef lexTokenDef2 = new LexTokenDef();
                                lexTokenDef2.type = type;
                                lexTokenDef2.name = symbol;
                                lexTokenDef2.index = tokenIndex++;
                                lexTokenDef.Add(lexTokenDef2);
                            }
                            else
                                ruleNonterminalType.Add(symbol, type);
                        }
                    }
                }
            }

            return new Tuple<List<LexTokenDef>, Dictionary<string, string>>(lexTokenDef, ruleNonterminalType);
        }
    }

    public class RuleSectionParser
    {
        public static List<YaccRule> Parse(string input)
        {
            return Parse(input, new List<LexTokenDef>(), new Dictionary<string, string>());
        }

        public static List<YaccRule> Parse(string input, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            List<YaccRule> ret = new List<YaccRule>();

            YaccRule rule = ReadRule(ref input, lexTokenDef, ruleNonterminalType);
            while (rule != null)
            {
                ret.Add(rule);
                rule = ReadRule(ref input, lexTokenDef, ruleNonterminalType);
            }

            //ret = ConvertIndirectLeftRecursion(ret, lexTokenDef, ruleNonterminalType);

            ret = ConvertLeftRecursion(ret, lexTokenDef, ruleNonterminalType);

            foreach (YaccRule r in ret)
            {
                for (int i = 0; i < r.productions.Count; i++)
                {
                    Production p = r.productions[i];
                    p.index = i;
                    p.action = ConvertActionVariable(p.action);
                }
            }

            return ret;
        }

        private static string ConvertActionVariable(string action)
        {
            if (action == null)
                return null;

            action = action.Replace("$$", "_0");

            StringBuilder sb = new StringBuilder();

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < action.Length; i++)
            {
                if (action[i] == '$' && i + 1 < action.Length && char.IsDigit(action[i + 1]))
                {
                    // Replace '$' with '_'
                    sb.Append('_');
                }
                else
                {
                    sb.Append(action[i]);
                }
            }
            return sb.ToString();
        }

        public static Production ReadProduction2(string productionString, Nonterminal lhs, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            List<Symbol> symbols = new List<Symbol>();
            string[] rhsTokens = productionString.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            // empty rule
            if (rhsTokens.Length == 0)
            {
                symbols.Add(Terminal.BuildEmptyTerminal());
                return new Production(lhs, symbols, lexTokenDef, ruleNonterminalType, null, ProductionType.Plain);
            }
            else
            {
                for (int i = 0; i < rhsTokens.Length; i++)
                {
                    string token = rhsTokens[i].Trim();
                    bool isToken = false;
                    foreach (LexTokenDef l in lexTokenDef)
                    {
                        if (l.name == token)
                            isToken = true;
                    }

                    if (isToken)
                    {
                        symbols.Add(Terminal.BuildToken(token));
                    }
                    else if (token.StartsWith("'") && token.EndsWith("'"))
                    {
                        symbols.Add(Terminal.BuildConstCharTerminal(token[1]));
                    }
                    else
                    {
                        symbols.Add(new Nonterminal(token));
                    }
                }

                return new Production(lhs, symbols, lexTokenDef, ruleNonterminalType, null, ProductionType.Plain);
            }
        }

        private static Production ReadProduction(ref string input, Nonterminal lhs, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            input = input.Trim();
            if (input.Length == 0)
                return null;

            if (input[0] == '|')
            {
                input = input.Substring(1).Trim();
            }
            else if (input[0] == ';')
            {
                input = input.Substring(1).Trim();
                return null;
            }

            string productionString = null;
            string action = null;

            int keyPos = LexYaccUtil.FindCharNotInLiteral(input, new List<char>() { '{', '|', ';' }, true);

            if (keyPos == -1)
            {
                // the very last production in the end of input
                productionString = input;
                input = "";
            }
            else if (input[keyPos] == '{')
            {
                productionString = input.Substring(0, keyPos).Trim();
                int rightBrace = LexYaccUtil.FindCharNotInLiteral(input, '}', true);
                if (rightBrace == -1)
                    throw new Exception("Syntax error");
                action = input.Substring(keyPos + 1, rightBrace - keyPos - 1);
                action = LexYaccUtil.RemoveHeadAndTailEmptyLine(action);

                input = input.Substring(rightBrace + 1).Trim();
            }
            else if (input[keyPos] == '|' || input[keyPos] == ';')
            {
                productionString = input.Substring(0, keyPos);
                input = input.Substring(keyPos).Trim();
            }


            Production p = ReadProduction2(productionString, lhs, lexTokenDef, ruleNonterminalType);
            p.action = action;

            return p;
        }

        private static YaccRule ReadRule(ref string input, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            input = input.Trim();
            if (input.Length == 0)
                return null;

            int comma = input.IndexOf(':');
            if (comma == -1)
                throw new Exception("Syntax error");

            string lhs = input.Substring(0, comma).Trim();
            input = input.Substring(comma + 1);
            YaccRule rule = new YaccRule();
            rule.lhs = new Nonterminal(lhs);

            // special handling for empty production in the front case:   a: | 'A' or a : {} | 'A'
            input = input.Trim();
            if (input[0] == '|')
            {
                rule.productions.Add(new Production(rule.lhs, new List<Symbol> { Terminal.BuildEmptyTerminal() }, lexTokenDef, ruleNonterminalType, null, ProductionType.Plain));
            }
            else if (input[0] == '{')
            {
                int rightCurlyPos = LexYaccUtil.FindCharNotInLiteral(input, '}', true);
                string action = input.Substring(1, rightCurlyPos - 1).Trim();
                input = input.Substring(rightCurlyPos + 1);
                rule.productions.Add(new Production(rule.lhs, new List<Symbol> { Terminal.BuildEmptyTerminal() }, lexTokenDef, ruleNonterminalType, action, ProductionType.Plain));

            }

            Production production = ReadProduction(ref input, rule.lhs, lexTokenDef, ruleNonterminalType);
            while (production != null)
            {
                rule.productions.Add(production);
                production = ReadProduction(ref input, rule.lhs, lexTokenDef, ruleNonterminalType);
            }

            return rule;
        }

        private static List<YaccRule> GetIndirectLeftRecursionDfs(YaccRule rule, List<YaccRule> rules, List<YaccRule> traversed)
        {
            traversed.Add(rule);
            foreach (Production p in rule.productions)
            {
                if (p.IsEmptyProduction())
                    continue;

                if (p.symbols[0] is Terminal)
                    continue;

                Nonterminal nt = (Nonterminal)p.symbols[0];
                if (nt.name == rule.lhs.name)
                    continue;


            }

            return null;
        }

        private static List<YaccRule> GetIndirectLeftRecursion(List<YaccRule> rulesParam)
        {
            List<YaccRule> rules = new List<YaccRule>(rulesParam);

            while (rules.Count > 0)
            {
                List<YaccRule> traversed = new List<YaccRule>();
                List<YaccRule> indirect = GetIndirectLeftRecursionDfs(rules[0], rules, traversed);
                if (indirect != null)
                    return indirect;

                foreach (YaccRule r in traversed)
                    rules.Remove(r);
            }

            return null;
        }

        public static List<YaccRule> ConvertIndirectLeftRecursion(List<YaccRule> rules, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            List<YaccRule> ret = new List<YaccRule>(rules);

            List<YaccRule> indirectLeftRecursionRule = GetIndirectLeftRecursion(rules);

            while (indirectLeftRecursionRule != null)
            {
                indirectLeftRecursionRule = GetIndirectLeftRecursion(rules);
            }

            return ret;
        }

        public static List<YaccRule> ConvertLeftRecursion(List<YaccRule> rules, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            List<YaccRule> ret = new List<YaccRule>();

            foreach (YaccRule pr in rules)
            {
                bool leftRecursion = false;
                foreach (Production p in pr.productions)
                {
                    if (p.symbols[0] is Nonterminal && ((Nonterminal)p.symbols[0]).name == pr.lhs.name)
                    {
                        leftRecursion = true;
                        break;
                    }
                }

                if (!leftRecursion)
                {
                    ret.Add(pr);
                }
                else
                {
                    YaccRule newPr1 = new YaccRule();
                    newPr1.lhs = new Nonterminal(pr.lhs.name);
                    ret.Add(newPr1);

                    YaccRule newPr2 = new YaccRule();
                    string expandedName = pr.lhs.name + "_LeftRecursionExpand";
                    newPr2.lhs = new Nonterminal(expandedName);
                    ret.Add(newPr2);

                    if (ruleNonterminalType.ContainsKey(pr.lhs.name))
                        ruleNonterminalType.Add(expandedName, ruleNonterminalType[pr.lhs.name]);

                    /*
                        a: a 'A' | a 'B' | 'C' | 'D' =>

                        a: 'C' a2 | 'D' a2
                        a2: 'A' a2 | 'B' a2 | empty
                    */
                    foreach (Production p in pr.productions)
                    {
                        // is left recursive
                        if (p.symbols.Count > 0 && p.symbols[0] is Nonterminal && ((Nonterminal)(p.symbols[0])).name == pr.lhs.name)
                        {
                            Production p2 = p.Clone();
                            p2.type = ProductionType.LeftRecursiveSecond;
                            p2.symbols.RemoveAt(0);
                            p2.symbols.Add(new Nonterminal(expandedName));
                            p2.lhs = newPr2.lhs;
                            newPr2.productions.Add(p2);
                        }
                        else
                        {
                            Production p2 = p.Clone();
                            p2.type = ProductionType.LeftRecursiveFirst;
                            p2.symbols.Add(new Nonterminal(expandedName));
                            newPr1.productions.Add(p2);
                        }
                    }

                    // empty production
                    newPr2.productions.Add(new Production(newPr2.lhs, new List<Symbol>() { Terminal.BuildEmptyTerminal() }, lexTokenDef, ruleNonterminalType, "", ProductionType.LeftRecursiveSecond));
                }
            }

            return ret;
        }
    }

    public class YaccRuleReader
    {
        public static void Parse(string input, out Section sections, out List<YaccRule> productionRules, out List<LexTokenDef> lexTokenDef, out Dictionary<string, string> ruleNonterminalType)
        {
            sections = SplitSecction(input, true);
            productionRules = new List<YaccRule>();

            Tuple<List<LexTokenDef>, Dictionary<string, string>> types = TypeSectionParser.Parse(sections.typeSection);
            lexTokenDef = types.Item1;
            ruleNonterminalType = types.Item2;

            productionRules = RuleSectionParser.Parse(sections.ruleSection, lexTokenDef, ruleNonterminalType);
            InsertStartRule(productionRules, lexTokenDef, ruleNonterminalType);
        }

        private static void InsertStartRule(List<YaccRule> productionRules, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            string userStartNonterminal = productionRules[0].lhs.name;
            Nonterminal start = new Nonterminal("start");
            Production startProduction = RuleSectionParser.ReadProduction2(userStartNonterminal, start, lexTokenDef, ruleNonterminalType);
            startProduction.action = "_0 = _1;";
            if (ruleNonterminalType.ContainsKey(userStartNonterminal))
                ruleNonterminalType["start"] = ruleNonterminalType[userStartNonterminal];
            startProduction.index = 0;
            YaccRule pr = new YaccRule(start, new List<Production> { startProduction });
            productionRules.Insert(0, pr);
        }

        public static Section SplitSecction(string input, bool singleQuoteAsString)
        {
            Section s = new Section();

            int definitionStart = LexYaccUtil.FindStringNotInLiteral(input, "%{", singleQuoteAsString);
            if (definitionStart != -1)
            {
                int definitionEnd = LexYaccUtil.FindStringNotInLiteral(input, "%}", singleQuoteAsString);
                s.definitionSection = input.Substring(definitionStart + 2, definitionEnd - definitionStart - 2).Trim();
                input = input.Substring(definitionEnd + 2);

                int ruleStart = LexYaccUtil.FindStringNotInLiteral(input, "%%", singleQuoteAsString);
                s.typeSection = input.Substring(0, ruleStart).Trim();
                input = input.Substring(ruleStart + 2);

                int ruleEnd = LexYaccUtil.FindStringNotInLiteral(input, "%%", singleQuoteAsString);
                s.ruleSection = input.Substring(0, ruleEnd).Trim();
            }
            else
            {
                // special format for ut that has only rule section
                s.ruleSection = input.Trim();
            }

            return s;
        }
    }
}
