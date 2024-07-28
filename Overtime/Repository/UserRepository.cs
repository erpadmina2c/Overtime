using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NHibernate.Linq;
using Overtime.Models;
using Overtime.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Repository
{
    public class UserRepository : IUser
    {
        private DBContext db;

        public IEnumerable<User> GetUsers => db.Users;

        public IEnumerable<EmpInfo> getUsersEmployeeDetails()
        {
            var quary=from u in db.Users
            join r in db.Roles
              on u.u_role_id equals r.r_id
            join k in db.Users
             on u.u_cre_by equals k.u_id
            where r.r_active_yn == "Y"
            select new EmpInfo
            {
                id = u.u_id,
                code = u.u_name,
                name = u.u_full_name
            };
            return quary;
        }

        public UserRepository(DBContext _db)
        {
            db = _db;
        }

       
        public IEnumerable<List_User> GetUsersList() {

            var userShifts = db.List_User.FromSqlRaw<List_User>("EXECUTE dbo.GetUsersList").ToList();
            return userShifts;

        }



        public void Add(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
        }


        public User GetUser(int id)
        {
            User user = db.Users.Find(id);
            return user;
        }

        public User getUserbyUsername(string u_name)
        {
            var query = from u in db.Users
                        join r in db.Roles
                          on u.u_role_id equals r.r_id
                        join e in db.Users
                        on u.u_cre_by equals e.u_id
                        where u.u_name== u_name && u.u_active_yn=="Y"
                        select new User
                        {
                           u_id=u.u_id,
                           u_full_name=u.u_full_name,
                           u_name=u.u_name,
                           u_email = u.u_email,
                           u_password =u.u_password,
                           u_is_admin=u.u_is_admin,
                           u_role_id=u.u_role_id,
                           u_role_description=r.r_description,
                           u_accomodation=u.u_accomodation,
                           u_active_yn=u.u_active_yn,
                           u_allocation_yn=u.u_allocation_yn,
                            u_cancelation_date = u.u_cancelation_date,
                            u_canceled_by = u.u_canceled_by,
                            u_canceled_on = u.u_canceled_on,
                            u_joining_date = u.u_joining_date,
                            u_cre_by =u.u_cre_by,
                           u_cre_by_Name=e.u_name,
                           u_cre_date=u.u_cre_date
                        };

            return query.FirstOrDefault<User>();
           
        }

        public void Remove(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
        }

        public void Update(User user)
        {
            db.Users.Update(user);
            db.SaveChanges();
        }

        public IEnumerable<string> getUsersNames(string name)
        {
            /*  var query = from u in db.Users
                          where SqlMethods.Like(u.u_full_name, "%" + name + "%")
                          select u.u_full_name;*/


            var query = from u in db.Users
                        where u.u_full_name.Contains(name)
                        select u.u_name+"-"+u.u_full_name;
          


            return query;
        }

        public List<List_User> getEmployeeInformation(int u_id, string type)
        {
            var _u_id = new SqlParameter("u_id", u_id + "");
            var _type = new SqlParameter("type", type + "");

            var users = db.List_User.FromSqlRaw<List_User>("EXECUTE getEmployeeInformation @u_id,@type", _u_id, _type).ToList();

            return users;
        }
        public DbResult CancelEmployee(int u_id, string u_cancelation_date,int cre_by)
        {
            var _u_id = new SqlParameter("u_id", u_id + "");
            var _u_cancelation_date = new SqlParameter("u_cancelation_date", u_cancelation_date + "");
            var _cre_by = new SqlParameter("cre_by", cre_by + "");

            var dbResult = db.DbResult.FromSqlRaw<DbResult>("EXECUTE CancelEmployee @u_id,@u_cancelation_date,@cre_by", _u_id, _u_cancelation_date, _cre_by).ToList().FirstOrDefault();

            return dbResult;
        }

        public IEnumerable<List_User> getActiveUsers()
        {
            var users = db.List_User.FromSqlRaw<List_User>("EXECUTE dbo.getActiveUsers").ToList();
            return users;
        }

        public List<List_User> getSupervisors()
        {
            var users = db.List_User.FromSqlRaw<List_User>("EXECUTE dbo.getSupervisors").ToList();
            return users;
        }
    }
}
