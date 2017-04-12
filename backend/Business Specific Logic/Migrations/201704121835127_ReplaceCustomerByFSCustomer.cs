namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReplaceCustomerByFSCustomer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CQAHeader", "CustomerKey", "dbo.Customer");
            DropIndex("dbo.CQAHeader", new[] { "CustomerKey" });
            DropTable("dbo.Customer");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerKey = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false, maxLength: 100),
                        FS_CustomerID = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.CustomerKey);
            
            CreateIndex("dbo.CQAHeader", "CustomerKey");
            AddForeignKey("dbo.CQAHeader", "CustomerKey", "dbo.Customer", "CustomerKey");
        }
    }
}
