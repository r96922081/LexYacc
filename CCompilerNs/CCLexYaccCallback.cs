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

        public static Program Program(AstNode n)
        {
            Program p = new Program();
            p.childrenForPrint.Add(n);

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

        public static FunDecl FuncDecl(string returnType, string functionName, List<LocalVariable> paramList, List<Statement> statements)
        {
            FunDecl f = new FunDecl();
            f.returnType = Util.GetType(returnType);

            f.functionName = functionName;
            f.statements = statements;

            if (paramList != null)
            {
                foreach (LocalVariable param in paramList)
                    f.AddParamVariable(param);
            }

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

            if (expression != null)
                n.childrenForPrint.Add(expression);

            return n;
        }

        public static DeclareStatement DeclareStatement(string type, string id, Expression expression)
        {
            DeclareStatement n = new DeclareStatement();
            n.type = Util.GetType(type);
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

        public static Expression Expression(FunctionCallExpression functionCallExpression)
        {
            Expression a = new Expression();
            a.functionCall = functionCallExpression;

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


        public static Expression Expression(Expression lhs, string op, FunctionCallExpression functionCallExpression)
        {
            Expression a = new Expression();
            a.lhs = lhs;
            a.op = op;

            a.rhs = new Expression();
            a.rhs.functionCall = functionCallExpression;

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

        public static List<Expression> FuncCallParams(Expression param, List<Expression> prevParams)
        {
            List<Expression> paramList = new List<Expression>();
            if (prevParams != null)
                paramList.AddRange(prevParams);

            paramList.Add(param);

            return paramList;
        }

        public static List<LocalVariable> FuncParams(string type, string name, List<LocalVariable> prevFunParams)
        {
            List<LocalVariable> funcParams = new List<LocalVariable>();
            if (prevFunParams != null)
                funcParams.AddRange(prevFunParams);

            LocalVariable funParam = new LocalVariable();
            funParam.name = name;
            funcParams.Add(funParam);

            return funcParams;
        }

        public static FunctionCallExpression FunctionCallExpression(string name, List<Expression> parameters)
        {
            FunctionCallExpression f = new FunctionCallExpression();
            f.name = name;
            f.parameters = parameters;

            return f;
        }

        public static IfStatement IfStatement(Expression lhs, string op, Expression rhs, List<Statement> statements)
        {
            IfStatement i = new IfStatement();
            i.lhs = lhs;
            i.op = op;
            i.rhs = rhs;
            i.statements = statements;

            return i;
        }

        public static List<IfStatement> ElseIfStatements(List<IfStatement> prevStatements, IfStatement statement)
        {
            List<IfStatement> statements = new List<IfStatement>();
            if (prevStatements != null)
                statements.AddRange(prevStatements);

            statements.Add(statement);

            return statements;
        }

        public static CompoundIfStatement CompoundIfStatement(IfStatement ifStatement)
        {
            CompoundIfStatement c = new CompoundIfStatement();

            c.ifStatement = ifStatement;

            return c;
        }

        public static CompoundIfStatement CompoundIfStatement(IfStatement ifStatement, List<IfStatement> elseIfStatements)
        {
            CompoundIfStatement c = new CompoundIfStatement();

            c.ifStatement = ifStatement;
            c.elseIfStatements = elseIfStatements;

            return c;
        }

        public static CompoundIfStatement CompoundIfStatement(IfStatement ifStatement, List<IfStatement> elseIfStatements, IfStatement elseStatement)
        {
            CompoundIfStatement c = new CompoundIfStatement();

            c.ifStatement = ifStatement;
            c.elseIfStatements = elseIfStatements;
            c.elseStatement = elseStatement;

            return c;
        }

        public static CompoundIfStatement CompoundIfStatement(IfStatement ifStatement, IfStatement elseStatement)
        {
            CompoundIfStatement c = new CompoundIfStatement();

            c.ifStatement = ifStatement;
            c.elseStatement = elseStatement;

            return c;
        }
    }
}
