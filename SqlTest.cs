using System.Diagnostics;

public class SqlYaccData
{
    public static List<string> columnNames;
    public static List<ColumnType> columnTypes;
    public static Table table;

    public static void Reset()
    {
        columnNames = new List<string>();
        columnTypes = new List<ColumnType>();
        table = null;
    }
}


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
    public Dictionary<string, int> columnIndexMap;
    public Dictionary<string, ColumnType> columnTypesMap;

    public List<object[]> rows = new List<object[]>();

    public void Insert(object[] row)
    {
        rows.Add(row);
    }

}

public class DB
{
    public static List<Table> tables = new List<Table>();
}


public class SqlTest
{
    public static void Check(bool b)
    {
        if (!b)
            Trace.Assert(false);
    }

    public static void CreateTable(string name, List<string> columnNames, List<ColumnType> columnTypes)
    {
        Table table = new Table();
        table.tableName = name;
        table.columnNames = columnNames.ToArray();
        table.columnTypes = columnTypes.ToArray();
        table.columnIndexMap = new Dictionary<string, int>();
        table.columnTypesMap = new Dictionary<string, ColumnType>();

        Array.Reverse(table.columnNames);
        Array.Reverse(table.columnTypes);

        for (int i = 0; i < table.columnNames.Length; i++)
        {
            table.columnIndexMap.Add(table.columnNames[i], i);
            table.columnTypesMap.Add(table.columnNames[i], table.columnTypes[i]);
        }

        DB.tables.Add(table);
    }

    public class DB
    {
        public static List<Table> tables = new List<Table>();
    }


    public static void VerifyInsertRow(string tableName, string columnNameString, string valueString)
    {
        List<string> columnNames = null;
        if (columnNameString != null)
            columnNames = columnNameString.Split(",").ToList();

        List<string> values = null;
        if (valueString != null)
            values = valueString.Split(",").ToList();


        Table table = DB.tables.FirstOrDefault(t => t.tableName == tableName);
        if (table == null)
            throw new Exception("no table named: " + tableName);

        if (columnNames == null)
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

    public static void InsertRow(string tableName, string columnNameString, string valueString)
    {
        VerifyInsertRow(tableName, columnNameString, valueString);

        List<string> columnNames = null;
        if (columnNameString != null)
            columnNames = columnNameString.Split(",").ToList();

        List<string> values = null;
        if (valueString != null)
            values = valueString.Split(",").ToList();

        Table table = DB.tables.FirstOrDefault(t => t.tableName == tableName);
        if (columnNames == null)
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


    public static void test()
    {
        SqlYaccData.Reset();
        object ret = sql_lexyacc.Parse("CREATE TABLE A ( NAME VARCHAR, AGE NUMBER)");
        Check(ret == null || ret.ToString() == "");

        ret = sql_lexyacc.Parse("INSERT INTO A ( NAME, AGE ) VALUES ( DEF, 33  )");
        Check(ret == null || ret.ToString() == "");

        ret = sql_lexyacc.Parse("INSERT INTO A VALUES ( 44, 55  )");
        Check(ret == null || ret.ToString() == "");

        ret = sql_lexyacc.Parse("INSERT INTO A ( AGE, NAME ) VALUES ( 66, ABC  )");
        Check(ret == null || ret.ToString() == "");

        ret = sql_lexyacc.Parse("INSERT INTO A ( AGE ) VALUES ( 999  )");
        Check(ret == null || ret.ToString() == "");

        //SqlYaccData.Reset();
        //Check(sql_lexyacc.Parse("CREATE TABLE A ( aaa VARCHAR, bbb NUMBER)").ToString() == "");


        // INSERT INTO A (aaa, bbb) VALUES (AAA, BB)
        // INSERT INTO A (bbb) VALUES (BB)
        // INSERT INTO A VALUES (AAA, BB)

        List<object> row = new List<object>();
        row.Add("aaa");
        row.Add(2);

        InsertRow("A", null, "888,333");
    }

}