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
                    table.name = reader.ReadString();

                    // Load column names
                    int columnCount = reader.ReadInt32();
                    table.columns = new Column[columnCount];

                    for (int j = 0; j < columnCount; j++)
                    {
                        table.columns[j] = new Column();
                        table.columns[j].columnName = reader.ReadString();
                        table.columns[j].tableName = table.name;
                    }

                    // Load column types
                    columnCount = reader.ReadInt32();
                    for (int j = 0; j < columnCount; j++)
                        table.columns[j].type = (ColumnType)reader.ReadInt32();

                    // Load column sizes
                    columnCount = reader.ReadInt32();
                    for (int j = 0; j < columnCount; j++)
                        table.columns[j].size = reader.ReadInt32();

                    // row count
                    int rowCount = reader.ReadInt32();
                    for (int j = 0; j < rowCount; j++)
                    {
                        object[] row = new object[columnCount];
                        table.rows.Add(row);

                        for (int k = 0; k < columnCount; k++)
                        {
                            ColumnType type = table.columns[k].type;
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
