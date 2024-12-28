namespace MyDBNs
{
    public class Insert
    {
        public static void InsertRows(string tableName, List<string> columnNames, List<string> values)
        {
            if (columnNames != null)
                columnNames = columnNames.Select(name => name.ToUpper()).ToList();

            Table table = Util.GetTable(tableName);

            if (columnNames == null || columnNames.Count == 0)
                columnNames = table.columnNames.ToList();

            object[] rows = new object[table.columnNames.Length];

            for (int i = 0; i < values.Count; i++)
            {
                string columnName = columnNames[i];
                int columnIndex = table.GetColumnIndex(columnName);
                ColumnType columnType = table.GetColumnType(columnName);



                string value = values[i];
                if (value == null)
                {
                    rows[columnIndex] = null;
                }
                else
                {
                    StringType type = Util.GetStringType(value);

                    if (columnType == ColumnType.NUMBER)
                    {
                        if (type != StringType.Number)
                            throw new Exception("value = " + value + " is not a number");

                        rows[columnIndex] = Util.GetNumber(value);
                    }
                    else if (columnType == ColumnType.VARCHAR)
                    {
                        if (type != StringType.String)
                            throw new Exception("value = " + value + " is not a varchar");

                        rows[columnIndex] = Util.ExtractStringFromSingleQuote(value);
                    }
                }
            }

            table.rows.Add(rows);
        }
    }
}
