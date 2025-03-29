namespace MyDBNs
{
    public class Delete
    {
        public static int DeleteRows(string tableName, string condition)
        {
            Table table = Util.GetTable(tableName);
            List<object[]> deletedRows = new List<object[]>();
            SqlBooleanExpressionLexYaccCallback.table = table;
            HashSet<int> rows = null;

            if (condition != null)
            {
                object ret = sql_boolean_expression.Parse(condition);
                rows = (HashSet<int>)ret;
            }

            int deleteCount = 0;
            for (int i = table.rows.Count - 1; i >= 0; i--)
            {
                if (condition != null && !rows.Contains(i))
                    continue;

                deletedRows.Add(table.rows[i]);
                table.rows.RemoveAt(i);
                deleteCount++;
            }

            if (DB.inTransaction)
            {
                DB.transactionLog.Push(() =>
                {
                    Transaction.UndoDelete(table, deletedRows);
                });
            }

            return deleteCount;
        }
    }
}
