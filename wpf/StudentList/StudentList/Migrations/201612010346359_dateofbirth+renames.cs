namespace StudentList.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateofbirthrenames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "FirstName", c => c.String(maxLength: 32));
            AddColumn("dbo.Students", "LastName", c => c.String(maxLength: 32));
            AddColumn("dbo.Students", "DateOfBirth", c => c.DateTime());
            DropColumn("dbo.Students", "Name");
            DropColumn("dbo.Students", "Surname");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "Surname", c => c.String(maxLength: 32));
            AddColumn("dbo.Students", "Name", c => c.String(maxLength: 32));
            DropColumn("dbo.Students", "DateOfBirth");
            DropColumn("dbo.Students", "LastName");
            DropColumn("dbo.Students", "FirstName");
        }
    }
}
