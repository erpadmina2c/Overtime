using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class ArchivedLeave
    {
        [Key]
        [Display(Name = "AL Id")]
        public int al_id { get; set; }

        [Display(Name = "User")]
        public int? al_user_id { get; set; }

        [Display(Name = "User")]
        public string? al_user_name { get; set; }

        [Display(Name = "Archived Leave Days")]
        public int? al_leave_days { get; set; }

        [Display(Name = "Updated By")]
        public int? al_upd_by { get; set; }

        [Display(Name = "Updated By")]
        public string? al_upd_by_name { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime? al_upd_date { get; set; }

        [Display(Name = "Created By")]
        public int? al_cre_by { get; set; }

        [Display(Name = "Created By")]
        public string? al_cre_by_name { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? al_cre_date { get; set; }

    }
}
