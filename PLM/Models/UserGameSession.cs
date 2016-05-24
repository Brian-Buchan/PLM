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
        public int Score;
        public int currentGuess;
        public Module currentModule;
        public List<Picture> Pictures;
        public List<AnsPicIndex> PictureIndicies;
        public ApplicationUser User;

        public UserGameSession()
        {
            currentModule = new Module();
            Pictures = new List<Picture>();
            PictureIndicies = new List<AnsPicIndex>();
        }
    }
}