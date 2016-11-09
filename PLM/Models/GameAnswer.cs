using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLM
{
    public class GameAnswer
    {
        public string AnswerString { get; set; }
        public List<Models.GamePicture> Pictures { get; set; }
        public bool Usable { get; set; }

        public GameAnswer()
        {

        }

        public GameAnswer(Answer answer)
        {
            Usable = true; ;
            AnswerString = answer.AnswerString;
            Pictures = ConvertPictures(answer.Pictures);
        }

        private List<Models.GamePicture> ConvertPictures(List<Picture> pictures)
        {
            List<Models.GamePicture> gPictures = new List<Models.GamePicture>();
            foreach (Picture picture in pictures)
            {
                gPictures.Add(new Models.GamePicture(picture));
            }
            return gPictures;
        }
    }
}