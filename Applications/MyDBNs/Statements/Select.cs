using System.Text;

namespace MyDBNs
{
    public class Select
    {
        private static List<int> GetSelectedRows(string tableName, string whereCondition)
        {
            List<int> rows = new List<int>();
            if (whereCondition == null)
            {
                Table table = Util.GetTable(tableName);
                rows = new List<int>();
                for (int i = 0; i < table.rows.Count; i++)
                    rows.Add(i);

                return rows;
            }

            SqlBooleanExpressionLexYaccCallback.table = Util.GetTable(tableName);
            rows = new List<int>((HashSet<int>)sql_boolean_expression.Parse(whereCondition));
            rows.Sort();
            return rows;
        }

        private static SelectedData GetSelectedData(Table table, List<AggregationColumn> columns, string whereCondition, string joinCondition)
        {
            SelectedData s = new SelectedData();

            s.table = table;

            foreach (AggregationColumn column in columns)
            {
                s.selectedColumnNames.Add(column.columnName);
                s.customColumnNames.Add(column.customColumnName);
                s.selectedColumnIndex.Add(s.table.GetColumnIndex(column.tableName + "." + column.columnName));
            }

            s.selectedRows = GetSelectedRows(s.table.name, whereCondition);

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
                    for (int i = 0; i < s.selectedColumnNames.Count; i++)
                    {
                        if (s.selectedColumnNames[i].ToUpper() == ((string)order.column).ToUpper())
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
                    int columnIndex = s.selectedColumnIndex[o.selectColumnIndex];
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

        public static Table JoinTable(Tables tables)
        {
            Table joined = new Table();
            joined.name = "TempJoinedTable_" + (Gv.sn++);

            int columnCount = tables.GetAllTables().Select(t => Util.GetTable(t.targetTableName).columns.Length).Sum();
            joined.columns = new Column[columnCount];

            int index = 0;
            for (int tableIndex = 0; tableIndex < tables.GetAllTables().Count; tableIndex++)
            {
                TableNameAlias tableAlias = tables.GetAllTables()[tableIndex];
                Table table = Util.GetTable(tableAlias.targetTableName);

                foreach (Column column in table.columns)
                {
                    Column newColumn = new Column();
                    newColumn.columnName = column.columnName;
                    newColumn.userColumnName = column.userColumnName;
                    newColumn.size = column.size;
                    newColumn.type = column.type;

                    if (tableAlias.aliasTableName != null)
                        newColumn.tableName = tableAlias.aliasTableName;
                    else
                        newColumn.tableName = tableAlias.targetTableName;

                    joined.columns[index++] = newColumn;
                }
            }

            int totalRowCount = tables.GetAllTables().Select(t => Util.GetTable(t.targetTableName).rows.Count).Aggregate(1, (a, b) => a * b);
            joined.rows = new List<object[]>();
            for (int i = 0; i < totalRowCount; i++)
            {
                object[] row = new object[columnCount];
                joined.rows.Add(row);
            }

            int columnStartIndex = 0;
            int repeatCount = totalRowCount;
            for (int tableIndex = 0; tableIndex < tables.GetAllTables().Count; tableIndex++)
            {
                Table table = Util.GetTable(tables.GetAllTables()[tableIndex].targetTableName);
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

        private static List<AggregationColumn> ExpandWildCard(List<AggregationColumn> columns, Tables tables)
        {
            List<AggregationColumn> expandedColumns = new List<AggregationColumn>();

            foreach (AggregationColumn a in columns)
            {
                if (a.columnName != "*")
                {
                    expandedColumns.Add(a);
                    continue;
                }

                if (a.tableName == null)
                {
                    foreach (TableNameAlias table in tables.GetAllTables())
                    {
                        Table t = Util.GetTable(table.targetTableName);
                        foreach (Column column in t.columns)
                        {
                            AggregationColumn newColumn = new AggregationColumn();
                            if (table.aliasTableName != null)
                                newColumn.tableName = table.aliasTableName;
                            else
                                newColumn.tableName = table.targetTableName;
                            newColumn.columnName = column.columnName;
                            newColumn.op = a.op;
                            expandedColumns.Add(newColumn);
                        }
                    }
                }
                else
                {
                    Table t = null;
                    TableNameAlias tableAlias = null;
                    foreach (TableNameAlias table in tables.GetAllTables())
                    {
                        if (table.aliasTableName != null && table.aliasTableName.ToUpper() == a.tableName.ToUpper())
                        {
                            t = Util.GetTable(table.targetTableName);
                            tableAlias = table;
                            break;
                        }
                        else if (table.targetTableName.ToUpper() == a.tableName.ToUpper())
                        {
                            t = Util.GetTable(table.targetTableName);
                            tableAlias = table;
                            break;
                        }
                    }
                    if (t == null)
                        throw new Exception("Table " + a.tableName + " not found");

                    foreach (Column column in t.columns)
                    {
                        AggregationColumn newColumn = new AggregationColumn();
                        if (tableAlias.aliasTableName != null)
                            newColumn.tableName = tableAlias.aliasTableName;
                        else
                            newColumn.tableName = tableAlias.targetTableName;
                        newColumn.columnName = column.columnName;
                        newColumn.op = a.op;
                        expandedColumns.Add(newColumn);
                    }
                }
            }

            return expandedColumns;
        }

        private static void AddTableNameToColumn(List<AggregationColumn> columns, Tables tables)
        {
            foreach (AggregationColumn column in columns)
            {
                if (column.tableName == null)
                {
                    foreach (TableNameAlias table in tables.GetAllTables())
                    {
                        Table t = Util.GetTable(table.targetTableName);
                        if (t.GetColumnIndex(column.columnName.ToUpper()) != -1)
                        {
                            if (table.aliasTableName != null)
                                column.tableName = table.aliasTableName;
                            else
                                column.tableName = table.targetTableName;
                            break;
                        }
                    }
                }
            }
        }

        public static SelectedData SelectRowsNoGroupBy(List<AggregationColumn> columns, Tables tables, string whereCondition, List<OrderByColumn> orders)
        {
            columns = ExpandWildCard(columns, tables);

            AddTableNameToColumn(columns, tables);

            SelectedData s = new SelectedData();

            Table table = JoinTable(tables);

            s.table = table;

            foreach (AggregationColumn column in columns)
            {
                s.selectedColumnNames.Add(column.columnName);
                s.customColumnNames.Add(column.customColumnName);

                // set column index
                int columnIndex = 0;
                int foundColumnIndex = -1;
                List<TableNameAlias> availableTables = tables.GetAllTables();
                for (int i = 0; i < availableTables.Count && foundColumnIndex == -1; i++)
                {
                    TableNameAlias availableTable = availableTables[i];
                    Table t = Util.GetTable(availableTables[i].targetTableName);
                    if ((availableTable.aliasTableName != null && availableTable.aliasTableName.ToUpper() == column.tableName.ToUpper())
                        ||
                        (availableTable.targetTableName.ToUpper() == column.tableName.ToUpper()))
                    {
                        for (int j = 0; j < t.columns.Length; j++)
                        {
                            if (t.columns[j].columnName.ToUpper() == column.columnName.ToUpper())
                            {
                                foundColumnIndex = columnIndex + j;
                                break;
                            }
                        }
                    }
                    else
                    {
                        columnIndex += t.columns.Length;
                    }
                }

                if (foundColumnIndex == -1)
                    throw new Exception("Column " + column.columnName + " not found");

                s.selectedColumnIndex.Add(foundColumnIndex);
            }

            s.selectedRows = GetSelectedRows(s.table.name, whereCondition);

            SortSelectedData(s, orders);

            return s;
        }

        public static SelectedData SelectRowsGroupBy(List<AggregationColumn> columns, List<string> groupByColumns, Table t, string whereCondition, List<OrderByColumn> orders)
        {
            SelectedData s = GetSelectedData(t, columns, whereCondition, null, groupByColumns);

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

        public static SelectedData GetSelectedData(Table table, List<AggregationColumn> aggregrationColumns, string whereCondition, string joinCondition, List<string> groupByColumns)
        {
            List<int> groupByColumnIndex = null;
            if (groupByColumns != null)
                groupByColumnIndex = Util.GetColumnIndexFromName(table, groupByColumns);
            List<int> aggregrationColumnIndex = Util.GetColumnIndexFromName(table, aggregrationColumns.Select(s => s.columnName).ToList());
            SelectedData src = GetSelectedData(table, aggregrationColumns, whereCondition, joinCondition);

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
            result.selectedColumnNames = result.table.columns.Select(s => s.columnName).ToList();
            result.selectedColumnIndex = Enumerable.Range(0, aggregrationColumns.Count).ToList();
            result.selectedRows = Enumerable.Range(0, result.table.rows.Count).ToList();
            result.needToDispose = true;

            return result;
        }
    }
}
