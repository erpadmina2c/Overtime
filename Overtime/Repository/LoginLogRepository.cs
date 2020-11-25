using Overtime.Models;
using Overtime.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Repository
{
    public class LoginLogRepository : ILoginLog
    {
        private DBContext db;

        public LoginLogRepository(DBContext _db)
        {
            db = _db;
        }
        public IEnumerable<LoginLog> GetLoginLogs => db.LoginLogs;

        public void Add(LoginLog loginLog)
        {
            db.LoginLogs.Add(loginLog);
            db.SaveChanges();
        }

        public LoginLog GetLoginLog(int id)
        {
            LoginLog loginLog = db.LoginLogs.Find(id);
            return loginLog;
        }

        public IEnumerable<LoginLog> GetLoginLogsBySearch(int id, DateTime start_time, DateTime end_time)
        {
            var query = from u in db.LoginLogs
                        join k in db.Users
                         on u.ll_cre_by equals k.u_id
                        orderby u.ll_id descending
                        select new LoginLog
                        {
                            ll_id = u.ll_id,
                            ll_login_time=u.ll_login_time,
                            ll_cre_by = u.ll_cre_by,
                            ll_cre_by_name = k.u_name+" - "+k.u_full_name,
                            ll_cre_date = u.ll_cre_date,
                        };

           
            if (id != 0)
                query = query.Where(x => x.ll_cre_by == id);
            if (!start_time.ToString().Equals("1/1/0001 12:00:00 AM") && !end_time.ToString().Equals("1/1/0001 12:00:00 AM"))
                query = query.Where(x => x.ll_login_time >= start_time && x.ll_login_time <= end_time);
          
           
            return query;
        }

        public void Remove(int id)
        {
            LoginLog loginlog = db.LoginLogs.Find(id);
            db.LoginLogs.Remove(loginlog);
            db.SaveChanges();
        }

        public void Update(LoginLog loginlog)
        {
            db.LoginLogs.Update(loginlog);
            db.SaveChanges();
        }
     
    }
}
