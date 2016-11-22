using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web.Mvc.Html;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PLM.Extensions;
namespace PLM.Controllers
{
    public class GameController : Controller
    {
        private string CorrectAnswer;
        private UserGameSession currentGameSession;

        [HttpGet]
        public ActionResult Setup(int PLMid, bool changeSettings)
        {
            if (currentGameSession == null)
            {
                GenerateSession();
            }
            GenerateModule(PLMid);

            if (changeSettings)
            {
                return View(((UserGameSession)Session["userGameSession"]).gameModule);
            }
            else
            {
                TakeDefaultGameSettings();
                SetupGame();
                return RedirectToAction("Play");
            }
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Quit()
        {
            Session["userGameSession"] = null;
            return RedirectToAction("Index", "Modules");
        }

        #region NEW
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setup(int time, int numQuestions, int numAnswers)
        {
            GameSettings ugs = new GameSettings(time, numQuestions, numAnswers);
            TakeUserGameSettings(ugs);
            SetupGame();
            return RedirectToAction("Play");
        }

        private void GenerateSession()
        {
            Session["userGameSession"] = null;
            currentGameSession = new UserGameSession();
            Session["userGameSession"] = currentGameSession;
        }
        private void GenerateModule(int PLMid)
        {
            ((UserGameSession)Session["userGameSession"]).gameModule = null;
            using (Repos repo = new Repos())
            {
                ((UserGameSession)Session["userGameSession"]).gameModule = new GameModule(repo.GetModuleByID(PLMid));
            }
        }
        private void SetupGame()
        {
            foreach (GameAnswer ga in ((UserGameSession)Session["userGameSession"]).gameModule.Answers)
            {
                ga.Pictures.Shuffle();
            }
            ((UserGameSession)Session["userGameSession"]).gameModule.Answers.Shuffle();
            SetTime();
        }
        private void SetTime()
        {
            int timeHours = (((UserGameSession)Session["userGameSession"]).GameSettings.Time / 60);
            int timeMinutes = (((UserGameSession)Session["userGameSession"]).GameSettings.Time % 60);
            ((UserGameSession)Session["userGameSession"]).timeLeft = new TimeSpan(timeHours, timeMinutes, 0);
        }
        private void TakeDefaultGameSettings()
        {
            ((UserGameSession)Session["userGameSession"]).GameSettings.Questions = ((UserGameSession)Session["userGameSession"]).gameModule.DefaultNumQuestions;
            ((UserGameSession)Session["userGameSession"]).GameSettings.Time = ((UserGameSession)Session["userGameSession"]).gameModule.DefaultTime;
            ((UserGameSession)Session["userGameSession"]).GameSettings.Answers = ((UserGameSession)Session["userGameSession"]).gameModule.DefaultNumAnswers;
        }
        private void TakeUserGameSettings(GameSettings ugs)
        {
            ((UserGameSession)Session["userGameSession"]).GameSettings.Questions = ugs.Questions;
            ((UserGameSession)Session["userGameSession"]).GameSettings.Answers = ugs.Answers;
            ((UserGameSession)Session["userGameSession"]).GameSettings.Time = ugs.Time;
        }
        private void HandleUserGuess(string guess)
        {
            if (guess == CorrectAnswer)
            {
                ((UserGameSession)Session["userGameSession"]).Score += 100;
            }
                ((UserGameSession)Session["userGameSession"]).currentQuestion++;
        }
        private bool GameIsDone(TimeSpan Time)
        {
            if (((UserGameSession)Session["userGameSession"]).currentQuestion >= ((UserGameSession)Session["userGameSession"]).GameSettings.Questions | (Time.CompareTo(new TimeSpan(0, 0, 0)) < 1))
            {
                return true;
            }
            return false;
        }
        private bool OutOfAnswers(GameModule gm)
        {
            foreach (GameAnswer ga in gm.Answers)
            {
                if (ga.Pictures.Count == 0)
                {
                    return false;
                }
            }
            return true;
        }
        [HttpGet]
        public ActionResult Play()
        {
            Question question = new Question(((UserGameSession)Session["userGameSession"]));
            CorrectAnswer = question.CorrectAnswer;
            return View(question);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleGuess(string Time, string Guess, string correctAnswer)
        {
            ((UserGameSession)Session["userGameSession"]).timeLeft = TimeSpan.Parse(Time);
            CorrectAnswer = correctAnswer;
            HandleUserGuess(Guess);
            if (GameIsDone(((UserGameSession)Session["userGameSession"]).timeLeft))
            {
                return RedirectToAction("Complete", new { Score = ((UserGameSession)Session["userGameSession"]).Score });
            }
            else
            {
                return RedirectToAction("Play");
            }
        }
        public ActionResult Complete(int score)
        {
            ViewBag.ModuleID = ((UserGameSession)Session["userGameSession"]).gameModule.ModuleID;
            Score newScore = new Score(score, ((UserGameSession)Session["userGameSession"]).gameModule.ModuleID, ((UserGameSession)Session["userGameSession"]).GameSettings.Questions);
            string error = "";
            SaveScore(newScore, error);
            GameCompleted resultScreen = new GameCompleted();
            resultScreen.correctAnswers = newScore.CorrectAnswers;
            resultScreen.totalQuestions = newScore.TotalAnswers;
            if (error != "")
            {
                resultScreen.errorMessage = error;
            }
            using (Repos repo = new Repos())
            {
                var dbScores = repo.GetTop10Scores(((UserGameSession)Session["userGameSession"]).gameModule.ModuleID);
                foreach (var dbs in dbScores)
                {
                    resultScreen.Top10Scores.Add(dbs);
                }
            }
            return View(resultScreen);
        }
        private void SaveScore(Score score, string error)
        {
            if (Request.IsAuthenticated)
            {
                if (((UserGameSession)Session["userGameSession"]).GameSettings.Time == ((UserGameSession)Session["userGameSession"]).gameModule.DefaultTime &&
                    ((UserGameSession)Session["userGameSession"]).GameSettings.Answers == ((UserGameSession)Session["userGameSession"]).gameModule.DefaultNumAnswers &&
                    ((UserGameSession)Session["userGameSession"]).GameSettings.Questions == ((UserGameSession)Session["userGameSession"]).gameModule.DefaultNumQuestions)
                {
                    score.UserID = User.Identity.GetUserId();
                    using (Repos repo = new Repos())
                    {
                        if (!repo.AddScore(score))
                        {
                            error = "There was an error saving the score to the database";
                        }
                    }
                }
                else
                {
                    error = "You must use default settings for your score to save";
                }
            }
            else
            {
                error = "You must be logged in for your score to save";
            }
        }
        #endregion
    }
}