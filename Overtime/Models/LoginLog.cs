using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class LoginLog
    {
        [Key]
        public int ll_id { get; set; }

        [Display(Name = "Login Time")]
        [DataType(DataType.DateTime)]
        public DateTime ll_login_time{ get; set; }

        [Display(Name = "Cre by")]
        public int ll_cre_by { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Cre by Name")]
        public string ll_cre_by_name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Cre Date")]
        public DateTime ll_cre_date { get; set; }
    }
}
