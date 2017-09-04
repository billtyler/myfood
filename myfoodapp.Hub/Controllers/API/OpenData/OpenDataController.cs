using myfoodapp.Hub.Business;
using myfoodapp.Hub.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Data.Entity;
using System.Web.Http;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
using myfoodapp.Hub.Services;
using myfoodapp.Hub.Services.OpenData;
using myfoodapp.Hub.Models.OpenData;

namespace myfoodapp.Hub.Controllers.Api
{
    public class OpendDataController : ApiController
    {
        [HttpGet]
        [Route("opendata/productionunits/")]
        public List<OpenProductionUnitViewModel> GetAll()
        {
            var db = new ApplicationDbContext();
            var openDataService = new OpenDataService(db);

            var all = openDataService.GetAll();

            return all.ToList();
        }

        [HttpGet]
        [Route("opendata/productionunits/{Id:int}")]
        public List<OpenProductionUnitViewModel> GetById(int Id)
        {
            var db = new ApplicationDbContext();
            var openDataService = new OpenDataService(db);

            var one = openDataService.One(Id);

            return one.ToList();
        }

        [HttpGet]
        [Route("opendata/productionunits/{Id:int}/Measures")]
        public List<OpenMeasureViewModel> GetMeasures(int Id)
        {
            var db = new ApplicationDbContext();
            var openDataService = new OpenDataService(db);

            var measures = openDataService.GetMeasures(Id);

            return measures.ToList();
        }
    }
}