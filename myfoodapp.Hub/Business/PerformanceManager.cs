using myfoodapp.Hub.Models;
using myfoodapp.Hub.Models.OpenData;
using myfoodapp.Hub.Services;
using System;
using System.Data.Entity;
using System.Linq;

namespace myfoodapp.Hub.Business
{
    public static class PerformanceManager
    {
        private static int monthlyAverageProductionAerospring = 4;
        private static int monthlyAverageProductionCity = 7;
        private static int monthlyAverageProductionFamily14 = 10;
        private static int monthlyAverageProductionFamily22 = 15;
        private static int monthlyAverageProductionFarm = 25;

        //CARLSSON-KANYAMA, Annika, et al. "Potential contributions of food consumption patterns to climate change."
        // The American journal of clinical nutrition. 2009, 1704S-1709S
        private static double CO2SparedPerKilogramLocallyProduced = 1.1;

        public static OpenProductionUnitsStatsViewModel GetNetworkStatistic(ApplicationDbContext db)
        {
            MeasureService measureService = new MeasureService(db);

            var rslt = db.ProductionUnits.Include(p => p.productionUnitStatus)
                                         .Where(p => p.productionUnitType.Id <= 5);

            var productionUnitNumber = rslt.Count();

            var totalBalcony = rslt.Where(p => p.productionUnitType.Id == 1).Count();
            var totalCity = rslt.Where(p => p.productionUnitType.Id == 2).Count();
            var totalFamily14 = rslt.Where(p => p.productionUnitType.Id == 3).Count();
            var totalFamily22 = rslt.Where(p => p.productionUnitType.Id == 4).Count();
            var totalFarm = rslt.Where(p => p.productionUnitType.Id == 5).Count();

            var totalMonthlyProduction = totalBalcony * monthlyAverageProductionAerospring 
                                           + totalCity * monthlyAverageProductionCity 
                                           + totalFamily14 * monthlyAverageProductionFamily14 
                                           + totalFamily22 * monthlyAverageProductionFamily22 
                                           + totalFarm * monthlyAverageProductionFarm;

            var totalMonthlySparedCO2 = Math.Round(totalMonthlyProduction * CO2SparedPerKilogramLocallyProduced);

            return new OpenProductionUnitsStatsViewModel() { productionUnitNumber = productionUnitNumber,
                                                             totalMonthlyProduction = totalMonthlyProduction,
                                                             totalMonthlySparedCO2 = totalMonthlySparedCO2
                                                           }; 
        }

        public static int GetEstimatedMonthlyProduction(int productionUnitTypeId)
        {
            var averageMonthlyProduction = 0;

            switch (productionUnitTypeId)
            {
                case 1:
                    //AeroSpring
                    averageMonthlyProduction = monthlyAverageProductionAerospring;
                    break;
                case 2:
                    //City
                    averageMonthlyProduction = monthlyAverageProductionCity;
                    break;
                case 3:
                    //Family14
                    averageMonthlyProduction = monthlyAverageProductionFamily14;
                    break;
                case 4:
                    //Family22
                    averageMonthlyProduction = monthlyAverageProductionFamily22;
                    break;
                case 5:
                    //Farm
                    averageMonthlyProduction = monthlyAverageProductionFarm;
                    break;
                default:
                    break;
            }

            return averageMonthlyProduction;
        }

        public static double GetEstimatedMonthlySparedCO2(int averageMonthlyProduction)
        {
            return averageMonthlyProduction * CO2SparedPerKilogramLocallyProduced;
        }
    }
}