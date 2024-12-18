namespace MyDBNs
{
    public class Select
    {
        private static HashSet<int> GetSelectedRows(string tableName, string condition)
        {
            if (condition == null)
                return null;

            SqlConditionLexYaccCallback.tableName = tableName;
            return (HashSet<int>)sql_condition_lexyacc.Parse(condition);
        }

        private static void GetColumns(Table table, List<string> columns, List<string> columnNames, List<int> columnIndex)
        {
            columnNames.Clear();
            columnIndex.Clear();

            foreach (string column in columns)
            {
                if (column == "*")
                {
                    foreach (string column2 in table.columnNames)
                    {
                        columnNames.Add(column2);
                        columnIndex.Add(table.columnNameToIndexMap[column2]);
                    }
                }
                else
                {
                    columnNames.Add(column);
                    columnIndex.Add(table.columnNameToIndexMap[column]);
                }
            }
        }

        private static int[] GetDisplayColumnWidth(Table table, List<string> columnNames, List<int> columnIndex)
        {
            int[] columnWidths = new int[columnNames.Count];
            for (int i = 0; i < columnIndex.Count; i++)
            {
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

        private static SelectedData GetSelectedData(Table table, List<string> columnInput, string condition)
        {
            SelectedData s = new SelectedData();

            s.table = table;
            GetColumns(table, columnInput, s.columnNames, s.columnIndex);
            s.selectedRows = GetSelectedRows(table.tableName, condition);

            return s;
        }

        private static void PrintTable(SelectedData s)
        {
            int[] columnWidths = GetDisplayColumnWidth(s.table, s.columnNames, s.columnIndex);

            // show column name
            System.Console.Write("| ");
            for (int i = 0; i < s.columnNames.Count; i++)
            {
                System.Console.Write(s.columnNames[i].PadRight(columnWidths[i]));
                System.Console.Write(" | ");
            }
            System.Console.WriteLine();

            // show cell
            for (int rowIndex = 0; rowIndex < s.table.rows.Count; rowIndex++)
            {
                if (s.selectedRows != null && !s.selectedRows.Contains(rowIndex))
                    continue;


                object[] row = s.table.rows[rowIndex];
                System.Console.Write("| ");
                int j = 0;
                foreach (int i in s.columnIndex)
                {
                    if (row[i] == null)
                        System.Console.Write("".PadRight(columnWidths[j]));
                    else
                        System.Console.Write(row[i].ToString().PadRight(columnWidths[j]));
                    j++;
                    System.Console.Write(" | ");
                }
                System.Console.WriteLine();
            }
        }

        public static void SelectRows(List<string> columnInput, string tableName, string whereCondition, List<List<object>> orders)
        {
            tableName = tableName.ToUpper();

#if !MarkUserOfSqlCodeGen

            Table table = Util.GetTable(tableName);

            SelectedData s = GetSelectedData(table, columnInput, whereCondition);
            PrintTable(s);
#endif
        }
    }
}
