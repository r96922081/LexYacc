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


            table.columnNameToIndexMap = new Dictionary<string, int>();
            table.columnNameToTypesMap = new Dictionary<string, ColumnType>();

            for (int i = 0; i < table.columnNames.Length; i++)
            {
                table.columnNameToIndexMap.Add(table.columnNames[i], i);
                table.columnNameToTypesMap.Add(table.columnNames[i], table.columnTypes[i]);
            }

            SetUpperCase(table);
            tables.Add(table);
        }


        private static void SetUpperCase(Table table)
        {
            table.upperCaseTableName = table.tableName.ToUpper();
            table.upperCaseColumnNames = new string[table.columnNames.Length];

            for (int i = 0; i < table.columnNames.Length; i++)
                table.upperCaseColumnNames[i] = table.columnNames[i].ToUpper();

            foreach (var kv in table.columnNameToIndexMap)
                if (!table.columnNameToIndexMap.ContainsKey(kv.Key.ToUpper()))
                    table.columnNameToIndexMap.Add(kv.Key.ToUpper(), kv.Value);

            foreach (var kv in table.columnNameToTypesMap)
                if (!table.columnNameToTypesMap.ContainsKey(kv.Key.ToUpper()))
                    table.columnNameToTypesMap.Add(kv.Key.ToUpper(), kv.Value);
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

        public string upperCaseTableName;
        public string[] upperCaseColumnNames;

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
}
