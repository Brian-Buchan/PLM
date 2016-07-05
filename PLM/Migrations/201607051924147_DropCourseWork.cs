namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropCourseWork : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Scores", "Assignment_ID", "dbo.Assignments");
            DropForeignKey("dbo.Courses", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "Instructor_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.Modules", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.CourseWorks", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.Assignments", "CourseWork_ID", "dbo.CourseWorks");
            DropForeignKey("dbo.Assignments", "User_Id", "dbo.AspNetUsers");

            DropIndex("dbo.Scores", new[] { "Assignment_ID" });
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Courses", new[] { "Instructor_Id" });
            DropIndex("dbo.CourseWorks", new[] { "Course_ID" });
            DropIndex("dbo.Assignments", new[] { "User_Id" });
            DropIndex("dbo.Assignments", new[] { "CourseWork_ID" });

            DropTable("dbo.Courses");
            DropTable("dbo.CourseWorks");
            DropTable("dbo.Assignments");
        }
    }
}
