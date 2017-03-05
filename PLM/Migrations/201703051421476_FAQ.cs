namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FAQ : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FAQs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Question = c.String(maxLength: 255),
                        Answer = c.String(),
                        SortOrder = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FAQs");
        }
    }
}
