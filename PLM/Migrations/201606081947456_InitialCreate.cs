namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        AnswerID = c.Int(nullable: false, identity: true),
                        AnswerString = c.String(),
                        ModuleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AnswerID)
                .ForeignKey("dbo.Modules", t => t.ModuleID, cascadeDelete: true)
                .Index(t => t.ModuleID);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        ModuleID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CategoryId = c.Int(nullable: false),
                        DefaultNumAnswers = c.Int(nullable: false),
                        DefaultTime = c.Int(nullable: false),
                        DefaultNumQuestions = c.Int(nullable: false),
                        isPrivate = c.Boolean(nullable: false),
                        Course_ID = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ModuleID)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Courses", t => t.Course_ID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.CategoryId)
                .Index(t => t.Course_ID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Institution = c.String(nullable: false),
                        ProfilePicture = c.String(),
                        OverrideNumberOfAnswers = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Course_ID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.Course_ID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Course_ID);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Instructor_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        ApplicationUser_Id1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Instructor_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id1)
                .Index(t => t.Instructor_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id1);
            
            CreateTable(
                "dbo.CourseWorks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Open = c.DateTime(nullable: false),
                        Close = c.DateTime(nullable: false),
                        OverrideNumOfAnswers = c.Int(nullable: false),
                        MaxAttempts = c.Int(nullable: false),
                        Course_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.Course_ID)
                .Index(t => t.Course_ID);
            
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NumOfAttempts = c.Int(nullable: false),
                        CourseWork_ID = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CourseWorks", t => t.CourseWork_ID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.CourseWork_ID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Scores",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CorrectAnswers = c.Int(nullable: false),
                        TotalAnswers = c.Int(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        Module_ModuleID = c.Int(),
                        User_Id = c.String(maxLength: 128),
                        Assignment_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Modules", t => t.Module_ModuleID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.Assignments", t => t.Assignment_ID)
                .Index(t => t.Module_ModuleID)
                .Index(t => t.User_Id)
                .Index(t => t.Assignment_ID);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        PictureID = c.Int(nullable: false, identity: true),
                        Location = c.String(),
                        AnswerID = c.Int(nullable: false),
                        Attribution = c.String(),
                    })
                .PrimaryKey(t => t.PictureID)
                .ForeignKey("dbo.Answers", t => t.AnswerID, cascadeDelete: true)
                .Index(t => t.AnswerID);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        moduleID = c.Int(nullable: false),
                        description = c.String(nullable: false),
                        userID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Pictures", "AnswerID", "dbo.Answers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Modules", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.Modules", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.Courses", "Instructor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CourseWorks", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.Assignments", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Scores", "Assignment_ID", "dbo.Assignments");
            DropForeignKey("dbo.Scores", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Scores", "Module_ModuleID", "dbo.Modules");
            DropForeignKey("dbo.Assignments", "CourseWork_ID", "dbo.CourseWorks");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Modules", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Answers", "ModuleID", "dbo.Modules");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Pictures", new[] { "AnswerID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Scores", new[] { "Assignment_ID" });
            DropIndex("dbo.Scores", new[] { "User_Id" });
            DropIndex("dbo.Scores", new[] { "Module_ModuleID" });
            DropIndex("dbo.Assignments", new[] { "User_Id" });
            DropIndex("dbo.Assignments", new[] { "CourseWork_ID" });
            DropIndex("dbo.CourseWorks", new[] { "Course_ID" });
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Courses", new[] { "Instructor_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Course_ID" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Modules", new[] { "User_Id" });
            DropIndex("dbo.Modules", new[] { "Course_ID" });
            DropIndex("dbo.Modules", new[] { "CategoryId" });
            DropIndex("dbo.Answers", new[] { "ModuleID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Reports");
            DropTable("dbo.Pictures");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Scores");
            DropTable("dbo.Assignments");
            DropTable("dbo.CourseWorks");
            DropTable("dbo.Courses");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Categories");
            DropTable("dbo.Modules");
            DropTable("dbo.Answers");
        }
    }
}
