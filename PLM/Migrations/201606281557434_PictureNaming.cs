namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PictureNaming : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "PictureCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Answers", "PictureCount");
        }
    }
}
