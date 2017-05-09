using System;
using System.Collections.Generic;

namespace HexMultiplicationFlashCards
{
    class FlashCardDeck
    {
        //REFACTOR: put these somewhere else
        const string hexFormat = "X";
        const string percentFormat = "P";
        const string quitSig = "q";
        const string answerSig = "?";

        //TODO: make private
        public int maxMultiplier { get; private set; }
        public int maxMultiplicand { get; private set; }
        public Stack<FlashCard> unansweredCards { get; private set; }
        public Stack<FlashCard> wrongCards { get; private set; }
        public Stack<FlashCard> rightCards { get; private set; }
        public IList<FlashCard> cards { get; set; }
        public FlashCardDeck(int maxMultiplier, int maxMultiplicand)
        {
            this.maxMultiplicand = maxMultiplicand;
            this.maxMultiplier = maxMultiplier;

            cards = new List<FlashCard>();
            unansweredCards = new Stack<FlashCard>();
            wrongCards = new Stack<FlashCard>();
            rightCards = new Stack<FlashCard>();

            for (int multiplier = 0; multiplier <= maxMultiplier; multiplier++)
            {
                for (int multiplicand = 0; multiplicand <= maxMultiplicand; multiplicand++)
                {
                    cards.Add(new FlashCard(multiplier, multiplicand));
                }
            }

            //PERFECTION: use LINQ
            foreach (FlashCard c in cards)
            {
                unansweredCards.Push(c);
            }
        }
        public void Prompt()
        {
            //get next card
            FlashCard flashCard = unansweredCards.Peek();
            Console.WriteLine($"{flashCard.multiplier.ToString(hexFormat)} * {flashCard.multiplicand.ToString(hexFormat)} ?");
        }
        public void CheckAnswer(int guess)
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
        public void ShowAnswer()
        {
            FlashCard flashCard = unansweredCards.Pop();
            Console.WriteLine($"Correct Answer: {flashCard.product.ToString(hexFormat)}");
            wrongCards.Push(flashCard);
        }
        public void AskQuestions()
        {
            //container
            string resp = string.Empty;

            //main question ask/answer loop
            while (resp != quitSig && unansweredCards.Count > 0)
            {
                //display question
                Console.WriteLine();
                Console.WriteLine($"{unansweredCards.Count.ToString(hexFormat)} UNANSWERED CARDS REMAINING");
                Prompt();

                //gather response
                resp = Console.ReadLine();

                //skip any further screen writing if given the quit signal
                if (resp != quitSig)
                {
                    //container
                    int multResp;

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
                        //show error
                        Console.WriteLine($"Enter a valid hex number to answer, enter '{answerSig}' if you don't know an answer, or enter '{quitSig}' to quit.");
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("YOU HAVE PROVIDED A GUESS FOR ALL FLASH CARDS");
            Console.WriteLine($"CORRECT ANSWERS: {rightCards.Count}");
            Console.WriteLine($"WRONG ANSWERS: {wrongCards.Count}");
            Console.WriteLine($"PERCENT CORRECT: {(rightCards.Count / (double)cards.Count).ToString(percentFormat)}");
            Console.ReadLine();
        }
    }
}
