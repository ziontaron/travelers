namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptionalCustomerForCQAHeader : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CQAHeader", new[] { "CustomerKey" });
            AlterColumn("dbo.CQAHeader", "CustomerKey", c => c.Int());
            CreateIndex("dbo.CQAHeader", "CustomerKey");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CQAHeader", new[] { "CustomerKey" });
            AlterColumn("dbo.CQAHeader", "CustomerKey", c => c.Int(nullable: false));
            CreateIndex("dbo.CQAHeader", "CustomerKey");
        }
    }
}
