using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace PLM
{
    public class CascadeDeleter
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static void DeleteModule(int moduleID)
        {
            Module module = db.Modules.Find(moduleID);

            for (int m = module.Answers.Count; m > 0; m--)
            {
                Answer ansToDelete = db.Answers.Find(module.Answers.ElementAt(m - 1).AnswerID);
                DeleteAnswer(ansToDelete.AnswerID);
            }

            db.Modules.Remove(module);

            Directory.Delete("/PerceptualLearning/Content/Images/PLM/" + module.Name);
            db.SaveChanges();
        }

        public static void DeleteAnswer(int answerID)
        {
            Answer answer = db.Answers.Find(answerID);

            for (int i = answer.Pictures.Count; i > 0; i--)
            {
                Picture picToDelete = db.Pictures.Find(answer.Pictures.ElementAt(i - 1).PictureID);
                db.Pictures.Remove(picToDelete);
                File.Delete(picToDelete.Location);
            }

            db.Answers.Remove(answer);
            db.SaveChanges();

        }
    }
}