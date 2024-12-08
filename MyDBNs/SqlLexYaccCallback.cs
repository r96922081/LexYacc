namespace MyDBNs
{
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
            MyDBNs.DB.CreateTable(name, columnDeclare);
        }

        public static void ShowTables()
        {
            foreach (Table table in DB.tables)
                System.Console.WriteLine(table.GetSchema());
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

            System.Console.WriteLine(t.GetData());
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
        public static void VerifyInsert(string tableName, List<string> columnNames, List<string> values)
        {
            Table table = MyDBNs.DB.GetTable(tableName);
            if (table == null)
                throw new Exception("no table named: " + tableName);

            if (columnNames == null || columnNames.Count == 0)
                columnNames = table.columnNames.ToList();

            if (columnNames.Count != values.Count)
                throw new Exception("column count != value count");

            foreach (string columnName in columnNames)
            {
                if (!table.columnNameToIndexMap.ContainsKey(columnName))
                    throw new Exception("no column named: " + columnName);
            }

            for (int i = 0; i < values.Count; i++)
            {
                string columnName = columnNames[i];
                ColumnType columnType = table.columnNameToTypesMap[columnName];
                string value = values[i];
                if (value == null)
                    continue;

                if (columnType == ColumnType.NUMBER)
                {
                    double result;
                    if (double.TryParse(value.ToString(), out result) == false)
                        throw new Exception("Invalid input = " + value + ", type of column " + columnName + " is " + columnType);
                }
            }
        }

        public static void Insert(string tableName, List<string> columnNames, List<string> values)
        {
            VerifyInsert(tableName, columnNames, values);

            Table table = MyDBNs.DB.GetTable(tableName);

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

        public static void Select(List<string> columns, string tableName, string condition)
        {
#if !MarkUserOfSqlCodeGen
            Table table = MyDBNs.DB.GetTable(tableName);

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

        public static void BooleanExpression1(ref string booleanExpression, string rhs)
        {
            booleanExpression = " ( " + rhs + " ) ";
        }

        public static void BooleanExpression2(ref string booleanExpression, string lhs, string op, string rhs)
        {
            booleanExpression = lhs + " " + op + " " + rhs;
        }
    }
}
