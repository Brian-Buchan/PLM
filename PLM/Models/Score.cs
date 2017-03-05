using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace PLM
{
    public class Score
    {   [Key]
        public int ID { get; set; }

        [Display(Name = "ModuleID")]
        public int ModuleID { get; set; }

        [Display(Name = "UserID")]
        public string UserID { get; set; }

        [Display(Name = "Correct Answers")]
        public int CorrectAnswers { get; set; }

        [Display(Name = "Total Answers")]
        public int TotalAnswers { get; set; }

        [Display(Name = "Time Completed")]
        public DateTime TimeStamp { get; set; }

        public Score()
        {
            TimeStamp = DateTime.Now;
        }

        public Score(int score, int moduleID, int answers)
        {
            CorrectAnswers = (score / 100);
            ModuleID = moduleID;
            TotalAnswers = answers;
            TimeStamp = DateTime.Now;
        }

        public Score(int score, int moduleID, int answers, string userID)
        {
            CorrectAnswers = (score / 100);
            ModuleID = moduleID;
            TotalAnswers = answers;
            UserID = UserID;
            TimeStamp = DateTime.Now;
        }
    }
}