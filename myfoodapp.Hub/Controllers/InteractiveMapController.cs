using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using myfoodapp.Hub.Business;
using myfoodapp.Hub.Models;
using myfoodapp.Hub.Services;
using myfoodapp.Hub.Viewmodels;
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

            if (lang != String.Empty)
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

            var currentProductionUnit = db.ProductionUnits.Where(p => p.lastMeasureReceived != null).ToList()[id];

            var waterTempSensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.waterTemperature, db);

            var pHSensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.ph, db);


            return Json(new
            {
                CurrentWaterTempValue = waterTempSensorValueSet.CurrentValue,
                CurrentWaterTempCaptureTime = waterTempSensorValueSet.CurrentCaptureTime,
                AverageHourWaterTempValue = waterTempSensorValueSet.AverageHourValue,
                AverageDayWaterTempValue = waterTempSensorValueSet.AverageDayValue,
                LastDayWaterTempCaptureTime = waterTempSensorValueSet.LastDayCaptureTime,

                CurrentpHValue = pHSensorValueSet.CurrentValue,
                CurrentpHCaptureTime = pHSensorValueSet.CurrentCaptureTime,
                AverageHourpHValue = pHSensorValueSet.AverageHourValue,
                AverageDaypHValue = pHSensorValueSet.AverageDayValue,
                LastDaypHCaptureTime = pHSensorValueSet.LastDayCaptureTime,
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
                var currentProductionUnit = db.ProductionUnits.Where(p => p.lastMeasureReceived != null).ToList();

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

        public ActionResult GetProductionUnitDetailPopUp(string SelectedProductionUnitCoord)
        {
            var db = new ApplicationDbContext();

            var strLat = Double.Parse(SelectedProductionUnitCoord.Split('|')[0]);
            var strLong = Double.Parse(SelectedProductionUnitCoord.Split('|')[1]);

            var responseData = db.ProductionUnits.Where(p => p.locationLatitude == strLat && p.locationLongitude == strLong)
                                         .Include(p => p.owner.preferedMoment)
                                         .Include(p => p.productionUnitType)
                                         .Include(p => p.productionUnitStatus).ToList();

            var lst = new object();
            lst = new
            {
                PioneerCitizenName = responseData[0].owner.pioneerCitizenName,
                PioneerCitizenNumber = responseData[0].owner.pioneerCitizenNumber,
                ProductionUnitStartDate = responseData[0].startDate,
                ProductionUnitInfo = responseData[0].info,
                ProductionUnitType = responseData[0].productionUnitType.name,
                ProductionUnitStatus = responseData[0].productionUnitStatus.name,
                PhoneNumber = responseData[0].owner.phoneNumber == null ? "00 33 3 67 37 00 56" : responseData[0].owner.phoneNumber,
                ContactMail = responseData[0].owner.contactMail == null ? "contact@nyfood.eu" : responseData[0].owner.contactMail,
                PicturePath = responseData[0].picturePath == null ? "NoImage.png" : responseData[0].picturePath,

                PreferedMoment = responseData[0].owner.preferedMoment == null ? "" : responseData[0].owner.preferedMoment.name,
                Location = responseData[0].owner.location == null ? "" : responseData[0].owner.location,
            };

            return Json(lst);
        }

        public ActionResult GetProductionUnitDetailList()
        {
            var db = new ApplicationDbContext();

            //TODO uncomment
            var prodUnitListCount = db.ProductionUnits.Where(p => p.picturePath != null && p.lastMeasureReceived != null).Count();
            //var prodUnitListCount = db.ProductionUnits.Where(p => p.lastMeasureReceived != null).Count();

            if (prodUnitListCount == 0)
                return null;

            //TODO uncomment
            var currentProductionUnitList = db.ProductionUnits.Where(p => p.picturePath != null && p.lastMeasureReceived != null)
                                         .Include(p => p.owner.preferedMoment)
                                         .Include(p => p.productionUnitType)
                                         .Include(p => p.productionUnitStatus).ToList();

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

                if (p.owner.preferedMoment != null)
                {
                    lst.Add(new
                    {
                        PioneerCitizenName = p.owner.pioneerCitizenName,
                        PioneerCitizenNumber = p.owner.pioneerCitizenNumber,
                        ProductionUnitVersion = p.version,
                        ProductionUnitStartDate = p.startDate,
                        ProductionUnitType = p.productionUnitType.name,
                        ProductionUnitStatus = p.productionUnitStatus.name,

                        PhoneNumber = p.owner.phoneNumber == null ? "00 33 3 67 37 00 56" : p.owner.phoneNumber,
                        ContactMail = p.owner.contactMail == null ? "contact@myfood.eu" : p.owner.contactMail,
                        PicturePath = p.picturePath == null ? "NoImage.png" : p.picturePath,
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

                        PhoneNumber = p.owner.phoneNumber == null ? "00 33 3 67 37 00 56" : p.owner.phoneNumber,
                        ContactMail = p.owner.contactMail == null ? "contact@myfood.eu" : p.owner.contactMail,
                        PicturePath = p.picturePath == null ? "NoImage.png" : p.picturePath,

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
            MeasureService measureService = new MeasureService(db);

            var rslt = db.ProductionUnits.Include("productionUnitType")
                                         .Where(p => p.productionUnitType.Id <= 5);

            var productionUnitNumber = rslt.Count();

            var totalBalcony = rslt.Where(p => p.productionUnitType.Id == 1).Count();
            var totalCity = rslt.Where(p => p.productionUnitType.Id == 2).Count();
            var totalFamily14 = rslt.Where(p => p.productionUnitType.Id == 3).Count();
            var totalFamily22 = rslt.Where(p => p.productionUnitType.Id == 4).Count();
            var totalFarm = rslt.Where(p => p.productionUnitType.Id == 5).Count();

            var totalMonthlyProduction = totalBalcony * 4 + totalCity * 7 + totalFamily14 * 10 + totalFamily22 * 15 + totalFarm * 25;
            var totalMonthlySparedCO2 = Math.Round(totalMonthlyProduction * 0.3, 0);

            return Json(new
            {
                ProductionUnitNumber = productionUnitNumber,
                TotalMonthlyProduction = totalMonthlyProduction,
                TotalMonthlySparedCO2 = totalMonthlySparedCO2,
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductionUnitStatus_Read([DataSourceRequest] DataSourceRequest request)
        {
            var db = new ApplicationDbContext();

            var rslt = db.ProductionUnits.Include("productionUnitStatus").ToList();

            var waitConfCount = rslt.Where(p => p.productionUnitStatus.Id == 1).Count();
            var setupPlannedCount = rslt.Where(p => p.productionUnitStatus.Id == 2).Count();
            var upRunningCount = rslt.Where(p => p.productionUnitStatus.Id == 3).Count();
            var onMaintenanceCount = rslt.Where(p => p.productionUnitStatus.Id == 4).Count();
            var stoppedCount = rslt.Where(p => p.productionUnitStatus.Id == 5).Count();
            var offineCount = rslt.Where(p => p.productionUnitStatus.Id == 6).Count();

            var statusList = new List<NewPieModel>();

            statusList.Add(new NewPieModel() { name = "[[[Wait Confirm.]]]", y = waitConfCount });
            statusList.Add(new NewPieModel() { name = "[[[Setup Planned]]]", y = setupPlannedCount });
            statusList.Add(new NewPieModel() { name = "[[[Up & Running]]]", y = upRunningCount });
            statusList.Add(new NewPieModel() { name = "[[[On Maintenance]]]", y = onMaintenanceCount });
            statusList.Add(new NewPieModel() { name = "[[[Stopped]]]", y = stoppedCount });
            statusList.Add(new NewPieModel() { name = "[[[Offline]]]", y = offineCount });
            return Json(statusList);
        }
    }
}
