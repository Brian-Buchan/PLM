using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
//TODO: COPY OVER
namespace PLM
{
    public class Repos : IDisposable
    {
        ApplicationDbContext _dc = new ApplicationDbContext();


        public Models.Report GetReportByID(int id)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "reportID",
                Value = id > 0 ? id : SqlInt32.Null
            };
            var _Item = _dc.Database.SqlQuery<Models.Report>(
                "Select * from Reports Where Reports.ID = @reportID", idParam
                ).Single<Models.Report>();
            return _Item;
        }
        
        public Module GetModuleByID(int id)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "ModuleID",
                Value = id
            };
            var module = _dc.Database.SqlQuery<Module>(
                "Select from Modules Where Modules.ModuleID = @ModuleID", idParam
                ).Single<Module>();
            return module;
        }
        
        public IEnumerable<Models.Report> GetReportList()
        {
            var _List = _dc.Database.SqlQuery<Models.Report>(
                "Select * from Reports"
                ).ToList<Models.Report>();
            return _List;
        }

        public IEnumerable<Models.Report> GetUserCreatedReportList(string userID)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "userID",
                Value = userID
            };
            var _List = _dc.Database.SqlQuery<Models.Report>(
                "Select * From Reports Where Report.UserID = @userID", idParam
                ).ToList<Models.Report>();
            return _List;
        }

        public IEnumerable<Module> GetModuleList()
        {
            var _List = _dc.Database.SqlQuery<Module>(
                "Select * from modules"
            ).ToList<Module>();
            return _List;
        }

        public IEnumerable<Top10Score> GetTop10Scores(int moduleID)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "moduleID",
                Value = moduleID
            };
            var _List = _dc.Database.SqlQuery<Top10Score>(
                "Select TOP 10 CONCAT(u.[FirstName], ' ', u.[LastName]) as Name, CONCAT(CAST(s.[CorrectAnswers] as VARCHAR(10)), '/', CAST(s.[TotalAnswers] as VARCHAR(10))) as Score, s.[TimeStamp] as Date from [dbo].[Scores] as s left outer join[dbo].[AspNetUsers] as u on s.UserID = u.ID Where S.ModuleID = 3 order by s.[CorrectAnswers] DESC", idParam
                ).ToList<Top10Score>();
            return _List;
        }

        public IEnumerable<Score> GetScoresByModuleID(int moduleID)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "moduleID",
                Value = moduleID
            };
            var _List = _dc.Database.SqlQuery<Score>(
                "Select * From Scores Where ModuleID = @moduleID", idParam
                ).ToList<Score>();
            return _List;
        }

        public IEnumerable<Score> GetScoreList()
        {
            var _List = _dc.Database.SqlQuery<Score>(
                "Select * From Scores"
                ).ToList<Score>();
            return _List;
        }

        public IEnumerable<AnswerViewModel> GetAnswerList(int moduleID = 0)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "moduleID",
                Value = moduleID > 0 ? moduleID : SqlInt32.Null
            };
            var _List = _dc.Database.SqlQuery<AnswerViewModel>(
                "Select AnswerID, AnswerString, Answers.ModuleID, Modules.Name as ModuleName from Answers join Modules on Answers.ModuleID = Modules.ModuleID Where Answers.ModuleID = @moduleID or @moduleID is null", idParam
            ).ToList<AnswerViewModel>();
            return _List;
        }

        public IEnumerable<Picture> GetPictureList(int answerID = 0)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "answerID",
                Value = answerID > 0 ? answerID : SqlInt32.Null
            };
            var _List = _dc.Database.SqlQuery<Picture>(
                "Select PictureID, AnswerID, Attribution, PictureData, Answers.AnswerString as AnswerString from Pictures join Answer on Picture.AnswerID = Answers.AnswerID Where Picture.AnswerID = @answerID or @answerID is null", idParam
            ).ToList<Picture>();
            return _List;
        }

        public IEnumerable<Module> GetCategoryList()
        {
            var _List = _dc.Database.SqlQuery<Module>(
                "Select * from Categories"
            ).ToList<Module>();
            return _List;
        }

        public IEnumerable<ModuleFilterMenuList> GetModuleFilterMenuList()
        {
            var _List = _dc.Database.SqlQuery<ModuleFilterMenuList>(
                "select c.[CategoryName], c.[CategoryID], count(m.ModuleID) as ModuleCount from[dbo].[Categories] as c left outer join[dbo].[Modules] as m on m.CategoryId = c.CategoryID group by c.[CategoryName], c.[CategoryID] order by c.[CategoryName]"
            ).ToList<ModuleFilterMenuList>();
            return _List;
        }

        public void Dispose()
        {

        }
    }
}