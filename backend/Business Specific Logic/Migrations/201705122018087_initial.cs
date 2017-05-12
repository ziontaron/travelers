namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.cat_TicketType",
                c => new
                    {
                        TicketTypeKey = c.Int(nullable: false, identity: true),
                        TicketType = c.String(nullable: false, maxLength: 50),
                        TicketTypeDescirption = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.TicketTypeKey);
            
            CreateTable(
                "dbo.InventoryEvent",
                c => new
                    {
                        InventoryEventKey = c.Int(nullable: false, identity: true),
                        InventoryEventName = c.String(nullable: false, maxLength: 50),
                        InventoryEventDescription = c.String(nullable: false),
                        CreationDate = c.DateTime(nullable: false, storeType: "date"),
                        TerminationDate = c.DateTime(nullable: false, storeType: "date"),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.InventoryEventKey);
            
            CreateTable(
                "dbo.MOTagCount",
                c => new
                    {
                        MOTagCountKey = c.Int(nullable: false, identity: true),
                        MOTagHeaderKey = c.Int(nullable: false),
                        SeqNum = c.Int(nullable: false),
                        Component = c.String(nullable: false, maxLength: 50),
                        CompDesc = c.String(nullable: false, maxLength: 50),
                        UM = c.String(nullable: false, maxLength: 10, fixedLength: true),
                    })
                .PrimaryKey(t => t.MOTagCountKey)
                .ForeignKey("dbo.MOTagHeader", t => t.MOTagHeaderKey, cascadeDelete: true)
                .Index(t => t.MOTagHeaderKey);
            
            CreateTable(
                "dbo.MOTagHeader",
                c => new
                    {
                        MOTagHeaderKey = c.Int(nullable: false, identity: true),
                        TicketKey = c.Int(nullable: false),
                        Planner = c.String(maxLength: 50),
                        MO = c.String(maxLength: 50),
                        MO_Ln = c.String(maxLength: 10),
                        MO_Status = c.String(maxLength: 3),
                        Order_Qty = c.Int(),
                        QtyRecv = c.Int(),
                        LineType = c.String(maxLength: 3),
                        QtyWip = c.Int(),
                    })
                .PrimaryKey(t => t.MOTagHeaderKey)
                .ForeignKey("dbo.Ticket", t => t.TicketKey)
                .Index(t => t.TicketKey);
            
            CreateTable(
                "dbo.Ticket",
                c => new
                    {
                        TicketKey = c.Int(nullable: false),
                        TicketCounter = c.Int(nullable: false, identity: true),
                        cat_TicketTypeKey = c.Int(nullable: false),
                        InventoryEventKey = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TicketKey);
            
            CreateTable(
                "dbo.TicketCount",
                c => new
                    {
                        TagCountKey = c.Int(nullable: false, identity: true),
                        CounterInitials = c.String(maxLength: 10),
                        CountedDate = c.DateTime(),
                        TicketKey = c.Int(nullable: false),
                        ItemNumber = c.String(maxLength: 50),
                        ItemDescription = c.String(maxLength: 100),
                        ItemRef = c.String(maxLength: 10),
                        LotNumber = c.String(maxLength: 50),
                        CountQTY = c.Int(nullable: false),
                        ReCountQty = c.Int(nullable: false),
                        SKT = c.String(maxLength: 15),
                        BIN = c.String(maxLength: 15),
                        IC = c.String(maxLength: 3),
                        Verified = c.Boolean(nullable: false),
                        CountStatus = c.String(maxLength: 10),
                        BlankTag = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TagCountKey)
                .ForeignKey("dbo.Ticket", t => t.TicketKey, cascadeDelete: true)
                .Index(t => t.TicketKey);
            
            CreateTable(
                "dbo.Track",
                c => new
                    {
                        TrackKey = c.Int(nullable: false, identity: true),
                        Entity_ID = c.Int(nullable: false),
                        Entity_Kind = c.String(nullable: false, maxLength: 50),
                        User_CreatedByKey = c.Int(),
                        Date_CreatedOn = c.DateTime(nullable: false),
                        Date_EditedOn = c.DateTime(),
                        Date_RemovedOn = c.DateTime(),
                        Date_LastTimeUsed = c.DateTime(),
                        User_LastEditedByKey = c.Int(),
                        User_RemovedByKey = c.Int(),
                        User_AssignedToKey = c.Int(),
                        User_AssignedByKey = c.Int(),
                    })
                .PrimaryKey(t => t.TrackKey)
                .ForeignKey("dbo.User", t => t.User_LastEditedByKey)
                .ForeignKey("dbo.User", t => t.User_RemovedByKey)
                .ForeignKey("dbo.User", t => t.User_AssignedToKey)
                .ForeignKey("dbo.User", t => t.User_AssignedByKey)
                .ForeignKey("dbo.User", t => t.User_CreatedByKey)
                .Index(t => t.User_CreatedByKey)
                .Index(t => t.User_LastEditedByKey)
                .Index(t => t.User_RemovedByKey)
                .Index(t => t.User_AssignedToKey)
                .Index(t => t.User_AssignedByKey);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserKey = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 50),
                        UserName = c.String(nullable: false, maxLength: 20),
                        Role = c.String(maxLength: 50),
                        AuthorizatorPassword = c.String(maxLength: 50),
                        Email = c.String(maxLength: 256),
                        Phone1 = c.String(maxLength: 50),
                        Phone2 = c.String(maxLength: 50),
                        Identicon = c.Binary(),
                        Identicon64 = c.String(unicode: false),
                        EmailPassword = c.String(),
                        EmailServer = c.String(),
                        EmailPort = c.String(),
                        sys_active = c.Boolean(nullable: false),
                        is_locked = c.Boolean(nullable: false),
                        document_status = c.String(),
                    })
                .PrimaryKey(t => t.UserKey)
                .Index(t => t.UserName, unique: true);
            
            CreateTable(
                "dbo.Gridster",
                c => new
                    {
                        GridsterKey = c.Int(nullable: false, identity: true),
                        Gridster_Entity_ID = c.Int(nullable: false),
                        Gridster_Entity_Kind = c.String(nullable: false, maxLength: 50),
                        Gridster_User_ID = c.Int(),
                        Gridster_Edited_On = c.DateTime(),
                        Gridster_ManyToMany_ID = c.Int(),
                        cols = c.Int(nullable: false),
                        rows = c.Int(nullable: false),
                        y = c.Int(nullable: false),
                        x = c.Int(nullable: false),
                        FontSize = c.Decimal(precision: 18, scale: 2),
                        IsShared = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GridsterKey)
                .ForeignKey("dbo.User", t => t.Gridster_User_ID)
                .Index(t => t.Gridster_User_ID);
            
            CreateTable(
                "dbo.Sort",
                c => new
                    {
                        SortKey = c.Int(nullable: false, identity: true),
                        Sort_Entity_ID = c.Int(nullable: false),
                        Sort_Entity_Kind = c.String(nullable: false, maxLength: 50),
                        Sort_User_ID = c.Int(),
                        Sort_Edited_On = c.DateTime(),
                        Sort_Sequence = c.Int(),
                        Sort_ParentInfo = c.String(),
                        IsShared = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SortKey)
                .ForeignKey("dbo.User", t => t.Sort_User_ID)
                .Index(t => t.Sort_User_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Track", "User_CreatedByKey", "dbo.User");
            DropForeignKey("dbo.Track", "User_AssignedByKey", "dbo.User");
            DropForeignKey("dbo.Track", "User_AssignedToKey", "dbo.User");
            DropForeignKey("dbo.Track", "User_RemovedByKey", "dbo.User");
            DropForeignKey("dbo.Track", "User_LastEditedByKey", "dbo.User");
            DropForeignKey("dbo.Sort", "Sort_User_ID", "dbo.User");
            DropForeignKey("dbo.Gridster", "Gridster_User_ID", "dbo.User");
            DropForeignKey("dbo.TicketCount", "TicketKey", "dbo.Ticket");
            DropForeignKey("dbo.MOTagHeader", "TicketKey", "dbo.Ticket");
            DropForeignKey("dbo.MOTagCount", "MOTagHeaderKey", "dbo.MOTagHeader");
            DropIndex("dbo.Sort", new[] { "Sort_User_ID" });
            DropIndex("dbo.Gridster", new[] { "Gridster_User_ID" });
            DropIndex("dbo.User", new[] { "UserName" });
            DropIndex("dbo.Track", new[] { "User_AssignedByKey" });
            DropIndex("dbo.Track", new[] { "User_AssignedToKey" });
            DropIndex("dbo.Track", new[] { "User_RemovedByKey" });
            DropIndex("dbo.Track", new[] { "User_LastEditedByKey" });
            DropIndex("dbo.Track", new[] { "User_CreatedByKey" });
            DropIndex("dbo.TicketCount", new[] { "TicketKey" });
            DropIndex("dbo.MOTagHeader", new[] { "TicketKey" });
            DropIndex("dbo.MOTagCount", new[] { "MOTagHeaderKey" });
            DropTable("dbo.Sort");
            DropTable("dbo.Gridster");
            DropTable("dbo.User");
            DropTable("dbo.Track");
            DropTable("dbo.TicketCount");
            DropTable("dbo.Ticket");
            DropTable("dbo.MOTagHeader");
            DropTable("dbo.MOTagCount");
            DropTable("dbo.InventoryEvent");
            DropTable("dbo.cat_TicketType");
        }
    }
}
