namespace SqlNs
{
    public class PlayGround
    {
        public static void Play()
        {
            Console.WriteLine("input sql:\n\n");
            string line;
            while ((line = Console.ReadLine()) != null)
            {
                line = line.ToUpper();
                object result = sql_lexyacc.Parse(line);
                if (result != null && result.ToString() != "")
                    Console.WriteLine(result);

                Console.WriteLine("input sql:\n\n");
            }
        }
    }
}
