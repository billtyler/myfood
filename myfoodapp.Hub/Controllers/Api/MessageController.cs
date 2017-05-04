using myfoodapp.Hub.Business;
using myfoodapp.Hub.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data.Entity;
using System.Web.Http;

namespace myfoodapp.Hub.Controllers.Api
{
    public class MessagesController : ApiController
    {
        // POST api/<controller>
        //[Authorize]
        public void Post([FromBody]JObject data)
        {
            var content = data["content"].ToObject<string>();
            var device = data["device"].ToObject<string>();

            var db = new ApplicationDbContext();
            var dbLog = new ApplicationDbContext();

            var date = DateTime.Now;

            try
            {
                var measureType = db.MessageTypes.Where(m => m.Id == 1).FirstOrDefault();

                db.Messages.Add(new Message() { content = content, device = device, date = date, messageType = measureType });
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                dbLog.Logs.Add(Log.CreateErrorLog("Error on Message from Sigfox", ex));
                dbLog.SaveChanges();
            }

            try
            {
                var currentProductionUnit = db.ProductionUnits.Include(p => p.owner.user)
                                                              .Include(p => p.hydroponicType)
                                                              .Include(p => p.productionUnitStatus)
                                                              .Include(p => p.productionUnitType)
                                                              .Where(p => p.reference == device).FirstOrDefault();
                
                if (currentProductionUnit == null)
                {
                    db.Logs.Add(Log.CreateLog(String.Format("Production Unit not found - {0}", device), Log.LogType.Warning));
                    db.SaveChanges();
                }

                currentProductionUnit.lastMeasureReceived = date;

                var productionUnitOwnerMail = currentProductionUnit.owner.user.Email;

                var currentMeasures = new GroupedMeasure();
                currentMeasures.hydroponicTypeName = currentProductionUnit.hydroponicType.name;

                var phSensor = db.SensorTypes.Where(s => s.Id == 1).FirstOrDefault();
                var waterTemperatureSensor = db.SensorTypes.Where(s => s.Id == 2).FirstOrDefault();
                var dissolvedOxySensor = db.SensorTypes.Where(s => s.Id == 3).FirstOrDefault();
                var ORPSensor = db.SensorTypes.Where(s => s.Id == 4).FirstOrDefault();
                var airTemperatureSensor = db.SensorTypes.Where(s => s.Id == 5).FirstOrDefault();
                var airHumidity = db.SensorTypes.Where(s => s.Id == 6).FirstOrDefault();

                var phContent = content.Substring(0, 4).Insert(3,".");
                var waterTempContent = content.Substring(4, 4).Insert(3, ".");
                var dissolvedOxyContent  = content.Substring(8, 4).Insert(3, ".");
                var orpContent = content.Substring(12, 4).Insert(3, ".");
                var airTempContent = content.Substring(16, 4).Insert(3, ".");
                var airHumidityContent = content.Substring(20, 4).Insert(3, ".");

                if(!phContent.Contains("a"))
                {
                    decimal pHvalue = 0;
                    if(decimal.TryParse(phContent, out pHvalue))
                    {
                        currentMeasures.pHvalue = pHvalue;

                        db.Measures.Add(new Measure() { captureDate = date, productionUnit = currentProductionUnit, sensor = phSensor, value = pHvalue });
                    }       
                }

                if (!waterTempContent.Contains("a"))
                {
                    decimal waterTempvalue = 0;
                    if (decimal.TryParse(waterTempContent, out waterTempvalue))
                    {
                        currentMeasures.waterTempvalue = waterTempvalue;

                        db.Measures.Add(new Measure() { captureDate = date, productionUnit = currentProductionUnit, sensor = waterTemperatureSensor, value = waterTempvalue });
                    }          
                }

                if (!dissolvedOxyContent.Contains("a"))
                {
                    decimal DOvalue = 0;
                    if (decimal.TryParse(dissolvedOxyContent, out DOvalue))
                    {
                        currentMeasures.DOvalue = DOvalue;

                        db.Measures.Add(new Measure() { captureDate = date, productionUnit = currentProductionUnit, sensor = dissolvedOxySensor, value = DOvalue });
                    }
                }

                if (!orpContent.Contains("a"))
                {
                    decimal ORPvalue = 0;
                    if (decimal.TryParse(orpContent, out ORPvalue))
                    {
                        currentMeasures.ORPvalue = ORPvalue;

                        db.Measures.Add(new Measure() { captureDate = date, productionUnit = currentProductionUnit, sensor = ORPSensor, value = ORPvalue });
                    }
                }

                if (!airTempContent.Contains("a"))
                {
                    decimal airTempvalue = 0;
                    if (decimal.TryParse(airTempContent, out airTempvalue))
                    {
                        currentMeasures.airTempvalue = airTempvalue;

                        db.Measures.Add(new Measure() { captureDate = date, productionUnit = currentProductionUnit, sensor = airTemperatureSensor, value = airTempvalue });
                    }
                }

                if (!airHumidityContent.Contains("a"))
                {
                    decimal humidityvalue = 0;
                    if (decimal.TryParse(airHumidityContent, out humidityvalue))
                    {
                        currentMeasures.humidityvalue = humidityvalue;

                        db.Measures.Add(new Measure() { captureDate = date, productionUnit = currentProductionUnit, sensor = airHumidity, value = humidityvalue });
                    }                     
                }

                db.SaveChanges();

                DateTime lastDay = date.AddDays(-1);

                var currentLastDayPHValue = db.Measures.Where(m => m.captureDate > lastDay &&
                                                   m.productionUnit.Id == currentProductionUnit.Id &&
                                                   m.sensor.Id == phSensor.Id).OrderBy(m => m.Id).FirstOrDefault();

                if (currentLastDayPHValue == null)
                    currentMeasures.lastDayPHvariation = 0;
                else
                    currentMeasures.lastDayPHvariation = Math.Abs(currentLastDayPHValue.value - currentMeasures.pHvalue);

                AquaponicsRulesManager.ValidateRules(currentMeasures, currentProductionUnit.Id, productionUnitOwnerMail); 
            }
            catch (Exception ex)
            {
                dbLog.Logs.Add(Log.CreateErrorLog("Error on Convert Message into Measure", ex));
                dbLog.SaveChanges();
            }
        }
    }
}