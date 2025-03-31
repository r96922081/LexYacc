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

        private static Table GetMainTableForJoin(TableNameAlias tableNameAlias)
        {
            Table joined = new Table();
            joined.name = "TempTableJoined_" + (Gv.sn++);

            Table targetTable = Util.GetTable(tableNameAlias.targetTableName);
            joined.columns = new Column[targetTable.columns.Length];

            for (int i = 0; i < targetTable.columns.Length; i++)
            {
                Column column = targetTable.columns[i];
                Column newColumn = new Column();
                newColumn.columnName = column.columnName;
                newColumn.userColumnName = column.userColumnName;
                newColumn.size = column.size;
                newColumn.type = column.type;

                if (tableNameAlias.aliasTableName != null)
                    newColumn.tableName = tableNameAlias.aliasTableName;
                else
                    newColumn.tableName = tableNameAlias.targetTableName;

                joined.columns[i] = newColumn;
            }
            joined.rows = targetTable.rows;

            Create.AddTable(joined);

            return joined;
        }

        public static Table JoinTable(Tables tables)
        {
            Table joined = GetMainTableForJoin(tables.mainTable);

            if (tables.joinTables == null)
                return joined;

            for (int i = 0; i < tables.joinTables.Count; i++)
            {
                Table newJoined = new Table();
                newJoined.name = "TempTableJoined_" + (Gv.sn++);

                JoinTable joinSetting = tables.joinTables[i];
                Table targetTable = Util.GetTable(joinSetting.tableNameAlias.targetTableName);

                newJoined.columns = new Column[joined.columns.Length + targetTable.columns.Length];

                for (int j = 0; j < joined.columns.Length; j++)
                {
                    newJoined.columns[j] = joined.columns[j].Clone();
                }

                for (int j = 0; j < targetTable.columns.Length; j++)
                {
                    Column column = targetTable.columns[j];
                    Column newColumn = new Column();
                    newColumn.columnName = column.columnName;
                    newColumn.userColumnName = column.userColumnName;
                    newColumn.size = column.size;
                    newColumn.type = column.type;

                    if (joinSetting.tableNameAlias.aliasTableName != null)
                        newColumn.tableName = joinSetting.tableNameAlias.aliasTableName;
                    else
                        newColumn.tableName = joinSetting.tableNameAlias.targetTableName;

                    newJoined.columns[joined.columns.Length + j] = newColumn;
                }

                for (int j = 0; j < joined.rows.Count; j++)
                {
                    for (int k = 0; k < targetTable.rows.Count; k++)
                    {
                        object[] row = new object[newJoined.columns.Length];
                        newJoined.rows.Add(row);

                        for (int l = 0; l < joined.columns.Length; l++)
                            row[l] = joined.rows[j][l];

                        for (int l = 0; l < targetTable.columns.Length; l++)
                            row[joined.columns.Length + l] = targetTable.rows[k][l];
                    }
                }

                // filter rows
                if (joinSetting.conditions != null)
                {
                    SqlBooleanExpressionLexYaccCallback.table = newJoined;
                    HashSet<int> filteredRows = (HashSet<int>)sql_boolean_expression.Parse(joinSetting.conditions);
                    for (int j = newJoined.rows.Count - 1; j >= 0; j--)
                    {
                        if (!filteredRows.Contains(j))
                            newJoined.rows.RemoveAt(j);
                    }
                }

                Drop.DropTable(joined.name);
                Create.AddTable(newJoined);

                joined = newJoined;
            }

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

        public static SelectedData DoSelect(List<AggregationColumn> columns, Tables tables, string whereCondition, List<string> groupByColumns, List<OrderByColumn> orderByColumns)
        {
            int aggregrationColumnCount = columns.Where(c => c.op != AggerationOperation.NONE).Count();

            SelectedData s = new SelectedData();
            Table table = JoinTable(tables);
            s.table = table;
            s.selectedRows = GetSelectedRows(s.table.name, whereCondition);

            if (aggregrationColumnCount == 0)
                SetColumnsWithoutGroupBy(s, columns, tables);
            else
                SetColumnsWithGroupBy(s, columns, tables, groupByColumns);

            SortSelectedData(s, orderByColumns);

            return s;
        }

        public static void SetColumnsWithGroupBy(SelectedData s, List<AggregationColumn> aggregrationColumns, Tables tables, List<string> groupByColumns)
        {
            // modify count(*) to count(C1)
            foreach (AggregationColumn a in aggregrationColumns)
            {
                if (a.columnName == "*")
                    a.columnName = s.table.columns[0].columnName;
            }

            AddTableNameToColumn(aggregrationColumns, tables);
            List<int> aggregrationColumnIndex = new List<int>();
            foreach (AggregationColumn a in aggregrationColumns)
            {
                int columnIndex = s.table.GetColumnIndex(a.tableName + "." + a.columnName);
                aggregrationColumnIndex.Add(columnIndex);
            }

            Table srcTable = s.table;
            List<int> groupByColumnIndex = new List<int>();

            if (groupByColumns != null)
            {
                foreach (string groupByColumn in groupByColumns)
                {
                    int columnIndex = srcTable.GetColumnIndex(groupByColumn);
                    if (columnIndex == -1)
                        throw new Exception("Column " + groupByColumn + " not found");
                    groupByColumnIndex.Add(columnIndex);
                }
            }
            else
            {
                for (int i = 0; i < s.table.columns.Length; i++)
                    groupByColumnIndex.Add(i);
            }

            Dictionary<string, object[]> groupByRows = new Dictionary<string, object[]>();

            foreach (int rowIndex in s.selectedRows)
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

                string tempTableName = "TempTableGroupBy_" + (Gv.sn++);
                CreateTempGroupByTable(srcTable, tempTableName, aggregrationColumns);

                Insert.InsertRows(tempTableName, groupByRows.Values.ToList());

                s.table = Util.GetTable(tempTableName);
                s.selectedColumnNames = s.table.columns.Select(s => s.columnName).ToList();
                s.customColumnNames = aggregrationColumns.Select(s => s.customColumnName).ToList();
                s.selectedColumnIndex = Enumerable.Range(0, aggregrationColumns.Count).ToList();
                s.selectedRows = Enumerable.Range(0, s.table.rows.Count).ToList();
            }
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

        public static void SetColumnsWithoutGroupBy(SelectedData s, List<AggregationColumn> columns, Tables tables)
        {
            columns = ExpandWildCard(columns, tables);
            AddTableNameToColumn(columns, tables);
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
    }
}
