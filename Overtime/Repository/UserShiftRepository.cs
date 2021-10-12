using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Overtime.Models;
using Overtime.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Repository
{
    public class UserShiftRepository : IUserShift
    {
        private readonly DBContext db;

        public UserShiftRepository(DBContext _db)
        {
            db = _db;
        }

        public IEnumerable<UserShift> GetUserShifts => db.UserShift;

        public DbResult Add(UserShift userShift)
        {
            var us_u_id = new SqlParameter("us_u_id", userShift.us_u_id + "");
            var us_start_time = new SqlParameter("us_start_time", userShift.us_start_time + "");
            var us_end_time = new SqlParameter("us_end_time", userShift.us_end_time + "");
            var us_start_date = new SqlParameter("us_start_date", userShift.us_start_date + "");
            var us_cre_by = new SqlParameter("us_cre_by", userShift.us_cre_by + "");
            var result = db.DbResult.FromSqlRaw<DbResult>
                ("EXECUTE dbo.createUserShift @us_u_id,@us_start_time,@us_end_time,@us_start_date,@us_cre_by",
                us_u_id, us_start_time, us_end_time, us_start_date, us_cre_by).ToList().FirstOrDefault();
            return result;
        }

        public UserShift GetUserShift(int id)
        {
            UserShift userShift = db.UserShift.Find(id);
            return userShift;
        }

        public void Remove(int id)
        {
            UserShift userShift = db.UserShift.Find(id);
            db.UserShift.Remove(userShift);
            db.SaveChanges();
        }

        public IEnumerable<List_UserShift> UserShiftData(DateTime from, DateTime to, int id)
        {
            var user = new SqlParameter("id",id + "");
            var from_date = new SqlParameter("from", from + "");
            var to_date = new SqlParameter("to", to + "");
            var userShifts = db.List_UserShift.FromSqlRaw<List_UserShift>("EXECUTE dbo.UserShiftData @id,@from,@to", user, from_date, to_date).ToList();
            return userShifts;
        }

        public void Update(UserShift userShift)
        {
            db.UserShift.Update(userShift);
            db.SaveChanges();
        }
    }
}
