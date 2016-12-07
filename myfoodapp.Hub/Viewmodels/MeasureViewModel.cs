﻿using myfoodapp.Hub.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace myfoodapp.Hub.Models
{
    public class MeasureViewModel
    {
        public Int64 Id { get; set; }
        public DateTime captureDate { get; set; }
        public decimal value { get; set; }
        public SensorTypeViewModel sensor { get; set; }
        public int sensorId { get; set; }
        public int productionUnitId { get; set; }
        public ProductionUnitViewModel productionUnit { get; set; }
    }

    public class SensorTypeViewModel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime? lastCalibration { get; set; }
    }

    public class GroupedMeasure
    {
        public Int64 Id { get; set; }
        public DateTime captureDate { get; set; }
        public decimal pHvalue { get; set; }
        public decimal waterTempvalue { get; set; }
        public decimal ORPvalue { get; set; }
        public decimal DOvalue { get; set; }
        public decimal airTempvalue { get; set; }
        public decimal humidityvalue { get; set; }
        public string hydroponicTypeName { get; set; }
    }

    public class Rule
    {
        public string ruleEvaluator { get; set; }
        public string warningContent { get; set; }
        public string bindingPropertyValue { get; set; }
    }
}