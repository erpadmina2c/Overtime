using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class UserReportingHierarchy
    {
        [Key]
        [Display(Name = "URH Id")]
        public int urh_id { get; set; }

        [Display(Name = "User")]
        public int urh_u_id { get; set; }

        [Display(Name = "Reporting To")]
        public int urh_reporting_to { get; set; }
      
        [Display(Name = "Priority")]
        public int urh_priority { get; set; }
    
        [Display(Name = "Created by")]
        public int urh_cre_by { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime urh_cre_date { get; set; }
    }
}
