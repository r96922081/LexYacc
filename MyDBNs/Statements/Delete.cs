namespace MyDBNs
{
    public class Delete
    {
        public static void DeleteRows(string tableName, string condition)
        {
            tableName = tableName.ToUpper();

#if !MarkUserOfSqlCodeGen
            SqlBooleanExpressionLexYaccCallback.table = Util.GetTable(tableName);
            HashSet<int> rows = null;
            if (condition != null)
            {
                object ret = sql_boolean_expression.Parse(condition);
                rows = (HashSet<int>)ret;
            }

            Table table = Util.GetTable(tableName);
            for (int i = table.rows.Count - 1; i >= 0; i--)
            {
                if (condition != null && !rows.Contains(i))
                    continue;

                table.rows.RemoveAt(i);
            }
#endif
        }
    }
}
