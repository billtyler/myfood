using System;
using System.Linq;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Timers;
using myfoodapp.Hub.Models;
using System.Text;
using myfoodapp.Hub.Common;
using myfoodapp.Hub.Migrations;
using System.Data.Entity.Migrations;

namespace myfoodapp.Hub
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static double TimerIntervalInMilliseconds = Convert.ToDouble(WebConfigurationManager.AppSettings["timerIntervalInMilliseconds"]);

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // Disable the HTTP Header X-Frame-Options: SAMEORIGIN
            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;

            GlobalConfiguration.Configuration.MessageHandlers.Add(new AuthorizationHeaderHandler());

            Timer timer = new Timer(TimerIntervalInMilliseconds);
            timer.Enabled = true;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }
        static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime MyScheduledRunTime = DateTime.Parse(WebConfigurationManager.AppSettings["timerStartTime"]);
            DateTime CurrentSystemTime = DateTime.Now;
            DateTime LatestRunTime = MyScheduledRunTime.AddMilliseconds(TimerIntervalInMilliseconds);
            if ((CurrentSystemTime.CompareTo(MyScheduledRunTime) >= 0) && (CurrentSystemTime.CompareTo(LatestRunTime) <= 0))
            {
                SendDailyMessage();
            }
        }

        private static void SendDailyMessage()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var yesterdayDate = DateTime.Now.AddDays(-1);

            var todayEvents = db.Events.Include("productionUnit.owner.user").Where(ev => ev.date > yesterdayDate).ToList();

            var groupedEvents = todayEvents.GroupBy(ev => ev.productionUnit);

            foreach (var item in groupedEvents)
            {
                var productionUnitOwnerMail = item.Key.owner.user.Email;
                var productionUnitOwnerName = item.Key.owner.pioneerCitizenName;
                var notificationPushKey = item.Key.owner.notificationPushKey;
                var productionUnitId = item.Key.Id;
                var productionUnitInfo = item.Key.info;

                var mailSubject = String.Format("Daily Events on your myfood Unit {0}", productionUnitInfo);
                var mailContent = new StringBuilder();

                NotificationPushManager.PushMessage(mailSubject, "Click to see your production unit's status", productionUnitId, notificationPushKey);

                //mailContent.AppendLine(String.Format("Hello {0}, your unit reported few events today", productionUnitOwnerName));
                //mailContent.AppendLine(String.Format(@"Get more details at https://hub.myfood.eu/ProductionUnits/Events/{0}", productionUnitId));

                //var eventMessageList = item.ToList();

                //try
                //{
                //    MailManager.SendMail(productionUnitOwnerMail, mailSubject, mailContent.ToString());
                //}
                //catch (Exception ex)
                //{
                //    db.Logs.Add(Log.CreateErrorLog(String.Format("Error with Rule Manager - Mail Sending"), ex));
                //    db.SaveChanges();
                //}
            }
        }
             
    }
}
