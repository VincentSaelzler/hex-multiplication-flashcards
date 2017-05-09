using System;

namespace HexMultiplicationFlashCards
{
    class Program
    {
        private static string resp { get; set; }

        //variables and constants
        const string quitSig = "q";
        const string answerSig = "?";
        const string hexFormat = "X";

        static void Main(string[] args)
        {
            //control flow
            bool valueSelected = false;

            int maxMultiplier;
            const int defaultMaxMultiplier = 6;
            //const int defaultMaxMultiplicand = 6;
            FlashCardDeck flashCardDeck = null; //PERFECTION: function call to get deck and avoid kludge of FlashCardDeck flashCardDeck = null;

            //intro text
            Console.WriteLine("DIRECTIONS");
            Console.WriteLine("This program repeatedly asks multication problems.");
            Console.WriteLine($"Type your answer in hex format, and then push 'Enter'.");
            Console.WriteLine();
            Console.WriteLine("NOTES");
            Console.WriteLine($"Type '{quitSig}' and then push 'Enter' to exit the program.");
            Console.WriteLine($"Type '{answerSig}' and then push 'Enter' to display the answer without attempting a guess.");
            Console.WriteLine("The numbers in (parentheses) on the right are the decimal equivalent of the hex numbers on the left.");

            //TODO: allow for a non-square max selection
            //max range selection
            do
            {
                Console.WriteLine();
                Console.WriteLine($"What is the largest value you would like to see?");
                Console.WriteLine($"Push 'Enter' for default value of '{defaultMaxMultiplier.ToString(hexFormat)}'");

                resp = Console.ReadLine();

                //skip any further screen writing if given the quit signal
                if (resp != quitSig)
                {
                    valueSelected = false;

                    if (resp == string.Empty)
                    {
                        //use default value
                        flashCardDeck = new FlashCardDeck(defaultMaxMultiplier, defaultMaxMultiplier);//HACK: using multiplier for both args
                        valueSelected = true;
                        Console.WriteLine($"Using default value {flashCardDeck.maxMultiplier.ToString(hexFormat)}");
                    }
                    else
                    {
                        //parse entered response
                        if (int.TryParse(resp, System.Globalization.NumberStyles.HexNumber, null, out maxMultiplier)) //failed parses set maxAnswer = 0
                        {
                            //use parsed response
                            flashCardDeck = new FlashCardDeck(maxMultiplier, maxMultiplier); //HACK: using multiplier for both args
                            valueSelected = true;
                            Console.WriteLine($"Using entered value {flashCardDeck.maxMultiplier.ToString(hexFormat)}");
                        }
                        else
                        {
                            //display error
                            Console.WriteLine($"Enter a valid hex number, or enter '{quitSig}' to quit.");
                        }
                    }
                }
            } while (resp != quitSig && valueSelected == false); //we didn't (quit or enter "" or get a correctly parsed value)

            flashCardDeck.AskQuestions();

        }

    }
}
