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

        public SqlParameter NullChecker(SqlParameter param)
        {
            if (param.Value == null)
            {
                return new SqlParameter { ParameterName = param.ParameterName, Value = DBNull.Value };
            }
            return param;
        }

        #region CREATE
        public bool AddAnswer(Answer answer)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "AnswerString",
                Value = answer.AnswerString
            };
            var idParam1 = new SqlParameter
            {
                ParameterName = "ModuleID",
                Value = answer.ModuleID
            };
            try
            {
                _dc.Database.ExecuteSqlCommand(
                    "INSERT INTO [dbo].[Answers] ([AnswerString],[ModuleID]) VALUES (@AnswerString, @ModuleID)", idParam, idParam1
                    );
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool AddPicture(Picture picture)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "Location",
                Value = picture.Location
            };
            var idParam1 = new SqlParameter
            {
                ParameterName = "AnswerID",
                Value = picture.AnswerID
            };
            var idParam2 = new SqlParameter
            {
                ParameterName = "Attribution",
                Value = picture.Attribution
            };
            var idParam3 = new SqlParameter
            {
                ParameterName = "PictureData",
                Value = picture.PictureData
            };
            try
            {
                _dc.Database.ExecuteSqlCommand(
                    "INSERT INTO Pictures (Location, AnswerID, Attribution, PictureData) VALUES (@Location, @AnswerID, @Attribution, @PictureData)", idParam, idParam1, idParam2, idParam3
                    );
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool AddCategory(Category category)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "categoryName",
                Value = category.CategoryName
            };
            try
            {
                _dc.Database.ExecuteSqlCommand(
                    "INSERT INTO Categories (CategoryName) VALUES (@CategoryName)", idParam
                    );
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool AddModule(Module module)
        {
            var idParam = new SqlParameter { ParameterName = "Name", Value = module.Name.ToString(), DbType=System.Data.DbType.String };
            var idParam1 = NullChecker(new SqlParameter { ParameterName = "Description", Value = module.Description, DbType = System.Data.DbType.String });
            var idParam2 = new SqlParameter { ParameterName = "CategoryID", Value = module.CategoryId };
            var idParam3 = new SqlParameter { ParameterName = "DefaultNumAnswers", Value = module.DefaultNumAnswers };
            var idParam4 = new SqlParameter { ParameterName = "DefaultTime", Value = module.DefaultTime };
            var idParam5 = new SqlParameter { ParameterName = "DefaultNumQuestions", Value = module.DefaultNumQuestions };
            var idParam6 = new SqlParameter { ParameterName = "isPrivate", Value = module.isPrivate };
            var idParam7 = new SqlParameter { ParameterName = "user_Id", Value = module.User.Id };
            var idParam8 = NullChecker(new SqlParameter { ParameterName = "rightAnswerString", Value = module.rightAnswerString, DbType = System.Data.DbType.String });
            var idParam9 = NullChecker(new SqlParameter { ParameterName = "wrongAnswerString", Value = module.wrongAnswerString, DbType = System.Data.DbType.String });
            var idParam10 = new SqlParameter { ParameterName = "isDisabled", Value = module.isDisabled };
            var idParam11 = NullChecker(new SqlParameter { ParameterName = "DisableModuleNote", Value = module.DisableModuleNote, DbType = System.Data.DbType.String });
            var idParam12 = NullChecker(new SqlParameter { ParameterName = "DisableReason", Value = module.DisableReason });
            //try
            //{
                _dc.Database.ExecuteSqlCommand(
                    "INSERT INTO Modules(Name, Description, CategoryId, DefaultNumAnswers, DefaultTime, DefaultNumQuestions, isPrivate, User_Id, rightAnswerString, wrongAnswerString, isDisabled, DisableModuleNote, DisableReason) VALUES (@Name, @Description, @CategoryId, @DefaultNumAnswers, @DefaultTime, @DefaultNumQuestions, @isPrivate, @User_Id, @rightAnswerString, @wrongAnswerString, @isDisabled, @DisableModuleNote, @DisableReason)",
                    idParam, idParam1, idParam2, idParam3, idParam4, idParam5, idParam6, idParam7, idParam8, idParam9, idParam10, idParam11, idParam12
                    );
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
            return true;
        }

        public bool AddScore(Score score)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "CorrectAnswers",
                Value = score.CorrectAnswers
            };
            var idParam1 = new SqlParameter
            {
                ParameterName = "TotalAnswers",
                Value = score.TotalAnswers
            };
            var idParam2 = new SqlParameter
            {
                ParameterName = "TimeStamp",
                Value = score.TimeStamp
            };
            var idParam3 = new SqlParameter
            {
                ParameterName = "UserID",
                Value = score.UserID
            };
            var idParam4 = new SqlParameter
            {
                ParameterName = "ModuleID",
                Value = score.ModuleID
            };
            //try
            //{
            var rc =     _dc.Database.ExecuteSqlCommand(
                    "INSERT INTO Scores (CorrectAnswers, TotalAnswers, TimeStamp, UserID, ModuleID) VALUES (@CorrectAnswers, @TotalAnswers, @TimeStamp, @UserID, @ModuleID)", idParam, idParam1, idParam2, idParam3, idParam4
                    );
           
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
            return true;
        }
        #endregion

        #region READ

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
                "Select * from Modules Where Modules.ModuleID = @ModuleID", idParam
                ).Single<Module>();
            return module;
        }

        public Answer GetAnswerByID(int id)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "AnswerID",
                Value = id
            };
            var answer = _dc.Database.SqlQuery<Answer>(
                "Select * from Answers Where AnswerID = @AnswerID", idParam
                ).Single();
            return answer;
        }

        public Picture GetPictureByID(int id)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "PictureID",
                Value = id
            };
            var picture = _dc.Database.SqlQuery<Picture>(
                "Select * from Pictures Where PictureID = @PictureID", idParam
                ).Single();
            return picture;
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
                "Select TOP 10 CONCAT(u.[FirstName], ' ', u.[LastName]) as Name, CONCAT(CAST(s.[CorrectAnswers] as VARCHAR(10)), '/', CAST(s.[TotalAnswers] as VARCHAR(10))) as Score, s.[TimeStamp] as Date from [dbo].[Scores] as s left outer join[dbo].[AspNetUsers] as u on s.UserID = u.ID Where S.ModuleID = @moduleID order by s.[CorrectAnswers] DESC", idParam
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

        public IEnumerable<AnswerViewModel> GetAnswerViewList(int moduleID = 0)
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

        public IEnumerable<Answer> GetAnswerList(int moduleID = 0)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "moduleID",
                Value = moduleID > 0 ? moduleID : SqlInt32.Null
            };
            var _List = _dc.Database.SqlQuery<Answer>(
                "Select * from Answers Where Answers.ModuleID = @moduleID or @moduleID is null", idParam
            ).ToList<Answer>();
            return _List;
        }

        public IEnumerable<Picture> GetViewBagPictureList(int answerID = 0)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "answerID",
                Value = answerID > 0 ? answerID : SqlInt32.Null
            };
            var _List = _dc.Database.SqlQuery<Picture>(
                "Select p.PictureID, p.AnswerID, p.Location, p.Attribution, p.PictureData, a.AnswerString from Pictures p join Answers a on p.AnswerID = a.AnswerID Where p.AnswerID = @answerID or @answerID is null", idParam
            ).ToList<Picture>();
            return _List;
        }

        public IEnumerable<Picture> GetAllPictures()
        {
            var _List = _dc.Database.SqlQuery<Picture>(
                "Select * from Pictures"
            ).ToList();
            return _List;
        }

        public IEnumerable<Picture> GetPicturesByAnswerID(int answerID = 0)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "AnswerID",
                Value = answerID
            };
            var _List = _dc.Database.SqlQuery<Picture>(
                "Select * from Pictures where AnswerID = @AnswerID", idParam
                ).ToList<Picture>();
            return _List;
        }

        public IEnumerable<Category> GetCategoryList()
        {
            var _List = _dc.Database.SqlQuery<Category>(
                "Select * from Categories"
            ).ToList<Category>();
            return _List;
        }

        public Category GetCategoryByID(int id = 0)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "categoryID",
                Value = id
            };
            var category = _dc.Database.SqlQuery<Category>(
                "Select * from Categories Where CategoryID = @categoryID", idParam
                ).Single<Category>();
            return category;
        }

        public IEnumerable<ModuleFilterMenuList> GetModuleFilterMenuList()
        {
            var _List = _dc.Database.SqlQuery<ModuleFilterMenuList>(
                "select c.[CategoryName], c.[CategoryID], count(m.ModuleID) as ModuleCount from[dbo].[Categories] as c left outer join[dbo].[Modules] as m on m.CategoryId = c.CategoryID group by c.[CategoryName], c.[CategoryID] order by c.[CategoryName]"
            ).ToList<ModuleFilterMenuList>();
            return _List;
        }
        #endregion

        #region UPDATE
        public bool UpdateAnswer(Answer answer)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "AnswerString",
                Value = answer.AnswerString
            };
            var idParam1 = new SqlParameter
            {
                ParameterName = "ModuleID",
                Value = answer.ModuleID
            };
            var idParam2 = new SqlParameter
            {
                ParameterName = "AnswerID",
                Value = answer.AnswerID
            };
            try
            {
                _dc.Database.ExecuteSqlCommand(
                    "UPDATE Answers SET AnswerString = @AnswerString, ModuleID = @ModuleID WHERE AnswerID = @AnswerID", idParam, idParam1, idParam2
                    );
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool UpdatePicture(Picture picture)
        {
            var idParam = NullChecker(new SqlParameter
            {
                ParameterName = "Location",
                Value = picture.Location
            });
            var idParam1 = new SqlParameter
            {
                ParameterName = "AnswerID",
                Value = picture.AnswerID
            };
            var idParam2 = NullChecker(new SqlParameter
            {
                ParameterName = "Attribution",
                Value = picture.Attribution
            });
            var idParam3 = new SqlParameter
            {
                ParameterName = "PictureData",
                Value = picture.PictureData
            };
            var idParam4 = new SqlParameter
            {
                ParameterName = "PictureID",
                Value = picture.PictureID
            };
            try
            {
                _dc.Database.ExecuteSqlCommand(
                    "UPDATE Pictures SET Location = @Location, AnswerID = @AnswerID, Attribution = @Attribution, PictureData = @PictureData Where PictureID = @PictureID", idParam, idParam1, idParam2, idParam3, idParam4
                    );
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool UpdateModule(Module module)
        {
            var idParam = new SqlParameter { ParameterName = "Name", Value = module.Name };
            var idParam1 = new SqlParameter { ParameterName = "Description", Value = module.Description };
            var idParam2 = new SqlParameter { ParameterName = "CategoryID", Value = module.CategoryId };
            var idParam3 = new SqlParameter { ParameterName = "DefaultNumAnswers", Value = module.DefaultNumAnswers };
            var idParam4 = new SqlParameter { ParameterName = "DefaultTime", Value = module.DefaultTime };
            var idParam5 = new SqlParameter { ParameterName = "DefaultNumQuestions", Value = module.DefaultNumQuestions };
            var idParam6 = new SqlParameter { ParameterName = "isPrivate", Value = module.isPrivate };
            var idParam7 = new SqlParameter { ParameterName = "user_Id", Value = module.User.Id };
            var idParam8 = new SqlParameter { ParameterName = "rightAnswerString", Value = module.rightAnswerString };
            var idParam9 = new SqlParameter { ParameterName = "wrongAnswerString", Value = module.wrongAnswerString };
            var idParam10 = new SqlParameter { ParameterName = "isDisabled", Value = module.isDisabled };
            var idParam11 = new SqlParameter { ParameterName = "DisableModuleNote", Value = module.DisableModuleNote };
            var idParam12 = new SqlParameter { ParameterName = "DisableReason", Value = module.DisableReason };
            var idParam13 = new SqlParameter { ParameterName = "ModuleID", Value = module.ModuleID };
            try
            {
                _dc.Database.ExecuteSqlCommand(
                    "Update Modules SET Name = @Name, Description = @Description, CategoryId = @CategoryID, DefaultNumAnswers = @DefaultNumAnswers, DefaultTime = @DefaultTime, DefaultNumQuestions = @DefaultNumQuestions, isPrivate = @isPrivate, User_Id = @User_Id, rightAnswerString = @rightAnswerString, wrongAnswerString = @wrongAnswerString, isDisabled = @isDisabled, DisableModuleNote = @DisableModuleNote, DisableReason = @DisableReason where ModuleID = @ModuleID",
                    idParam, idParam1, idParam2, idParam3, idParam4, idParam5, idParam6, idParam7, idParam8, idParam9, idParam10, idParam11, idParam12, idParam13
                    );
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }

        public bool UpdateCategory(Category category)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "CategoryName",
                Value = category.CategoryName
            };
            var idParam1 = new SqlParameter
            {
                ParameterName = "CategoryID",
                Value = category.CategoryID
            };
            try
            {
                _dc.Database.ExecuteSqlCommand(
                    "UPDATE Categories SET CategoryName = @CategoryName WHERE CategoryID = @CategoryID", idParam, idParam1
                    );
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region DELETE
        public bool DeleteModule(int ModuleID)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "ModuleID",
                Value = ModuleID
            };
            try
            {
                _dc.Database.ExecuteSqlCommand(
                    "DELETE FROM modules WHERE ModuleID = @ModuleID", idParam
                    );
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool DeleteAnswer(int answerID)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "AnswerID",
                Value = answerID
            };
            try
            {
                _dc.Database.ExecuteSqlCommand(
                    "DELETE FROM Answers WHERE AnswerID = @AnswerID", idParam
                    );
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool DeletePicture(int pictureID)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "PictureID",
                Value = pictureID
            };
            try
            {
                _dc.Database.ExecuteSqlCommand(
                    "DELETE FROM Pictures WHERE PictureID = @PictureID", idParam
                    );
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool DeleteCategory(int categoryID)
        {
            var idParam = new SqlParameter
            {
                ParameterName = "CategoryID",
                Value = categoryID
            };
            try
            {
                _dc.Database.ExecuteSqlCommand(
                    "DELETE FROM Categories WHERE CategoryID = @CategoryID", idParam
                    );
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion



        public int GetNextFAQSortOrder()
        {
            int rc = 0;
            try
            {
                IQueryable<FAQ> _f = _dc.FAQs;
                
                rc = _f.Count() + 1;

            }
            catch (Exception e)
            {
                // 
                // e.Message;
                //TODO: do something with errors
                rc = 0;
            }

            return rc;

        }
        public int ReOrderFAQ( FAQ _faq, int newSortOrder )
        {
            int rc = 0;
            var currentid = _faq.Id;
            string sql = "";
            try
            {
                var idParam = new SqlParameter
                {
                    ParameterName = "id",
                    Value = currentid,
                    DbType = System.Data.DbType.Int32
                };

                var sortParam = new SqlParameter
                {
                    ParameterName = "newSortOrder",
                    Value = newSortOrder,
                    DbType = System.Data.DbType.Int32
                };

                var sortParam2 = new SqlParameter
                {
                    ParameterName = "newSortOrder",
                    Value = newSortOrder,
                    DbType = System.Data.DbType.Int32
                };
                // update sort moving everything to high number ge new sort order
                sql = "update FAQs set SortOrder += 1000000 where SortOrder >= @newSortOrder and SortOrder < 1000000  ";
                rc = _dc.Database.ExecuteSqlCommand(sql, sortParam2);

                // set sort order for item
                sql = "update FAQs set SortOrder = @newSortOrder where Id = @id ";
                rc = _dc.Database.ExecuteSqlCommand(sql, sortParam, idParam);
                // reorder
                sql = "UPDATE x SET x.SortOrder = x.newsort from ( SELECT a.SortOrder, ROW_NUMBER() OVER (ORDER BY a.SortOrder) AS newsort       FROM FAQs a 	  join FAQs b 	   on a.Id = b.Id 	   ) x";
                rc = _dc.Database.ExecuteSqlCommand(sql);

            }
            catch (Exception e)
            {
                // 
                sql= e.Message;
                //TODO: do something with errors
                rc = 0;
            }

            return rc;

        }


        public void Dispose()
        {

        }
    }
}