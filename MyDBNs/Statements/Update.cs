namespace MyDBNs
{
    public class Update
    {
        public static void UpdateRows(string tableName, List<SetExpressionType> setExpressions, string condition)
        {
            tableName = tableName.ToUpper();

            //Verifier.VerifyUpdate(tableName, setExpressions);

#if !MarkUserOfSqlCodeGen
            SqlBooleanExpressionLexYaccCallback.table = Util.GetTable(tableName);
            HashSet<int> rows = null;
            if (condition != null)
            {
                object ret = sql_boolean_expression.Parse(condition);
                rows = (HashSet<int>)ret;
            }

            Table table = Util.GetTable(tableName);
            foreach (SetExpressionType setExpression in setExpressions)
            {
                for (int i = 0; i < table.rows.Count; i++)
                {
                    if (condition != null && !rows.Contains(i))
                        continue;

                    object[] row = table.rows[i];

                    string columnName = setExpression.lhsColumn;
                    int columnIndex = table.columnNameToIndexMap[columnName];

                    ColumnType type = table.columnNameToTypesMap[columnName];

                    object value = null;

                    if (setExpression.rhsType == StringType.String)
                    {
                        value = Util.GetString(setExpression.rhs);
                    }
                    else if (setExpression.rhsType == StringType.Number)
                    {
                        value = Util.GetNumber(setExpression.rhs);
                    }
                    else if (setExpression.rhsType == StringType.Column)
                    {
                        value = row[table.columnNameToIndexMap[setExpression.rhs.ToUpper()]];
                    }

                    row[columnIndex] = value;
                }
            }
#endif
        }
    }
}
