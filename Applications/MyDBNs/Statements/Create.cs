namespace MyDBNs
{
    public class Create
    {
        public static void CreateTable(string name, List<ColumnDeclare> columnDeclares)
        {
            Table table = new Table();
            table.originalTableName = name;
            table.tableName = name.ToUpper();
            table.columnNames = new string[columnDeclares.Count];
            table.originalColumnNames = new string[columnDeclares.Count];
            table.columnTypes = new ColumnType[columnDeclares.Count];
            table.columnSizes = new int[columnDeclares.Count];

            for (int i = 0; i < columnDeclares.Count; i++)
            {
                table.originalColumnNames[i] = columnDeclares[i].columnName;
                table.columnNames[i] = columnDeclares[i].columnName.ToUpper();
                table.columnTypes[i] = columnDeclares[i].type;
                table.columnSizes[i] = columnDeclares[i].size;
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
