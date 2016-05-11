using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLM
{
    public class AnsPicIndex
    {
        public int AnswerIndex;
        public int PictureIndex;
        public Picture PictureOBJ;

        public AnsPicIndex(int ans, int pic, Picture picOBJ)
        {
            AnswerIndex = ans;
            PictureIndex = pic;
            PictureOBJ = picOBJ;
        }
    }
}