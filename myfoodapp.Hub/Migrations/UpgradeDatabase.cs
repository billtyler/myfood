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
            //var permacultureBedsGardeningType = context.GardeningTypes.Where(g => g.Id == 0).FirstOrDefault();
            //var verticalTowersGardeningType = context.GardeningTypes.Where(g => g.Id == 1).FirstOrDefault();
            //var towerGardenGardeningType = context.GardeningTypes.Where(g => g.Id == 2).FirstOrDefault();
            //var allGardeningType = context.GardeningTypes.Where(g => g.Id == 3).FirstOrDefault();
            //var fishTanksGardeningType = context.GardeningTypes.Where(g => g.Id == 4).FirstOrDefault();

            //var lightWateringLevel = context.WateringLevels.Where(g => g.Id == 0).FirstOrDefault();
            //var moderateWateringLevel = context.WateringLevels.Where(g => g.Id == 1).FirstOrDefault();
            //var normalWateringLevel = context.WateringLevels.Where(g => g.Id == 2).FirstOrDefault();
            //var maximumWateringLevel = context.WateringLevels.Where(g => g.Id == 3).FirstOrDefault();

            //var januaryMonth = context.Months.Where(g => g.Id == 0).FirstOrDefault();
            //var februaryMonth = context.Months.Where(g => g.Id == 1).FirstOrDefault();
            //var marchMonth = context.Months.Where(g => g.Id == 2).FirstOrDefault();
            //var aprilMonth = context.Months.Where(g => g.Id == 3).FirstOrDefault();
            //var mayMonth = context.Months.Where(g => g.Id == 4).FirstOrDefault();
            //var juneMonth = context.Months.Where(g => g.Id == 5).FirstOrDefault();
            //var julyMonth = context.Months.Where(g => g.Id == 6).FirstOrDefault();
            //var augustMonth = context.Months.Where(g => g.Id == 7).FirstOrDefault();
            //var septemberMonth = context.Months.Where(g => g.Id == 8).FirstOrDefault();
            //var octoberMonth = context.Months.Where(g => g.Id == 9).FirstOrDefault();
            //var novemberMonth = context.Months.Where(g => g.Id == 10).FirstOrDefault();
            //var decemberMonth = context.Months.Where(g => g.Id == 11).FirstOrDefault();

            //var prodUnitTypeBalcony = context.ProductionUnitTypes.Where(m => m.Id == 1).FirstOrDefault();
            //var prodUnitTypeCity = context.ProductionUnitTypes.Where(m => m.Id == 2).FirstOrDefault();
            //var prodUnitTypeFam14 = context.ProductionUnitTypes.Where(m => m.Id == 3).FirstOrDefault();
            //var prodUnitTypeFam22 = context.ProductionUnitTypes.Where(m => m.Id == 4).FirstOrDefault();
            //var prodUnitTypeFarm = context.ProductionUnitTypes.Where(m => m.Id == 5).FirstOrDefault();
            //var prodUnitTypeDevKit = context.ProductionUnitTypes.Where(m => m.Id == 6).FirstOrDefault();
            //var prodUnitTypeExperimental = context.ProductionUnitTypes.Where(m => m.Id == 7).FirstOrDefault();

            //var prodUnitStatusWait = context.ProductionUnitStatus.Where(m => m.Id == 1).FirstOrDefault();
            //var prodUnitStatusReadyForInstall = context.ProductionUnitStatus.Where(m => m.Id == 2).FirstOrDefault();
            //var prodUnitStatusRunning = context.ProductionUnitStatus.Where(m => m.Id == 3).FirstOrDefault();
            //var prodUnitStatusMaintenance = context.ProductionUnitStatus.Where(m => m.Id == 4).FirstOrDefault();
            //var prodUnitStatusStopped = context.ProductionUnitStatus.Where(m => m.Id == 5).FirstOrDefault();

            //var hydroTypeNotApplicable = context.HydroponicTypes.Where(m => m.Id == 1).FirstOrDefault();
            //var hydroTypeBioponics = context.HydroponicTypes.Where(m => m.Id == 2).FirstOrDefault();
            //var hydroTypeAquaponicsCarp = context.HydroponicTypes.Where(m => m.Id == 3).FirstOrDefault();
            //var hydroTypeAquaponicsTilapia = context.HydroponicTypes.Where(m => m.Id == 4).FirstOrDefault();
            //var hydroTypeAquaponicsTruit = context.HydroponicTypes.Where(m => m.Id == 5).FirstOrDefault();
            //var hydroTypeAquaponicsCrayfish = context.HydroponicTypes.Where(m => m.Id == 6).FirstOrDefault();
            //var hydroTypeAquaponicsColdFish = context.HydroponicTypes.Where(m => m.Id == 7).FirstOrDefault();
            //var hydroTypeAquaponicsWarmFish = context.HydroponicTypes.Where(m => m.Id == 8).FirstOrDefault();

            //var towers11Option = context.Options.Where(m => m.Id == 0).FirstOrDefault();
            //var towers18Option = context.Options.Where(m => m.Id == 1).FirstOrDefault();
            //var towers24Option = context.Options.Where(m => m.Id == 2).FirstOrDefault();
            //var towers36Option = context.Options.Where(m => m.Id == 3).FirstOrDefault();
            //var solarPanelOption = context.Options.Where(m => m.Id == 4).FirstOrDefault();
            //var pelletStoveOption = context.Options.Where(m => m.Id == 5).FirstOrDefault();
            //var monitoringKitv1Option = context.Options.Where(m => m.Id == 6).FirstOrDefault();
            //var monitoringKitv2Option = context.Options.Where(m => m.Id == 7).FirstOrDefault();
            //var advancedMonitoringOption = context.Options.Where(m => m.Id == 8).FirstOrDefault();
            //var touchlessScreenOption = context.Options.Where(m => m.Id == 9).FirstOrDefault();
            //var sigfoxConnectionOption = context.Options.Where(m => m.Id == 10).FirstOrDefault();
            //var permacultureBedOption = context.Options.Where(m => m.Id == 11).FirstOrDefault();
            //var permacultureBiocharOption = context.Options.Where(m => m.Id == 12).FirstOrDefault();

            //var userCTA = new ApplicationUser() { Email = "colette@myfood.eu", UserName = "colette@myfood.eu" };
            ////var userGME = new ApplicationUser() { Email = "gerard@myfood.eu", UserName = "gerard@myfood.eu" };
            ////var userMGV = new ApplicationUser() { Email = "michelg@myfood.eu", UserName = "michelg@myfood.eu" };
            ////var userFBA = new ApplicationUser() { Email = "felix@myfood.eu", UserName = "felix@myfood.eu" };
            ////var userCGT = new ApplicationUser() { Email = "casinoforges@myfood.eu", UserName = "casinoforges@myfood.eu" };
            ////var userMBA = new ApplicationUser() { Email = "myriam@myfood.eu", UserName = "myriam@myfood.eu" };

            //var store = new UserStore<ApplicationUser>(context);
            //var manager = new ApplicationUserManager(store);

            //var defaultPassword = ConfigurationManager.AppSettings["defaultPassword"];

            //var t = Task.Run(async () =>
            //{
            //    await manager.CreateAsync(userCTA, defaultPassword);
            //    //await manager.CreateAsync(userGME, defaultPassword);
            //    //await manager.CreateAsync(userMGV, defaultPassword);
            //    //await manager.CreateAsync(userFBA, defaultPassword);
            //    //await manager.CreateAsync(userCGT, defaultPassword);
            //    //await manager.CreateAsync(userMBA, defaultPassword);
            //});

            //t.Wait();

            //var ColetteTOwner = new ProductionUnitOwner() { Id = 49, user = userCTA, pioneerCitizenName = "Colette T." };
            ////var GerardMOwner = new ProductionUnitOwner() { Id = 50, user = userGME, pioneerCitizenName = "Gérard M." };
            ////var MichelGOwner = new ProductionUnitOwner() { Id = 51, user = userMGV, pioneerCitizenName = "Michel G." };
            ////var FelixBOwner = new ProductionUnitOwner() { Id = 52, user = userFBA, pioneerCitizenName = "Felix B." };
            ////var CasinoForgesFOwner = new ProductionUnitOwner() { Id = 53, user = userCGT, pioneerCitizenName = "Casino Forges" };
            ////var MyriamBOwner = new ProductionUnitOwner() { Id = 54, user = userMBA, pioneerCitizenName = "Myriam B." };

            //context.ProductionUnitOwners.Add(ColetteTOwner);
            ////context.ProductionUnitOwners.Add(GerardMOwner);
            ////context.ProductionUnitOwners.Add(MichelGOwner);
            ////context.ProductionUnitOwners.Add(FelixBOwner);
            ////context.ProductionUnitOwners.Add(CasinoForgesFOwner);
            ////context.ProductionUnitOwners.Add(MyriamBOwner);

            //var CTAProdUnit = new ProductionUnit()
            //{
            //    locationLatitude = 48.9311498,
            //    locationLongitude = 2.188086,
            //    reference = "FTATXX",
            //    info = "Restaurant Luzzu",
            //    startDate = new DateTime(2017, 07, 8),
            //    version = "2",
            //    owner = ColetteTOwner,
            //    productionUnitType = prodUnitTypeFam22,
            //    hydroponicType = hydroTypeAquaponicsWarmFish,
            //    productionUnitStatus = prodUnitStatusRunning,
            //};

            ////var GMEProdUnit = new ProductionUnit()
            ////{
            ////    locationLatitude = 44.2419021,
            ////    locationLongitude = -1.0765354,
            ////    reference = "FTAT03",
            ////    info = "Family Experimentation",
            ////    startDate = new DateTime(2017, 07, 13),
            ////    version = "2",
            ////    owner = GerardMOwner,
            ////    productionUnitType = prodUnitTypeFam22,
            ////    hydroponicType = hydroTypeAquaponicsWarmFish,
            ////    productionUnitStatus = prodUnitStatusReadyForInstall,
            ////};

            ////var MGVProdUnit = new ProductionUnit()
            ////{
            ////    locationLatitude = 48.8715998,
            ////    locationLongitude = 2.1457637,
            ////    reference = "FTAT57",
            ////    info = "Family Experimentation",
            ////    startDate = new DateTime(2017, 07, 03),
            ////    version = "2",
            ////    owner = MichelGOwner,
            ////    productionUnitType = prodUnitTypeFam22,
            ////    hydroponicType = hydroTypeAquaponicsWarmFish,
            ////    productionUnitStatus = prodUnitStatusReadyForInstall,
            ////};

            ////var FBAProdUnit = new ProductionUnit()
            ////{
            ////    locationLatitude = 51.4408238,
            ////    locationLongitude = 6.8759363,
            ////    reference = "FTAT01",
            ////    info = "Family Experimentation",
            ////    startDate = new DateTime(2017, 07, 06),
            ////    version = "2",
            ////    owner = FelixBOwner,
            ////    productionUnitType = prodUnitTypeFam14,
            ////    hydroponicType = hydroTypeAquaponicsWarmFish,
            ////    productionUnitStatus = prodUnitStatusReadyForInstall,
            ////};

            ////var CGTProdUnit = new ProductionUnit()
            ////{
            ////    locationLatitude = 49.6129522,
            ////    locationLongitude = 1.5162938,
            ////    reference = "FTAT03",
            ////    info = "Family Experimentation",
            ////    startDate = new DateTime(2017, 07, 18),
            ////    version = "2",
            ////    owner = CasinoForgesFOwner,
            ////    productionUnitType = prodUnitTypeFam22,
            ////    hydroponicType = hydroTypeAquaponicsWarmFish,
            ////    productionUnitStatus = prodUnitStatusReadyForInstall,
            ////};

            ////var MBAProdUnit = new ProductionUnit()
            ////{
            ////    locationLatitude = 48.7439002,
            ////    locationLongitude = 7.2836429,
            ////    reference = "FTAT57",
            ////    info = "Family Experimentation",
            ////    startDate = new DateTime(2017, 07, 20),
            ////    version = "2",
            ////    owner = MyriamBOwner,
            ////    productionUnitType = prodUnitTypeFam22,
            ////    hydroponicType = hydroTypeAquaponicsWarmFish,
            ////    productionUnitStatus = prodUnitStatusReadyForInstall,
            ////};

            //context.ProductionUnits.Add(CTAProdUnit);
            ////context.ProductionUnits.Add(GMEProdUnit);
            ////context.ProductionUnits.Add(MGVProdUnit);
            ////context.ProductionUnits.Add(FBAProdUnit);
            ////context.ProductionUnits.Add(CGTProdUnit);
            ////context.ProductionUnits.Add(MBAProdUnit);

            //context.SaveChanges();

            //var optionsCTA = new List<OptionList>();

            //optionsCTA.Add(new OptionList() { productionUnit = CTAProdUnit, option = monitoringKitv2Option });
            //optionsCTA.Add(new OptionList() { productionUnit = CTAProdUnit, option = towers18Option });
            //optionsCTA.Add(new OptionList() { productionUnit = CTAProdUnit, option = sigfoxConnectionOption });
            //optionsCTA.Add(new OptionList() { productionUnit = CTAProdUnit, option = permacultureBedOption });

            ////var optionsGME = new List<OptionList>();

            ////optionsGME.Add(new OptionList() { productionUnit = GMEProdUnit, option = monitoringKitv2Option });
            ////optionsGME.Add(new OptionList() { productionUnit = GMEProdUnit, option = towers18Option });
            ////optionsGME.Add(new OptionList() { productionUnit = GMEProdUnit, option = permacultureBedOption });
            ////optionsGME.Add(new OptionList() { productionUnit = GMEProdUnit, option = sigfoxConnectionOption });

            ////var optionsMGV = new List<OptionList>();

            ////optionsMGV.Add(new OptionList() { productionUnit = MGVProdUnit, option = monitoringKitv2Option });
            ////optionsMGV.Add(new OptionList() { productionUnit = MGVProdUnit, option = towers24Option });
            ////optionsMGV.Add(new OptionList() { productionUnit = MGVProdUnit, option = sigfoxConnectionOption });
            ////optionsMGV.Add(new OptionList() { productionUnit = MGVProdUnit, option = permacultureBedOption });

            ////var optionsFBA = new List<OptionList>();

            ////optionsFBA.Add(new OptionList() { productionUnit = FBAProdUnit, option = monitoringKitv2Option });
            ////optionsFBA.Add(new OptionList() { productionUnit = FBAProdUnit, option = towers18Option });
            ////optionsFBA.Add(new OptionList() { productionUnit = FBAProdUnit, option = permacultureBiocharOption });
            ////optionsFBA.Add(new OptionList() { productionUnit = FBAProdUnit, option = sigfoxConnectionOption });

            ////var optionsCGT = new List<OptionList>();

            ////optionsCGT.Add(new OptionList() { productionUnit = CGTProdUnit, option = monitoringKitv2Option });
            ////optionsCGT.Add(new OptionList() { productionUnit = CGTProdUnit, option = towers18Option });
            ////optionsCGT.Add(new OptionList() { productionUnit = CGTProdUnit, option = sigfoxConnectionOption });
            ////optionsCGT.Add(new OptionList() { productionUnit = CGTProdUnit, option = pelletStoveOption });

            ////var optionsMBA = new List<OptionList>();

            ////optionsMBA.Add(new OptionList() { productionUnit = MBAProdUnit, option = monitoringKitv2Option });
            ////optionsMBA.Add(new OptionList() { productionUnit = MBAProdUnit, option = towers18Option });
            ////optionsMBA.Add(new OptionList() { productionUnit = MBAProdUnit, option = solarPanelOption });
            ////optionsMBA.Add(new OptionList() { productionUnit = MBAProdUnit, option = permacultureBedOption });

            //context.OptionLists.AddRange(optionsCTA);
            ////context.OptionLists.AddRange(optionsGME);
            ////context.OptionLists.AddRange(optionsMGV);
            ////context.OptionLists.AddRange(optionsFBA);
            ////context.OptionLists.AddRange(optionsCGT);
            ////context.OptionLists.AddRange(optionsMBA);

            //context.SaveChanges();

        }
    }
}