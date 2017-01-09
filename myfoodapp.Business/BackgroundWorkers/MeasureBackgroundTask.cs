using GalaSoft.MvvmLight.Messaging;
using myfoodapp.Business.Clock;
using myfoodapp.Business.Sensor;
using myfoodapp.Business.Sensor.HumidityTemperature;
using myfoodapp.Common;
using myfoodapp.Model;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace myfoodapp.Business
{
    public sealed class MeasureBackgroundTask
    {
        private BackgroundWorker bw = new BackgroundWorker();
        private AtlasSensorManager sensorManager;
        private SigfoxInterfaceManager sigfoxManager;

        private UserSettingsModel userSettingsModel = UserSettingsModel.GetInstance;
        private LogModel logModel = LogModel.GetInstance;
        private DatabaseModel databaseModel = DatabaseModel.GetInstance;

        private int TICKSPERCYCLE = 600000;
        private int TICKSPERCYCLE_DIAGNOTIC_MODE = 60000;

        public event EventHandler Completed;

        private static MeasureBackgroundTask instance;

        public static MeasureBackgroundTask GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MeasureBackgroundTask();
                }
                return instance;
            }
        }

        private MeasureBackgroundTask()
        {
            logModel.AppendLog(Log.CreateLog("Measure Service starting...", Log.LogType.System));

            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += Bw_DoWork;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.ProgressChanged += Bw_ProgressChanged;
        }

        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Messenger.Default.Send(new RefreshDashboardMessage());
        }

        public void Run()
        {
            if (bw.IsBusy)
                return;

            logModel.AppendLog(Log.CreateLog("Measure Service running...", Log.LogType.System));
            bw.RunWorkerAsync();
        }

        public void Stop()
        {
            bw.CancelAsync();
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            logModel.AppendLog(Log.CreateLog("Measure Service stopping...", Log.LogType.System));
            Completed?.Invoke(this, EventArgs.Empty);
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var watch = Stopwatch.StartNew();

            var userSettings = new UserSettings();

            var taskUser = Task.Run(async () => { userSettings = await userSettingsModel.GetUserSettingsAsync(); });
            taskUser.Wait();

            if(userSettings.measureFrequency >= 60000)
            TICKSPERCYCLE = userSettings.measureFrequency;

            if (userSettings.isDiagnosticModeEnable)
                TICKSPERCYCLE = TICKSPERCYCLE_DIAGNOTIC_MODE;

            var clockManager = ClockManager.GetInstance;

            var captureDateTime = DateTime.Now;

            if (clockManager != null)
            {
                var taskClock = Task.Run(async () =>
                {
                    await clockManager.Connect();
                });
                taskClock.Wait();

                captureDateTime = clockManager.ReadDate();

                clockManager.Dispose();
            }

            sigfoxManager = SigfoxInterfaceManager.GetInstance;

            var taskSigfox = Task.Run(async () => { await sigfoxManager.InitSensors(); });
            taskSigfox.Wait();

            sensorManager = AtlasSensorManager.GetInstance;

            var taskSensor = Task.Run(async () => { await sensorManager.InitSensors(userSettings.isSleepModeEnable); });
            taskSensor.Wait();

            sensorManager.SetDebugLedMode(userSettings.isDebugLedEnable);

            var humTempManager = HumidityTemperatureManager.GetInstance;

            if (userSettings.isTempHumiditySensorEnable)
            {
                var taskHumManager = Task.Run(async () =>
                {
                    await humTempManager.Connect();
                });
                taskHumManager.Wait();
            }

            while (!bw.CancellationPending)
            {
                var elapsedMs = watch.ElapsedMilliseconds;

                try
                {
                    if (elapsedMs % TICKSPERCYCLE == 0)
                    {
                            captureDateTime = captureDateTime.AddMilliseconds(TICKSPERCYCLE);

                            TimeSpan t = TimeSpan.FromMilliseconds(elapsedMs);

                            string logDescription = string.Format("[ {0:d} - {0:t} ] Service running since {1:D2}d:{2:D2}h:{3:D2}m:{4:D2}s",
                                                    captureDateTime,
                                                    t.Days,
                                                    t.Hours,
                                                    t.Minutes,
                                                    t.Seconds,
                                                    t.Milliseconds);

                            logModel.AppendLog(Log.CreateLog(logDescription, Log.LogType.Information));

                            var watchMesures = Stopwatch.StartNew();     

                            if (sensorManager.isSensorOnline(SensorTypeEnum.waterTemperature))
                            {
                                if (userSettings.isDiagnosticModeEnable)
                                    logModel.AppendLog(Log.CreateLog("Water Temperature capturing", Log.LogType.Information));

                                decimal capturedValue = 0;
                                capturedValue = sensorManager.RecordSensorsMeasure(SensorTypeEnum.waterTemperature, userSettings.isSleepModeEnable);

                                if (capturedValue > -20 && capturedValue < 80)
                                {
                                    sensorManager.SetWaterTemperatureForPHSensor(capturedValue);

                                        var task = Task.Run(async () =>
                                        {
                                            await databaseModel.AddMesure(captureDateTime, capturedValue, SensorTypeEnum.waterTemperature);
                                        });
                                        task.Wait();
                                
                                        if (userSettings.isDiagnosticModeEnable)
                                        {
                                            logModel.AppendLog(Log.CreateLog(String.Format("Water Temperature captured : {0}", capturedValue), Log.LogType.Information));
                                            var status = sensorManager.GetSensorStatus(SensorTypeEnum.waterTemperature, userSettings.isSleepModeEnable);
                                            logModel.AppendLog(Log.CreateLog(String.Format("Water Temperature status : {0}", status), Log.LogType.System));
                                        }     
                                }
                                else
                                logModel.AppendLog(Log.CreateLog(String.Format("Water Temperature value out of range - {0}", capturedValue), Log.LogType.Warning));
                           }

                            if (sensorManager.isSensorOnline(SensorTypeEnum.ph))
                            {
                                if (userSettings.isDiagnosticModeEnable)
                                    logModel.AppendLog(Log.CreateLog("pH capturing", Log.LogType.Information));

                                decimal capturedValue = 0;
                                capturedValue = sensorManager.RecordPhMeasure(userSettings.isSleepModeEnable);

                                if (capturedValue > 1 && capturedValue < 12)
                                {
                                    var task = Task.Run(async () =>
                                    {
                                        await databaseModel.AddMesure(captureDateTime, capturedValue, SensorTypeEnum.ph);
                                    });
                                    task.Wait();

                                    if (userSettings.isDiagnosticModeEnable)
                                    {
                                        logModel.AppendLog(Log.CreateLog(String.Format("pH captured : {0}", capturedValue), Log.LogType.Information));
                                        var status = sensorManager.GetSensorStatus(SensorTypeEnum.ph, userSettings.isSleepModeEnable);
                                        logModel.AppendLog(Log.CreateLog(String.Format("pH status : {0}", status), Log.LogType.System));
                                    }              
                                }
                                else
                                logModel.AppendLog(Log.CreateLog(String.Format("pH value out of range - {0}", capturedValue), Log.LogType.Warning));
                            }

                            if (sensorManager.isSensorOnline(SensorTypeEnum.orp))
                            {
                                if (userSettings.isDiagnosticModeEnable)
                                   logModel.AppendLog(Log.CreateLog("ORP capturing", Log.LogType.Information));

                                decimal capturedValue = 0;
                                capturedValue = sensorManager.RecordSensorsMeasure(SensorTypeEnum.orp, userSettings.isSleepModeEnable);

                                if (capturedValue > 0 && capturedValue < 1500)
                                {
                                    var task = Task.Run(async () =>
                                    {
                                        await databaseModel.AddMesure(captureDateTime, capturedValue, SensorTypeEnum.orp);
                                    });
                                    task.Wait();

                                    if (userSettings.isDiagnosticModeEnable)
                                    {
                                        logModel.AppendLog(Log.CreateLog(String.Format("ORP captured : {0}", capturedValue), Log.LogType.Information));
                                        var status = sensorManager.GetSensorStatus(SensorTypeEnum.orp, userSettings.isSleepModeEnable);
                                        logModel.AppendLog(Log.CreateLog(String.Format("ORP status : {0}", status), Log.LogType.System));
                                    }
                                }
                                else
                                    logModel.AppendLog(Log.CreateLog(String.Format("ORP value out of range - {0}", capturedValue), Log.LogType.Warning));
                            }  

                            if (sensorManager.isSensorOnline(SensorTypeEnum.dissolvedOxygen))
                            {
                                if (userSettings.isDiagnosticModeEnable)
                                  logModel.AppendLog(Log.CreateLog("DO capturing", Log.LogType.Information));

                                decimal capturedValue = 0;
                                capturedValue = sensorManager.RecordSensorsMeasure(SensorTypeEnum.dissolvedOxygen, userSettings.isSleepModeEnable);

                                if (capturedValue > 0 && capturedValue < 100)
                                {
                                    var task = Task.Run(async () =>
                                    {
                                        await databaseModel.AddMesure(captureDateTime, capturedValue, SensorTypeEnum.dissolvedOxygen);
                                    });
                                    task.Wait();

                                    if (userSettings.isDiagnosticModeEnable)
                                    {
                                        logModel.AppendLog(Log.CreateLog(String.Format("DO captured : {0}", capturedValue), Log.LogType.Information));
                                        var status = sensorManager.GetSensorStatus(SensorTypeEnum.dissolvedOxygen, userSettings.isSleepModeEnable);
                                        logModel.AppendLog(Log.CreateLog(String.Format("DO status : {0}", status), Log.LogType.System));
                                    }
                                }
                                else
                                logModel.AppendLog(Log.CreateLog(String.Format("DO value out of range - {0}", capturedValue), Log.LogType.Warning));
                            }

                            if (userSettings.isTempHumiditySensorEnable)
                            {
                                try
                                {
                                    if (userSettings.isDiagnosticModeEnable)
                                        logModel.AppendLog(Log.CreateLog("Air Temperature capturing", Log.LogType.Information));

                                    decimal capturedValue = (decimal)humTempManager.Temperature;

                                if (capturedValue > 0 && capturedValue < 100)
                                {
                                    var taskTemp = Task.Run(async () =>
                                    {
                                        await databaseModel.AddMesure(captureDateTime, capturedValue, SensorTypeEnum.airTemperature);
                                    });
                                    taskTemp.Wait();

                                    if (userSettings.isDiagnosticModeEnable)
                                        logModel.AppendLog(Log.CreateLog(String.Format("Air Temperature captured : {0}", capturedValue), Log.LogType.Information));
                                }
                                else
                                    logModel.AppendLog(Log.CreateLog(String.Format("Air Temperature out of range - {0}", capturedValue), Log.LogType.Warning));
                                }
                                catch (Exception ex)
                                {
                                    logModel.AppendLog(Log.CreateErrorLog("Exception on Air Temperature Sensor", ex));
                                }

                                try
                                {
                                    if (userSettings.isDiagnosticModeEnable)
                                        logModel.AppendLog(Log.CreateLog("Humidity capturing", Log.LogType.Information));

                                    decimal capturedValue = (decimal)humTempManager.Humidity;

                                if (capturedValue > 0 && capturedValue < 100)
                                {
                                    var taskHum = Task.Run(async () =>
                                    {
                                        await databaseModel.AddMesure(captureDateTime, capturedValue, SensorTypeEnum.humidity);
                                    });
                                    taskHum.Wait();

                                    if (userSettings.isDiagnosticModeEnable)
                                        logModel.AppendLog(Log.CreateLog(String.Format("Air Humidity captured : {0}", capturedValue), Log.LogType.Information));
                                }
                                else
                                    logModel.AppendLog(Log.CreateLog(String.Format("Air Humidity out of range - {0}", capturedValue), Log.LogType.Warning));
                                }
                                catch (Exception ex)
                                {
                                logModel.AppendLog(Log.CreateErrorLog("Exception on Air Humidity Sensor", ex));
                                }
                            }

                           logModel.AppendLog(Log.CreateLog(String.Format("Measures captured in {0} sec.", watchMesures.ElapsedMilliseconds / 1000), Log.LogType.System));  
                        
                        if(userSettings.isSigFoxComEnable && sigfoxManager.isInitialized && TICKSPERCYCLE >= 600000)
                        {
                            watchMesures.Restart();

                            string sigFoxSignature = String.Empty;

                            var taskSig = Task.Run(async () =>
                            {
                                sigFoxSignature = await databaseModel.GetLastMesureSignature();
                            });
                            taskSig.Wait();

                            sigfoxManager.SendMessage(sigFoxSignature);

                            if (userSettings.isDiagnosticModeEnable)
                                sigfoxManager.Listen();

                            logModel.AppendLog(Log.CreateLog(String.Format("Data sent to Azure via Sigfox in {0} sec.", watchMesures.ElapsedMilliseconds / 1000), Log.LogType.System));
                        }

                        if (!userSettings.isSigFoxComEnable && NetworkInterface.GetIsNetworkAvailable())
                        {
                            string sigFoxSignature = String.Empty;

                            var taskSig = Task.Run(async () =>
                            {
                                sigFoxSignature = await databaseModel.GetLastMesureSignature();
                            });
                            taskSig.Wait();

                            using (var client = new HttpClient())
                            {
                                var request = new Message()
                                {
                                    content = sigFoxSignature,
                                    device = userSettings.productionSiteId,
                                    date = DateTime.Now.ToString(),
                                };

                                var taskWeb = Task.Run(async () =>
                                {
                                    try
                                    {
                                        var response = await client.PostAsync(userSettings.hubMessageAPI,
                                       new StringContent(JsonConvert.SerializeObject(request).ToString(),
                                       Encoding.UTF8, "application/json"));

                                        if (response.IsSuccessStatusCode)
                                        {
                                            logModel.AppendLog(Log.CreateLog("Measures sent to Azure via Internet", Log.LogType.Information));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        logModel.AppendLog(Log.CreateErrorLog("Exception on Measures to Azure", ex));
                                    }
                                   
                                });

                                taskWeb.Wait();           
                            }
                        }

                        bw.ReportProgress(33);         
                    }
                }
                catch (Exception ex)
                {
                    logModel.AppendLog(Log.CreateErrorLog("Exception on Measures", ex));
                }
            }
            watch.Stop();
        }
    }

}
