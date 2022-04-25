using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace QMR.Models
{
    public partial class QuarterlyReviewDetails
    {
        public int Id { get; set; }
        [NotMapped]
        public string QuarterDetail { get; set; }
        
        public int QuarterId{ get; set; }
        public int MeasureId { get; set; }
        [NotMapped]
        public string Title { get; set; }
        [NotMapped]
        public string Description { get; set; }
        [NotMapped]
        [DisplayName("RollUp")]
        public string RollUpMeasure { get; set; }
        [NotMapped]
        public double Target { get; set; }
        [NotMapped]
        public double GreenRange { get; set; }
        [NotMapped]
        public double YellowRange { get; set; }
        [NotMapped]
        public double RedRange { get; set; }
        [NotMapped]
        public string measureGroupName { get; set; }
        [NotMapped]
        public string MeasureTypeDescription { get; set; }
        [NotMapped]
        public bool? Status { get; set; }
        [NotMapped]
        public string DataSteward { get; set; }
        [NotMapped]
        public string MeasureOwner { get; set; }

        public double CurrentValue { get; set; }
        public int ActionId { get; set; }
        [NotMapped]
        [DisplayName("Action")]
        public string Action { get; set; }
        [NotMapped]
        public List<string> listAction { get; set; }

        public int TrendId { get; set; }
        [NotMapped]
        public string Trend { get; set; }
        [NotMapped]
        public List<string> listTrend { get; set; }
        [NotMapped]
        [DisplayName("Season")]
        public string SeasonName { get; set; }
        [NotMapped]
        public string MeasureUnit { get; set; }

        [NotMapped]
        public int Weighingfactor { get; set; }
        public int? DisplayOrder { get; set; }
        [NotMapped]
        public string CurrentValueRange { get; set; }
        [NotMapped]
        public bool Reporting { get; set; }
    }
}
