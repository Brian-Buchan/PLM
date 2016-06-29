using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLM
{
    public class PlayViewModel
    {
        public string ImageURL { get; set; }
        public string Attribution { get; set; }
        public List<string> possibleAnswers { get; set; }
        public string Answer { get; set; }
        public int Score { get; set; }
        public TimeSpan Time { get; set; }
        public int CurrentQuestion { get; set; }
        public int NumCorrect { get; set; }
        public int TotalQuestions { get; set; }

        public PlayViewModel()
        {
            possibleAnswers = new List<string>();
            Score = 0;
        }
    }
}