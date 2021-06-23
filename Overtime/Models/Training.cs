using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class Training
    {
        [Key]
        [Display(Name = "Training Id")]
        public int tr_id { get; set; }

        [Display(Name = "Employee Id")]
        public int? tr_u_id { get; set; }

        [Display(Name = "Employee Code")]
        public string? tr_u_name { get; set; }

        [Display(Name = "With Effected Date")]
        [DataType(DataType.Date)]
        public DateTime? tr_start_date { get; set; }

        [Display(Name = "Finished at")]
        [DataType(DataType.Date)]
        public DateTime? tr_end_date { get; set; }

        [Display(Name = "Created by")]
        public int? tr_cre_by { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime? tr_cre_date { get; set; }
    }
}
