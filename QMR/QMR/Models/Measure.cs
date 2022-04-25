using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using QMR.Common;
namespace QMR.Models
{
    public partial class Measure: BaseViewModel
    {
        [NotMapped]
        public MeasureGroupType MeasureGroupType { get; set; }
        public int MeasureId { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "A Title was not entered.")]
        public string Title { get; set; }
        public string Description { get; set; }
     
        public int MeasureGroupId { get; set; }
        [DisplayName("RollUp")]
        public bool RollUpMeasure { get; set; } = false;
        [NotMapped]
        public List<string> MeasuresForSelectedGroup { get; set; }
        public double Target { get; set; }
        public double GreenRange { get; set; }
        [NotMapped]
        public float YellowRange { get; set; }
        public double RedRange { get; set; }
     
        [NotMapped]
        [ForeignKey("MeasureGroupId")]
        public virtual MeasureGroups measureGroups { get; set; }
        [NotMapped]
        [ForeignKey("MeasureTypeId")]
        public virtual MeasureType MeasureType { get; set; }
    
        public bool Status { get; set; }
        
        public string MeasureOwner { get; set; }

        public string DataSteward { get; set; }
        [ForeignKey("SeasonId")]
        public int SeasonId { get; set; }
        public virtual Season Season { get; set; }
        [DisplayName("Unit of Measure")]
        public string MeasureUnit { get; set; }
        [NotMapped]
        public List<string> MeasureUnitList { get; set; }

        [DisplayName("Reporting")]
        public bool Reporting { get; set; }

        [ForeignKey("CurrentValueOptionId")]
        public int CurrentValueOptionId { get; set; }
        [NotMapped]
        public virtual CurrentValueOption CurrentValueOption { get; set; }

        [NotMapped]
        [Display(Name = "CurrentValueOption")]
        public string SelectedCurrentValueOption { get; set; }
        [NotMapped]
        public List<string> CurrentValueOptions { get; set; }

        public string LastUpdatedBy { get; set; }

        [MaxLength(4000)]
        public string Comment { get; set; }
    }
}
