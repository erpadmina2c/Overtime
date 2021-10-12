using Microsoft.EntityFrameworkCore;
using Overtime.Models;
using Overtime.Services;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Repository
{
    public class UserReportingHierarchyRepository : IUserReportingHierarchy
    {
        private DBContext db;

        public UserReportingHierarchyRepository(DBContext _db)
        {
            db = _db;
        }

        public IEnumerable<UserReportingHierarchy> GetUserReportingHierarchys => db.UserReportingHierarchy;

        public void Add(Department department)
        {
            db.Departments.Add(department);
            db.SaveChanges();
        }

        public void Add(UserReportingHierarchy UserReportingHierarchy)
        {
            throw new NotImplementedException();
        }

        public DbResult addUserReportingHierarchy(UserReportingHierarchy userReportingHierarchy)
        {
            var urh_u_id = new SqlParameter("urh_u_id", userReportingHierarchy.urh_u_id + "");
            var urh_reporting_to = new SqlParameter("urh_reporting_to", userReportingHierarchy.urh_reporting_to + "");
            var urh_priority = new SqlParameter("urh_priority", userReportingHierarchy.urh_priority + "");
            var urh_cre_by = new SqlParameter("urh_cre_by", userReportingHierarchy.urh_cre_by + "");
            var result = db.DbResult.FromSqlRaw<DbResult>
                ("EXECUTE dbo.addUserReportingHierarchy @urh_u_id,@urh_reporting_to,@urh_priority,@urh_cre_by",
                urh_u_id, urh_reporting_to, urh_priority, urh_cre_by).ToList().FirstOrDefault();
            return result;
        }

        public UserReportingHierarchy GetUserReportingHierarchy(int id)
        {
            UserReportingHierarchy UserReportingHierarchys = db.UserReportingHierarchy.Find(id);
            return UserReportingHierarchys;
        }

        public IEnumerable<List_UserReportingHierarchy> GetUserReportingHierarchysByuser(UserReportingHierarchy userReportingHierarchy)
        {

            var urh_u_id = new SqlParameter("urh_u_id", userReportingHierarchy.urh_u_id + "");
            var urh_reporting_to = new SqlParameter("urh_reporting_to", userReportingHierarchy.urh_reporting_to + "");
            var urh_priority = new SqlParameter("urh_priority", userReportingHierarchy.urh_priority + "");

            var pellets = db.List_UserReportingHierarchy.FromSqlRaw<List_UserReportingHierarchy>("EXECUTE dbo.GetUserReportingHierarchysByuser @urh_u_id,@urh_reporting_to,@urh_priority",
                urh_u_id, urh_reporting_to, urh_priority).ToList();

            return pellets;
        }

        public void Remove(int id)
        {
            UserReportingHierarchy UserReportingHierarchy = db.UserReportingHierarchy.Find(id);
            db.UserReportingHierarchy.Remove(UserReportingHierarchy);
            db.SaveChanges();
           
        }

        public void Update(UserReportingHierarchy UserReportingHierarchy)
        {
            db.Update(UserReportingHierarchy);
            db.SaveChanges();
        }

      
    }
}
