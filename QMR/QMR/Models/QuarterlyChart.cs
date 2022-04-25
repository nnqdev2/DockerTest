using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMR.Models
{
    public partial class QuarterlyChart: BaseViewModel
    {
        public int MeasureId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double GreenRange { get; set; }
        public double RedRange { get; set; }
        public string measureGroupName { get; set; }
        public string MeasureTypeDescription { get; set; }
        public string MeasureUnit { get; set; }
        public virtual List<ChartQData> ChartData { get; set; }
        public string Redheight { get; set; }
        public string Greenheight { get; set; }
        public string Yellowheight { get; set; }
        public double Yheight { get; set; }
        public int QuarterlyReviewDetailsId { get; set; }
    }
}
