using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace PLM
{
    public class PlayViewModel
    {
        public string ImageURL { get; set; }
        [MaxLength(25)]
        public string Attribution { get; set; }
        public List<string> possibleAnswers { get; set; }
        [MaxLength(25)]
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