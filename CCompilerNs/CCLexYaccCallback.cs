namespace CCompilerNs
{
    public class CCLexYaccCallback
    {
        public static Program Program(List<FunDecl> funcDecls)
        {
            Program p = new Program();
            p.childrenForPrint.AddRange(funcDecls);

            return p;
        }

        public static List<FunDecl> FunDecls(FunDecl funcDecl, List<FunDecl> prevFuncDecls)
        {
            List<FunDecl> funcDecls = new List<FunDecl>();
            if (prevFuncDecls != null)
                funcDecls.AddRange(prevFuncDecls);

            funcDecls.Add(funcDecl);

            return funcDecls;
        }

        public static FunDecl FuncDecl(string returnType, string functionName, List<Statement> statements)
        {
            FunDecl f = new FunDecl();
            if (returnType == "int")
            {
                f.returnType = VariableType.int_type;
            }

            f.functionName = functionName;
            f.statements = statements;

            foreach (Statement s in statements)
            {
                if (s is DeclareStatement)
                {
                    DeclareStatement a = (DeclareStatement)s;
                    f.AddLocalVariable(a);
                }
            }


            f.childrenForPrint.AddRange(statements);

            return f;
        }

        public static List<Statement> Statements(Statement s, List<Statement> prevStatements)
        {
            List<Statement> statements = new List<Statement>();
            if (prevStatements != null)
                statements.AddRange(prevStatements);

            statements.Add(s);

            return statements;
        }

        public static ReturnStatement ReturnStatement(Expression expression)
        {
            ReturnStatement n = new ReturnStatement();
            n.returnValue = expression;
            n.childrenForPrint.Add(expression);

            return n;
        }

        public static DeclareStatement DeclareStatement(string type, string id, Expression expression)
        {
            DeclareStatement n = new DeclareStatement();
            if (type == "int")
            {
                n.type = VariableType.int_type;
            }

            n.name = id;
            n.value = expression;

            if (expression != null)
                n.childrenForPrint.Add(expression);

            return n;
        }

        public static AssignmentStatement AssignmentStatement(string id, Expression expression)
        {
            AssignmentStatement a = new AssignmentStatement();
            a.name = id;
            a.value = expression;
            a.childrenForPrint.Add(expression);

            return a;
        }

        public static Expression Expression(int intValue)
        {
            Expression a = new Expression();
            a.intValue = intValue;

            return a;
        }

        public static Expression Expression(string id)
        {
            Expression a = new Expression();
            a.variableName = id;

            return a;
        }

        public static Expression ExpressionCallFunc(string functionName)
        {
            Expression a = new Expression();
            a.functionName = functionName;

            return a;
        }

        public static Expression Expression(Expression lhs)
        {
            Expression a = new Expression();
            a.lhs = lhs;
            a.childrenForPrint.Add(lhs);

            return a;
        }

        public static Expression Expression(Expression lhs, string op, int rhsIntValue)
        {
            Expression a = new Expression();
            a.lhs = lhs;
            a.op = op;

            a.rhs = new Expression();
            a.rhs.intValue = rhsIntValue;

            a.childrenForPrint.Add(lhs);
            a.childrenForPrint.Add(a.rhs);

            return a;
        }

        public static Expression ExpressionCallFunc(Expression lhs, string op, string functionName)
        {
            Expression a = new Expression();
            a.lhs = lhs;
            a.op = op;

            a.rhs = new Expression();
            a.rhs.functionName = functionName;

            a.childrenForPrint.Add(lhs);
            a.childrenForPrint.Add(a.rhs);

            return a;
        }

        public static Expression Expression(Expression lhs, string op, string variableName)
        {
            Expression a = new Expression();

            a.lhs = lhs;
            a.op = op;

            a.rhs = new Expression();
            a.rhs.variableName = variableName;

            a.childrenForPrint.Add(lhs);
            a.childrenForPrint.Add(a.rhs);

            return a;
        }

        public static Expression Expression(Expression lhs, string op, Expression rhs)
        {
            Expression a = new Expression();
            a.lhs = lhs;
            a.op = op;
            a.rhs = rhs;

            a.childrenForPrint.Add(lhs);
            a.childrenForPrint.Add(rhs);

            return a;
        }
    }
}
