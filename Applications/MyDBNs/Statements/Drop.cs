namespace MyDBNs
{
    public class Drop
    {
        public static void DropTable(string name)
        {
            for (int i = 0; i < DB.tables.Count; i++)
            {
                if (DB.tables[i].name.ToUpper() == name.ToUpper())
                {
                    DB.tables.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
