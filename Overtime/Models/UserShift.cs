using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class UserShift
    {
        [Key]
        [Display(Name = "uamId")]
        public int us_id { get; set; }

        [Display(Name = "Employee Id")]
        public int? us_u_id { get; set; }

        [Display(Name = "Shift Started At")]
        [DataType(DataType.DateTime)]
        public DateTime? us_start_time { get; set; }

        [Display(Name = "Shift End On ")]
        [DataType(DataType.DateTime)]
        public DateTime? us_end_time { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime? us_start_date { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        public DateTime? us_end_date { get; set; }

        [Display(Name = "Created by")]
        public int? us_cre_by { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        public DateTime? us_cre_date { get; set; }
    }
}
