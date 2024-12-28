namespace MyDBNs
{
    public class SqlUt
    {
        public static void Ut()
        {
            Gv.ut = false;

            if (Gv.ut)
            {
                new CreateDropUt().Ut();
                new InsertUt().Ut();
                new SelectUt().Ut();
                new BooleanExpressionUt().Ut();
                new ArithmeticExpressionUt().Ut();
                new SaveLoadUt().Ut();
            }
            else
            {
                Console.Interactive();
            }
        }

    }
}
