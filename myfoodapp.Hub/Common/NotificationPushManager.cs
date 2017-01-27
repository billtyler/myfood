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

        public static void PushMessage(string title, string message, int currentProductionUnitOwnerId, string currentProductionOwnerKey)
        {
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", String.Format("Basic {0}", OneSignalAPIKey));

            var serializer = new JavaScriptSerializer();

            var obj = new
            {
                app_id = "674761cf-c65d-4eaa-adc7-960d13fe4e54",
                headings = new { en = title },
                contents = new { en = message },
                include_player_ids = new string[] { currentProductionOwnerKey },
                url = String.Format(@"https://hub.myfood.eu/ProductionUnits/Events/{0}", currentProductionUnitOwnerId),
                android_background_layout = new { image = "https://hub.myfood.eu/Content/miniLogoWhite.jpg", headings_color = "FFFF0000", contents_color = "FF00FF00"}
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
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }

            System.Diagnostics.Debug.WriteLine(responseContent);

        }
    }

}