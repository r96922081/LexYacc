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

        public int GetColumnIndex(string searchColumnNameParam)
        {
            string searchTableName = null;
            string searchColumnName = null;

            string[] tokens = searchColumnNameParam.Split(".");
            if (tokens.Length == 2)
            {
                searchTableName = tokens[0];
                searchColumnName = tokens[1];
            }
            else
                searchColumnName = tokens[0];


            int index = -1;
            string prevColumnTableName = "";

            for (int i = 0; i < columns.Length; i++)
            {
                Column column = columns[i];
                if (column.tableName == null || column.tableName == "")
                    throw new Exception("Table name is not set for column " + column.columnName);

                if (searchTableName == null)
                {
                    if (column.columnName.ToUpper() == searchColumnName.ToUpper())
                    {
                        if (index != -1 && column.tableName.ToUpper() != prevColumnTableName.ToUpper())
                            throw new Exception("Ambiguous column name " + searchColumnName);
                        index = i;
                        prevColumnTableName = column.tableName;
                    }
                }
                else
                {
                    if (column.columnName.ToUpper() == searchColumnName.ToUpper() && column.tableName.ToUpper() == searchTableName.ToUpper())
                    {
                        index = i;
                    }
                }
            }

            if (index == -1)
                throw new Exception("Column " + searchColumnNameParam + " not found in table " + name);

            return index;
        }

        public ColumnType GetColumnType(string columnName)
        {
            return columns[GetColumnIndex(columnName)].type;
        }

        public int GetColumnSize(string columnName)
        {
            return columns[GetColumnIndex(columnName)].size;
        }
    }

    public class Column
    {
        public string tableName; // used in table join table to tell which table this column belongs to
        public string userColumnName;
        public string columnName; // upper cased
        public ColumnType type;
        public int size;

        public Column Clone()
        {
            Column c = new Column();
            c.tableName = tableName;
            c.userColumnName = userColumnName;
            c.columnName = columnName;
            c.type = type;
            c.size = size;
            return c;
        }
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
                    allTables.Add(joinTable.tableNameAlias);
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
        public TableNameAlias tableNameAlias;
        public string conditions;
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

    public class SelectedData : IDisposable
    {
        public Table table;

        public List<string> selectedColumnNames = new List<string>();
        public List<int> selectedColumnIndex = new List<int>();
        public List<string> customColumnNames = new List<string>();

        public List<int> selectedRows = new List<int>();

        public void Dispose()
        {
            if (table.name.StartsWith("TempTable"))
            {
                Drop.DropTable(table.name);
            }
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
