namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.cat_ConcernType",
                c => new
                    {
                        ConcernTypeKey = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ConcernTypeKey);
            
            CreateTable(
                "dbo.CQAHeader",
                c => new
                    {
                        CQAHeaderKey = c.Int(nullable: false, identity: true),
                        CustomerKey = c.Int(nullable: false),
                        CQANumberKey = c.Int(nullable: false),
                        NotificationDate = c.DateTime(nullable: false),
                        PartNumberKey = c.Int(nullable: false),
                        ReoccurringIssue = c.Boolean(),
                        ProductLineKey = c.Int(),
                        ConcernTypeKey = c.Int(),
                        ConcertDescription = c.String(unicode: false),
                        FirstResponseDate = c.DateTime(),
                        ResultKey = c.Int(),
                        StatusKey = c.Int(),
                        NumberConcernedParts = c.String(maxLength: 100),
                        sys_active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CQAHeaderKey)
                .ForeignKey("dbo.cat_ConcernType", t => t.ConcernTypeKey)
                .ForeignKey("dbo.cat_PartNumber", t => t.PartNumberKey)
                .ForeignKey("dbo.cat_ProductLine", t => t.ProductLineKey)
                .ForeignKey("dbo.cat_Result", t => t.ResultKey)
                .ForeignKey("dbo.cat_Status", t => t.StatusKey)
                .ForeignKey("dbo.CQANumber", t => t.CQANumberKey)
                .ForeignKey("dbo.Customer", t => t.CustomerKey)
                .Index(t => t.CustomerKey)
                .Index(t => t.CQANumberKey)
                .Index(t => t.PartNumberKey)
                .Index(t => t.ProductLineKey)
                .Index(t => t.ConcernTypeKey)
                .Index(t => t.ResultKey)
                .Index(t => t.StatusKey);
            
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
            
            CreateTable(
                "dbo.cat_ProductLine",
                c => new
                    {
                        ProductLineKey = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductLineKey);
            
            CreateTable(
                "dbo.cat_Result",
                c => new
                    {
                        ResultKey = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ResultKey);
            
            CreateTable(
                "dbo.cat_Status",
                c => new
                    {
                        StatusKey = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.StatusKey);
            
            CreateTable(
                "dbo.CQALine",
                c => new
                    {
                        CQALinelKey = c.Int(nullable: false, identity: true),
                        CQAHeaderKey = c.Int(nullable: false),
                        OngoingActivities = c.String(nullable: false, unicode: false),
                        DueDate = c.DateTime(),
                        ClosedDate = c.DateTime(),
                        Champion = c.String(maxLength: 100),
                        sys_active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CQALinelKey)
                .ForeignKey("dbo.CQAHeader", t => t.CQAHeaderKey, cascadeDelete: true)
                .Index(t => t.CQAHeaderKey);
            
            CreateTable(
                "dbo.CQANumber",
                c => new
                    {
                        CQANumberKey = c.Int(nullable: false, identity: true),
                        CreatedDate = c.DateTime(nullable: false),
                        GeneratedNumber = c.String(nullable: false, maxLength: 11, unicode: false),
                        Revision = c.String(maxLength: 50),
                        RevisionFrom = c.Int(),
                        DuplicatedFrom = c.Int(),
                        Sequence = c.Int(nullable: false),
                        TaskDescriptionRevisionReason = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.CQANumberKey);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerKey = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false, maxLength: 100),
                        FS_CustomerID = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.CustomerKey);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CQAHeader", "CustomerKey", "dbo.Customer");
            DropForeignKey("dbo.CQAHeader", "CQANumberKey", "dbo.CQANumber");
            DropForeignKey("dbo.CQALine", "CQAHeaderKey", "dbo.CQAHeader");
            DropForeignKey("dbo.CQAHeader", "StatusKey", "dbo.cat_Status");
            DropForeignKey("dbo.CQAHeader", "ResultKey", "dbo.cat_Result");
            DropForeignKey("dbo.CQAHeader", "ProductLineKey", "dbo.cat_ProductLine");
            DropForeignKey("dbo.CQAHeader", "PartNumberKey", "dbo.cat_PartNumber");
            DropForeignKey("dbo.CQAHeader", "ConcernTypeKey", "dbo.cat_ConcernType");
            DropIndex("dbo.CQALine", new[] { "CQAHeaderKey" });
            DropIndex("dbo.CQAHeader", new[] { "StatusKey" });
            DropIndex("dbo.CQAHeader", new[] { "ResultKey" });
            DropIndex("dbo.CQAHeader", new[] { "ConcernTypeKey" });
            DropIndex("dbo.CQAHeader", new[] { "ProductLineKey" });
            DropIndex("dbo.CQAHeader", new[] { "PartNumberKey" });
            DropIndex("dbo.CQAHeader", new[] { "CQANumberKey" });
            DropIndex("dbo.CQAHeader", new[] { "CustomerKey" });
            DropTable("dbo.Customer");
            DropTable("dbo.CQANumber");
            DropTable("dbo.CQALine");
            DropTable("dbo.cat_Status");
            DropTable("dbo.cat_Result");
            DropTable("dbo.cat_ProductLine");
            DropTable("dbo.cat_PartNumber");
            DropTable("dbo.CQAHeader");
            DropTable("dbo.cat_ConcernType");
        }
    }
}
