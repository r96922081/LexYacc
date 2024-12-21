namespace MyDBNs
{
    public class SqlBooleanExpressionLexYaccCallback
    {
        public static string tableName = "";

        public static void VerifyBooleanExpression(string lhs, string op, string rhs)
        {
            List<Table> tables = MyDBNs.DB.tables;
            Table table = Util.GetTable(tableName);
            if (table == null)
                throw new Exception("Table does not exist: " + tableName);

            StringType lhsType = Util.GetStringType(lhs);
            StringType rhsType = Util.GetStringType(rhs);

            StringType lhsType2 = lhsType;
            if (lhsType2 == StringType.Column)
            {
                lhs = lhs.ToUpper();
                ColumnType t = table.columnNameToTypesMap[lhs];
                if (t == ColumnType.NUMBER)
                    lhsType2 = StringType.Number;
                else
                    lhsType2 = StringType.String;
            }

            StringType rhsType2 = rhsType;
            if (rhsType2 == StringType.Column)
            {
                rhs = rhs.ToUpper();
                ColumnType t = table.columnNameToTypesMap[rhs];
                if (t == ColumnType.NUMBER)
                    rhsType2 = StringType.Number;
                else
                    rhsType2 = StringType.String;
            }

            if (lhsType2 != rhsType2)
                throw new Exception(lhs + " & " + rhs + " have different type");
        }

        public static bool EvaluateBooleanExpression(BooleanOperator op, object lhs, object rhs, ColumnType type)
        {
            switch (op)
            {
                case BooleanOperator.Equal:
                    if (lhs == null && rhs == null)
                    {
                        return true;
                    }
                    else if (lhs == null && rhs != null)
                    {
                        return false;
                    }
                    else if (lhs != null && rhs == null)
                    {
                        return false;
                    }
                    else
                    {
                        if (type == ColumnType.VARCHAR)
                        {
                            return ((string)lhs).CompareTo((string)rhs) == 0;
                        }
                        else
                        {
                            return ((double)lhs).CompareTo((double)rhs) == 0;
                        }
                    }
                case BooleanOperator.NotEqual:
                    if (lhs == null && rhs == null)
                    {
                        return false;
                    }
                    else if (lhs == null && rhs != null)
                    {
                        return true;
                    }
                    else if (lhs != null && rhs == null)
                    {
                        return true;
                    }
                    else
                    {
                        if (type == ColumnType.VARCHAR)
                        {
                            return ((string)lhs).CompareTo((string)rhs) != 0;
                        }
                        else
                        {
                            return ((double)lhs).CompareTo((double)rhs) != 0;
                        }
                    }
                case BooleanOperator.LessThan:
                    if (lhs == null && rhs == null)
                    {
                        return false;
                    }
                    else if (lhs == null && rhs != null)
                    {
                        return true;
                    }
                    else if (lhs != null && rhs == null)
                    {
                        return false;
                    }
                    else
                    {
                        if (type == ColumnType.VARCHAR)
                        {
                            return ((string)lhs).CompareTo((string)rhs) < 0;
                        }
                        else
                        {
                            return ((double)lhs).CompareTo((double)rhs) < 0;
                        }
                    }
                case BooleanOperator.LessThanEqualTo:
                    return EvaluateBooleanExpression(BooleanOperator.LessThan, lhs, rhs, type) && EvaluateBooleanExpression(BooleanOperator.Equal, lhs, rhs, type);
                case BooleanOperator.GreaterThan:
                    return !EvaluateBooleanExpression(BooleanOperator.LessThan, lhs, rhs, type) && !EvaluateBooleanExpression(BooleanOperator.Equal, lhs, rhs, type);
                case BooleanOperator.GreaterThanEqualTo:
                    return !EvaluateBooleanExpression(BooleanOperator.LessThan, lhs, rhs, type);
            }

            return false;
        }

        public static HashSet<int> BooleanExpressionNumberColumn(string lhs, string op, string rhs)
        {
            SqlArithmeticExpressionLexYaccCallback.tableName = tableName;
            List<double> lhsValues = (List<double>)sql_arithmetic_expression.Parse(lhs);
            List<double> rhsValues = (List<double>)sql_arithmetic_expression.Parse(rhs);

            HashSet<int> rows = new HashSet<int>();
            Table table = Util.GetTable(tableName);

            for (int i = 0; i < lhsValues.Count; i++)
            {
                switch (op)
                {
                    case "=":
                        if (EvaluateBooleanExpression(BooleanOperator.Equal, lhsValues[i], rhsValues[i], ColumnType.NUMBER))
                            rows.Add(i);
                        break;
                    case "!=":
                        if (EvaluateBooleanExpression(BooleanOperator.NotEqual, lhsValues[i], rhsValues[i], ColumnType.NUMBER))
                            rows.Add(i);
                        break;
                    case "<":
                        if (EvaluateBooleanExpression(BooleanOperator.LessThan, lhsValues[i], rhsValues[i], ColumnType.NUMBER))
                            rows.Add(i);
                        break;
                    case "<=":
                        if (EvaluateBooleanExpression(BooleanOperator.LessThanEqualTo, lhsValues[i], rhsValues[i], ColumnType.NUMBER))
                            rows.Add(i);
                        break;
                    case ">":
                        if (EvaluateBooleanExpression(BooleanOperator.GreaterThan, lhsValues[i], rhsValues[i], ColumnType.NUMBER))
                            rows.Add(i);
                        break;
                    case ">=":
                        if (EvaluateBooleanExpression(BooleanOperator.GreaterThanEqualTo, lhsValues[i], rhsValues[i], ColumnType.NUMBER))
                            rows.Add(i);
                        break;
                }
            }

            return rows;
        }

        public static ColumnType GetType(string s, ref int columnIndex)
        {
            StringType type = Util.GetStringType(s);
            if (type == StringType.Column)
                s = s.ToUpper();

            List<Table> tables = MyDBNs.DB.tables;
            Table table = Util.GetTable(tableName);

            ColumnType type2 = ColumnType.NUMBER;
            if (type == StringType.String)
                type2 = ColumnType.VARCHAR;
            else if (type == StringType.Number)
                type2 = ColumnType.NUMBER;
            else
            {
                ColumnType t = table.columnNameToTypesMap[s];
                if (t == ColumnType.NUMBER)
                    type2 = ColumnType.NUMBER;
                else
                    type2 = ColumnType.VARCHAR;

                columnIndex = table.columnNameToIndexMap[s];
            }

            return type2;
        }

        public static object GetValue(object[] row, string value, ColumnType type, int columnIndex)
        {
            if (columnIndex != -1)
            {
                return row[columnIndex];
            }
            else if (type == ColumnType.NUMBER)
            {
                if (value != null)
                    return double.Parse(value);
                else
                    return null;
            }
            else if (type == ColumnType.VARCHAR)
            {
                if (value != null)
                    return value.Substring(1, value.Length - 2);
                else
                    return null;
            }

            return null;
        }

        public static HashSet<int> BooleanExpressionVarcharColumn(string lhs, string op, string rhs)
        {
            int lhsColumnIndex = -1;
            int rhsColumnIndex = -1;
            ColumnType lhsType = GetType(lhs, ref lhsColumnIndex);
            ColumnType rhsType = GetType(lhs, ref lhsColumnIndex);

            VerifyBooleanExpression(lhs, op, rhs);

            HashSet<int> rows = new HashSet<int>();

            List<Table> tables = MyDBNs.DB.tables;
            Table table = Util.GetTable(tableName);

            for (int i = 0; i < table.rows.Count; i++)
            {
                object[] row = table.rows[i];
                object lhsValue = GetValue(row, lhs, lhsType, lhsColumnIndex);
                object rhsValue = GetValue(row, rhs, rhsType, rhsColumnIndex); ;

                switch (op)
                {
                    case "=":
                        if (EvaluateBooleanExpression(BooleanOperator.Equal, lhsValue, rhsValue, lhsType))
                            rows.Add(i);
                        break;
                    case "!=":
                        if (EvaluateBooleanExpression(BooleanOperator.NotEqual, lhsValue, rhsValue, lhsType))
                            rows.Add(i);
                        break;
                    case "<":
                        if (EvaluateBooleanExpression(BooleanOperator.LessThan, lhsValue, rhsValue, lhsType))
                            rows.Add(i);
                        break;
                    case "<=":
                        if (EvaluateBooleanExpression(BooleanOperator.LessThanEqualTo, lhsValue, rhsValue, lhsType))
                            rows.Add(i);
                        break;
                    case ">":
                        if (EvaluateBooleanExpression(BooleanOperator.GreaterThan, lhsValue, rhsValue, lhsType))
                            rows.Add(i);
                        break;
                    case ">=":
                        if (EvaluateBooleanExpression(BooleanOperator.GreaterThanEqualTo, lhsValue, rhsValue, lhsType))
                            rows.Add(i);
                        break;
                }
            }

            return rows;
        }
    }
}
