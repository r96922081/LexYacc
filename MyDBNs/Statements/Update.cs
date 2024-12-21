namespace MyDBNs
{
    public class Update
    {
        public static void UpdateRows(string tableName, List<Tuple<string, string>> setExpression, string condition)
        {
            tableName = tableName.ToUpper();
            setExpression = setExpression.Select(tuple => Tuple.Create(tuple.Item1.ToUpper(), tuple.Item2)).ToList();

            Verifier.VerifyUpdate(tableName, setExpression);

#if !MarkUserOfSqlCodeGen
            SqlBooleanExpressionLexYaccCallback.table = Util.GetTable(tableName);
            HashSet<int> rows = null;
            if (condition != null)
            {
                object ret = sql_boolean_expression.Parse(condition);
                rows = (HashSet<int>)ret;
            }

            Table table = Util.GetTable(tableName);
            foreach (var kv in setExpression)
            {
                for (int i = 0; i < table.rows.Count; i++)
                {
                    if (condition != null && !rows.Contains(i))
                        continue;

                    object[] row = table.rows[i];

                    string columnName = kv.Item1;
                    int columnIndex = table.columnNameToIndexMap[columnName];

                    ColumnType type = table.columnNameToTypesMap[columnName];

                    StringType valueType = Util.GetStringType(kv.Item2);
                    object value = null;


                    if (valueType == StringType.String)
                    {
                        value = Util.GetString(kv.Item2);
                    }
                    else if (valueType == StringType.Number)
                    {
                        value = Util.GetNumber(kv.Item2);
                    }
                    else if (valueType == StringType.Column)
                    {
                        value = row[table.columnNameToIndexMap[kv.Item2.ToUpper()]];
                    }

                    row[columnIndex] = value;
                }
            }
#endif
        }
    }
}
