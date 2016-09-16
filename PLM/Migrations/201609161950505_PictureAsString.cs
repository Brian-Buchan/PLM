namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PictureAsString : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pictures", "PictureData", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pictures", "PictureData");
        }
    }
}
