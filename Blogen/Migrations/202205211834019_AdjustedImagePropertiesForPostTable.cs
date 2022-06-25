namespace blogen.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustedImagePropertiesForPostTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "ImagePath", c => c.String());
            DropColumn("dbo.Posts", "postImage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "postImage", c => c.Binary());
            DropColumn("dbo.Posts", "ImagePath");
        }
    }
}
