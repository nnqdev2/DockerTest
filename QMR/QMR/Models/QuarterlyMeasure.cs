using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace QMR.Models
{
    public partial class QuarterlyMeasure
    {
        public int QuarterId { get; set; }
        public string Quarter { get; set; }
        public int  Year { get; set; }
        public bool Locked { get; set; }
        [NotMapped]
        [DisplayName("Roll Up")]
        public int RollUp { get; set; }
        [NotMapped]
        [DisplayName("Measure Counted")]
        public int MeasureCounted { get; set; }
        [NotMapped]
        [DisplayName("Measure Collected")]
        public int MeasureCollected { get; set; }
        [NotMapped]
        public bool IsCurrentReportingQuarter { get; set; }
    }
}
