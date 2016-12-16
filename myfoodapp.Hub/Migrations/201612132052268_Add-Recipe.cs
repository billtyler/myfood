namespace myfoodapp.Hub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRecipe : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        productionUnit_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GardeningTypes", t => t.productionUnit_Id, cascadeDelete: true)
                .Index(t => t.productionUnit_Id);
            
            CreateTable(
                "dbo.GardeningTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        name = c.String(nullable: false),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recipes", "productionUnit_Id", "dbo.GardeningTypes");
            DropIndex("dbo.Recipes", new[] { "productionUnit_Id" });
            DropTable("dbo.GardeningTypes");
            DropTable("dbo.Recipes");
        }
    }
}
