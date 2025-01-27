namespace CCompilerNs
{
    public class LexYaccCallback
    {
        public static Program Program(Program prev, GlobalDeclare globalDeclare)
        {
            Program p = new Program();
            if (prev != null)
                p.globalDeclares.AddRange(prev.globalDeclares);

            if (globalDeclare != null)
                p.globalDeclares.Add(globalDeclare);

            return p;
        }

        public static FunctionDeclare FuncDecl(string returnType, string functionName, List<Variable> paramList, List<Statement> statements)
        {
            FunctionDeclare f = new FunctionDeclare();
            f.returnTypeInfo = Util.GetType(returnType);

            f.functionName = functionName;

            if (statements == null)
                statements = new List<Statement>();

            f.statements = statements;

            if (paramList != null)
            {
                foreach (Variable param in paramList)
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

            return n;
        }

        public static GlobalVariable GlobalVariable(Declare d, int? intValue)
        {
            GlobalVariable g = new GlobalVariable();
            g.typeInfo = Util.GetType(d.type);
            g.name = d.name;
            g.int_value = intValue;

            if (d.arraySize != null)
                g.typeInfo.arraySize.AddRange(d.arraySize);

            return g;
        }

        public static GlobalVariable GlobalVariable(string type, string id, char charValue, List<int> arraySize)
        {
            GlobalVariable g = new GlobalVariable();
            g.typeInfo = Util.GetType(type);
            g.name = id;
            g.int_value = charValue;

            if (arraySize != null)
                g.typeInfo.arraySize.AddRange(arraySize);

            return g;
        }

        public static DeclareStatement DeclareStatement(Declare d, Expression expression)
        {
            DeclareStatement n = new DeclareStatement();
            n.typeInfo = Util.GetType(d.type);
            n.name = d.name;
            n.rhsValue = expression;

            if (d.arraySize != null)
                n.typeInfo.arraySize.AddRange(d.arraySize);

            return n;
        }

        public static AssignmentStatement AssignmentStatement(VariableId variableId, Expression expression)
        {
            AssignmentStatement a = new AssignmentStatement();

            VariableIdExpression e = new VariableIdExpression();
            e.variableId = variableId;

            a.lhs = e;
            a.rhsValue = expression;

            return a;
        }

        public static AssignmentStatement OpAssignmentStatement(VariableId variableId, Expression rhs, string op)
        {
            AssignmentStatement a = new AssignmentStatement();
            VariableIdExpression e = new VariableIdExpression();
            e.variableId = variableId;

            a.lhs = e;

            Expression lhs = null;

            // +=, -=, *=, /=
            op = op.Substring(0, 1);

            lhs = Expression(variableId);

            a.rhsValue = Expression(lhs, op, rhs);

            return a;
        }

        public static AssignmentStatement IncrementDecrement(VariableId variableId, string op)
        {
            AssignmentStatement a = new AssignmentStatement();
            VariableIdExpression e = new VariableIdExpression();
            e.variableId = variableId;
            a.lhs = e;

            Expression rhs = Expression(1);

            // ++, --
            op = op.Substring(0, 1);

            Expression lhs = Expression(variableId);

            a.rhsValue = Expression(lhs, op, rhs);

            return a;
        }

        public static Expression Expression(int intValue)
        {
            Expression a = new Expression();
            a.intValue = intValue;

            return a;
        }

        public static Expression Expression(string s)
        {
            Expression e = new Expression();
            e.stringLiternal = s;

            if (!Gv.stringLiteral.ContainsKey(s))
                Gv.stringLiteral.Add(s, "stringLiteral_" + (Gv.sn++));

            return e;
        }

        public static Expression Expression(VariableId variableId)
        {
            VariableIdExpression v = new VariableIdExpression();
            v.variableId = variableId;

            return v;
        }

        public static Expression Expression(VariableId variableId, bool addressOf)
        {
            VariableIdExpression v = new VariableIdExpression();
            v.variableId = variableId;
            v.variableId.addressOf = addressOf;

            return v;
        }

        public static Expression Expression(VariableId variableId, string pointers)
        {
            VariableIdExpression v = new VariableIdExpression();
            v.variableId = variableId;
            v.variableId.pointerCount = pointers.Length;

            return v;
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
            a.first = lhs;

            return a;
        }

        public static Expression Expression(Expression lhs, string op, int rhsIntValue)
        {
            Expression a = new Expression();
            a.first = lhs;
            a.op = op;

            a.second = new Expression();
            a.second.intValue = rhsIntValue;

            return a;
        }


        public static Expression Expression(Expression lhs, string op, FunctionCallExpression functionCallExpression)
        {
            Expression a = new Expression();
            a.first = lhs;
            a.op = op;

            a.second = new Expression();
            a.second.functionCall = functionCallExpression;

            return a;
        }

        public static Expression Expression(Expression lhs, string op, VariableId variableId)
        {
            Expression a = new Expression();

            a.first = lhs;
            a.op = op;
            a.second = Expression(variableId);

            return a;
        }

        public static Expression Expression(Expression lhs, string op, Expression rhs)
        {
            Expression a = new Expression();
            a.first = lhs;
            a.op = op;
            a.second = rhs;

            return a;
        }

        public static List<Expression> FunctionCallParams(Expression param, List<Expression> prevParams)
        {
            List<Expression> paramList = new List<Expression>();
            if (prevParams != null)
                paramList.AddRange(prevParams);

            paramList.Add(param);

            return paramList;
        }

        public static List<int> ArraySize(int size, List<int> prev)
        {
            List<int> arraySize = new List<int>();
            if (prev != null)
                arraySize.AddRange(prev);

            arraySize.Add(size);

            return arraySize;
        }

        public static List<Expression> ArrayIndex(Expression exp, List<Expression> prev)
        {
            List<Expression> arrayIndex = new List<Expression>();
            if (prev != null)
                arrayIndex.AddRange(prev);

            arrayIndex.Add(exp);

            return arrayIndex;
        }



        public static List<Variable> FunctionParams(Declare d, List<Variable> prevFunParams)
        {
            List<Variable> functionParams = new List<Variable>();
            if (prevFunParams != null)
                functionParams.AddRange(prevFunParams);

            Variable functionParam = new Variable();
            functionParam.name = d.name;
            functionParam.typeInfo = Util.GetType(d.type);
            functionParam.typeInfo.arraySize.AddRange(d.arraySize);
            functionParam.scope = VariableScopeEnum.param;
            functionParams.Add(functionParam);

            return functionParams;
        }

        public static Declare Declare(string type, string name, List<int> arraySize)
        {
            Declare d = new Declare();
            d.name = name;
            d.type = type;
            if (arraySize != null)
                d.arraySize.AddRange(arraySize);

            return d;
        }

        public static Declare FunctionParamDeclare(string type, string name, bool hasArray, List<int> arraySize)
        {
            Declare d = new Declare();
            d.name = name;
            d.type = type;

            if (hasArray)
            {
                d.arraySize.Add(-1);
                if (arraySize != null)
                    d.arraySize.AddRange(arraySize);
            }

            return d;
        }

        public static FunctionCallExpression FunctionCallExpression(string name, List<Expression> parameters)
        {
            FunctionCallExpression f = new FunctionCallExpression();
            f.name = name;
            f.parameters = parameters;

            return f;
        }

        public static IfStatement IfStatement(BooleanExpressions booleanExpressions, List<Statement> statements)
        {
            IfStatement i = new IfStatement();
            i.booleanExpressions = booleanExpressions;
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

        public static ForLoopStatement ForLoopStatement(AssignmentStatement initializer, BooleanExpressions booleanExpressions, AssignmentStatement updater, List<Statement> statements)
        {
            ForLoopStatement f = new ForLoopStatement();
            f.initializer = initializer;
            f.booleanExpressions = booleanExpressions;
            f.updater = updater;
            f.statements = statements;

            return f;
        }

        public static WhileLoopStatement WhileLoopStatement(BooleanExpressions booleanExpressions, List<Statement> statements)
        {
            WhileLoopStatement w = new WhileLoopStatement();
            w.booleanExpressions = booleanExpressions;
            w.statements = statements;

            return w;
        }

        public static BooleanExpressions BooleanExpressions(BooleanExpression b2, string op, BooleanExpressions prev)
        {
            BooleanExpressions b = new BooleanExpressions();
            if (prev != null)
            {
                b.booleanExpressions.AddRange(prev.booleanExpressions);
                b.ops.AddRange(prev.ops);
            }

            b.booleanExpressions.Add(b2);
            if (op != null)
                b.ops.Add(op);
            return b;
        }

        public static BooleanExpression BooleanExpression(Expression lhs, string op, Expression rhs)
        {
            BooleanExpression b = new BooleanExpression();
            b.lhs = lhs;

            if (op == null)
            {
                // while (1) case
                b.op = "!=";
                b.rhs = Expression(0);
            }
            else
            {
                b.op = op;
                b.rhs = rhs;
            }

            return b;
        }

        public static BreakStatement BreakStatement()
        {
            return new BreakStatement();
        }

        public static ContinueStatement ContinueStatement()
        {
            return new ContinueStatement();
        }

        public static EmptyStatement EmptyStatement()
        {
            return new EmptyStatement();
        }

        public static List<Statement> ForLoopBody(string s, List<Statement> prevStatements)
        {
            List<Statement> statements = new List<Statement>();
            if (prevStatements != null)
                statements.AddRange(prevStatements);

            if (s == null)
                ;
            else if (s == "break")
                statements.Add(new BreakStatement());
            else if (s == "continue")
                statements.Add(new ContinueStatement());

            return statements;
        }

        public static List<Statement> ForLoopBody(List<Statement> currentStatements, List<Statement> prevStatements)
        {
            List<Statement> statements = new List<Statement>();

            if (prevStatements != null)
                statements.AddRange(prevStatements);

            if (currentStatements != null)
                statements.AddRange(currentStatements);

            return statements;
        }

        public static StructDef StructDef(string name, List<StructField> fields)
        {
            StructDef s = new StructDef();
            s.name = name;
            s.fields = fields;

            return s;
        }

        public static StructField StructField(Declare d)
        {
            StructField f = new StructField();
            f.typeInfo = Util.GetType(d.type);
            f.name = d.name;

            if (d.arraySize != null)
                f.typeInfo.arraySize.AddRange(d.arraySize);

            return f;
        }

        public static List<StructField> StructFields(List<StructField> prev, StructField f)
        {
            List<StructField> fields = new List<StructField>();
            if (prev != null)
                fields.AddRange(prev);

            fields.Add(f);

            return fields;
        }

        public static VariableId VariableId(VariableId prev, string id, List<Expression> arrayIndex, string op)
        {
            VariableId v = new VariableId();
            if (prev != null)
            {
                v.name.AddRange(prev.name);
                v.arrayIndexList.AddRange(prev.arrayIndexList);
                v.op.AddRange(prev.op);
            }

            v.name.Add(id);

            if (arrayIndex != null)
                v.arrayIndexList.Add(arrayIndex);
            else
                v.arrayIndexList.Add(new List<Expression>());

            if (op != null)
                v.op.Add(op);

            return v;
        }

        public static VariableId VariableId(VariableId v, string pointers)
        {
            if (pointers == null)
                v.pointerCount = 0;
            else
                v.pointerCount = pointers.Length;

            return v;
        }
    }
}
