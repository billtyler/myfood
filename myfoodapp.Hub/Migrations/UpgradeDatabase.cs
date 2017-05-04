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

            //var userALH = new ApplicationUser() { Email = "alain@myfood.eu", UserName = "alain@myfood.eu" };
            //var userVTH = new ApplicationUser() { Email = "vincent@myfood.eu", UserName = "vincent@myfood.eu" };

            //var store = new UserStore<ApplicationUser>(context);
            //var manager = new ApplicationUserManager(store);

            //var defaultPassword = ConfigurationManager.AppSettings["defaultPassword"];

            //var t = Task.Run(async () =>
            //{
            //    await manager.CreateAsync(userALH, defaultPassword);
            //    await manager.CreateAsync(userVTH, defaultPassword);
            //});

            //t.Wait();

            //var AlainHOwner = new ProductionUnitOwner() { Id = 34, user = userALH, pioneerCitizenName = "Alain H." };
            //var VincentTOwner = new ProductionUnitOwner() { Id = 35, user = userVTH, pioneerCitizenName = "Vincent T." };

            //context.ProductionUnitOwners.Add(AlainHOwner);
            //context.ProductionUnitOwners.Add(VincentTOwner);

            //context.SaveChanges();

            context.PreferedMoments.Add(new PreferedMoment() { Id = 0, name = "Morning" });
            context.PreferedMoments.Add(new PreferedMoment() { Id = 1, name = "Afternoon" });
            context.PreferedMoments.Add(new PreferedMoment() { Id = 2, name = "Night" });
            context.PreferedMoments.Add(new PreferedMoment() { Id = 3, name = "Weekend" });
            context.PreferedMoments.Add(new PreferedMoment() { Id = 4, name = "Noon" });
            context.PreferedMoments.Add(new PreferedMoment() { Id = 5, name = "All Day" });

            context.SaveChanges();

        }
    }
}