namespace MyDBNs
{
    public class GroupByUt : BaseUt
    {
        private void Ut1()
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

            /*
                -----------------------------------------------------------------------------------------------------------
                | C1  | 2_MAX(C3) | 3_MIN(C3) | 4_COUNT(C3) | 5_SUM(C3) | 6_MAX(C4) | 7_MIN(C4) | 8_COUNT(C4) | 9_SUM(C4) |
                -----------------------------------------------------------------------------------------------------------
                | ABC | 44        | 11        | 2           | 55        | 555       | 22        | 2           | 577       |
                | DE  | 22        | 22        | 1           | 22        | 33        | 33        | 1           | 33        |
                | GH  | 22        | 22        | 1           | 22        |           |           | 0           | 0         |
                -----------------------------------------------------------------------------------------------------------             
             */
            object o = sql_statements.Parse("SELECT C1, MAX(C3), MIN(C3), COUNT(C3), SUM(C3), MAX(C4), MIN(C4), COUNT(C4), SUM(C4)  FROM A GROUP BY C1");
            SelectedData s = o as SelectedData;
            //InteractiveConsole.PrintTable(s);
            object[] row = s.table.rows[0];
            int col = 0;
            Check((string)row[col++] == "ABC");
            Check((double)row[col++] == 44);
            Check((double)row[col++] == 11);
            Check((double)row[col++] == 2);
            Check((double)row[col++] == 55);
            Check((double)row[col++] == 555);
            Check((double)row[col++] == 22);
            Check((double)row[col++] == 2);
            Check((double)row[col++] == 577);

            row = s.table.rows[2];
            col = 0;
            Check((string)row[col++] == "GH");
            Check((double)row[col++] == 22);
            Check((double)row[col++] == 22);
            Check((double)row[col++] == 1);
            Check((double)row[col++] == 22);
            Check(row[col++] == null);
            Check(row[col++] == null);
            Check((double)row[col++] == 0);
            Check((double)row[col++] == 0);
        }

        private void Ut2()
        {
            /*
                --------------------------------
                | C1   | C2     | C3 | C4 | C5 |
                --------------------------------
                | ABC  | ABCDE  | 10 |    | 20 |
                | ABC  | ABCDEF | 20 |    | 30 |
                | DEF  | ABCDEF | 20 |    | 30 |
                | DEF  | ABCDEF | 40 | 50 | 35 |
                | DEF  | ABCDEF | 45 | 50 | 35 |
                | DEFX | ABCDEF | 45 | 50 |    |
                --------------------------------
            */

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_GROUP_BY_2.DB"));

            /*
                -------------------------------------------------------------
                | 1_MAX(C1) | 2_MIN(C3) | 3_COUNT(C4) | 4_SUM(C5) | C4 | C5 |
                -------------------------------------------------------------
                | ABC       | 10        | 0           | 20        |    | 20 |
                | DEF       | 20        | 0           | 60        |    | 30 |
                | DEF       | 40        | 2           | 70        | 50 | 35 |
                | DEFX      | 45        | 1           | 0         | 50 |    |
                -------------------------------------------------------------          
             */
            object o = sql_statements.Parse("SELECT MAX(C1), MIN(C3), COUNT(C4), SUM(C5), C4, C5 FROM A GROUP BY C4, C5");
            using (SelectedData s = o as SelectedData)
            {
                //InteractiveConsole.PrintTable(s);
                int col = 0;
                Check((string)s.selectedColumnNames[col++] == "1_MAX(C1)");
                Check((string)s.selectedColumnNames[col++] == "2_MIN(C3)");
                Check((string)s.selectedColumnNames[col++] == "3_COUNT(C4)");
                Check((string)s.selectedColumnNames[col++] == "4_SUM(C5)");
                Check((string)s.selectedColumnNames[col++] == "C4");
                Check((string)s.selectedColumnNames[col++] == "C5");

                object[] row = s.table.rows[0];
                col = 0;
                Check((string)row[col++] == "ABC");
                Check((double)row[col++] == 10);
                Check((double)row[col++] == 0);
                Check((double)row[col++] == 20);
                Check(row[col++] == null);
                Check((double)row[col++] == 20);

                row = s.table.rows[2];
                col = 0;
                Check((string)row[col++] == "DEF");
                Check((double)row[col++] == 40);
                Check((double)row[col++] == 2);
                Check((double)row[col++] == 70);
                Check((double)row[col++] == 50);
                Check((double)row[col++] == 35);

                row = s.table.rows[3];
                col = 0;
                Check((string)row[col++] == "DEFX");
                Check((double)row[col++] == 45);
                Check((double)row[col++] == 1);
                Check((double)row[col++] == 0);
                Check((double)row[col++] == 50);
                Check(row[col++] == null);
            }
        }

        private void Ut3_orderby()
        {
            /*
                --------------------------------
                | C1   | C2     | C3 | C4 | C5 |
                --------------------------------
                | ABC  | ABCDE  | 10 |    | 20 |
                | ABC  | ABCDEF | 20 |    | 30 |
                | DEF  | ABCDEF | 20 |    | 30 |
                | DEF  | ABCDEF | 40 | 50 | 35 |
                | DEF  | ABCDEF | 45 | 50 | 35 |
                | DEFX | ABCDEF | 45 | 50 |    |
                --------------------------------
            */

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_GROUP_BY_2.DB"));

            /*
                -------------------------------------------------------------
                | 1_MAX(C1) | 2_MIN(C3) | 3_COUNT(C4) | 4_SUM(C5) | C4 | C5 |
                -------------------------------------------------------------
                | DEFX      | 45        | 1           | 0         | 50 |    |
                | DEF       | 40        | 2           | 70        | 50 | 35 |
                | DEF       | 20        | 0           | 60        |    | 30 |
                | ABC       | 10        | 0           | 20        |    | 20 |
                -------------------------------------------------------------       
             */
            object o = sql_statements.Parse("SELECT MAX(C1), MIN(C3), COUNT(C4), SUM(C5), C4, C5 FROM A GROUP BY C4, C5 ORDER BY 1 DESC, C5 DESC");
            using (SelectedData s = o as SelectedData)
            {
                //InteractiveConsole.PrintTable(s);
                int col = 0;

                object[] row = s.table.rows[s.selectedRows[0]];
                Check((string)row[0] == "DEFX");

                row = s.table.rows[s.selectedRows[1]];
                Check((string)row[0] == "DEF");
                Check((double)row[5] == 35);

                row = s.table.rows[s.selectedRows[2]];
                Check((string)row[0] == "DEF");
                Check((double)row[5] == 30);
            }

            o = sql_statements.Parse("SELECT MAX(C1), MIN(C3), COUNT(C4), SUM(C5), C4, C5 FROM A GROUP BY C4, C5 ORDER BY 1 DESC, 6");
            using (SelectedData s = o as SelectedData)
            {
                //InteractiveConsole.PrintTable(s);
                int col = 0;

                object[] row = s.table.rows[s.selectedRows[0]];
                Check((string)row[0] == "DEFX");

                row = s.table.rows[s.selectedRows[1]];
                Check((string)row[0] == "DEF");
                Check((double)row[5] == 30);

                row = s.table.rows[s.selectedRows[2]];
                Check((string)row[0] == "DEF");
                Check((double)row[5] == 35);
            }
        }

        private void Ut4_groupByNone()
        {
            /*
                --------------------------------
                | C1   | C2     | C3 | C4 | C5 |
                --------------------------------
                | ABC  | ABCDE  | 10 |    | 20 |
                | ABC  | ABCDEF | 20 |    | 30 |
                | DEF  | ABCDEF | 20 |    | 30 |
                | DEF  | ABCDEF | 40 | 50 | 35 |
                | DEF  | ABCDEF | 45 | 50 | 35 |
                | DEFX | ABCDEF | 45 | 50 |    |
                --------------------------------
            */

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_GROUP_BY_2.DB"));

            /*
                -----------------------------------------------------------------------------
                | 1_MAX(C1) | 2_MIN(C2) | 3_MIN(C3) | 4_COUNT(C3) | 5_COUNT(C4) | 6_SUM(C5) |
                -----------------------------------------------------------------------------
                | DEFX      | ABCDE     | 10        | 6           | 3           | 150       |
                -----------------------------------------------------------------------------            
             */
            object o = sql_statements.Parse("SELECT MAX(C1), MIN(C2), MIN(C3), COUNT(C3),COUNT(C4), SUM(C5) FROM A ORDER BY 1,2,3,4, 5");
            using (SelectedData s = o as SelectedData)
            {
                //InteractiveConsole.PrintTable(s);
                int col = 0;

                object[] row = s.table.rows[s.selectedRows[0]];
                Check((string)row[0] == "DEFX");
                Check((string)row[1] == "ABCDE");
                Check((double)row[2] == 10);
                Check((double)row[3] == 6);
                Check((double)row[4] == 3);
                Check((double)row[5] == 150);

            }
        }
        
        private void Ut5_where()
        {
            /*
                --------------------------------
                | C1   | C2     | C3 | C4 | C5 |
                --------------------------------
                | ABC  | ABCDE  | 10 |    | 20 |
                | ABC  | ABCDEF | 20 |    | 30 |
                | DEF  | ABCDEF | 20 |    | 30 |
                | DEF  | ABCDEF | 40 | 50 | 35 |
                | DEF  | ABCDEF | 45 | 50 | 35 |
                | DEFX | ABCDEF | 45 | 50 |    |
                --------------------------------
            */

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_GROUP_BY_2.DB"));

            /*
                -------------------------
                | 1_MAX(C1) | 2_SUM(C5) |
                -------------------------
                | DEFX      | 70        |
                -------------------------        
             */
            object o = sql_statements.Parse("SELECT MAX(C1), SUM(C5) FROM A WHERE C4 IS NOT NULL");
            using (SelectedData s = o as SelectedData)
            {
                object[] row = s.table.rows[0];
                Check((string)row[0] == "DEFX");
                Check((double)row[1] == 70);
            }
        }


        public void Ut()
        {
            Ut1();
            Ut2();
            Ut3_orderby();
            Ut4_groupByNone();
            Ut5_where();
        }
    }
}
