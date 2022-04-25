using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMR.Models
{
    public partial class MeasureDetails: BaseViewModel
    {
        public int MeasureId { get; set; }
        
        public string Title { get; set; }
        public string Description { get; set; }

        [DisplayName("RollUp")]
        public string RollUpMeasure { get; set; }
        
        public double Target { get; set; }
        public double GreenRange { get; set; }
        public double YellowRange { get; set; }
        public double RedRange { get; set; }
        public string measureGroupName { get; set; }
     
        public string MeasureTypeDescription { get; set; }

        public bool? Status { get; set; }

        public string MeasureOwner { get; set; }

        public string DataSteward { get; set; }
        [DisplayName("Season")]
        public string SeasonName { get; set; }
        public string MeasureUnit { get; set; }
    }
}
