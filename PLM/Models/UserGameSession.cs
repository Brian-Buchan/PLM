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
        /// <summary>
        /// The user that is associated with the session
        /// </summary>
        public ApplicationUser currentUser { get; set; }
        
        /// <summary>
        /// The current score of the user
        /// </summary>
        public int Score { get; set; }
        
        /// <summary>
        /// The current question number the user is on.
        /// </summary>
        public int currentQuestion { get; set; }
        
        /// <summary>
        /// The user's current position in the PictureIndices list. Resets each loop through said list
        /// </summary>
        public int iteratedQuestion { get; set; }

        /// <summary>
        /// The module being played
        /// </summary>
        public Module currentModule { get; set; }

        /// <summary>
        /// The list of pictures with associated answers, used as the building block for questions
        /// </summary>
        public List<AnsPicIndex> PictureIndices { get; set; }
        
        /// <summary>
        /// The number of answers to be displayed each question
        /// </summary>
        public int numAnswers { get; set; }
        
        /// <summary>
        /// The number of questions to be asked during this game
        /// </summary>
        public int numQuestions { get; set; }

        /// <summary>
        /// The amount of time (in minutes) alloted for the module
        /// </summary>
        public int time { get; set; }

        /// <summary>
        /// The amount of time left in the module
        /// </summary>
        public TimeSpan timeLeft { get; set; }

        public UserGameSession()
        {
            currentModule = new Module();
            PictureIndices = new List<AnsPicIndex>();
        }
    }
}