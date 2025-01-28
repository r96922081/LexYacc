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

            SqlBooleanExpressionLexYaccCallback.table = Util.GetTable(tableName);
            rows = new List<int>((HashSet<int>)sql_boolean_expression.Parse(condition));
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
                        columnIndex.Add(table.GetColumnIndex(column2));
                    }
                }
                else
                {
                    columnNames.Add(column);
                    columnIndex.Add(table.GetColumnIndex(column));
                }
            }
        }



        private static SelectedData GetSelectedData(Table table, List<string> columnInput, string condition)
        {
            SelectedData s = new SelectedData();

            s.table = table;
            GetColumns(table, columnInput, s.columnNames, s.columnIndex);
            s.selectedRows = GetSelectedRows(table.tableName, condition);

            return s;
        }



        private static List<OrderBy> ConvertOrder(SelectedData s, List<OrderByColumn> orders)
        {
            List<OrderBy> orders2 = new List<OrderBy>();
            foreach (OrderByColumn order in orders)
            {
                OrderByDirection ascending = order.direction;

                if (order.column is string)
                {
                    for (int i = 0; i < s.columnNames.Count; i++)
                    {
                        if (s.columnNames[i].ToUpper() == ((string)order.column).ToUpper())
                        {
                            OrderBy orderBy = new OrderBy();
                            orderBy.op = ascending;
                            orderBy.selectColumnIndex = (int)i;
                            orders2.Add(orderBy);
                            break;
                        }
                    }
                }
                else
                {
                    OrderBy orderBy = new OrderBy();
                    orderBy.op = ascending;
                    orderBy.selectColumnIndex = (int)order.column - 1;
                    orders2.Add(orderBy);
                }
            }

            return orders2;
        }

        private static void SortRows(SelectedData s, List<OrderBy> order2)
        {
            s.selectedRows.Sort((lIndex, rIndex) =>
            {
                object[] lhsColumns = s.table.rows[lIndex];
                object[] rhsColumns = s.table.rows[rIndex];

                foreach (OrderBy o in order2)
                {
                    int columnIndex = s.columnIndex[o.selectColumnIndex];
                    object lhsValue = lhsColumns[columnIndex];
                    object rhsValue = rhsColumns[columnIndex];

                    if (lhsValue == null && rhsValue == null)
                        continue;

                    if (lhsValue == null && rhsValue != null)
                        return o.op == OrderByDirection.ASEC ? -1 : 1;

                    if (lhsValue != null && rhsValue == null)
                        return o.op == OrderByDirection.ASEC ? 1 : -1;

                    ColumnType t = s.table.columnTypes[columnIndex];

                    IComparable lCompara = (IComparable)lhsValue;
                    IComparable rCompara = (IComparable)rhsValue;

                    int result = (o.op == OrderByDirection.ASEC ? 1 : -1) * lCompara.CompareTo(rCompara);
                    if (result == 0)
                        continue;

                    return result;
                }

                return 0;
            });
        }

        public static SelectedData SelectRows(List<string> columnInput, string tableName, string whereCondition, List<OrderByColumn> orders)
        {
            Table table = Util.GetTable(tableName);

            SelectedData s = GetSelectedData(table, columnInput, whereCondition);

            if (orders != null)
            {
                List<OrderBy> order2 = ConvertOrder(s, orders);
                SortRows(s, order2);
            }

            return s;
        }


        public static SelectedData SelectRows(List<AggregationColumn> columns, string tableName, string whereCondition, List<string> groupByColumns, List<OrderByColumn> orderByColumns)
        {
            Table table = Util.GetTable(tableName);

            List<string> columns2 = columns.Select(column => column.columnName).ToList();

            SelectedData s = GetSelectedData(table, columns2, whereCondition);
            string tempTableName = "TempTable_" + (Gv.sn++);

            List<ColumnDeclare> columnDeclares = new List<ColumnDeclare>();
            for (int i = 0; i < columns.Count; i++)
            {
                AggregationColumn column = columns[i];
                if (column.op == AggerationOperation.COUNT || column.op == AggerationOperation.SUM)
                {
                    ColumnDeclare c = new ColumnDeclare();
                    c.columnName = column.columnName;
                    c.type = ColumnType.NUMBER;
                    c.size = -1;
                    columnDeclares.Add(c);
                }
                else
                {
                    ColumnDeclare c = new ColumnDeclare();
                    c.columnName = column.columnName;
                    c.type = table.GetColumnType(column.columnName);
                    c.size = table.GetColumnSize(column.columnName);
                    columnDeclares.Add(c);
                }
            }

            Create.CreateTable(tempTableName, columnDeclares);
            List<string> row = new List<string>();

            foreach (AggregationColumn column in columns)
            {

            }

            //Insert.InsertRows(tempTableName, null);

            return null;
        }
    }
}
