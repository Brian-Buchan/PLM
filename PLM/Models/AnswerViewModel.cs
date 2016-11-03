using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLM
{
    public class AnswerViewModel
    {
        public int AnswerID { get; set; }
        public string AnswerString { get; set; }
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }

        public AnswerViewModel()
        {

        }
    }
}