﻿namespace MyDBNs
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

        private static SelectedData Select(List<AggregationColumn> columns, TableId tableId, string condition, List<OrderByColumn> orderByColumns)
        {
            return MyDBNs.Select.SelectRows(columns, tableId, condition, orderByColumns);
        }

        private static SelectedData Select(List<AggregationColumn> columns, List<string> groupByColumns, TableId tableId, string condition, List<OrderByColumn> orderByColumns)
        {
            return MyDBNs.Select.SelectRows(columns, groupByColumns, tableId, condition, orderByColumns);
        }

        private static List<AggregationColumn> SplitTableColumnName(List<AggregationColumn> columns)
        {
            List<AggregationColumn> columns2 = new List<AggregationColumn>();
            foreach (AggregationColumn a in columns)
            {
                AggregationColumn a2 = new AggregationColumn();
                a2.op = a.op;

                if (a.columnName.Contains("."))
                {
                    a2.table = a.columnName.Substring(0, a.columnName.IndexOf("."));
                    a2.columnName = a.columnName.Substring(a.columnName.IndexOf(".") + 1);
                }
                else
                {
                    a2.table = "";
                    a2.columnName = a.columnName;
                }

                columns2.Add(a2);
            }

            return columns2;
        }

        public static SelectedData Select(List<AggregationColumn> columns, TableId tableId, string condition, List<string> groupByColumns, List<OrderByColumn> orderByColumns)
        {
            columns = SplitTableColumnName(columns);
            int aggregrationColumnCount = columns.Where(c => c.op != AggerationOperation.NONE).Count();

            if (aggregrationColumnCount == 0)
                return Select(columns, tableId, condition, orderByColumns);
            else
                return Select(columns, groupByColumns, tableId, condition, orderByColumns);
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
            a.columnName= columnName;
            a.op= op;

            return a;
        }

        public static AggregationColumn AggregationColumnAs(AggregationColumn a, string displayName)
        {
            a.displayColumnName = displayName;

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
