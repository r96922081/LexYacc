namespace MyDBNs
{
    public class MainUt
    {
        public static void Ut()
        {
            Gv.ut = true;

            if (Gv.ut)
            {
                new CreateDropUt().Ut();
                new InsertUt().Ut();
                new SelectUt().Ut();
                new BooleanExpressionUt().Ut();
                new ArithmeticExpressionUt().Ut();
                new DeleteUt().Ut();
                new UpdateUt().Ut();
                new SaveLoadUt().Ut();
                new TransactionUt().Ut();
                Console.WriteLine("Ut Done!");
            }
            else
            {
                DBConsole.Interactive();
            }
        }

    }
}
