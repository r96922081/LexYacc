namespace MyDBNs
{
    public class Delete
    {
        public static int DeleteRows(string tableName, string condition)
        {
            Table table = Util.GetTable(tableName);
#if !MarkUserOfSqlCodeGen
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

                table.rows.RemoveAt(i);
                deleteCount++;
            }

            return deleteCount;
#endif
        }
    }
}
