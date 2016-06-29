namespace PLM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class answerString : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Modules", "rightAnswerString", c => c.String());
            AddColumn("dbo.Modules", "wrongAnswerString", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Modules", "wrongAnswerString");
            DropColumn("dbo.Modules", "rightAnswerString");
        }
    }
}
