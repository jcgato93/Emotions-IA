namespace Emotion.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmoEmotions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Score = c.Single(nullable: false),
                        EmoFaceId = c.Int(nullable: false),
                        EmotionType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmoFaces", t => t.EmoFaceId, cascadeDelete: true)
                .Index(t => t.EmoFaceId);
            
            CreateTable(
                "dbo.EmoFaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmoPictureId = c.Int(nullable: false),
                        X = c.Int(nullable: false),
                        Y = c.Int(nullable: false),
                        Width = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EmoPictures", t => t.EmoPictureId, cascadeDelete: true)
                .Index(t => t.EmoPictureId);
            
            CreateTable(
                "dbo.EmoPictures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmoFaces", "EmoPictureId", "dbo.EmoPictures");
            DropForeignKey("dbo.EmoEmotions", "EmoFaceId", "dbo.EmoFaces");
            DropIndex("dbo.EmoFaces", new[] { "EmoPictureId" });
            DropIndex("dbo.EmoEmotions", new[] { "EmoFaceId" });
            DropTable("dbo.EmoPictures");
            DropTable("dbo.EmoFaces");
            DropTable("dbo.EmoEmotions");
        }
    }
}
