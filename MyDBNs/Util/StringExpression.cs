﻿namespace MyDBNs
{
    public class StringExpression
    {
        public static List<string> Parse(Table table, string expression)
        {
            List<string> rows = new List<string>();
            for (int i = 0; i < table.rows.Count; i++)
                rows.Add(null);


            string[] string_id_array = expression.Split("||");
            foreach (string string_id2 in string_id_array)
            {
                string string_id = string_id2.Trim();

                for (int i = 0; i < table.rows.Count; i++)
                {
                    string value = null;

                    if (Util.GetStringType(string_id) == StringType.String)
                        value = Util.ExtractStringFromSingleQuote(string_id);
                    else
                    {
                        int columnIndex = table.GetColumnIndex(string_id);
                        value = (string)table.rows[i][columnIndex];
                    }

                    if (rows[i] == null)
                        rows[i] = value;
                    else
                        rows[i] += value;
                }
            }

            return rows;
        }
    }
}