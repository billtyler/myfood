using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using myfoodapp.Hub.Models;
using myfoodapp.Hub.Services;
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
        // GET: ProductionUnits
        public async Task<ActionResult> Index()
        {
            PopulateProductionUnitTypes();
            PopulateOwners();

            ApplicationDbContext db = new ApplicationDbContext();
            return View(await db.ProductionUnits.ToListAsync());
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

        public ActionResult Event_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var rslt = db.Events.Where(e => e.productionUnit.Id == id).ToList();

            return Json(rslt.ToDataSourceResult(request));
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ProductionUnitService productionUnitService = new ProductionUnitService(db);

            return Json(productionUnitService.Read().ToDataSourceResult(request));
        }

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

        public ActionResult HydroponicTypes_Read([DataSourceRequest] DataSourceRequest request)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            MeasureService measureService = new MeasureService(db);

            var rslt = db.HydroponicTypes;

            return Json(rslt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Measures_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            MeasureService measureService = new MeasureService(db);

            var groupedValue = new List<GroupedMeasure>();
            var rslt = measureService.Read(id);

            var groupedRslt = rslt.GroupBy(m => m.captureDate);

            groupedRslt.ToList().ForEach(gr => 
            {
                var newMeas = new GroupedMeasure() { captureDate = gr.Key };

                gr.ToList().ForEach(meas => 
                {
                    if (meas.sensorId == 1)
                        newMeas.pHvalue = meas.value;
                    if (meas.sensorId == 2)
                        newMeas.waterTempvalue = meas.value;
                    if (meas.sensorId == 3)
                        newMeas.DOvalue = meas.value;
                    if (meas.sensorId == 4)
                        newMeas.ORPvalue = meas.value;
                    if (meas.sensorId == 5)
                        newMeas.airTempvalue = meas.value;
                    if (meas.sensorId == 6)
                        newMeas.humidityvalue = meas.value;
                });

                groupedValue.Add(newMeas);
            });

            return Json(groupedValue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdvancedMeasures_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            MeasureService measureService = new MeasureService(db);

            var groupedValue = new List<GroupedMeasure>();
            var rslt = measureService.Read(id);

            var groupedRslt = rslt.GroupBy(m => m.captureDate);

            groupedRslt.ToList().ForEach(gr =>
            {
                var newMeas = new GroupedMeasure() { captureDate = gr.Key };

                gr.ToList().ForEach(meas =>
                {
                    if (meas.sensorId == 3)
                        newMeas.DOvalue = meas.value;
                    if (meas.sensorId == 4)
                        newMeas.ORPvalue = meas.value;
                });

                groupedValue.Add(newMeas);
            });

            return Json(groupedValue, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProductionUnitDetail(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            MeasureService measureService = new MeasureService(db);

            var rslt = db.ProductionUnits.Include("owner.user")
                                         .Include("productionUnitType")
                                         .Where(p => p.Id == id).FirstOrDefault();

            var averageMonthlyProduction = 0;

            switch (rslt.productionUnitType.Id)
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

            return Json(new
            {
                PioneerCitizenName = rslt.owner.pioneerCitizenName,
                PioneerCitizenNumber = rslt.owner.pioneerCitizenNumber,
                ProductionUnitVersion = rslt.version,
                ProductionUnitStartDate = rslt.startDate,
                ProductionUnitType = rslt.productionUnitType.name,
                PicturePath = rslt.picturePath,
                AverageMonthlyProduction = averageMonthlyProduction,
                AverageMonthlySparedCO2 = averageMonthlySparedCO2,
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Option_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            MeasureService measureService = new MeasureService(db);

            var rslt = db.OptionLists.Include("productionUnit")
                                    .Include("option")
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
