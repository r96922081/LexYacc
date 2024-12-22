namespace MyDBNs
{
    public class Insert
    {
        public static void InsertRows(string tableName, List<string> columnNames, List<string> values)
        {
            if (columnNames != null)
                columnNames = columnNames.Select(name => name.ToUpper()).ToList();

            Verifier.VerifyInsert(tableName, columnNames, values);

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
                    if (columnType == ColumnType.NUMBER)
                    {
                        rows[columnIndex] = Util.GetNumber(value);
                    }
                    else if (columnType == ColumnType.VARCHAR)
                    {
                        rows[columnIndex] = Util.GetString(value);
                    }
                }
            }

            table.rows.Add(rows);
        }
    }
}
