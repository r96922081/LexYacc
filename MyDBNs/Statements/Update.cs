namespace MyDBNs
{
    public class Update
    {
        public static void UpdateRowsVarchar(Table table, int lhsColumnIndex, SetExpressionType setExpression, HashSet<int> selectedRows)
        {
            List<string> rows = StringExpression.Parse(table, setExpression.rhs);

            for (int i = 0; i < rows.Count; i++)
            {
                if (selectedRows != null && !selectedRows.Contains(i))
                    continue;

                table.rows[i][lhsColumnIndex] = rows[i];
            }
        }

        public static void UpdateRowsNumber(Table table, int lhsColumnIndex, SetExpressionType setExpression, HashSet<int> selectedRows)
        {
            SqlArithmeticExpressionLexYaccCallback.table = table;
            List<double> values = (List<double>)sql_arithmetic_expression.Parse(setExpression.rhs);

            for (int i = 0; i < table.rows.Count; i++)
            {
                if (selectedRows != null && !selectedRows.Contains(i))
                    continue;

                object[] row = table.rows[i];
                row[lhsColumnIndex] = values[i];
            }
        }

        public static void UpdateRowsNull(Table table, int lhsColumnIndex, HashSet<int> selectedRows)
        {
            for (int i = 0; i < table.rows.Count; i++)
            {
                if (selectedRows != null && !selectedRows.Contains(i))
                    continue;

                object[] row = table.rows[i];
                row[lhsColumnIndex] = null;
            }
        }



        public static int UpdateRows(string tableName, List<SetExpressionType> setExpressions, string condition)
        {
            Table table = Util.GetTable(tableName);

#if !MarkUserOfSqlCodeGen
            SqlBooleanExpressionLexYaccCallback.table = table;
            HashSet<int> selectedRows = null;
            if (condition != null)
            {
                object ret = sql_boolean_expression.Parse(condition);
                selectedRows = (HashSet<int>)ret;
            }

            foreach (SetExpressionType setExpression in setExpressions)
            {
                int lhsColumnIndex = table.GetColumnIndex(setExpression.lhsColumn);

                if (setExpression.rhs == null)
                {
                    UpdateRowsNull(table, lhsColumnIndex, selectedRows);
                }
                else if (setExpression.rhsType == StringType.String)
                {
                    if (table.columnTypes[lhsColumnIndex] != ColumnType.VARCHAR)
                        throw new Exception("Update with wrong type");

                    UpdateRowsVarchar(table, lhsColumnIndex, setExpression, selectedRows);
                }
                else if (setExpression.rhsType == StringType.Number)
                {
                    if (table.columnTypes[lhsColumnIndex] != ColumnType.NUMBER)
                        throw new Exception("Update with wrong type");

                    UpdateRowsNumber(table, lhsColumnIndex, setExpression, selectedRows);
                }
            }

            if (condition == null)
                return table.rows.Count;
            else
                return selectedRows.Count;
#endif
        }
    }
}
