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

        public static void UpdateRows(string tableName, List<SetExpressionType> setExpressions, string condition)
        {
            Table table = Util.GetTable(tableName);

            //Verifier.VerifyUpdate(tableName, setExpressions);

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

                if (setExpression.rhsType == StringType.String)
                {
                    UpdateRowsVarchar(table, lhsColumnIndex, setExpression, selectedRows);
                }
                else if (setExpression.rhsType == StringType.Number)
                {
                    UpdateRowsNumber(table, lhsColumnIndex, setExpression, selectedRows);
                }
            }
#endif
        }
    }
}
