namespace MyDBNs
{
    public class Util
    {
        public static void DeleteAllTable()
        {
            Gv.db.tables.Clear();
        }

        public static Table GetTable(string tableName)
        {
            return Gv.db.tables.FirstOrDefault(t => t.name.ToUpper() == tableName.ToUpper());
        }

        public static List<Table> GetTables()
        {
            return Gv.db.tables;
        }

        public static List<object[]> GetSelectRows(SelectedData s)
        {
            List<object[]> rows = new List<object[]>();

            Table t = s.table;
            for (int i = 0; i < s.selectedRows.Count; i++)
            {
                object[] selectedRow = t.rows[s.selectedRows[i]];

                object[] row = new object[s.columnIndex.Count];
                for (int j = 0; j < s.columnIndex.Count; j++)
                    row[j] = selectedRow[s.columnIndex[j]];

                rows.Add(row);
            }

            return rows;
        }

        public static StringType GetStringType(string s)
        {
            if (s.StartsWith("'") && s.EndsWith("'"))
                return StringType.String;

            double n = 0;
            bool ret = double.TryParse(s, out n);

            if (ret)
                return StringType.Number;

            return StringType.Column;
        }

        public static double GetNumber(string s)
        {
            return double.Parse(s);
        }

        public static string ExtractStringFromSingleQuote(string s)
        {
            // remove ' ' 
            return s.Substring(1, s.Length - 2);
        }

        public static List<int> GetColumnIndexFromName(Table t, List<string> names)
        {
            List<int> ret = new List<int>();
            foreach (string name in names)
                ret.Add(t.GetColumnIndex(name));

            return ret;
        }

        public static int CompareNonNullColumn(object column1, object column2)
        {
            if (column1 == null || column2 == null)
                throw new Exception();

            if (column1 is double)
                return ((double)column1).CompareTo((double)column2);
            else
                return ((string)column1).CompareTo((string)column2);
        }

        public static bool IsValid(string s)
        {
            if (s == null)
                return false;

            if (s.Length == 0)
                return false;

            return true;
        }
    }
}
