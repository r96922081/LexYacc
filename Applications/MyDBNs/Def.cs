namespace MyDBNs
{
    public class Gv
    {
        public static int sn = 0;
        public static bool ut = false;
    }

    public class Table
    {
        public string name;
        public string originaName;
        public Column[] columns;
        public List<object[]> rows = new List<object[]>();

        public int GetColumnIndex(string columnNameParam)
        {
            string queryTableName = null;
            string columnName = null;

            string[] tokens = columnNameParam.Split(".");
            if (tokens.Length == 2)
            {
                queryTableName = tokens[0];
                columnName = tokens[1];
            }
            else
                columnName = tokens[0];


            int index = -1;

            for (int i = 0; i < columns.Length; i++)
            {
                Column column = columns[i];

                if (queryTableName == null)
                {
                    if (column.columnName == columnName.ToUpper())
                    {
                        if (index != -1)
                            throw new Exception("Ambiguous column name " + columnName);
                        index = i;
                    }
                }
                else
                {
                    if (column.userTableName == queryTableName && column.columnName == columnName.ToUpper())
                    {
                        if (index != -1)
                            throw new Exception("Ambiguous column name " + columnName);
                        index = i;
                    }
                }

            }

            return index;
        }

        public ColumnType GetColumnType(string columnName)
        {
            foreach (Column c in columns)
            {
                if (c.columnName == columnName.ToUpper())
                    return c.type;
            }

            return ColumnType.INVALID;
        }

        public int GetColumnSize(string columnName)
        {
            foreach (Column c in columns)
            {
                if (c.columnName == columnName.ToUpper())
                    return c.size;
            }

            return -1;
        }
    }

    public class Column
    {
        public string userTableName;
        public string userColumnName;
        public string columnName; // upper cased
        public string originalColumnName;
        public ColumnType type;
        public int size;
    }

    public class TableId
    {
        public string tableName;
        public string userTableName;

        public TableId()
        {
        }

        public TableId(string tableName)
        {
            this.tableName = tableName;
            userTableName = tableName;
        }

        public TableId(string tableName, string displayTableName) : this(tableName)
        {
            if (displayTableName != null && displayTableName != "")
                this.userTableName = displayTableName;
        }
    }

    public class TableOrJoins
    {
        public TableId mainTableId;
        public List<TableId> allTableIds = new List<TableId>();
        public List<JoinTable> joins = new List<JoinTable>();

        public TableId GetTableIdByDisplayTableName(string displayTable)
        {
            foreach (TableId tableId in allTableIds)
            {
                if (tableId.userTableName == displayTable)
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

                foreach (Column c in t.columns)
                {
                    if (c.columnName == columnName.ToUpper())
                    {
                        tableIds.Add(tableId);
                    }
                }
            }
            return tableIds;
        }

        public List<TableId> GetTableIdsByDisplayTableName(string displayTableName)
        {
            List<TableId> tableIds = new List<TableId>();
            foreach (TableId tableId in allTableIds)
            {
                if (tableId.userTableName == displayTableName)
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
        VARCHAR,
        INVALID
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
        public string userTableName;

        public List<string> columnNames = new List<string>();
        public List<string> userColumnNames = new List<string>();
        public List<int> columnIndex = new List<int>();
        public List<int> userColumnIndex = new List<int>();

        public List<int> selectedRows = new List<int>();
        public bool needToDispose = false;

        public void Dispose()
        {
            if (needToDispose)
                Drop.DropTable(table.name);
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
        public string userTableName;
        public string columnName;
        public string userColumnName;
        public AggerationOperation op;
    }
}
