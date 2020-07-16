using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class WorkingHour
    {
        [Key]
        public int wh_id { get; set; }


        public int wh_fun_doc_id { get; set; }
        public int wh_doc_id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "hours")]
        public decimal wh_hours { get; set; }



        [Display(Name = "remarks")]
        public String wh_remarks { get; set; }
        

        [Display(Name = "Cre by")]
        public int wh_cre_by { get; set; }
        [DataType(DataType.Date)]

        [Display(Name = "Cre by Name")]
        public string wh_cre_by_name { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Cre Date")]
        public DateTime wh_cre_date { get; set; }
    }
}
