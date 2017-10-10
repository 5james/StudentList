namespace StudentList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "IndexID", c => c.Int(nullable: false));
            DropColumn("dbo.Students", "index");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "index", c => c.Int(nullable: false));
            DropColumn("dbo.Students", "IndexID");
        }
    }
}
