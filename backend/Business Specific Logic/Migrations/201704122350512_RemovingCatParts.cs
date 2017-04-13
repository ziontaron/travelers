namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovingCatParts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CQAHeader", "PartNumberKey", "dbo.cat_PartNumber");
            DropIndex("dbo.CQAHeader", new[] { "PartNumberKey" });
            DropTable("dbo.cat_PartNumber");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.cat_PartNumber",
                c => new
                    {
                        PartNumberKey = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 100),
                        PartDescription = c.String(maxLength: 200),
                        ProductLineKey = c.Int(),
                    })
                .PrimaryKey(t => t.PartNumberKey);
            
            CreateIndex("dbo.CQAHeader", "PartNumberKey");
            AddForeignKey("dbo.CQAHeader", "PartNumberKey", "dbo.cat_PartNumber", "PartNumberKey");
        }
    }
}
