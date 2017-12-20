namespace ChatServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCipherType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "CipherType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "CipherType");
        }
    }
}
