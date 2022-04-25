using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace QMR.Models
{
    public partial class MeasureAction: BaseViewModel
    {  
        public int ActionId { get; set; }
        [DisplayName("Title")]
        [Required]
        public string ActionName { get; set; }
        [DisplayName("Color")]
        public string ActionColor { get; set; }
        [DisplayName("Status")]
        public bool ActionStatus { get; set; } = false;
    }
}

