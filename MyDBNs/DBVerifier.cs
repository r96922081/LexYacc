﻿namespace MyDBNs
{
    public class DBVerifier
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
    }
}
