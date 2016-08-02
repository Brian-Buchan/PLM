namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class scoreupdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Scores", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Scores", new[] { "User_Id" });
            AddColumn("dbo.Scores", "UserID", c => c.String());
            DropColumn("dbo.Scores", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Scores", "User_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.Scores", "UserID");
            CreateIndex("dbo.Scores", "User_Id");
            AddForeignKey("dbo.Scores", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
