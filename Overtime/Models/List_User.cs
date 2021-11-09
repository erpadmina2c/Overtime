using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class List_User
    {

        [Key]
        [Display(Name = "Id")]
        public int u_id { get; set; }

        [Display(Name = "Full Name")]
        public string u_full_name{ get; set;}

        [Display(Name = "Username")]
        public string u_name{ get; set; }

        [Display(Name = "Email")]
        public string? u_email { get; set; }

        [Display(Name = "Created by")]
        public string u_cre_by_Name { get; set; }

        [Display(Name = "Joining Date")]
        [DataType(DataType.Date)]
        public DateTime? u_joining_date { get; set; }

        [Display(Name = "Last Working Day")]
        [DataType(DataType.Date)]
        public DateTime? u_cancelation_date { get; set; }

        [DataType(DataType.Date)]
        public int? u_canceled_by { get; set; }

        [Display(Name = "Cancelation updated By")]
        public string? u_canceled_by_name { get; set; }

        [Display(Name = "Cancelation updated On")]
        [DataType(DataType.Date)]
        public DateTime? u_canceled_on { get; set; }

        [Display(Name = "Created On")]
        [DataType(DataType.Date)]
        public DateTime u_cre_date { get; set; }

    }
}
