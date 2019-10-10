namespace Learn_ASP.NET.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 256),
                        PasswordHash = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, name: "UserNameIndex");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", "UserNameIndex");
            DropTable("dbo.Users");
        }
    }
}
