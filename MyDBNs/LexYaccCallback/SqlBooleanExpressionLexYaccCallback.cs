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
                ColumnType t = table.columnNameToTypesMap[lhs];
                if (t == ColumnType.NUMBER)
                    lhsType2 = StringType.Number;
                else
                    lhsType2 = StringType.String;
            }

            StringType rhsType2 = rhsType;
            if (rhsType2 == StringType.Column)
            {
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
            return null;
        }

        public static HashSet<int> BooleanExpressionVarcharColumn(string lhs, string op, string rhs)
        {
            StringType lhsType = Util.GetStringType(lhs);
            StringType rhsType = Util.GetStringType(rhs);
            if (lhsType == StringType.Column)
                lhs = lhs.ToUpper();
            if (rhsType == StringType.Column)
                rhs = rhs.ToUpper();

            VerifyBooleanExpression(lhs, op, rhs);

            HashSet<int> rows = new HashSet<int>();

            List<Table> tables = MyDBNs.DB.tables;
            Table table = Util.GetTable(tableName);

            int lhsColumnIndex = -1;
            int rhsColumnIndex = -1;

            ColumnType lhsType2 = ColumnType.VARCHAR;
            if (lhsType == StringType.String)
                lhsType2 = ColumnType.VARCHAR;
            else
            {
                ColumnType t = table.columnNameToTypesMap[lhs];
                if (t == ColumnType.NUMBER)
                    lhsType2 = ColumnType.NUMBER;
                else
                    lhsType2 = ColumnType.VARCHAR;

                lhsColumnIndex = table.columnNameToIndexMap[lhs];
            }

            ColumnType rhsType2 = ColumnType.VARCHAR;
            if (rhsType == StringType.String)
                rhsType2 = ColumnType.VARCHAR;
            else
            {
                ColumnType t = table.columnNameToTypesMap[rhs];
                if (t == ColumnType.NUMBER)
                    rhsType2 = ColumnType.NUMBER;
                else
                    rhsType2 = ColumnType.VARCHAR;

                rhsColumnIndex = table.columnNameToIndexMap[rhs];
            }


            for (int i = 0; i < table.rows.Count; i++)
            {
                object[] row = table.rows[i];

                object lhsValue = null;
                object rhsValue = null;

                if (lhsType == StringType.Column)
                {
                    lhsValue = row[lhsColumnIndex];
                }
                else if (lhsType2 == ColumnType.VARCHAR)
                {
                    if (lhs != null)
                        lhsValue = lhs.Substring(1, lhs.Length - 2);
                }

                if (rhsType == StringType.Column)
                {
                    rhsValue = row[rhsColumnIndex];
                }
                else if (rhsType2 == ColumnType.VARCHAR)
                {
                    if (rhs != null)
                        rhsValue = rhs.Substring(1, rhs.Length - 2);
                }

                switch (op)
                {
                    case "=":
                        if (EvaluateBooleanExpression(BooleanOperator.Equal, lhsValue, rhsValue, lhsType2))
                            rows.Add(i);
                        break;
                    case "!=":
                        if (EvaluateBooleanExpression(BooleanOperator.NotEqual, lhsValue, rhsValue, lhsType2))
                            rows.Add(i);
                        break;
                    case "<":
                        if (EvaluateBooleanExpression(BooleanOperator.LessThan, lhsValue, rhsValue, lhsType2))
                            rows.Add(i);
                        break;
                    case "<=":
                        if (EvaluateBooleanExpression(BooleanOperator.LessThanEqualTo, lhsValue, rhsValue, lhsType2))
                            rows.Add(i);
                        break;
                    case ">":
                        if (EvaluateBooleanExpression(BooleanOperator.GreaterThan, lhsValue, rhsValue, lhsType2))
                            rows.Add(i);
                        break;
                    case ">=":
                        if (EvaluateBooleanExpression(BooleanOperator.GreaterThanEqualTo, lhsValue, rhsValue, lhsType2))
                            rows.Add(i);
                        break;
                }
            }

            return rows;
        }
    }
}
