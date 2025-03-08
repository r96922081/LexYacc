namespace MyDBNs
{
    public class CreateDropUt : BaseUt
    {
        public void Ut()
        {
            Util.DeleteAllTable();

            CheckOk(sql_statements.Parse("CREATE TABLE A ( C1 VARCHAR(123), C2 NUMBER)"));

            Table t = Util.GetTable("A");
            Check(t.columns[0].columnName == "C1");
            Check(t.columns[1].columnName == "C2");
            Check(t.columns[0].size == 123);
            Check(t.columns[0].type == ColumnType.VARCHAR);
            Check(t.columns[1].type == ColumnType.NUMBER);

            CheckOk(sql_statements.Parse("DROP TABLE A"));
            Check(Util.GetTables().Count == 0);
        }
    }
}
