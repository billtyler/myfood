using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using myfoodapp.Hub.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using myfoodapp.Hub.Services;

namespace myfoodapp.Hub.Controllers
{
    public class LogController : Controller
    {
        // GET: Log
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var db = new ApplicationDbContext();

            return View(await db.Logs.ToListAsync());
        }

        [Authorize]
        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {
            var db = new ApplicationDbContext();
            var messageService = new MessageService(db);

            var logs = db.Logs.Take(200).OrderByDescending(l => l.date).ToList();

            return Json(logs.ToDataSourceResult(request));
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}
