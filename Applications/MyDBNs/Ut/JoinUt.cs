namespace MyDBNs
{
    public class JoinUt : BaseUt
    {
        private void Ut1_wo_join_condition()
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
                Check(s.selectedColumnNames.Count == 6);
                Check(s.selectedRows.Count == 15);
                //InteractiveConsole.PrintTable(s);
            }

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_JOIN.DB"));
            o = sql_statements.Parse("SELECT A.C1, B.C2, A.*, B.*, * FROM A JOIN B");
            using (SelectedData s = o as SelectedData)
            {
                Check(s.selectedColumnNames.Count == 14);
                Check(s.selectedRows.Count == 15);
                //InteractiveConsole.PrintTable(s);
            }


            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_JOIN.DB"));
            o = sql_statements.Parse("SELECT * FROM A JOIN B WHERE A.C1 != 2");
            using (SelectedData s = o as SelectedData)
            {
                Check(s.selectedRows.Count == 12);
                //InteractiveConsole.PrintTable(s);
            }

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_JOIN.DB"));
            o = sql_statements.Parse("SELECT * FROM A JOIN B WHERE B.C1 = 'AB' AND A.C1 = 1");
            using (SelectedData s = o as SelectedData)
            {
                Check(s.selectedRows.Count == 4);
                //InteractiveConsole.PrintTable(s);
            }

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_JOIN.DB"));
            o = sql_statements.Parse("SELECT * FROM A X JOIN B Y WHERE Y.C1 = 'AB' AND X.C1 = 1");
            using (SelectedData s = o as SelectedData)
            {
                Check(s.selectedRows.Count == 4);
                //InteractiveConsole.PrintTable(s);
            }
        }

        private void Ut2_w_join_condition()
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

                table C
                ------------
                | C1 | C2  |
                ------------
                | 5  | EE  |
                | 2  | R   |
                | 7  | ABC |
                ------------
            */

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_JOIN_2.DB"));
            object o = sql_statements.Parse("SELECT * FROM A JOIN B ON A.C2 = B.C1 JOIN C ON B.C4 = C.C2 AND A.C1 + 6 = C.C1");
            using (SelectedData s = o as SelectedData)
            {
                Check(s.selectedColumnNames.Count == 8);
                Check(s.selectedRows.Count == 1);
                //InteractiveConsole.PrintTable(s);
            }

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_JOIN_2.DB"));
            o = sql_statements.Parse("SELECT * FROM A X JOIN B Y ON X.C2 = Y.C1 JOIN C Z ON Y.C4 = Z.C2 AND X.C1 + 6 = Z.C1");
            using (SelectedData s = o as SelectedData)
            {
                Check(s.selectedColumnNames.Count == 8);
                Check(s.selectedRows.Count == 1);
                //InteractiveConsole.PrintTable(s);
            }
        }


        public void Ut()
        {
            Ut1_wo_join_condition();
            Ut2_w_join_condition();
        }
    }
}
