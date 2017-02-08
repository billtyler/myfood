using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using myfoodapp.Hub.Business;
using myfoodapp.Hub.Models;
using myfoodapp.Hub.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace myfoodapp.Hub.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            var db = new ApplicationDbContext();
            var measureService = new MeasureService(db);

            var listMarker = new List<Marker>();

            db.ProductionUnits.ToList().ForEach(p => 
                                        listMarker.Add(new Marker(p.locationLatitude, p.locationLongitude, p.info) { shape = "redMarker" }));

            var map = new Models.Map()
            {
                Name = "map",
                CenterLatitude = 44.0235561,
                CenterLongitude = -10.3640063,
                Zoom = 4,
                TileUrlTemplate = "http://#= subdomain #.tile.openstreetmap.org/#= zoom #/#= x #/#= y #.png",
                TileSubdomains = new string[] { "a", "b", "c" },
                TileAttribution = "&copy; <a href='http://osm.org/copyright'>OpenStreetMap contributors</a>",
                Markers = listMarker
            };

            return View(map);
        }

        public ActionResult GetProductionUnitDetail(double SelectedProductionUnitLat, double SelectedProductionUnitLong)
        {
            var db = new ApplicationDbContext();

            var currentProductionUnit = db.ProductionUnits.Include("owner.user")
                                         .Include("productionUnitType")
                                         .Where(p => p.locationLatitude == SelectedProductionUnitLat &&
                                                     p.locationLongitude == SelectedProductionUnitLong).FirstOrDefault();

            var pHSensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.ph, db);
            var waterTempSensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.waterTemperature, db);
            var airTempSensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.airTemperature, db);
            var humiditySensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.humidity, db);

            return Json(new {
                              PioneerCitizenName = currentProductionUnit.owner.pioneerCitizenName,
                              PioneerCitizenNumber = currentProductionUnit.owner.pioneerCitizenNumber,
                              ProductionUnitVersion = currentProductionUnit.version,
                              ProductionUnitStartDate = currentProductionUnit.startDate,
                              ProductionUnitType = currentProductionUnit.productionUnitType.name,
                              PicturePath = currentProductionUnit.picturePath,

                              CurrentPhValue = pHSensorValueSet.CurrentValue,
                              CurrentPhCaptureTime = pHSensorValueSet.CurrentCaptureTime,
                              AverageHourPhValue = pHSensorValueSet.AverageHourValue,
                              AverageDayPhValue = pHSensorValueSet.AverageDayValue,
                              LastDayPhCaptureTime = pHSensorValueSet.LastDayCaptureTime,

                              CurrentWaterTempValue = waterTempSensorValueSet.CurrentValue,
                              CurrentWaterTempCaptureTime = waterTempSensorValueSet.CurrentCaptureTime,
                              AverageHourWaterTempValue = waterTempSensorValueSet.AverageHourValue,
                              AverageDayWaterTempValue = waterTempSensorValueSet.AverageDayValue,
                              LastDayWaterTempCaptureTime = waterTempSensorValueSet.LastDayCaptureTime,

                              CurrentAirTempValue = airTempSensorValueSet.CurrentValue,
                              CurrentAirTempCaptureTime = airTempSensorValueSet.CurrentCaptureTime,
                              AverageHourAirTempValue = airTempSensorValueSet.AverageHourValue,
                              AverageDayAirTempValue = airTempSensorValueSet.AverageDayValue,
                              LastDayAirTempCaptureTime = airTempSensorValueSet.LastDayCaptureTime,

                              CurrentHumidityValue = humiditySensorValueSet.CurrentValue,
                              CurrentHumidityCaptureTime = humiditySensorValueSet.CurrentCaptureTime,
                              AverageHourHumidityValue = humiditySensorValueSet.AverageHourValue,
                              AverageDayHumidityValue = humiditySensorValueSet.AverageDayValue,
                              LastDayHumidityCaptureTime = humiditySensorValueSet.LastDayCaptureTime,

                            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRandomProductionUnitDetail()
        {
            var db = new ApplicationDbContext();

            var prodUnitListCount = db.ProductionUnits.Where(p => p.measures.Count != 0).Count();

            if (prodUnitListCount == 0)
                return null;

            var random = new Random();
            int randomIndex = random.Next(0, prodUnitListCount);

            var currentProductionUnit = db.ProductionUnits.Include("owner.user")
                                         .Include("productionUnitType")
                                         .Where(p => p.measures.Count != 0).ToList()[randomIndex];

            var pHSensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.ph, db);
            var waterTempSensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.waterTemperature, db);
            var airTempSensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.airTemperature, db);
            var humiditySensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.humidity, db);

            return Json(new
            {
                PioneerCitizenName = currentProductionUnit.owner.pioneerCitizenName,
                PioneerCitizenNumber = currentProductionUnit.owner.pioneerCitizenNumber,
                ProductionUnitVersion = currentProductionUnit.version,
                ProductionUnitStartDate = currentProductionUnit.startDate,
                ProductionUnitType = currentProductionUnit.productionUnitType.name,
                PicturePath = currentProductionUnit.picturePath,

                LocationLatitude = currentProductionUnit.locationLatitude,
                LocationLongitude = currentProductionUnit.locationLongitude,

                CurrentPhValue = pHSensorValueSet.CurrentValue,
                CurrentPhCaptureTime = pHSensorValueSet.CurrentCaptureTime,
                AverageHourPhValue = pHSensorValueSet.AverageHourValue,
                AverageDayPhValue = pHSensorValueSet.AverageDayValue,
                LastDayPhCaptureTime = pHSensorValueSet.LastDayCaptureTime,

                CurrentWaterTempValue = waterTempSensorValueSet.CurrentValue,
                CurrentWaterTempCaptureTime = waterTempSensorValueSet.CurrentCaptureTime,
                AverageHourWaterTempValue = waterTempSensorValueSet.AverageHourValue,
                AverageDayWaterTempValue = waterTempSensorValueSet.AverageDayValue,
                LastDayWaterTempCaptureTime = waterTempSensorValueSet.LastDayCaptureTime,

                CurrentAirTempValue = airTempSensorValueSet.CurrentValue,
                CurrentAirTempCaptureTime = airTempSensorValueSet.CurrentCaptureTime,
                AverageHourAirTempValue = airTempSensorValueSet.AverageHourValue,
                AverageDayAirTempValue = airTempSensorValueSet.AverageDayValue,
                LastDayAirTempCaptureTime = airTempSensorValueSet.LastDayCaptureTime,

                CurrentHumidityValue = humiditySensorValueSet.CurrentValue,
                CurrentHumidityCaptureTime = humiditySensorValueSet.CurrentCaptureTime,
                AverageHourHumidityValue = humiditySensorValueSet.AverageHourValue,
                AverageDayHumidityValue = humiditySensorValueSet.AverageDayValue,
                LastDayHumidityCaptureTime = humiditySensorValueSet.LastDayCaptureTime,

            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Option_Read([DataSourceRequest] DataSourceRequest request, double SelectedProductionUnitLat, double SelectedProductionUnitLong)
        {
            var db = new ApplicationDbContext();

            var rslt = db.OptionLists.Include("productionUnit")
                                    .Include("option")
                                    .Where(p => p.productionUnit.locationLatitude == SelectedProductionUnitLat &&
                                                p.productionUnit.locationLongitude == SelectedProductionUnitLong)
                                    .Select(p => p.option);

            return Json(rslt.ToDataSourceResult(request));
        }

        public ActionResult ProductionUnitStatus_Read([DataSourceRequest] DataSourceRequest request)
        {
            var db = new ApplicationDbContext();

            var rslt = db.ProductionUnits.Include("productionUnitStatus").ToList();

            var waitConfCount = rslt.Where(p => p.productionUnitStatus.Id == 1).Count();
            var setuPlannedCount = rslt.Where(p => p.productionUnitStatus.Id == 2).Count();
            var upRunningCount = rslt.Where(p => p.productionUnitStatus.Id == 3).Count();
            var onMaintenanceCount = rslt.Where(p => p.productionUnitStatus.Id == 4).Count();
            var stoppedCount  = rslt.Where(p => p.productionUnitStatus.Id == 5).Count();

            var statusList = new List<PieChartViewModel>();

            statusList.Add(new PieChartViewModel() { Category = "Wait Confirm.", Value = waitConfCount, Color = "#9de219" });
            statusList.Add(new PieChartViewModel() { Category = "Setup Planned", Value = setuPlannedCount, Color = "#90cc38" });
            statusList.Add(new PieChartViewModel() { Category = "Up & Running", Value = upRunningCount, Color = "#068c35" });
            statusList.Add(new PieChartViewModel() { Category = "On Maintenance", Value = onMaintenanceCount, Color = "#006634" });
            statusList.Add(new PieChartViewModel() { Category = "Stopped", Value = stoppedCount, Color = "#004d38" });

            return Json(statusList);
        }

        public ActionResult GetNetworkStats()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            MeasureService measureService = new MeasureService(db);

            var rslt = db.ProductionUnits.Include("productionUnitType")
                                         .Where(p => p.productionUnitType.Id <= 5);

            var productionUnitNumber = rslt.Count();

            var totalBalcony = rslt.Where(p => p.productionUnitType.Id == 1).Count();
            var totalCity = rslt.Where(p => p.productionUnitType.Id == 2).Count();
            var totalFamily14 = rslt.Where(p => p.productionUnitType.Id == 3).Count();
            var totalFamily22 = rslt.Where(p => p.productionUnitType.Id == 4).Count();
            var totalFarm = rslt.Where(p => p.productionUnitType.Id == 5).Count();

            var totalMonthlyProduction = totalBalcony * 5 + totalCity * 10 + totalFamily14 * 15 + totalFamily22 * 25 + totalFarm * 50;
            var totalMonthlySparedCO2 = totalMonthlyProduction * 0.3;

            return Json(new
            {
                ProductionUnitNumber = productionUnitNumber,
                TotalMonthlyProduction = totalMonthlyProduction,
                TotalMonthlySparedCO2 = totalMonthlySparedCO2,
            }, JsonRequestBehavior.AllowGet);
        }

       
    }
}
