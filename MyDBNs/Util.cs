namespace MyDBNs
{
    public class Util
    {
        public static Table GetTable(string tableName)
        {
            return DB.tables.FirstOrDefault(t => t.tableName.ToUpper() == tableName.ToUpper());
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

        public static string GetString(string s)
        {
            // remove ' ' 
            return s.Substring(1, s.Length - 2);
        }
    }
}
