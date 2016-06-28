using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace PLM
{
    public class Answer
    {
        public int AnswerID { get; set; }
        [Display(Name="Answer Text")]
        public string AnswerString { get; set; }
        [Display(Name="Module")]
        public int ModuleID { get; set; }
        public int PictureCount { get; set; }

        public Answer()
        {
            PictureCount = 0;
        }

        public virtual Module Module { get; set; }
        public virtual List<Picture> Pictures { get; set; }
    }
}