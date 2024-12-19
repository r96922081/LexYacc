﻿namespace MyDBNs
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
                    table.tableName = reader.ReadString();

                    // Load column names
                    int columnNameCount = reader.ReadInt32();
                    table.columnNames = new string[columnNameCount];
                    for (int j = 0; j < columnNameCount; j++)
                        table.columnNames[j] = reader.ReadString();

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

                    DB.tables.Add(table);
                }
            }
        }
    }
}