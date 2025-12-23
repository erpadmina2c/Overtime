using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class InAndOut
    {
        [Key]
        [Display(Name = "IO Id")]
        public int io_id { get; set; }

        [Display(Name = "User ID")]
        public int io_u_id { get; set; }

        [Display(Name = "Campus")]
        public int io_campus { get; set; }

        [Display(Name = "Punch Date")]
        public DateTime? io_date { get; set; }

        [Display(Name = "Punch Time")]
        public DateTime? io_punchtime { get; set; }

        [Display(Name = "Punch Type")]
        public string? io_punch_type { get; set;}

        [Display(Name = "Remarks")]
        public string? io_remarks { get; set; }

        [Display(Name = "Modified By")]
        public int? io_modified_by { get; set;}

        [Display(Name = "Modified Date")]
        public DateTime? io_modified_date { get; set;}

        [Display(Name = "Created by")]
        public int? io_created_by { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime? io_created_date { get; set; }
    }
}
