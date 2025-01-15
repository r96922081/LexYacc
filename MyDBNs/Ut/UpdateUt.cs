namespace MyDBNs
{
    public class UpdateUt : BaseUt
    {
        public void Ut()
        {
            /*
            -------------------------
            | C1  | C2   | C3 | C4  |
            -------------------------
            | ABC | ABCD | 11 | 22  |
            | DE  | CDE  | 22 | 33  |
            | GH  |      | 22 |     |
            | ABC | B    | 44 | 555 |
            -------------------------
            */

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_UPDATE.DB"));
            int count = (int)sql_statements.Parse("UPDATE A SET C2 = 'ABC'");
            Check(count == 4);

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_UPDATE.DB"));
            count = (int)sql_statements.Parse("UPDATE A SET C2 = 'ABC' where c4 is null");
            Check(count == 1);

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_UPDATE.DB"));
            CheckSyntaxErrorOrException(() => { return sql_statements.Parse("UPDATE A SET C2 = 1"); });

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_UPDATE.DB"));
            CheckSyntaxErrorOrException(() => { return sql_statements.Parse("UPDATE A SET C3 = 'A'"); });

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_UPDATE.DB"));
            count = (int)sql_statements.Parse("UPDATE A SET C1 = 'XD', C2 = NULL, C3 = NULL, C4 = 33");
            Check(count == 4);

            List<object[]> rows = RunSelectStatementAndConvertResult("SELECT C1, C2, C3, C4 FROM A");
            foreach (object[] row in rows)
            {
                Check((string)row[0] == "XD");
                Check(row[1] == null);
                Check(row[2] == null);
                Check((double)row[3] == 33);
            }
        }
    }
}
