namespace MyDBNs
{
    public class DeleteUt : BaseUt
    {
        public void Ut()
        {
            /*
            | C1  | C2    | C3 | C4 |
            -------------------------
            | ABC | ABCD  | 11 | 22 |
            | DE  | CDE   | 22 | 33 |
            | GH  |       | 22 |    |
            | ABC | B     | 44 | 555|
            */

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_DELETE.DB"));
            int count = (int)sql_statements.Parse("DELETE FROM A WHERE C2 IS NOT NULL");
            Check(count == 3);

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_DELETE.DB"));
            count = (int)sql_statements.Parse("DELETE FROM A WHERE C2 IS NULL");
            Check(count == 1);

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_DELETE.DB"));
            count = (int)sql_statements.Parse("DELETE FROM A");
            Check(count == 4);

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_DELETE.DB"));
            count = (int)sql_statements.Parse("DELETE FROM A WHERE C2 = ''");
            Check(count == 0);

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_DELETE.DB"));
            count = (int)sql_statements.Parse("DELETE FROM A WHERE (C2 = 'ABCD' OR C3 > 30) AND C4 < 100");
            Check(count == 1);
        }
    }
}
