using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myfoodapp.Hub.Business;
using myfoodapp.Hub.Models;

namespace myfoodapp.Hub.Tests
{
    [TestClass]
    public class RulesUnitTest
    {
        string productionUnitOwnerMail = "foo@myfood.eu";
        [TestMethod]
        public void TestMethod1()
        {
            var currentMeasures = new GroupedMeasure();
            currentMeasures.pHvalue = 2;
            currentMeasures.waterTempvalue = -50;
            currentMeasures.ORPvalue = 900;
            currentMeasures.DOvalue = 0;
            currentMeasures.airTempvalue = -7;
            currentMeasures.humidityvalue = 10;

            AquaponicsRulesManager.ValidateRules(currentMeasures, productionUnitOwnerMail);
        }

        [TestMethod]
        public void TestMethodHumidityTempUnder10()
        {
            var currentMeasures = new GroupedMeasure();
            currentMeasures.pHvalue = 7;
            currentMeasures.waterTempvalue = 15;
            currentMeasures.ORPvalue = 350;
            currentMeasures.DOvalue = 10;
            currentMeasures.airTempvalue = 7;
            currentMeasures.humidityvalue = 90;

            AquaponicsRulesManager.ValidateRules(currentMeasures, productionUnitOwnerMail);
        }

        [TestMethod]
        public void TestMethodHumidityTempUnder15()
        {
            var currentMeasures = new GroupedMeasure();
            currentMeasures.pHvalue = 7;
            currentMeasures.waterTempvalue = 15;
            currentMeasures.ORPvalue = 350;
            currentMeasures.DOvalue = 10;
            currentMeasures.airTempvalue = 11;
            currentMeasures.humidityvalue = 95;

            AquaponicsRulesManager.ValidateRules(currentMeasures, productionUnitOwnerMail);
        }

        [TestMethod]
        public void TestMethodHumidityTempUnder30()
        {
            var currentMeasures = new GroupedMeasure();
            currentMeasures.pHvalue = 7;
            currentMeasures.waterTempvalue = 15;
            currentMeasures.ORPvalue = 350;
            currentMeasures.DOvalue = 10;
            currentMeasures.airTempvalue = 23;
            currentMeasures.humidityvalue = 99;

            AquaponicsRulesManager.ValidateRules(currentMeasures, productionUnitOwnerMail);
        }
    }
}
