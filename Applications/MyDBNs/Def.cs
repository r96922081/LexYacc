namespace MyDBNs
{
    public class Gv
    {
        public static bool ut = false;
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

        public int GetColumnIndex(string columnName)
        {
            return columnNameToIndexMap[columnName.ToUpper()];
        }

        public ColumnType GetColumnType(string columnName)
        {
            return columnNameToTypesMap[columnName.ToUpper()];
        }
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

    public class SetExpressionType
    {
        public string lhsColumn;
        public StringType rhsType;
        public string rhs;

        public SetExpressionType(string lhsColumn, StringType rhsType, string rhs)
        {
            this.lhsColumn = lhsColumn;
            this.rhsType = rhsType;
            this.rhs = rhs;
        }
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

    public class UndoUpdateData
    {
        public object[] row;
        public int columnIndex;
        public object value;

        public UndoUpdateData(object[] row, int columnIndex, object value)
        {
            this.row = row;
            this.columnIndex = columnIndex;
            this.value = value;
        }
    }

    public enum AggerationOperation
    {
        MAX,
        MIN,
        COUNT,
        SUM
    }

    public class AggregationColumn
    {
        public string columnName;
        public AggerationOperation op;
    }
}
