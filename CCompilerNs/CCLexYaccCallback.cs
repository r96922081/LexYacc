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

        public static ReturnStatement ReturnStatement(Expression expression)
        {
            ReturnStatement n = new ReturnStatement();
            n.returnValue = expression;
            n.children.Add(expression);

            return n;
        }

        public static Expression Expression(int intValue)
        {
            Expression a = new Expression();
            a.intValue = intValue;

            return a;
        }

        public static Expression Expression(Expression lhs)
        {
            Expression a = new Expression();
            a.lhs = lhs;
            a.children.Add(lhs);

            return a;
        }

        public static Expression Expression(Expression lhs, string op, int rhsIntValue)
        {
            Expression a = new Expression();
            a.lhs = lhs;
            a.op = op;

            a.rhs = new Expression();
            a.rhs.intValue = rhsIntValue;

            a.children.Add(lhs);
            a.children.Add(a.rhs);

            return a;
        }

        public static Expression Expression(Expression lhs, string op, Expression rhs)
        {
            Expression a = new Expression();
            a.lhs = lhs;
            a.op = op;
            a.rhs = rhs;

            a.children.Add(lhs);
            a.children.Add(rhs);

            return a;
        }
    }
}
