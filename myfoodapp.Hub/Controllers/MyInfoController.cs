using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using myfoodapp.Hub.Models;
using System.Web.Security;
using System.Data.Entity;
using System.Linq;
using Kendo.Mvc.UI;

namespace myfoodapp.Hub.Controllers
{   
    [Authorize]
    public class MyInfoController : Controller
    {        
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
             
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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
        public ActionResult Update()
        {
            var currentUser = this.User.Identity.GetUserName();
            var applicationUser = UserManager.FindByEmail(currentUser);

            var db = new ApplicationDbContext();

            var currentProductOwner = db.ProductionUnitOwners.Include(p => p.language)
                                                             .Where(p => p.user.UserName == currentUser).FirstOrDefault();

            var currentLanguageId = currentProductOwner.language.Id;

            return View(new UserViewModel() { Email = applicationUser.Email, Language = currentLanguageId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(UserViewModel model, string returnUrl)
        {
            var db = new ApplicationDbContext();
            var currentUser = this.User.Identity.GetUserName();

            var currentProductOwner = db.ProductionUnitOwners.Include(p => p.language)
                                                 .Where(p => p.user.UserName == currentUser).FirstOrDefault();

            currentProductOwner.language = db.Languages.Where(l => l.Id == model.Language).FirstOrDefault();
            db.SaveChanges();

            if (ModelState.IsValid)
            {
                var applicationUser = UserManager.FindByEmail(currentUser);

                if (model.Email != applicationUser.Email)
                {
                    applicationUser.Email = model.Email;
                    applicationUser.UserName = model.Email;
                    var resultMailChanged = await UserManager.UpdateAsync(applicationUser);
                    if(!resultMailChanged.Succeeded)
                    {
                        AddErrors(resultMailChanged);
                        return View(model);
                    }
                }

                if(model.OldPassword != null && model.NewPassword != null)
                {
                    var resultPasswordChanged = await UserManager.ChangePasswordAsync(applicationUser.Id, model.OldPassword, model.NewPassword);
                    if (resultPasswordChanged.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    AddErrors(resultPasswordChanged);
                }


            }

            return View(model);
        }

        [Authorize]
        public ActionResult AddPushNotification(string id)
        {
            var currentUser = this.User.Identity.GetUserName();

            var db = new ApplicationDbContext();

            var currentProductionOwner = db.ProductionUnitOwners.Include(p => p.user)
                                                           .Where(p => p.user.UserName == currentUser).FirstOrDefault();
            if (currentProductionOwner != null)
            {
                currentProductionOwner.notificationPushKey = id;
                db.SaveChanges();

                return Json(currentProductionOwner, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        [Authorize]
        public ActionResult Languages_Read([DataSourceRequest] DataSourceRequest request)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var rslt = db.Languages;

            return Json(rslt, JsonRequestBehavior.AllowGet);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [Authorize]
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}