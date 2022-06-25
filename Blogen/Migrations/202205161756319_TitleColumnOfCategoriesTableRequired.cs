namespace blogen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TitleColumnOfCategoriesTableRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "Title", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Categories", "Title", c => c.String());
        }
    }
}
