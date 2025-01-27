namespace MyDBNs
{
    public class ArithmeticExpressionUt : BaseUt
    {
        public void Ut()
        {
            /*
            | C1 | C2 | C3  |
            | 1  | 10 | 100 |
            | 2  | 20 | 200 |
            | 3  | 5  |     |
            | 100| 10 | 1   |
            */

            sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_ARITHMETIC_EXPRESSION.DB"));


            List<object[]> rows = RunSelectStatementAndConvertResult("SELECT * FROM A WHERE C3 = 90 + 40 - 6 * 4 - 6");
            Check(rows.Count == 1);

            rows = RunSelectStatementAndConvertResult("SELECT * FROM A WHERE C2 = 5 + c3");
            Check(rows.Count == 1);

            rows = RunSelectStatementAndConvertResult("SELECT * FROM A WHERE C3 IS NULL");
            Check(rows.Count == 1);

            rows = RunSelectStatementAndConvertResult("SELECT * FROM A WHERE C3/C1 = 100");
            Check(rows.Count == 2);

            // NUMBER +-*/ NULL = NUMBER

            rows = RunSelectStatementAndConvertResult("SELECT * FROM A WHERE 7 = 7 + c3");
            Check(rows.Count == 1);

            rows = RunSelectStatementAndConvertResult("SELECT * FROM A WHERE 7 = 7 - C3");
            Check(rows.Count == 1);

            rows = RunSelectStatementAndConvertResult("SELECT * FROM A WHERE 7 = 7 / c3 AND C3 != 1");
            Check(rows.Count == 1);

            rows = RunSelectStatementAndConvertResult("SELECT * FROM A WHERE 7 = 7 * c3 AND C3 != 1");
            Check(rows.Count == 1);



            // NULL +- NUMBER = NUMBER
            // NULL */ NUMBER = 0

            rows = RunSelectStatementAndConvertResult("SELECT * FROM A WHERE 2 = C3 + 2");
            Check(rows.Count == 1);

            rows = RunSelectStatementAndConvertResult("SELECT * FROM A WHERE 2 = C3 - 2");
            Check(rows.Count == 1);

            rows = RunSelectStatementAndConvertResult("SELECT * FROM A WHERE 0 = C3 * 2");
            Check(rows.Count == 1);

            rows = RunSelectStatementAndConvertResult("SELECT * FROM A WHERE 0 = C3 / 2");
            Check(rows.Count == 1);



        }
    }
}
