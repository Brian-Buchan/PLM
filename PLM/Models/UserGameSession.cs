using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace PLM
{
    public class UserGameSession
    {
        // Logged in user - allowed to be null
        public ApplicationUser currentUser { get; set; }
        // Module to be played
        public Module currentModule { get; set; }
        public GameModule gameModule { get; set; }
        public GameSettings GameSettings { get; set; }

        public TimeSpan timeLeft { get; set; }
        public int time { get; set; }
        public int Score { get; set; }
        public int currentQuestion { get; set; }

        public int iteratedQuestion { get; set; }
        public List<AnsPicIndex> PictureIndices { get; set; }
        public int defaultNumAnswer { get; set; }
        public int numQuestions { get; set; }
        public int numAnswers { get; set; }
        public int numCorrect { get; set; }

        public UserGameSession()
        {
            //currentModule = new Module();
            //numCorrect = 0;

            //PictureIndices = new List<AnsPicIndex>();
        }
    }
}