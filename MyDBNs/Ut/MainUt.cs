﻿namespace MyDBNs
{
    public class MainUt
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
                new DeleteUt().Ut();
                new UpdateUt().Ut();
                new SaveLoadUt().Ut();
                Console.WriteLine("Ut Done!");
            }
            else
            {
                DBConsole.Interactive();
            }
        }

    }
}