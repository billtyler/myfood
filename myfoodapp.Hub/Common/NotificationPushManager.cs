using myfoodapp.Hub.Models;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace myfoodapp.Hub.Common
{
    public static class NotificationPushManager
    {
        private static string OneSignalAPIKey = WebConfigurationManager.AppSettings["oneSignalAPIKey"];
        private static string OneSignalAPIId = WebConfigurationManager.AppSettings["oneSignalAPIId"];
        private static string WebAppUrl = "https://hub.myfood.eu/";

        public static void PioneerUnitEventMessage(string title, string message, int currentProductionUnitOwnerId, string currentProductionOwnerKey)
        {
            var dbLog = new ApplicationDbContext();
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", String.Format("Basic {0}", OneSignalAPIKey));

            var serializer = new JavaScriptSerializer();

            var obj = new
            {
                app_id = OneSignalAPIId,
                headings = new { en = title },
                contents = new { en = message },
                include_player_ids = new string[] { currentProductionOwnerKey },
                url = String.Format(WebAppUrl + @"ProductionUnits/Events/{0}", currentProductionUnitOwnerId),
                chrome_web_icon = WebAppUrl + "Content/favicon.ico"
            };

            var param = serializer.Serialize(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                dbLog.Logs.Add(Log.CreateErrorLog(String.Format("Error with Onesignal Push Notification"), ex));
                dbLog.SaveChanges();
            }

        }

        public static void PioneerUnitOwnerFeelingMessage(ProductionUnit currentProductionUnit)
        {
            var dbLog = new ApplicationDbContext();
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", String.Format("Basic {0}", OneSignalAPIKey));

            var serializer = new JavaScriptSerializer();

            var obj = new
            {
                app_id = OneSignalAPIId,
                headings = new { en = "Tell me, how do you feel today?" },
                contents = new { en = "from your Production Unit | Smart Greenhouse" },
                chrome_web_icon = WebAppUrl + "Content/favicon.ico",
                chrome_web_image = WebAppUrl + String.Format("Content/Pictures/Sites/{0}", currentProductionUnit.picturePath),
                include_player_ids = new string[] { currentProductionUnit.owner.notificationPushKey },
                url = String.Format(WebAppUrl + @"ProductionUnits/Events/{0}&_osp=do_not_open", currentProductionUnit.owner.Id),
                android_background_layout = new { image = WebAppUrl + "Content/miniLogoWhite.jpg", headings_color = "FFFF0000", contents_color = "FF00FF00" },
                web_buttons = new[] {
                                    new { id = "happy-button", text = "Happy", icon = WebAppUrl + "Content/Pictures/Feelings/happy.png", url = WebAppUrl + String.Format("api/push?productionUnitId={0}&eventTypeId=7&eventTypeItemId=51", currentProductionUnit.Id) },
                                    new { id = "needHelp-button", text = "Need help", icon = WebAppUrl + "Content/Pictures/Feelings/needHelp.png", url = WebAppUrl + String.Format("api/push?productionUnitId={0}&eventTypeId=7&eventTypeItemId=55", currentProductionUnit.Id) }
                                }
            };

            var param = serializer.Serialize(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                dbLog.Logs.Add(Log.CreateErrorLog(String.Format("Error with Onesignal Push Notification"), ex));
                dbLog.SaveChanges();
            }

        }
    }

}