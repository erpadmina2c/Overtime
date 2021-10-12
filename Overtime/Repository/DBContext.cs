using Microsoft.EntityFrameworkCore;
using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Repository
{
    public class DBContext : DbContext
    {

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        public DbSet<User> Users {get;set;}
        public DbSet<Role> Roles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowDetail> WorkflowDetails { get; set; }
        public DbSet<WorkflowTracker> workflowTrackers { get; set; }
        public DbSet<OverTimeRequest> OverTimeRequest { get; set; }
        public DbSet<Documents> Documents { get; set; }

        public DbSet<Hold> Holds { get; set; }

        public DbSet<RoleMenu> RoleMenus { get; set; }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<UserDepartment> UserDepartments { get; set; }

        public DbSet<Insight> Insights { get; set; }

        public DbSet<OvCustomModel> OvCustomModel { get; set; }
        public DbSet<HourRate> HourRates { get; set; }

        public DbSet<WorkingHour> WorkingHour { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<Training> Training { get; set; }
        public DbSet<List_Training> List_Training { get; set; }
        public DbSet<DbResult> DbResult { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<MachineDetail> MachineDetails { get; set; }
        public DbSet<Leavetypetbl> Leavetypetbl { get; set; }
        public DbSet<LeaveDetail> LeaveDetail { get; set; }
        public DbSet<UserShift> UserShift { get; set; }
        public DbSet<List_UserShift> List_UserShift { get; set; }
        public DbSet<UserReportingHierarchy> UserReportingHierarchy { get; set; }
        public DbSet<List_UserReportingHierarchy> List_UserReportingHierarchy { get; set; }
        public DbSet<List_Attendance> List_Attendance { get; set; }


    }
}
