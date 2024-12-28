namespace MyDBNs
{
    public class Verifier
    {
        public static void VerifyCreateTable(string name, List<(string, string)> columnDeclare)
        {
            for (int i = 0; i < columnDeclare.Count; i++)
            {
                string columnType = columnDeclare[i].Item2;
                if (columnType.StartsWith("VARCHAR"))
                {
                    int left = columnType.IndexOf('(');
                    int right = columnType.LastIndexOf(')');

                    int lengthInt;
                    string length = columnType.Substring(left + 1, right - left - 2);
                    if (!int.TryParse(length, out lengthInt))
                        throw new Exception("Invalid VARCHAR length = " + lengthInt);
                }
            }
        }
    }
}
