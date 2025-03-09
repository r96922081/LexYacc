﻿using System.Text;

namespace MyDBNs
{
    public class Show
    {
        public static void ShowTables()
        {
            foreach (Table t in Gv.db.tables)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("table: " + t.originaName);
                for (int i = 0; i < t.columns.Length; i++)
                {
                    sb.Append(t.columns[i].originalColumnName + " " + t.columns[i].type);
                    if (t.columns[i].type == ColumnType.VARCHAR)
                    {
                        sb.Append("(" + t.columns[i].size + ")");
                    }

                    if (i != t.columns.Length - 1)
                        sb.AppendLine(",");
                    else
                        sb.AppendLine();
                }

                System.Console.WriteLine(sb.ToString());
            }
        }
    }
}
