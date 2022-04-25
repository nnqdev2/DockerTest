using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QMR.Models
{
    public partial class RollUpMeasures
    {
        public int Id { get; set; }
        public int RollUpMeasureId { get; set; }
        public int MeasureId { get; set; }
        public int WeightingFactor { get; set; }
    }
}
