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

            //Quizzes/Create
            int quizId = NewQuiz();

            //Rounds/Create?qid=5
            int roundId = NewRound(quizId, 1);
            IEnumerable<FlashCard> cards = GetCards();
            AddCards(cards);
            AddQuestions(roundId, cards);






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
        private static void AddQuestions(int roundId, IEnumerable<FlashCard> flashCards)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                var round = db.Round.Single(r => r.Id == roundId);
                IList<DAL.FlashCard> dbFlashCards = new List<DAL.FlashCard>();

                //PERFECTION: do this is a LINQ projection syntax (avoids having to use a list)
                foreach (var flashCard in flashCards)
                {
                    dbFlashCards.Add(db.FlashCard
                        .Single(fc => fc.Multiplicand == flashCard.multiplicand && fc.Multiplier == flashCard.multiplier));
                }

                foreach (var flashCard in dbFlashCards)
                {
                    var question = new DAL.Question() { Round = round, FlashCard = flashCard };
                    db.Question.Add(question);
                }
                db.SaveChanges();
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
        private static int NewQuiz()
        {
            //add student, quiz, and round
            using (var db = new DAL.FlashCardEntities())
            {
                //TODO: actually select student
                //PERFECTION: take these out of the using?
                var s = new DAL.Student { Name = "Vince" };
                var q = new DAL.Quiz { Started = DateTime.Now };
                s.Quiz.Add(q);

                db.Student.Add(s);
                db.SaveChanges();

                return q.Id;
            }
        }
        private static void AddCards(IEnumerable<FlashCard> cards)
        {
            using (var db = new DAL.FlashCardEntities())
            {
                IList<DAL.FlashCard> dbFlashCards = new List<DAL.FlashCard>();
                foreach (var flashCard in cards)
                {
                    //PERFECTION: handle concurrency if two users are adding the same cards at the same time
                    if (!db.FlashCard
                        .Any(fc => fc.Multiplicand == flashCard.multiplicand && fc.Multiplier == flashCard.multiplier))
                    {
                        dbFlashCards.Add(Mapper.Map<FlashCard, DAL.FlashCard>(flashCard));
                    }
                }
                db.FlashCard.AddRange(dbFlashCards);
                db.SaveChanges();
            }
        }
        #endregion
        #region Utility
        private static IEnumerable<FlashCard> GetCards()
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
