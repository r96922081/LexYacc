namespace MyDBNs
{
    public class Create
    {
        public static void CreateTable(string name, List<ColumnDeclare> columnDeclares)
        {
            Table table = new Table();
            table.name = name;
            table.columns = new Column[columnDeclares.Count];

            for (int i = 0; i < columnDeclares.Count; i++)
            {
                table.columns[i] = new Column();
                table.columns[i].columnName = columnDeclares[i].columnName;
                table.columns[i].type = columnDeclares[i].type;
                table.columns[i].size = columnDeclares[i].size;
                table.columns[i].tableName = name;
            }

            DB.tables.Add(table);
        }

        public static void AddTable(Table t)
        {
            DB.tables.Add(t);
        }
    }
}
