namespace MyDBNs
{
    public class SqlUt
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
            }
            else
            {
                Console.Interactive();
            }
        }

    }
}
