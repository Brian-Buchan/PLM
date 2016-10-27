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
        public List<AnsPicIndex> PictureIndices { get; set; }
        public ApplicationUser currentUser { get; set; }
        public int iteratedQuestion { get; set; }
        public Module currentModule { get; set; }
        public int defaultNumAnswer { get; set; }
        public int currentQuestion { get; set; }
        public TimeSpan timeLeft { get; set; }
        public int numQuestions { get; set; }
        public int numCorrect { get; set; }
        public int numAnswers { get; set; }
        public int Score { get; set; }
        public int time { get; set; }

        public UserGameSession()
        {
            currentModule = new Module();
            PictureIndices = new List<AnsPicIndex>();
            numCorrect = 0;
        }
    }
}