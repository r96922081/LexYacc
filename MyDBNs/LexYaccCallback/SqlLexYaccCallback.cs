namespace MyDBNs
{
    public class SqlLexYaccCallback
    {
        public static void CreateTable(string name, List<(string, string)> columnDeclare)
        {
            Create.CreateTable(name, columnDeclare);
        }

        public static void ShowTables()
        {
            Show.ShowTables();
        }

        public static void Insert(string tableName, List<string> columnNames, List<string> values)
        {
            MyDBNs.Insert.InsertRows(tableName, columnNames, values);
        }

        public static void Delete(string tableName, string condition)
        {
            MyDBNs.Delete.DeleteRows(tableName, condition);
        }

        public static void Update(string tableName, List<Tuple<string, string>> setExpression, string condition)
        {
            MyDBNs.Update.UpdateRows(tableName, setExpression, condition);
        }

        public static List<Tuple<string, string>> SetExpression(string id, string string_number_id, List<Tuple<string, string>> setExpression)
        {
            List<Tuple<string, string>> ret = new List<Tuple<string, string>>();

            ret.AddRange(setExpression);
            ret.Add(new Tuple<string, string>(id, string_number_id));

            return ret;
        }

        public static List<Tuple<string, string>> SetExpression(string id, string string_number_id)
        {
            List<Tuple<string, string>> ret = new List<Tuple<string, string>>();

            ret.Add(new Tuple<string, string>(id, string_number_id));

            return ret;
        }

        public static void CommaSepID(List<string> l, string s)
        {
            l.Add(s);
        }

        public static void CommaSepID(List<string> list, string s, List<string> prevList)
        {
            list.Add(s);
            list.AddRange(prevList);
        }

        public static void CommaSepIDIncludeStar(List<string> l, string s)
        {
            CommaSepID(l, s);
        }

        public static void CommaSepIDIncludeStar(List<string> list, string s, List<string> prevList)
        {
            CommaSepID(list, s, prevList);
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

        public static void Select(List<string> columns, string tableName, string condition, List<List<object>> orders)
        {
            MyDBNs.Select.SelectRows(columns, tableName, condition, orders);
        }

        public static void BooleanExpression1(ref string booleanExpression, string rhs)
        {
            booleanExpression = " ( " + rhs + " ) ";
        }

        public static void BooleanExpression2(ref string booleanExpression, string lhs, string op, string rhs)
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
