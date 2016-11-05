using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace PLM
{
    public class PlayViewModel
    {
        public PLM.Models.GamePicture PictureToView { get; set; }
        public List<string> possibleAnswers { get; set; }
        public string Answer { get; set; }
        public int Score { get; set; }
        public TimeSpan Time { get; set; }
        public int CurrentQuestion { get; set; }
        public int TotalQuestions { get; set; }

        public PlayViewModel()
        {
            PictureToView = new Models.GamePicture();
            possibleAnswers = new List<string>();
            Score = 0;
        }
    }
}