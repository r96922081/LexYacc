namespace MyDBNs
{
    public class Delete
    {
        public static void DeleteRows(string tableName, string condition)
        {
            tableName = tableName.ToUpper();

#if !MarkUserOfSqlCodeGen
            SqlConditionLexYaccCallback.tableName = tableName;
            HashSet<int> rows = null;
            if (condition != null)
            {
                object ret = sql_condition_lexyacc.Parse(condition);
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
