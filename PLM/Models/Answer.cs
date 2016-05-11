using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace PLM
{
    public class Answer
    {
        public int AnswerID { get; set; }
        public string AnswerString { get; set; }
        public int ModuleID { get; set; }

        public virtual Module Module { get; set; }
        public virtual List<Picture> Pictures { get; set; }
    }
}