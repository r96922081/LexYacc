namespace LexYaccNs
{

    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class State
    {

        public State()
        { }

        public State(Symbol s)
        {
            this.symbol = s;
        }

        public Symbol symbol;
    }

    public class DFA
    {
        public int startState = 0;
        public int acceptedState = -1;
        public int currentState = 0;
        public List<State> states = new List<State>();

        public List<LexTokenDef> lexTokenDef;
        public Dictionary<string, string> ruleNonterminalType;
        public Dictionary<int, object> tokenObjects = new Dictionary<int, object>();
        Dictionary<int, object> param = new Dictionary<int, object>();

        public Production production = null;
        public Dictionary<int, List<DFA>> subDFAs = new Dictionary<int, List<DFA>>();
        public Dictionary<int, int> symbolIndexDict = new Dictionary<int, int>();
        public Yacc yacc = null;

        public DFA(Yacc yacc, Production p, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType)
        {
            production = p;
            this.lexTokenDef = lexTokenDef;
            this.ruleNonterminalType = ruleNonterminalType;
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

        private bool IsFalseAccept(int symbolIndex)
        {
            if (!yacc.IsAccept())
                return false;

            if (symbolIndex + 1 == yacc.symbols.Count)
                return false;
            else
                return true;
        }

        public void Feed(Yacc yacc, int symbolIndex, bool empty)
        {
            symbolIndexDict[currentState] = symbolIndex;

            if (states[currentState].symbol is Nonterminal)
            {
                if (yacc.dfaStack.Count == 0 || yacc.dfaStack.Peek() != subDFAs[currentState][0])
                    yacc.dfaStack.Push(subDFAs[currentState][0]);

                yacc.dfaStack.Peek().Feed(yacc, symbolIndex, empty);
            }
            else
            {
                Symbol symbol = null;
                if (!empty)
                    symbol = yacc.symbols[symbolIndex];
                else
                    symbol = Terminal.BuildEmptyTerminal();

                Terminal t = (Terminal)symbol;

                if (production.IsEmptyProduction())
                {
                    // empty
                    if (t.type == TerminalType.EMPTY)
                    {
                        yacc.AdvanceToNextState();
                        return;
                    }
                    else
                    {
                        yacc.BackToPrevNonterminal();
                        return;
                    }
                }
                else
                {
                    if (t.type == TerminalType.EMPTY)
                    {

                    }
                    else
                    {
                        Terminal stateTerminal = (Terminal)states[currentState].symbol;

                        if (stateTerminal.type == t.type)
                        {
                            if (stateTerminal.type == TerminalType.CONSTANT_CHAR)
                            {
                                if (stateTerminal.constCharValue == t.constCharValue)
                                {
                                    currentState++;
                                    if (currentState == acceptedState)
                                    {
                                        if (IsFalseAccept(symbolIndex))
                                            yacc.BackToPrevNonterminal();
                                        else
                                            yacc.AdvanceToNextState();
                                        return;
                                    }
                                }
                                else
                                {
                                    yacc.BackToPrevNonterminal();
                                    return;
                                }
                            }
                            else if (stateTerminal.type == TerminalType.TOKEN)
                            {
                                if (stateTerminal.tokenName == t.tokenName)
                                {
                                    tokenObjects[currentState] = t.tokenObject;

                                    currentState++;
                                    if (currentState == acceptedState)
                                    {
                                        if (IsFalseAccept(symbolIndex))
                                            yacc.BackToPrevNonterminal();
                                        else
                                            yacc.AdvanceToNextState();
                                        return;
                                    }
                                }
                                else
                                {
                                    yacc.BackToPrevNonterminal();
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
                            yacc.BackToPrevNonterminal();
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
                        param[i + 2] = subDFAs[i][0].CallAction(invokeFunction);
                    else
                        param[i + 1] = subDFAs[i][0].CallAction(invokeFunction);
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
                        param[i + 2] = subDFAs[i][0].CallAction(invokeFunction);
                    else
                        param[i + 1] = subDFAs[i][0].CallAction(invokeFunction);
                }
            }

            object o = invokeFunction(production.GetFunctionName(), param);
            subDFAs[production.symbols.Count - 1][0].param[1] = o;

            return subDFAs[production.symbols.Count - 1][0].CallAction(invokeFunction);
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
    }

}