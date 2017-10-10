namespace StudentList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class index_required1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "IndexID", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "IndexID", c => c.String(maxLength: 10));
        }
    }
}
