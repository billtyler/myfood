using myfoodapp.Hub.Common;
using myfoodapp.Hub.Models;
using Newtonsoft.Json;
using SimpleExpressionEvaluator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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

            var data = File.ReadAllText("Content/AquaponicsRules.json");
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
                        db.Events.Add(new Event() { date = DateTime.Now, description = message, productionUnit = currentProductionUnit });

                        isValid = false;
                    }
                }
                catch (Exception ex)
                {
                    db.Logs.Add(Log.CreateErrorLog(String.Format("Error with Rule Manager Evaluator"), ex));
                    db.SaveChanges();
                }
            }

            try
            {
                MailManager.SendMail(productionUnitOwnerMail, mailSubject, mailContent.ToString());
            }
            catch (Exception ex)
            {
                db.Logs.Add(Log.CreateErrorLog(String.Format("Error with Rule Manager - Mail Sending"), ex));
                db.SaveChanges();
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                db.Logs.Add(Log.CreateErrorLog(String.Format("Error with Rule Manager - Save Events"), ex));
                db.SaveChanges();
            }

            return isValid;
        }
    }
}