using System.Text;

namespace MyDBNs
{
    public class Show
    {
        public static void ShowTables()
        {
            foreach (Table t in DB.tables)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("table: " + t.originalTableName);
                for (int i = 0; i < t.originalColumnNames.Length; i++)
                {
                    sb.Append(t.originalColumnNames[i] + " " + t.columnTypes[i]);
                    if (t.columnTypes[i] == ColumnType.VARCHAR)
                    {
                        sb.Append("(" + t.columnSizes[i] + ")");
                    }

                    if (i != t.originalColumnNames.Length - 1)
                        sb.AppendLine(",");
                    else
                        sb.AppendLine();
                }

                System.Console.WriteLine(sb.ToString());
            }
        }
    }
}
