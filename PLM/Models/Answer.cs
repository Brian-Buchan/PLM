using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLM
{
    public class Answer
    {
        public int AnswerID { get; set; }
        [Display(Name="Answer Text")]
        [MaxLength(25)]
        [Required]
        [Index("IX_AnswerStringModuleId", 2, IsUnique = true)]
        public string AnswerString { get; set; }
        [Display(Name="Module")]
        [Index("IX_AnswerStringModuleId", 1, IsUnique = true)]
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