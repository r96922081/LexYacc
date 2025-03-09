namespace MyDBNs
{
    public class DB
    {
        public List<Table> tables = new List<Table>();
        public bool inTransaction = false;
        public Stack<Action> transactionLog = new Stack<Action>();
    }
}
