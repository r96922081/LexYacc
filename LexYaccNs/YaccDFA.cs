namespace LexYaccNs
{

    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class State
    {
        public DFA nontermnimalDFA = null;
        public Symbol symbol;

        public State()
        { }

        public State(Symbol s)
        {
            this.symbol = s;
        }
    }

    public class DFA
    {
        public int startState = 0;
        public int acceptedState = -1;
        public int currentState = 0;

        public List<State> states = new List<State>();
        public Dictionary<int, DFA> nonterminalDFA = new Dictionary<int, DFA>();

        public Dictionary<int, object> tokenObjects = new Dictionary<int, object>();
        Dictionary<int, object> param = new Dictionary<int, object>();

        public Production production = null;
        public Yacc yacc = null;

        public DFA()
        {
        }

        public DFA(Yacc yacc, Production p)
        {
            production = p;
            this.yacc = yacc;

            if (p.symbols.Count == 0)
            {
                // empty production
                startState = states.Count;
                states.Add(new State());

                acceptedState = states.Count;
                states.Add(new State());

                states[startState].symbol = Terminal.BuildEmptyTerminal();

                currentState = startState;
            }
            else
            {
                foreach (Symbol s in p.symbols)
                    states.Add(new State(s));

                acceptedState = states.Count;
                states.Add(new State());
                startState = 0;
                currentState = 0;
            }
        }

        public void Feed(Yacc yacc, int lexTokenIndex, bool empty)
        {
            if (states[currentState].symbol is Nonterminal)
            {
                if (yacc.route.dfaStack.Count == 0 || yacc.route.dfaStack.Peek() != nonterminalDFA[currentState])
                    yacc.route.dfaStack.Push(nonterminalDFA[currentState]);

                yacc.route.dfaStack.Peek().Feed(yacc, lexTokenIndex, empty);
            }
            else
            {
                Terminal lexToken = null;
                if (!empty)
                    lexToken = yacc.lexTokens[lexTokenIndex];
                else
                    lexToken = Terminal.BuildEmptyTerminal();

                if (production.IsEmptyProduction())
                {
                    // empty
                    if (lexToken.type == TerminalType.EMPTY)
                    {
                        yacc.AdvanceToNextState();
                        return;
                    }
                    else
                    {
                        yacc.route.result = Result.Rejected;
                        return;
                    }
                }
                else
                {
                    if (lexToken.type == TerminalType.EMPTY)
                    {

                    }
                    else
                    {
                        Terminal stateTerminal = (Terminal)states[currentState].symbol;

                        if (stateTerminal.type == lexToken.type)
                        {
                            if (stateTerminal.type == TerminalType.CONSTANT_CHAR)
                            {
                                if (stateTerminal.constCharValue == lexToken.constCharValue)
                                {
                                    currentState++;
                                    if (currentState == acceptedState)
                                    {
                                        yacc.AdvanceToNextState();
                                        return;
                                    }
                                }
                                else
                                {
                                    yacc.route.result = Result.Rejected;
                                    return;
                                }
                            }
                            else if (stateTerminal.type == TerminalType.TOKEN)
                            {
                                if (stateTerminal.tokenName == lexToken.tokenName)
                                {
                                    tokenObjects[currentState] = lexToken.tokenObject;

                                    currentState++;
                                    if (currentState == acceptedState)
                                    {
                                        yacc.AdvanceToNextState();
                                        return;
                                    }
                                }
                                else
                                {
                                    yacc.route.result = Result.Rejected;
                                    return;
                                }
                            }
                            else
                            {
                                Trace.Assert(false);
                            }
                        }
                        else
                        {
                            yacc.route.result = Result.Rejected;
                            return;
                        }
                    }
                }
            }
        }

        public object CallPlainAction(Yacc.CallActionDelegate invokeFunction)
        {
            if (production.IsEmptyProduction())
                return param[1];

            for (int i = 0; i < production.symbols.Count; i++)
            {
                Symbol symbol = production.symbols[i];

                if (symbol is Terminal)
                {
                    Terminal t = (Terminal)symbol;
                    if (t.type == TerminalType.CONSTANT_CHAR || t.type == TerminalType.EMPTY)
                        continue;

                    if (production.type == ProductionType.LeftRecursiveSecond)
                        param[i + 2] = tokenObjects[i];
                    else
                        param[i + 1] = tokenObjects[i];
                }
                else
                {
                    if (production.type == ProductionType.LeftRecursiveSecond)
                        param[i + 2] = nonterminalDFA[i].CallAction(invokeFunction);
                    else
                        param[i + 1] = nonterminalDFA[i].CallAction(invokeFunction);
                }
            }

            return invokeFunction(production.GetFunctionName(), param);
        }

        public object CallLeftRecursionAction(Yacc.CallActionDelegate invokeFunction)
        {
            if (production.IsEmptyProduction())
                return param[1];

            for (int i = 0; i < production.symbols.Count - 1; i++)
            {
                Symbol symbol = production.symbols[i];

                if (symbol is Terminal)
                {
                    Terminal t = (Terminal)symbol;
                    if (t.type == TerminalType.CONSTANT_CHAR)
                        continue;

                    if (production.type == ProductionType.LeftRecursiveSecond)
                        param[i + 2] = tokenObjects[i];
                    else
                        param[i + 1] = tokenObjects[i];
                }
                else
                {
                    if (production.type == ProductionType.LeftRecursiveSecond)
                        param[i + 2] = nonterminalDFA[i].CallAction(invokeFunction);
                    else
                        param[i + 1] = nonterminalDFA[i].CallAction(invokeFunction);
                }
            }

            object o = invokeFunction(production.GetFunctionName(), param);
            nonterminalDFA[production.symbols.Count - 1].param[1] = o;

            return nonterminalDFA[production.symbols.Count - 1].CallAction(invokeFunction);
        }

        public object CallAction(Yacc.CallActionDelegate invokeFunction)
        {
            if (production.type == ProductionType.Plain)
                return CallPlainAction(invokeFunction);
            else
                return CallLeftRecursionAction(invokeFunction);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return RuntimeHelpers.GetHashCode(this);
        }

        public DFA clone(Dictionary<DFA, DFA> oldDFAtoNewDFAMapping)
        {
            DFA newDFA = new DFA();
            newDFA.startState = this.startState;
            newDFA.acceptedState = this.acceptedState;
            newDFA.currentState = this.currentState;
            newDFA.states.AddRange(this.states);
            foreach (var tokenObject in tokenObjects)
                newDFA.tokenObjects.Add(tokenObject.Key, tokenObject.Value);
            foreach (var p in param)
                newDFA.param.Add(p.Key, p.Value);
            newDFA.production = this.production;
            newDFA.yacc = this.yacc;

            oldDFAtoNewDFAMapping.Add(this, newDFA);

            foreach (var s in nonterminalDFA)
                newDFA.nonterminalDFA[s.Key] = s.Value.clone(oldDFAtoNewDFAMapping);


            return newDFA;
        }
    }

}