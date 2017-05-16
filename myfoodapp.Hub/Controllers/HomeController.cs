using i18n;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using myfoodapp.Hub.Business;
using myfoodapp.Hub.Models;
using myfoodapp.Hub.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace myfoodapp.Hub.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = new ApplicationDbContext();
            //var currentUser = this.User.Identity.GetUserName();

            //if(currentUser != string.Empty)
            //{
            //    var currentProductionUnitOwner = db.ProductionUnitOwners.Include(p => p.language)
            //                                      .Where(p => p.user.UserName == currentUser).FirstOrDefault();

            //    System.Web.HttpContext.Current.Session["UserLang"] = currentProductionUnitOwner.language.description;
            //}

            var listMarker = new List<Marker>();

            db.ProductionUnits.ToList().ForEach(p =>
                                        listMarker.Add(new Marker(p.locationLatitude, p.locationLongitude, String.Format("{0} </br> start since {1}",
                                                                  p.info, p.startDate.ToShortDateString()))
                                        { shape = "redMarker" }));

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

        public ActionResult GetProductionUnitMeasures(int id)
        {
            var db = new ApplicationDbContext();

            var currentProductionUnit = db.ProductionUnits.Where(p => p.picturePath != null).ToList()[id];

            var pHSensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.ph, db);
            var waterTempSensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.waterTemperature, db);
            var airTempSensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.airTemperature, db);
            var humiditySensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.humidity, db);

            return Json(new
            {
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

        public ActionResult GetProductionUnitIndex(double SelectedProductionUnitLat, double SelectedProductionUnitLong)
        {
            var db = new ApplicationDbContext();

            var currentProductionUnit = db.ProductionUnits.Where(p => p.picturePath != null).ToList();

            var currentProductionUnitIndex = currentProductionUnit.FindIndex(p => p.locationLatitude == SelectedProductionUnitLat &&
                                                                             p.locationLongitude == SelectedProductionUnitLong);

            return Json(new
            {
                CurrentIndex = currentProductionUnitIndex
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProductionUnitDetailList()
        {
            var db = new ApplicationDbContext();

            var prodUnitListCount = db.ProductionUnits.Where(p => p.picturePath != null).Count();

            if (prodUnitListCount == 0)
                return null;

            var currentProductionUnitList = db.ProductionUnits.Include(p => p.owner)
                                         .Include(p => p.productionUnitType)
                                         .Include(p => p.productionUnitStatus)
                                         .Where(p => p.picturePath != null).ToList();

            var lst = new List<object>();

            currentProductionUnitList.ForEach(p =>
            {
                var options = db.OptionLists.Include(o => o.productionUnit)
                .Include(o => o.option)
                .Where(o => o.productionUnit.Id == p.Id)
                .Select(o => o.option);

                var optionList = string.Empty;

                if (options.Count() > 0)
                {
                    options.ToList().ForEach(o => { optionList += o.name + " / "; });
                }

                lst.Add(new
                {

                    PioneerCitizenName = p.owner.pioneerCitizenName,
                    PioneerCitizenNumber = p.owner.pioneerCitizenNumber,
                    ProductionUnitVersion = p.version,
                    ProductionUnitStartDate = p.startDate,
                    ProductionUnitType = p.productionUnitType.name,
                    ProductionUnitStatus = p.productionUnitStatus.name,
                    PicturePath = p.picturePath,

                    LocationLatitude = p.locationLatitude,
                    LocationLongitude = p.locationLongitude,

                    ProductionUnitOptions = optionList,
                }
                );
            });

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductionUnitStatus_Read([DataSourceRequest] DataSourceRequest request)
        {
            var db = new ApplicationDbContext();

            var rslt = db.ProductionUnits.Include(p => p.productionUnitStatus).ToList();

            var waitConfCount = rslt.Where(p => p.productionUnitStatus.Id == 1).Count();
            var setuPlannedCount = rslt.Where(p => p.productionUnitStatus.Id == 2).Count();
            var upRunningCount = rslt.Where(p => p.productionUnitStatus.Id == 3).Count();
            var onMaintenanceCount = rslt.Where(p => p.productionUnitStatus.Id == 4).Count();
            var stoppedCount  = rslt.Where(p => p.productionUnitStatus.Id == 5).Count();

            var statusList = new List<PieChartViewModel>();

            statusList.Add(new PieChartViewModel() { Category = "[[[Wait Confirm.]]]", Value = waitConfCount, Color = "#9de219" });
            statusList.Add(new PieChartViewModel() { Category = "[[[Setup Planned]]]", Value = setuPlannedCount, Color = "#90cc38" });
            statusList.Add(new PieChartViewModel() { Category = "[[[Up and Running]]]", Value = upRunningCount, Color = "#068c35" });
            statusList.Add(new PieChartViewModel() { Category = "[[[On Maintenance]]]", Value = onMaintenanceCount, Color = "#006634" });
            statusList.Add(new PieChartViewModel() { Category = "[[[Stopped]]]", Value = stoppedCount, Color = "#004d38" });

            return Json(statusList);
        }

        public ActionResult GetNetworkStats()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            MeasureService measureService = new MeasureService(db);

            var rslt = db.ProductionUnits.Include(p => p.productionUnitStatus)
                                         .Where(p => p.productionUnitType.Id <= 5);

            var productionUnitNumber = rslt.Count();

            var totalBalcony = rslt.Where(p => p.productionUnitType.Id == 1).Count();
            var totalCity = rslt.Where(p => p.productionUnitType.Id == 2).Count();
            var totalFamily14 = rslt.Where(p => p.productionUnitType.Id == 3).Count();
            var totalFamily22 = rslt.Where(p => p.productionUnitType.Id == 4).Count();
            var totalFarm = rslt.Where(p => p.productionUnitType.Id == 5).Count();

            var totalMonthlyProduction = totalBalcony * 5 + totalCity * 10 + totalFamily14 * 15 + totalFamily22 * 25 + totalFarm * 50;
            var totalMonthlySparedCO2 = Math.Round(totalMonthlyProduction * 0.3);

            return Json(new
            {
                ProductionUnitNumber = productionUnitNumber,
                TotalMonthlyProduction = totalMonthlyProduction,
                TotalMonthlySparedCO2 = totalMonthlySparedCO2,
            }, JsonRequestBehavior.AllowGet);
        }  
    }
}
