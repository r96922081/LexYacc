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

        public bool result = false;

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

        public bool IsAccept()
        {
            List<DFA> dfas = new List<DFA>(dfaStack);

            // skip first dfa
            for (int i = 1; i < dfas.Count; i++)
            {
                DFA dfa = dfas[i];
                if (dfa.currentState + 1 != dfa.acceptedState)
                    return false;
            }

            return true;
        }

        public void Rebuild()
        {
            dfaStack.Clear();
            startDFA = new DFA(this, productionRules[0].productions[0], lexTokenDef, ruleNonterminalType);
            dfaStack.Push(startDFA);
            symbolIndex = 0;
            result = false;
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

        public bool Feed(List<Symbol> s)
        {
            symbols = s;

            while (dfaStack.Count != 0)
            {
                ExpandAndFeedEmpty(symbolIndex);

                while (symbolIndex < symbols.Count)
                {
                    if (dfaStack.Count == 0)
                        return false;

                    int tempSymbolIndex = symbolIndex++;

                    dfaStack.Peek().Feed(this, tempSymbolIndex, false);

                    ExpandAndFeedEmpty(tempSymbolIndex + 1);
                }

                if (dfaStack.Count == 0)
                    return result;
                else
                    BackToPrevNonterminal();
            }

            return result;
        }

        public void BackToPrevNonterminal()
        {
            if (dfaStack.Count == 0)
            {
                result = false;
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
                result = true;
                return;
            }

            DFA dfa = dfaStack.Peek();
            dfa.currentState++;
            if (dfa.currentState == dfa.acceptedState)
                AdvanceToNextState();
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