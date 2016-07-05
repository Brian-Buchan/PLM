namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropCourseAssignments : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Scores", "Assignment_ID", "dbo.Assignments");
            DropIndex("dbo.Scores", new[] { "Assignment_ID" });

            DropForeignKey("dbo.Courses", "ApplicationUser_Id1", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Courses", "Instructor_Id", "dbo.AspNetUsers");

            DropForeignKey("dbo.Assignments", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Assignments", "CourseWork_ID", "dbo.CourseWorks");

            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id1" });
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Courses", new[] { "Instructor_Id" });

            DropIndex("dbo.Assignments", new[] { "User_Id" });
            DropIndex("dbo.Assignments", new[] { "CourseWork_ID" });

            DropTable("dbo.Assignments");
            DropTable("dbo.Courses");
        }
    }
}
