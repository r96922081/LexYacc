namespace MyDBNs
{
    public class SqlLexYaccCallback
    {
        public static void SaveDB(string name)
        {
            Save.SaveDB(name);
        }

        public static void LoadDB(string name)
        {
            Load.LoadDB(name);
        }

        public static void CreateTable(string name, List<(string, string)> columnDeclare)
        {
            Create.CreateTable(name, columnDeclare);
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

        public static void ColumnDeclare(List<(string, string)> columnDeclare, string columnName, string columnType)
        {
            columnDeclare.Add((columnName, columnType));
        }

        public static void ColumnDeclare(List<(string, string)> columnDeclare, string columnName, string columnType, List<(string, string)> prevColumnDeclare)
        {
            columnDeclare.Add((columnName, columnType));
            columnDeclare.AddRange(prevColumnDeclare);
        }

        public static SelectedData Select(List<string> columns, string tableName, string condition, List<List<object>> orders)
        {
            return MyDBNs.Select.SelectRows(columns, tableName, condition, orders);
        }

        public static void BooleanExpression(ref string booleanExpression, string lhs, string op, string rhs)
        {
            booleanExpression = lhs + " " + op + " " + rhs;
        }

        public static void OrderByColumn(ref List<object> ret, object column, bool ascending)
        {
            ret.Add(column);
            ret.Add(ascending);
        }

        public static void OrderByCondition(List<List<object>> condition, List<object> order, List<List<object>> prevCondition)
        {
            condition.Add(order);

            if (prevCondition != null)
                condition.AddRange(prevCondition);
        }
    }
}
