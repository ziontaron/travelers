namespace BusinessSpecificLogic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
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
            
            CreateTable(
                "dbo.TravelerHeader",
                c => new
                    {
                        TravelerHeaderKey = c.Int(nullable: false, identity: true),
                        PartNumber = c.String(nullable: false, maxLength: 50),
                        PartDescription = c.String(nullable: false, maxLength: 50),
                        TravelerNumber = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TravelerHeaderKey);
            
            CreateTable(
                "dbo.TravelerLine",
                c => new
                    {
                        TravelerLineKey = c.Int(nullable: false, identity: true),
                        TravelerHeaderKey = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TravelerLineKey)
                .ForeignKey("dbo.TravelerHeader", t => t.TravelerHeaderKey, cascadeDelete: true)
                .Index(t => t.TravelerHeaderKey);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TravelerLine", "TravelerHeaderKey", "dbo.TravelerHeader");
            DropForeignKey("dbo.Track", "User_CreatedByKey", "dbo.User");
            DropForeignKey("dbo.Track", "User_AssignedByKey", "dbo.User");
            DropForeignKey("dbo.Track", "User_AssignedToKey", "dbo.User");
            DropForeignKey("dbo.Track", "User_RemovedByKey", "dbo.User");
            DropForeignKey("dbo.Track", "User_LastEditedByKey", "dbo.User");
            DropForeignKey("dbo.Sort", "Sort_User_ID", "dbo.User");
            DropForeignKey("dbo.Gridster", "Gridster_User_ID", "dbo.User");
            DropIndex("dbo.TravelerLine", new[] { "TravelerHeaderKey" });
            DropIndex("dbo.Sort", new[] { "Sort_User_ID" });
            DropIndex("dbo.Gridster", new[] { "Gridster_User_ID" });
            DropIndex("dbo.User", new[] { "UserName" });
            DropIndex("dbo.Track", new[] { "User_AssignedByKey" });
            DropIndex("dbo.Track", new[] { "User_AssignedToKey" });
            DropIndex("dbo.Track", new[] { "User_RemovedByKey" });
            DropIndex("dbo.Track", new[] { "User_LastEditedByKey" });
            DropIndex("dbo.Track", new[] { "User_CreatedByKey" });
            DropTable("dbo.TravelerLine");
            DropTable("dbo.TravelerHeader");
            DropTable("dbo.Sort");
            DropTable("dbo.Gridster");
            DropTable("dbo.User");
            DropTable("dbo.Track");
        }
    }
}
