namespace MyDBNs
{
    public class JoinUt : BaseUt
    {
        private void Ut1()
        {
            /*
                table A
                ------------
                | C1 | C2  |
                ------------
                | 1  | AB  |
                | 1  | ACC |
                | 2  | ABC |
                | 3  | ABC |
                | 4  | AB  |
                ------------

                table B
                -----------------------
                | C1  | C2 | C3 | C4  |
                -----------------------
                | AB  | 3  | 5  | QQ  |
                | ABC | 9  | -1 |     |
                | AB  | 4  | 2  | ABC |
                -----------------------
            */

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_JOIN.DB"));
            object o = sql_statements.Parse("SELECT * FROM A JOIN B");
            using (SelectedData s = o as SelectedData)
            {
                InteractiveConsole.PrintTable(s);
            }

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_JOIN.DB"));
            o = sql_statements.Parse("SELECT A.C1, B.C2, A.*, B.*, * FROM A JOIN B");
            using (SelectedData s = o as SelectedData)
            {
                Check(s.columnNames.Count == 14);
                InteractiveConsole.PrintTable(s);
            }

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_JOIN.DB"));
            o = sql_statements.Parse("SELECT A.C1, B.C2, A.*, B.*, * FROM A JOIN B ON A.C1 = B.C3");
            using (SelectedData s = o as SelectedData)
            {
                InteractiveConsole.PrintTable(s);
            }
        }

        public void Ut()
        {
            Ut1();
        }
    }
}
