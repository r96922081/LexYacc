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
        public int lexTokenIndex = -1;
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
        public List<Terminal> lexTokens = new List<Terminal>();

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
            route.startDFA = new DFA(this, productionRules[0].productions[0]);
            route.dfaStack.Push(route.startDFA);
            route.lexTokenIndex = 0;
            route.result = Result.Alive;

            this.route = route;
            routes.Clear();
            routes.Add(route);
        }

        public Route CloneRoute(int lexTokenIndex)
        {
            Dictionary<DFA, DFA> oldDFAtoNewDFAMapping = new Dictionary<DFA, DFA>();

            DFA newStartDFA = route.startDFA.clone(oldDFAtoNewDFAMapping);
            Route newRoute = new Route();
            newRoute.startDFA = newStartDFA;
            newRoute.lexTokenIndex = lexTokenIndex;
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

        public void ExpandNonterminal(int lexTokenIndex)
        {
            DFA dfa = route.dfaStack.Peek();
            while (dfa.states[dfa.currentState].symbol is Nonterminal)
            {
                if (!dfa.nonterminalDFA.ContainsKey(dfa.currentState))
                {
                    Nonterminal nt = (Nonterminal)dfa.states[dfa.currentState].symbol;
                    List<Production> productions = GetProductions(nt.name);

                    // create new route
                    for (int i = 1; i < productions.Count; i++)
                    {
                        Route newRoute = CloneRoute(lexTokenIndex);

                        DFA newDFA = new DFA(this, productions[i]);
                        newRoute.dfaStack.Peek().nonterminalDFA[dfa.currentState] = newDFA;
                        newRoute.dfaStack.Push(newDFA);

                        routes.Add(newRoute);
                    }

                    DFA newDFA2 = new DFA(this, productions[0]);
                    dfa.nonterminalDFA[dfa.currentState] = newDFA2;
                    route.dfaStack.Push(newDFA2);
                }
                dfa = dfa.nonterminalDFA[dfa.currentState];
            }
        }

        public void ExpandAndFeedEmpty(int lexTokenIndex)
        {
            bool continueFeed = true;

            while (continueFeed)
            {
                if (route.dfaStack.Count == 0)
                    break;

                ExpandNonterminal(lexTokenIndex);

                DFA dfa = route.dfaStack.Peek();
                continueFeed = false;

                while (true)
                {
                    if (dfa.production.IsEmptyProduction())
                    {
                        route.dfaStack.Peek().Feed(this, lexTokenIndex, true);
                        continueFeed = true;
                        break;
                    }

                    if (dfa.nonterminalDFA.ContainsKey(dfa.currentState))
                        dfa = dfa.nonterminalDFA[dfa.currentState];
                    else
                        break;
                }
            }
        }

        public bool FeedInternal()
        {
            ExpandAndFeedEmpty(route.lexTokenIndex);

            for (; route.lexTokenIndex < lexTokens.Count; route.lexTokenIndex++)
            {
                if (route.dfaStack.Count == 0)
                    return false;

                route.dfaStack.Peek().Feed(this, route.lexTokenIndex, false);
                if (route.result == Result.Rejected)
                    return false;

                ExpandAndFeedEmpty(route.lexTokenIndex + 1);
            }

            if (route.result == Result.Accepted)
                return true;
            else
                return false;
        }

        public bool Feed(List<Terminal> lexTokens)
        {
            this.lexTokens = lexTokens;

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