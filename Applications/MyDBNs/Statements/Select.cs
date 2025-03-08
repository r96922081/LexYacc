using System.Text;

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

        private static SelectedData GetSelectedData(Table table, List<AggregationColumn> columns, string condition)
        {
            SelectedData s = new SelectedData();

            s.table = table;

            foreach (AggregationColumn column in columns)
            {
                s.columnNames.Add(column.columnName);
                s.userColumnNames.Add(column.userColumnName);
                s.columnIndex.Add(s.table.GetColumnIndex(column.userTableName + "." + column.columnName));
            }

            s.selectedRows = GetSelectedRows(s.table.name, condition);

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

        private static void SortSelectedData(SelectedData s, List<OrderByColumn> orderByColumns)
        {
            if (orderByColumns == null)
                return;

            List<OrderBy> order = ConvertOrder(s, orderByColumns);
            SortRows(s, order);

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

                    ColumnType t = s.table.columns[columnIndex].type;

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

        public static Table JoinTable(TableOrJoins tables)
        {
            Table joined = new Table();
            joined.name = "TempJoinedTable_" + (Gv.sn++);
            joined.originaName = joined.name;

            int columnCount = tables.allTableIds.Select(t => Util.GetTable(t.tableName).columns.Length).Sum();
            joined.columns = new Column[columnCount];

            int index = 0;
            foreach (TableId tableId in tables.allTableIds)
            {
                Table table = Util.GetTable(tableId.tableName);
                foreach (Column column in table.columns)
                {
                    Column newColumn = new Column();
                    newColumn.columnName = column.columnName;
                    newColumn.originalColumnName = column.originalColumnName;
                    newColumn.userColumnName = column.userColumnName;
                    newColumn.userTableName = table.name;
                    newColumn.size = column.size;
                    newColumn.type = column.type;
                    joined.columns[index++] = newColumn;
                }
            }

            int totalRowCount = tables.allTableIds.Select(t => Util.GetTable(t.tableName).rows.Count).Aggregate(1, (a, b) => a * b);
            joined.rows = new List<object[]>();
            for (int i = 0; i < totalRowCount; i++)
            {
                object[] row = new object[columnCount];
                joined.rows.Add(row);
            }

            int columnStartIndex = 0;
            int repeatCount = totalRowCount;
            for (int tableIndex = 0; tableIndex < tables.allTableIds.Count; tableIndex++)
            {
                Table table = Util.GetTable(tables.allTableIds[tableIndex].tableName);
                repeatCount /= table.rows.Count;
                int rowIndex = 0;

                while (rowIndex < totalRowCount)
                {
                    for (int i = 0; i < table.rows.Count; i++)
                    {
                        for (int j = 0; j < repeatCount; j++)
                        {
                            for (int k = 0; k < table.columns.Length; k++)
                            {
                                joined.rows[rowIndex][columnStartIndex + k] = table.rows[i][k];
                            }
                            rowIndex++;
                        }
                    }
                }

                columnStartIndex += table.columns.Length;
            }

            Create.AddTable(joined);

            return joined;
        }


        public static SelectedData SelectRows(List<AggregationColumn> columns, TableOrJoins table, string whereCondition, List<OrderByColumn> orders)
        {
            Table joinedTable = JoinTable(table);

            SelectedData s = GetSelectedData(joinedTable, columns, whereCondition);
            s.userTableName = joinedTable.name;

            SortSelectedData(s, orders);

            return s;
        }

        public static SelectedData SelectRows(List<AggregationColumn> columns, List<string> groupByColumns, TableId tableId, string whereCondition, List<OrderByColumn> orders)
        {
            Table t = Util.GetTable(tableId.tableName);

            SelectedData s = GetSelectedData(t, columns, whereCondition, groupByColumns);

            SortSelectedData(s, orders);

            return s;
        }

        private static string GetGroupKey(object[] row, List<int> groupByColumnIndex)
        {
            StringBuilder sb = new StringBuilder();

            foreach (int index in groupByColumnIndex)
                sb.Append(row[index] + "_");

            return sb.ToString();
        }

        private static string GetTempGroupByColumnName(int columnIndex, AggregationColumn column)
        {
            if (column.op == AggerationOperation.NONE)
                return column.columnName;
            else
                return (columnIndex + 1) + "_" + column.op + "(" + column.columnName + ")";
        }

        private static void CreateTempGroupByTable(Table srcTable, string tempTableName, List<AggregationColumn> columns)
        {
            List<ColumnDeclare> columnDeclares = new List<ColumnDeclare>();
            for (int i = 0; i < columns.Count; i++)
            {
                AggregationColumn aggregrationColumn = columns[i];
                if (aggregrationColumn.op == AggerationOperation.COUNT || aggregrationColumn.op == AggerationOperation.SUM)
                {
                    ColumnDeclare c = new ColumnDeclare();
                    c.columnName = GetTempGroupByColumnName(i, aggregrationColumn);
                    c.type = ColumnType.NUMBER;
                    c.size = -1;
                    columnDeclares.Add(c);
                }
                else if (aggregrationColumn.op == AggerationOperation.MAX || aggregrationColumn.op == AggerationOperation.MIN || aggregrationColumn.op == AggerationOperation.NONE)
                {
                    ColumnDeclare c = new ColumnDeclare();
                    c.columnName = GetTempGroupByColumnName(i, aggregrationColumn);
                    c.type = srcTable.GetColumnType(aggregrationColumn.columnName);
                    c.size = srcTable.GetColumnSize(aggregrationColumn.columnName);
                    columnDeclares.Add(c);
                }
            }

            Create.CreateTable(tempTableName, columnDeclares);
        }

        public static SelectedData GetSelectedData(Table table, List<AggregationColumn> aggregrationColumns, string condition, List<string> groupByColumns)
        {
            List<int> groupByColumnIndex = null;
            if (groupByColumns != null)
                groupByColumnIndex = Util.GetColumnIndexFromName(table, groupByColumns);
            List<int> aggregrationColumnIndex = Util.GetColumnIndexFromName(table, aggregrationColumns.Select(s => s.columnName).ToList());
            SelectedData src = GetSelectedData(table, aggregrationColumns, condition);

            string materializeTableName = "TempTable_" + (Gv.sn++);
            CreateTempGroupByTable(table, materializeTableName, aggregrationColumns);
            Dictionary<string, object[]> groupByRows = new Dictionary<string, object[]>();

            foreach (int rowIndex in src.selectedRows)
            {
                object[] srcRow = table.rows[rowIndex];

                string groupKey = "";

                if (groupByColumns != null)
                    groupKey = GetGroupKey(srcRow, groupByColumnIndex);

                if (!groupByRows.ContainsKey(groupKey))
                {
                    object[] rowToInsert = new object[aggregrationColumns.Count];
                    for (int i = 0; i < aggregrationColumns.Count; i++)
                    {
                        AggregationColumn a = aggregrationColumns[i];

                        if (a.op == AggerationOperation.MAX || a.op == AggerationOperation.MIN || a.op == AggerationOperation.NONE)
                            rowToInsert[i] = srcRow[aggregrationColumnIndex[i]];
                        else if (a.op == AggerationOperation.COUNT)
                        {
                            if (srcRow[aggregrationColumnIndex[i]] == null)
                                rowToInsert[i] = 0d;
                            else
                                rowToInsert[i] = 1d;
                        }
                        else if (a.op == AggerationOperation.SUM)
                        {
                            if (srcRow[aggregrationColumnIndex[i]] == null)
                                rowToInsert[i] = 0d;
                            else
                                rowToInsert[i] = (double)srcRow[aggregrationColumnIndex[i]];
                        }
                    }
                    groupByRows.Add(groupKey, rowToInsert);
                }
                else
                {
                    object[] groupByRow = groupByRows[groupKey];
                    for (int i = 0; i < aggregrationColumns.Count; i++)
                    {
                        AggregationColumn a = aggregrationColumns[i];
                        object groupByValue = groupByRow[i];
                        object srcValue = srcRow[aggregrationColumnIndex[i]];

                        if (a.op == AggerationOperation.NONE)
                            ;
                        else if (a.op == AggerationOperation.MAX)
                        {
                            if (groupByValue == null)
                                groupByRow[i] = srcValue;
                            else if (srcValue == null)
                                ;
                            else if (Util.CompareNonNullColumn(srcValue, groupByValue) > 0)
                                groupByRow[i] = srcValue;
                        }
                        else if (a.op == AggerationOperation.MIN)
                        {
                            if (groupByValue == null)
                                groupByRow[i] = srcValue;
                            else if (srcValue == null)
                                ;
                            else if (Util.CompareNonNullColumn(srcValue, groupByValue) < 0)
                                groupByRow[i] = srcValue;
                        }
                        else if (a.op == AggerationOperation.COUNT)
                        {
                            if (srcValue != null)
                                groupByRow[i] = ((double)groupByRow[i]) + 1;
                        }
                        else if (a.op == AggerationOperation.SUM)
                        {
                            if (srcValue != null)
                                groupByRow[i] = ((double)groupByRow[i]) + (double)srcValue;
                        }
                    }
                }
            }

            Insert.InsertRows(materializeTableName, groupByRows.Values.ToList());

            SelectedData result = new SelectedData();
            result.table = Util.GetTable(materializeTableName);
            result.userTableName = table.name;

            result.columnNames = result.table.columns.Select(s => s.columnName).ToList();
            result.columnIndex = Enumerable.Range(0, aggregrationColumns.Count).ToList();
            result.selectedRows = Enumerable.Range(0, result.table.rows.Count).ToList();
            result.needToDispose = true;

            return result;
        }
    }
}
