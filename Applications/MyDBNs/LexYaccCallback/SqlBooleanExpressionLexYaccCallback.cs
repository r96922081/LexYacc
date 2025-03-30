namespace MyDBNs
{
    public class SqlBooleanExpressionLexYaccCallback
    {
        public static Table table = null;

        public static void VerifyBooleanExpression(string lhs, string op, string rhs)
        {
            StringType lhsType = Util.GetStringType(lhs);
            StringType rhsType = Util.GetStringType(rhs);

            StringType lhsType2 = lhsType;
            if (lhsType2 == StringType.Column)
            {
                ColumnType t = table.GetColumnType(lhs);
                if (t == ColumnType.NUMBER)
                    lhsType2 = StringType.Number;
                else
                    lhsType2 = StringType.String;
            }

            StringType rhsType2 = rhsType;
            if (rhsType2 == StringType.Column)
            {
                ColumnType t = table.GetColumnType(rhs);
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
                    return EvaluateBooleanExpression(BooleanOperator.LessThan, lhs, rhs, type) || EvaluateBooleanExpression(BooleanOperator.Equal, lhs, rhs, type);
                case BooleanOperator.GreaterThan:
                    return !EvaluateBooleanExpression(BooleanOperator.LessThan, lhs, rhs, type) && !EvaluateBooleanExpression(BooleanOperator.Equal, lhs, rhs, type);
                case BooleanOperator.GreaterThanEqualTo:
                    return !EvaluateBooleanExpression(BooleanOperator.LessThan, lhs, rhs, type);
            }

            return false;
        }

        // return true if s is column and its type  is varchar
        private static bool IsVarcharColumn(string s)
        {
            try
            {
                table.GetColumnIndex(s);
            }
            catch
            {
                return false;
            }

            return table.GetColumnType(s) == ColumnType.VARCHAR;
        }

        public static HashSet<int> BooleanExpressionNumberColumn(string lhs, string op, string rhs)
        {
            // work around the yacc rule issue,
            // that in this case, C1 = C2, although both C1 and C2 are VARCHAR type,
            // but in my yacc declare it will be consider as both are NUMBER type
            if (IsVarcharColumn(lhs) && IsVarcharColumn(rhs))
                return BooleanExpressionVarcharColumn(lhs, op, rhs);

            SqlArithmeticExpressionLexYaccCallback.table = table;
            List<double> lhsValues = (List<double>)sql_arithmetic_expression.Parse(lhs);
            List<double> rhsValues = (List<double>)sql_arithmetic_expression.Parse(rhs);

            HashSet<int> rows = new HashSet<int>();

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

        // Gen by ChatGPT
        public static bool VarcharIsLike(string text, string pattern)
        {
            return Match(text, 0, pattern, 0);
        }

        private static bool Match(string text, int tIndex, string pattern, int pIndex)
        {
            if (tIndex == text.Length && pIndex == pattern.Length)
                return true;

            if (pIndex == pattern.Length)
                return false;

            if (pattern[pIndex] == '%')
            {
                for (int i = tIndex; i <= text.Length; i++)
                {
                    if (Match(text, i, pattern, pIndex + 1))
                        return true;
                }
                return false;
            }

            if (pattern[pIndex] == '_')
            {
                return tIndex < text.Length && Match(text, tIndex + 1, pattern, pIndex + 1);
            }

            return tIndex < text.Length && text[tIndex] == pattern[pIndex] && Match(text, tIndex + 1, pattern, pIndex + 1);
        }

        public static HashSet<int> BooleanExpressionLike(string lhs, string op, string rhs)
        {
            bool not = op.ToUpper().Trim().StartsWith("NOT");

            List<string> lhsValues = StringExpression.Parse(table, lhs);

            string pattern = Util.ExtractStringFromSingleQuote(rhs);

            HashSet<int> rows = new HashSet<int>();

            for (int i = 0; i < lhsValues.Count; i++)
            {
                if (lhsValues[i] == null)
                    continue;

                bool like = VarcharIsLike(lhsValues[i], pattern);

                if (like && !not)
                    rows.Add(i);
                else if (!like && not)
                    rows.Add(i);
            }

            return rows;
        }


        public static HashSet<int> BooleanExpressionNullity(string lhs, string op)
        {
            bool isNull = op.ToUpper() == "IS";

            HashSet<int> rows = new HashSet<int>();

            int columnIndex = table.GetColumnIndex(lhs);

            for (int i = 0; i < table.rows.Count; i++)
            {
                if (isNull)
                {
                    if (table.rows[i][columnIndex] == null)
                        rows.Add(i);
                }
                else
                {
                    if (table.rows[i][columnIndex] != null)
                        rows.Add(i);
                }
            }

            return rows;
        }

        public static ColumnType GetType(string s, ref int columnIndex)
        {
            StringType type = Util.GetStringType(s);

            ColumnType type2 = ColumnType.NUMBER;
            if (type == StringType.String)
                type2 = ColumnType.VARCHAR;
            else if (type == StringType.Number)
                type2 = ColumnType.NUMBER;
            else
            {
                ColumnType t = table.GetColumnType(s);
                if (t == ColumnType.NUMBER)
                    type2 = ColumnType.NUMBER;
                else
                    type2 = ColumnType.VARCHAR;

                columnIndex = table.GetColumnIndex(s);
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
            List<string> lhsValues = StringExpression.Parse(table, lhs);
            List<string> rhsValues = StringExpression.Parse(table, rhs);

            HashSet<int> rows = new HashSet<int>();

            for (int i = 0; i < lhsValues.Count; i++)
            {
                switch (op)
                {
                    case "=":
                        if (EvaluateBooleanExpression(BooleanOperator.Equal, lhsValues[i], rhsValues[i], ColumnType.VARCHAR))
                            rows.Add(i);
                        break;
                    case "!=":
                        if (EvaluateBooleanExpression(BooleanOperator.NotEqual, lhsValues[i], rhsValues[i], ColumnType.VARCHAR))
                            rows.Add(i);
                        break;
                    case "<":
                        if (EvaluateBooleanExpression(BooleanOperator.LessThan, lhsValues[i], rhsValues[i], ColumnType.VARCHAR))
                            rows.Add(i);
                        break;
                    case "<=":
                        if (EvaluateBooleanExpression(BooleanOperator.LessThanEqualTo, lhsValues[i], rhsValues[i], ColumnType.VARCHAR))
                            rows.Add(i);
                        break;
                    case ">":
                        if (EvaluateBooleanExpression(BooleanOperator.GreaterThan, lhsValues[i], rhsValues[i], ColumnType.VARCHAR))
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
    }
}
