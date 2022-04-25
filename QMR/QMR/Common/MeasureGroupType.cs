using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace QMR.Common
{
    public class MeasureGroupType
    {
        [Display(Name = "MeasureGroupType")]
        public string measureGroupType { get; set; }
       
        [Display(Name = "Group")]
        public string SelectedGroup { get; set; }
        public List<string> MeasureGroup { get; set; }

       
        [Display(Name = "Type")]
        public string SelectedType { get; set; }
        public List<string> MeasureType { get; set; }

        [Display(Name = "Season")]
        public string SelectedSeason{ get; set; }
        public List<string> Season { get; set; }

    }
}
