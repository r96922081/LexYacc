namespace MyDBNs
{
    public class Transaction
    {
        public static void UndoInsert(Table t, object[] row)
        {
            for (int i = 0; i < t.rows.Count; i++)
            {
                if (t.rows[i] == row)
                {
                    t.rows.Remove(row);
                    return;
                }
            }
        }

        public static void UndoDelete(Table t, List<object[]> rows)
        {
            t.rows.AddRange(rows);
        }

        public static void UndoUpdate(Stack<UndoUpdateData> undos)
        {
            while (undos.Count > 0)
            {
                UndoUpdateData data = undos.Pop();
                data.row[data.columnIndex] = data.value;
            }
        }

        public static string TransactionStart()
        {
            DB.inTransaction = true;

            return "start transaction";
        }

        public static int Commit()
        {
            int count = DB.transactionLog.Count;
            DB.transactionLog.Clear();
            DB.inTransaction = false;

            return count;
        }

        public static int Rollback()
        {
            int count = DB.transactionLog.Count;

            while (DB.transactionLog.Count > 0)
            {
                Action action = DB.transactionLog.Pop();
                action();
            }

            DB.inTransaction = false;

            return count;
        }
    }
}
