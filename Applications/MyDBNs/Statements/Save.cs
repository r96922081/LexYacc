﻿namespace MyDBNs
{
    public class Save
    {
        public static void SaveDB(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            using (var writer = new BinaryWriter(stream))
            {
                // Write the number of tables
                writer.Write(Gv.db.tables.Count);

                foreach (var table in Gv.db.tables)
                {
                    // Save table name
                    writer.Write(table.originaName);

                    // Save column names
                    writer.Write(table.columns.Length);
                    foreach (Column column in table.columns)
                        writer.Write(column.originalColumnName);

                    // Save column types
                    writer.Write(table.columns.Length);
                    foreach (Column column in table.columns)
                        writer.Write((int)column.type);

                    // Save column sizes
                    writer.Write(table.columns.Length);
                    foreach (Column column in table.columns)
                        writer.Write(column.size);

                    // row count
                    writer.Write(table.rows.Count);
                    foreach (var row in table.rows)
                    {
                        for (int i = 0; i < table.columns.Length; i++)
                        {
                            ColumnType type = table.columns[i].type;
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
