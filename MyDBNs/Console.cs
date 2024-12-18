namespace MyDBNs
{
    public class Console
    {
        public static void Interactive()
        {
#if !MarkUserOfSqlCodeGen
            System.Console.WriteLine("input sql:\n\n");
            string line;
            while ((line = System.Console.ReadLine()) != null)
            {
                object result = sql_lexyacc.Parse(line);
                if (result != null && result.ToString() != "")
                    System.Console.WriteLine(result);

                System.Console.WriteLine("input sql:\n\n");
            }
#endif
        }
    }
}
