using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using myfoodapp.Hub.Business;
using myfoodapp.Hub.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace myfoodapp.Hub.Controllers
{
    public class InteractiveMapController : Controller
    {
        public ActionResult Index(string lang)
        {
            ViewBag.Title = "Interactive Map Page";

            if(lang != String.Empty)
                System.Web.HttpContext.Current.Session["UserLang"] = lang;

            return View();
        }

        public ActionResult ClusterMap()
        {
            ViewBag.Title = "Interactive Map Page";

            return View();
        }

        public ActionResult GetProductionUnitMeasures(int id)
        {
            var db = new ApplicationDbContext();

            var currentProductionUnit = db.ProductionUnits.Where(p => p.picturePath != null && p.lastMeasureReceived != null).ToList()[id];                                         

            var waterTempSensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.waterTemperature, db);

            return Json(new { 
                             CurrentWaterTempValue = waterTempSensorValueSet.CurrentValue,
                             CurrentWaterTempCaptureTime = waterTempSensorValueSet.CurrentCaptureTime,
                             AverageHourWaterTempValue = waterTempSensorValueSet.AverageHourValue,
                             AverageDayWaterTempValue = waterTempSensorValueSet.AverageDayValue,
                             LastDayWaterTempCaptureTime = waterTempSensorValueSet.LastDayCaptureTime,
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProductionUnitIndex(string SelectedProductionUnitCoord)
        {
            var db = new ApplicationDbContext();

            NumberStyles style;
            CultureInfo culture;

            style = NumberStyles.AllowDecimalPoint;
            culture = CultureInfo.CreateSpecificCulture("en-US");

            var strLat = SelectedProductionUnitCoord.Split('|')[0];
            var strLong = SelectedProductionUnitCoord.Split('|')[1];

            double latitude = 0;
            double longitude = 0;

            if (double.TryParse(strLat, style, culture, out latitude) && double.TryParse(strLong, style, culture, out longitude))
            {
                var currentProductionUnit = db.ProductionUnits.Where(p => p.picturePath != null && p.lastMeasureReceived != null).ToList();

                var currentProductionUnitIndex = currentProductionUnit.FindIndex(p => p.locationLatitude == latitude &&
                                                                                 p.locationLongitude == longitude);

                return Json(new
                {
                    CurrentIndex = currentProductionUnitIndex
                }, JsonRequestBehavior.AllowGet);
            }
            else
                return null;
        }

        public ActionResult GetProductionUnitDetailList()
        {
            var db = new ApplicationDbContext();

            var prodUnitListCount = db.ProductionUnits.Where(p => p.picturePath != null && p.lastMeasureReceived != null).Count();

            if (prodUnitListCount == 0)
                return null;

            var currentProductionUnitList = db.ProductionUnits.Where(p => p.picturePath != null && p.lastMeasureReceived != null)
                                         .Include(p => p.owner.preferedMoment)
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

                if (p.owner.preferedMoment != null && p.owner.phoneNumber != String.Empty && p.owner.contactMail != String.Empty)
                {
                    lst.Add(new
                    {
                        PioneerCitizenName = p.owner.pioneerCitizenName,
                        PioneerCitizenNumber = p.owner.pioneerCitizenNumber,
                        ProductionUnitVersion = p.version,
                        ProductionUnitStartDate = p.startDate,
                        ProductionUnitType = p.productionUnitType.name,
                        ProductionUnitStatus = p.productionUnitStatus.name,
                        PhoneNumber = p.owner.phoneNumber,
                        ContactMail = p.owner.contactMail,
                        PicturePath = p.picturePath,
                        PreferedMoment = p.owner.preferedMoment.name,
                        Location = p.owner.location,

                        LocationLatitude = p.locationLatitude,
                        LocationLongitude = p.locationLongitude,

                        ProductionUnitOptions = optionList,
                    });

                }
                else
                {
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
                }
            });

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetNetworkStats()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var stats = PerformanceManager.GetNetworkStatistic(db);

            return Json(new
            {
                ProductionUnitNumber = stats.productionUnitNumber,
                TotalMonthlyProduction = stats.totalMonthlyProduction,
                TotalMonthlySparedCO2 = stats.totalMonthlySparedCO2,
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductionUnitStatus_Read([DataSourceRequest] DataSourceRequest request)
        {
            var db = new ApplicationDbContext();

            var rslt = db.ProductionUnits.Include(p => p.productionUnitStatus).ToList();

            var waitConfCount = rslt.Where(p => p.productionUnitStatus.Id == 1).Count();
            var setupPlannedCount = rslt.Where(p => p.productionUnitStatus.Id == 2).Count();
            var upRunningCount = rslt.Where(p => p.productionUnitStatus.Id == 3).Count();
            var onMaintenanceCount = rslt.Where(p => p.productionUnitStatus.Id == 4).Count();
            var stoppedCount = rslt.Where(p => p.productionUnitStatus.Id == 5).Count();
            var offineCount = rslt.Where(p => p.productionUnitStatus.Id == 6).Count();

            var statusList = new List<PieChartViewModel>();

            statusList.Add(new PieChartViewModel() { Category = "[[[Wait Confirm.]]]", Value = waitConfCount, Color = "#9de219" });
            statusList.Add(new PieChartViewModel() { Category = "[[[Setup Planned]]]", Value = setupPlannedCount, Color = "#90cc38" });
            statusList.Add(new PieChartViewModel() { Category = "[[[Up & Running]]]", Value = upRunningCount, Color = "#068c35" });
            statusList.Add(new PieChartViewModel() { Category = "[[[On Maintenance]]]", Value = onMaintenanceCount, Color = "#006634" });
            statusList.Add(new PieChartViewModel() { Category = "[[[Stopped]]]", Value = stoppedCount, Color = "#004d38" });
            statusList.Add(new PieChartViewModel() { Category = "[[[Offline]]]", Value = offineCount, Color = "#003F38" });

            return Json(statusList);
        }
    }
}
