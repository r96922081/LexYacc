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
                    if (column.columnName.ToUpper() == columnName.ToUpper())
                    {
                        if (index != -1)
                            throw new Exception("Ambiguous column name " + columnName);
                        index = i;
                    }
                }
                else
                {
                    if (column.columnName.ToUpper() == columnName.ToUpper())
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
        public string userColumnName;
        public string columnName; // upper cased
        public string originalColumnName;
        public ColumnType type;
        public int size;
    }

    public class TableNameAlias
    {
        public string targetTableName;
        public string aliasTableName;

        public TableNameAlias()
        {
        }

        public TableNameAlias(string tableName)
        {
            targetTableName = tableName;
            aliasTableName = tableName;
        }

        public TableNameAlias(string tableName, string aliasTableName)
        {
            targetTableName = tableName;

            if (aliasTableName == null || aliasTableName == "")
                aliasTableName = tableName;
            else
                this.aliasTableName = aliasTableName;
        }
    }

    public class Tables
    {
        public TableNameAlias mainTable;
        public List<JoinTable> joinTables = new List<JoinTable>();

        public List<TableNameAlias> GetAllTables()
        {
            List<TableNameAlias> allTables = new List<TableNameAlias>();
            allTables.Add(mainTable);

            if (joinTables != null)
            {
                foreach (JoinTable joinTable in joinTables)
                    allTables.Add(joinTable.rhsTableId);
            }

            return allTables;
        }

        public List<TableNameAlias> GetTablesByColumnName(string columnName)
        {
            List<TableNameAlias> tables = new List<TableNameAlias>();

            foreach (TableNameAlias table in GetAllTables())
            {
                Table t = Util.GetTable(table.targetTableName);

                foreach (Column c in t.columns)
                {
                    if (c.columnName.ToUpper() == columnName.ToUpper())
                    {
                        tables.Add(table);
                    }
                }
            }
            return tables;
        }

        public List<TableNameAlias> GetTablesByTableName(string tableName)
        {
            List<TableNameAlias> tableIds = new List<TableNameAlias>();
            foreach (TableNameAlias tableId in GetAllTables())
            {
                if (tableId.aliasTableName.ToUpper() == tableName.ToUpper())
                    tableIds.Add(tableId);
            }
            return tableIds;
        }
    }

    public class JoinTable
    {
        public string joinType;
        public TableNameAlias rhsTableId;
        public string joinConditions;
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

        public List<string> selectedColumnNames = new List<string>();
        public List<int> selectedColumnIndex = new List<int>();

        public List<string> customColumnNames = new List<string>();
        public List<int> customColumnIndex = new List<int>();

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
        public string tableName;
        public string columnName;
        public string customColumnName;
        public AggerationOperation op;
    }
}
