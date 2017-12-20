namespace ChatServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        FirstConnectionId = c.String(nullable: false, maxLength: 128),
                        SecondConnectionId = c.String(),
                        Date = c.DateTime(nullable: false),
                        MessageText = c.String(),
                    })
                .PrimaryKey(t => t.FirstConnectionId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Messages");
        }
    }
}
