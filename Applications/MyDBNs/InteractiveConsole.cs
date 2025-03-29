namespace MyDBNs
{
    public class InteractiveConsole
    {
        private static int[] GetDisplayColumnWidth(Table table, List<string> columnNames, List<string> customColumnNames, List<int> columnIndex)
        {
            int[] columnWidths = new int[columnNames.Count];
            for (int i = 0; i < columnIndex.Count; i++)
            {
                if (customColumnNames[i] != null)
                    columnWidths[i] = customColumnNames[i].Length;
                else
                    columnWidths[i] = columnNames[i].Length;
            }

            // get column width
            foreach (object[] row in table.rows)
            {
                for (int i = 0; i < columnIndex.Count; i++)
                {
                    if (row[columnIndex[i]] == null)
                        continue;

                    int cellLength = row[columnIndex[i]].ToString().Length;
                    if (cellLength > columnWidths[i])
                    {
                        columnWidths[i] = cellLength;
                    }
                }
            }

            return columnWidths;
        }

        private static void PrintSeprator(int[] columnWidths)
        {
            // separator --------------
            Console.Write("--");
            for (int i = 0; i < columnWidths.Length; i++)
            {
                for (int j = 0; j < columnWidths[i]; j++)
                    Console.Write("-");

                if (i == columnWidths.Length - 1)
                    Console.Write("--");
                else
                    Console.Write("---");
            }
            Console.WriteLine();
        }

        public static void PrintTable(SelectedData s)
        {
            int[] columnWidths = GetDisplayColumnWidth(s.table, s.selectedColumnNames, s.customColumnNames, s.selectedColumnIndex);

            Console.WriteLine();
            PrintSeprator(columnWidths);

            // show column name
            Console.Write("| ");
            for (int i = 0; i < s.customColumnNames.Count; i++)
            {
                string columnName = s.customColumnNames[i];
                if (columnName == null)
                    columnName = s.selectedColumnNames[i];

                Console.Write(columnName.PadRight(columnWidths[i]));
                if (i == columnWidths.Length - 1)
                    Console.Write(" |");
                else
                    Console.Write(" | ");
            }
            Console.WriteLine();

            PrintSeprator(columnWidths);

            // show cell
            for (int i = 0; i < s.selectedRows.Count; i++)
            {
                object[] row = s.table.rows[s.selectedRows[i]];
                Console.Write("| ");
                int k = 0;
                foreach (int j in s.selectedColumnIndex)
                {
                    if (row[j] == null)
                        Console.Write("".PadRight(columnWidths[k]));
                    else
                        Console.Write(row[j].ToString().PadRight(columnWidths[k]));
                    k++;
                    Console.Write(" | ");
                }
                Console.WriteLine();
            }

            PrintSeprator(columnWidths);
        }

        private static void PrintResult(string input, object result)
        {

            if (input.ToUpper().Trim().StartsWith("SELECT"))
            {
                SelectedData s = (SelectedData)result;
                using (s)
                {
                    PrintTable(s);
                }
            }
            else if (input.ToUpper().Trim().StartsWith("INSERT"))
            {
                int count = (int)result;
                Console.WriteLine(count + " row(s) inserted");
            }
            else if (input.ToUpper().Trim().StartsWith("DELETE"))
            {
                int count = (int)result;
                Console.WriteLine(count + " row(s) deleted");
            }
            else if (input.ToUpper().Trim().StartsWith("UPDATE"))
            {
                int count = (int)result;
                Console.WriteLine(count + " row(s) updated");
            }
            else if (input.ToUpper().Trim().StartsWith("COMMIT"))
            {
                int count = (int)result;
                Console.WriteLine(count + " operation(s) commited");
            }
            else if (input.ToUpper().Trim().StartsWith("ROLLBACK"))
            {
                int count = (int)result;
                Console.WriteLine(count + " operation(s) rolled back");
            }
            else
            {
                System.Console.WriteLine(result.ToString());
            }
        }

        public static void Interactive()
        {
            sql_statements.Parse("show tables");


            System.Console.Write("\ninput sql: ");
            string input;
            while ((input = System.Console.ReadLine()) != null)
            {
                object result = null;

                try
                {
                    try
                    {
                        result = sql_statements.Parse(input);
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine("Error occurred");
                        continue;
                    }

                    if (result is string && ((string)result).ToLower() == "syntax error")
                    {
                        System.Console.WriteLine("syntax error");
                        continue;
                    }

                    PrintResult(input, result);
                }
                finally
                {
                    System.Console.Write("\ninput sql: ");
                }
            }
        }
    }
}
