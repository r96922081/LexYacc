﻿using System.Diagnostics;

namespace SqlNs
{

    public class SqlUt
    {
        public static void Check(bool b)
        {
            if (!b)
                Trace.Assert(false);
        }


        public static void AdhocUt()
        {
            object ret = sql_lexyacc.Parse("CREATE TABLE A ( NAME VARCHAR(123), AGE NUMBER)");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("CREATE TABLE A2 ( AAA VARCHAR(456), BBB NUMBER)");
            Check(ret == null || ret.ToString() == "");


            ret = sql_lexyacc.Parse("INSERT INTO A VALUES ( 'DEF', 33  )");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("INSERT INTO A ( NAME, AGE ) VALUES ( 'DEF', 33  )");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("INSERT INTO A VALUES ( 44, 55  )");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("INSERT INTO A ( AGE, NAME ) VALUES ( 66, 'ABC'  )");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("INSERT INTO A ( AGE ) VALUES ( 999)");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("SHOW TABLES");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("DELETE FROM A WHERE NAME = 'ABC' OR AGE = 55 ");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("SELECT * FROM A WHERE 1 = 1");
            Check(ret == null || ret.ToString() == "");

            ret = sql_lexyacc.Parse("SELECT AGE, NAME, * FROM A WHERE 1 = 1");
            Check(ret == null || ret.ToString() == "");
        }

        public static void Ut()
        {
            AdhocUt();
            PlayGround.Play();
        }

    }
}
