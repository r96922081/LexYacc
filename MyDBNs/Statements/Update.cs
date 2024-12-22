namespace MyDBNs
{
    public class Update
    {
        public static void UpdateRowsVarchar(Table table, int lhsColumnIndex, SetExpressionType setExpression, HashSet<int> selectedRows)
        {
            for (int i = 0; i < table.rows.Count; i++)
            {
                if (selectedRows != null && !selectedRows.Contains(i))
                    continue;

                object[] row = table.rows[i];
                ColumnType type = table.GetColumnType(setExpression.lhsColumn);

                object value = null;

                if (setExpression.rhsType == StringType.String)
                {
                    value = Util.GetString(setExpression.rhs);
                }
                else if (setExpression.rhsType == StringType.Column)
                {
                    value = row[table.GetColumnIndex(setExpression.rhs)];
                }

                row[lhsColumnIndex] = value;
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
