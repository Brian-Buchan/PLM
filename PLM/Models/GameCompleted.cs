using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLM
{
    public class GameCompleted
    {
        public int correctAnswers { get; set; }
        public int totalQuestions { get; set; }
        public string errorMessage { get; set; }
        public List<Top10Score> Top10Scores { get; set; }

        public GameCompleted()
        {
            Top10Scores = new List<Top10Score>();
        }
    }
}