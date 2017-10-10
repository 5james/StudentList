namespace StudentList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timestampindex : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "Stamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Students", "Stamp", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AlterColumn("dbo.Groups", "Name", c => c.String(nullable: false, maxLength: 16));
            AlterColumn("dbo.Students", "FirstName", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.Students", "LastName", c => c.String(nullable: false, maxLength: 64));
            AlterColumn("dbo.Students", "City", c => c.String(maxLength: 64));
            DropColumn("dbo.Students", "IndexID");
            AddColumn("dbo.Students", "IndexID", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "IndexID", c => c.Int(nullable: false));
            AlterColumn("dbo.Students", "City", c => c.String(maxLength: 32));
            AlterColumn("dbo.Students", "LastName", c => c.String(maxLength: 32));
            AlterColumn("dbo.Students", "FirstName", c => c.String(maxLength: 32));
            AlterColumn("dbo.Groups", "Name", c => c.String());
            DropColumn("dbo.Students", "Stamp");
            DropColumn("dbo.Groups", "Stamp");
        }
    }
}
