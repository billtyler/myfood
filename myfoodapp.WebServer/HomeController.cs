using myfoodapp.Business;
using myfoodapp.Business.Sensor;
using myfoodapp.Model;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myfoodapp.WebServer
{
    [RestController(InstanceCreationType.Singleton)]
    public class HomeController
    {
        [UriFormat("/measuresFile")]
        public IGetResponse GetMeasuresFile()
        {
            var databaseModel = DatabaseModel.GetInstance;
            var measures = new List<Measure>();

            var task = Task.Run(async () =>
            {
                measures = await databaseModel.GetLastWeeksMesures();
            });
            task.Wait();

            if (measures == null)
                return new GetResponse(
                  GetResponse.ResponseStatus.NotFound);

            var formatJson = measures.Select(f => new {
                date = String.Format("{0} {1}", f.captureDate.ToString("yyyy-MM-dd"), f.captureDate.ToString("HH:mm:ss")),
                sensor = f.sensor.name,
                value = f.value
            });

            return new GetResponse(
              GetResponse.ResponseStatus.OK, formatJson);
        }

        [UriFormat("/logsFile")]
        public IGetResponse GetLogsFile()
        {
            var databaseModel = LogModel.GetInstance;
            var logs = new List<Log>();

            var task = Task.Run(async () =>
            {
                logs = await databaseModel.GetLogsAsync();
            });
            task.Wait();

            if(logs == null)
                return new GetResponse(
                  GetResponse.ResponseStatus.NotFound);

            var formatJson = logs.Select(f => new {
                date = String.Format("{0} {1}", f.date.ToString("yyyy-MM-dd"), f.date.ToString("HH:mm:ss")),
                type = Enum.GetName(typeof(Log.LogType), f.type),
                description = f.description,
                stackCall = f.stackCall
            });

            return new GetResponse(
              GetResponse.ResponseStatus.OK, formatJson);
        }

        [UriFormat("/setPHTo7")]
        public IGetResponse SetPHTo7()
        {
            var logModel = LogModel.GetInstance;
            logModel.AppendLog(Log.CreateLog("Set pH to 7 started", Log.LogType.Information));

            var mesureBackgroundTask = MeasureBackgroundTask.GetInstance;
            mesureBackgroundTask.Completed += SetPhAtSevenBackgroundTask_Completed;
            mesureBackgroundTask.Stop();

            return new GetResponse(
              GetResponse.ResponseStatus.OK);
        }

        [UriFormat("/resetToFactory")]
        public IGetResponse ResetToFactory()
        {
            var logModel = LogModel.GetInstance;
            logModel.AppendLog(Log.CreateLog("Reset to factory started", Log.LogType.Information));

            var mesureBackgroundTask = MeasureBackgroundTask.GetInstance;
            mesureBackgroundTask.Completed += ResetHardwareBackgroundTask_Completed;
            mesureBackgroundTask.Stop();

            return new GetResponse(
              GetResponse.ResponseStatus.OK);
        }

        [UriFormat("/restartApp")]
        public IGetResponse RestartApp()
        {
            var logModel = LogModel.GetInstance;
            logModel.AppendLog(Log.CreateLog("Restart app started", Log.LogType.Information));

            var mesureBackgroundTask = MeasureBackgroundTask.GetInstance;
            mesureBackgroundTask.Completed += RestartAppBackgroundTask_Completed;
            mesureBackgroundTask.Stop();

            return new GetResponse(
              GetResponse.ResponseStatus.OK);
        }

        [UriFormat("/eraseLogs")]
        public IGetResponse EraseLogs()
        {
            var logModel = LogModel.GetInstance;
            logModel.AppendLog(Log.CreateLog("Erase logs started", Log.LogType.Information));

            var task = Task.Run(async () => { await logModel.ClearLog(); });
            task.Wait();

            logModel.AppendLog(Log.CreateLog("Erase logs ended", Log.LogType.Information));

            return new GetResponse(
              GetResponse.ResponseStatus.OK);
        }

        [UriFormat("/eraseMeasures")]
        public IGetResponse EraseMeasures()
        {
            var logModel = LogModel.GetInstance;
            var databaseModel = DatabaseModel.GetInstance;

            logModel.AppendLog(Log.CreateLog("Erase measures started", Log.LogType.Information));

            var task = Task.Run(async () => { await databaseModel.DeleteAllMesures(); });
            task.Wait();

            logModel.AppendLog(Log.CreateLog("Erase measures ended", Log.LogType.Information));

            return new GetResponse(
              GetResponse.ResponseStatus.OK);
        }

        [UriFormat("data/type/{sensorType}")]
        public IGetResponse GetMeasures(string sensorType)
        {
            SensorTypeEnum? currentSensorType = null;

            switch (sensorType)
            {
                case "waterTemp" :
                    currentSensorType = SensorTypeEnum.waterTemperature;
                    break;
                case "pH" :
                    currentSensorType = SensorTypeEnum.ph;
                    break;
                case "airTemp":
                    currentSensorType = SensorTypeEnum.airTemperature;
                    break;
                case "airHum":
                    currentSensorType = SensorTypeEnum.humidity;
                    break;
                default:
                    break;
            }

            var databaseModel = DatabaseModel.GetInstance;
            string response = String.Empty;

            var listMes = new List<Measure>();

            var task= Task.Run(async () =>
            {
                if(currentSensorType != null)
                 listMes = await databaseModel.GetLastWeeksMesures(currentSensorType.Value);
            });
            task.Wait();

            listMes.OrderBy(m => m.captureDate).ToList().ForEach(m => {
                response += (String.Format("{0}T{1},{2}\n", m.captureDate.ToString("yyyy-MM-dd"), m.captureDate.ToString("HH:mm:ss"), m.value));
                                 });

            return new GetResponse(
              GetResponse.ResponseStatus.OK, response);
        }

        private void ResetHardwareBackgroundTask_Completed(object sender, EventArgs e)
        {
            var logModel = LogModel.GetInstance;
            var mesureBackgroundTask = MeasureBackgroundTask.GetInstance;
            mesureBackgroundTask.Completed -= ResetHardwareBackgroundTask_Completed;

            try
            {
                AtlasSensorManager.GetInstance.ResetSensorsToFactory();
                mesureBackgroundTask.Run();
            }
            catch (Exception ex)
            {
                logModel.AppendLog(Log.CreateErrorLog("Exception on Reset Hardware", ex));
            }
            finally
            {
                logModel.AppendLog(Log.CreateLog("Hardware reset ended", Log.LogType.Information));
            }
        }

        private void SetPhAtSevenBackgroundTask_Completed(object sender, EventArgs e)
        {
            var logModel = LogModel.GetInstance;
            var mesureBackgroundTask = MeasureBackgroundTask.GetInstance;
            mesureBackgroundTask.Completed -= SetPhAtSevenBackgroundTask_Completed;

            try
            {
                AtlasSensorManager.GetInstance.SetCalibration(SensorTypeEnum.ph, AtlasSensorManager.CalibrationType.Mid);
                mesureBackgroundTask.Run();
            }
            catch (Exception ex)
            {
                logModel.AppendLog(Log.CreateErrorLog("Exception on Set pH to 7", ex));
            }
            finally
            {
                logModel.AppendLog(Log.CreateLog("Set pH to 7 ended", Log.LogType.Information));
            }
        }

        private void RestartAppBackgroundTask_Completed(object sender, EventArgs e)
        {
            var logModel = LogModel.GetInstance;
            var mesureBackgroundTask = MeasureBackgroundTask.GetInstance;
            mesureBackgroundTask.Completed -= RestartAppBackgroundTask_Completed;

            try
            {
                Windows.ApplicationModel.Core.CoreApplication.Exit();
            }
            catch (Exception ex)
            {
                logModel.AppendLog(Log.CreateErrorLog("Exception on Restart App", ex));
            }
            finally
            {
                logModel.AppendLog(Log.CreateLog("App restart ended", Log.LogType.Information));
            }
        }
    }
}
