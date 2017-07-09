using i18n;
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
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace myfoodapp.Hub.Controllers
{
    public class EventsController : Controller
    {

        [Authorize]
        public ActionResult Index(int id)
        {
            ViewBag.Title = "Production Unit Event Page";

            PopulateEventType();

            return View();
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
        public ActionResult Event_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            EventService eventService = new EventService(db);

            var rslt = eventService.GetAll(id).OrderByDescending(ev => ev.date);

            return Json(rslt.ToDataSourceResult(request));
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Event_Update([DataSourceRequest] DataSourceRequest request, EventViewModel currentEvent)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            EventService eventService = new EventService(db);

            if (currentEvent != null)
            {
                    eventService.Update(currentEvent);
            }

            return Json(new[] { currentEvent }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Event_Destroy([DataSourceRequest] DataSourceRequest request, EventViewModel currentEvent)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            EventService eventService = new EventService(db);

            if (currentEvent != null)
            {
                    eventService.Destroy(currentEvent);
            }

            return Json(new[] { currentEvent }.ToDataSourceResult(request, ModelState));
        }    

        protected override void Dispose(bool disposing)
        {
                base.Dispose(disposing);
        }
    }
}
