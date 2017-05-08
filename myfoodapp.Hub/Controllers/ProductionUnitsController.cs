using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using myfoodapp.Hub.Business;
using myfoodapp.Hub.Models;
using myfoodapp.Hub.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace myfoodapp.Hub.Controllers
{
    public class ProductionUnitsController : Controller
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var currentUser = this.User.Identity.GetUserName();
            var db = new ApplicationDbContext();

            var userId = UserManager.FindByName(currentUser).Id;
            var isAdmin = this.UserManager.IsInRole(userId, "Admin");

            if (isAdmin)
            {
                PopulateProductionUnitTypes();
                PopulateOwners();
                PopulateProductionUnitStatus();

                return View(await db.ProductionUnits.OrderBy(p => p.startDate).ToListAsync());
            }
            else
            {
                var currentProductionUnits = db.ProductionUnits.Include(p => p.owner.user)
                                                               .Where(p => p.owner.user.UserName == currentUser).ToList();
                if (currentProductionUnits != null)
                {
                    return RedirectToAction("Details", "ProductionUnits", new { Id = currentProductionUnits.FirstOrDefault().Id });
                }
                else
                    return View("Home");
            }
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            ViewBag.DisplayManagementBtn = "None";

            var currentUser = this.User.Identity.GetUserName();
            var userId = UserManager.FindByName(currentUser).Id;
            var isAdmin = this.UserManager.IsInRole(userId, "Admin");

            if (isAdmin)
                ViewBag.DisplayManagementBtn = "All";
            else
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var currentProductionUnit = db.ProductionUnits.Include("owner.user").Where(p => p.Id == id).FirstOrDefault();
                if(currentProductionUnit != null && currentProductionUnit.owner.user.UserName == currentUser)
                {
                    ViewBag.DisplayManagementBtn = "All";
                }
            }

            ViewBag.Title = "Production Unit Detail Page";

            return View();
        }

        [Authorize]
        public ActionResult Events(int id)
        {
            ViewBag.Title = "Production Unit Event Page";

            PopulateEventType();

            return View();
        }

        [Authorize]
        public ActionResult Update(int id)
        {
            var currentUser = this.User.Identity.GetUserName();
            var userId = UserManager.FindByName(currentUser).Id;
            var isAdmin = this.UserManager.IsInRole(userId, "Admin");

            var db = new ApplicationDbContext();
            var productionUnitService = new ProductionUnitService(db);

            var currentProductionUnit = db.ProductionUnits.Include(p => p.owner.user).Include(p => p.options).Where(p => p.Id == id).FirstOrDefault();
            if (currentProductionUnit != null && currentProductionUnit.owner.user.UserName == currentUser || isAdmin)
            {
                var currentProductionUnitViewModel = productionUnitService.One(id);
                return View(currentProductionUnitViewModel);
            }

            return Redirect("Home/Index");
        }

        [HttpPost]
        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update(ProductionUnitViewModel model, string returnUrl)
        {
            //if (ModelState.IsValid)
            //{
                var db = new ApplicationDbContext();
                var productionUnitService = new ProductionUnitService(db);

                productionUnitService.Update(model);
            // }

            return Redirect("/ProductionUnits/Details/" + model.Id);
        }

        [Authorize]
        public ActionResult Event_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            EventService eventService = new EventService(db);

            var rslt = eventService.GetAll(id).OrderByDescending(ev => ev.date);

            return Json(rslt.ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ProductionUnitService productionUnitService = new ProductionUnitService(db);

            return Json(productionUnitService.Read().ToDataSourceResult(request));
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ProductionUnitViewModel> productionUnits)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ProductionUnitService productionUnitService = new ProductionUnitService(db);

            var results = new List<ProductionUnitViewModel>();

            if (productionUnits != null && ModelState.IsValid)
            {
                foreach (var productionUnit in productionUnits)
                {
                    productionUnitService.Create(productionUnit);
                    results.Add(productionUnit);
                }
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ProductionUnitViewModel> productionUnits)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ProductionUnitService productionUnitService = new ProductionUnitService(db);

            if (productionUnits != null && ModelState.IsValid)
            {
                foreach (var productionUnit in productionUnits)
                {
                    productionUnitService.Update(productionUnit);
                }
            }

            return Json(productionUnits.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<ProductionUnitViewModel> productionUnits)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ProductionUnitService productionUnitService = new ProductionUnitService(db);

            if (productionUnits.Any())
            {
                foreach (var productionUnit in productionUnits)
                {
                    productionUnitService.Destroy(productionUnit);
                }
            }

            return Json(productionUnits.ToDataSourceResult(request, ModelState));
        }

        private void PopulateProductionUnitTypes()
        {
             ApplicationDbContext db = new ApplicationDbContext();

             var productionUnitTypes = db.ProductionUnitTypes
                        .Select(m => new ProductionUnitTypeViewModel
                        {
                            Id = m.Id,
                            name = m.name
                        })
                        .OrderBy(e => e.name);

            ViewData["ProductionUnitTypes"] = productionUnitTypes;
        }

        private void PopulateOwners()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var owners = db.ProductionUnitOwners
                        .Select(m => new OwnerViewModel
                        {
                            Id = m.Id,
                            pioneerCitizenName = m.pioneerCitizenName,
                            pioneerCitizenNumber = m.pioneerCitizenNumber
                        })
                        .OrderBy(e => e.pioneerCitizenNumber);

            ViewData["owners"] = owners;
        }

        private void PopulateProductionUnitStatus()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var productionUnitStatus = db.ProductionUnitStatus
                       .Select(m => new ProductionUnitStatusViewModel
                       {
                           Id = m.Id,
                           name = m.name
                       })
                       .OrderBy(e => e.name);

            ViewData["ProductionUnitStatus"] = productionUnitStatus;
        }

        private void PopulateEventType()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var eventTypes = db.EventTypes
                       .Select(m => new EventTypeViewModel
                       {
                           Id = m.Id,
                           name = m.name
                       })
                       .OrderBy(e => e.name);

            ViewData["EventTypes"] = eventTypes;
        }

        [Authorize]
        public ActionResult HydroponicTypes_Read([DataSourceRequest] DataSourceRequest request)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var rslt = db.HydroponicTypes;

            return Json(rslt, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult PHMeasure_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            var db = new ApplicationDbContext();
            var measureService = new MeasureService(db);

            return Json(measureService.Read(SensorTypeEnum.ph, id), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult WaterTempMeasure_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            var db = new ApplicationDbContext();
            var measureService = new MeasureService(db);

            return Json(measureService.Read(SensorTypeEnum.waterTemperature, id), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AirTempMeasure_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            var db = new ApplicationDbContext();
            var measureService = new MeasureService(db);

            return Json(measureService.Read(SensorTypeEnum.airTemperature, id), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult HumidityMeasure_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            var db = new ApplicationDbContext();
            var measureService = new MeasureService(db);

            return Json(measureService.Read(SensorTypeEnum.humidity, id), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ORPMeasure_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            var db = new ApplicationDbContext();
            var measureService = new MeasureService(db);

            return Json(measureService.Read(SensorTypeEnum.orp, id), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult DissolvedOxyMeasure_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            var db = new ApplicationDbContext();
            var measureService = new MeasureService(db);

            return Json(measureService.Read(SensorTypeEnum.dissolvedOxygen, id), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetProductionUnitDetail(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            MeasureService measureService = new MeasureService(db);

            var currentProductionUnit = db.ProductionUnits.Include(p => p.owner.user)
                                          .Include(p => p.productionUnitType)
                                          .Include(p => p.options)
                                          .Where(p => p.Id == id).FirstOrDefault();

            var averageMonthlyProduction = 0;

            switch (currentProductionUnit.productionUnitType.Id)
            {
                case 1:
                    averageMonthlyProduction = 5;
                    break;
                case 2:
                    averageMonthlyProduction = 10;
                    break;
                case 3:
                    averageMonthlyProduction = 15;
                    break;
                case 4:
                    averageMonthlyProduction = 25;
                    break;
                case 5:
                    averageMonthlyProduction = 50;
                    break;
                default:
                    break;
            }

            var averageMonthlySparedCO2 = averageMonthlyProduction * 0.3;

            var pHSensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.ph, db);
            var waterTempSensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.waterTemperature, db);
            var airTempSensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.airTemperature, db);
            var humiditySensorValueSet = SensorValueManager.GetSensorValueSet(currentProductionUnit.Id, SensorTypeEnum.humidity, db);

            var options = db.OptionLists.Include(o => o.productionUnit)
                        .Include(o => o.option)
                        .Where(p => p.productionUnit.Id == id)
                        .Select(p => p.option);

            var optionList = string.Empty;

            if(options.Count() > 0)
            {
                options.ToList().ForEach(o => { optionList += o.name + " / "; });
            }

            var onlineSinceWeeks = Math.Round((DateTime.Now - currentProductionUnit.startDate).TotalDays / 7);

            return Json(new
            {
                PioneerCitizenName = currentProductionUnit.owner.pioneerCitizenName,
                PioneerCitizenNumber = currentProductionUnit.owner.pioneerCitizenNumber,
                ProductionUnitVersion = currentProductionUnit.version,
                ProductionUnitType = currentProductionUnit.productionUnitType.name,
                PicturePath = currentProductionUnit.picturePath,

                ProductionUnitOptions = optionList,
                OnlineSinceWeeks = onlineSinceWeeks,

                AverageMonthlyProduction = averageMonthlyProduction,
                AverageMonthlySparedCO2 = averageMonthlySparedCO2,

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

        [Authorize]
        public ActionResult Option_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            MeasureService measureService = new MeasureService(db);

            var rslt = db.OptionLists.Include(o => o.productionUnit)
                                     .Include(o => o.option)
                                     .Where(p => p.productionUnit.Id == id)
                                     .Select(p => p.option);

            return Json(rslt.ToDataSourceResult(request));
        }

        protected override void Dispose(bool disposing)
        {
                base.Dispose(disposing);
        }
    }
}
