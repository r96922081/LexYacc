namespace MyDBNs
{
    public class Create
    {
        public static void CreateTable(string name, List<(string, string)> columnDeclare)
        {
            Verifier.VerifyCreateTable(name, columnDeclare);

            Table table = new Table();
            table.originalTableName = name;
            table.tableName = name.ToUpper();
            table.columnNames = new string[columnDeclare.Count];
            table.originalColumnNames = new string[columnDeclare.Count];
            table.columnTypes = new ColumnType[columnDeclare.Count];
            table.columnSizes = new int[columnDeclare.Count];

            for (int i = 0; i < columnDeclare.Count; i++)
            {
                table.originalColumnNames[i] = columnDeclare[i].Item1;
                table.columnNames[i] = columnDeclare[i].Item1.ToUpper();
                string columnType = columnDeclare[i].Item2.ToUpper();
                if (columnType == "NUMBER_DOUBLE")
                {
                    table.columnTypes[i] = ColumnType.NUMBER;
                }
                else if (columnType.StartsWith("VARCHAR"))
                {
                    table.columnTypes[i] = ColumnType.VARCHAR;
                    int left = columnType.IndexOf('(');
                    int right = columnType.LastIndexOf(')');

                    int lengthInt;
                    string length = columnType.Substring(left + 1, right - left - 1);
                    int.TryParse(length, out lengthInt);
                    table.columnSizes[i] = lengthInt;
                }
            }


            table.columnNameToIndexMap = new Dictionary<string, int>();
            table.columnNameToTypesMap = new Dictionary<string, ColumnType>();

            for (int i = 0; i < table.columnNames.Length; i++)
            {
                table.columnNameToIndexMap.Add(table.columnNames[i], i);
                table.columnNameToTypesMap.Add(table.columnNames[i], table.columnTypes[i]);
            }

            DB.tables.Add(table);
        }
    }
}
