using i18n;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using myfoodapp.Hub.Business;
using myfoodapp.Hub.Common;
using myfoodapp.Hub.Models;
using myfoodapp.Hub.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace myfoodapp.Hub.Controllers
{
    public class ProductionUnitsController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            base.Initialize(requestContext);
        }

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
        public ActionResult Index()
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
                PopulateHydroponicType();

                return View();
            }
            else
            {
                var currentProductionUnits = db.ProductionUnits.Include(p => p.owner.user)
                                               .Where(p => p.owner.user.UserName == currentUser).ToList();

				if (currentProductionUnits != null)
                {
					if (isAdmin)
						return RedirectToAction("Details", "ProductionUnits", new { Id = currentProductionUnits.FirstOrDefault().Id });
					else
						return RedirectToAction("UserUnit", "ProductionUnits");
                }
                else
                    return View("Home");
            }
        }

		[Authorize]
		public ActionResult UserUnit()
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

				var userProductionUnit = db.ProductionUnits.Include(p => p.owner.user).Where(p => p.owner.user.UserName == currentUser).ToList().FirstOrDefault();


				ViewBag.CurrentUser = userProductionUnit.Id;

				if (userProductionUnit != null && userProductionUnit.owner.user.UserName == currentUser)
				{
					ViewBag.DisplayManagementBtn = "All";
				}
			}
			ViewBag.Title = "Production Unit Detail Page";

			return View();
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
				
				var currentProductionUnit = db.ProductionUnits.Include(p => p.owner.user).Where(p => p.Id == id).FirstOrDefault();
				

                if(currentProductionUnit != null && currentProductionUnit.owner.user.UserName == currentUser)
                {
                    ViewBag.DisplayManagementBtn = "All";
                }
            }
            ViewBag.Title = "Production Unit Detail Page";

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
                PopulateOptions(currentProductionUnitViewModel);
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

                var tt = model.options;

                productionUnitService.Update(model);
            //}

            return Redirect("/ProductionUnits/Details/" + model.Id);
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
        public ActionResult Editing_Create([DataSourceRequest] DataSourceRequest request, ProductionUnitViewModel currentProductionUnit)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ProductionUnitService productionUnitService = new ProductionUnitService(db);

            var results = new List<ProductionUnitViewModel>();

            if (ModelState.IsValid)
            {
                    productionUnitService.Create(currentProductionUnit);
            }

            return Json(new[] { currentProductionUnit }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Update([DataSourceRequest] DataSourceRequest request, ProductionUnitViewModel currentProductionUnit)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ProductionUnitService productionUnitService = new ProductionUnitService(db);

            if (ModelState.IsValid)
            {
                    productionUnitService.Update(currentProductionUnit);
            }
            
            return Json(new[] { currentProductionUnit }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Editing_Destroy([DataSourceRequest] DataSourceRequest request, ProductionUnitViewModel currentProductionUnit)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ProductionUnitService productionUnitService = new ProductionUnitService(db);

            if (currentProductionUnit != null)
            {
                if (currentProductionUnit.lastMeasureReceived != null)
                    ModelState.AddModelError("inUse", new Exception("[[[Production Unit already in use]]]"));
                else
                    productionUnitService.Destroy(currentProductionUnit);
            }

            return Json(new[] { currentProductionUnit }.ToDataSourceResult(request, ModelState));
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

        private void PopulateHydroponicType()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var hydroponicTypes = db.HydroponicTypes
                       .Select(m => new ProductionUnitStatusViewModel
                       {
                           Id = m.Id,
                           name = m.name
                       })
                       .OrderBy(e => e.name);

            ViewData["HydroponicType"] = hydroponicTypes;
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

        private void PopulateOptions(ProductionUnitViewModel productionUnitViewModel)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var currentOptions = db.OptionLists.Include(o => o.productionUnit)
            .Include(o => o.option)
            .Where(p => p.productionUnit.Id == productionUnitViewModel.Id)
            .Select(p => p.option);

            productionUnitViewModel.options = currentOptions.Select(m => new OptionViewModel
            {
                Id = m.Id,
                name = m.name
            })
           .OrderBy(e => e.name).ToList();

            var options = db.Options;

            ViewBag.Options = options.Except(currentOptions).Select(m => new OptionViewModel
            {
                Id = m.Id,
                name = m.name
            })
           .OrderBy(e => e.name); 
        }

        [Authorize]
        public ActionResult HydroponicTypes_Read([DataSourceRequest] DataSourceRequest request)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var rslt = db.HydroponicTypes;

            return Json(rslt, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult PHMeasure_Read([DataSourceRequest] DataSourceRequest request, int id, string range)
        {
            var db = new ApplicationDbContext();
            var measureService = new MeasureService(db);
            var json = new List<List<object>>();

            var hpList = measureService.Read(SensorTypeEnum.ph, id, range).ToList();
            var phFiltered = FilterArrayForGraph(hpList);

            foreach (var PHMeasure in phFiltered)
            {
                var list = new List<object>();
                list.Add(PHMeasure.captureDate.ToString("R"));
                list.Add(PHMeasure.value);
                json.Add(list);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetProductionUnitDetailPopUp(int id)
        {
            var db = new ApplicationDbContext();
            

            var responseData = db.ProductionUnits.Where(p => p.Id == id)
                                         .Include(p => p.owner.preferedMoment)
                                         .Include(p => p.productionUnitType)
                                         .Include(p => p.productionUnitStatus).ToList()[0];


            var options = db.OptionLists.Include(o => o.productionUnit)
                .Include(o => o.option)
                .Where(o => o.productionUnit.Id == responseData.Id)
                .Select(o => o.option);

            var optionList = string.Empty;

            if (options.Count() > 0)
            {
                options.ToList().ForEach(o => { optionList += o.name + "/"; });
            }
            var averageMonthlyProduction = PerformanceManager.GetEstimatedMonthlyProduction(responseData.productionUnitType.Id);
            var onlineSinceWeeks = Math.Round((DateTime.Now - responseData.startDate).TotalDays / 7);
            var averageMonthlySparedCO2 = averageMonthlyProduction * 0.3;

            var lst = new object();
            lst = new
            {
                AverageMonthlySparedCO2 = averageMonthlySparedCO2,
                OnlineSinceWeeks = onlineSinceWeeks,
                AverageMonthlyProduction = averageMonthlyProduction,
                PioneerCitizenName = responseData.owner.pioneerCitizenName,
                PioneerCitizenNumber = responseData.owner.pioneerCitizenNumber,
                ProductionUnitStartDate = responseData.startDate,
                ProductionUnitInfo = responseData.info,
                ProductionUnitTypeName = responseData.productionUnitType.name,
                ProductionUnitTypeImage = responseData.productionUnitType.imagePath,
                ProductionUnitStatus = responseData.productionUnitStatus.name,

                PhoneNumber = responseData.owner.phoneNumber == null ? "00 33 3 67 37 00 56" : responseData.owner.phoneNumber,
                ContactMail = responseData.owner.contactMail == null ? "contact@myfood.eu" : responseData.owner.contactMail,
                PicturePath = responseData.picturePath == null ? "NoImage.png" : responseData.picturePath,

                PreferedMoment = responseData.owner.preferedMoment == null ? "" : responseData.owner.preferedMoment.name,
                Location = responseData.owner.location == null ? "" : responseData.owner.location,

                ProductionUnitOptions = optionList,
            };

            return Json(lst);
        }

        [Authorize]
        public ActionResult WaterTempMeasure_Read([DataSourceRequest] DataSourceRequest request, int id, string range)
        {
            var db = new ApplicationDbContext();
            var measureService = new MeasureService(db);

            var waterTemperature = measureService.Read(SensorTypeEnum.waterTemperature, id, range).ToList();
            var waterTemperatureFiltered = FilterArrayForGraph(waterTemperature);

            var json = new List<List<object>>();
            foreach (var WaterTempMeasure in waterTemperatureFiltered)
            {
                var list = new List<object>();
                list.Add(WaterTempMeasure.captureDate.ToString("R"));
                list.Add(WaterTempMeasure.value);
                json.Add(list);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AirTempMeasure_Read([DataSourceRequest] DataSourceRequest request, int id, string range)
        {
            var db = new ApplicationDbContext();
            var measureService = new MeasureService(db);
            
            return Json(measureService.Read(SensorTypeEnum.airTemperature, id, range), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult TempMeasure_Read([DataSourceRequest] DataSourceRequest request, int id, string range)
        {
            var db = new ApplicationDbContext();
            var measureService = new MeasureService(db);

            var airTemperature = measureService.Read(SensorTypeEnum.airTemperature, id, range).ToList();

            var airTemperatureFiltered = FilterArrayForGraph(airTemperature);


            var air = new List<List<object>>();
            foreach (var airtemp in airTemperatureFiltered)
            {
                var list = new List<object>();
                list.Add(airtemp.captureDate.ToString("R"));
                list.Add(airtemp.value);
                air.Add(list);
            }


            var airExternalTemperature = measureService.Read(SensorTypeEnum.externalAirTemperature, id, range).ToList();

            var airExternalTemperatureFiltered = FilterArrayForGraph(airExternalTemperature);

            var externalAir = new List<List<object>>();
            foreach (var externalAirTemperature in airExternalTemperatureFiltered)
            {
                var list = new List<object>();
                list.Add(externalAirTemperature.captureDate.ToString("R"));
                list.Add(externalAirTemperature.value);
                externalAir.Add(list);
            }

            var obj = new object();
            obj = new
            {
                air = air,
                externalAir = externalAir,
            };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult AirHumidityMeasure_Read([DataSourceRequest] DataSourceRequest request, int id, string range)
        {
            var db = new ApplicationDbContext();
            var measureService = new MeasureService(db);

            return Json(measureService.Read(SensorTypeEnum.humidity, id, range), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult HumidityMeasure_Read([DataSourceRequest] DataSourceRequest request, int id, string range)
        {
            var db = new ApplicationDbContext();
            var measureService = new MeasureService(db);

            var measuresList = new List<MeasureViewModel>();


            var result = measureService.Read(SensorTypeEnum.humidity, id, range).ToList();

            var filteredResult = FilterArrayForGraph(result);

            var hum = new List<List<object>>();

            foreach (var airTemperature in filteredResult)
            {
                var list = new List<object>();
                list.Add(airTemperature.captureDate.ToString("R"));
                list.Add(airTemperature.value);
                hum.Add(list);
            }

            var resultHumidiy = measureService.Read(SensorTypeEnum.externalHumidity, id, range).ToList();
            var filteredResultHumidiy = FilterArrayForGraph(resultHumidiy);

            var externalHum = new List<List<object>>();
            foreach (var externalHumidity in filteredResultHumidiy)
            {
                var list = new List<object>();
                list.Add(externalHumidity.captureDate.ToString("R"));
                list.Add(externalHumidity.value);
                externalHum.Add(list);
            }

            var obj = new object();
            obj = new
            {
                humidity = hum,
                externalHumidity = externalHum,
            };


            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        private List<MeasureViewModel> FilterArrayForGraph(List<MeasureViewModel> result)
        {
            var filteredResult = new List<MeasureViewModel>();

            var totalCount = result.Count;
            var delta = 100;

            if (totalCount > delta)
            {
                var step = totalCount / delta;
                for (int i = 0; i < totalCount; i = i + step)
                {
                    filteredResult.Add(result[i]);
                }
            }
            else
            {
                filteredResult = result;
            }

            return filteredResult;
        }

        [Authorize]
        public ActionResult ORPMeasure_Read([DataSourceRequest] DataSourceRequest request, int id, string range)
        {
            var db = new ApplicationDbContext();
            var measureService = new MeasureService(db);

            return Json(measureService.Read(SensorTypeEnum.orp, id, range), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult DissolvedOxyMeasure_Read([DataSourceRequest] DataSourceRequest request, int id, string range)
        {
            var db = new ApplicationDbContext();
            var measureService = new MeasureService(db);

            return Json(measureService.Read(SensorTypeEnum.dissolvedOxygen, id, range), JsonRequestBehavior.AllowGet);
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


            var averageMonthlyProduction = PerformanceManager.GetEstimatedMonthlyProduction(currentProductionUnit.productionUnitType.Id);
            var onlineSinceWeeks = Math.Round((DateTime.Now - currentProductionUnit.startDate).TotalDays / 7);
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

        [Authorize]
        public ActionResult AddOptionFromProductionUnit(int id, int optionId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            MeasureService measureService = new MeasureService(db);

            var currentProductionUnit = db.ProductionUnits.Where(o => o.Id == id).FirstOrDefault();
            var currentOption = db.Options.Where(o => o.Id == optionId).FirstOrDefault();

            if(currentOption == null)
                return HttpNotFound();

            db.OptionLists.Add(new OptionList() {option = currentOption, productionUnit = currentProductionUnit });

            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Authorize]
        public ActionResult SendFeelingRequest()
        {
            var db = new ApplicationDbContext();

            var upRunningStatus = db.ProductionUnitStatus.Where(s => s.Id == 3).FirstOrDefault();
            var upRunningProductionUnits = db.ProductionUnits.Include(p => p.owner.language).Where(p => p.productionUnitStatus.Id == upRunningStatus.Id).ToList();

            upRunningProductionUnits.ForEach(p =>
            {
                if (p.owner.notificationPushKey != null)
                {
                    NotificationPushManager.PioneerUnitOwnerFeelingMessage(p);
                }
            });

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Authorize]
        public ActionResult RemoveOptionFromProductionUnit(int id, int optionId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            MeasureService measureService = new MeasureService(db);

            var currentOption = db.Options.Where(o => o.Id == optionId).FirstOrDefault();

            if (currentOption == null)
                return HttpNotFound();

            var currentProductionUnitOptions = db.OptionLists.Include(o => o.productionUnit).Include(o => o.option)
                            .Where(p => p.productionUnit.Id == id);

            var removeOption = db.OptionLists.Include(o => o.productionUnit)
                                        .Where(p => p.productionUnit.Id == id && p.option.Id == currentOption.Id).FirstOrDefault();

            db.OptionLists.Remove(removeOption);

            db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [Authorize]
        public ActionResult EventType_Read([DataSourceRequest] DataSourceRequest request)
        {
            var db = new ApplicationDbContext();

            var eventTypes = db.EventTypes.Where(et => et.isDisplayedForUser).OrderBy(et=> et.order).ToList();

            var eventTypesModel = eventTypes.Select(vm => new
            {
                Id = vm.Id,
                description = vm.description,
                order = vm.order,
                name = vm.name,
            });

            return Json(eventTypesModel.ToDataSourceResult(request, ModelState));       
        }

        [Authorize]
        public ActionResult EventTypeItem_Read([DataSourceRequest] DataSourceRequest request, int evenTypeId)
        {
            var db = new ApplicationDbContext();

            var currentUser = this.User.Identity.GetUserName();
            var userId = UserManager.FindByName(currentUser).Id;
            var isAdmin = this.UserManager.IsInRole(userId, "Admin");

            var eventTypesItems = new List<EventTypeItem>();

            if (isAdmin)
                eventTypesItems = db.EventTypeItems.OrderBy(et => et.order).Where(et => et.eventType.Id == evenTypeId).ToList();
            else
                eventTypesItems = db.EventTypeItems.OrderBy(et => et.order).Where(et => et.eventType.Id == evenTypeId && et.isRestrictedForAdmin == false).ToList();

            return Json(eventTypesItems.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        public ActionResult AddEvent(int productionUnitId, int eventTypeId, int eventTypeItemId, string note, DateTime currentDate, string details)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationDbContext dbLog = new ApplicationDbContext();

            var currentUser = this.User.Identity.GetUserName();
            var userId = UserManager.FindByName(currentUser).Id;

            var currentProductUnitOwner = db.ProductionUnitOwners.Where(p => p.user.Id == userId).FirstOrDefault();
            var currentProductionUnit = db.ProductionUnits.Where(p => p.Id == productionUnitId).FirstOrDefault();
            var currentEventType = db.EventTypes.Where(et => et.Id == eventTypeId).FirstOrDefault();

            var currentEventTypeItem = db.EventTypeItems.Where(et => et.eventType.Id == eventTypeId && et.Id == eventTypeItemId).FirstOrDefault();

            bool isOpen = false;

            if (currentEventType.name.Contains("Issue"))
                isOpen = true;

            var newEvent = new Event() { productionUnit = currentProductionUnit,
                                         eventType = currentEventType,
                                         isOpen = isOpen,
                                         date = currentDate,
                                         details = details,
                                         createdBy = currentProductUnitOwner.pioneerCitizenName
            };

            if (!String.IsNullOrEmpty(note))
                if (currentEventTypeItem != null)
                    newEvent.description = String.Format("{0} : {1}", HttpContext.ParseAndTranslate(currentEventTypeItem.name), note);
                else
                    newEvent.description = String.Format("{0}", note);
            else
                if (currentEventTypeItem != null)
                    newEvent.description = String.Format("{0}", HttpContext.ParseAndTranslate(currentEventTypeItem.name));
            else
                    newEvent.description = String.Empty;

            newEvent.description = System.Web.HttpUtility.HtmlDecode(newEvent.description);

            try
            {
                db.Events.Add(newEvent);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                dbLog.Logs.Add(Log.CreateErrorLog(String.Format("Error with Event Creation"), ex));
                dbLog.SaveChanges();
                return Json(false);
            }

            return Json(true);
        }

        [Authorize]
        public ActionResult SavePicture(IEnumerable<HttpPostedFileBase> files, string picturePath)
        {
            var fileName = Path.GetFileName(files.FirstOrDefault().FileName);

            if (fileName != picturePath)
            {
                var db = new ApplicationDbContext();
                var prodUnit = db.ProductionUnits.Include(p => p.owner)
                                                 .Include(p => p.hydroponicType)
                                                 .Include(p => p.productionUnitType)
                                                 .Include(p => p.productionUnitStatus)
                                                 .Where(p => p.picturePath == picturePath).FirstOrDefault();
                if (prodUnit != null)
                {
                    prodUnit.picturePath = fileName;
                    db.SaveChanges();

                    var fullPath = Path.Combine(Server.MapPath("~/Content/Pictures/Sites/"), picturePath);

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }         
            }

            if (files != null)
            {
                foreach (var file in files)
                {
                    var currentfileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content/Pictures/Sites/"), currentfileName);
                    var fileExtension = Path.GetExtension(file.FileName);
                    file.SaveAs(physicalPath);
                }
            }

            return Json("");
        }

        [Authorize]
        public FileContentResult DownloadCSV(int id)
        {
            var dbLog = new ApplicationDbContext();

            try
            {
                ApplicationDbContext db = new ApplicationDbContext();
                var currentProductionUnit = db.ProductionUnits.Include(p => p.owner.user)
                                                              .Include(p => p.productionUnitType)
                                                              .Where(p => p.Id == id).FirstOrDefault();

                string fileName = String.Format("{0}_#{1}_[{2}].csv", currentProductionUnit.owner.pioneerCitizenName,
                                                                        currentProductionUnit.owner.pioneerCitizenNumber,
                                                                        DateTime.Now.ToShortDateString());

                StringBuilder csv = new StringBuilder();

                var mes = db.Measures.Include(m => m.sensor)
                           .Where(m => m.productionUnit.Id == currentProductionUnit.Id)
                           .OrderByDescending(m => m.captureDate)
                           .Take(15000);

                mes.OrderBy(m => m.captureDate).ToList().GroupBy(m => m.captureDate).ToList().ForEach(m =>
                {
                    csv.Append(m.Key + "; ");

                    m.OrderBy(c => c.sensor.Id).ToList().ForEach(g => {
                        csv.Append(g.value + "; ");
                    });

                    csv.Remove(csv.Length - 2, 1);
                    csv.Append("\r\n");
                }
                );

                return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", fileName);
            }
            catch (Exception ex)
            {
                dbLog.Logs.Add(Log.CreateErrorLog("Error on Convert Message into Measure", ex));
                dbLog.SaveChanges();
            }

            return null;

        }

        protected override void Dispose(bool disposing)
        {
                base.Dispose(disposing);
        }
    }
}
