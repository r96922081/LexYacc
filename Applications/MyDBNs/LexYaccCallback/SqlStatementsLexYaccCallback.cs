namespace MyDBNs
{
    public class SqlStatementsLexYaccCallback
    {
        public static void SaveDB(string name)
        {
            Save.SaveDB(name);
        }

        public static void LoadDB(string name)
        {
            Load.LoadDB(name);
        }

        public static void CreateTable(string name, List<ColumnDeclare> columnDeclares)
        {
            Create.CreateTable(name, columnDeclares);
        }

        public static void DropTable(string name)
        {
            Drop.DropTable(name);
        }

        public static void ShowTables()
        {
            Show.ShowTables();
        }

        public static int Insert(string tableName, List<string> columnNames, List<string> values)
        {
            return MyDBNs.Insert.InsertRows(tableName, columnNames, values);
        }

        public static int Delete(string tableName, string condition)
        {
            return MyDBNs.Delete.DeleteRows(tableName, condition);
        }

        public static int Update(string tableName, List<SetExpressionType> setExpression, string condition)
        {
            return MyDBNs.Update.UpdateRows(tableName, setExpression, condition);
        }

        public static List<SetExpressionType> SetExpressionVarchar(string id, string stringExpression, List<SetExpressionType> setExpression)
        {
            List<SetExpressionType> ret = new List<SetExpressionType>();

            ret.AddRange(setExpression);
            ret.Add(new SetExpressionType(id, StringType.String, stringExpression));

            return ret;
        }

        public static List<SetExpressionType> SetExpressionVarchar(string id, string stringExpression)
        {
            List<SetExpressionType> ret = new List<SetExpressionType>();
            ret.Add(new SetExpressionType(id, StringType.String, stringExpression));

            return ret;
        }

        public static List<SetExpressionType> SetExpressionNumber(string id, string arithmeticExpression, List<SetExpressionType> setExpression)
        {
            List<SetExpressionType> ret = new List<SetExpressionType>();

            ret.AddRange(setExpression);
            ret.Add(new SetExpressionType(id, StringType.Number, arithmeticExpression));

            return ret;
        }

        public static List<SetExpressionType> SetExpressionNumber(string id, string arithmeticExpression)
        {
            List<SetExpressionType> ret = new List<SetExpressionType>();
            ret.Add(new SetExpressionType(id, StringType.Number, arithmeticExpression));

            return ret;
        }

        public static List<SetExpressionType> SetExpressionNull(string id)
        {
            List<SetExpressionType> ret = new List<SetExpressionType>();
            ret.Add(new SetExpressionType(id, StringType.Column, null));

            return ret;
        }

        public static List<SetExpressionType> SetExpressionNull(string id, List<SetExpressionType> setExpression)
        {
            List<SetExpressionType> ret = new List<SetExpressionType>();

            ret.AddRange(setExpression);
            ret.Add(new SetExpressionType(id, StringType.Column, null));

            return ret;
        }

        public static void CommaSepColumn(List<string> l, string s)
        {
            l.Add(s);
        }

        public static void CommaSepColumn(List<string> list, string s, List<string> prevList)
        {
            list.Add(s);
            list.AddRange(prevList);
        }

        public static void CommaSep_Column_Star(List<string> l, string s)
        {
            CommaSepColumn(l, s);
        }

        public static void CommaSep_Column_Star(List<string> list, string s, List<string> prevList)
        {
            CommaSepColumn(list, s, prevList);
        }

        public static List<ColumnDeclare> ColumnDeclares(ColumnDeclare columnDeclare, List<ColumnDeclare> prev)
        {
            List<ColumnDeclare> l = new List<ColumnDeclare>();

            if (prev != null)
                l.AddRange(prev);

            l.Add(columnDeclare);

            return l;
        }

        public static ColumnDeclare ColumnDeclare(string columnName, string columnType)
        {
            ColumnDeclare c = new ColumnDeclare();
            c.columnName = columnName;
            if (columnType.ToUpper().Equals("NUMBER"))
            {
                c.type = ColumnType.NUMBER;
                c.size = -1;
            }
            else if (columnType.ToUpper().Contains("VARCHAR"))
            {
                c.type = ColumnType.VARCHAR;
                c.size = Int32.Parse(columnType.ToUpper().Replace(")", "").Replace("VARCHAR(", ""));
            }

            return c;
        }

        public static SelectedData Select(List<AggregationColumn> columns, Tables tables, string whereCondition, List<string> groupByColumns, List<OrderByColumn> orderByColumns)
        {
            int aggregrationColumnCount = columns.Where(c => c.op != AggerationOperation.NONE).Count();

            if (aggregrationColumnCount == 0)
                return MyDBNs.Select.SelectRowsNoGroupBy(columns, tables, whereCondition, orderByColumns);
            else
            {
                columns = FixColumns(columns, tables);
                return MyDBNs.Select.SelectRowsGroupBy(columns, groupByColumns, Util.GetTable(tables.mainTable.targetTableName), whereCondition, orderByColumns);
            }
        }

        private static List<AggregationColumn> FixColumns(List<AggregationColumn> columns, Tables tables)
        {
            List<AggregationColumn> newColumns = new List<AggregationColumn>();

            foreach (AggregationColumn a in columns)
            {
                // * and A.*
                if (a.columnName == "*" && a.op == AggerationOperation.NONE)
                {
                    foreach (TableNameAlias table in tables.GetAllTables())
                    {
                        // Distinguish between * and A.* 
                        if (a.tableName != null && table.aliasTableName != a.tableName)
                            continue;

                        Table t = Util.GetTable(table.targetTableName);
                        foreach (Column column in t.columns)
                        {
                            string columnName = column.columnName;
                            AggregationColumn newColumn = new AggregationColumn();
                            newColumn.tableName = a.tableName;
                            newColumn.columnName = columnName;
                            newColumn.customColumnName = columnName;
                            newColumn.op = a.op;
                            newColumns.Add(newColumn);
                        }
                    }
                }
                else
                {
                    // C1
                    if (a.tableName == null)
                    {
                        List<TableNameAlias> tableIds = tables.GetTablesByColumnName(a.columnName);
                        if (tableIds.Count != 1)
                            throw new Exception("Column name is ambiguous or not found: " + a.columnName);

                        TableNameAlias tableAlias = tableIds[0];

                        AggregationColumn newColumn = new AggregationColumn();
                        newColumn.tableName = tableAlias.aliasTableName;
                        newColumn.columnName = a.columnName;
                        newColumn.customColumnName = a.customColumnName;
                        newColumn.op = a.op;

                        newColumns.Add(newColumn);
                    }
                    // A.C1
                    else
                    {
                        List<TableNameAlias> tableIds = tables.GetTablesByTableName(a.tableName);
                        if (tableIds.Count != 1)
                            throw new Exception("Table name is ambiguous or not found: " + a.tableName);

                        TableNameAlias tableAlias = tableIds[0];
                        AggregationColumn newColumn = new AggregationColumn();
                        newColumn.tableName = tableAlias.aliasTableName;
                        newColumn.columnName = a.columnName;
                        newColumn.customColumnName = a.customColumnName;
                        newColumn.op = a.op;

                        newColumns.Add(newColumn);
                    }
                }
            }

            return newColumns;
        }

        public static Tables Tables(TableNameAlias t, List<JoinTable> joins)
        {
            Tables ret = new Tables();
            ret.mainTable = t;
            ret.joinTables = joins;

            return ret;
        }

        public static List<JoinTable> JoinTables(List<JoinTable> prev, JoinTable table)
        {
            List<JoinTable> l = new List<JoinTable>();
            if (prev != null)
                l.AddRange(prev);

            l.Add(table);

            return l;
        }

        public static JoinTable JoinTable(TableNameAlias rhsTableId, string join_conditions)
        {
            JoinTable j = new JoinTable();

            j.tableNameAlias = rhsTableId;
            j.conditions = join_conditions;

            return j;
        }

        public static string JoinConditions(string lhs, string op, string rhs)
        {
            if (op == null)
                return lhs;
            else
                return lhs + " " + op + " " + rhs;
        }

        public static string BooleanExpression(string lhs, string op, string rhs)
        {
            return lhs + " " + op + " " + rhs;
        }

        public static OrderByColumn OrderByColumn(object column, bool ascending)
        {
            OrderByColumn o = new OrderByColumn();
            o.column = column;
            if (ascending)
                o.direction = OrderByDirection.ASEC;
            else
                o.direction = OrderByDirection.DESC;

            return o;
        }

        public static List<OrderByColumn> OrderByColumns(OrderByColumn order, List<OrderByColumn> prev)
        {
            List<OrderByColumn> l = new List<OrderByColumn>();

            if (prev != null)
                l.AddRange(prev);

            l.Add(order);

            return l;
        }

        public static List<AggregationColumn> AggregrationColumns(List<AggregationColumn> prev, AggregationColumn column)
        {
            List<AggregationColumn> l = new List<AggregationColumn>();
            if (prev != null)
                l.AddRange(prev);

            l.Add(column);

            return l;
        }

        public static AggregationColumn AggregationColumn(AggerationOperation op, string columnName)
        {
            AggregationColumn a = new AggregationColumn();
            a.columnName = columnName;
            if (a.columnName.Contains("."))
            {
                a.tableName = a.columnName.Substring(0, a.columnName.IndexOf("."));
                a.columnName = a.columnName.Substring(a.columnName.IndexOf(".") + 1);
            }

            a.op = op;

            return a;
        }

        public static AggregationColumn AggregationColumnAs(AggregationColumn a, string displayName)
        {
            a.customColumnName = displayName;
            if (displayName == null)
                a.customColumnName = a.columnName;

            return a;
        }

        public static string TransactionStart()
        {
            return Transaction.TransactionStart();
        }

        public static int Commit()
        {
            return Transaction.Commit();
        }

        public static int Rollback()
        {
            return Transaction.Rollback();
        }

        public static TableNameAlias TableNameAlias(string tableName, string displayTableName)
        {
            return new TableNameAlias(tableName, displayTableName);
        }
    }
}
