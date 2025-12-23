using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class Campus
    {

        [Key]
        [Display(Name = "Id")]
        public int c_id { get; set; }

        [Display(Name = "Campus Name")]
        public string? c_name{ get; set;}


        [Display(Name = "Active Y/N")]
        public string? c_active_yn { get; set; }

        [Display(Name = "Created By")]
        public int c_cre_by { get; set; }

        [Display(Name = "Created by")]
        public string? c_cre_by_name { get; set; }

        [Display(Name = "Created On")]
        [DataType(DataType.Date)]
        public DateTime c_cre_date { get; set; }

    }
}
