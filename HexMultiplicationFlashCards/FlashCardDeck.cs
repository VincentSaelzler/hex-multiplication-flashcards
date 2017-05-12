using System;
using System.Collections.Generic;

namespace HexMultiplicationFlashCards
{
    class FlashCardDeck
    {
        //REFACTOR: put these somewhere else
        const string hexFormat = "X";
        const string percentFormat = "P";
        const string answerSig = "?";

        //REFACTOR: change all Console.WriteLine calls to just return strings

        //properties
        private int minMultiplier { get; set; }
        private int minMultiplicand { get; set; }
        private int maxMultiplier { get; set; }
        private int maxMultiplicand { get; set; }
        private Stack<FlashCard> unansweredCards { get; set; }
        private Stack<FlashCard> wrongCards { get; set; }
        private Stack<FlashCard> rightCards { get; set; }
        private IList<FlashCard> cards { get; set; }

        public bool CardsAvailable
        {
            get { return (unansweredCards.Count > 0); }
        }

        //public functions
        public FlashCardDeck(int minMultiplier, int minMultiplicand, int maxMultiplier, int maxMultiplicand)
        {
            //set properties
            this.minMultiplier = minMultiplier;
            this.minMultiplicand = minMultiplicand;
            this.maxMultiplier = maxMultiplier;
            this.maxMultiplicand = maxMultiplicand;

            cards = new List<FlashCard>();
            unansweredCards = new Stack<FlashCard>();
            wrongCards = new Stack<FlashCard>();
            rightCards = new Stack<FlashCard>();

            //populate card list
            for (int multiplier = minMultiplier; multiplier <= maxMultiplier; multiplier++)
            {
                for (int multiplicand = minMultiplicand; multiplicand <= maxMultiplicand; multiplicand++)
                {
                    cards.Add(new FlashCard(multiplier, multiplicand));
                }
            }

            //transfer whole list to unanswered stack
            foreach (FlashCard fc in cards) 
            {
                //SIDENOTE: I was originally going to try and use LINQ instead of a foreach loop,
                //but this MSDN article
                // https://blogs.msdn.microsoft.com/ericlippert/2009/05/18/foreach-vs-foreach/
                //convinced me it was a bad idea
                unansweredCards.Push(fc);
            }
        }
        public void AskQuestions()
        {
            //main question ask/answer loop
            while (unansweredCards.Count > 0)
            {
                //containers
                int multResp;
                string resp;

                //display question
                Console.WriteLine();
                Console.WriteLine($"{unansweredCards.Count.ToString(hexFormat)} UNANSWERED CARDS REMAINING");
                Prompt();

                //gather response
                resp = Console.ReadLine();

                if (resp == answerSig)
                {
                    ShowAnswer();
                }
                //parse entered response and mark correct or wrong
                else if (int.TryParse(resp, System.Globalization.NumberStyles.HexNumber, null, out multResp))
                {
                    CheckAnswer(multResp);
                }
                else
                {
                    //provide guidance
                    Console.WriteLine("Enter an answer in a valid hex format");
                }
            }
        }
        public static int ParseIntValue(int defaultValue) //TODO: move this out of this class
        {
            //containers
            int value;
            bool valueSelected = false;

            do
            {
                //container
                string resp;

                //prompt
                Console.WriteLine($"Push 'Enter' for default value of '{defaultValue.ToString(hexFormat)}'");

                //gather response
                resp = Console.ReadLine();

                valueSelected = false;

                if (resp == string.Empty)
                {
                    //use default value
                    valueSelected = true;
                    Console.WriteLine($"Using default value {defaultValue.ToString(hexFormat)}");
                    value = defaultValue;
                }
                else
                {
                    //parse entered response
                    if (int.TryParse(resp, System.Globalization.NumberStyles.HexNumber, null, out value))
                    {
                        //use parsed response
                        valueSelected = true;
                        Console.WriteLine($"Using entered value {value.ToString(hexFormat)}");
                    }
                    else
                    {
                        //display guidance
                        Console.WriteLine($"Enter a number in a valid hex format");
                    }
                }
            } while (valueSelected == false);

            return value;
        }
        public void SummarizeResults()
        {
            Console.WriteLine();
            Console.WriteLine("YOU HAVE PROVIDED A GUESS FOR ALL FLASH CARDS");
            Console.WriteLine($"CORRECT ANSWERS: {rightCards.Count}");
            Console.WriteLine($"WRONG ANSWERS: {wrongCards.Count}");
            Console.WriteLine($"PERCENT CORRECT: {(rightCards.Count / (double)cards.Count).ToString(percentFormat)}");
            Console.ReadLine();
        }
        public void MoveWrongToUnanswered()
        {
            foreach (FlashCard fc in wrongCards)
            {
                unansweredCards.Push(fc);
            }
            wrongCards.Clear();
        }

        //per-card functions
        private void Prompt()
        {
            //get next card
            FlashCard flashCard = unansweredCards.Peek();
            Console.WriteLine($"{flashCard.multiplier.ToString(hexFormat)} * {flashCard.multiplicand.ToString(hexFormat)} ?");
        }
        private void CheckAnswer(int guess)
        {
            FlashCard flashCard = unansweredCards.Pop();
            if (guess == flashCard.product)
            {
                Console.WriteLine($"Correct");
                rightCards.Push(flashCard);
            }
            else
            {
                Console.WriteLine("Wrong");
                Console.WriteLine($"Correct Answer: {flashCard.product.ToString(hexFormat)}");
                wrongCards.Push(flashCard);
            }
        }
        private void ShowAnswer()
        {
            FlashCard flashCard = unansweredCards.Pop();
            Console.WriteLine($"Correct Answer: {flashCard.product.ToString(hexFormat)}");
            wrongCards.Push(flashCard);
        }

        //text-writing
        public static void ShowIntroduction()
        {
            //intro text
            Console.WriteLine("DIRECTIONS");
            Console.WriteLine("This program repeatedly asks multication problems.");
            Console.WriteLine($"Type your answer in hex format, and then push 'Enter'.");
            Console.WriteLine();
            Console.WriteLine("HINT");
            Console.WriteLine($"Type '{answerSig}' and then push 'Enter' to display the answer without attempting a guess.");
        }

    }
}
