namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovingCQAHeaderFromCQANumber : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CQAHeader", "CQANumberKey", "dbo.CQANumber");
            AddForeignKey("dbo.CQAHeader", "CQANumberKey", "dbo.CQANumber", "CQANumberKey", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CQAHeader", "CQANumberKey", "dbo.CQANumber");
            AddForeignKey("dbo.CQAHeader", "CQANumberKey", "dbo.CQANumber", "CQANumberKey");
        }
    }
}
