using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLM
{
    public class Module
    {
        public int ModuleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual int CategoryId { get; set; }
        public int DefaultNumAnswers { get; set; }
        public int DefaultTime { get; set; }
        public int DefaultNumPictures { get; set; }
        public virtual List<Answer> Answers { get; set; }
        public virtual ApplicationUser User { get; set; }
        public bool isPrivate { get; set; }
    }
}