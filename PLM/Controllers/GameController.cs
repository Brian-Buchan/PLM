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
        private ApplicationDbContext db = new ApplicationDbContext();
        private PlayViewModel currentGuess = new PlayViewModel();
        private List<int> GeneratedGuessIDs = new List<int>();
        private Module currentModule = new Module();
        private string CorrectAnswer;

        //private static Random rand = new Random();
        private UserGameSession currentGameSession;
        //private Score newScore;
        //private int answerID;
        //private int pictureID;
        //private bool loggedIn;
        //private int currentGuessNum;
        //private bool correctSettings;
        //private bool PLMgenerated = false;
        //private bool WrongAnswersGenerationNOTcompleted = true;



        [HttpGet]
        public ActionResult Setup(int PLMid, bool changeSettings)
        {
            //Remove
            //if (PLMgenerated == false)
            //{
            //    GenerateModule(PLMid);
            //}
            if (currentGameSession == null)
            {
                GenerateSession();
            }
            GenerateModule(PLMid);

            if (changeSettings)
            {
                return View(((UserGameSession)Session["userGameSession"]).currentModule);
            }
            else
            {
                TakeDefaultGameSettings();
                SetupGame();
                return RedirectToAction("Play");
            }
        }

        //#region OLD
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Setup([Bind(Include = "numAnswers,numQuestions,time")] UserGameSession ugs)
        //{
        //    int timeHours = (ugs.time / 60);
        //    int timeMinutes = (ugs.time % 60);
        //    ((UserGameSession)Session["userGameSession"]).numAnswers = ugs.numAnswers;
        //    ((UserGameSession)Session["userGameSession"]).numQuestions = ugs.numQuestions;
        //    ((UserGameSession)Session["userGameSession"]).time = ugs.time;
        //    ((UserGameSession)Session["userGameSession"]).timeLeft = new TimeSpan(timeHours, timeMinutes, 0);
        //    return RedirectToAction("Play");
        //}
        //[NonAction]
        //private void GenerateModule(int PLMid)
        //{
        //    // Create Session Variable
        //    currentGameSession = new UserGameSession();
        //    // Load Module
        //    currentGameSession.currentModule = db.Modules.Find(PLMid);
        //    // Reset Game Parameters
        //    currentGameSession.Score = 0;
        //    currentGameSession.currentQuestion = -1;
        //    currentGameSession.iteratedQuestion = -1;
        //    int answerIndex = -1;
        //    int pictureIndex;
        //    foreach (Answer answer in currentGameSession.currentModule.Answers)
        //    {
        //        answerIndex++;
        //        pictureIndex = -1;
        //        foreach (Picture picture in answer.Pictures)
        //        {
        //            pictureIndex++;
        //            currentGameSession.PictureIndices.Add(new AnsPicIndex(answerIndex, pictureIndex, picture));
        //        }
        //    }
        //    // Shuffle the list of pictures so Users itterate through them randomly
        //    currentGameSession.PictureIndices.Shuffle();
        //    //stuff that would be normally defined during setup. Will be overwritten in the setup POST action if it is accessed
        //    int timeHours = (currentGameSession.currentModule.DefaultTime / 60);
        //    int timeMinutes = (currentGameSession.currentModule.DefaultTime % 60);
        //    currentGameSession.numAnswers = currentGameSession.currentModule.DefaultNumAnswers;
        //    currentGameSession.defaultNumAnswer = currentGameSession.currentModule.DefaultNumAnswers;
        //    currentGameSession.numQuestions = currentGameSession.currentModule.DefaultNumQuestions;
        //    currentGameSession.time = currentGameSession.currentModule.DefaultTime;
        //    currentGameSession.timeLeft = new TimeSpan(timeHours, timeMinutes, 0);
        //    Session["userGameSession"] = currentGameSession;
        //}
        //[HttpGet]
        //public ActionResult Play()
        //{
        //    GenerateQuestionONEperPIC();
        //    currentGuess.Time = ((UserGameSession)Session["userGameSession"]).timeLeft;
        //    currentGuess.CurrentQuestion = ((UserGameSession)Session["userGameSession"]).currentQuestion + 1;
        //    currentGuess.TotalQuestions = ((UserGameSession)Session["userGameSession"]).numQuestions;
        //    return View(currentGuess);
        //}
        //[NonAction]
        //private void GenerateQuestionONEperPIC()
        //{
        //    //increment guess counters
        //    ((UserGameSession)Session["UserGameSession"]).currentQuestion += 1;
        //    ((UserGameSession)Session["UserGameSession"]).iteratedQuestion += 1;
        //    currentGuessNum = ((UserGameSession)Session["UserGameSession"]).iteratedQuestion;
        //    currentModule = ((UserGameSession)Session["userGameSession"]).currentModule;
        //    int[] indicies = GetPictureID(currentGuessNum);
        //    int answerIndex = indicies[0];
        //    int pictureIndex = indicies[1];
        //    currentGuess.Answer = currentModule.Answers.ElementAt(answerIndex).AnswerString;
        //    currentGuess.PictureID = currentModule.Answers.ElementAt(answerIndex).Pictures.ElementAt(pictureIndex).PictureID;
        //    currentGuess.possibleAnswers.Add(currentModule.Answers.ElementAt(answerIndex).AnswerString);
        //    if (currentModule.Answers.ElementAt(answerIndex).Pictures.ElementAt(pictureID).Attribution == null)
        //        currentGuess.PictureToView.Attribution = "";
        //    else
        //        currentGuess.PictureToView.Attribution = currentModule.Answers.ElementAt(answerIndex).Pictures.ElementAt(pictureID).Attribution;
        //    GeneratedGuessIDs.Add(answerIndex);
        //    currentGuess.PictureToView.PictureData = db.Pictures.Find(currentGuess.PictureID).PictureData;
        //    currentGuess.PictureToView.Attribution = currentGuess.PictureToView.Attribution;
        //    GenerateWrongAnswers();
        //    currentGuess.possibleAnswers.Shuffle();
        //}
        //[NonAction]
        //private int[] GetPictureID(int currentGuessNum)
        //{
        //    AnsPicIndex IndexItem = ((UserGameSession)Session["userGameSession"]).PictureIndices.ElementAt(currentGuessNum);
        //    return new int[] { IndexItem.AnswerIndex, IndexItem.PictureIndex };
        //}
        ///// <summary>
        ///// Generate the wrong answers to be displayed during each question
        ///// </summary>
        //[NonAction]
        //private void GenerateWrongAnswers()
        //{
        //    int wrongAnswerID;
        //    //while we still have work to do
        //    while (WrongAnswersGenerationNOTcompleted)
        //    {
        //        CheckMaxAnswers();
        //        do
        //        {
        //            wrongAnswerID = rand.Next(0, (currentModule.Answers.Count - 1));
        //        } while (GeneratedGuessIDs.Contains(wrongAnswerID));
        //        //add the selected answer to both the stuff to send over and the list of no longer addable answers
        //        currentGuess.possibleAnswers.Add(currentModule.Answers.ElementAt(wrongAnswerID).AnswerString);
        //        GeneratedGuessIDs.Add(wrongAnswerID);
        //        if (GeneratedGuessIDs.Count >= ((UserGameSession)Session["userGameSession"]).numAnswers)
        //        {
        //            //break out of the loop
        //            WrongAnswersGenerationNOTcompleted = false;
        //        }
        //    }
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Play(int Score, string Time, string isCorrect)
        //{
        //    bool BoolIsCorrect;
        //    //Update the user's score, progress, and time
        //    //If the isCorrect string is correctly parsed 
        //    if (Boolean.TryParse(isCorrect, out BoolIsCorrect))
        //    {
        //        //If the question was answered correctly
        //        if (BoolIsCorrect)
        //        {
        //            //Increase the correct questions counter
        //            ((UserGameSession)Session["userGameSession"]).numCorrect += 1;
        //        }
        //    }
        //    ((UserGameSession)Session["userGameSession"]).Score = Score;
        //    ((UserGameSession)Session["userGameSession"]).timeLeft = TimeSpan.Parse(Time);
        //    if (GameIsDone())
        //    {
        //        //If the user is done, break out of the loop and send the "Complete" action the final score               
        //        return RedirectToAction("Complete", new { Score = Score });
        //    }
        //    //GenerateQuestionONEperPIC();
        //    currentGuess.CurrentQuestion = ((UserGameSession)Session["userGameSession"]).currentQuestion + 1;
        //    currentGuess.TotalQuestions = ((UserGameSession)Session["userGameSession"]).numQuestions;
        //    currentGuess.Score = Score;
        //    var test = int.Parse(Request.Form.Get("Score"));
        //    currentGuess.Score = test;
        //    currentGuess.Time = ((UserGameSession)Session["userGameSession"]).timeLeft;
        //    return View(currentGuess);
        //}
        ///// <summary>
        ///// Check if the user has completed the game or has run out of time.If the user is not done 
        ///// and there are no more questions, reshuffle the PictureIndices and continue 
        ///// </summary>
        ///// <returns>Bool</returns>
        //[NonAction]
        //private bool GameIsDone()
        //{
        //    currentModule = ((UserGameSession)Session["userGameSession"]).currentModule;
        //    //if the question or time limit has been reached
        //    if (((UserGameSession)Session["userGameSession"]).currentQuestion
        //        //subtract 1 from number of questions since currentQuestion is zero based
        //        >= (((UserGameSession)Session["userGameSession"]).numQuestions - 1)
        //        |
        //        (((UserGameSession)Session["userGameSession"]).timeLeft.CompareTo(new TimeSpan(0, 0, 0)) < 1))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        //if the game is not over, and the list of pictures is exhausted, reshuffle. Otherwise, continue.
        //        if (((UserGameSession)Session["userGameSession"]).currentQuestion.IsDivisible(
        //            ((UserGameSession)Session["userGameSession"]).PictureIndices.Count - 1))
        //        {
        //            Reshuffle();
        //        }
        //    }
        //    return false;
        //}
        //public ActionResult Complete(int score)
        //{
        //    newScore = new Score();
        //    if (Request.IsAuthenticated)
        //    {
        //        loggedIn = true;
        //        if (((UserGameSession)Session["userGameSession"]).currentModule.DefaultNumAnswers == ((UserGameSession)Session["userGameSession"]).defaultNumAnswer
        //            && ((UserGameSession)Session["userGameSession"]).currentModule.DefaultNumQuestions == ((UserGameSession)Session["userGameSession"]).numQuestions
        //            && ((UserGameSession)Session["userGameSession"]).currentModule.DefaultTime == ((UserGameSession)Session["userGameSession"]).time)
        //        {
        //            correctSettings = true;
        //        }
        //        else
        //        {
        //            correctSettings = false;
        //            ViewBag.ErrorMessage = "You must use default settings for your score to save";
        //        }
        //    }
        //    else
        //    {
        //        loggedIn = false;
        //        ViewBag.ErrorMessage = "You must be logged in for your score to save";
        //    }
        //    SaveScore(score);
        //    ViewBag.ModuleID = ((UserGameSession)Session["userGameSession"]).currentModule.ModuleID;
        //    List<Score> scores = TopTenScore.GetTopTenScores(((UserGameSession)Session["userGameSession"]).currentModule.ModuleID);
        //    List<string[]> scoresToSend = new List<string[]>();
        //    foreach (Score top10score in scores)
        //    {
        //        string[] stringToSend = new string[3];
        //        ApplicationUser player = db.Users.Find(top10score.UserID);
        //        try
        //        {
        //            stringToSend[0] = (player.FirstName + ", " + player.LastName[0]);
        //        }
        //        catch
        //        {
        //            stringToSend[0] = "Error";
        //        }
        //        try
        //        {
        //            stringToSend[1] = (top10score.CorrectAnswers + " out of " + top10score.TotalAnswers);
        //        }
        //        catch
        //        {
        //            stringToSend[1] = "Error";
        //        }
        //        try
        //        {
        //            stringToSend[2] = top10score.TimeStamp.ToShortDateString();
        //        }
        //        catch
        //        {
        //            stringToSend[2] = "Error";
        //        }
        //        scoresToSend.Add(stringToSend);
        //    }
        //    ViewBag.Top10Scores = scoresToSend;
        //    return View(newScore);
        //}
        //[NonAction]
        //private void SaveScore(int score)
        //{
        //    newScore.CorrectAnswers = (score / 100);
        //    newScore.ModuleID = ((UserGameSession)Session["userGameSession"]).currentModule.ModuleID;
        //    newScore.TotalAnswers = ((UserGameSession)Session["userGameSession"]).numQuestions;
        //    if (loggedIn && correctSettings)
        //    {
        //        newScore.UserID = User.Identity.GetUserId();
        //        db.Entry(newScore).State = EntityState.Added;
        //        db.SaveChanges();
        //    }
        //}
        //#endregion

        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Quit()
        {
            Session["userGameSession"] = null;
            return RedirectToAction("Index", "Modules");
        }
        /// <summary>
        /// Check to make sure that there are enough answers to generate the required amount.
        /// If not, set the default number of answers to something that will not break the program.
        /// </summary>
        [NonAction]
        private void CheckMaxAnswers()
        {
            //IsDivisible(x, 3) 3 is hard coded as we want the game to display multiple answers in groups of 3
            while (currentModule.Answers.Count < ((UserGameSession)Session["userGameSession"]).numAnswers || !MathExtensions.IsDivisible(((UserGameSession)Session["userGameSession"]).numAnswers, 3))
            {
                ((UserGameSession)Session["userGameSession"]).numAnswers--;
            }
        }
        /// <summary>
        /// Reshuffle the picture indices list, and set the counter variable to -1
        /// </summary>
        [NonAction]
        private void Reshuffle()
        {
            //shuffle the picture indices
            ((UserGameSession)Session["userGameSession"]).PictureIndices.Shuffle();
            //reset the iterated guess counter
            ((UserGameSession)Session["userGameSession"]).iteratedQuestion = -1;
        }

        #region NEW
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setup([Bind(Include = "numAnswers,numQuestions,time")] GameSettings ugs)
        {
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
                    db.Entry(score).State = EntityState.Added;
                    db.SaveChanges();
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