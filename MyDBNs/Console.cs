namespace MyDBNs
{
    public class Console
    {
        public static void Interactive()
        {
#if !MarkUserOfSqlCodeGen
            sql_statements.Parse("load db 1.txt");
            sql_statements.Parse("show tables");


            System.Console.WriteLine("input sql:\n\n");
            string line;
            while ((line = System.Console.ReadLine()) != null)
            {
                object result = null;

                try
                {
                    result = sql_statements.Parse(line);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Error occurred");
                }

                if (result != null && result.ToString() != "")
                    System.Console.WriteLine(result);

                System.Console.WriteLine("input sql:\n\n");
#endif
            }
        }
    }
}
