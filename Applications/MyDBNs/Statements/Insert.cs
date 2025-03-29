namespace MyDBNs
{
    public class Insert
    {
        public static int InsertRows(string tableName, List<string> columnNames, List<string> values)
        {
            Table table = Util.GetTable(tableName);

            if (columnNames != null)
                columnNames = columnNames.Select(name => name.ToUpper()).ToList();
            else
                columnNames = table.columns.Select(column => column.columnName.ToUpper()).ToList();

            object[] singleRow = new object[table.columns.Length];

            for (int i = 0; i < values.Count; i++)
            {
                string columnName = columnNames[i];
                int columnIndex = table.GetColumnIndex(columnName);
                ColumnType columnType = table.GetColumnType(columnName);

                string value = values[i];
                if (value == null)
                {
                    singleRow[columnIndex] = null;
                }
                else
                {
                    StringType type = Util.GetStringType(value);

                    if (columnType == ColumnType.NUMBER)
                    {
                        if (type != StringType.Number)
                            throw new Exception("value = " + value + " is not a number");

                        singleRow[columnIndex] = Util.GetNumber(value);
                    }
                    else if (columnType == ColumnType.VARCHAR)
                    {
                        if (type != StringType.String)
                            throw new Exception("value = " + value + " is not a varchar");

                        singleRow[columnIndex] = Util.ExtractStringFromSingleQuote(value);
                    }
                    else
                    {
                        throw new Exception("Unknown column type");
                    }
                }
            }

            table.rows.Add(singleRow);

            if (DB.inTransaction)
            {
                DB.transactionLog.Push(() =>
                {
                    Transaction.UndoInsert(table, singleRow);
                });
            }

            return 1;
        }

        public static void InsertRows(string tableName, List<object[]> rows)
        {
            Table table = Util.GetTable(tableName);
            table.rows.AddRange(rows);
        }
    }
}
