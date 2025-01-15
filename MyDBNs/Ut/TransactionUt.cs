namespace MyDBNs
{
    public class TransactionUt : BaseUt
    {


        public void InsertUt()
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
            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_TRANSACTION.DB"));

            sql_statements.Parse("TRANSACTION START");
            sql_statements.Parse("INSERT INTO A VALUES (NULL, NULL, 1, 1)");
            List<object[]> rows = RunSelectStatementAndConvertResult("SELECT * FROM A");
            Check(rows.Count == 5);
            int count = (int)sql_statements.Parse("COMMIT");
            Check(count == 1);

            sql_statements.Parse("TRANSACTION START");
            sql_statements.Parse("INSERT INTO A VALUES (NULL, NULL, 2, 2)");
            rows = RunSelectStatementAndConvertResult("SELECT * FROM A");
            Check(rows.Count == 6);
            count = (int)sql_statements.Parse("ROLLBACK");
            Check(count == 1);
            rows = RunSelectStatementAndConvertResult("SELECT * FROM A");
            Check(rows.Count == 5);
        }


        public void CompositeUt()
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
            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_TRANSACTION.DB"));

            sql_statements.Parse("TRANSACTION START");
            sql_statements.Parse("DELETE FROM A WHERE C3 < 30");
            sql_statements.Parse("INSERT INTO A VALUES (NULL, NULL, 2, 2)");
            List<object[]> rows = RunSelectStatementAndConvertResult("SELECT * FROM A");
            Check(rows.Count == 2);

            sql_statements.Parse("ROLLBACK");
            rows = RunSelectStatementAndConvertResult("SELECT * FROM A");
            Check(rows.Count == 4);
        }

        public void DeleteUt()
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
            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_TRANSACTION.DB"));

            sql_statements.Parse("TRANSACTION START");
            sql_statements.Parse("DELETE FROM A WHERE C3 < 30");
            List<object[]> rows = RunSelectStatementAndConvertResult("SELECT * FROM A");
            Check(rows.Count == 1);

            sql_statements.Parse("ROLLBACK");
            rows = RunSelectStatementAndConvertResult("SELECT * FROM A");
            Check(rows.Count == 4);
        }

        public void UpdateUt()
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
            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_TRANSACTION.DB"));

            sql_statements.Parse("TRANSACTION START");
            sql_statements.Parse("UPDATE A SET C1 = 'A'");
            sql_statements.Parse("UPDATE A SET C3 = 9");
            sql_statements.Parse("UPDATE A SET C1 = 'B'");

            List<object[]> rows = RunSelectStatementAndConvertResult("SELECT * FROM A");
            Check((string)rows[0][0] == "B");
            Check((double)rows[0][2] == 9);

            sql_statements.Parse("ROLLBACK");
            rows = RunSelectStatementAndConvertResult("SELECT * FROM A");
            Check((string)rows[0][0] == "ABC");
            Check((double)rows[0][2] == 11);
        }

        public void BasicUt()
        {
            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_TRANSACTION.DB"));
            CheckOk(sql_statements.Parse("TRANSACTION START"));
            CheckOk(sql_statements.Parse("COMMIT"));
            CheckOk(sql_statements.Parse("ROLLBACK"));
        }

        public void Ut()
        {
            BasicUt();
            InsertUt();
            DeleteUt();
            UpdateUt();

            CompositeUt();
        }
    }
}
