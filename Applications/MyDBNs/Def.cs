namespace MyDBNs
{
    public class Gv
    {
        public static int sn = 0;
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

        public int GetColumnSize(string columnName)
        {
            return columnSizes[GetColumnIndex(columnName.ToUpper())];
        }
    }

    public class TableId
    {
        public string tableName;
        public string displayTableName;

        public TableId()
        {
        }

        public TableId(string tableName)
        {
            this.tableName = tableName;
            displayTableName = tableName;
        }

        public TableId(string tableName, string displayTableName) : this(tableName)
        {
            if (displayTableName != null && displayTableName != "")
                this.displayTableName = displayTableName;
        }
    }

    public class TableOrJoins
    {
        public TableId mainTableId;
        public List<TableId> allTableIds = new List<TableId>();
        public List<JoinTable> joins = new List<JoinTable>();

        public TableId GetTableIdByDisplayTable(string displayTable)
        {
            foreach (TableId tableId in allTableIds)
            {
                if (tableId.displayTableName == displayTable)
                    return tableId;
            }
            return null;
        }

        public List<TableId> GetTableIdsByColumnName(string columnName)
        {
            List<TableId> tableIds = new List<TableId>();
            foreach (TableId tableId in allTableIds)
            {
                Table t = Util.GetTable(tableId.tableName);

                if (t.columnNameToIndexMap.ContainsKey(columnName.ToUpper()))
                    tableIds.Add(tableId);
            }
            return tableIds;
        }

        public List<TableId> GetTableIdsByDisplayTableName(string displayTableName)
        {
            List<TableId> tableIds = new List<TableId>();
            foreach (TableId tableId in allTableIds)
            {
                if (tableId.displayTableName == displayTableName)
                    tableIds.Add(tableId);
            }
            return tableIds;
        }
    }

    public class JoinTable
    {
        public string joinType;
        public TableId rhsTableId;
        public string join_conditions;
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

    public class SelectedData2
    {
        public List<SelectedData> selectedData = new List<SelectedData>();

        public void Dispose()
        {
            foreach (SelectedData s in selectedData)
                s.Dispose();
        }
    }

    public class SelectedData : IDisposable
    {
        public Table table;
        public string displayTableName;

        public List<string> columnNames = new List<string>();
        public List<string> displayColumnNames = new List<string>();
        public List<int> columnIndex = new List<int>();
        public List<int> displayColumnIndex = new List<int>();

        public List<int> selectedRows = new List<int>();
        public bool needToDispose = false;

        public void Dispose()
        {
            if (needToDispose)
                Drop.DropTable(table.tableName);
        }
    }

    public class OrderBy
    {
        public OrderByDirection op;
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

    public class ColumnDeclare
    {
        public string columnName;
        public ColumnType type;
        public int size;
    }

    public enum OrderByDirection
    {
        ASEC,
        DESC
    }

    public class OrderByColumn
    {
        public object column;
        public OrderByDirection direction;
    }

    public enum AggerationOperation
    {
        MAX,
        MIN,
        COUNT,
        SUM,
        NONE
    }

    public class AggregationColumn
    {
        public string table;
        public string displayTableName;
        public string columnName;
        public string displayColumnName;
        public AggerationOperation op;
    }
}
