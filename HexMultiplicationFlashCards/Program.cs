using HexMultiplicationFlashCards.Repositories;
using HexMultiplicationFlashCards.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

//TODO: Put in README: big lesson learned - do not use a completely different pattern for an initial program draft
//using a console application for simplicity then transitioning to web is fine, but using .NET stacks versus actual
//DB objects forced a re-write of everything.

//TODO: put in README: decided not to change name of round table (despite being a TSQL keyword)
//because it was the only word that really made logical sense as that object type
namespace HexMultiplicationFlashCards
{
    class Program
    {
        static void Main(string[] args)
        {
            AutoMapperConfig.Initialize();

            //display intro
            ShowIntroduction();

            //Students/Create
            int studentId = FlashCardRepo.NewStudent();

            //Quizzes/Create
            int quizId = FlashCardRepo.NewQuiz(studentId);

            IEnumerable<FlashCard> cards = NewCards();
            CreateRound(quizId, cards);

            //Quizzes/5
            Console.WriteLine($"Done! Overall {FlashCardRepo.NumQuizQuestions(quizId)} Attempts, {FlashCardRepo.NumQuizQuestionsCorrect(quizId)} Correct, and {FlashCardRepo.NumQuizQuestionsWrong(quizId)} Wrong.");
            Console.ReadLine();
        }
        //Rounds/Edit/4
        public static void ActionEditRound(int roundId)
        {
            //get info
            IEnumerable<FlashCard> flashCards = FlashCardRepo.GetCards(roundId);
            int roundNumber = FlashCardRepo.GetCurrentRoundNum(roundId);

            //prompt
            foreach (var fc in flashCards)
            {
                Console.WriteLine($"{fc.Multiplicand} X {fc.Multiplier} ?");
                fc.Response = int.Parse(Console.ReadLine());
            }

            //save (this would actually be a POST to //Rounds/Edit/4)
            FlashCardRepo.SaveQuestions(flashCards);
            Console.WriteLine($"Round {roundNumber} Complete. {flashCards.Where(fc => fc.Response == fc.Product).Count()} Correct, {flashCards.Where(fc => fc.Response != fc.Product).Count()} Wrong.");

            //generate a new round if necessary
            if (flashCards.Any(fc => fc.Response != fc.Product))
            {
                flashCards = flashCards.Where(fc => fc.Response != fc.Product);
                CreateRound(FlashCardRepo.GetQuizId(roundId), flashCards);
            }
        }
        private static void CreateRound(int quizId, IEnumerable<FlashCard> cards)
        {
            //create new round and add questions
            int roundNumber = FlashCardRepo.GetMaxRoundNum(quizId) + 1; //returns a default int value of 0 when no rounds exist for the quiz yet
            int roundId = FlashCardRepo.NewRound(quizId, roundNumber);
            FlashCardRepo.NewQuestions(roundId, cards);

            //call the edit page
            ActionEditRound(roundId);
        }
        private static IEnumerable<FlashCard> NewCards()
        {
            const int defaultMinValue = 4;  //arbitrary choice
            const int defaultMaxValue = 5;  //arbitrary choice

            //get maximum factors
            Console.WriteLine();
            Console.WriteLine("Choose the Minimum Multiplier");
            int minMultiplier = ParseIntValue(defaultMinValue);
            Console.WriteLine();
            Console.WriteLine("Choose the Minimum Multiplicand");
            int minMultiplicand = ParseIntValue(defaultMinValue);
            Console.WriteLine();
            Console.WriteLine("Choose the Max Multiplier");
            int maxMultiplier = ParseIntValue(defaultMaxValue);
            Console.WriteLine();
            Console.WriteLine("Choose the Max Multiplicand");
            int maxMultiplicand = ParseIntValue(defaultMaxValue);

            //populate card list
            IList<FlashCard> cards = new List<FlashCard>();
            for (int multiplier = minMultiplier; multiplier <= maxMultiplier; multiplier++)
            {
                for (int multiplicand = minMultiplicand; multiplicand <= maxMultiplicand; multiplicand++)
                {
                    cards.Add(new FlashCard(multiplier, multiplicand));
                }
            }

            return cards;
        }
        public static void ShowIntroduction()
        {
            //intro text
            Console.WriteLine("DIRECTIONS");
            Console.WriteLine("This program repeatedly asks multication problems.");
            Console.WriteLine($"Type your answer in hex format, and then push 'Enter'.");
        }
        public static int ParseIntValue(int defaultValue) //TODO: move this out of this class
        {
            const string hexFormat = "X";
            
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
    }
}
