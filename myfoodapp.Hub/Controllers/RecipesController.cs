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
    public class RecipesController : Controller
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
            ApplicationDbContext db = new ApplicationDbContext();
            return View(await db.Recipes
                                    .Include(r => r.plantingIndoorMonths)
                                    .Include(r => r.plantingOutdoorMonths)
                                    .Include(r => r.harvestingMonths)
                                    .Include(r => r.wateringLevel)
                                    .Include(r => r.gardeningType)
                                    .ToListAsync());
        }

        public ActionResult ProductionUnits_Read([DataSourceRequest] DataSourceRequest request)
        {
            var currentUser = this.User.Identity.GetUserName();
            var userId = UserManager.FindByName(currentUser).Id;

            var db = new ApplicationDbContext();
            var productionUnitService = new ProductionUnitService(db);

            var currentProductionUnits = db.ProductionUnits.Include(p => p.owner.user)
                                                           .Where(p => p.owner.user.UserName == currentUser).ToList();
            if (currentProductionUnits != null)
            {
                return Json(currentProductionUnits, JsonRequestBehavior.AllowGet);
            }

            return null;
        }
    }
}
