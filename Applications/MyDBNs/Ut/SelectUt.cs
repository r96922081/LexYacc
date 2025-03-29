namespace MyDBNs
{
    public class SelectUt : BaseUt
    {
        public void Basic()
        {
            List<object[]> rows = RunSelectStatementAndConvertResult("SELECT * FROM A");
            Check(rows.Count == 5);

            rows = RunSelectStatementAndConvertResult("SELECT C2 FROM A");
            int columnCount = rows[0].Length;
            Check(columnCount == 1);
            Check((double)rows[4][0] == 5);

            rows = RunSelectStatementAndConvertResult("SELECT c1 FROM A");
            Check(rows[4][0] == null);

            rows = RunSelectStatementAndConvertResult("SELECT *, C1, C2 FROM A");
            columnCount = rows[0].Length;
            Check(columnCount == 4);
            Check((double)rows[4][3] == 5);
        }

        public void OrderBy()
        {
            List<object[]> rows = RunSelectStatementAndConvertResult("SELECT C1 FROM A ORDER BY C1 DESC");
            Check((string)rows[0][0] == "D");
            Check(rows[4][0] == null);

            rows = RunSelectStatementAndConvertResult("SELECT * FROM A ORDER BY C1 desc, 2 desc");
            Check((double)rows[1][1] == 3);
            Check((double)rows[2][1] == 2);

            rows = RunSelectStatementAndConvertResult("SELECT * FROM A ORDER BY C1, C2, 1,2,1,2");

            CheckSyntaxErrorOrException(() => { return sql_statements.Parse("SELECT C1 FROM A ORDER BY 2"); });
        }

        public void TableName()
        {
            object o = sql_statements.Parse("SELECT A.C1, A.C2, C2, A.*, * FROM A");
            using (SelectedData s = o as SelectedData)
            {
                Check(s.selectedColumnNames.Count == 7);
            }

            o = sql_statements.Parse("SELECT AA.C1, AA.C2, C2, AA.*, * FROM A AS AA");
            using (SelectedData s = o as SelectedData)
            {
                Check(s.selectedColumnNames.Count == 7);
            }

            CheckSyntaxErrorOrException(() => { return sql_statements.Parse("SELECT AA.C1 FROM A"); });
        }

        public void ColumnName()
        {
            SelectedData ret = (SelectedData)sql_statements.Parse("SELECT C1 AS C1_ALIAS, C2 C2_ALIAS FROM A");

            Check(ret.customColumnNames[0] == "C1_ALIAS");
            Check(ret.customColumnNames[1] == "C2_ALIAS");
        }

        public void Ut()
        {
            Util.DeleteAllTable();

            sql_statements.Parse("CREATE TABLE A ( C1 VARCHAR(123), C2 NUMBER)");
            sql_statements.Parse("INSERT INTO A VALUES ( 'A', 1 )");
            sql_statements.Parse("INSERT INTO A VALUES ( 'C', 2 )");
            sql_statements.Parse("INSERT INTO A VALUES ( 'C', 3 )");
            sql_statements.Parse("INSERT INTO A VALUES ( 'D', null )");
            sql_statements.Parse("INSERT INTO A VALUES ( null, 5 )");

            Basic();
            OrderBy();
            TableName();
            ColumnName();
        }
    }
}
