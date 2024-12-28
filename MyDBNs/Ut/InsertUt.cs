﻿namespace MyDBNs
{
    public class InsertUt : Ut
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
            CheckOk(sql_statements.Parse("CREATE TABLE A ( C1 VARCHAR(123), C2 NUMBER)"));
            CheckOk(sql_statements.Parse("INSERT INTO A ( C1, C2 ) VALUES ( 'ABC', 11 )"));
            CheckOk(sql_statements.Parse("INSERT INTO A ( C1 ) VALUES ( 'def' )"));
            CheckOk(sql_statements.Parse("INSERT INTO A ( C2 ) VALUES ( 22 )"));
            CheckOk(sql_statements.Parse("INSERT INTO A VALUES ( 'GG', 33 )"));
            CheckOk(sql_statements.Parse("INSERT INTO A ( C2, C1 ) VALUES ( 55, 'EE' )"));
            CheckOk(sql_statements.Parse("INSERT INTO A VALUES ( null, 66 )"));
            CheckOk(sql_statements.Parse("INSERT INTO A VALUES ( 'G', null )"));
            Table t = Util.GetTable("A");
            Check(t.rows.Count == 7);
            Check((string)t.rows[1][0] == "def");
            Check(t.rows[1][1] == null);
            Check((string)t.rows[3][0] == "GG");
            Check((double)t.rows[3][1] == 33);
            Check((string)t.rows[4][0] == "EE");
            Check((double)t.rows[4][1] == 55);
            Check(t.rows[5][0] == null);
            Check(t.rows[6][1] == null);

            CheckErrorOrException(() => { return sql_statements.Parse("INSERT INTO A ( C1 ) VALUES ( 1234 )"); });
            CheckErrorOrException(() => { return sql_statements.Parse("INSERT INTO A ( C2 ) VALUES ( 'ABC' )"); });
            CheckErrorOrException(() => { return sql_statements.Parse("INSERT INTO A ( C3 ) VALUES ( 1234 )"); });
            CheckErrorOrException(() => { return sql_statements.Parse("INSERT INTO A ( C1, C2, C3 ) VALUES ( 'ABC', 11, 22 )"); });
            CheckErrorOrException(() => { return sql_statements.Parse("INSERT INTO B VALUES ( 1234 )"); });
        }
    }
}