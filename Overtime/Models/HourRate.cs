using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class HourRate
    {
        [Key]
        [Display(Name = "Id")]
        public int hr_id { get; set; }

        [Display(Name = "Role")]
        public int? hr_role_id{ get; set; }

        [Display(Name = "Role Description")]
        public string hr_role_description { get; set; }


        [Display(Name = "Date")]
        public DateTime hr_date { get; set; }


        [Display(Name = "Hour Rate")]
        public Decimal hr_rate { get; set; }

        [Display(Name = "Active")]
        public string hr_active_yn  { get; set; }


        [Display(Name = "Created by")]
        public int hr_cre_by { get; set; }

        [NotMapped]
        [Display(Name = "Created by Name")]
        public string hr_cre_by_name { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime hr_cre_date { get; set; }
    }
}
