namespace myfoodapp.Hub.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Threading.Tasks;

    internal sealed class Configuration : DbMigrationsConfiguration<myfoodapp.Hub.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "myfoodapp.Hub.Models.ApplicationDbContext";
        }
        protected override void Seed(ApplicationDbContext context)
        {
            return;

            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();

            //CLEAN UP
            context.Measures.RemoveRange(context.Measures);
            context.SensorTypes.RemoveRange(context.SensorTypes);

            context.Messages.RemoveRange(context.Messages);
            context.MessageTypes.RemoveRange(context.MessageTypes);

            context.ProductionUnits.RemoveRange(context.ProductionUnits);

            context.ProductionUnitOwners.RemoveRange(context.ProductionUnitOwners);
            context.ProductionUnitTypes.RemoveRange(context.ProductionUnitTypes);
            context.ProductionUnitStatus.RemoveRange(context.ProductionUnitStatus);
            context.HydroponicTypes.RemoveRange(context.HydroponicTypes);

            context.Events.RemoveRange(context.Events);
            context.EventTypes.RemoveRange(context.EventTypes);

            context.OptionLists.RemoveRange(context.OptionLists);
            context.Options.RemoveRange(context.Options);

            context.OptionLists.RemoveRange(context.OptionLists);
            context.Options.RemoveRange(context.Options);

            context.Recipes.RemoveRange(context.Recipes);
            context.GardeningTypes.RemoveRange(context.GardeningTypes);
            context.WateringLevels.RemoveRange(context.WateringLevels);
            context.Months.RemoveRange(context.Months);
            context.Seasons.RemoveRange(context.Seasons);

            context.SaveChanges();

            //REQUIRED BUSINESS DATA
            context.MessageTypes.Add(new MessageType() { Id = 1, name = "Measure" });
            context.MessageTypes.Add(new MessageType() { Id = 2, name = "Health" });
            context.MessageTypes.Add(new MessageType() { Id = 3, name = "Performance" });
            context.MessageTypes.Add(new MessageType() { Id = 4, name = "Status" });

            context.SensorTypes.Add(new SensorType() { Id = 1, name = "pH Sensor", description = "Common values between 6-7" });
            context.SensorTypes.Add(new SensorType() { Id = 2, name = "Water Temperature Sensor", description = "Common values between 15-30" });
            context.SensorTypes.Add(new SensorType() { Id = 3, name = "Dissolved Oxygen Sensor", description = "Common values between 0-100" });
            context.SensorTypes.Add(new SensorType() { Id = 4, name = "ORP sensor", description = "Common values between 300-800" });
            context.SensorTypes.Add(new SensorType() { Id = 5, name = "Air Temperature Sensor", description = "Common values between 5-32" });
            context.SensorTypes.Add(new SensorType() { Id = 6, name = "Air Humidity Sensor", description = "Common values between 40-80" });

            context.ProductionUnitTypes.Add(new ProductionUnitType() { Id = 1, name = "Balcony" });
            context.ProductionUnitTypes.Add(new ProductionUnitType() { Id = 2, name = "City" });
            context.ProductionUnitTypes.Add(new ProductionUnitType() { Id = 3, name = "Family 14" });
            context.ProductionUnitTypes.Add(new ProductionUnitType() { Id = 4, name = "Family 22" });
            context.ProductionUnitTypes.Add(new ProductionUnitType() { Id = 5, name = "Farm" });
            context.ProductionUnitTypes.Add(new ProductionUnitType() { Id = 6, name = "Development Kit" });
            context.ProductionUnitTypes.Add(new ProductionUnitType() { Id = 7, name = "Custom Lab" });

            context.HydroponicTypes.Add(new HydroponicType() { Id = 1, name = "Not applicable" });
            context.HydroponicTypes.Add(new HydroponicType() { Id = 2, name = "Bioponics" });
            context.HydroponicTypes.Add(new HydroponicType() { Id = 3, name = "Aquaponics - Carp" });
            context.HydroponicTypes.Add(new HydroponicType() { Id = 4, name = "Aquaponics - Tilapia" });
            context.HydroponicTypes.Add(new HydroponicType() { Id = 5, name = "Aquaponics - Trout" });
            context.HydroponicTypes.Add(new HydroponicType() { Id = 6, name = "Aquaponics - Crayfish" });
            context.HydroponicTypes.Add(new HydroponicType() { Id = 7, name = "Aquaponics - Oth. Cold Fish" });
            context.HydroponicTypes.Add(new HydroponicType() { Id = 8, name = "Aquaponics - Oth. Warm Fish" });

            context.ProductionUnitStatus.Add(new ProductionUnitStatus() { Id = 1, name = "Wait Confirm." });
            context.ProductionUnitStatus.Add(new ProductionUnitStatus() { Id = 2, name = "Setup planned" });
            context.ProductionUnitStatus.Add(new ProductionUnitStatus() { Id = 3, name = "Up and Running" });
            context.ProductionUnitStatus.Add(new ProductionUnitStatus() { Id = 4, name = "In Maintenance" });
            context.ProductionUnitStatus.Add(new ProductionUnitStatus() { Id = 5, name = "Stopped" });

            context.EventTypes.Add(new EventType() { Id = 1, name = "Warning" });
            context.EventTypes.Add(new EventType() { Id = 2, name = "Issue" });
            context.EventTypes.Add(new EventType() { Id = 3, name = "Info" });
            context.EventTypes.Add(new EventType() { Id = 4, name = "Advices" });
            context.EventTypes.Add(new EventType() { Id = 5, name = "Improvement" });

            context.Options.Add(new Option() { Id = 0, name = "11 towers" });
            context.Options.Add(new Option() { Id = 1, name = "18 towers" });
            context.Options.Add(new Option() { Id = 2, name = "24 towers" });
            context.Options.Add(new Option() { Id = 3, name = "36 towers" });
            context.Options.Add(new Option() { Id = 4, name = "Solar panels" });
            context.Options.Add(new Option() { Id = 5, name = "Pellet stove" });
            context.Options.Add(new Option() { Id = 6, name = "Monitoring kit v.1" });
            context.Options.Add(new Option() { Id = 7, name = "Monitoring kit v.2" });
            context.Options.Add(new Option() { Id = 8, name = "Advanced monitoring" });
            context.Options.Add(new Option() { Id = 9, name = "Onboard touchscreen" });
            context.Options.Add(new Option() { Id = 10, name = "Sigfox connectivity kit" });
            context.Options.Add(new Option() { Id = 11, name = "Permaculture beds" });
            context.Options.Add(new Option() { Id = 12, name = "Permaculture beds & biochar" });

            context.Seasons.Add(new Season() { Id = 0, name = "Winter" });
            context.Seasons.Add(new Season() { Id = 1, name = "Spring" });
            context.Seasons.Add(new Season() { Id = 2, name = "Summer" });
            context.Seasons.Add(new Season() { Id = 3, name = "Autumn" });

            context.GardeningTypes.Add(new GardeningType() { Id = 0, name = "Permaculture Bed" });
            context.GardeningTypes.Add(new GardeningType() { Id = 1, name = "Zipgrow Tower" });
            context.GardeningTypes.Add(new GardeningType() { Id = 2, name = "Tower Garden" });
            context.GardeningTypes.Add(new GardeningType() { Id = 3, name = "All" });            
            context.GardeningTypes.Add(new GardeningType() { Id = 4, name = "Fish Tank" });
            
            context.WateringLevels.Add(new WateringLevel() { Id = 0, name = "Light" });
            context.WateringLevels.Add(new WateringLevel() { Id = 1, name = "Moderate" });
            context.WateringLevels.Add(new WateringLevel() { Id = 2, name = "Normal" });
            context.WateringLevels.Add(new WateringLevel() { Id = 3, name = "Maximum" });

            context.SaveChanges();

            context.Months.Add(new Month() { Id = 0, name = "January", order = 1, season = context.Seasons.Where(s => s.Id == 0).FirstOrDefault()});
            context.Months.Add(new Month() { Id = 1, name = "February", order = 2, season = context.Seasons.Where(s => s.Id == 0).FirstOrDefault() });
            context.Months.Add(new Month() { Id = 2, name = "March", order = 3, season = context.Seasons.Where(s => s.Id == 1).FirstOrDefault() });
            context.Months.Add(new Month() { Id = 3, name = "April", order = 4, season = context.Seasons.Where(s => s.Id == 1).FirstOrDefault() });
            context.Months.Add(new Month() { Id = 4, name = "May", order = 5, season = context.Seasons.Where(s => s.Id == 1).FirstOrDefault() });
            context.Months.Add(new Month() { Id = 5, name = "June", order = 6, season = context.Seasons.Where(s => s.Id == 2).FirstOrDefault() });
            context.Months.Add(new Month() { Id = 6, name = "July", order = 7, season = context.Seasons.Where(s => s.Id == 2).FirstOrDefault() });
            context.Months.Add(new Month() { Id = 7, name = "August", order = 8, season = context.Seasons.Where(s => s.Id == 2).FirstOrDefault() });
            context.Months.Add(new Month() { Id = 8, name = "September", order = 9, season = context.Seasons.Where(s => s.Id == 3).FirstOrDefault() });
            context.Months.Add(new Month() { Id = 9, name = "October", order = 10, season = context.Seasons.Where(s => s.Id == 3).FirstOrDefault() });
            context.Months.Add(new Month() { Id = 10, name = "November", order = 11, season = context.Seasons.Where(s => s.Id == 3).FirstOrDefault() });
            context.Months.Add(new Month() { Id = 11, name = "December", order = 12, season = context.Seasons.Where(s => s.Id == 0).FirstOrDefault() });

            context.SaveChanges();

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

            //LETTUCE
            var plantingIndoorMonthsLettuce = new List<Month>();
            plantingIndoorMonthsLettuce.Add(februaryMonth);
            plantingIndoorMonthsLettuce.Add(marchMonth);
            plantingIndoorMonthsLettuce.Add(aprilMonth);
            plantingIndoorMonthsLettuce.Add(mayMonth);
            plantingIndoorMonthsLettuce.Add(juneMonth);
            plantingIndoorMonthsLettuce.Add(julyMonth);
            plantingIndoorMonthsLettuce.Add(augustMonth);

            var plantingOutdoorMonthsLettuce = new List<Month>();
            plantingIndoorMonthsLettuce.Add(aprilMonth);
            plantingIndoorMonthsLettuce.Add(mayMonth);
            plantingIndoorMonthsLettuce.Add(juneMonth);
            plantingIndoorMonthsLettuce.Add(julyMonth);
            plantingIndoorMonthsLettuce.Add(augustMonth);

            var harvestMonthsLettuce = new List<Month>();
            plantingIndoorMonthsLettuce.Add(mayMonth);
            plantingIndoorMonthsLettuce.Add(juneMonth);
            plantingIndoorMonthsLettuce.Add(julyMonth);
            plantingIndoorMonthsLettuce.Add(augustMonth);
            plantingIndoorMonthsLettuce.Add(septemberMonth);
            plantingIndoorMonthsLettuce.Add(octoberMonth);

            var lettuceRecipe = new Recipes()
            {
                name = "Lettuce",
                description = "Lettuce (Lactuca sativa) is an annual plant of the daisy family, Asteraceae. It is most often grown as a leaf vegetable, but sometimes for its stem and seeds. Lettuce is most often used for salads, although it is also seen in other kinds of food, such as soups, sandwiches and wraps; it can also be grilled. One variety, the woju, or asparagus lettuce (celtuce), is grown for its stems, which are eaten either raw or cooked. In addition to its main use as a leafy green, it has also gathered religious and medicinal significance over centuries of human consumption. Europe and North America originally dominated the market for lettuce, but by the late 20th century the consumption of lettuce had spread throughout the world. World production of lettuce and chicory for calendar year 2013 was 24.9 million tonnes, over half of which came from China.",
                idealMinTemperature = 15,
                idealMaxTemperature = 21,
                acceptableMinTemperature = 0,
                acceptableMaxTemperature = 25,
                wateringLevel = normalWateringLevel,
                gardeningType = allGardeningType,
                proteinPercentage = 22,
                daysOfGermination = 10,
                daysOfHarvestFromSowing = 65,
                minimumSpaceBetweenPlantInTower = 30,
                plantingIndoorMonths = plantingIndoorMonthsLettuce,
                plantingOutdoorMonths = plantingOutdoorMonthsLettuce,
                harvestingMonths = harvestMonthsLettuce,
                reference = "SATI-1231",
                picturePath = "lettuce.jpg"                
            };

            context.Recipes.Add(lettuceRecipe);

            //TOMATOES
            var plantingIndoorMonthsTomatoes = new List<Month>();
            plantingIndoorMonthsTomatoes.Add(februaryMonth);
            plantingIndoorMonthsTomatoes.Add(marchMonth);

            var plantingOutdoorMonthsTomatoes = new List<Month>();
            plantingOutdoorMonthsTomatoes.Add(mayMonth);

            var harvestMonthsTomatoes = new List<Month>();
            harvestMonthsTomatoes.Add(julyMonth);
            harvestMonthsTomatoes.Add(augustMonth);
            harvestMonthsTomatoes.Add(septemberMonth);
            harvestMonthsTomatoes.Add(octoberMonth);

            var tomatoesRecipe = new Recipes()
            {
                name = "Tomatoes",
                description = "Its use as a food originated in Mexico, and spread throughout the world following the Spanish colonization of the Americas. Tomato is consumed in diverse ways, including raw, as an ingredient in many dishes, sauces, salads, and drinks. While tomatoes are botanically berry-type fruits, they are considered culinary vegetables, being ingredients of savory meals.",
                idealMinTemperature = 18,
                idealMaxTemperature = 29,
                acceptableMinTemperature = 13,
                acceptableMaxTemperature = 32,
                wateringLevel = normalWateringLevel,
                gardeningType = permacultureBedsGardeningType,
                proteinPercentage = 12,
                daysOfGermination = 10,
                daysOfHarvestFromSowing = 130,
                minimumSpaceBetweenPlantInTower = 45,
                plantingIndoorMonths = plantingIndoorMonthsTomatoes,
                plantingOutdoorMonths = plantingOutdoorMonthsTomatoes,
                harvestingMonths = harvestMonthsTomatoes,
                reference = "SATI-1231",
                picturePath = "tomatoes.jpg"
            };

            context.Recipes.Add(tomatoesRecipe);

            //CUCUMBER
            var plantingIndoorMonthsCucumber = new List<Month>();
            plantingIndoorMonthsCucumber.Add(aprilMonth);

            var plantingOutdoorMonthsCucumber = new List<Month>();
            plantingOutdoorMonthsCucumber.Add(mayMonth);
            plantingOutdoorMonthsCucumber.Add(juneMonth);

            var harvestMonthsCucumber = new List<Month>();
            harvestMonthsCucumber.Add(julyMonth);
            harvestMonthsCucumber.Add(augustMonth);
            harvestMonthsCucumber.Add(septemberMonth);

            var cucumberRecipe = new Recipes()
            {
                name = "Cucumber",
                description = "The cucumber is a creeping vine that roots in the ground and grows up trellises or other supporting frames, wrapping around supports with thin, spiraling tendrils. The plant may also root in a soilless medium and will sprawl along the ground if it does not have supports. The vine has large leaves that form a canopy over the fruits.",
                idealMinTemperature = 17,
                idealMaxTemperature = 32,
                acceptableMinTemperature = 15,
                acceptableMaxTemperature = 40,
                wateringLevel = normalWateringLevel,
                gardeningType = permacultureBedsGardeningType,
                proteinPercentage = 11,
                daysOfGermination = 10,
                daysOfHarvestFromSowing = 100,
                minimumSpaceBetweenPlantInTower = 15,
                plantingIndoorMonths = plantingIndoorMonthsCucumber,
                plantingOutdoorMonths = plantingOutdoorMonthsCucumber,
                harvestingMonths = harvestMonthsCucumber,
                reference = "SATI-1231",
                picturePath = "cucumber.jpg"
            };

            context.Recipes.Add(cucumberRecipe);

            //STRAWBERRY
            var plantingIndoorMonthsStrawberry = new List<Month>();
            plantingIndoorMonthsStrawberry.Add(augustMonth);
            plantingIndoorMonthsStrawberry.Add(septemberMonth);

            var plantingOutdoorMonthsStrawberry = new List<Month>();
            plantingOutdoorMonthsStrawberry.Add(augustMonth);
            plantingOutdoorMonthsStrawberry.Add(septemberMonth);

            var harvestMonthsStrawberry = new List<Month>();
            harvestMonthsStrawberry.Add(julyMonth);
            harvestMonthsStrawberry.Add(augustMonth);
            harvestMonthsStrawberry.Add(septemberMonth);
            harvestMonthsStrawberry.Add(octoberMonth);

            var strawberryRecipe = new Recipes()
            {
                name = "Strawberry",
                description = "The first garden strawberry was grown in Brittany, France during the late 18th century. Prior to this, wild strawberries and cultivated selections from wild strawberry species were the common source of the fruit.",
                idealMinTemperature = 15,
                idealMaxTemperature = 26,
                acceptableMinTemperature = 0,
                acceptableMaxTemperature = 29,
                wateringLevel = normalWateringLevel,
                gardeningType = allGardeningType,
                proteinPercentage = 7,
                daysOfGermination = 20,
                daysOfHarvestFromSowing = 170,
                minimumSpaceBetweenPlantInTower = 10,
                plantingIndoorMonths = plantingIndoorMonthsStrawberry,
                plantingOutdoorMonths = plantingOutdoorMonthsStrawberry,
                harvestingMonths = harvestMonthsStrawberry,
                reference = "SATI-1231",
                picturePath = "strawberries.jpg"
            };

            context.Recipes.Add(strawberryRecipe);

            //GREENBEANS
            var plantingIndoorMonthsGreenBeans = new List<Month>();
            plantingIndoorMonthsGreenBeans.Add(marchMonth);
            plantingIndoorMonthsGreenBeans.Add(aprilMonth);

            var plantingOutdoorMonthsGreenBeans = new List<Month>();
            plantingOutdoorMonthsGreenBeans.Add(mayMonth);
            plantingOutdoorMonthsGreenBeans.Add(juneMonth);
            plantingOutdoorMonthsGreenBeans.Add(julyMonth);

            var harvestMonthsGreenBeans = new List<Month>();
            harvestMonthsGreenBeans.Add(augustMonth);
            harvestMonthsGreenBeans.Add(septemberMonth);

            var greenBeanRecipe = new Recipes()
            {
                name = "Green Beans",
                description = "Green beans, also known as French beans, string beans, or snap beans, are the unripe fruit and protective pods of various cultivars of the common bean (Phaseolus vulgaris). Immature pods of the runner bean (Phaseolus coccineus), yardlong bean (Vigna unguiculata subsp. sesquipedalis), and hyancinth bean (Lablab purpureus), are also used as snap beans.",
                idealMinTemperature = 18,
                idealMaxTemperature = 30,
                acceptableMinTemperature = 9,
                acceptableMaxTemperature = 32,
                wateringLevel = normalWateringLevel,
                gardeningType = allGardeningType,
                proteinPercentage = 14,
                daysOfGermination = 14,
                daysOfHarvestFromSowing = 70,
                minimumSpaceBetweenPlantInTower = 10,
                plantingIndoorMonths = plantingIndoorMonthsGreenBeans,
                plantingOutdoorMonths = plantingOutdoorMonthsGreenBeans,
                harvestingMonths = harvestMonthsGreenBeans,
                reference = "SATI-1231",
                picturePath = "greenbeans.jpg"
            };

            context.Recipes.Add(greenBeanRecipe);

            //CARROT
            var plantingIndoorMonthsCarrot = new List<Month>();
            plantingIndoorMonthsCarrot.Add(mayMonth);
            plantingIndoorMonthsCarrot.Add(juneMonth);
            plantingIndoorMonthsCarrot.Add(julyMonth);

            var plantingOutdoorMonthsCarrot = new List<Month>();
            plantingOutdoorMonthsCarrot.Add(mayMonth);
            plantingOutdoorMonthsCarrot.Add(juneMonth);
            plantingOutdoorMonthsCarrot.Add(julyMonth);

            var harvestMonthsCarrot = new List<Month>();
            harvestMonthsCarrot.Add(augustMonth);
            harvestMonthsCarrot.Add(septemberMonth);
            harvestMonthsCarrot.Add(octoberMonth);
            harvestMonthsCarrot.Add(novemberMonth);
            harvestMonthsCarrot.Add(decemberMonth);

            var carrotRecipe = new Recipes()
            {
                name = "Carrot",
                description = "The carrot (Daucus carota subsp. sativus) is a root vegetable, usually orange in colour, though purple, black, red, white, and yellow varieties exist. Carrots are a domesticated form of the wild carrot, Daucus carota, native to Europe and southwestern Asia.",
                idealMinTemperature = 15,
                idealMaxTemperature = 21,
                acceptableMinTemperature = 10,
                acceptableMaxTemperature = 25,
                wateringLevel = normalWateringLevel,
                gardeningType = permacultureBedsGardeningType,
                proteinPercentage = 6,
                daysOfGermination = 15,
                daysOfHarvestFromSowing = 240,
                minimumSpaceBetweenPlantInTower = 10,
                plantingIndoorMonths = plantingIndoorMonthsCarrot,
                plantingOutdoorMonths = plantingIndoorMonthsCarrot,
                harvestingMonths = plantingOutdoorMonthsCarrot,
                reference = "SATI-1231",
                picturePath = "carrot.jpg"
            };

            context.Recipes.Add(carrotRecipe);

            //BASIL
            var plantingIndoorMonthsBasil = new List<Month>();
            plantingIndoorMonthsBasil.Add(marchMonth);

            var plantingOutdoorMonthsBasil = new List<Month>();
            plantingOutdoorMonthsBasil.Add(marchMonth);
            plantingOutdoorMonthsBasil.Add(aprilMonth);

            var harvestMonthsBasil = new List<Month>();
            harvestMonthsBasil.Add(juneMonth);
            harvestMonthsBasil.Add(julyMonth);
            harvestMonthsBasil.Add(augustMonth);

            var basilRecipe = new Recipes()
            {
                name = "Basil",
                description = "The word basil comes from the Greek basileus, meaning king, as it has come to be associated with the Feast of the Cross commemorating the finding of the True Cross by St. Helena, mother of the emperor Constantine I.",
                idealMinTemperature = 12,
                idealMaxTemperature = 23,
                acceptableMinTemperature = 10,
                acceptableMaxTemperature = 29,
                wateringLevel = lightWateringLevel,
                gardeningType = allGardeningType,
                proteinPercentage = 6,
                daysOfGermination = 5,
                daysOfHarvestFromSowing = 30,
                minimumSpaceBetweenPlantInTower = 10,
                plantingIndoorMonths = plantingIndoorMonthsBasil,
                plantingOutdoorMonths = plantingOutdoorMonthsBasil,
                harvestingMonths = harvestMonthsBasil,
                reference = "SATI-1231",
                picturePath = "basil.jpg"
            };

            context.Recipes.Add(basilRecipe);

            //KALE
            var plantingIndoorMonthsKale = new List<Month>();
            plantingIndoorMonthsKale.Add(marchMonth);

            var plantingOutdoorMonthsKale = new List<Month>();
            plantingOutdoorMonthsKale.Add(marchMonth);
            plantingOutdoorMonthsKale.Add(aprilMonth);

            var harvestMonthsKale = new List<Month>();
            harvestMonthsKale.Add(augustMonth);
            harvestMonthsKale.Add(septemberMonth);
            harvestMonthsKale.Add(octoberMonth);
            harvestMonthsKale.Add(novemberMonth);
            harvestMonthsKale.Add(decemberMonth);
            harvestMonthsKale.Add(januaryMonth);
            harvestMonthsKale.Add(februaryMonth);

            var kaleRecipe = new Recipes()
            {
                name = "Kale",
                description = "Kale or leaf cabbage is a group of vegetable cultivars within the plant species Brassica oleracea. They have green or purple leaves, in which the central leaves do not form a head (as opposed to headed cabbages).",
                idealMinTemperature = 12,
                idealMaxTemperature = 18,
                acceptableMinTemperature = 0,
                acceptableMaxTemperature = 26,
                wateringLevel = lightWateringLevel,
                gardeningType = allGardeningType,
                proteinPercentage = 16,
                daysOfGermination = 10,
                daysOfHarvestFromSowing = 160,
                minimumSpaceBetweenPlantInTower = 40,
                plantingIndoorMonths = plantingIndoorMonthsKale,
                plantingOutdoorMonths = plantingOutdoorMonthsKale,
                harvestingMonths = harvestMonthsKale,
                reference = "SATI-1231",
                picturePath = "kale.jpg"
            };

            context.Recipes.Add(kaleRecipe);

            context.SaveChanges();

            var store = new UserStore<ApplicationUser>(context);
            var manager = new ApplicationUserManager(store);
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            manager.Users.ToList().ForEach(u =>
            {
                var task = Task.Run(async () => { await store.DeleteAsync(u); });
                task.Wait();
            });

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

            //USERS
            //GREENHOUSE OWNERS           
            var userMGA = new ApplicationUser() { Email = "mickael@myfood.eu", UserName = "mickael@myfood.eu" };
            var userMUR = new ApplicationUser() { Email = "matthieu@myfood.eu", UserName = "matthieu@myfood.eu" };
            var userJNA = new ApplicationUser() { Email = "johan@myfood.eu", UserName = "johan@myfood.eu" };

            var userCLA = new ApplicationUser() { Email = "christophel@myfood.eu", UserName = "christophel@myfood.eu" };
            var userJPM = new ApplicationUser() { Email = "jean-philippe@myfood.eu", UserName = "jean-philippe@myfood.eu" };
            var userRRO = new ApplicationUser() { Email = "rosario@myfood.eu", UserName = "rosario@myfood.eu" };
            var userAMA = new ApplicationUser() { Email = "andrew@myfood.eu", UserName = "andrew@myfood.eu" };
            var userSCR = new ApplicationUser() { Email = "sebastien@myfood.eu", UserName = "sebastien@myfood.eu" };
            var userMSV = new ApplicationUser() { Email = "michel@myfood.eu", UserName = "michel@myfood.eu" };
            var userDPR = new ApplicationUser() { Email = "didier@myfood.eu", UserName = "didier@myfood.eu" };
            var userCPE = new ApplicationUser() { Email = "christophep@myfood.eu", UserName = "christophep@myfood.eu" };
            var userSCO = new ApplicationUser() { Email = "sabine@myfood.eu", UserName = "sabine@myfood.eu" };
            var userPTO = new ApplicationUser() { Email = "philippe@myfood.eu", UserName = "philippe@myfood.eu" };
            var userSAS = new ApplicationUser() { Email = "stan@myfood.eu", UserName = "stan@myfood.eu" };
            var userCWI = new ApplicationUser() { Email = "christiane@myfood.eu", UserName = "christiane@myfood.eu" };
            var userMWI = new ApplicationUser() { Email = "margot@myfood.eu", UserName = "margot@myfood.eu" };
            var userGDE = new ApplicationUser() { Email = "gilles@myfood.eu", UserName = "gilles@myfood.eu" };
            var userSMA = new ApplicationUser() { Email = "stephane@myfood.eu", UserName = "stephane@myfood.eu" };
            var userDMA = new ApplicationUser() { Email = "dario@myfood.eu", UserName = "dario@myfood.eu" };
            var userSGP = new ApplicationUser() { Email = "sgp@myfood.eu", UserName = "sdp@myfood.eu" };
            var userBDE = new ApplicationUser() { Email = "benoit@myfood.eu", UserName = "benoit@myfood.eu" };

            //TO BE DEPLOYED
            var userCDE = new ApplicationUser() { Email = "cristof@myfood.eu", UserName = "cristof@myfood.eu" };
            var userAHE = new ApplicationUser() { Email = "amous@myfood.eu", UserName = "amous@myfood.eu" };
            var userDMO = new ApplicationUser() { Email = "donatien@myfood.eu", UserName = "donatien@myfood.eu" };
            var userMSE = new ApplicationUser() { Email = "sevran@myfood.eu", UserName = "sevran@myfood.eu" };
            var userEDC = new ApplicationUser() { Email = "eleonore@myfood.eu", UserName = "eleonore@myfood.eu" };

            //TO BE CONFIRMED
            var userMLA = new ApplicationUser() { Email = "marc@myfood.eu", UserName = "marc@myfood.eu" };
            var userBGU = new ApplicationUser() { Email = "brigitte@myfood.eu", UserName = "brigitte@myfood.eu" };
            var userPCL = new ApplicationUser() { Email = "pieterjan@myfood.eu", UserName = "pieterjan@myfood.eu" };

            //CONTRIBUTORS
            var userJTE = new ApplicationUser() { Email = "joel@myfood.eu", UserName = "joel@myfood.eu" };
            var userAPO = new ApplicationUser() { Email = "anhhung@myfood.eu", UserName = "anhhung@myfood.eu" };
            var userNRO = new ApplicationUser() { Email = "nicolas@myfood.eu", UserName = "nicolas@myfood.eu" };
            var userCEL = new ApplicationUser() { Email = "cyrille@myfood.eu", UserName = "cyrille@myfood.eu" };

            var defaultPassword = ConfigurationManager.AppSettings["defaultPassword"];

            var t = Task.Run(async () =>
            {
                //ADD USERS
                //GREENHOUSE OWNERS  
                await manager.CreateAsync(userMGA, defaultPassword);
                await manager.CreateAsync(userMUR, defaultPassword);
                await manager.CreateAsync(userJNA, defaultPassword);

                await manager.CreateAsync(userCLA, defaultPassword);
                await manager.CreateAsync(userJPM, defaultPassword);
                await manager.CreateAsync(userRRO, defaultPassword);
                await manager.CreateAsync(userAMA, defaultPassword);
                await manager.CreateAsync(userSCR, defaultPassword);
                await manager.CreateAsync(userMSV, defaultPassword);
                await manager.CreateAsync(userDPR, defaultPassword);
                await manager.CreateAsync(userCPE, defaultPassword);
                await manager.CreateAsync(userPTO, defaultPassword);
                await manager.CreateAsync(userSCO, defaultPassword);
                await manager.CreateAsync(userSAS, defaultPassword);
                await manager.CreateAsync(userCWI, defaultPassword);
                await manager.CreateAsync(userGDE, defaultPassword);
                await manager.CreateAsync(userMWI, defaultPassword);
                await manager.CreateAsync(userSMA, defaultPassword);
                await manager.CreateAsync(userDMA, defaultPassword);
                await manager.CreateAsync(userSGP, defaultPassword);

                //TO BE DEPLOYED                   
                await manager.CreateAsync(userCDE, defaultPassword);
                await manager.CreateAsync(userAHE, defaultPassword);
                await manager.CreateAsync(userBDE, defaultPassword);
                await manager.CreateAsync(userDMO, defaultPassword);
                await manager.CreateAsync(userMSE, defaultPassword);
                await manager.CreateAsync(userEDC, defaultPassword);

                //TO BE CONFIRMED
                await manager.CreateAsync(userPCL, defaultPassword);
                await manager.CreateAsync(userMLA, defaultPassword);
                await manager.CreateAsync(userBGU, defaultPassword);

                //CONTRIBUTORS
                await manager.CreateAsync(userJTE, defaultPassword);
                await manager.CreateAsync(userAPO, defaultPassword);
                await manager.CreateAsync(userNRO, defaultPassword);
                await manager.CreateAsync(userCEL, defaultPassword);

            });
            t.Wait();

            var r = Task.Run(async () =>
            {
                var MGA = await manager.FindByNameAsync("mickael@myfood.eu");
                await manager.AddToRoleAsync(MGA.Id, "Admin");
            });
            r.Wait();

            //PRODUCTION UNIT OWNERS
            //GREENHOUSE OWNERS 

            var MickaelGOwner = new ProductionUnitOwner() { Id = 1, user = userMGA, pioneerCitizenName = "Mickaël G." };
            var MatthieuUOwner = new ProductionUnitOwner() { Id = 2, user = userMUR, pioneerCitizenName = "Matthieu U." };
            var JohanNOwner = new ProductionUnitOwner() { Id = 3, user = userJNA, pioneerCitizenName = "Johan N." };

            var ChristopheLOwner = new ProductionUnitOwner() { Id = 4, user = userCLA, pioneerCitizenName = "Christophe L.", pioneerCitizenNumber = 1 };
            var JeanPhilippeMGOwner = new ProductionUnitOwner() { Id = 5, user = userJPM, pioneerCitizenName = "Jean-Philippe M.", pioneerCitizenNumber = 2 };
            var AndrewMOwner = new ProductionUnitOwner() { Id = 6, user = userAMA, pioneerCitizenName = "Andrew M.", pioneerCitizenNumber = 3 };
            var RosarioRGOwner = new ProductionUnitOwner() { Id = 7, user = userRRO, pioneerCitizenName = "Rosario M.", pioneerCitizenNumber = 4 };
            var SebastienCOwner = new ProductionUnitOwner() { Id = 8, user = userSCR, pioneerCitizenName = "Sébastien C.", pioneerCitizenNumber = 5 };
            var MichelVSOwner = new ProductionUnitOwner() { Id = 9, user = userMSV, pioneerCitizenName = "Michel VS.", pioneerCitizenNumber = 7 };
            var DidierPOwner = new ProductionUnitOwner() { Id = 10, user = userDPR, pioneerCitizenName = "Didier P.", pioneerCitizenNumber = 8 };
            var ChristophePOwner = new ProductionUnitOwner() { Id = 11, user = userCPE, pioneerCitizenName = "Christophe P.", pioneerCitizenNumber = 9 };
            var PhilippeTOwner = new ProductionUnitOwner() { Id = 12, user = userPTO, pioneerCitizenName = "Philippe T.", pioneerCitizenNumber = 10 };
            var SabineCOwner = new ProductionUnitOwner() { Id = 13, user = userSCO, pioneerCitizenName = "Sabine C.", pioneerCitizenNumber = 11 };

            var StanAOwner = new ProductionUnitOwner() { Id = 14, user = userSAS, pioneerCitizenName = "Stan A.", pioneerCitizenNumber = 12 };
            var ChristianeWOwner = new ProductionUnitOwner() { Id = 15, user = userCWI, pioneerCitizenName = "Christiane W.", pioneerCitizenNumber = 13 };
            var GillesDOwner = new ProductionUnitOwner() { Id = 16, user = userGDE, pioneerCitizenName = "Gilles D.", pioneerCitizenNumber = 15 };
            var MargotWOwner = new ProductionUnitOwner() { Id = 17, user = userMWI, pioneerCitizenName = "Margot W.", pioneerCitizenNumber = 16 };
            var StephaneMOwner = new ProductionUnitOwner() { Id = 18, user = userSMA, pioneerCitizenName = "Stéphane M.", pioneerCitizenNumber = 18 };
            var DarioMOwner = new ProductionUnitOwner() { Id = 19, user = userDMA, pioneerCitizenName = "Dario M.", pioneerCitizenNumber = 19 };
            var SGPOwner = new ProductionUnitOwner() { Id = 20, user = userSGP, pioneerCitizenName = "Soc. Grand Paris" };
            var CristofDOwner = new ProductionUnitOwner() { Id = 21, user = userCDE, pioneerCitizenName = "Cristof D.", pioneerCitizenNumber = 20 };
            var DonatienMOwner = new ProductionUnitOwner() { Id = 24, user = userBDE, pioneerCitizenName = "Donatien M.", pioneerCitizenNumber = 21 };

            //TO BE DEPLOYED 
            var CyrilleEOwner = new ProductionUnitOwner() { Id = 30, user = userCEL, pioneerCitizenName = "Cyrille E." };
            var AmousHOwner = new ProductionUnitOwner() { Id = 22, user = userAHE, pioneerCitizenName = "Amous H.", pioneerCitizenNumber = 17 };
            var BenoitDOwner = new ProductionUnitOwner() { Id = 23, user = userBDE, pioneerCitizenName = "Benoit D." };
            var MairieSOwner = new ProductionUnitOwner() { Id = 23, user = userMSE, pioneerCitizenName = "Mairie Sevran" };
            var EleonoreDCOwner = new ProductionUnitOwner() { Id = 24, user = userEDC, pioneerCitizenName = "Eléornore DC." };

            //TO BE CONFIRMED
            var MarcLOwner = new ProductionUnitOwner() { Id = 24, user = userMLA, pioneerCitizenName = "Marc L." };
            var BrigitteGOwner = new ProductionUnitOwner() { Id = 25, user = userBGU, pioneerCitizenName = "Brigitte G." };

            //CONTRIBUTORS
            var JoelTOwner = new ProductionUnitOwner() { Id = 27, user = userJTE, pioneerCitizenName = "Joël T." };
            var AnhHungPOwner = new ProductionUnitOwner() { Id = 28, user = userAPO, pioneerCitizenName = "Anh Hung P." };
            var NicolasROwner = new ProductionUnitOwner() { Id = 29, user = userNRO, pioneerCitizenName = "Nicolas R." };

            //ADD PRODUCTION UNIT OWNERS
            //GREENHOUSE OWNERS 
            context.ProductionUnitOwners.Add(MickaelGOwner);
            context.ProductionUnitOwners.Add(MatthieuUOwner);
            context.ProductionUnitOwners.Add(JohanNOwner);

            context.ProductionUnitOwners.Add(ChristopheLOwner);
            context.ProductionUnitOwners.Add(JeanPhilippeMGOwner);
            context.ProductionUnitOwners.Add(AndrewMOwner);
            context.ProductionUnitOwners.Add(RosarioRGOwner);
            context.ProductionUnitOwners.Add(SebastienCOwner);
            context.ProductionUnitOwners.Add(MichelVSOwner);
            context.ProductionUnitOwners.Add(DidierPOwner);
            context.ProductionUnitOwners.Add(ChristophePOwner);
            context.ProductionUnitOwners.Add(PhilippeTOwner);
            context.ProductionUnitOwners.Add(SabineCOwner);
            context.ProductionUnitOwners.Add(StanAOwner);
            context.ProductionUnitOwners.Add(ChristianeWOwner);
            context.ProductionUnitOwners.Add(MargotWOwner);
            context.ProductionUnitOwners.Add(GillesDOwner);
            context.ProductionUnitOwners.Add(StephaneMOwner);
            context.ProductionUnitOwners.Add(DarioMOwner);
            context.ProductionUnitOwners.Add(SGPOwner);
            context.ProductionUnitOwners.Add(CristofDOwner);
            context.ProductionUnitOwners.Add(AmousHOwner);

            //TO BE DEPLOYED 
            context.ProductionUnitOwners.Add(CyrilleEOwner);
            context.ProductionUnitOwners.Add(BenoitDOwner);
            context.ProductionUnitOwners.Add(DonatienMOwner);
            context.ProductionUnitOwners.Add(MairieSOwner);
            context.ProductionUnitOwners.Add(EleonoreDCOwner);

            //TO BE CONFIRMED
            context.ProductionUnitOwners.Add(MarcLOwner);
            context.ProductionUnitOwners.Add(BrigitteGOwner);

            //CONTRIBUTORS
            context.ProductionUnitOwners.Add(JoelTOwner);
            context.ProductionUnitOwners.Add(AnhHungPOwner);
            context.ProductionUnitOwners.Add(NicolasROwner);

            context.SaveChanges();


            //PRODUCTION UNITS
            //GREENHOUSE OWNERS 
            var MGAProdUnit = new ProductionUnit()
            {
                locationLatitude = 49.148315,
                locationLongitude = 6.300190,
                reference = "74711",
                info = "Family Farm Sainte Barbe",
                startDate = new DateTime(2013, 01, 01),
                version = "2",
                owner = MickaelGOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeAquaponicsColdFish,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "SainteBarbeFamily22.jpg"
            };

            var MURProdUnit = new ProductionUnit()
            {
                locationLatitude = 48.4127102,
                locationLongitude = 7.4652961,
                reference = "74621",
                info = "Family Farm Gertwiller",
                startDate = new DateTime(2016, 01, 20),
                version = "2",
                owner = MatthieuUOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeAquaponicsCarp,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "GertwillerFamily22.jpg"
            };

            var JNAProdUnit = new ProductionUnit()
            {
                locationLatitude = 48.9370822,
                locationLongitude = 2.440039,
                reference = "79123",
                info = "Parisian Showroom",
                startDate = new DateTime(2016, 09, 20),
                version = "1",
                owner = JohanNOwner,
                productionUnitType = prodUnitTypeCity,
                hydroponicType = hydroTypeAquaponicsColdFish,
                productionUnitStatus = prodUnitStatusRunning,
            };

            var CLAProdUnit = new ProductionUnit()
            {
                locationLatitude = 49.9284996,
                locationLongitude = 1.0752494,
                reference = "76981",
                info = "Family Experimentation",
                startDate = new DateTime(2015, 11, 16),
                version = "1",
                owner = ChristopheLOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeAquaponicsColdFish,
                productionUnitStatus = prodUnitStatusMaintenance,
                picturePath = "BivilleFamily22.jpg"
            };

            var JPMProdUnit = new ProductionUnit()
            {
                locationLatitude = 49.5783078,
                locationLongitude = 0.9141116,
                reference = "76399",
                info = "Solar Greenhouse Showcase",
                startDate = new DateTime(2016, 04, 07),
                version = "1",
                owner = JeanPhilippeMGOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeAquaponicsColdFish,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "PavillyFamily22.jpg"
            };

            var AMAProdUnit = new ProductionUnit()
            {
                locationLatitude = 41.1258641,
                locationLongitude = 1.2035542,
                reference = "76789",
                info = "Off-the-Grid Experimentation",
                startDate = new DateTime(2016, 04, 14),
                version = "1",
                owner = AndrewMOwner,
                productionUnitType = prodUnitTypeFam14,
                hydroponicType = hydroTypeAquaponicsWarmFish,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "TarragonaFamily14.jpg"
            };

            var RROProdUnit = new ProductionUnit()
            {
                locationLatitude = 48.9136864,
                locationLongitude = 2.6471735,
                reference = "76399",
                info = "Permaculture Garden",
                startDate = new DateTime(2016, 04, 21),
                version = "1",
                owner = RosarioRGOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeNotApplicable,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "VillevaudeFamily22.jpg"
            };

            var SCRProdUnit = new ProductionUnit()
            {
                locationLatitude = 45.7498386,
                locationLongitude = 3.2077446,
                reference = "70123",
                info = "Agronomist Experimentation",
                startDate = new DateTime(2016, 04, 28),
                version = "1",
                owner = SebastienCOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeNotApplicable,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "MezelFamily22.jpg"
            };

            var MSVProdUnit = new ProductionUnit()
            {
                locationLatitude = 43.7031655,
                locationLongitude = 7.1828945,
                reference = "76909",
                info = "Family Garden",
                startDate = new DateTime(2016, 05, 25),
                version = "1",
                owner = MichelVSOwner,
                productionUnitType = prodUnitTypeFam14,
                hydroponicType = hydroTypeAquaponicsWarmFish,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "CannesFamily14.jpg"
            };

            var DPRProdUnit = new ProductionUnit()
            {
                locationLatitude = 47.3384192,
                locationLongitude = -1.3903849,
                reference = "76321",
                info = "Family Experimentation",
                startDate = new DateTime(2016, 06, 01),
                version = "1",
                owner = DidierPOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeAquaponicsTilapia,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "LeCeillierFamily22.jpg"
            };

            var CPEProdUnit = new ProductionUnit()
            {
                locationLatitude = 48.7195359,
                locationLongitude = 5.239357,
                reference = "74096",
                info = "Organic Farm Exploitation",
                startDate = new DateTime(2016, 06, 11),
                version = "1",
                owner = ChristophePOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeAquaponicsTilapia,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "GuerpontFamily22.jpg"
            };

            var PTOProdUnit = new ProductionUnit()
            {
                locationLatitude = 50.7398027,
                locationLongitude = 4.8197652,
                reference = "74916",
                info = "Family Experimentation",
                startDate = new DateTime(2016, 06, 29),
                version = "1",
                owner = PhilippeTOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeAquaponicsColdFish,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "MelinFamily22.jpg"
            };

            var SCOProdUnit = new ProductionUnit()
            {
                locationLatitude = 50.6796641,
                locationLongitude = 4.2504106,
                reference = "74996",
                info = "Family Experimentation",
                startDate = new DateTime(2016, 06, 27),
                version = "1",
                owner = SabineCOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeAquaponicsColdFish,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "BraineFamily22.jpg"
            };

            var SASProdUnit = new ProductionUnit()
            {
                locationLatitude = 47.9038042,
                locationLongitude = 1.5004396,
                reference = "74776",
                info = "Family Experimentation",
                startDate = new DateTime(2016, 07, 02),
                version = "1",
                owner = StanAOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeAquaponicsColdFish,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "OuzoueFamily22.jpg"
            };

            var CWIProdUnit = new ProductionUnit()
            {
                locationLatitude = 49.7287232,
                locationLongitude = 5.8390948,
                reference = "746F6",
                info = "Pall Center Oberpallen",
                startDate = new DateTime(2016, 10, 01),
                version = "2",
                owner = ChristianeWOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeAquaponicsCarp,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "OberpallenFamily22.jpg"
            };

            var GDEProdUnit = new ProductionUnit()
            {
                locationLatitude = 44.8313483,
                locationLongitude = -0.7519868,
                reference = "76123",
                info = "Family Experimentation",
                startDate = new DateTime(2016, 11, 07),
                version = "2",
                owner = GillesDOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeAquaponicsColdFish,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "MerigniacFamily22.JPG"
            };

            var MWIProdUnit = new ProductionUnit()
            {
                locationLatitude = 48.9151423,
                locationLongitude = 2.2513185,
                reference = "1AD15B",
                info = "Urban Experimentation",
                startDate = new DateTime(2016, 10, 15),
                version = "1",
                owner = MargotWOwner,
                productionUnitType = prodUnitTypeCity,
                hydroponicType = hydroTypeAquaponicsColdFish,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "BoisColombesCity.jpg"
            };

            var SMAProdUnit = new ProductionUnit()
            {
                locationLatitude = 45.735519,
                locationLongitude = 4.8941029,
                reference = "76423",
                info = "Family Experimentation",
                startDate = new DateTime(2016, 11, 23),
                version = "2",
                owner = StephaneMOwner,
                productionUnitType = prodUnitTypeFam14,
                hydroponicType = hydroTypeAquaponicsColdFish,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "BronFamily14.JPG"
            };

            var DMAProdUnit = new ProductionUnit()
            {
                locationLatitude = 48.9617278,
                locationLongitude = 8.1760656,
                reference = "768EA",
                info = "Family Experimentation",
                startDate = new DateTime(2016, 11, 29),
                version = "2",
                owner = DarioMOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeAquaponicsColdFish,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "LauterbourgFamily22.JPG"
            };

            var SGPProdUnit = new ProductionUnit()
            {
                locationLatitude = 48.9196549,
                locationLongitude = 2.3526728,
                reference = "1AXDC8",
                info = "Public Exhibition",
                startDate = new DateTime(2016, 12, 01),
                version = "2",
                owner = SGPOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeAquaponicsColdFish,
                productionUnitStatus = prodUnitStatusMaintenance,
                picturePath = "SaintDenisFamily22.JPG"
            };

            var CDEProdUnit = new ProductionUnit()
            {
                locationLatitude = 50.9406272,
                locationLongitude = 3.0499455,
                reference = "1ACDC8",
                info = "ACD Showroom",
                startDate = new DateTime(2017, 01, 30),
                version = "2",
                owner = CristofDOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeNotApplicable,
                productionUnitStatus = prodUnitStatusRunning,
            };

            var DMOProdUnit = new ProductionUnit()
            {
                locationLatitude = 50.6311167,
                locationLongitude = 3.0120553,
                reference = "73331",
                info = "Entrepreunor Experimentation",
                startDate = new DateTime(2017, 01, 20),
                version = "2",
                owner = DonatienMOwner,
                productionUnitType = prodUnitTypeFam14,
                hydroponicType = hydroTypeNotApplicable,
                productionUnitStatus = prodUnitStatusRunning,
                picturePath = "LilleFamily14.JPG",
            };

            //TO BE DEPLOYED 
            var AHEProdUnit = new ProductionUnit()
            {
                locationLatitude = 34.7568479,
                locationLongitude = 10.7129123,
                reference = "76671",
                info = "Commercial Experimentation",
                startDate = new DateTime(2017, 01, 06),
                version = "2",
                owner = AmousHOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeNotApplicable,
                productionUnitStatus = prodUnitStatusReadyForInstall,
            };

            var CELProdUnit = new ProductionUnit()
            {
                locationLatitude = 46.3274736,
                locationLongitude = -0.5313457,
                reference = "76555",
                info = "Open Source Contributor",
                startDate = new DateTime(2017, 01, 30),
                version = "2",
                owner = CyrilleEOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeNotApplicable,
                productionUnitStatus = prodUnitStatusReadyForInstall,
            };

            var BDEProdUnit = new ProductionUnit()
            {
                locationLatitude = 45.1842207,
                locationLongitude = 5.6804372,
                reference = "76671",
                info = "Engineer School Lab",
                startDate = new DateTime(2017, 03, 06),
                version = "2",
                owner = BenoitDOwner,
                productionUnitType = prodUnitTypeCity,
                hydroponicType = hydroTypeNotApplicable,
                productionUnitStatus = prodUnitStatusReadyForInstall,
            };

            var MSEProdUnit = new ProductionUnit()
            {
                locationLatitude = 48.9377862,
                locationLongitude = 2.5135276,
                reference = "72Z71",
                info = "Sevran's City Hall",
                startDate = new DateTime(2017, 03, 06),
                version = "2",
                owner = MairieSOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeNotApplicable,
                productionUnitStatus = prodUnitStatusReadyForInstall,
            };

            var EDCProdUnit = new ProductionUnit()
            {
                locationLatitude = 48.7511227,
                locationLongitude = 2.236909,
                reference = "23AZ1",
                info = "Ayurvedic Center",
                startDate = new DateTime(2017, 02, 20),
                version = "2",
                owner = EleonoreDCOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeNotApplicable,
                productionUnitStatus = prodUnitStatusReadyForInstall,
            };

            //TO BE CONFIRMED
            var MLAProdUnit = new ProductionUnit()
            {
                locationLatitude = 44.9893288,
                locationLongitude = -0.218259,
                reference = "76709",
                info = "Organic Farm Exploitation",
                startDate = new DateTime(2016, 11, 05),
                version = "2",
                owner = MarcLOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeNotApplicable,
                productionUnitStatus = prodUnitStatusWait,
            };

            //var PCLProdUnit = new ProductionUnit()
            //{
            //    locationLatitude = 50.9282888,
            //    locationLongitude = 4.3886957,
            //    reference = "76739",
            //    info = "Living Tomorrow Hub",
            //    startDate = new DateTime(2016, 11, 30),
            //    version = "2",
            //    owner = PieterjanGOwner,
            //    productionUnitType = prodUnitTypeFam22,
            //    hydroponicType = hydroTypeNotApplicable,
            //    productionUnitStatus = prodUnitStatusWait,
            //};

            var BGUProdUnit = new ProductionUnit()
            {
                locationLatitude = 45.2475762,
                locationLongitude = 4.7951584,
                reference = "76AA3",
                info = "Restaurant Experimentation",
                startDate = new DateTime(2016, 12, 01),
                version = "2",
                owner = BrigitteGOwner,
                productionUnitType = prodUnitTypeFam22,
                hydroponicType = hydroTypeNotApplicable,
                productionUnitStatus = prodUnitStatusWait,
            };

            //CONTRIBUTORS
            var APOProdUnit = new ProductionUnit()
            {
                locationLatitude = 49.1652218,
                locationLongitude = 6.1219681,
                reference = "790A3",
                info = "Permaculture Garden",
                startDate = new DateTime(2016, 03, 10),
                version = "1",
                owner = AnhHungPOwner,
                productionUnitType = prodUnitTypeExperimental,
                hydroponicType = hydroTypeNotApplicable,
                productionUnitStatus = prodUnitStatusMaintenance,
            };

            var JTEProdUnit = new ProductionUnit()
            {
                locationLatitude = 49.8910777,
                locationLongitude = 1.69874,
                reference = "79AZ3",
                info = "Indoor Aquaponics",
                startDate = new DateTime(2016, 03, 20),
                version = "1",
                owner = JoelTOwner,
                productionUnitType = prodUnitTypeExperimental,
                hydroponicType = hydroTypeNotApplicable,
                productionUnitStatus = prodUnitStatusMaintenance,
            };

            var NROProdUnit = new ProductionUnit()
            {
                locationLatitude = 50.4593883,
                locationLongitude = 4.7834004,
                reference = "76321",
                info = "Open Source Contributor",
                startDate = new DateTime(2016, 10, 15),
                version = "2",
                owner = NicolasROwner,
                productionUnitType = prodUnitTypeExperimental,
                hydroponicType = hydroTypeNotApplicable,
                productionUnitStatus = prodUnitStatusMaintenance,
            };

            //ADD PRODUCTION UNITS
            //GREENHOUSE OWNERS
            context.ProductionUnits.Add(MGAProdUnit);
            context.ProductionUnits.Add(MURProdUnit);
            context.ProductionUnits.Add(JNAProdUnit);

            context.ProductionUnits.Add(CLAProdUnit);
            context.ProductionUnits.Add(JPMProdUnit);
            context.ProductionUnits.Add(AMAProdUnit);
            context.ProductionUnits.Add(RROProdUnit);

            context.ProductionUnits.Add(SCRProdUnit);
            context.ProductionUnits.Add(MSVProdUnit);
            context.ProductionUnits.Add(DPRProdUnit);
            context.ProductionUnits.Add(CPEProdUnit);
            context.ProductionUnits.Add(PTOProdUnit);
            context.ProductionUnits.Add(SCOProdUnit);

            context.ProductionUnits.Add(SASProdUnit);
            context.ProductionUnits.Add(CWIProdUnit);
            context.ProductionUnits.Add(GDEProdUnit);
            context.ProductionUnits.Add(MWIProdUnit);
            context.ProductionUnits.Add(SMAProdUnit);
            context.ProductionUnits.Add(DMAProdUnit);
            context.ProductionUnits.Add(SGPProdUnit);
            context.ProductionUnits.Add(CDEProdUnit);
            context.ProductionUnits.Add(DMOProdUnit);

            //TO BE DEPLOYED
            context.ProductionUnits.Add(CELProdUnit);
            context.ProductionUnits.Add(AHEProdUnit);
            context.ProductionUnits.Add(BDEProdUnit);
            context.ProductionUnits.Add(MSEProdUnit);
            context.ProductionUnits.Add(EDCProdUnit);

            //TO BE CONFIRMED
            context.ProductionUnits.Add(MLAProdUnit);
            context.ProductionUnits.Add(BGUProdUnit);
            //context.ProductionUnits.Add(PCLProdUnit);

            //CONTRIBUTORS
            context.ProductionUnits.Add(APOProdUnit);
            context.ProductionUnits.Add(JTEProdUnit);
            context.ProductionUnits.Add(NROProdUnit);

            //OPTIONS
            //GREENHOUSES OWNERS
            var optionsMGA = new List<OptionList>();

            optionsMGA.Add(new OptionList() { productionUnit = MGAProdUnit, option = towers18Option });
            optionsMGA.Add(new OptionList() { productionUnit = MGAProdUnit, option = monitoringKitv2Option });
            optionsMGA.Add(new OptionList() { productionUnit = MGAProdUnit, option = advancedMonitoringOption });
            optionsMGA.Add(new OptionList() { productionUnit = MGAProdUnit, option = permacultureBiocharOption });
            optionsMGA.Add(new OptionList() { productionUnit = MGAProdUnit, option = sigfoxConnectionOption });
            optionsMGA.Add(new OptionList() { productionUnit = MGAProdUnit, option = touchlessScreenOption });

            var optionsMUR = new List<OptionList>();

            optionsMUR.Add(new OptionList() { productionUnit = MURProdUnit, option = towers24Option });
            optionsMUR.Add(new OptionList() { productionUnit = MURProdUnit, option = monitoringKitv2Option });
            optionsMUR.Add(new OptionList() { productionUnit = MURProdUnit, option = advancedMonitoringOption });
            optionsMUR.Add(new OptionList() { productionUnit = MURProdUnit, option = pelletStoveOption });
            optionsMUR.Add(new OptionList() { productionUnit = MURProdUnit, option = permacultureBiocharOption });

            var optionsJNA = new List<OptionList>();

            optionsJNA.Add(new OptionList() { productionUnit = JNAProdUnit, option = towers11Option });
            optionsJNA.Add(new OptionList() { productionUnit = JNAProdUnit, option = monitoringKitv2Option });

            var optionsCLA = new List<OptionList>();

            optionsCLA.Add(new OptionList() { productionUnit = CLAProdUnit, option = towers18Option });
            optionsCLA.Add(new OptionList() { productionUnit = CLAProdUnit, option = monitoringKitv1Option });
            optionsCLA.Add(new OptionList() { productionUnit = CLAProdUnit, option = permacultureBedOption });

            var optionsJPM = new List<OptionList>();

            optionsJPM.Add(new OptionList() { productionUnit = JPMProdUnit, option = towers24Option });
            optionsJPM.Add(new OptionList() { productionUnit = JPMProdUnit, option = monitoringKitv1Option });
            optionsJPM.Add(new OptionList() { productionUnit = JPMProdUnit, option = permacultureBedOption });
            optionsJPM.Add(new OptionList() { productionUnit = JPMProdUnit, option = pelletStoveOption });
            optionsJPM.Add(new OptionList() { productionUnit = JPMProdUnit, option = solarPanelOption });

            var optionsAMA = new List<OptionList>();

            optionsAMA.Add(new OptionList() { productionUnit = AMAProdUnit, option = towers18Option });
            optionsAMA.Add(new OptionList() { productionUnit = AMAProdUnit, option = monitoringKitv1Option });

            var optionsRRO = new List<OptionList>();

            optionsRRO.Add(new OptionList() { productionUnit = RROProdUnit, option = towers18Option });
            optionsRRO.Add(new OptionList() { productionUnit = RROProdUnit, option = monitoringKitv1Option });
            optionsRRO.Add(new OptionList() { productionUnit = RROProdUnit, option = permacultureBedOption });

            var optionsSCR = new List<OptionList>();

            optionsSCR.Add(new OptionList() { productionUnit = SCRProdUnit, option = towers18Option });
            optionsSCR.Add(new OptionList() { productionUnit = SCRProdUnit, option = monitoringKitv1Option });
            optionsSCR.Add(new OptionList() { productionUnit = SCRProdUnit, option = permacultureBedOption });

            var optionsMSV = new List<OptionList>();

            optionsMSV.Add(new OptionList() { productionUnit = MSVProdUnit, option = towers18Option });
            optionsMSV.Add(new OptionList() { productionUnit = MSVProdUnit, option = monitoringKitv1Option });

            var optionsDPR = new List<OptionList>();

            optionsDPR.Add(new OptionList() { productionUnit = DPRProdUnit, option = towers11Option });
            optionsDPR.Add(new OptionList() { productionUnit = DPRProdUnit, option = monitoringKitv1Option });
            optionsDPR.Add(new OptionList() { productionUnit = DPRProdUnit, option = permacultureBedOption });

            var optionsCPE = new List<OptionList>();

            optionsCPE.Add(new OptionList() { productionUnit = CPEProdUnit, option = towers24Option });
            optionsCPE.Add(new OptionList() { productionUnit = CPEProdUnit, option = monitoringKitv1Option });
            optionsCPE.Add(new OptionList() { productionUnit = CPEProdUnit, option = permacultureBedOption });

            var optionsPTO = new List<OptionList>();

            optionsPTO.Add(new OptionList() { productionUnit = PTOProdUnit, option = towers18Option });
            optionsPTO.Add(new OptionList() { productionUnit = PTOProdUnit, option = monitoringKitv1Option });
            optionsPTO.Add(new OptionList() { productionUnit = PTOProdUnit, option = permacultureBedOption });

            var optionsSCO = new List<OptionList>();

            optionsSCO.Add(new OptionList() { productionUnit = SCOProdUnit, option = towers18Option });
            optionsSCO.Add(new OptionList() { productionUnit = SCOProdUnit, option = monitoringKitv1Option });
            optionsSCO.Add(new OptionList() { productionUnit = SCOProdUnit, option = permacultureBedOption });

            var optionsSAS = new List<OptionList>();

            optionsSAS.Add(new OptionList() { productionUnit = SASProdUnit, option = towers18Option });
            optionsSAS.Add(new OptionList() { productionUnit = SASProdUnit, option = monitoringKitv1Option });
            optionsSAS.Add(new OptionList() { productionUnit = SASProdUnit, option = permacultureBiocharOption });

            var optionsCWI = new List<OptionList>();

            optionsCWI.Add(new OptionList() { productionUnit = CWIProdUnit, option = towers18Option });
            optionsCWI.Add(new OptionList() { productionUnit = CWIProdUnit, option = monitoringKitv2Option });

            var optionsGDE = new List<OptionList>();

            optionsGDE.Add(new OptionList() { productionUnit = GDEProdUnit, option = monitoringKitv2Option });
            optionsGDE.Add(new OptionList() { productionUnit = GDEProdUnit, option = towers18Option });
            optionsGDE.Add(new OptionList() { productionUnit = GDEProdUnit, option = permacultureBedOption });

            var optionsMWI = new List<OptionList>();

            optionsMWI.Add(new OptionList() { productionUnit = MWIProdUnit, option = monitoringKitv2Option });
            optionsMWI.Add(new OptionList() { productionUnit = MWIProdUnit, option = towers11Option });

            var optionsSMA = new List<OptionList>();

            optionsSMA.Add(new OptionList() { productionUnit = SMAProdUnit, option = monitoringKitv2Option });
            optionsSMA.Add(new OptionList() { productionUnit = SMAProdUnit, option = towers18Option });
            optionsSMA.Add(new OptionList() { productionUnit = SMAProdUnit, option = permacultureBedOption });

            var optionsDMA = new List<OptionList>();

            optionsDMA.Add(new OptionList() { productionUnit = DMAProdUnit, option = monitoringKitv2Option });
            optionsDMA.Add(new OptionList() { productionUnit = DMAProdUnit, option = towers18Option });
            optionsDMA.Add(new OptionList() { productionUnit = DMAProdUnit, option = permacultureBedOption });
            optionsDMA.Add(new OptionList() { productionUnit = DMAProdUnit, option = pelletStoveOption });

            var optionsSGP = new List<OptionList>();

            optionsSGP.Add(new OptionList() { productionUnit = SGPProdUnit, option = monitoringKitv2Option });
            optionsSGP.Add(new OptionList() { productionUnit = SGPProdUnit, option = towers18Option });
            optionsSGP.Add(new OptionList() { productionUnit = SGPProdUnit, option = permacultureBedOption });
            optionsSGP.Add(new OptionList() { productionUnit = SGPProdUnit, option = solarPanelOption });

            //TO BE DEPLOYED
            var optionsCDE = new List<OptionList>();

            optionsCDE.Add(new OptionList() { productionUnit = CDEProdUnit, option = monitoringKitv2Option });
            optionsCDE.Add(new OptionList() { productionUnit = CDEProdUnit, option = towers18Option });
            optionsCDE.Add(new OptionList() { productionUnit = CDEProdUnit, option = permacultureBedOption });
            optionsCDE.Add(new OptionList() { productionUnit = CDEProdUnit, option = solarPanelOption });
            optionsCDE.Add(new OptionList() { productionUnit = CDEProdUnit, option = sigfoxConnectionOption });
            optionsCDE.Add(new OptionList() { productionUnit = CDEProdUnit, option = touchlessScreenOption });

            var optionsAHE = new List<OptionList>();

            optionsAHE.Add(new OptionList() { productionUnit = AHEProdUnit, option = monitoringKitv2Option });
            optionsAHE.Add(new OptionList() { productionUnit = AHEProdUnit, option = towers24Option });
            optionsAHE.Add(new OptionList() { productionUnit = AHEProdUnit, option = permacultureBedOption });

            var optionsBDE = new List<OptionList>();

            optionsBDE.Add(new OptionList() { productionUnit = BDEProdUnit, option = monitoringKitv2Option });
            optionsBDE.Add(new OptionList() { productionUnit = BDEProdUnit, option = towers11Option });
            optionsBDE.Add(new OptionList() { productionUnit = BDEProdUnit, option = sigfoxConnectionOption });
            optionsBDE.Add(new OptionList() { productionUnit = BDEProdUnit, option = solarPanelOption });

            var optionsDMO = new List<OptionList>();

            optionsDMO.Add(new OptionList() { productionUnit = DMOProdUnit, option = monitoringKitv2Option });
            optionsDMO.Add(new OptionList() { productionUnit = DMOProdUnit, option = towers11Option });

            var optionsMSE = new List<OptionList>();

            optionsMSE.Add(new OptionList() { productionUnit = MSEProdUnit, option = monitoringKitv2Option });
            optionsMSE.Add(new OptionList() { productionUnit = MSEProdUnit, option = towers18Option });

            var optionsEDC = new List<OptionList>();

            optionsEDC.Add(new OptionList() { productionUnit = EDCProdUnit, option = monitoringKitv2Option });
            optionsEDC.Add(new OptionList() { productionUnit = EDCProdUnit, option = towers18Option });
            optionsEDC.Add(new OptionList() { productionUnit = EDCProdUnit, option = permacultureBedOption });

            //TO BE CONFIRMED
            var optionsMLA = new List<OptionList>();

            optionsMLA.Add(new OptionList() { productionUnit = MLAProdUnit, option = monitoringKitv2Option });
            optionsMLA.Add(new OptionList() { productionUnit = MLAProdUnit, option = towers18Option });
            optionsMLA.Add(new OptionList() { productionUnit = MLAProdUnit, option = permacultureBedOption });

            var optionsBGU = new List<OptionList>();

            optionsBGU.Add(new OptionList() { productionUnit = BGUProdUnit, option = monitoringKitv2Option });
            optionsBGU.Add(new OptionList() { productionUnit = BGUProdUnit, option = towers18Option });
            optionsBGU.Add(new OptionList() { productionUnit = BGUProdUnit, option = permacultureBedOption });

            //CONTRIBUTORS
            var optionsCEL = new List<OptionList>();

            optionsCEL.Add(new OptionList() { productionUnit = CELProdUnit, option = monitoringKitv2Option });
            optionsCEL.Add(new OptionList() { productionUnit = CELProdUnit, option = touchlessScreenOption });

            var optionsNRO = new List<OptionList>();

            optionsNRO.Add(new OptionList() { productionUnit = NROProdUnit, option = monitoringKitv2Option });

            var optionsJTE = new List<OptionList>();

            optionsJTE.Add(new OptionList() { productionUnit = JTEProdUnit, option = towers11Option });

            var optionsAPO = new List<OptionList>();

            optionsAPO.Add(new OptionList() { productionUnit = APOProdUnit, option = permacultureBiocharOption });

            //ADD OPTIONS
            //GREENHOUSE OWNERS
            context.OptionLists.AddRange(optionsMGA);
            context.OptionLists.AddRange(optionsMUR);
            context.OptionLists.AddRange(optionsJNA);

            context.OptionLists.AddRange(optionsCLA);
            context.OptionLists.AddRange(optionsJPM);
            context.OptionLists.AddRange(optionsAMA);
            context.OptionLists.AddRange(optionsRRO);
            context.OptionLists.AddRange(optionsSCR);
            context.OptionLists.AddRange(optionsMSV);
            context.OptionLists.AddRange(optionsDPR);
            context.OptionLists.AddRange(optionsCPE);
            context.OptionLists.AddRange(optionsPTO);
            context.OptionLists.AddRange(optionsSCO);
            context.OptionLists.AddRange(optionsSAS);
            context.OptionLists.AddRange(optionsCWI);
            context.OptionLists.AddRange(optionsGDE);
            context.OptionLists.AddRange(optionsMWI);
            context.OptionLists.AddRange(optionsSMA);
            context.OptionLists.AddRange(optionsDMA);
            context.OptionLists.AddRange(optionsSGP);
            context.OptionLists.AddRange(optionsCDE);
            context.OptionLists.AddRange(optionsDMO);

            //TO BE DEPLOYED
            context.OptionLists.AddRange(optionsAHE);
            context.OptionLists.AddRange(optionsBDE);
            context.OptionLists.AddRange(optionsMSE);
            context.OptionLists.AddRange(optionsEDC);

            //TO BE CONFIRMED
            context.OptionLists.AddRange(optionsMLA);
            context.OptionLists.AddRange(optionsBGU);            

            //CONTRIBUTORS
            context.OptionLists.AddRange(optionsAPO);
            context.OptionLists.AddRange(optionsJTE);
            context.OptionLists.AddRange(optionsNRO);
            context.OptionLists.AddRange(optionsCEL);

            context.SaveChanges();

            var messMeasure = context.MessageTypes.Where(m => m.Id == 1).FirstOrDefault();

            if (!context.Measures.Any())
            {
                var phSensor = context.SensorTypes.Where(s => s.Id == 1).FirstOrDefault();
                var waterTemperatureSensor = context.SensorTypes.Where(s => s.Id == 2).FirstOrDefault();
                var dissolvedOxySensor = context.SensorTypes.Where(s => s.Id == 3).FirstOrDefault();
                var ORPSensor = context.SensorTypes.Where(s => s.Id == 4).FirstOrDefault();
                var airTemperatureSensor = context.SensorTypes.Where(s => s.Id == 5).FirstOrDefault();
                var airHumidity = context.SensorTypes.Where(s => s.Id == 6).FirstOrDefault();

                var productionUnitList = context.ProductionUnits;

                //foreach (ProductionUnit productionUnit in productionUnitList)
                //{
                //    for (int i = 0; i < 6 * 2; i++)
                //    {
                //        Random rnd = new Random();
                //        var currentDate = DateTime.Now;
                //        currentDate = currentDate.AddTicks(-(currentDate.Ticks % TimeSpan.TicksPerSecond)).AddMinutes(-10 * i);

                //        context.Messages.Add(new Message() { date = currentDate, content = "007002190082248902680400", device = productionUnit.reference, messageType = messMeasure });
                //        context.Messages.Add(new Message() { date = currentDate, content = "006802340082248902680400", device = productionUnit.reference, messageType = messMeasure });
                //        context.Messages.Add(new Message() { date = currentDate, content = "006702540082248902680400", device = productionUnit.reference, messageType = messMeasure });

                //        decimal phValue = Convert.ToDecimal(Math.Round(7 + Math.Sin(0.5 * i) + 0.1 * rnd.Next(-1, 1), 3));
                //        context.Measures.Add(new Measure() { captureDate = currentDate, value = phValue, sensor = phSensor, productionUnit = productionUnit });

                //        decimal waterTemperatureValue = Convert.ToDecimal(Math.Round(15 + Math.Sin(0.1 * i) + 0.5 * rnd.Next(-1, 1), 3));
                //        context.Measures.Add(new Measure() { captureDate = currentDate, value = waterTemperatureValue, sensor = waterTemperatureSensor, productionUnit = productionUnit });

                //        decimal dissolvedOxyValue = Convert.ToDecimal(Math.Round(250 + Math.Sin(0.01 * i) + 0.5 * rnd.Next(-1, 1), 3));
                //        context.Measures.Add(new Measure() { captureDate = currentDate, value = dissolvedOxyValue, sensor = dissolvedOxySensor, productionUnit = productionUnit });

                //        decimal ORPValue = Convert.ToDecimal(Math.Round(500 + Math.Sin(0.01 * i) + 0.7 * rnd.Next(-1, 1), 3));
                //        context.Measures.Add(new Measure() { captureDate = currentDate, value = ORPValue, sensor = ORPSensor, productionUnit = productionUnit });

                //        decimal airTemperatureValue = Convert.ToDecimal(Math.Round(20 + Math.Sin(0.001 * i) + 0.5 * rnd.Next(-1, 1), 3));
                //        context.Measures.Add(new Measure() { captureDate = currentDate, value = airTemperatureValue, sensor = airTemperatureSensor, productionUnit = productionUnit });

                //        decimal humidityValue = Convert.ToDecimal(Math.Round(50 + Math.Sin(0.001 * i) + 0.5 * rnd.Next(-1, 1), 3));
                //        context.Measures.Add(new Measure() { captureDate = currentDate, value = humidityValue, sensor = airHumidity, productionUnit = productionUnit });
                //    };

                //}
                context.SaveChanges();
            }
        }

    }
}
