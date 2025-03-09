namespace MyDBNs
{
    public class Drop
    {
        public static void DropTable(string name)
        {
            for (int i = 0; i < Gv.db.tables.Count; i++)
            {
                if (Gv.db.tables[i].name.ToUpper() == name.ToUpper())
                {
                    Gv.db.tables.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
