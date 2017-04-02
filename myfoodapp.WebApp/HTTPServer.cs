using myfoodapp.Business;
using myfoodapp.Business.Sensor;
using myfoodapp.Common;
using myfoodapp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Streams;
using static myfoodapp.Business.Log;

namespace myfoodapp.WebApp
{
    public class HTTPServer
    {
        private StorageFolder folder = ApplicationData.Current.LocalFolder;
        private LogModel logModel = LogModel.GetInstance;

        public HTTPServer()
        {
            DefaultPage = File.ReadAllText("Assets\\Web\\index.html");
        }

        public void Initialise()
        {
            listener = new StreamSocketListener();

            var t = Task.Run(async() =>
            {
                // listen on port 80, this is the standard HTTP port (use a different port if you have a service already running on 80)
                await listener.BindServiceNameAsync("5000");

                listener.ConnectionReceived += (sender, args) =>
                {
                    // call the handle request function when a request comes in
                    HandleRequest(sender, args);
                };
            });

            t.Wait();
        }

        public async void HandleRequest(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            StringBuilder request = new StringBuilder();
            string responseHTML = "<html><body>ERROR</body></html>";

            // Handle a incoming request
            // First read the request
            using (IInputStream input = args.Socket.InputStream)
            {
                byte[] data = new byte[BufferSize];
                IBuffer buffer = data.AsBuffer();
                uint dataRead = BufferSize;
                while (dataRead == BufferSize)
                {
                    await input.ReadAsync(buffer, BufferSize, InputStreamOptions.Partial);
                    request.Append(Encoding.UTF8.GetString(data, 0, data.Length));
                    dataRead = buffer.Length;
                }
            }

            responseHTML = PrepareResponse(ParseRequest(request.ToString()));

            // Send a response back
            using (IOutputStream output = args.Socket.OutputStream)
            {
                using (Stream response = output.AsStreamForWrite())
                {
                    byte[] bodyArray = Encoding.UTF8.GetBytes(responseHTML);

                    var bodyStream = new MemoryStream(bodyArray);

                    var header = String.Empty;

                        header = "HTTP/1.1 200 OK\r\n" +
                          $"Content-Length: {bodyStream.Length}\r\n" +
                          "Connection: close\r\n\r\n";

                    if(request.ToString().Contains("csv"))
                    {
                        header = "HTTP/1.1 200 OK\r\n" +
                          $"Content-Length: {bodyStream.Length}\r\n" +
                          $"Content-Type: application/csv" +
                          "Connection: close\r\n\r\n";
                    }

                    byte[] headerArray = Encoding.UTF8.GetBytes(header);

                    await response.WriteAsync(headerArray, 0, headerArray.Length);
                    await bodyStream.CopyToAsync(response);
                    await response.FlushAsync();
                }
            }
        }

        private DatabaseModel databaseModel = DatabaseModel.GetInstance;
        public NotifyTaskCompletion<List<Measure>> Measures { get; private set; }

        private string PrepareResponse(string request)
        {
            try
            {
                string response = "ERROR";

                response = DefaultPage;

                if (request == "")
                {
                    response = DefaultPage;
                }
                else if (request.ToString().Contains("temp_water.txt"))
                {
                    var listMes = new List<Measure>();

                    var taskClock = Task.Run(async () =>
                    {
                        listMes = await databaseModel.GetLastWeeksMesures(SensorTypeEnum.waterTemperature);
                    });
                    taskClock.Wait();

                    response = "";

                    listMes.ForEach(m => response += String.Format("{0} {1},{2}\r\n", m.captureDate.ToString("d"), m.captureDate.ToString("t"), m.value));
                }
                else if (request.ToString().Contains("ph.txt"))
                {
                    var listMes = new List<Measure>();

                    var taskClock = Task.Run(async () =>
                    {
                        listMes = await databaseModel.GetLastWeeksMesures(SensorTypeEnum.ph);
                    });
                    taskClock.Wait();

                    response = "";

                    listMes.ForEach(m => response += String.Format("{0} {1},{2}\r\n", m.captureDate.ToString("d"), m.captureDate.ToString("t"), m.value));
                }
                else if (request.ToString().Contains("temp_air.txt"))
                {
                    var listMes = new List<Measure>();

                    var taskClock = Task.Run(async () =>
                    {
                        listMes = await databaseModel.GetLastWeeksMesures(SensorTypeEnum.airTemperature);
                    });
                    taskClock.Wait();

                    response = "";

                    listMes.ForEach(m => response += String.Format("{0} {1},{2}\r\n", m.captureDate.ToString("d"), m.captureDate.ToString("t"), m.value));
                }
                else if (request.ToString().Contains("rh_air.txt"))
                {
                    var listMes = new List<Measure>();

                    var taskClock = Task.Run(async () =>
                    {
                        listMes = await databaseModel.GetLastWeeksMesures(SensorTypeEnum.humidity);
                    });
                    taskClock.Wait();

                    response = "";

                    listMes.ForEach(m => response += String.Format("{0} {1},{2}\r\n", m.captureDate.ToString("d"), m.captureDate.ToString("t"), m.value));
                }
                else if (request.ToString().Contains("data.csv"))
                {
                    var listMes = new List<Measure>();

                    var taskClock = Task.Run(async () =>
                    {
                        listMes = await databaseModel.GetLastWeeksMesures();
                    });
                    taskClock.Wait();

                    response = "";

                    listMes.ForEach(m => response += String.Format("{0} {1};{2};{3}\r\n", m.captureDate.ToString("d"), m.captureDate.ToString("t"), m.value, m.sensor.Id));
                }
                else if (request.ToString().Contains("factory.html"))
                {
                    var mesureBackgroundTask = MeasureBackgroundTask.GetInstance;
                    mesureBackgroundTask.Completed += ResetHardwareBackgroundTask_Completed;
                    mesureBackgroundTask.Stop();

                    response = "<html><body>RESET SENSORS TO FACTORY SETTINGS DONE</body></html>";
                }
                else if (request.ToString().Contains("setphatseven.html"))
                {
                    var mesureBackgroundTask = MeasureBackgroundTask.GetInstance;
                    mesureBackgroundTask.Completed += SetPhAtSevenBackgroundTask_Completed;
                    mesureBackgroundTask.Stop();

                    response = "<html><body>SET PH SENSOR TO 7 DONE</body></html>";
                }
                else
                {
                    response = File.ReadAllText(String.Format("Assets\\Web\\{0}", request));
                }

                return response;
            }
            catch (Exception ex)
            {
                var mess = ex.Message;
            }

            return string.Empty;
          
        }

        private string ParseRequest(string buffer)
        {
            string request = "ERROR";

            string[] tokens = buffer.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            // ensure that this is a GET request
            if (tokens[0] == "GET")
            {
                request = tokens[1];
                request = request.Replace("/", "");
                request = request.ToLower();
            }

            return request;
        }

        private void ResetHardwareBackgroundTask_Completed(object sender, EventArgs e)
        {
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
                logModel.AppendLog(Log.CreateLog("Hardware reset ended", LogType.Information));
            }
        }

        private void SetPhAtSevenBackgroundTask_Completed(object sender, EventArgs e)
        {
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
                logModel.AppendLog(Log.CreateLog("Set pH to 7 ended", LogType.Information));
            }
        }

        private StreamSocketListener listener;                                               

        private const uint BufferSize = 8192; // this is the max size of the buffer in bytes 

        private string DefaultPage;
    }
}
