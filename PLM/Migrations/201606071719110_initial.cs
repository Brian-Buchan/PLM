namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        moduleID = c.Int(nullable: false),
                        description = c.String(),
                        userID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Reports");
        }
    }
}
