using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace myfoodapp.Hub.Models
{
    public class ProductionUnitViewModel
    {
        public int Id { get; set; }
        public DateTime? startDate { get; set; }
        public string reference { get; set; }
        [UIHint("GridForeignKey")]
        public int productionUnitTypeId { get; set; }
        public ProductionUnitTypeViewModel productionUnitType { get; set; }
        [UIHint("GridForeignKey")]
        public int hydroponicTypeId { get; set; }
        [Display(Name = "[[[hydroponicType]]]")]
        public HydroponicTypeViewModel hydroponicType { get; set; }
        [UIHint("GridForeignKey")]
        public int productionUnitStatusId { get; set; }
        public ProductionUnitStatusViewModel productionUnitStatus { get; set; }
        public double locationLatitude { get; set; }
        public double locationLongitude { get; set; }
        [Display(Name = "[[[info]]]")]
        public String info { get; set; }
        public String version { get; set; }
        public List<Option> options { get; set; }
        [UIHint("GridForeignKey")]
        public int ownerId { get; set; }
        [Display(Name = "[[[owner]]]")]
        public OwnerViewModel owner { get; set; }
        public string picturePath { get; set; }
        public DateTime? lastMeasureReceived { get; set; }
        
    }
}