namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnsStringReq : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Answers", "AnswerString", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.Modules", "Name", c => c.String(maxLength: 25));
            AlterColumn("dbo.Modules", "Description", c => c.String(maxLength: 200));
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.AspNetUsers", "Institution", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Pictures", "Attribution", c => c.String(maxLength: 40));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pictures", "Attribution", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Institution", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Modules", "Description", c => c.String());
            AlterColumn("dbo.Modules", "Name", c => c.String());
            AlterColumn("dbo.Answers", "AnswerString", c => c.String());
        }
    }
}
