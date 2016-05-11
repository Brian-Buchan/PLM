using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace PLM
{
    public class Picture
    {
        public int PictureID { get; set; }
        public string Location { get; set; }
        public int AnswerID { get; set; }

        public virtual Answer Answer { get; set; }
    }
}