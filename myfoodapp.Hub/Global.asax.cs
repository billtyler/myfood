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
using i18n;
using System.Threading;
using System.Globalization;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Web;

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

            //Change from the default of 'en'.
            i18n.LocalizedApplication.Current.DefaultLanguage = "en";

            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");

            // Change from the default of 'i18n.langtag'.
            //i18n.LocalizedApplication.Current.CookieName = "i18n_langtag";

            // Change from the of temporary redirects during URL localization
            //i18n.LocalizedApplication.Current.PermanentRedirects = true;

            // Change the URL localization scheme from Scheme1.
            i18n.UrlLocalizer.UrlLocalizationScheme = i18n.UrlLocalizationScheme.Void;

            // Specifies a custom method called after a nugget has been translated
            // that allows the resulting message to be modified, for instance according to content type.
            i18n.LocalizedApplication.Current.TweakMessageTranslation = delegate (System.Web.HttpContextBase context, i18n.Helpers.Nugget nugget, i18n.LanguageTag langtag, string message)
            {
                switch (context.Response.ContentType)
                {
                    case "text/html":
                        return message.Replace("\'", "&apos;");
                }
                return message;
            };

            // Blacklist certain URLs from being 'localized' via a callback.
            i18n.UrlLocalizer.IncomingUrlFilters += delegate (Uri url) {
                if (url.LocalPath.EndsWith("sitemap.xml", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                return true;
            };

            System.Timers.Timer timer = new System.Timers.Timer(TimerIntervalInMilliseconds);
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

        protected void Application_AuthenticateRequest()
        {
            if (HttpContext.Current.User != null)
            {
                var db = new ApplicationDbContext();
                var currentUser = HttpContext.Current.User.Identity.GetUserName();

                var currentProductionUnitOwner = db.ProductionUnitOwners
                                                               .Include(p => p.user)
                                                               .Include(p => p.language)
                                                               .Where(p => p.user.UserName == currentUser).FirstOrDefault();

                i18n.HttpContextExtensions.SetPrincipalAppLanguageForRequest(
                System.Web.HttpContext.Current,
                i18n.LanguageHelpers.GetMatchingAppLanguage(currentProductionUnitOwner.language.description)
             );
            }
            else if (HttpContext.Current.User == null && (HttpContext.Current.Request.UrlReferrer != null || !String.IsNullOrEmpty(Request.QueryString["lang"])))
            {
                if (Request.QueryString["lang"] == "fr" || Request.QueryString["lang"] == "en" || Request.QueryString["lang"] == "lu" || Request.QueryString["lang"] == "fl")
                {
                    i18n.HttpContextExtensions.SetPrincipalAppLanguageForRequest(
                    System.Web.HttpContext.Current,
                    i18n.LanguageHelpers.GetMatchingAppLanguage(Request.QueryString["lang"])
                    );
                }
                else if ((HttpUtility.ParseQueryString(HttpContext.Current.Request.UrlReferrer.Query).Get("lang") != String.Empty))
                {
                    var strLang = HttpUtility.ParseQueryString(HttpContext.Current.Request.UrlReferrer.Query).Get("lang");

                    if (strLang == "fr" || strLang == "en" || strLang == "lu" || strLang == "fl")
                    {
                        i18n.HttpContextExtensions.SetPrincipalAppLanguageForRequest(
                        System.Web.HttpContext.Current,
                        i18n.LanguageHelpers.GetMatchingAppLanguage(strLang)
                        );
                    }
                }
            }
        }

        private static void SendDailyMessage()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var yesterdayDate = DateTime.Now.AddDays(-1);

            var todayEvents = db.Events.Include(e => e.productionUnit.owner.user).Where(ev => ev.date > yesterdayDate).ToList();

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
            }
        }

    }
}
