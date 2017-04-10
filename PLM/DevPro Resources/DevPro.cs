using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PLM
{
    public static class DevPro
    {
        //public static string connectionStringName = "Azure";
        //public static string baseFileDirectory = "NeedToRemoveFeature";

        //public static string connectionStringName = "Development";
        //public static string baseFileDirectory = "/PerceptualLearningDevelopment/Content/Images/";

        //public static string connectionStringName = "Production";
        //public static string baseFileDirectory = "/PerceptualLearning/Content/Images/";

        public static string connectionStringName = "DAR";
        public static string baseFileDirectory = "none";

        //CHANGE IN: ImageEditorProd.js
        //In Scripts Folder
        //ADD: [NonAction] to all actionresult stuff in ImageEditorController
    }
}

/*
 -- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
ALTER FUNCTION [dbo].[getUIDfromEmail]
(
	-- Add the parameters for the function here
	@email varchar(128)
)
RETURNS uniqueidentifier
AS
BEGIN
	-- Declare the return variable here
	DECLARE @uid uniqueidentifier 
	

	-- Add the T-SQL statements to compute the return value here
	SELECT @uid = u.Id from AspNetUsers u where u.Email = @email
	;
	 

	-- Return the result of the function
	RETURN @uid;

END


    -- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
ALTER FUNCTION [dbo].[GetValidModuleCount]
(
	@catid int
	
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar int

	-- Add the T-SQL statements to compute the return value here
	SELECT @ResultVar = count(*) from Modules m
	 outer apply (
			select dbo.IsValidModule(v.ModuleID) isvalid from Modules v
			where  v.ModuleID = m.ModuleID
	) v
	 where
	  m.CategoryId = @catid and
	  v.isvalid = 'True'

	-- Return the result of the function
	RETURN @ResultVar

END

    
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
ALTER FUNCTION [dbo].[IsValidModule]
(
	-- Add the parameters for the function here
	@ModuleID int
)
RETURNS bit
AS
BEGIN
	-- Declare the return variable here
	DECLARE  @Result bit;
	DECLARE @c int, @p int;
	set @Result = 'False';
	-- Add the T-SQL statements to compute the return value here

	SELECT @c = count(*) from Answers a 
	  outer apply (
	    select count(*) picCount from Pictures p 
		  where p.AnswerID = a.AnswerID
	  ) p
	   where a.ModuleID = @ModuleID
	     and p.picCount > 0;

	if @c >= 5 set @Result = 'True';     

	-- Return the result of the function
	RETURN  @Result;

END

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[AddRoles] 
	-- Add the parameters for the stored procedure here
	@uid uniqueidentifier, 
	@mode int
AS
BEGIN
	
	SET NOCOUNT ON;

    if @mode = 99 begin -- SUPERADMIN

		INSERT INTO [dbo].[AspNetUserRoles]
           ([UserId]
           ,[RoleId])
		   select @uid, r.id from [dbo].[AspNetRoles] r
		     where  r.Name in ('Admin', 'Instructor', 'UserAdmin')
             and @uid not in  (
                   select x.id  from [dbo].[AspNetRoles] x
		           where  x.Name = r.Name
				)


 
	end
	
END


     
     
 */

/*

   namespace PLM.Migrations
{
   using System;
   using System.Data.Entity.Migrations;

   public partial class usr : DbMigration
   {
       public override void Up()
       {
           DropTable("dbo.DisableModuleViewModels");
       }

       public override void Down()
       {
           CreateTable(
               "dbo.DisableModuleViewModels",
               c => new
                   {
                       Name = c.String(nullable: false, maxLength: 128),
                       ModuleID = c.Int(nullable: false),
                       isDisabled = c.Boolean(nullable: false),
                       DisableModuleNote = c.String(),
                       DisableReason = c.Int(nullable: false),
                   })
               .PrimaryKey(t => t.Name);

       }
   }
}

*/
