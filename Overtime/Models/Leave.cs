using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Models
{
    public class Leave
    {
        [Key]
        [Display(Name = "Leave Id")]
        public int l_id { get; set; }

        [Display(Name = "Leave Id")]
        public int l_doc_id { get; set; }

        [Display(Name = "Department")]
        public int? l_dep_id { get; set; }

        [Display(Name = "Department")]
        public string? l_dep_name { get; set; }

        [Display(Name = "Designation")]
        public int? l_designation { get; set; }

        [Display(Name = "Designation")]
        public string? l_designation_name { get; set; }

        [Display(Name = "Type")]
        public int? l_type { get; set; }

        [Display(Name = "Type")]
        public string? l_type_name { get; set; }

        [Display(Name = "Reason")]
        public string? l_reason { get; set; }

        [Display(Name = "For")]
        public int? l_leave_for { get; set; }

        [Display(Name = "For")]
        public string? l_leave_for_name { get; set; }

        [Display(Name = "Leave From")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? l_leave_from { get; set; }

        [Display(Name = "Leave To")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? l_leave_to { get; set; }

        [Display(Name = "Leave Days")]
        public int? l_leave_days { get; set; } 
        
        [Display(Name = "Salary Required or Not")]
        public string? l_salary_required { get; set; }   
        
        [Display(Name = "Salary Month")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? l_salary_month { get; set; }

        [Display(Name = "Required Amount")]
        public int? l_required_amount { get; set; }

        [Display(Name = "Required Date ")]
        public DateTime? l_required_date { get; set; }

        [Display(Name = "Address")]
        public string? l_address { get; set; }
        
        [Display(Name = "Contact No 1")]
        public string? l_contact_no1 { get; set; } 
        
        [Display(Name = "Contact No 2")]
        public string? l_contact_no2 { get; set; }

        [Display(Name = "Status")]
        public int? l_status { get; set; } 
        
        [Display(Name = "Status")]
        public string? l_status_name { get; set; }

        [Display(Name = "Approval Status")]
        public int? l_approval_status { get; set; }

        [Display(Name = "Approval Status")]
        public string? l_approval_status_name { get; set; }
        
        [Display(Name = "Authorization")]
        public string? l_authorization { get; set; }

        [Display(Name = "Created By")]
        public int? l_cre_by { get; set; }

        [Display(Name = "Created By")]
        public string? l_cre_by_name { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? l_cre_date { get; set; }

    }
}
