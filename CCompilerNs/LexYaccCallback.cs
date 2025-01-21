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

        public static GlobalVariable GlobalVariable(string type, string id, int? intValue, List<int> arraySize)
        {
            GlobalVariable g = new GlobalVariable();
            g.typeInfo = Util.GetType(type);
            g.name = id;
            g.int_value = intValue;

            if (arraySize != null)
                g.typeInfo.arraySize.AddRange(arraySize);

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

        public static DeclareStatement DeclareStatement(string type, string id, Expression expression, List<int> arraySize)
        {
            DeclareStatement n = new DeclareStatement();
            n.typeInfo = Util.GetType(type);
            n.name = id;
            n.value = expression;

            if (arraySize != null)
                n.typeInfo.arraySize.AddRange(arraySize);

            return n;
        }

        public static AssignmentStatement AssignmentStatement(VariableId variableId, Expression expression)
        {
            AssignmentStatement a = new AssignmentStatement();
            a.name = variableId.name;
            a.value = expression;

            if (variableId.arrayIndex != null)
                a.arrayIndex.AddRange(variableId.arrayIndex);

            return a;
        }

        public static AssignmentStatement OpAssignmentStatement(VariableId variableId, Expression rhs, string op)
        {
            AssignmentStatement a = new AssignmentStatement();
            a.name = variableId.name;
            if (variableId.arrayIndex != null)
                a.arrayIndex.AddRange(variableId.arrayIndex);

            Expression lhs = null;

            // +=, -=, *=, /=
            op = op.Substring(0, 1);

            lhs = Expression(variableId);

            a.value = Expression(lhs, op, rhs);

            return a;
        }

        public static AssignmentStatement IncrementDecrement(VariableId variableId, string op)
        {
            AssignmentStatement a = new AssignmentStatement();
            a.name = variableId.name;
            if (variableId.arrayIndex != null)
                a.arrayIndex.AddRange(variableId.arrayIndex);

            Expression rhs = Expression(1);

            Expression lhs = null;

            // ++, --
            op = op.Substring(0, 1);

            lhs = Expression(variableId);

            a.value = Expression(lhs, op, rhs);

            return a;
        }

        public static Expression Expression(int intValue)
        {
            Expression a = new Expression();
            a.intValue = intValue;

            return a;
        }

        public static Expression Expression(VariableId variableId)
        {
            Expression a = new Expression();
            a.variableName = variableId.name;
            a.arrayIndex.AddRange(variableId.arrayIndex);

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

            return a;
        }

        public static Expression Expression(Expression lhs, string op, int rhsIntValue)
        {
            Expression a = new Expression();
            a.lhs = lhs;
            a.op = op;

            a.rhs = new Expression();
            a.rhs.intValue = rhsIntValue;

            return a;
        }


        public static Expression Expression(Expression lhs, string op, FunctionCallExpression functionCallExpression)
        {
            Expression a = new Expression();
            a.lhs = lhs;
            a.op = op;

            a.rhs = new Expression();
            a.rhs.functionCall = functionCallExpression;

            return a;
        }

        public static Expression Expression(Expression lhs, string op, string variableName)
        {
            Expression a = new Expression();

            a.lhs = lhs;
            a.op = op;

            a.rhs = new Expression();
            a.rhs.variableName = variableName;

            return a;
        }

        public static Expression Expression(Expression lhs, string op, Expression rhs)
        {
            Expression a = new Expression();
            a.lhs = lhs;
            a.op = op;
            a.rhs = rhs;

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

        public static List<int> ArraySize(int size, List<int> prev)
        {
            List<int> arraySize = new List<int>();
            if (prev != null)
                arraySize.AddRange(prev);

            arraySize.Add(size);

            return arraySize;
        }

        public static List<int> ParamArraySize(List<int> size)
        {
            List<int> arraySize = new List<int>();
            arraySize.Add(-1);

            if (size != null)
                arraySize.AddRange(size);

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



        public static List<Variable> FuncParams(string type, string name, List<Variable> prevFunParams)
        {
            List<Variable> functionParams = new List<Variable>();
            if (prevFunParams != null)
                functionParams.AddRange(prevFunParams);

            Variable funParam = new Variable();
            funParam.name = name;
            funParam.typeInfo = Util.GetType(type);
            funParam.scope = VariableScopeEnum.param;
            functionParams.Add(funParam);

            return functionParams;
        }

        public static List<Variable> FuncParamsArray(string type, string name, List<int> arraySize, List<Variable> prevFunParams)
        {
            List<Variable> functionParams = new List<Variable>();
            if (prevFunParams != null)
                functionParams.AddRange(prevFunParams);

            Variable funParam = new Variable();
            funParam.name = name;
            funParam.typeInfo = Util.GetType(type);
            funParam.typeInfo.arraySize.AddRange(arraySize);
            funParam.scope = VariableScopeEnum.param;
            functionParams.Add(funParam);

            return functionParams;
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

        public static ForLoopStatement ForLoopStatement(AssignmentStatement initializer, Expression conditionLhs, string conditionOp, Expression conditionrhs, AssignmentStatement updater, List<Statement> statements)
        {
            ForLoopStatement f = new ForLoopStatement();
            f.initializer = initializer;
            f.conditionLhs = conditionLhs;
            f.conditionOp = conditionOp;
            f.conditionrhs = conditionrhs;
            f.updater = updater;
            f.statements = statements;

            return f;
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

        public static StructField StructField(string type, string name, List<int> arraySize)
        {
            StructField f = new StructField();
            f.typeInfo = Util.GetType(type);
            f.name = name;

            if (arraySize != null)
                f.typeInfo.arraySize.AddRange(arraySize);

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

        public static VariableId VariableId(string id, List<Expression> arrayIndex)
        {
            VariableId v = new VariableId();
            v.name = id;
            if (arrayIndex != null)
                v.arrayIndex.AddRange(arrayIndex);
            return v;
        }
    }
}
