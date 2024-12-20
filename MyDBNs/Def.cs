namespace MyDBNs
{
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
    }

    public enum ColumnType
    {
        NUMBER,
        VARCHAR
    }

    public enum StringType
    {
        Column,
        String,
        Number
    }

    public class SelectedData
    {
        public Table table;
        public List<string> columnNames = new List<string>();
        public List<int> columnIndex = new List<int>();
        public List<int> selectedRows = new List<int>();
    }

    public class OrderBy
    {
        public bool ascending;
        public int selectColumnIndex;
    }

    public enum BooleanOperator
    {
        Equal,
        NotEqual,
        LessThan,
        GreaterThan,
        LessThanEqualTo,
        GreaterThanEqualTo
    }
}
