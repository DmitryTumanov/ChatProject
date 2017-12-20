namespace ChatServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCipherType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Messages", "CipherType", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Messages", "CipherType", c => c.String());
        }
    }
}
