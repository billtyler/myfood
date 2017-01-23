using myfoodapp.Hub.Common;
using myfoodapp.Hub.Models;
using Newtonsoft.Json;
using SimpleExpressionEvaluator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace myfoodapp.Hub.Business
{
    public class AquaponicsRulesManager
    {
        public static bool ValidateRules(GroupedMeasure currentMeasures, int productionUnitId, string productionUnitOwnerMail)
        {
            Evaluator evaluator = new Evaluator();
            bool isValid = true;
            var mailSubject = "Warning from myfood Hub";

            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationDbContext dbLog = new ApplicationDbContext();

            var data = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/AquaponicsRules.json"));
            var rulesList = JsonConvert.DeserializeObject<List<Rule>>(data);

            var currentProductionUnit = db.ProductionUnits.Where(p => p.Id == productionUnitId).FirstOrDefault();

            var mailContent = new StringBuilder();

            foreach (var rule in rulesList)
            {
                try
                {
                    bool rslt = evaluator.Evaluate(rule.ruleEvaluator, currentMeasures);
                    if (rslt)
                    {
                        var bindingValue = currentMeasures.GetType().GetProperty(rule.bindingPropertyValue).GetValue(currentMeasures, null);
                        var message = String.Format(rule.warningContent, bindingValue);
                        mailContent.AppendLine(message + "<br>");

                        if(currentProductionUnit != null)
                        {
                            db.Events.Add(new Event() { date = DateTime.Now, description = message, productionUnit = currentProductionUnit });
                            db.SaveChanges();
                        }

                        isValid = false;
                    }
                }
                catch (Exception ex)
                {
                    dbLog.Logs.Add(Log.CreateErrorLog(String.Format("Error with Rule Manager Evaluator"), ex));
                    dbLog.SaveChanges();
                }
            }

            try
            {
                MailManager.SendMail(productionUnitOwnerMail, mailSubject, mailContent.ToString());
            }
            catch (Exception ex)
            {
                dbLog.Logs.Add(Log.CreateErrorLog(String.Format("Error with Rule Manager - Mail Sending"), ex));
                dbLog.SaveChanges();
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                dbLog.Logs.Add(Log.CreateErrorLog(String.Format("Error with Rule Manager - Save Events"), ex));
                dbLog.SaveChanges();
            }

            return isValid;
        }
    }
}