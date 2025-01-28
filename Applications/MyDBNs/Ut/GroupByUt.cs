namespace MyDBNs
{
    public class GroupByUt : BaseUt
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

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_GROUP_BY.DB"));

            object s = sql_statements.Parse("SELECT MAX(C3) FROM A GROUP BY C3");
        }
    }
}
