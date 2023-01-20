using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class Accomadation
    {

        [Key]
        [Display(Name = "Id")]
        public int ac_id { get; set; }

        [Display(Name = "Accomodation Name")]
        public string ac_name{ get; set;}

        [Display(Name = "1st Punch Type")]
        public string ac_first_punch_type{ get; set; }

        [Display(Name = "Active Y/N")]
        public string ac_active_yn { get; set; }

        [Display(Name = "Created By")]
        public int ac_cre_by { get; set; }

        [NotMapped]
        [Display(Name = "Created by")]
        public string ac_cre_by_Name { get; set; }

        [Display(Name = "Created On")]
        [DataType(DataType.Date)]
        public DateTime ac_cre_date { get; set; }

    }
}
