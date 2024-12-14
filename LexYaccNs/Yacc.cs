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

    public class Route
    {
        public DFA startDFA = null;
        public int symbolIndex = -1;
        public Stack<DFA> dfaStack = new Stack<DFA>();
        public bool result = false;
    }

    public class Yacc
    {
        public string input = "";
        public Section sections = null;
        public List<YaccRule> productionRules = null;
        public List<LexTokenDef> lexTokenDef = null;
        public Dictionary<string, string> ruleNonterminalType = null;
        public List<Symbol> symbols = new List<Symbol>();

        public List<Route> routes = new List<Route>();
        public Route route = null;

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
            Route route = new Route();
            route.startDFA = new DFA(this, productionRules[0].productions[0], lexTokenDef, ruleNonterminalType);
            route.dfaStack.Push(route.startDFA);
            route.symbolIndex = 0;
            route.result = false;

            this.route = route;
            routes.Clear();
            routes.Add(route);
        }

        public void ExpandNontermianl(int symbolIndex)
        {
            DFA dfa = route.dfaStack.Peek();
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
                if (route.dfaStack.Count == 0)
                    break;

                ExpandNontermianl(symbolIndex);

                DFA dfa = route.dfaStack.Peek();
                continueFeed = false;

                while (true)
                {
                    if (dfa.production.IsEmptyProduction())
                    {
                        route.dfaStack.Peek().Feed(this, symbolIndex, true);
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

            while (route.dfaStack.Count != 0)
            {
                ExpandAndFeedEmpty(route.symbolIndex);

                while (route.symbolIndex < symbols.Count)
                {
                    if (route.dfaStack.Count == 0)
                        return false;

                    int tempSymbolIndex = route.symbolIndex++;

                    route.dfaStack.Peek().Feed(this, tempSymbolIndex, false);

                    ExpandAndFeedEmpty(tempSymbolIndex + 1);
                }

                if (route.dfaStack.Count == 0)
                    return route.result;
                else
                    BackToPrevNonterminal();
            }

            return route.result;
        }

        public void BackToPrevNonterminal()
        {
            if (route.dfaStack.Count == 0)
            {
                route.result = false;
                return;
            }

            DFA dfa = route.dfaStack.Peek();
            while (dfa.currentState != -1)
            {
                if (dfa.states[dfa.currentState].symbol is Nonterminal)
                {
                    route.symbolIndex = dfa.symbolIndexDict[dfa.currentState];
                    dfa.subDFAs[dfa.currentState].RemoveAt(0);
                    if (dfa.subDFAs[dfa.currentState].Count > 0)
                        return;
                }
                dfa.currentState--;
            }

            route.dfaStack.Pop();
            BackToPrevNonterminal();
        }

        public void AdvanceToNextState()
        {
            route.dfaStack.Pop();
            if (route.dfaStack.Count == 0)
            {
                route.result = true;
                return;
            }

            DFA dfa = route.dfaStack.Peek();
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