namespace MyDBNs
{
    public class Transaction
    {
        public static void UndoInsert(Table t, object param1)
        {
            object[] row = (object[])param1;
            for (int i = 0; i < t.rows.Count; i++)
            {
                if (t.rows[i] == row)
                {
                    t.rows.Remove(row);
                    return;
                }
            }
        }

        public static void UndoDelete(Table t, object param1)
        {
            List<object[]> rows = (List<object[]>)param1;
            t.rows.AddRange(rows);
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
