using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLM
{
    public class GameModule
    {
        public int ModuleID { get; set; }
        public string Name { get; set; }
        public List<GameAnswer> Answers { get; set; }

        // Put in a GameSettings object
        public int DefaultNumAnswers { get; set; }
        public int DefaultTime { get; set; }
        public int DefaultNumQuestions { get; set; }

        // Put in a InstructorSettings object
        public string rightAnswerString { get; set; }
        public string wrongAnswerString { get; set; }

        public GameModule()
        {

        }

        public GameModule(Module module)
        {
            ModuleID = module.ModuleID;
            Name = module.Name;
            DefaultNumAnswers = module.DefaultNumAnswers;
            DefaultNumQuestions = module.DefaultNumQuestions;
            DefaultTime = module.DefaultTime;
            using (Repos repo = new Repos())
            {
                module.Answers = repo.GetAnswerList(module.ModuleID).ToList();
            }
            Answers = ConvertAnswers(module.Answers);
            rightAnswerString = module.rightAnswerString;
            wrongAnswerString = module.wrongAnswerString;
        }

        private List<GameAnswer> ConvertAnswers(List<Answer> answers)
        {
            List<GameAnswer> gAnswers = new List<GameAnswer>();
            foreach (Answer answer in answers)
            {
                gAnswers.Add(new GameAnswer(answer));
            }
            return gAnswers;
        }
    }
}