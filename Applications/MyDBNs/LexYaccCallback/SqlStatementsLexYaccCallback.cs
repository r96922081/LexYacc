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

        public static SelectedData Select(List<AggregationColumn> columns, TableId tableId, string condition, List<string> groupByColumns, List<OrderByColumn> orderByColumns)
        {
            columns = NormalizeColumns(columns, tableId);
            int aggregrationColumnCount = columns.Where(c => c.op != AggerationOperation.NONE).Count();

            if (aggregrationColumnCount == 0)
                return MyDBNs.Select.SelectRows(columns, tableId, condition, orderByColumns);
            else
                return MyDBNs.Select.SelectRows(columns, groupByColumns, tableId, condition, orderByColumns);
        }

        private static List<AggregationColumn> NormalizeColumns(List<AggregationColumn> columns, TableId tableId)
        {
            List<AggregationColumn> newColumns = new List<AggregationColumn>();
            foreach (AggregationColumn a in columns)
            {
                if (a.displayTable == null)
                {
                    a.displayTable = tableId.displayTableName;
                    a.table = tableId.tableName;
                }
                else if (a.displayTable == tableId.displayTableName)
                {
                    a.table = tableId.tableName;
                }
                else
                    throw new Exception();

                if (a.columnName == "*" && a.op == AggerationOperation.NONE)
                {
                    Table t = Util.GetTable(tableId.tableName);
                    foreach (string columnName in t.columnNames)
                    {
                        AggregationColumn newColumn = new AggregationColumn();
                        newColumn.table = a.table;
                        newColumn.displayTable = a.displayTable;
                        newColumn.columnName = columnName;
                        newColumn.displayColumnName = columnName;
                        newColumn.op = a.op;
                        newColumns.Add(newColumn);
                    }
                }
                else
                    newColumns.Add(a);
            }

            return newColumns;
        }

        public static void BooleanExpression(ref string booleanExpression, string lhs, string op, string rhs)
        {
            booleanExpression = lhs + " " + op + " " + rhs;
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
                a.displayTable = a.columnName.Substring(0, a.columnName.IndexOf("."));
                a.columnName = a.columnName.Substring(a.columnName.IndexOf(".") + 1);
            }

            a.op = op;

            return a;
        }

        public static AggregationColumn AggregationColumnAs(AggregationColumn a, string displayName)
        {
            a.displayColumnName = displayName;
            if (displayName == null)
                a.displayColumnName = a.columnName;

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

        public static TableId TableId(string tableName, string displayTableName)
        {
            return new TableId(tableName, displayTableName);
        }
    }
}
