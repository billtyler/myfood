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
        public static bool ValidateRules(GroupedMeasure currentMeasures, int productionUnitId)
        {
            Evaluator evaluator = new Evaluator();
            bool isValid = true;

            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationDbContext dbLog = new ApplicationDbContext();

            var data = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Content/AquaponicsRules.json"));
            var rulesList = JsonConvert.DeserializeObject<List<Rule>>(data);

            var currentProductionUnit = db.ProductionUnits.Where(p => p.Id == productionUnitId).FirstOrDefault();
            var warningEventType = db.EventTypes.Where(p => p.Id == 1).FirstOrDefault();

            foreach (var rule in rulesList)
            {
                try
                {
                    bool rslt = evaluator.Evaluate(rule.ruleEvaluator, currentMeasures);
                    if (rslt)
                    {
                        var bindingValue = currentMeasures.GetType().GetProperty(rule.bindingPropertyValue).GetValue(currentMeasures, null);
                        var message = String.Format(rule.warningContent, bindingValue);

                        if(currentProductionUnit != null)
                        {
                            db.Events.Add(new Event() { date = DateTime.Now, description = message, productionUnit = currentProductionUnit, eventType = warningEventType });
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

    public class GroupedMeasure
    {
        public Int64 Id { get; set; }
        public DateTime captureDate { get; set; }
        public decimal pHvalue { get; set; }
        public decimal lastDayMinpHvalue { get; set; }
        public decimal lastDayMaxpHvalue { get; set; }
        public decimal lastDayPHvariation { get; set; }
        public decimal threeLastDayPHvariation { get; set; }
        public decimal waterTempvalue { get; set; }
        public decimal lastDayMinWaterTempvalue { get; set; }
        public decimal lastDayMaxWaterTempvalue { get; set; }
        public decimal ORPvalue { get; set; }
        public decimal DOvalue { get; set; }
        public decimal airTempvalue { get; set; }
        public decimal lastDayMinAirTempvalue { get; set; }
        public decimal lastDayMaxAirTempvalue { get; set; }
        public decimal lastDayMeanAirTempvalue { get; set; }
        public decimal humidityvalue { get; set; }
        public decimal lastDayMaxHumidityvalue { get; set; }
        public string hydroponicTypeName { get; set; }
    }
}