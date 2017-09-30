using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HexMultiplicationFlashCards
{
    class Program
    //TODO: Put in README: big lesson learned - do not use a completely different pattern for an initial program draft
    //using a console application for simplicity then transitioning to web is fine, but using .NET stacks versus actual
    //DB objects forced a re-write of everything.

    //TODO: change name of round table because it is a TSQL keyword
    {
        private static string resp { get; set; }

        static void Main(string[] args)
        {
            AutoMapperConfig.Initialize();

            //display intro
            FlashCardDeck.ShowIntroduction();

            //Students/Create
            int studentId = NewStudent();

            //Quizzes/Create
            int quizId = NewQuiz(studentId);


            int roundNumber = 1;

            //Rounds/Create?qid=5
            int roundId = NewRound(quizId, roundNumber);
            IEnumerable<FlashCard> cards = NewCards();
            NewQuestions(roundId, cards);

            while (true)
            {
                roundNumber++;

                IEnumerable<FlashCard> flashCards = GetCards(roundId);
                foreach (var fc in flashCards)
                {
                    Console.WriteLine($"{fc.Multiplicand} X {fc.Multiplier} ?");
                    fc.Response = int.Parse(Console.ReadLine());
                }
                SaveQuestions(flashCards);

                //if (flashCards.Any)
                
            } 





            var haveWrong = true;

            //per-round loop
            //  //collect all answers
            //  //check answers
            //  //if 
            while (haveWrong)
            {
                //do the quiz
                //check if some are wrong

            }

            //add wrong





            ////run procedures
            //while (flashCardDeck.CardsAvailable)
            //{
            //    flashCardDeck.AskQuestions();
            //    flashCardDeck.SummarizeResults();
            //    flashCardDeck.MoveWrongToUnanswered();
            //}
        }

        #region Data Repository

        //CREATE
        private static int NewStudent()
        {
            using (var db = new DAL.FlashCardEntities())
            {
                //TODO: actually select student
                //PERFECTION: take these out of the using?
                var s = new DAL.Student { Name = "Vince" };
                db.Student.Add(s);
                db.SaveChanges();
                return s.Id;
            }
        }
        private static int NewQuiz(int studentId)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                //PERFECTION: take these out of the using?
                var student = db.Student.Single(s => s.Id == studentId);
                var q = new DAL.Quiz { Started = DateTime.Now };
                student.Quiz.Add(q);

                db.SaveChanges();

                return q.Id;
            }
        }
        private static int NewRound(int quizId, int roundNum)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                var quiz = db.Quiz.Single(q => q.Id == quizId);
                var round = new DAL.Round() { Num = roundNum };
                quiz.Round.Add(round);
                db.SaveChanges();
                return round.Id;
            }
        }
        private static void NewQuestions(int roundId, IEnumerable<FlashCard> flashCards)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                var round = db.Round.Single(r => r.Id == roundId);
                IList<DAL.Question> questions = new List<DAL.Question>();

                foreach (var fc in flashCards)
                {
                    var question = Mapper.Map<FlashCard, DAL.Question>(fc);
                    question.Round = round;
                    db.Question.Add(question);
                }
                db.SaveChanges();
            }
        }

        //READ
        public static IEnumerable<FlashCard> GetCards(int roundId)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                IList<FlashCard> flashCards = new List<FlashCard>();
                var round = db.Round.Single(r => r.Id == roundId);
                foreach (var q in round.Question)
                {
                    flashCards.Add(Mapper.Map<DAL.Question, FlashCard>(q));
                }
                return flashCards;
            }
        }
       

        //UPDATE
        private static void SaveQuestions(IEnumerable<FlashCard> flashCards)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                //PERFECTION: only modify the "response" field
                //by querying from the DB by ID
                var questions = flashCards.Select(fc => Mapper.Map<FlashCard, DAL.Question>(fc));
                foreach (var q in questions)
                {
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
            }
        }


        #endregion
        #region Utility
        private static IEnumerable<FlashCard> NewCards()
        {
            const int defaultMinValue = 4;  //arbitrary choice
            const int defaultMaxValue = 5;  //arbitrary choice

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
        #endregion
    }
}
