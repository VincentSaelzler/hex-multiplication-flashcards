using System;

namespace HexMultiplicationFlashCards
{
    class Program
    {
        private static string resp { get; set; }

        static void Main(string[] args)
        {
            const int defaultMinValue = 3;  //arbitrary choice
            const int defaultMaxValue = 7;  //arbitrary choice

            //display intro
            FlashCardDeck.ShowIntroduction();

            //get maximum factors
            Console.WriteLine();
            Console.WriteLine("Choose the Minimum Multiplier");
            int minMultiplier = FlashCardDeck.ParseIntValue(defaultMinValue);
            Console.WriteLine();
            Console.WriteLine("Choose the Minimum Multiplicand");
            int minMultiplicand = FlashCardDeck.ParseIntValue(defaultMinValue);
            Console.WriteLine();
            Console.WriteLine("Choose the Max Multiplier");
            int maxMultiplier = FlashCardDeck.ParseIntValue(defaultMaxValue);
            Console.WriteLine();
            Console.WriteLine("Choose the Max Multiplicand");
            int maxMultiplicand = FlashCardDeck.ParseIntValue(defaultMaxValue);

            //instantiate deck
            FlashCardDeck flashCardDeck = new FlashCardDeck(minMultiplier, minMultiplicand, maxMultiplier, maxMultiplicand);

            //run procedures
            while (flashCardDeck.CardsAvailable)
            {
                flashCardDeck.AskQuestions();
                flashCardDeck.SummarizeResults();
                flashCardDeck.MoveWrongToUnanswered();
            }
        }

    }
}
