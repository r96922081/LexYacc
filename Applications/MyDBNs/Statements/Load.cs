namespace MyDBNs
{
    public class Load
    {
        public static void LoadDB(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (var reader = new BinaryReader(stream))
            {
                // Clear existing tables
                DB.tables.Clear();

                // Read the number of tables
                int tableCount = reader.ReadInt32();

                for (int i = 0; i < tableCount; i++)
                {
                    var table = new Table();

                    // Load table name
                    table.originalTableName = reader.ReadString();
                    table.tableName = table.originalTableName.ToUpper();

                    // Load column names
                    int columnNameCount = reader.ReadInt32();
                    table.originalColumnNames = new string[columnNameCount];
                    table.columnNames = new string[columnNameCount];

                    for (int j = 0; j < columnNameCount; j++)
                    {
                        table.originalColumnNames[j] = reader.ReadString();
                        table.columnNames[j] = table.originalColumnNames[j].ToUpper();
                    }

                    // Load column types
                    int columnTypeCount = reader.ReadInt32();
                    table.columnTypes = new ColumnType[columnTypeCount];
                    for (int j = 0; j < columnTypeCount; j++)
                        table.columnTypes[j] = (ColumnType)reader.ReadInt32();

                    // Load column sizes
                    int columnSizeCount = reader.ReadInt32();
                    table.columnSizes = new int[columnSizeCount];
                    for (int j = 0; j < columnSizeCount; j++)
                        table.columnSizes[j] = reader.ReadInt32();

                    table.columnNameToIndexMap = new Dictionary<string, int>();
                    table.columnNameToTypesMap = new Dictionary<string, ColumnType>();

                    for (int j = 0; j < table.columnNames.Length; j++)
                    {
                        table.columnNameToIndexMap.Add(table.columnNames[j], j);
                        table.columnNameToTypesMap.Add(table.columnNames[j], table.columnTypes[j]);
                    }

                    // row count
                    int rowCount = reader.ReadInt32();
                    for (int j = 0; j < rowCount; j++)
                    {
                        object[] row = new object[table.columnTypes.Length];
                        table.rows.Add(row);

                        for (int k = 0; k < table.columnTypes.Length; k++)
                        {
                            ColumnType type = table.columnTypes[k];
                            bool hasValue = reader.ReadBoolean();
                            if (hasValue)
                            {
                                if (type == ColumnType.NUMBER)
                                    row[k] = reader.ReadDouble();
                                else
                                    row[k] = reader.ReadString();
                            }
                        }
                    }

                    DB.tables.Add(table);
                }
            }
        }
    }
}
