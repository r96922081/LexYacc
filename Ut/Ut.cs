public class Ut
{
    public static void RunAllUt()
    {
        YaccUt.RunAllUt();
        LexUt.RunAllUt();
        LexYaccUt.RunAllUt();


        MyDBNs.MainUt.Ut();
        CCompilerNs.MainUt.Ut();
    }
}
