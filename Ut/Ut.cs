public class Ut
{
    public static void RunAllUt()
    {
        DateTime start = DateTime.Now;

        YaccUt.RunAllUt();
        LexUt.RunAllUt();
        LexYaccUt.RunAllUt();

        MyDBNs.MainUt.Ut();
        CCompilerNs.MainUt.Ut();

        Console.WriteLine(string.Format("UT took {0} seconds", (int)((DateTime.Now - start).TotalSeconds)));
    }
}
