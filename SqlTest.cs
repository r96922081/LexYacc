using System.Diagnostics;
using System.Text;

public enum ColumnType
{
    NUMBER,
    VARCHAR
}

public class Table
{
    public string tableName;
    public string[] columnNames;
    public ColumnType[] columnTypes;
    public int[] columnSizes;
    public Dictionary<string, int> columnIndexMap;
    public Dictionary<string, ColumnType> columnTypesMap;

    public List<object[]> rows = new List<object[]>();

    public void Insert(object[] row)
    {
        rows.Add(row);
    }

    public string GetSchema()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("table: " + tableName);
        for (int i = 0; i < columnNames.Length; i++)
        {
            sb.Append(columnNames[i] + " " + columnTypes[i]);
            if (columnTypes[i] == ColumnType.VARCHAR)
            {
                sb.Append("(" + columnSizes[i] + ")");
            }

            if (i != columnNames.Length - 1)
                sb.AppendLine(",");
            else
                sb.AppendLine();
        }


        return sb.ToString();
    }

    public string GetData()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("table: " + tableName);

        foreach (object[] row in rows)
        {
            sb.Append("| ");
            for (int i = 0; i < row.Length; i++)
            {
                if (row[i] == null)
                    ;
                else
                    sb.Append(row[i].ToString());
                sb.Append(" | ");
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }

}

public class DB
{
    public static List<Table> tables = new List<Table>();
}


public class SqlLexYaccCallback
{
    public static void VerifyCreateTable(string name, List<(string, string)> columnDeclare)
    {
        for (int i = 0; i < columnDeclare.Count; i++)
        {
            string columnType = columnDeclare[i].Item2;
            if (columnType.StartsWith("VARCHAR"))
            {
                int left = columnType.IndexOf('(');
                int right = columnType.LastIndexOf(')');

                int lengthInt;
                string length = columnType.Substring(left + 1, right - left - 2);
                if (!int.TryParse(length, out lengthInt))
                    throw new Exception("Invalid VARCHAR length = " + lengthInt);
            }
        }

    }

    public static void CreateTable(string name, List<(string, string)> columnDeclare)
    {
        VerifyCreateTable(name, columnDeclare);

        Table table = new Table();
        table.tableName = name;
        table.columnNames = new string[columnDeclare.Count];
        table.columnTypes = new ColumnType[columnDeclare.Count];
        table.columnSizes = new int[columnDeclare.Count];

        for (int i = 0; i < columnDeclare.Count; i++)
        {
            table.columnNames[i] = columnDeclare[i].Item1;
            string columnType = columnDeclare[i].Item2;
            if (columnType == "NUMBER")
            {
                table.columnTypes[i] = ColumnType.NUMBER;
            }
            else if (columnType.StartsWith("VARCHAR"))
            {
                table.columnTypes[i] = ColumnType.VARCHAR;
                int left = columnType.IndexOf('(');
                int right = columnType.LastIndexOf(')');

                int lengthInt;
                string length = columnType.Substring(left + 1, right - left - 1);
                int.TryParse(length, out lengthInt);
                table.columnSizes[i] = lengthInt;
            }
        }


        table.columnIndexMap = new Dictionary<string, int>();
        table.columnTypesMap = new Dictionary<string, ColumnType>();

        for (int i = 0; i < table.columnNames.Length; i++)
        {
            table.columnIndexMap.Add(table.columnNames[i], i);
            table.columnTypesMap.Add(table.columnNames[i], table.columnTypes[i]);
        }

        DB.tables.Add(table);
    }

    public static void ShowTables()
    {
        foreach (Table table in DB.tables)
            Console.WriteLine(table.GetSchema());
    }

    public static void VerifyShowTable(string table)
    {
        foreach (Table t2 in DB.tables)
            if (t2.tableName == table)
                return;

        throw new Exception("No table named: " + table);
    }


    public static void ShowTable(string table)
    {
        VerifyShowTable(table);

        Table t = null;
        foreach (Table t2 in DB.tables)
        {
            if (t2.tableName == table)
            {
                t = t2;
                break;
            }
        }

        Console.WriteLine(t.GetData());
    }

    public class DB
    {
        public static List<Table> tables = new List<Table>();
    }

    public enum BooleanOperator
    {
        EQUAL,
        NOT_EQUAL,
        LESS,
        GREATER,
        LESS_OR_EQUAL,
        GREATER_OR_EQUAL,
        NONE
    }

    public class BooleanOperand { }

    public class BooleanOperandString : BooleanOperand
    {
        public string s;

        public BooleanOperandString()
        {

        }

        public BooleanOperandString(string s)
        {
            this.s = s;
        }
    }

    public class BooleanOperation : BooleanOperand
    {
        public BooleanOperand lhs;
        public BooleanOperand rhs;
        public BooleanOperator op;

        public BooleanOperation()
        {
            op = BooleanOperator.NONE;
        }

        public BooleanOperation(BooleanOperand lhs, BooleanOperand rhs, BooleanOperator op)
        {
            this.lhs = lhs;
            this.rhs = rhs;
            this.op = op;
        }
    }

    // INSERT INTO A (aaa, bbb) VALUES (AAA, BB)
    // INSERT INTO A (bbb) VALUES (BB)
    // INSERT INTO A VALUES (AAA, BB)
    public static void VerifyInsertRow(string tableName, List<string> columnNames, List<string> values)
    {
        Table table = DB.tables.FirstOrDefault(t => t.tableName == tableName);
        if (table == null)
            throw new Exception("no table named: " + tableName);

        if (columnNames == null || columnNames.Count == 0)
            columnNames = table.columnNames.ToList();

        if (columnNames.Count != values.Count)
            throw new Exception("column count != value count");

        foreach (string columnName in columnNames)
        {
            if (!table.columnIndexMap.ContainsKey(columnName))
                throw new Exception("no column named: " + columnName);
        }

        for (int i = 0; i < values.Count; i++)
        {
            string columnName = columnNames[i];
            ColumnType columnType = table.columnTypesMap[columnName];
            string value = values[i];
            if (value == null)
                continue;

            if (columnType == ColumnType.NUMBER)
            {
                int result;
                if (int.TryParse(value.ToString(), out result) == false)
                    throw new Exception("Invalid input = " + value + ", type of column " + columnName + " is " + columnType);
            }
        }
    }

    public static void InsertRow(string tableName, List<string> columnNames, List<string> values)
    {
        VerifyInsertRow(tableName, columnNames, values);

        Table table = DB.tables.FirstOrDefault(t => t.tableName == tableName);

        if (columnNames == null || columnNames.Count == 0)
            columnNames = table.columnNames.ToList();

        object[] rows = new object[table.columnNames.Length];

        for (int i = 0; i < values.Count; i++)
        {
            string columnName = columnNames[i];
            int columnIndex = table.columnIndexMap[columnName];
            ColumnType columnType = table.columnTypesMap[columnName];
            string value = values[i];
            if (value == null)
            {
                rows[columnIndex] = null;
            }
            else
            {
                if (columnType == ColumnType.NUMBER)
                {
                    rows[columnIndex] = int.Parse(value.ToString());
                }
                else if (columnType == ColumnType.VARCHAR)
                {
                    rows[columnIndex] = value.ToString();
                }
            }
        }

        table.rows.Add(rows);
    }

    public static void CommaSepString(List<string> l, string s)
    {
        l.Add(s);
    }

    public static void CommaSepString(List<string> list, string s, List<string> prevList)
    {
        list.Add(s);
        list.AddRange(prevList);
    }

    public static void CommaSepStringIncludeStar(List<string> l, string s)
    {
        CommaSepString(l, s);
    }

    public static void CommaSepStringIncludeStar(List<string> list, string s, List<string> prevList)
    {
        CommaSepString(list, s, prevList);
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

    public static void Select(List<string> columns, string tableName)
    {
        Table table = DB.tables.FirstOrDefault(t => t.tableName == tableName);

        List<string> columnNames = new List<string>();
        List<int> columnIndex = new List<int>();
        foreach (string column in columns)
        {
            if (column == "*")
            {
                foreach (string column2 in table.columnNames)
                {
                    columnNames.Add(column2);
                    columnIndex.Add(table.columnIndexMap[column2]);
                }
            }
            else
            {
                columnNames.Add(column);
                columnIndex.Add(table.columnIndexMap[column]);
            }
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("table: " + table.tableName);

        sb.Append("| ");
        foreach (string columnName in columnNames)
        {
            sb.Append(columnName);
            sb.Append(" | ");
        }
        sb.AppendLine();

        foreach (object[] row in table.rows)
        {
            sb.Append("| ");
            foreach (int index in columnIndex)
            {
                sb.Append(row[index]);
                sb.Append(" | ");
            }
            sb.AppendLine();
        }

        Console.WriteLine(sb.ToString());
    }

    public static void BooleanExpressionAnd(ref bool result, bool lhs, bool rhs)
    {
        result = lhs && rhs;
    }


    public static void BooleanExpression2Equal(ref BooleanOperation result, string lhs, bool rhs)
    {
        //result = new BooleanOperation(lhs, rhs, BooleanOperator.EQUAL);
    }
}

public class SqlTest()
{
    public static void Check(bool b)
    {
        if (!b)
            Trace.Assert(false);
    }


    public static void Ut()
    {

        object ret = sql_lexyacc.Parse("CREATE TABLE A ( NAME VARCHAR(123), AGE NUMBER)");
        Check(ret == null || ret.ToString() == "");

        ret = sql_lexyacc.Parse("CREATE TABLE A2 ( AAA VARCHAR(456), BBB NUMBER)");
        Check(ret == null || ret.ToString() == "");

        ret = sql_lexyacc.Parse("INSERT INTO A ( NAME, AGE ) VALUES ( DEF, 33  )");
        Check(ret == null || ret.ToString() == "");

        ret = sql_lexyacc.Parse("INSERT INTO A VALUES ( 44, 55  )");
        Check(ret == null || ret.ToString() == "");

        ret = sql_lexyacc.Parse("INSERT INTO A ( AGE, NAME ) VALUES ( 66, ABC  )");
        Check(ret == null || ret.ToString() == "");

        ret = sql_lexyacc.Parse("INSERT INTO A ( AGE ) VALUES ( 999  )");
        Check(ret == null || ret.ToString() == "");

        ret = sql_lexyacc.Parse("SHOW TABLES");
        Check(ret == null || ret.ToString() == "");

        ret = sql_lexyacc.Parse("DELETE FROM A WHERE (A > B) OR (D <= E AND F >= G)");
        Check(ret == null || ret.ToString() == "");

        ret = sql_lexyacc.Parse("SELECT * FROM A");
        Check(ret == null || ret.ToString() == "");

        ret = sql_lexyacc.Parse("SELECT AGE, * FROM A");
        Check(ret == null || ret.ToString() == "");

        //ret = sql_lexyacc.Parse("SELECT AGE,  *, NAME FROM A");
        //Check(ret == null || ret.ToString() == "");
    }

}