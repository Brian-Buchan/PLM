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
        public int Score { get; set; }
        public int currentGuess { get; set; }
        public Module currentModule { get; set; }
        public List<Picture> Pictures { get; set; }
        public List<AnsPicIndex> PictureIndicies { get; set; }
        public int numAnswers { get; set; }
        public int numQuestions { get; set; }
        public int time { get; set; }
        public ApplicationUser User;

        public UserGameSession()
        {
            currentModule = new Module();
            Pictures = new List<Picture>();
            PictureIndicies = new List<AnsPicIndex>();

        }
    }
}