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

            var pHSensorValueSet = GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.ph, db);
            var waterTempSensorValueSet = GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.waterTemperature, db);
            var airTempSensorValueSet = GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.airTemperature, db);
            var humiditySensorValueSet = GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.humidity, db);

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

            var prodUnitList = db.ProductionUnits.Where(p => p.measures.Count != 0).Count();
            var random = new Random();
            int randomIndex = random.Next(0, prodUnitList);

            var currentProductionUnit = db.ProductionUnits.Include("owner.user")
                                         .Include("productionUnitType")
                                         .Where(p => p.measures.Count != 0).ToList()[randomIndex];

            var pHSensorValueSet = GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.ph, db);
            var waterTempSensorValueSet = GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.waterTemperature, db);
            var airTempSensorValueSet = GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.airTemperature, db);
            var humiditySensorValueSet = GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.humidity, db);

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

        public SensorValueSet GetSensorValueSet(int currentProductionUnitId, SensorTypeEnum sensor, ApplicationDbContext db)
        {
            var currentValue = 0M;
            var averageHourValue = 0M;
            var averageDayValue = 0M;

            var currentCaptureTime = String.Empty;
            var lastDayCaptureTime = String.Empty;

            var lastValue = db.Measures.Where(m => m.productionUnit.Id == currentProductionUnitId
                                               && m.sensor.Id == (int)sensor)
                                         .OrderByDescending(m => m.captureDate).FirstOrDefault();
            if (lastValue != null)
            {
                currentValue = lastValue.value;
                currentCaptureTime = lastValue.captureDate.ToShortTimeString();

                var lastHour = lastValue.captureDate.AddHours(-1);

                var averageHourValueRslt = db.Measures.Where(m => m.productionUnit.Id == currentProductionUnitId
                                                      && m.sensor.Id == (int)sensor
                                                      && m.captureDate >= lastHour)
                                                     .OrderByDescending(m => m.captureDate);

                averageHourValue = Math.Round(averageHourValueRslt.Average(m => m.value), 1);

                var lastDay = lastValue.captureDate.AddDays(-1);
                lastDayCaptureTime = lastDay.ToShortDateString();

                var averageDayValueRslt = db.Measures.Where(m => m.productionUnit.Id == currentProductionUnitId
                                                      && m.sensor.Id == (int)sensor
                                                      && m.captureDate >= lastDay)
                                                     .OrderByDescending(m => m.captureDate);

                averageDayValue = Math.Round(averageDayValueRslt.Average(m => m.value), 1);

                return new SensorValueSet()
                {
                    CurrentValue = currentValue,
                    CurrentCaptureTime = currentCaptureTime,
                    AverageHourValue = averageHourValue,
                    AverageDayValue = averageDayValue,
                    LastDayCaptureTime = lastDayCaptureTime
                };
            }

            return new SensorValueSet()
            {
                CurrentValue = 0,
                CurrentCaptureTime = "-",
                AverageHourValue = 0,
                AverageDayValue = 0,
                LastDayCaptureTime = "-"
            };
        }
    }
}
