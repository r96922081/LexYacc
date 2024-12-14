namespace LexYaccNs
{
    public class LexTokenDef
    {
        public string name;
        public string type;
        public int index;
    }

    public enum Result
    {
        Alive,
        Accepted,
        Rejected,
    }

    public class Route
    {
        public DFA startDFA = null;
        public int symbolIndex = -1;
        public Stack<DFA> dfaStack = new Stack<DFA>();
        public Result result = Result.Alive;
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
            route.result = Result.Alive;

            this.route = route;
            routes.Clear();
            routes.Add(route);
        }

        public Route CloneRoute(int symbolIndex)
        {
            Dictionary<DFA, DFA> oldDFAtoNewDFAMapping = new Dictionary<DFA, DFA>();

            DFA newStartDFA = route.startDFA.clone(oldDFAtoNewDFAMapping);
            Route newRoute = new Route();
            newRoute.startDFA = newStartDFA;
            newRoute.symbolIndex = symbolIndex;
            newRoute.result = Result.Alive;

            // restore stack
            Stack<DFA> reverse = new Stack<DFA>(route.dfaStack);
            while (reverse.Count > 0)
            {
                DFA oldDFA = reverse.Pop();
                newRoute.dfaStack.Push(oldDFAtoNewDFAMapping[oldDFA]);
            }

            return newRoute;
        }

        public void ExpandNontermianl(int symbolIndex)
        {
            DFA dfa = route.dfaStack.Peek();
            while (dfa.states[dfa.currentState].symbol is Nonterminal)
            {
                if (!dfa.subDFAs.ContainsKey(dfa.currentState))
                {
                    Nonterminal nt = (Nonterminal)dfa.states[dfa.currentState].symbol;
                    List<Production> productions = GetProductions(nt.name);

                    // create new route
                    for (int i = 1; i < productions.Count; i++)
                    {
                        Route newRoute = CloneRoute(symbolIndex);

                        DFA newDFA = new DFA(this, productions[i], lexTokenDef, ruleNonterminalType);
                        newRoute.dfaStack.Peek().subDFAs[dfa.currentState] = newDFA;
                        newRoute.dfaStack.Push(newDFA);

                        routes.Add(newRoute);
                    }

                    DFA newDFA2 = new DFA(this, productions[0], lexTokenDef, ruleNonterminalType);
                    dfa.subDFAs[dfa.currentState] = newDFA2;
                    route.dfaStack.Push(newDFA2);
                }
                dfa = dfa.subDFAs[dfa.currentState];
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

                    if (dfa.subDFAs.ContainsKey(dfa.currentState))
                        dfa = dfa.subDFAs[dfa.currentState];
                    else
                        break;
                }
            }
        }

        public bool FeedInternal()
        {
            ExpandAndFeedEmpty(route.symbolIndex);

            for (; route.symbolIndex < symbols.Count; route.symbolIndex++)
            {
                if (route.dfaStack.Count == 0)
                    return false;

                route.dfaStack.Peek().Feed(this, route.symbolIndex, false);
                if (route.result == Result.Rejected)
                    return false;

                ExpandAndFeedEmpty(route.symbolIndex + 1);
            }

            if (route.result == Result.Accepted)
                return true;
            else
                return false;
        }

        public bool Feed(List<Symbol> s)
        {
            symbols = s;

            while (routes.Count > 0)
            {
                route = routes[routes.Count - 1];
                routes.RemoveAt(routes.Count - 1);
                bool result = FeedInternal();
                if (result == true)
                    return true;
            }

            return false;
        }

        public void AdvanceToNextState()
        {
            route.dfaStack.Pop();
            if (route.dfaStack.Count == 0)
            {
                route.result = Result.Accepted;
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