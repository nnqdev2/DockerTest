using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMR.Models
{
    public partial class MeasureGroups
    {
        public int MeasureGroupId { get; set; }
        public int MeasureTypeId { get; set; }
        public string MeasureGroupName { get; set; }
        public bool? MeasureGroupStatus { get; set; }
        [ForeignKey("MeasureTypeId")]
        public MeasureType MeasureType { get; set; }
    }
}
