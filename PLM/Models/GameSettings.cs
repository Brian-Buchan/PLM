using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLM
{
    public class GameSettings
    {
        public int Time { get; set; }
        public int Questions { get; set; }
        public int Answers { get; set; }

        public GameSettings()
        {

        }

        public GameSettings(int time, int questions, int answers)
        {
            Time = time;
            Questions = questions;
            Answers = answers;
        }
    }
}