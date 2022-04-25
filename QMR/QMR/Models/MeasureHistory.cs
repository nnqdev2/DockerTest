using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QMR.Models
{
    public partial class MeasureHistory 
    {
        public int MeasureId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MeasureGroupId { get; set; }
        [DisplayName("RollUp")]
        public bool RollUpMeasure { get; set; } = false;
        [NotMapped]
        public string RollUpMeasureString { get; set; }
        public double Target { get; set; }
        public double GreenRange { get; set; }
        [NotMapped]
        public float YellowRange { get; set; }
        public double RedRange { get; set; }

        [NotMapped]
        public virtual MeasureType MeasureType { get; set; }

        public bool Status { get; set; }

        public string MeasureOwner { get; set; }

        public string DataSteward { get; set; }
        public int SeasonId { get; set; }
        public virtual Season Season { get; set; }
        [DisplayName("Unit of Measure")]
        public string MeasureUnit { get; set; }

        [DisplayName("Reporting")]
        public bool Reporting { get; set; }
        [NotMapped]
        public string ReportingString { get; set; }
        //public bool RollUpMeasureBool { get; set; }
        //public int MeasureGroupId { get; set; }
        //public int SeasonId { get; set; }

        public int CurrentValueOptionId { get; set; }
        [NotMapped]
        [DisplayName("Current Value Color")]
        public string CurrentValueOptionDescription { get; set; }
        [NotMapped]
        [DisplayName("Group")]
        public string measureGroupName { get; set; }
        [NotMapped]
        [DisplayName("Type")]
        public string MeasureTypeDescription { get; set; }
        [NotMapped] 
        public string SeasonName { get; set; }
        [DisplayName("Valid From")]
        public DateTime SysStartTime { get; set; }
        [DisplayName("Valid To")]
        public DateTime SysEndTime { get; set; }
        [DisplayName("Updated By")]
        public string LastUpdatedBy { get; set; }
        public string Comment { get; set; }
    }
}
