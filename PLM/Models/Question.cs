using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLM
{
    public class Question
    {
        public int Score { get; set; }
        public int TimeRemaining { get; set; }
        public int CurrentQuestion { get; set; }
        public int TotalQuestions { get; set; }
        public Models.GamePicture GamePicture { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> Guesses { get; set; }
        public string CorrectAnswerString { get; set; }
        public string IncorrectAnswerString { get; set; }

        public Question()
        {
            Guesses = new List<string>();
        }

        public Question(UserGameSession gameSession)
        {
            Guesses = new List<string>();
            Score = gameSession.Score;
            CurrentQuestion = gameSession.currentQuestion;
            TimeRemaining = (int)gameSession.timeLeft.TotalSeconds;
            TotalQuestions = gameSession.GameSettings.Questions;
            CorrectAnswerString = gameSession.gameModule.rightAnswerString;
            IncorrectAnswerString = gameSession.gameModule.wrongAnswerString;
            CorrectAnswer = GetCorrectAnswer(gameSession.gameModule.Answers);
            if (CorrectAnswer == "error")
            {
                Shuffle(gameSession.gameModule.Answers);
                CorrectAnswer = GetCorrectAnswer(gameSession.gameModule.Answers);
                // Doesn't check for error again
            }
            Guesses.Add(CorrectAnswer);
            AddWrongAnswers(gameSession.GameSettings.Answers, gameSession.gameModule.Answers);
        }

        private void AddWrongAnswers(int maxAnswer, List<GameAnswer> gameAnswers)
        {
            string guess;
            Random r = new Random(DateTime.Now.Second);
            while (Guesses.Count < maxAnswer)
            {
                guess = gameAnswers.ElementAt(r.Next(0, gameAnswers.Count - 1)).AnswerString;
                if (!Guesses.Contains(guess))
                {
                    Guesses.Add(guess);
                }
            }
        }

        private void Shuffle(List<GameAnswer> ga)
        {
            foreach (GameAnswer a in ga)
            {
                foreach (Models.GamePicture gp in a.Pictures)
                {
                    gp.Usable = true;
                }
                a.Pictures.Shuffle();
                a.Usable = true;
            }
            ga.Shuffle();
        }

        private string GetCorrectAnswer(List<GameAnswer> ga)
        {
            foreach (GameAnswer a in ga)
            {
                if (a.Usable)
                {
                    foreach (Models.GamePicture gp in a.Pictures)
                    {
                        if (gp.Usable)
                        {
                            GamePicture = gp;
                            gp.Usable = false;
                            a.Usable = false;
                            return a.AnswerString;
                        }
                        // No available pictures for answer
                        a.Usable = false;
                    }
                }
            }
            // No answers with available pictures
            return "error";
        }
    }
}