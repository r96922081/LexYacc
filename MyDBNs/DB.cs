using System.Text;

namespace MyDBNs
{
    public class DB
    {
        public static List<Table> tables = new List<Table>();

        public static Table GetTable(string tableName)
        {
            return tables.FirstOrDefault(t => t.tableName.ToUpper() == tableName.ToUpper());
        }

        public static void CreateTable(string name, List<(string, string)> columnDeclare)
        {
            DBVerifier.VerifyCreateTable(name, columnDeclare);

            Table table = new Table();
            table.originalTableName = name;
            table.tableName = name.ToUpper();
            table.columnNames = new string[columnDeclare.Count];
            table.originalColumnNames = new string[columnDeclare.Count];
            table.columnTypes = new ColumnType[columnDeclare.Count];
            table.columnSizes = new int[columnDeclare.Count];

            for (int i = 0; i < columnDeclare.Count; i++)
            {
                table.originalColumnNames[i] = columnDeclare[i].Item1;
                table.columnNames[i] = columnDeclare[i].Item1.ToUpper();
                string columnType = columnDeclare[i].Item2.ToUpper();
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


            table.columnNameToIndexMap = new Dictionary<string, int>();
            table.columnNameToTypesMap = new Dictionary<string, ColumnType>();

            for (int i = 0; i < table.columnNames.Length; i++)
            {
                table.columnNameToIndexMap.Add(table.columnNames[i], i);
                table.columnNameToTypesMap.Add(table.columnNames[i], table.columnTypes[i]);
            }

            tables.Add(table);
        }

        public static void ShowTables()
        {
            foreach (Table table in DB.tables)
                System.Console.WriteLine(table.GetSchema());
        }

        public static void Insert(string tableName, List<string> columnNames, List<string> values)
        {
            tableName = tableName.ToUpper();

            if (columnNames != null)
                columnNames = columnNames.Select(name => name.ToUpper()).ToList();

            DBVerifier.VerifyInsert(tableName, columnNames, values);

            Table table = GetTable(tableName);

            if (columnNames == null || columnNames.Count == 0)
                columnNames = table.columnNames.ToList();

            object[] rows = new object[table.columnNames.Length];

            for (int i = 0; i < values.Count; i++)
            {
                string columnName = columnNames[i];
                int columnIndex = table.columnNameToIndexMap[columnName];
                ColumnType columnType = table.columnNameToTypesMap[columnName];
                string value = values[i];
                if (value == null)
                {
                    rows[columnIndex] = null;
                }
                else
                {
                    if (columnType == ColumnType.NUMBER)
                    {
                        rows[columnIndex] = double.Parse(value.ToString());
                    }
                    else if (columnType == ColumnType.VARCHAR)
                    {
                        // remove ' ' 
                        string s = value.ToString();
                        if (s.StartsWith("'") && s.EndsWith("'"))
                            s = s.Substring(1, value.Length - 2);

                        rows[columnIndex] = s;
                    }
                }
            }

            table.rows.Add(rows);
        }

        public static void Delete(string tableName, string condition)
        {
            tableName = tableName.ToUpper();

#if !MarkUserOfSqlCodeGen
            SqlConditionLexYaccCallback.tableName = tableName;
            object ret = sql_condition_lexyacc.Parse(condition);
            HashSet<int> rows = (HashSet<int>)ret;

            Table table = MyDBNs.DB.GetTable(tableName);
            for (int i = table.rows.Count - 1; i >= 0; i--)
            {
                if (rows.Contains(i))
                    table.rows.RemoveAt(i);
            }
#endif
        }

        public static void Update(string tableName, List<Tuple<string, string>> setExpression, string condition)
        {
            tableName = tableName.ToUpper();
            setExpression = setExpression.Select(tuple => Tuple.Create(tuple.Item1.ToUpper(), tuple.Item2)).ToList();

#if !MarkUserOfSqlCodeGen
            SqlConditionLexYaccCallback.tableName = tableName;
            object ret = sql_condition_lexyacc.Parse(condition);
            HashSet<int> rows = (HashSet<int>)ret;

            Table table = GetTable(tableName);
            for (int i = 0; i < table.rows.Count - 1; i++)
            {

            }
#endif
        }

        public static void Select(List<string> columns, string tableName, string condition)
        {
            tableName = tableName.ToUpper();

#if !MarkUserOfSqlCodeGen
            Table table = GetTable(tableName);

            SqlConditionLexYaccCallback.tableName = tableName;
            object ret = sql_condition_lexyacc.Parse(condition);
            HashSet<int> selectedRows = (HashSet<int>)ret;

            List<string> columnNames = new List<string>();
            List<int> columnIndex = new List<int>();
            foreach (string column in columns)
            {
                if (column == "*")
                {
                    foreach (string column2 in table.columnNames)
                    {
                        columnNames.Add(column2);
                        columnIndex.Add(table.columnNameToIndexMap[column2]);
                    }
                }
                else
                {
                    columnNames.Add(column);
                    columnIndex.Add(table.columnNameToIndexMap[column]);
                }
            }

            System.Console.WriteLine("table: " + table.tableName);

            int[] columnWidths = new int[columnNames.Count];
            for (int i = 0; i < columnIndex.Count; i++)
            {
                columnWidths[i] = columnNames[i].Length;
            }

            // get column width
            foreach (object[] row in table.rows)
            {
                for (int i = 0; i < columnIndex.Count; i++)
                {
                    if (row[columnIndex[i]] == null)
                        continue;

                    int cellLength = row[columnIndex[i]].ToString().Length;
                    if (cellLength > columnWidths[i])
                    {
                        columnWidths[i] = cellLength;
                    }
                }
            }

            // show column name
            System.Console.Write("| ");
            for (int i = 0; i < columnNames.Count; i++)
            {
                System.Console.Write(columnNames[i].PadRight(columnWidths[i]));
                System.Console.Write(" | ");
            }
            System.Console.WriteLine();

            // show cell
            for (int rowIndex = 0; rowIndex < table.rows.Count; rowIndex++)
            {
                if (!selectedRows.Contains(rowIndex))
                    continue;


                object[] row = table.rows[rowIndex];
                System.Console.Write("| ");
                int j = 0;
                foreach (int i in columnIndex)
                {
                    if (row[i] == null)
                        System.Console.Write("".PadRight(columnWidths[j]));
                    else
                        System.Console.Write(row[i].ToString().PadRight(columnWidths[j]));
                    j++;
                    System.Console.Write(" | ");
                }
                System.Console.WriteLine();
            }
#endif
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
        public int[] columnSizes;

        public Dictionary<string, int> columnNameToIndexMap;
        public Dictionary<string, ColumnType> columnNameToTypesMap;

        public string originalTableName;
        public string[] originalColumnNames;

        public List<object[]> rows = new List<object[]>();

        public void Insert(object[] row)
        {
            rows.Add(row);
        }

        public string GetSchema()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("table: " + originalTableName);
            for (int i = 0; i < originalColumnNames.Length; i++)
            {
                sb.Append(originalColumnNames[i] + " " + columnTypes[i]);
                if (columnTypes[i] == ColumnType.VARCHAR)
                {
                    sb.Append("(" + columnSizes[i] + ")");
                }

                if (i != originalColumnNames.Length - 1)
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
}
