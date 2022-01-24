using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Overtime.Models
{
    public class FoodSchedule
    {
        [Key]
        [Display(Name = "Food Id")]
        public int F_id { get; set; }
        public int? F_userid { get; set; }
        [Display(Name = "FoodsDate")]
        public DateTime? f_date { get; set; }
        public string? F_Breakfeedback { get; set; }
        public string? F_Lunchfeedback { get; set; }
        public string? F_Supperfeedback { get; set; }
        public DateTime F_FeedbackDate { get; set; }
        public string? F_cmdSuggestion { get; set; }
    }
}
