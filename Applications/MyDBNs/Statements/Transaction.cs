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
            Gv.db.inTransaction = true;

            return "start transaction";
        }

        public static int Commit()
        {
            int count = Gv.db.transactionLog.Count;
            Gv.db.transactionLog.Clear();
            Gv.db.inTransaction = false;

            return count;
        }

        public static int Rollback()
        {
            int count = Gv.db.transactionLog.Count;

            while (Gv.db.transactionLog.Count > 0)
            {
                Action action = Gv.db.transactionLog.Pop();
                action();
            }

            Gv.db.inTransaction = false;

            return count;
        }
    }
}
