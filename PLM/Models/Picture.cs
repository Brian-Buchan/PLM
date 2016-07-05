using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace PLM
{
    public class Picture
    {
        public int PictureID { get; set; }
        public string Location { get; set; }
        public int AnswerID { get; set; }
        [MaxLength(40)]
        public string Attribution { get; set; }

        public virtual Answer Answer { get; set; }
    }
}