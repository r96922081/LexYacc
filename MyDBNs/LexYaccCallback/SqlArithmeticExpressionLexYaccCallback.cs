namespace MyDBNs
{
    public class SqlArithmeticExpressionLexYaccCallback
    {
        public static Table table = null;

        public static List<double> ArithmeticExpression(List<double> lhs, string op, List<double> rhs)
        {
            List<double> result = new List<double>();
            for (int i = 0; i < lhs.Count; i++)
            {
                if (op == "+")
                    result.Add(lhs[i] + rhs[i]);
                else if (op == "-")
                    result.Add(lhs[i] - rhs[i]);
                else if (op == "*")
                    result.Add(lhs[i] * rhs[i]);
                else if (op == "/")
                    result.Add(lhs[i] / rhs[i]);
            }

            return result;
        }

        public static List<double> GetColumnValues(string column)
        {
            int columnIndex = -1;
            ColumnType type = SqlBooleanExpressionLexYaccCallback.GetType(column, ref columnIndex);

            List<double> values = new List<double>();
            for (int i = 0; i < table.rows.Count; i++)
            {
                object value = table.rows[i][columnIndex];
                if (value == null)
                    values.Add(0);
                else
                    values.Add((double)value);
            }

            return values;
        }

        public static List<double> GetColumnValues(double value)
        {
            List<double> values = new List<double>();
            for (int i = 0; i < table.rows.Count; i++)
                values.Add(value);

            return values;
        }

        public static List<double> GetColumnValues(int value)
        {
            return GetColumnValues((double)value);
        }
    }
}
