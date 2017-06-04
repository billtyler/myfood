using Microsoft.AspNet.Identity.EntityFramework;
using myfoodapp.Hub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;

namespace myfoodapp.Hub.Migrations
{
    public static class UpgradeDatabase
    {
        public static void DoWork(ApplicationDbContext context)
        {
            var permacultureBedsGardeningType = context.GardeningTypes.Where(g => g.Id == 0).FirstOrDefault();
            var verticalTowersGardeningType = context.GardeningTypes.Where(g => g.Id == 1).FirstOrDefault();
            var towerGardenGardeningType = context.GardeningTypes.Where(g => g.Id == 2).FirstOrDefault();
            var allGardeningType = context.GardeningTypes.Where(g => g.Id == 3).FirstOrDefault();
            var fishTanksGardeningType = context.GardeningTypes.Where(g => g.Id == 4).FirstOrDefault();

            var lightWateringLevel = context.WateringLevels.Where(g => g.Id == 0).FirstOrDefault();
            var moderateWateringLevel = context.WateringLevels.Where(g => g.Id == 1).FirstOrDefault();
            var normalWateringLevel = context.WateringLevels.Where(g => g.Id == 2).FirstOrDefault();
            var maximumWateringLevel = context.WateringLevels.Where(g => g.Id == 3).FirstOrDefault();

            var januaryMonth = context.Months.Where(g => g.Id == 0).FirstOrDefault();
            var februaryMonth = context.Months.Where(g => g.Id == 1).FirstOrDefault();
            var marchMonth = context.Months.Where(g => g.Id == 2).FirstOrDefault();
            var aprilMonth = context.Months.Where(g => g.Id == 3).FirstOrDefault();
            var mayMonth = context.Months.Where(g => g.Id == 4).FirstOrDefault();
            var juneMonth = context.Months.Where(g => g.Id == 5).FirstOrDefault();
            var julyMonth = context.Months.Where(g => g.Id == 6).FirstOrDefault();
            var augustMonth = context.Months.Where(g => g.Id == 7).FirstOrDefault();
            var septemberMonth = context.Months.Where(g => g.Id == 8).FirstOrDefault();
            var octoberMonth = context.Months.Where(g => g.Id == 9).FirstOrDefault();
            var novemberMonth = context.Months.Where(g => g.Id == 10).FirstOrDefault();
            var decemberMonth = context.Months.Where(g => g.Id == 11).FirstOrDefault();

            var prodUnitTypeBalcony = context.ProductionUnitTypes.Where(m => m.Id == 1).FirstOrDefault();
            var prodUnitTypeCity = context.ProductionUnitTypes.Where(m => m.Id == 2).FirstOrDefault();
            var prodUnitTypeFam14 = context.ProductionUnitTypes.Where(m => m.Id == 3).FirstOrDefault();
            var prodUnitTypeFam22 = context.ProductionUnitTypes.Where(m => m.Id == 4).FirstOrDefault();
            var prodUnitTypeFarm = context.ProductionUnitTypes.Where(m => m.Id == 5).FirstOrDefault();
            var prodUnitTypeDevKit = context.ProductionUnitTypes.Where(m => m.Id == 6).FirstOrDefault();
            var prodUnitTypeExperimental = context.ProductionUnitTypes.Where(m => m.Id == 7).FirstOrDefault();

            var prodUnitStatusWait = context.ProductionUnitStatus.Where(m => m.Id == 1).FirstOrDefault();
            var prodUnitStatusReadyForInstall = context.ProductionUnitStatus.Where(m => m.Id == 2).FirstOrDefault();
            var prodUnitStatusRunning = context.ProductionUnitStatus.Where(m => m.Id == 3).FirstOrDefault();
            var prodUnitStatusMaintenance = context.ProductionUnitStatus.Where(m => m.Id == 4).FirstOrDefault();
            var prodUnitStatusStopped = context.ProductionUnitStatus.Where(m => m.Id == 5).FirstOrDefault();

            var hydroTypeNotApplicable = context.HydroponicTypes.Where(m => m.Id == 1).FirstOrDefault();
            var hydroTypeBioponics = context.HydroponicTypes.Where(m => m.Id == 2).FirstOrDefault();
            var hydroTypeAquaponicsCarp = context.HydroponicTypes.Where(m => m.Id == 3).FirstOrDefault();
            var hydroTypeAquaponicsTilapia = context.HydroponicTypes.Where(m => m.Id == 4).FirstOrDefault();
            var hydroTypeAquaponicsTruit = context.HydroponicTypes.Where(m => m.Id == 5).FirstOrDefault();
            var hydroTypeAquaponicsCrayfish = context.HydroponicTypes.Where(m => m.Id == 6).FirstOrDefault();
            var hydroTypeAquaponicsColdFish = context.HydroponicTypes.Where(m => m.Id == 7).FirstOrDefault();
            var hydroTypeAquaponicsWarmFish = context.HydroponicTypes.Where(m => m.Id == 8).FirstOrDefault();

            var towers11Option = context.Options.Where(m => m.Id == 0).FirstOrDefault();
            var towers18Option = context.Options.Where(m => m.Id == 1).FirstOrDefault();
            var towers24Option = context.Options.Where(m => m.Id == 2).FirstOrDefault();
            var towers36Option = context.Options.Where(m => m.Id == 3).FirstOrDefault();
            var solarPanelOption = context.Options.Where(m => m.Id == 4).FirstOrDefault();
            var pelletStoveOption = context.Options.Where(m => m.Id == 5).FirstOrDefault();
            var monitoringKitv1Option = context.Options.Where(m => m.Id == 6).FirstOrDefault();
            var monitoringKitv2Option = context.Options.Where(m => m.Id == 7).FirstOrDefault();
            var advancedMonitoringOption = context.Options.Where(m => m.Id == 8).FirstOrDefault();
            var touchlessScreenOption = context.Options.Where(m => m.Id == 9).FirstOrDefault();
            var sigfoxConnectionOption = context.Options.Where(m => m.Id == 10).FirstOrDefault();
            var permacultureBedOption = context.Options.Where(m => m.Id == 11).FirstOrDefault();
            var permacultureBiocharOption = context.Options.Where(m => m.Id == 12).FirstOrDefault();

            var userETR = new ApplicationUser() { Email = "emmanuel@myfood.eu", UserName = "emmanuel@myfood.eu" };
            var userOFE = new ApplicationUser() { Email = "ophelia@myfood.eu", UserName = "ophelia@myfood.eu" };
            var userGTI = new ApplicationUser() { Email = "guillaume@myfood.eu", UserName = "guillaume@myfood.eu" };

            var store = new UserStore<ApplicationUser>(context);
            var manager = new ApplicationUserManager(store);

            var defaultPassword = ConfigurationManager.AppSettings["defaultPassword"];

            var t = Task.Run(async () =>
            {
                await manager.CreateAsync(userETR, defaultPassword);
                await manager.CreateAsync(userOFE, defaultPassword);
                await manager.CreateAsync(userGTI, defaultPassword);
            });

            t.Wait();

            var EmmmanuelTOwner = new ProductionUnitOwner() { Id = 46, user = userETR, pioneerCitizenName = "Emmanuel T." };
            var OpheliaFOwner = new ProductionUnitOwner() { Id = 47, user = userOFE, pioneerCitizenName = "Ophélia F." };
            var GuillaumeTOwner = new ProductionUnitOwner() { Id = 48, user = userGTI, pioneerCitizenName = "Guillaume T." };

            context.ProductionUnitOwners.Add(EmmmanuelTOwner);
            context.ProductionUnitOwners.Add(OpheliaFOwner);
            context.ProductionUnitOwners.Add(GuillaumeTOwner);

            var ETRProdUnit = new ProductionUnit()
            {
                locationLatitude = 46.0732302,
                locationLongitude = 6.2295138,
                reference = "FTAT01",
                info = "Family Experimentation",
                startDate = new DateTime(2017, 06, 16),
                version = "2",
                owner = EmmmanuelTOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeAquaponicsWarmFish,
                productionUnitStatus = prodUnitStatusReadyForInstall,
            };

            var OFEProdUnit = new ProductionUnit()
            {
                locationLatitude = 43.428125,
                locationLongitude = 6.5823434,
                reference = "FTAT03",
                info = "Family Experimentation",
                startDate = new DateTime(2017, 06, 19),
                version = "2",
                owner = OpheliaFOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeAquaponicsWarmFish,
                productionUnitStatus = prodUnitStatusReadyForInstall,
            };

            var GTIProdUnit = new ProductionUnit()
            {
                locationLatitude = 47.5321616,
                locationLongitude = -3.1774892,
                reference = "FTAT57",
                info = "Family Experimentation",
                startDate = new DateTime(2017, 06, 25),
                version = "2",
                owner = GuillaumeTOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeAquaponicsWarmFish,
                productionUnitStatus = prodUnitStatusReadyForInstall,
            };

            context.ProductionUnits.Add(ETRProdUnit);
            context.ProductionUnits.Add(OFEProdUnit);
            context.ProductionUnits.Add(GTIProdUnit);

            context.SaveChanges();

            var optionsETR = new List<OptionList>();

            optionsETR.Add(new OptionList() { productionUnit = ETRProdUnit, option = monitoringKitv2Option });
            optionsETR.Add(new OptionList() { productionUnit = ETRProdUnit, option = towers18Option });
            optionsETR.Add(new OptionList() { productionUnit = ETRProdUnit, option = permacultureBedOption });
            optionsETR.Add(new OptionList() { productionUnit = ETRProdUnit, option = solarPanelOption });
            optionsETR.Add(new OptionList() { productionUnit = ETRProdUnit, option = sigfoxConnectionOption });
            optionsETR.Add(new OptionList() { productionUnit = ETRProdUnit, option = pelletStoveOption });

            var optionsOFE = new List<OptionList>();

            optionsOFE.Add(new OptionList() { productionUnit = OFEProdUnit, option = monitoringKitv2Option });
            optionsOFE.Add(new OptionList() { productionUnit = OFEProdUnit, option = towers18Option });
            optionsOFE.Add(new OptionList() { productionUnit = OFEProdUnit, option = permacultureBedOption });
            optionsOFE.Add(new OptionList() { productionUnit = OFEProdUnit, option = solarPanelOption });

            var optionsGTI = new List<OptionList>();

            optionsGTI.Add(new OptionList() { productionUnit = GTIProdUnit, option = monitoringKitv2Option });
            optionsGTI.Add(new OptionList() { productionUnit = GTIProdUnit, option = towers18Option });
            optionsGTI.Add(new OptionList() { productionUnit = GTIProdUnit, option = solarPanelOption });
            optionsGTI.Add(new OptionList() { productionUnit = GTIProdUnit, option = permacultureBedOption });

            context.OptionLists.AddRange(optionsETR);
            context.OptionLists.AddRange(optionsOFE);
            context.OptionLists.AddRange(optionsGTI);

            context.SaveChanges();
        }
    }
}