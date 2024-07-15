using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class RemainingLeave
    {
        [Key]
        [Display(Name = "RL Id")]
        public int rl_id { get; set; }

        [Display(Name = "User Id")]
        public int rl_u_id { get; set; }

        [Display(Name = "User Name")]
        public string? rl_u_name { get; set; }

        [Display(Name = "Joining Date")]
        [DataType(DataType.Date)]
        public DateTime? rl_joining_date { get; set; }

        [Display(Name = "Leave Taken")]
        public decimal? rl_leave_taken { get; set; }

        [Display(Name = " Archived Leaves")]
        public decimal? rl_archived_leave { get; set; }

        [Display(Name = "Total Leave Taken")]
        public decimal? rl_total_leave_taken { get; set; }

        [Display(Name = "Eligible")]
        public decimal? rl_eligible { get; set; }

        [Display(Name = "Remaining")]
        public decimal? rl_remaining { get; set; }

    }
}
