using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myfoodapp.Hub.Business;
using myfoodapp.Hub.Models;
using System.Linq;

namespace myfoodapp.Hub.Tests
{
    [TestClass]
    public class RulesUnitTest
    {
        private string productionUnitOwnerMail = "foo@myfood.eu";
        private int productionUnitId;

        [TestInitialize()]
        private void Init()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            productionUnitId = db.ProductionUnits.FirstOrDefault().Id;
        }

        [TestMethod]
        public void TestMethodTempMiniCarp()
        {
            var currentMeasures = new GroupedMeasure();
            currentMeasures.waterTempvalue = -10;
            currentMeasures.hydroponicTypeName = "Aquaponics - Carp";

            Assert.IsFalse(AquaponicsRulesManager.ValidateRules(currentMeasures, productionUnitId, productionUnitOwnerMail));
        }

        [TestMethod]
        public void TestMethodTempMaxiCarp()
        {
            var currentMeasures = new GroupedMeasure();
            currentMeasures.waterTempvalue = 32;
            currentMeasures.hydroponicTypeName = "Aquaponics - Carp";

            Assert.IsFalse(AquaponicsRulesManager.ValidateRules(currentMeasures, productionUnitId, productionUnitOwnerMail));
        }

        [TestMethod]
        public void TestMethodHumidityTempUnder10()
        {
            var currentMeasures = new GroupedMeasure();
            currentMeasures.airTempvalue = 7;
            currentMeasures.humidityvalue = 90;
            currentMeasures.hydroponicTypeName = "Not applicable";

            Assert.IsFalse(AquaponicsRulesManager.ValidateRules(currentMeasures, productionUnitId, productionUnitOwnerMail));
        }

        [TestMethod]
        public void TestMethodHumidityTempUnder15()
        {
            var currentMeasures = new GroupedMeasure();
            currentMeasures.airTempvalue = 11;
            currentMeasures.humidityvalue = 95;
            currentMeasures.hydroponicTypeName = "Not applicable";

            Assert.IsFalse(AquaponicsRulesManager.ValidateRules(currentMeasures, productionUnitId, productionUnitOwnerMail));
        }

        [TestMethod]
        public void TestMethodHumidityTempUnder30()
        {
            var currentMeasures = new GroupedMeasure();
            currentMeasures.airTempvalue = 23;
            currentMeasures.humidityvalue = 99;
            currentMeasures.hydroponicTypeName = "Not applicable";

            Assert.IsFalse(AquaponicsRulesManager.ValidateRules(currentMeasures, productionUnitId, productionUnitOwnerMail));
        }
    }
}
