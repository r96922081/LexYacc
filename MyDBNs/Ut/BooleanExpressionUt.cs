﻿namespace MyDBNs
{
    public class BooleanExpressionUt : Ut
    {
        public void Ut()
        {
            Util.DeleteAllTable();

            /*
            | C1  | C2 |
            | ABC | 11 |
            | def |    |
            |     | 22 |
            | GG  | 33 |
            | EE  | 55 |
            |     | 66 |
            | G   |    |
             */

            sql_statements.Parse("CREATE TABLE A ( C1 VARCHAR(123), C2 NUMBER)");
            sql_statements.Parse("INSERT INTO A ( C1, C2 ) VALUES ( 'ABC', 11 )");
            sql_statements.Parse("INSERT INTO A ( C1 ) VALUES ( 'def' )");
            sql_statements.Parse("INSERT INTO A ( C2 ) VALUES ( 22 )");
            sql_statements.Parse("INSERT INTO A VALUES ( 'GG', 33 )");
            sql_statements.Parse("INSERT INTO A ( C2, C1 ) VALUES ( 55, 'EE' )");
            sql_statements.Parse("INSERT INTO A VALUES ( null, 66 )");
            sql_statements.Parse("INSERT INTO A VALUES ( 'G', null )");

            Table t = Util.GetTable("A");

            SqlBooleanExpressionLexYaccCallback.table = t;

            List<object[]> rows = (List<object[]>)sql_statements.Parse("SELECT * FROM A WHERE C1 IS NULL");
            Check(rows.Count == 2);

            rows = (List<object[]>)sql_statements.Parse("SELECT * FROM A WHERE C2 IS NULL and c1 = 'G'");
            Check(rows.Count == 1);

            rows = (List<object[]>)sql_statements.Parse("SELECT * FROM A WHERE C2 = 11 * 2 + 100 - 10 * 10");
            Check(rows.Count == 1);

            rows = (List<object[]>)sql_statements.Parse("SELECT * FROM A WHERE C2 = C2 + 2 - 2");
            Check(rows.Count == 7);

            rows = (List<object[]>)sql_statements.Parse("SELECT * FROM A WHERE C1 = 'A' || 'B' || 'C' AND C2 = 11");
            Check(rows.Count == 1);

            rows = (List<object[]>)sql_statements.Parse("SELECT * FROM A WHERE C1 = 'A' || 'B' || 'C' AND C2 IS NULL");
            Check(rows.Count == 0);

            rows = (List<object[]>)sql_statements.Parse("SELECT * FROM A WHERE C1 = 'A' || 'B' || 'C' OR C2 IS NULL");
            Check(rows.Count == 3);

            rows = (List<object[]>)sql_statements.Parse("SELECT * FROM A WHERE C2 != 11");
            Check(rows.Count == 6);
        }
    }
}
