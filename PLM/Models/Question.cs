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

        }

        public Question(GameModule gm, GameSettings gs)
        {
            Score = 0;
            TimeRemaining = gs.Time;
            CurrentQuestion = 0;
            TotalQuestions = gs.Questions;
            GamePicture = gm.Answers.ElementAt(0).Pictures.ElementAt(0);
            AddAnswers(gm.Answers, gs.Questions);
            CorrectAnswerString = gm.rightAnswerString;
            IncorrectAnswerString = gm.wrongAnswerString;
        }

        private void AddAnswers(List<GameAnswer> gas, int gameSettingQuestions)
        {
            List<string> AddedAnswers = new List<string>();
            CorrectAnswer = gas.ElementAt(0).AnswerString;
            Guesses.Add(CorrectAnswer);
            AddedAnswers.Add(CorrectAnswer);
            gas.Remove(gas.ElementAt(0));

            Random r = new Random(DateTime.Now.Millisecond);
            while (Guesses.Count < gameSettingQuestions)
            {
                string answerToAdd = gas.ElementAt(r.Next(0, gas.Count())).AnswerString;
                if (!Guesses.Contains(answerToAdd))
                {
                    Guesses.Add(answerToAdd);
                    AddedAnswers.Remove(answerToAdd);
                }
            }
            Guesses.Shuffle();
        }
    }
}