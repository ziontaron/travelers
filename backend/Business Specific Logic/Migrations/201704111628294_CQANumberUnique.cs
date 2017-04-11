namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CQANumberUnique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CQAHeader", new[] { "PartNumberKey" });
            AlterColumn("dbo.CQAHeader", "PartNumberKey", c => c.Int());
            CreateIndex("dbo.CQAHeader", "PartNumberKey");
            CreateIndex("dbo.CQANumber", "GeneratedNumber", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.CQANumber", new[] { "GeneratedNumber" });
            DropIndex("dbo.CQAHeader", new[] { "PartNumberKey" });
            AlterColumn("dbo.CQAHeader", "PartNumberKey", c => c.Int(nullable: false));
            CreateIndex("dbo.CQAHeader", "PartNumberKey");
        }
    }
}
