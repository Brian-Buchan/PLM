namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class databaseFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assignments", "CourseWork_ID", "dbo.CourseWorks");
            DropForeignKey("dbo.Scores", "Assignment_ID", "dbo.Assignments");
            DropForeignKey("dbo.Assignments", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CourseWorks", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.Courses", "Instructor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Modules", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.AspNetUsers", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropIndex("dbo.Modules", new[] { "Course_ID" });
            DropIndex("dbo.AspNetUsers", new[] { "Course_ID" });
            DropIndex("dbo.Courses", new[] { "Instructor_Id" });
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.CourseWorks", new[] { "Course_ID" });
            DropIndex("dbo.Assignments", new[] { "CourseWork_ID" });
            DropIndex("dbo.Assignments", new[] { "User_Id" });
            DropIndex("dbo.Scores", new[] { "Assignment_ID" });
            DropColumn("dbo.Modules", "Course_ID");
            DropColumn("dbo.AspNetUsers", "Course_ID");
            //DropColumn("dbo.Scores", "Assignment_ID");
            //DropTable("dbo.Courses");
            //DropTable("dbo.CourseWorks");
            //DropTable("dbo.Assignments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Assignments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NumOfAttempts = c.Int(nullable: false),
                        CourseWork_ID = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
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
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Instructor_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        ApplicationUser_Id1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Scores", "Assignment_ID", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Course_ID", c => c.Int());
            AddColumn("dbo.Modules", "Course_ID", c => c.Int());
            CreateIndex("dbo.Scores", "Assignment_ID");
            CreateIndex("dbo.Assignments", "User_Id");
            CreateIndex("dbo.Assignments", "CourseWork_ID");
            CreateIndex("dbo.CourseWorks", "Course_ID");
            CreateIndex("dbo.Courses", "ApplicationUser_Id1");
            CreateIndex("dbo.Courses", "ApplicationUser_Id");
            CreateIndex("dbo.Courses", "Instructor_Id");
            CreateIndex("dbo.AspNetUsers", "Course_ID");
            CreateIndex("dbo.Modules", "Course_ID");
            AddForeignKey("dbo.Courses", "ApplicationUser_Id1", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "Course_ID", "dbo.Courses", "ID");
            AddForeignKey("dbo.Modules", "Course_ID", "dbo.Courses", "ID");
            AddForeignKey("dbo.Courses", "Instructor_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.CourseWorks", "Course_ID", "dbo.Courses", "ID");
            AddForeignKey("dbo.Assignments", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Scores", "Assignment_ID", "dbo.Assignments", "ID");
            AddForeignKey("dbo.Assignments", "CourseWork_ID", "dbo.CourseWorks", "ID");
        }
    }
}
