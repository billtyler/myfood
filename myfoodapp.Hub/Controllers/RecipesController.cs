using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<ActionResult> Gallery()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var currentMonths = DateTime.Now.Month;

            var rstl = await db.Recipes
                                    .Include(r => r.plantingIndoorMonths)
                                    .Include(r => r.plantingOutdoorMonths)
                                    .Include(r => r.harvestingMonths)
                                    .Include(r => r.wateringLevel)
                                    .Include(r => r.gardeningType)
                                    .ToListAsync();

            rstl.ForEach(r =>
            {
                if (r.plantingIndoorMonths.Where(p => p.order == currentMonths).FirstOrDefault() != null)
                    r.isRecommended = true;
            }
                        );

            return Json(rstl);
        }

        [Authorize]
        public ActionResult ProductionUnits_Read([DataSourceRequest] DataSourceRequest request)
        {
            var currentUser = this.User.Identity.GetUserName();
            var db = new ApplicationDbContext();

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
