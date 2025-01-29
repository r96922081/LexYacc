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

        public static SelectedData SelectRows(List<AggregationColumn> aggregrationColumns, string tableName, string whereCondition, List<string> groupByColumns, List<OrderByColumn> orderByColumns)
        {
            Table srcTable = Util.GetTable(tableName);
            List<int> groupByColumnIndex = Util.GetColumnIndexFromName(srcTable, groupByColumns);
            List<int> aggregrationColumnIndex = Util.GetColumnIndexFromName(srcTable, aggregrationColumns.Select(s => s.columnName).ToList());
            SelectedData src = GetSelectedData(srcTable, aggregrationColumns.Select(s => s.columnName).ToList(), whereCondition);

            string tempTableName = "TempTable_" + (Gv.sn++);
            CreateTempGroupByTable(srcTable, tempTableName, aggregrationColumns);
            Dictionary<string, object[]> groupByRows = new Dictionary<string, object[]>();

            foreach (int rowIndex in src.selectedRows)
            {
                object[] srcRow = srcTable.rows[rowIndex];

                string groupKey = GetGroupKey(srcRow, groupByColumnIndex);
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

            Insert.InsertRows(tempTableName, groupByRows.Values.ToList());

            SelectedData result = new SelectedData();
            result.table = Util.GetTable(tempTableName);
            result.columnNames = result.table.columnNames.ToList();
            result.columnIndex = Enumerable.Range(0, aggregrationColumns.Count).ToList();
            result.selectedRows = Enumerable.Range(0, result.table.rows.Count).ToList();
            result.needToDispose = true;

            return result;
        }
    }
}
