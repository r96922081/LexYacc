namespace MyDBNs
{
    public class CreateDropUt : BaseUt
    {
        public void Ut()
        {
            Util.DeleteAllTable();

            CheckOk(sql_statements.Parse("CREATE TABLE A ( C1 VARCHAR(123), C2 NUMBER)"));

            Table t = Util.GetTable("A");
            Check(t.columnNames[0] == "C1");
            Check(t.columnNames[1] == "C2");
            Check(t.columnSizes[0] == 123);
            Check(t.columnTypes[0] == ColumnType.VARCHAR);
            Check(t.columnTypes[1] == ColumnType.NUMBER);

            CheckOk(sql_statements.Parse("DROP TABLE A"));
            Check(Util.GetTables().Count == 0);
        }
    }
}
