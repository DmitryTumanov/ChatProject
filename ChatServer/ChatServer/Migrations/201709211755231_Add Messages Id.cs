namespace ChatServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessagesId : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Messages");
            AddColumn("dbo.Messages", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Messages", "FirstConnectionId", c => c.String());
            AddPrimaryKey("dbo.Messages", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Messages");
            AlterColumn("dbo.Messages", "FirstConnectionId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Messages", "Id");
            AddPrimaryKey("dbo.Messages", "FirstConnectionId");
        }
    }
}
