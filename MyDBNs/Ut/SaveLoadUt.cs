namespace MyDBNs
{
    public class SaveLoadUt : Ut
    {
        public void Ut()
        {
            Util.DeleteAllTable();

            sql_statements.Parse("CREATE TABLE A ( C1 VARCHAR(123), C2 NUMBER)");
            sql_statements.Parse("INSERT INTO A ( C1, C2 ) VALUES ( 'ABC', 11 )");
            sql_statements.Parse("INSERT INTO A ( C1 ) VALUES ( 'def' )");
            sql_statements.Parse("INSERT INTO A ( C2 ) VALUES ( 22 )");
            sql_statements.Parse("INSERT INTO A VALUES ( 'GG', 33 )");
            sql_statements.Parse("INSERT INTO A ( C2, C1 ) VALUES ( 55, 'EE' )");
            sql_statements.Parse("INSERT INTO A VALUES ( null, 66 )");
            sql_statements.Parse("INSERT INTO A VALUES ( 'G', null )");

            sql_statements.Parse("SAVE DB SaveLoadUt.db");
            Util.DeleteAllTable();

            sql_statements.Parse("load DB SaveLoadUt.db");
            List<object[]> rows = (List<object[]>)sql_statements.Parse("SELECT * FROM A");
            Check(rows.Count == 7);
        }
    }
}
