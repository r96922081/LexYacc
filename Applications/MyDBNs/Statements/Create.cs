namespace MyDBNs
{
    public class Create
    {
        public static void CreateTable(string name, List<ColumnDeclare> columnDeclares)
        {
            Table table = new Table();
            table.originaName = name;
            table.name = name.ToUpper();
            table.columns = new Column[columnDeclares.Count];

            for (int i = 0; i < columnDeclares.Count; i++)
            {
                table.columns[i] = new Column();
                table.columns[i].originalColumnName = columnDeclares[i].columnName;
                table.columns[i].columnName = columnDeclares[i].columnName.ToUpper();
                table.columns[i].type = columnDeclares[i].type;
                table.columns[i].size = columnDeclares[i].size;
            }

            Gv.db.tables.Add(table);
        }

        public static void AddTable(Table t)
        {
            Gv.db.tables.Add(t);
        }
    }
}
