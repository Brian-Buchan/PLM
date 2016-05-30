using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        private UserGameSession currentGameSession;
        private ApplicationDbContext db = new ApplicationDbContext();
        private Module currentModule = new Module();
        private List<int> GeneratedGuessIDs = new List<int>();
        private PlayViewModel currentGuess = new PlayViewModel();
        private int currentGuessNum;

        private bool PLMgenerated = false;
        private bool WrongAnswersGenerationNOTcompleted = true;

        private int answerID;
        private int pictureID;
        //private int DefaultNumAnswers = 12;

        //Module currentModule;
        //
        // GET: /Game/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Complete(int score)
        {
            ViewBag.UserID = User.Identity.GetUserId();
            ViewBag.ModuleID = currentModule.ModuleID;
            //SaveScore(score);

            return View(score);
        }

        private void SaveScore(int score)
        {
            Score newScore = new Score();
            newScore.CorrectAnswers = (score / 100);
            newScore.Module = currentModule;

            db.Scores.Add(newScore);
            db.SaveChanges();
        }

        public ActionResult Play(int? PLMid)
        {
            GenerateGuessONEperPIC();
            return View(currentGuess);
        }

        [HttpPost]
        public ActionResult Play(int Score)
        {
            ((UserGameSession)Session["userGameSession"]).Score = Score;
            if (IsGameDone())
            {
                return RedirectToAction("Complete", new { Score = Score });
            }
            GenerateGuessONEperPIC();
            currentGuess.Score = Score;
            return View(currentGuess);
        }

        [HttpGet]
        public ActionResult Setup(int? PLMid)
        {
            int IDtoPASS = 1;
            if (PLMid != null)
        {
                // Attempts to set nullable value, If null sets to itself (DEFAULT IS 0 - AMERICAN GEO PLM)
                IDtoPASS = PLMid ?? 1;
            }

            if (PLMgenerated == false)
                GenerateModule(IDtoPASS);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setup([Bind(Include="numAnswers,numQuestions,time")] UserGameSession ugs)
        {
            ((UserGameSession)Session["userGameSession"]).numAnswers = ugs.numAnswers;
            ((UserGameSession)Session["userGameSession"]).numQuestions = ugs.numQuestions;
            ((UserGameSession)Session["userGameSession"]).time = ugs.time;
            return RedirectToAction("Play");
        }

        private void CheckMaxGuesses()
        {
            if (currentModule.Answers.Count <= currentModule.DefaultNumAnswers)
            {
                currentModule.DefaultNumAnswers = currentModule.Answers.Count - 2;
            }
        }

        private bool IsGameDone()
        {
            currentModule = ((UserGameSession)Session["userGameSession"]).currentModule;
            
            if (((UserGameSession)Session["userGameSession"]).currentGuess
                //subtract 1 from number of questions since current guess is zero based
                >= (((UserGameSession)Session["userGameSession"]).numQuestions -1))
            //if (((UserGameSession)Session["userGameSession"]).currentGuess >= (((UserGameSession)Session["userGameSession"]).PictureIndicies.Count))
            {
                return true;
            }
            else
            {
                //if the game is not over, and the list of pictures is exhausted
                if (((UserGameSession)Session["userGameSession"]).currentGuess.IsDivisible(
                    ((UserGameSession)Session["userGameSession"]).PictureIndicies.Count - 1))
                {
                    //shuffle the picture indices
                    ((UserGameSession)Session["userGameSession"]).PictureIndicies.Shuffle();
                    //reset the iterated guess counter
                    ((UserGameSession)Session["userGameSession"]).iteratedGuess = -1;
                }
            }
                return false;
        }

        private void GenerateModule(int PLMid)
        {
            currentGameSession = new UserGameSession();
            currentGameSession.currentModule = db.Modules.Find(PLMid);
            currentGameSession.Score = 0;

            // set to -1 because GenerateGuess() will increment it to 0 the first time it runs
            currentGameSession.currentGuess = -1;
            currentGameSession.iteratedGuess = -1;
            int answerIndex = -1;
            int pictureIndex;
            foreach (Answer answer in currentGameSession.currentModule.Answers)
            {
                answerIndex++;
                pictureIndex = -1;

                foreach (Picture picture in answer.Pictures)
                {
                    pictureIndex++;
                    //currentGameSession.Pictures.Add(picture);
                    currentGameSession.PictureIndicies.Add(new AnsPicIndex(answerIndex, pictureIndex, picture));
                }
            }
            // Shuffle the list of pictures so Users itterate through them randomly
            currentGameSession.PictureIndicies.Shuffle();
            Session["userGameSession"] = currentGameSession;
        }

        // Generates Guess, loops through each picture in each answer
        // the same answer will be chosen multiple times with different pictures
        private void GenerateGuessONEperPIC()
        {
            //increment guess counters
            ((UserGameSession)Session["UserGameSession"]).currentGuess += 1;
            ((UserGameSession)Session["UserGameSession"]).iteratedGuess += 1;
            currentGuessNum=((UserGameSession)Session["UserGameSession"]).iteratedGuess;
            //currentGuessNum = (((UserGameSession)Session["userGameSession"]).currentGuess++);
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
            currentGuess.Attribution = currentModule.Answers.ElementAt(answerIndex).Pictures.ElementAt(pictureID).Attribution;

            GeneratedGuessIDs.Add(answerIndex);
            GenerateWrongAnswers();

            currentGuess.possibleAnswers.Shuffle();
        }

        private int[] GetPictureID(int currentGuessNum)
        {
            AnsPicIndex IndexItem = ((UserGameSession)Session["userGameSession"]).PictureIndicies.ElementAt(currentGuessNum);
            return new int[] { IndexItem.AnswerIndex, IndexItem.PictureIndex };
            #region legacy code
            //Picture currentPicture = ((UserGameSession)Session["userGameSession"]).Pictures.ElementAt(currentGuessNum);
            //int AnswerTrackerIndex = -1;
            //int PictureTrackerIndex;

            //foreach (Answer answer in currentModule.Answers)
            //{
            //    AnswerTrackerIndex++;
            //    PictureTrackerIndex = -1;
            //    foreach (Picture picture in answer.Pictures)
            //    {
            //        PictureTrackerIndex++;
            //        if ((picture.PictureID == currentPicture.PictureID))
            //        {
            //            return new int[] { picture.PictureID, PictureTrackerIndex, AnswerTrackerIndex };
            //        }
            //    }
            //}
            //return new int[] { 1, 1, 1 }; 
            #endregion
        }

        #region Legacy Method
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
        private void GenerateGuessONEperANS()
        {
            ((UserGameSession)Session["userGameSession"]).currentGuess++;
            currentModule = ((UserGameSession)Session["userGameSession"]).currentModule;
            answerID = ((UserGameSession)Session["userGameSession"]).currentGuess;
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

        private void GenerateWrongAnswers()
        {
            int wrongAnswerID;
            //while we still have work to do
            while (WrongAnswersGenerationNOTcompleted)
            {
                CheckMaxGuesses();
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
    }
}