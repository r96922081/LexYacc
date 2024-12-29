namespace MyDBNs
{
    public class DB
    {
        public static List<Table> tables = new List<Table>();
        public static bool inTransaction = false;
        public static Stack<Action> transactionLog = new Stack<Action>();
    }
}
