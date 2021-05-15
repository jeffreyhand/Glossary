namespace Glossary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueTermField : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Entries", "Term", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Entries", new[] { "Term" });
        }
    }
}
