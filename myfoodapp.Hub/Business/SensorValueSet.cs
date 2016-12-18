using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace myfoodapp.Hub.Business
{
    public class SensorValueSet
    {
        public decimal CurrentValue { get; set; }
        public string CurrentCaptureTime { get; set; }
        public decimal AverageHourValue { get; set; }
        public decimal AverageDayValue { get; set; }
        public string LastDayCaptureTime { get; set; }
    }
}