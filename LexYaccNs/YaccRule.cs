namespace LexYaccNs
{

    /*
    Term:

    A: 'B' c | 'D' e

    Production rule = A: 'B' c | 'D' e
    Production body = 'B' c | 'D' e
    Production = 'B' c
    Production = 'D' e
    */

    public class YaccRule
    {
        public YaccRule()
        {

        }

        public YaccRule(Nonterminal lhs, List<Production> productions)
        {
            this.lhs = lhs;
            this.productions = productions;
        }

        public Nonterminal lhs;
        public List<Production> productions = new List<Production>();

        public override string ToString()
        {
            string ret = "";

            foreach (Production p in productions)
                ret += p.ToString() + "\n";

            return ret;
        }
    }

    public enum ProductionType
    {
        Plain,
        LeftRecursiveFirst,
        LeftRecursiveSecond,
    }

    public class Production
    {
        public List<Symbol> symbols = null;
        public string? action = null;
        public Nonterminal lhs = null;
        public int index = -1;
        public List<LexTokenDef> lexTokenDef;
        public Dictionary<string, string> ruleNonterminalType;
        public ProductionType type = ProductionType.Plain;

        public Production(Nonterminal lhs, List<Symbol> symbols, List<LexTokenDef> lexTokenDef, Dictionary<string, string> ruleNonterminalType, string action, ProductionType type)
        {
            this.lhs = lhs;
            this.symbols = symbols;
            this.lexTokenDef = lexTokenDef;
            this.ruleNonterminalType = ruleNonterminalType;
            this.action = action;
            this.type = type;
        }

        public Production Clone()
        {
            List<Symbol> cloneSymbols = new List<Symbol>();
            foreach (Symbol s in symbols)
                cloneSymbols.Add(s.Clone());

            Production p = new Production(lhs, cloneSymbols, lexTokenDef, ruleNonterminalType, action, type);
            p.index = index;
            return p;
        }

        public string GetFunctionName()
        {
            return string.Format("Rule_{0}_Producton_{1}", lhs.name, index);
        }

        public bool IsEmptyProduction()
        {
            return symbols.Count == 1 && symbols[0] is Terminal && ((Terminal)symbols[0]).type == TerminalType.EMPTY;
        }

        public override string ToString()
        {
            string s = lhs.name + ":";

            foreach (Symbol symbol in symbols)
            {
                if (symbol is Terminal)
                {
                    Terminal t = (Terminal)symbol;
                    if (t.type == TerminalType.TOKEN)
                        s += " " + t.tokenName;
                    else if (t.type == TerminalType.CONSTANT_CHAR)
                        s += " '" + t.constCharValue + "'";
                }
                else if (symbol is Nonterminal)
                {
                    Nonterminal n = (Nonterminal)symbol;
                    s += " " + n.name;
                }
            }

            if (action != null)
                s += " {" + action + "}";

            return s;
        }
    }


    public interface Symbol
    {
        public Symbol Clone();
    }

    public class Nonterminal : Symbol
    {
        public string name;

        public Nonterminal(string name)
        {
            this.name = name;
        }

        public Symbol Clone()
        {
            return new Nonterminal(name);
        }
    }

    // %token < strVal > VOID INT
    public enum TerminalType
    {
        TOKEN,
        CONSTANT_CHAR,
        EMPTY, // for empty rule
        None
    }

    public class Terminal : Symbol
    {
        public static Terminal BuildToken(string tokenName)
        {
            Terminal t = new Terminal();
            t.tokenName = tokenName;
            t.type = TerminalType.TOKEN;

            return t;
        }

        public static Terminal BuildToken(string tokenName, object tokenObject)
        {
            Terminal t = new Terminal();
            t.tokenName = tokenName;
            t.type = TerminalType.TOKEN;
            t.tokenObject = tokenObject;

            return t;
        }

        public static Terminal BuildConstCharTerminal(char constCharValue)
        {
            Terminal t = new Terminal();
            t.constCharValue = "" + constCharValue;
            t.type = TerminalType.CONSTANT_CHAR;

            return t;
        }

        public static Terminal BuildEmptyTerminal()
        {
            Terminal t = new Terminal();
            t.type = TerminalType.EMPTY;

            return t;
        }

        public string tokenName;
        public object tokenObject;
        public string constCharValue;
        public TerminalType type;

        public Symbol Clone()
        {
            Terminal t = new Terminal();
            t.type = type;
            t.tokenName = tokenName;
            t.tokenObject = tokenObject;
            t.constCharValue = constCharValue;

            return t;
        }
    }

}