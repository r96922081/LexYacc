namespace LexYaccNs
{
    public enum FeedResult
    {
        Accept,
        Reject,
        Alive
    }

    public class LexTokenDef
    {
        public string name;
        public string type;
        public int index;
    }

    public class Yacc
    {
        public string input = "";
        public Section sections = null;
        public List<YaccRule> productionRules = null;
        public List<LexTokenDef> lexTokenDef = null;
        public Dictionary<string, string> ruleNonterminalType = null;

        public DFA startDFA = null;
        public Stack<DFA> dfaStack = new Stack<DFA>();
        public List<Symbol> symbols = new List<Symbol>();
        private int symbolIndex = 0;

        public FeedResult result = FeedResult.Alive;

        public delegate object CallActionDelegate(string functionName, Dictionary<int, object> param);

        public Yacc()
        {

        }

        public Yacc(string input)
        {
            this.input = input;
            YaccRuleReader.Parse(input, out sections, out productionRules, out lexTokenDef, out ruleNonterminalType);
            Rebuild();
        }

        public void Rebuild()
        {
            dfaStack.Clear();
            startDFA = new DFA(this, productionRules[0].productions[0], lexTokenDef, ruleNonterminalType);
            dfaStack.Push(startDFA);
            symbols.Clear();
            symbolIndex = 0;
            result = FeedResult.Alive;
        }

        public void ExpandNontermianl(int symbolIndex)
        {
            DFA dfa = dfaStack.Peek();
            while (dfa.states[dfa.currentState].symbol is Nonterminal)
            {
                if (!dfa.subDFAs.ContainsKey(dfa.currentState))
                {
                    Nonterminal nt = (Nonterminal)dfa.states[dfa.currentState].symbol;
                    dfa.subDFAs[dfa.currentState] = new List<DFA>();
                    dfa.symbolIndexDict[dfa.currentState] = symbolIndex;
                    foreach (Production production in GetProductions(nt.name))
                        dfa.subDFAs[dfa.currentState].Add(new DFA(this, production, lexTokenDef, ruleNonterminalType));
                }
                dfa = dfa.subDFAs[dfa.currentState][0];
            }
        }

        public void ExpandAndFeedEmpty(int symbolIndex)
        {
            bool continueFeed = true;

            while (continueFeed)
            {
                if (dfaStack.Count == 0)
                    break;

                ExpandNontermianl(symbolIndex);

                DFA dfa = dfaStack.Peek();
                continueFeed = false;

                while (true)
                {
                    if (dfa.production.IsEmptyProduction())
                    {
                        dfaStack.Peek().Feed(this, symbolIndex, true);
                        continueFeed = true;
                        break;
                    }

                    if (dfa.subDFAs.ContainsKey(dfa.currentState) && dfa.subDFAs[dfa.currentState].Count > 0)
                        dfa = dfa.subDFAs[dfa.currentState][0];
                    else
                        break;
                }
            }
        }

        public void FeedInternal()
        {
            ExpandAndFeedEmpty(symbolIndex);

            while (symbolIndex < symbols.Count)
            {
                int tempSymbolIndex = symbolIndex++;

                if (dfaStack.Count == 0)
                {
                    if (tempSymbolIndex < symbols.Count)
                        result = FeedResult.Reject;
                    return;
                }

                result = FeedResult.Alive;
                dfaStack.Peek().Feed(this, tempSymbolIndex, false);

                ExpandAndFeedEmpty(tempSymbolIndex + 1);
            }
        }

        public void Feed(Symbol s)
        {
            symbols.Add(s);
            FeedInternal();
        }

        public void BackToPrevNonterminal()
        {
            if (dfaStack.Count == 0)
            {
                result = FeedResult.Reject;
                return;
            }

            DFA dfa = dfaStack.Peek();
            while (dfa.currentState != -1)
            {
                if (dfa.states[dfa.currentState].symbol is Nonterminal)
                {
                    symbolIndex = dfa.symbolIndexDict[dfa.currentState];
                    dfa.subDFAs[dfa.currentState].RemoveAt(0);
                    if (dfa.subDFAs[dfa.currentState].Count > 0)
                        return;
                }
                dfa.currentState--;
            }

            dfaStack.Pop();
            BackToPrevNonterminal();
        }

        public void AdvanceToNextState()
        {
            dfaStack.Pop();
            if (dfaStack.Count == 0)
            {
                result = FeedResult.Accept;
                return;
            }

            DFA dfa = dfaStack.Peek();
            dfa.currentState++;
            if (dfa.currentState == dfa.acceptedState)
                AdvanceToNextState();
        }

        public bool EndFeeding()
        {
            while (result == FeedResult.Alive)
            {
                BackToPrevNonterminal();
                FeedInternal();
            }

            return result == FeedResult.Accept;
        }

        public List<Production> GetProductions(string name)
        {
            foreach (YaccRule pr in productionRules)
                if (pr.lhs.name == name)
                    return pr.productions;
            return null;
        }

        public override string ToString()
        {
            string ret = "";

            foreach (YaccRule pr in productionRules)
            {
                foreach (Production p in pr.productions)
                {
                    ret += p.ToString() + ";\n";
                }
            }

            return ret;
        }
    }



}