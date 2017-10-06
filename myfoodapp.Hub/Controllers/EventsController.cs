using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using myfoodapp.Hub.Models;
using myfoodapp.Hub.Services;
using System;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Mvc;

namespace myfoodapp.Hub.Controllers
{
    public class EventsController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            base.Initialize(requestContext);
        }

        [Authorize]
        public ActionResult Index(int id)
        {
            ViewBag.Title = "Production Unit Event Page";

            PopulateEventType();

            return View();
        }

        [Authorize]
        public ActionResult Manage()
        {
            ViewBag.Title = "Production Units Event Page";

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

        public ActionResult Events_Read([DataSourceRequest] DataSourceRequest request)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            EventService eventService = new EventService(db);

            var rslt = eventService.GetAll().OrderByDescending(ev => ev.date).OrderByDescending(ev => ev.Id);

            return Json(rslt.ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult Event_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            EventService eventService = new EventService(db);

            var rslt = eventService.GetAll(id).OrderByDescending(ev => ev.date).OrderByDescending(ev => ev.Id);

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

        [Authorize]
        public FileContentResult DownloadCSV()
        {
            var dbLog = new ApplicationDbContext();

            try
            {
                ApplicationDbContext db = new ApplicationDbContext();

                string fileName = String.Format("Events_All_[{0}].csv", DateTime.Now.ToShortDateString());

                StringBuilder csv = new StringBuilder();

                var events = db.Events.Include(p => p.productionUnit.owner).Include(p => p.eventType).Take(15000);

                events.OrderBy(m => m.date).ToList().ForEach(m =>
                {
                    csv.Append(m.Id + "; ");
                    csv.Append(m.date + "; ");
                    csv.Append(m.description + "; ");
                    csv.Append(m.isOpen + "; ");
                    csv.Append(m.createdBy + "; ");
                    csv.Append(m.productionUnit.info + "; ");
                    csv.Append(m.productionUnit.owner.pioneerCitizenName + "; ");
                    //csv.Append(StringToCSVCell(m.details) + "; ");

                    csv.Remove(csv.Length - 2, 1);
                    csv.Append("\r\n");
                }
                );
                
                return File(Encoding.Unicode.GetBytes(csv.ToString()), "text/csv", fileName);
            } 
            catch (Exception ex)
            {
                dbLog.Logs.Add(Log.CreateErrorLog("Error on File Generation", ex));
                dbLog.SaveChanges();
            }

            return null;

        }

        private string StringToCSVCell(string str)
        {
            if (str == null)
                return String.Empty;

            bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
            if (mustQuote)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("\"");
                foreach (char nextChar in str)
                {
                    sb.Append(nextChar);
                    if (nextChar == '"')
                        sb.Append("\"");
                }
                sb.Append("\"");
                return sb.ToString();
            }

            return str;
        }

        protected override void Dispose(bool disposing)
        {
                base.Dispose(disposing);
        }
    }
}
