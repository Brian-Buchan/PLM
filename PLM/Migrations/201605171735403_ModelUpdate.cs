namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelUpdate : DbMigration
    {
        public override void Up()
        {
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
            
            AddColumn("dbo.Modules", "Description", c => c.String());
            AddColumn("dbo.Modules", "DefaultNumAnswers", c => c.Int(nullable: false));
            AddColumn("dbo.Modules", "DefaultTime", c => c.Int(nullable: false));
            AddColumn("dbo.Modules", "DefaultNumPictures", c => c.Int(nullable: false));
            AddColumn("dbo.Modules", "isPrivate", c => c.Boolean(nullable: false));
            AddColumn("dbo.Modules", "Course_ID", c => c.Int());
            AddColumn("dbo.AspNetUsers", "OverrideNumberOfAnswers", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Course_ID", c => c.Int());
            AlterColumn("dbo.AspNetUsers", "Institution", c => c.String(nullable: false));
            CreateIndex("dbo.Modules", "Course_ID");
            CreateIndex("dbo.AspNetUsers", "Course_ID");
            AddForeignKey("dbo.Modules", "Course_ID", "dbo.Courses", "ID");
            AddForeignKey("dbo.AspNetUsers", "Course_ID", "dbo.Courses", "ID");
        }
        
        public override void Down()
        {
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
            DropIndex("dbo.Scores", new[] { "Assignment_ID" });
            DropIndex("dbo.Scores", new[] { "User_Id" });
            DropIndex("dbo.Scores", new[] { "Module_ModuleID" });
            DropIndex("dbo.Assignments", new[] { "User_Id" });
            DropIndex("dbo.Assignments", new[] { "CourseWork_ID" });
            DropIndex("dbo.CourseWorks", new[] { "Course_ID" });
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Courses", new[] { "Instructor_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Course_ID" });
            DropIndex("dbo.Modules", new[] { "Course_ID" });
            AlterColumn("dbo.AspNetUsers", "Institution", c => c.String());
            DropColumn("dbo.AspNetUsers", "Course_ID");
            DropColumn("dbo.AspNetUsers", "OverrideNumberOfAnswers");
            DropColumn("dbo.Modules", "Course_ID");
            DropColumn("dbo.Modules", "isPrivate");
            DropColumn("dbo.Modules", "DefaultNumPictures");
            DropColumn("dbo.Modules", "DefaultTime");
            DropColumn("dbo.Modules", "DefaultNumAnswers");
            DropColumn("dbo.Modules", "Description");
            DropTable("dbo.Scores");
            DropTable("dbo.Assignments");
            DropTable("dbo.CourseWorks");
            DropTable("dbo.Courses");
        }
    }
}
