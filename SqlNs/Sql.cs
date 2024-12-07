using System.Text;

namespace SqlNs
{

    public class DB
    {
        public static List<Table> tables = new List<Table>();
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



}