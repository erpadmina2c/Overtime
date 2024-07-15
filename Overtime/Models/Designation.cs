using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class Designation
    {
        [Key]
        public int ds_id { get; set; }
        [Display(Name = "Designation")]
        public string ds_name { get; set; }

        [Display(Name = "Active")]
        public string ds_active_yn { get; set; }

        [Display(Name = "Created by")]
        public int ds_cre_by { get; set; }

        [Display(Name = "Created by")]
        public string ds_cre_by_name { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime ds_cre_date { get; set; }
    }
}
