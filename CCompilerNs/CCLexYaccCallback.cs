namespace CCompilerNs
{
    public class CCLexYaccCallback
    {
        public static Program Program(FunDecl funcDecl)
        {
            Program p = new Program();
            p.children.Add(funcDecl);

            return p;
        }

        public static FunDecl FuncDecl(string returnType, string functionName, Statement statement)
        {
            FunDecl f = new FunDecl();
            if (returnType == "int")
            {
                f.returnType = VariableType.int_type;
            }

            f.functionName = functionName;
            f.children.Add(statement);

            return f;
        }

        public static ReturnStatement ReturnStatement(AddExpression addExpression)
        {
            ReturnStatement n = new ReturnStatement();
            n.returnValue = addExpression;
            n.children.Add(addExpression);

            return n;
        }

        public static AddExpression AddExpression(int intValue)
        {
            AddExpression a = new AddExpression();
            a.intValue = intValue;

            return a;
        }

        public static AddExpression AddExpression(AddExpression lhs, string op, int rhsIntValue)
        {
            AddExpression a = new AddExpression();
            a.lhs = lhs;
            a.op = op;

            a.rhs = new AddExpression();
            a.rhs.intValue = rhsIntValue;

            a.children.Add(lhs);
            a.children.Add(a.rhs);

            return a;
        }
    }
}
