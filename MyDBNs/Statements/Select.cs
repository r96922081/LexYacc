namespace MyDBNs
{
    public class Select
    {
        private static List<int> GetSelectedRows(string tableName, string condition)
        {
            List<int> rows = new List<int>();
            if (condition == null)
            {
                Table table = Util.GetTable(tableName);
                rows = new List<int>();
                for (int i = 0; i < table.rows.Count; i++)
                    rows.Add(i);

                return rows;
            }

#if !MarkUserOfSqlCodeGen
            SqlBooleanExpressionLexYaccCallback.table = Util.GetTable(tableName);
            rows = new List<int>((HashSet<int>)sql_boolean_expression.Parse(condition));
#endif

            rows.Sort();
            return rows;
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
            for (int i = 0; i < s.selectedRows.Count; i++)
            {
                object[] row = s.table.rows[s.selectedRows[i]];
                System.Console.Write("| ");
                int k = 0;
                foreach (int j in s.columnIndex)
                {
                    if (row[j] == null)
                        System.Console.Write("".PadRight(columnWidths[k]));
                    else
                        System.Console.Write(row[j].ToString().PadRight(columnWidths[k]));
                    k++;
                    System.Console.Write(" | ");
                }
                System.Console.WriteLine();
            }
        }

        private static List<OrderBy> ConvertOrder(SelectedData s, List<List<object>> orders)
        {
            List<OrderBy> orders2 = new List<OrderBy>();
            foreach (List<object> order in orders)
            {
                bool ascending = (bool)order[1];

                if (order[0] is string)
                {
                    for (int i = 0; i < s.columnNames.Count; i++)
                    {
                        if (s.columnNames[i].ToUpper() == ((string)order[0]).ToUpper())
                        {
                            OrderBy orderBy = new OrderBy();
                            orderBy.ascending = ascending;
                            orderBy.selectColumnIndex = (int)i;
                            orders2.Add(orderBy);
                            break;
                        }
                    }
                }
                else
                {
                    OrderBy orderBy = new OrderBy();
                    orderBy.ascending = ascending;
                    orderBy.selectColumnIndex = (int)order[0] - 1;
                    orders2.Add(orderBy);
                }
            }

            return orders2;
        }

        private static void SortRows(SelectedData s, List<OrderBy> order2)
        {
            s.selectedRows.Sort((lIndex, rIndex) =>
            {
                object[] l = s.table.rows[lIndex];
                object[] r = s.table.rows[rIndex];

                foreach (OrderBy o in order2)
                {
                    if (l == null && r == null)
                        continue;

                    if (l == null && r != null)
                        return o.ascending ? -1 : 1;

                    if (l != null && r == null)
                        return o.ascending ? 1 : -1;

                    ColumnType t = s.table.columnTypes[s.columnIndex[o.selectColumnIndex]];

                    IComparable lCompara = (IComparable)l[s.columnIndex[o.selectColumnIndex]];
                    IComparable rCompara = (IComparable)r[s.columnIndex[o.selectColumnIndex]];

                    int result = (o.ascending ? 1 : -1) * lCompara.CompareTo(rCompara);
                    if (result == 0)
                        continue;

                    return result;
                }

                return 0;
            });
        }

        public static void SelectRows(List<string> columnInput, string tableName, string whereCondition, List<List<object>> orders)
        {
            tableName = tableName.ToUpper();

#if !MarkUserOfSqlCodeGen

            Table table = Util.GetTable(tableName);

            SelectedData s = GetSelectedData(table, columnInput, whereCondition);

            if (orders != null)
            {
                List<OrderBy> order2 = ConvertOrder(s, orders);
                SortRows(s, order2);
            }


            PrintTable(s);
#endif
        }
    }
}
