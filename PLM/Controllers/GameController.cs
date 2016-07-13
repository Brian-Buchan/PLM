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
        private static Random rand = new Random();
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserGameSession currentGameSession;
        private Module currentModule = new Module();
        private List<int> GeneratedGuessIDs = new List<int>();
        private PlayViewModel currentGuess = new PlayViewModel();
        private int currentGuessNum;
        private Score newScore;

        private bool PLMgenerated = false;
        private bool WrongAnswersGenerationNOTcompleted = true;

        private int answerID;
        private int pictureID;

        [HttpGet]
        public ActionResult Setup(int PLMid, int changeSettings)
        {
            int IDtoPASS = PLMid;

            if (PLMgenerated == false) 
            { 
                GenerateModule(IDtoPASS);
            }

            //If the user wants to change the settings of the game session
            if (changeSettings == 1)
            {
                return View(((UserGameSession)Session["userGameSession"]).currentModule);
            }
            else
            {
                return RedirectToAction("Play");
            }
            //return View(((UserGameSession)Session["userGameSession"]).currentModule);
        }

        /// <summary>
        /// Generate a module and create a UserGameSession session variable with that module.
        /// </summary>
        /// <param name="PLMid">The ID of the PLM to use</param>
        [NonAction]
        private void GenerateModule(int PLMid)
        {
            currentGameSession = new UserGameSession();
            currentGameSession.currentModule = db.Modules.Find(PLMid);
            currentGameSession.Score = 0;

            // set to -1 because GenerateGuess() will increment it to 0 the first time it runs
            currentGameSession.currentQuestion = -1;
            currentGameSession.iteratedQuestion = -1;
            int answerIndex = -1;
            int pictureIndex;
            foreach (Answer answer in currentGameSession.currentModule.Answers)
            {
                answerIndex++;
                pictureIndex = -1;

                foreach (Picture picture in answer.Pictures)
                {
                    pictureIndex++;
                    currentGameSession.PictureIndices.Add(new AnsPicIndex(answerIndex, pictureIndex, picture));
                }
            }
            // Shuffle the list of pictures so Users itterate through them randomly
            currentGameSession.PictureIndices.Shuffle();

            //stuff that would be normally defined during setup. Will be overwritten in the setup POST action if it is accessed
            int timeHours = (currentGameSession.currentModule.DefaultTime / 60);
            int timeMinutes = (currentGameSession.currentModule.DefaultTime % 60);
            currentGameSession.numAnswers = currentGameSession.currentModule.DefaultNumAnswers;
            currentGameSession.numQuestions = currentGameSession.currentModule.DefaultNumQuestions;
            currentGameSession.time = currentGameSession.currentModule.DefaultTime;
            currentGameSession.timeLeft = new TimeSpan(timeHours, timeMinutes, 0);

            Session["userGameSession"] = currentGameSession;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setup([Bind(Include = "numAnswers,numQuestions,time")] UserGameSession ugs)
        {
            int timeHours = (ugs.time / 60);
            int timeMinutes = (ugs.time % 60);
            ((UserGameSession)Session["userGameSession"]).numAnswers = ugs.numAnswers;
            ((UserGameSession)Session["userGameSession"]).numQuestions = ugs.numQuestions;
            ((UserGameSession)Session["userGameSession"]).time = ugs.time;
            ((UserGameSession)Session["userGameSession"]).timeLeft = new TimeSpan(timeHours, timeMinutes, 0);

            //This line is for testing the "Complete" action and the timer functionality.
            //Comment out the line of code just above it, then uncomment this code to enter "testing mode",
            //where the timer will always start at 30 seconds.

            //((UserGameSession)Session["userGameSession"]).timeLeft = new TimeSpan(0, 0, 30);
            return RedirectToAction("Play");
        }

        [HttpGet]
        public ActionResult Play()
        {
            GenerateQuestionONEperPIC();
            currentGuess.Time = ((UserGameSession)Session["userGameSession"]).timeLeft;
            currentGuess.CurrentQuestion = ((UserGameSession)Session["userGameSession"]).currentQuestion + 1;
            currentGuess.TotalQuestions = ((UserGameSession)Session["userGameSession"]).numQuestions;
            currentGuess.NumCorrect = ((UserGameSession)Session["userGameSession"]).numCorrect;
            return View(currentGuess);
        }

        /// <summary>
        /// Generate a question, loops through each picture in each answer
        /// The same answer will be chosen multiple times with different pictures
        /// </summary>
        [NonAction]
        private void GenerateQuestionONEperPIC()
        {
            //increment guess counters
            ((UserGameSession)Session["UserGameSession"]).currentQuestion += 1;
            ((UserGameSession)Session["UserGameSession"]).iteratedQuestion += 1;
            currentGuessNum = ((UserGameSession)Session["UserGameSession"]).iteratedQuestion;
            //currentGuessNum = (((UserGameSession)Session["userGameSession"]).currentQuestion++);
            currentModule = ((UserGameSession)Session["userGameSession"]).currentModule;
            int[] indicies = GetPictureID(currentGuessNum);
            int answerIndex = indicies[0];
            int pictureIndex = indicies[1];
            //pictureID = indicies[0];
            //pictureIndex = indicies[1];
            //answerIndex = indicies[2];

            currentGuess.Answer = currentModule.Answers.ElementAt(answerIndex).AnswerString;
            currentGuess.ImageURL = currentModule.Answers.ElementAt(answerIndex).Pictures.ElementAt(pictureIndex).Location;
            currentGuess.possibleAnswers.Add(currentModule.Answers.ElementAt(answerIndex).AnswerString);
            if (currentModule.Answers.ElementAt(answerIndex).Pictures.ElementAt(pictureID).Attribution == null)
            {
                currentGuess.Attribution = "";
            }
            else
            {
                currentGuess.Attribution = currentModule.Answers.ElementAt(answerIndex).Pictures.ElementAt(pictureID).Attribution;
            }

            GeneratedGuessIDs.Add(answerIndex);
            GenerateWrongAnswers();

            currentGuess.possibleAnswers.Shuffle();
        }

        [NonAction]
        private int[] GetPictureID(int currentGuessNum)
        {
            AnsPicIndex IndexItem = ((UserGameSession)Session["userGameSession"]).PictureIndices.ElementAt(currentGuessNum);
            return new int[] { IndexItem.AnswerIndex, IndexItem.PictureIndex };
        }

        /// <summary>
        /// Generate the wrong answers to be displayed during each question
        /// </summary>
        [NonAction]
        private void GenerateWrongAnswers()
        {
            int wrongAnswerID;
            //while we still have work to do
            while (WrongAnswersGenerationNOTcompleted)
            {
                CheckMaxAnswers();
                do
                {
                    wrongAnswerID = rand.Next(0, (currentModule.Answers.Count - 1));
                } while (GeneratedGuessIDs.Contains(wrongAnswerID));

                //add the selected answer to both the stuff to send over and the list of no longer addable answers
                currentGuess.possibleAnswers.Add(currentModule.Answers.ElementAt(wrongAnswerID).AnswerString);
                GeneratedGuessIDs.Add(wrongAnswerID);

                //if we've completed our work
                // TODO - Add functionality that checks if the module has enough answers to reach
                // the value of DefaultNumAnswers so that an error isn't thrown
                //if (GeneratedGuessIDs.Count >= currentModule.DefaultNumAnswers)
                if (GeneratedGuessIDs.Count >= ((UserGameSession)Session["userGameSession"]).numAnswers)
                {
                    //break out of the loop
                    WrongAnswersGenerationNOTcompleted = false;
                }
            }
        }

        [HttpPost]
        public ActionResult Play(int Score, string Time, string isCorrect)
        {
            bool BoolIsCorrect;

            //Update the user's score, progress, and time

            //If the isCorrect string is correctly parsed 
            if (Boolean.TryParse(isCorrect, out BoolIsCorrect))
            {
                //If the question was answered correctly
                if (BoolIsCorrect)
                {
                    //Increase the correct questions counter
                    ((UserGameSession)Session["userGameSession"]).numCorrect += 1;
                }
            }

            ((UserGameSession)Session["userGameSession"]).Score = Score;
            ((UserGameSession)Session["userGameSession"]).timeLeft = TimeSpan.Parse(Time);
            if (IsGameDone())
            {
                //If the user is done, break out of the loop and send the "Complete" action the final score               
                return RedirectToAction("Complete", new { Score = Score });
            }
            GenerateQuestionONEperPIC();
            currentGuess.CurrentQuestion = ((UserGameSession)Session["userGameSession"]).currentQuestion;
            currentGuess.TotalQuestions = ((UserGameSession)Session["userGameSession"]).numQuestions;
            currentGuess.NumCorrect = ((UserGameSession)Session["userGameSession"]).numCorrect;
            currentGuess.Score = Score;
            currentGuess.Time = ((UserGameSession)Session["userGameSession"]).timeLeft;
            return View(currentGuess);
        }

        /// <summary>
        /// Check if the user has completed the game or has run out of time. If the user is not done 
        /// and there are no more questions, reshuffle the PictureIndices and continue 
        /// </summary>
        /// <returns>Bool</returns>
        [NonAction]
        private bool IsGameDone()
        {
            currentModule = ((UserGameSession)Session["userGameSession"]).currentModule;

            //if the question or time limit has been reached
            if (((UserGameSession)Session["userGameSession"]).currentQuestion
                //subtract 1 from number of questions since currentQuestion is zero based
                >= (((UserGameSession)Session["userGameSession"]).numQuestions - 1)
                |
                (((UserGameSession)Session["userGameSession"]).timeLeft.CompareTo(new TimeSpan(0, 0, 0)) < 1))
            //if (((UserGameSession)Session["userGameSession"]).currentQuestion >= (((UserGameSession)Session["userGameSession"]).PictureIndices.Count))
            {
                return true;
            }
            else
            {
                //if the game is not over, and the list of pictures is exhausted, reshuffle. Otherwise, continue.
                if (((UserGameSession)Session["userGameSession"]).currentQuestion.IsDivisible(
                    ((UserGameSession)Session["userGameSession"]).PictureIndices.Count - 1))
                {
                    Reshuffle();
                }
            }
            return false;
        }

        public ActionResult Complete(int score)
        {
            newScore = new Score();
            SaveScore(score);
            ViewBag.ModuleID = ((UserGameSession)Session["userGameSession"]).currentModule.ModuleID;
            ViewBag.Top10Scores = TopTenScore.GetTopTenScores(((UserGameSession)Session["userGameSession"]).currentModule.ModuleID);
            return View(newScore);
        }

        [NonAction]
        private void SaveScore(int score)
        {
            newScore.CorrectAnswers = (score / 100);
            newScore.ModuleID = ((UserGameSession)Session["userGameSession"]).currentModule.ModuleID;
            newScore.UserID = User.Identity.GetUserId();
            newScore.TotalAnswers = ((UserGameSession)Session["userGameSession"]).numQuestions;

            db.Entry(newScore).State = EntityState.Added;
            db.SaveChanges();
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

        /// <summary>
        /// Check to make sure that there are enough answers to generate the required amount.
        /// If not, set the default number of answers to something that will not break the program.
        /// </summary>
        [NonAction]
        private void CheckMaxAnswers()
        {
            if (currentModule.Answers.Count <= ((UserGameSession)Session["userGameSession"]).numAnswers)
            {
                ((UserGameSession)Session["userGameSession"]).numAnswers = currentModule.Answers.Count - 2;
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

        #region Legacy Method GetAnswerID

        //private int GetAnswerID()
        //{
        //    foreach (Answer answer in currentModule.Answers)
        //    {
        //        foreach (Picture picture in answer.Pictures)
        //        {
        //            if (picture.PictureID == pictureID)
        //            {
        //                return answer.AnswerID;
        //            }
        //        }
        //    }
        //    // Defaults to 1 so error doesn't occur
        //    return 1;
        //}
        #endregion

        // Generates guess, only loops through each answer once, so only
        // one picture will be chosen per answer
        #region Legacy Method - GenerateGuessOnePerAnswer
        [NonAction]
        private void GenerateGuessONEperANS()
        {
            ((UserGameSession)Session["userGameSession"]).currentQuestion++;
            currentModule = ((UserGameSession)Session["userGameSession"]).currentModule;
            answerID = ((UserGameSession)Session["userGameSession"]).currentQuestion;
            pictureID = rand.Next(0, (currentModule.Answers.ElementAt(answerID).Pictures.Count - 1));

            //add the initial stuff to the guess to send over
            currentGuess.Answer = currentModule.Answers.ElementAt(answerID).AnswerString;
            currentGuess.ImageURL = currentModule.Answers.ElementAt(answerID).Pictures.ElementAt(pictureID).Location;
            currentGuess.possibleAnswers.Add(currentModule.Answers.ElementAt(answerID).AnswerString);

            //add the correct answer to the generated guess ids (to prevent duplicate entries)
            GeneratedGuessIDs.Add(answerID);

            //Generate a random selection of wrong answers and add them to the possible answers.
            GenerateWrongAnswers();

            //shuffle the list of possible answers so that the first answer isn't always the right one.
            currentGuess.possibleAnswers.Shuffle();
        }
        #endregion
    }
}