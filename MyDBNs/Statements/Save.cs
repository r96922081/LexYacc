namespace MyDBNs
{
    public class Save
    {
        public static void SaveDB(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            using (var writer = new BinaryWriter(stream))
            {
                // Write the number of tables
                writer.Write(DB.tables.Count);

                foreach (var table in DB.tables)
                {
                    // Save table name
                    writer.Write(table.originalTableName);

                    // Save column names
                    writer.Write(table.originalColumnNames.Length);
                    foreach (var columnName in table.originalColumnNames)
                        writer.Write(columnName);

                    // Save column types
                    writer.Write(table.columnTypes.Length);
                    foreach (var columnType in table.columnTypes)
                        writer.Write((int)columnType);

                    // Save column sizes
                    writer.Write(table.columnSizes.Length);
                    foreach (var columnSize in table.columnSizes)
                        writer.Write(columnSize);

                    // row count
                    writer.Write(table.rows.Count);
                    foreach (var row in table.rows)
                    {
                        for (int i = 0; i < table.columnTypes.Length; i++)
                        {
                            ColumnType type = table.columnTypes[i];
                            object value = row[i];
                            if (value == null)
                            {
                                writer.Write(false);
                            }
                            else
                            {
                                writer.Write(true);
                                if (type == ColumnType.NUMBER)
                                    writer.Write(((double)value));
                                else
                                    writer.Write(((string)value));
                            }
                        }
                    }
                }
            }
        }
    }
}
