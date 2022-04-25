using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QMR.Models
{
    public partial class ChartData
    {
        public int MeasureId { get; set; }
        public string QuarterDetail { get; set; }
        public double CurrentValue { get; set; }
        public double GreenRange { get; set; }
        public double RedRange { get; set; }
        public string MeasureUnit { get; set; }
        public string Redheight { get; set; }
        public string Greenheight { get; set; }
        public string Yellowheight { get; set; }
    }
}
