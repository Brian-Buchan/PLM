namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attribution : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pictures", "Attribution", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pictures", "Attribution");
        }
    }
}
