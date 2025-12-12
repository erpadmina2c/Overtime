using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Overtime.Models
{
    public class Salary
    {
        [Key]
        [Display(Name = "Id")]
        public int? s_id { get; set; }

        [Display(Name = "Year Month")]
        public string? s_year_month { get; set; }

        [Display(Name = "Emp Code")]
        public string? s_emp_code { get; set; }

        [Display(Name = "Emp Name")]
        public string? s_emp_name { get; set; }

        [Display(Name = "Category")]
        public string?  s_category{ get; set; }

        [Display(Name = "Department")]
        public string? s_department { get; set; }

        [Display(Name = "Basic Salary")]
        public int? s_basic_salary { get; set; }

        [Display(Name = "Daily OT (30 Minutes)")]
        public int? s_daily_ot { get; set; }

        [Display(Name = "Total Salary")]
        public int? s_total_salary { get; set; }

        [Display(Name = "Overtime")]
        public int? s_overtime { get; set; }

        [Display(Name = "Special Bonus")]
        public int? s_special_bonus { get; set; }

        [Display(Name = "Allowance")]
        public int? s_allowance { get; set; }

        [Display(Name = "Aditional Bonus")]
        public int? s_aditional_bonus { get; set; }

        [Display(Name = "Other Payment")]
        public int? s_other_payment { get; set; }

        [Display(Name = "Travelling Allowance")]
        public int? s_traveling_allowance { get; set; }

        [Display(Name = "Peformance Bonus")]
        public int? s_performance_bonus { get; set; }

        [Display(Name = "Deduction")]
        public int? s_deduction { get; set; }

        [Display(Name = "Recovery")]
        public int? s_recovery { get; set; }

        [Display(Name = "Advance")]
        public int? s_advance { get; set; }

        [Display(Name = "Advance")]
        public int? s_payable { get; set; }

        [Display(Name = "Advance")]
        public string? s_Message { get; set; }

        [Display(Name = "Created")]
        public int s_cre_by { get; set; }

        [NotMapped]
        [Display(Name = "Created by")]
        public string? s_cre_by_Name { get; set; }

        [Display(Name = "Created On")]
        [DataType(DataType.Date)]
        public DateTime s_cre_date { get; set; }


    }
}
