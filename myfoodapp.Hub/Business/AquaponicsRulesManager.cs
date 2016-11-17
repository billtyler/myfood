using Microsoft.ApplicationInsights.Extensibility.Implementation;
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
using System.Web.Script.Serialization;

namespace myfoodapp.Hub.Business
{
    public class AquaponicsRulesManager
    {
        public static void ValidateRules(GroupedMeasure currentMeasures, string productionUnitOwnerMail)
        {
            Evaluator evaluator = new Evaluator();
            var mailSubject = "Warning from myfood Hub";

            var data = File.ReadAllText("Content/AquaponicsRules.json");
            var rulesList = JsonConvert.DeserializeObject<List<Rule>>(data);

            var mailContent = new StringBuilder();

            foreach (var rule in rulesList)
            {
                try
                {
                    bool rslt = evaluator.Evaluate(rule.ruleEvaluator, currentMeasures);
                    if (rslt)
                    {
                        var bindingValue = currentMeasures.GetType().GetProperty(rule.bindingPropertyValue).GetValue(currentMeasures, null);
                        mailContent.AppendLine(String.Format(rule.warningContent, bindingValue) + "<br>");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                    //db.Logs.Add(Log.CreateLog(String.Format("Error with Rule Manager - {0}", rule.ruleEvaluator), Log.LogType.Warning));
                }
            }

            MailManager.SendMail(productionUnitOwnerMail, mailSubject, mailContent.ToString());
        }
    }
}