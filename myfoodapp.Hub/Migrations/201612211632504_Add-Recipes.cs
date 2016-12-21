namespace myfoodapp.Hub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRecipes : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Recipes", name: "productionUnit_Id", newName: "gardeningType_Id");
            RenameIndex(table: "dbo.Recipes", name: "IX_productionUnit_Id", newName: "IX_gardeningType_Id");
            CreateTable(
                "dbo.Months",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        name = c.String(nullable: false),
                        order = c.Int(nullable: false),
                        season_Id = c.Int(nullable: false),
                        Recipes_Id = c.Long(),
                        Recipes_Id1 = c.Long(),
                        Recipes_Id2 = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Seasons", t => t.season_Id, cascadeDelete: true)
                .ForeignKey("dbo.Recipes", t => t.Recipes_Id)
                .ForeignKey("dbo.Recipes", t => t.Recipes_Id1)
                .ForeignKey("dbo.Recipes", t => t.Recipes_Id2)
                .Index(t => t.season_Id)
                .Index(t => t.Recipes_Id)
                .Index(t => t.Recipes_Id1)
                .Index(t => t.Recipes_Id2);
            
            CreateTable(
                "dbo.Seasons",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WateringLevels",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        name = c.String(nullable: false),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Recipes", "daysOfGermination", c => c.Int(nullable: false));
            AddColumn("dbo.Recipes", "daysOfHarvest", c => c.Int(nullable: false));
            AddColumn("dbo.Recipes", "daysOfHarvestFromSowing", c => c.Int(nullable: false));
            AddColumn("dbo.Recipes", "minimumSpaceBetweenPlantInTower", c => c.Int(nullable: false));
            AddColumn("dbo.Recipes", "proteinPercentage", c => c.Int(nullable: false));
            AddColumn("dbo.Recipes", "idealMinTemperature", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Recipes", "idealMaxTemperature", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Recipes", "acceptableMaxTemperature", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Recipes", "acceptableMinTemperature", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Recipes", "picturePath", c => c.String(nullable: false));
            AddColumn("dbo.Recipes", "wateringLevel_Id", c => c.Int());
            CreateIndex("dbo.Recipes", "wateringLevel_Id");
            AddForeignKey("dbo.Recipes", "wateringLevel_Id", "dbo.WateringLevels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recipes", "wateringLevel_Id", "dbo.WateringLevels");
            DropForeignKey("dbo.Months", "Recipes_Id2", "dbo.Recipes");
            DropForeignKey("dbo.Months", "Recipes_Id1", "dbo.Recipes");
            DropForeignKey("dbo.Months", "Recipes_Id", "dbo.Recipes");
            DropForeignKey("dbo.Months", "season_Id", "dbo.Seasons");
            DropIndex("dbo.Recipes", new[] { "wateringLevel_Id" });
            DropIndex("dbo.Months", new[] { "Recipes_Id2" });
            DropIndex("dbo.Months", new[] { "Recipes_Id1" });
            DropIndex("dbo.Months", new[] { "Recipes_Id" });
            DropIndex("dbo.Months", new[] { "season_Id" });
            DropColumn("dbo.Recipes", "wateringLevel_Id");
            DropColumn("dbo.Recipes", "picturePath");
            DropColumn("dbo.Recipes", "acceptableMinTemperature");
            DropColumn("dbo.Recipes", "acceptableMaxTemperature");
            DropColumn("dbo.Recipes", "idealMaxTemperature");
            DropColumn("dbo.Recipes", "idealMinTemperature");
            DropColumn("dbo.Recipes", "proteinPercentage");
            DropColumn("dbo.Recipes", "minimumSpaceBetweenPlantInTower");
            DropColumn("dbo.Recipes", "daysOfHarvestFromSowing");
            DropColumn("dbo.Recipes", "daysOfHarvest");
            DropColumn("dbo.Recipes", "daysOfGermination");
            DropTable("dbo.WateringLevels");
            DropTable("dbo.Seasons");
            DropTable("dbo.Months");
            RenameIndex(table: "dbo.Recipes", name: "IX_gardeningType_Id", newName: "IX_productionUnit_Id");
            RenameColumn(table: "dbo.Recipes", name: "gardeningType_Id", newName: "productionUnit_Id");
        }
    }
}
