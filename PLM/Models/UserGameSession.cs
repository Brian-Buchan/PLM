using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLM
{
    public class UserGameSession
    {
        public int Score;
        public int currentGuess;
        public Module currentModule;
        public List<Picture> Pictures;
        public List<AnsPicIndex> PictureIndicies;

        public UserGameSession()
        {
            currentModule = new Module();
            Pictures = new List<Picture>();
            PictureIndicies = new List<AnsPicIndex>();
        }
    }
}