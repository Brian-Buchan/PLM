namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserModelUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "Institution", c => c.String());
            DropColumn("dbo.Modules", "Topic");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Modules", "Topic", c => c.String());
            DropColumn("dbo.AspNetUsers", "Institution");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
        }
    }
}
