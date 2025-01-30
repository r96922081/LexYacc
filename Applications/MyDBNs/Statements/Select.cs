using System.Collections.Generic;
using System.Data.Common;
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

        private static void GetColumns(Table table, List<AggregationColumn> columns, List<string> columnNames, List<int> columnIndex)
        {
            columnNames.Clear();
            columnIndex.Clear();

            foreach (AggregationColumn column in columns)
            {
                if (column.column == "*")
                {
                    foreach (string column2 in table.columnNames)
                    {
                        columnNames.Add(column2);
                        columnIndex.Add(table.GetColumnIndex(column2));
                    }
                }
                else
                {
                    columnNames.Add(column.column);
                    columnIndex.Add(table.GetColumnIndex(column.column));
                }
            }
        }



        private static SelectedData GetSelectedData(Table table, List<AggregationColumn> columns, string condition)
        {
            SelectedData s = new SelectedData();

            s.table = table;
            GetColumns(table, columns, s.columnNames, s.columnIndex);
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

        public static SelectedData SelectRows(List<AggregationColumn> columns, TableId tableId, string whereCondition, List<OrderByColumn> orders)
        {
            Table table = Util.GetTable(tableId.tableName);

            SelectedData s = GetSelectedData(table, columns, whereCondition);

            SortSelectedData(s, orders);

            return s;
        }

        public static SelectedData SelectRows(List<AggregationColumn> columns, List<string> groupByColumns, TableId tableId, string whereCondition, List<OrderByColumn> orders)
        {
            Table table = Util.GetTable(tableId.tableName);

            SelectedData s = GetSelectedData(table, columns, whereCondition, groupByColumns);

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
                return column.column;
            else
                return (columnIndex + 1) + "_" + column.op + "(" + column.column + ")";
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
                    c.type = srcTable.GetColumnType(aggregrationColumn.column);
                    c.size = srcTable.GetColumnSize(aggregrationColumn.column);
                    columnDeclares.Add(c);
                }
            }

            Create.CreateTable(tempTableName, columnDeclares);
        }

        public static SelectedData GetSelectedData(Table table, List<AggregationColumn> aggregrationColumns, string condition, List<string> groupByColumns)
        {
            Table srcTable = Util.GetTable(table.tableName);
            List<int> groupByColumnIndex = null;
            if (groupByColumns != null)
                groupByColumnIndex = Util.GetColumnIndexFromName(srcTable, groupByColumns);
            List<int> aggregrationColumnIndex = Util.GetColumnIndexFromName(srcTable, aggregrationColumns.Select(s => s.column).ToList());
            SelectedData src = GetSelectedData(srcTable, aggregrationColumns, condition);

            string materializeTableName = "TempTable_" + (Gv.sn++);
            CreateTempGroupByTable(srcTable, materializeTableName, aggregrationColumns);
            Dictionary<string, object[]> groupByRows = new Dictionary<string, object[]>();

            foreach (int rowIndex in src.selectedRows)
            {
                object[] srcRow = srcTable.rows[rowIndex];

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
            result.columnNames = result.table.columnNames.ToList();
            result.columnIndex = Enumerable.Range(0, aggregrationColumns.Count).ToList();
            result.selectedRows = Enumerable.Range(0, result.table.rows.Count).ToList();
            result.needToDispose = true;

            return result;
        }
    }
}
