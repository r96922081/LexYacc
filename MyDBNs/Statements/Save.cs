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
                    writer.Write(table.tableName);

                    // Save column names
                    writer.Write(table.columnNames.Length);
                    foreach (var columnName in table.columnNames)
                        writer.Write(columnName);

                    // Save column types
                    writer.Write(table.columnTypes.Length);
                    foreach (var columnType in table.columnTypes)
                        writer.Write((int)columnType);

                    // Save column sizes
                    writer.Write(table.columnSizes.Length);
                    foreach (var columnSize in table.columnSizes)
                        writer.Write(columnSize);
                }
            }
        }
    }
}
