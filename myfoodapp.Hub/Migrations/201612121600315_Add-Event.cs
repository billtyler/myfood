namespace myfoodapp.Hub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEvent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                        description = c.String(nullable: false),
                        productionUnit_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductionUnits", t => t.productionUnit_Id, cascadeDelete: true)
                .Index(t => t.productionUnit_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "productionUnit_Id", "dbo.ProductionUnits");
            DropIndex("dbo.Events", new[] { "productionUnit_Id" });
            DropTable("dbo.Events");
        }
    }
}
