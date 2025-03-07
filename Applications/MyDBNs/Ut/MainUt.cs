namespace MyDBNs
{
    public class MainUt
    {
        public static void Ut()
        {
            Gv.ut = true;

            if (Gv.ut)
            {
                //new JoinUt().Ut();
                new SaveLoadUt().Ut();
                new CreateDropUt().Ut();
                new InsertUt().Ut();
                new SelectUt().Ut();
                new BooleanExpressionUt().Ut();
                new ArithmeticExpressionUt().Ut();
                new DeleteUt().Ut();
                new UpdateUt().Ut();
                new TransactionUt().Ut();
                new GroupByUt().Ut();
                Console.WriteLine("MyDB Ut Done!");

                //DBConsole.Interactive();
            }
            else
            {
                sql_statements.Parse("LOAD DB " + Path.Join(UtUtil.GetUtFileFolder(), "TEST_JOIN.DB"));
                InteractiveConsole.Interactive();
            }
        }

    }
}
