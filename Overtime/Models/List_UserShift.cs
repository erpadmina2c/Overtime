using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class List_UserShift
    {
        [Key]
        [Display(Name = "uamId")]
        public int us_id { get; set; }

        [Display(Name = "Employee Id")]
        public int? us_u_id { get; set; }

        [Display(Name = "Employee Name")]
        public string? us_u_full_name { get; set; }

        [Display(Name = "Shift Started At")]
        [DataType(DataType.Time)]
        public DateTime? us_start_time { get; set; }

        [Display(Name = "Shift End On ")]
        [DataType(DataType.Time)]
        public DateTime? us_end_time { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? us_start_date { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? us_end_date { get; set; }

        [Display(Name = "Created by")]
        public int? us_cre_by { get; set; }

        [Display(Name = "Created by")]
        public string? us_cre_by_name { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime? us_cre_date { get; set; }
    }
}
