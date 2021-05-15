namespace Glossary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRestrictions : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Entries", "Term", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Entries", "Definition", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Entries", "Definition", c => c.String());
            AlterColumn("dbo.Entries", "Term", c => c.String());
        }
    }
}
