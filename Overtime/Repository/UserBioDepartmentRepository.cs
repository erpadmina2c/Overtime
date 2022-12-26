using Overtime.Models;
using Overtime.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Repository
{
    public class UserBioDepartmentRepository : IUserBioDepartment
    {
        private DBContext db;

        public UserBioDepartmentRepository(DBContext _db)
        {
            db = _db;
        }
       

        public void Add(UserBioDepartment userbioDepartment)
        {
            db.Bio_UserDepartments.Add(userbioDepartment);
            db.SaveChanges();
        }

        public bool getIsExistOrNot(int ud_user_id, int ud_depart_id)
        {
            bool isExist;
            var query = (from u in db.Bio_UserDepartments
                         where u.ud_user_id == ud_user_id && u.ud_depart_id== ud_depart_id
                        select  u
                        ).Count();
            if (query > 0) isExist = true;
            else
            {
                isExist = false;
            }

            return isExist;


        }

        public UserBioDepartment GetUserBioDepartment(int id)
        {
                    var query = from u in db.Bio_UserDepartments
                    join d in db.Departments on u.ud_depart_id equals d.d_id
                    join e in db.Users
                    on u.ud_user_id equals e.u_id
                    join b in db.Users
                    on u.ud_cre_by equals b.u_id
                    where u.ud_id == id
                    select new UserBioDepartment
                    {
                        ud_id = u.ud_id,
                        ud_user_id = u.ud_user_id,
                        ud_user_name=e.u_full_name,
                        ud_depart_id=u.ud_depart_id,
                        ud_depart_desc=d.d_description,
                        ud_active_yn=u.ud_active_yn,
                        ud_cre_by = u.ud_cre_by,
                        ud_cre_by_name=b.u_full_name,
                        ud_cre_date =u.ud_cre_date,
                        };

            return query.FirstOrDefault<UserBioDepartment>(); 
        }

        public void Remove(int id)
        {
                 UserBioDepartment userDepartment = db.Bio_UserDepartments.Find(id);
                 db.Bio_UserDepartments.Remove(userDepartment);
                 db.SaveChanges();
        }
    

        public void Update(UserBioDepartment userbioDepartment)
        {
                db.Bio_UserDepartments.Update(userbioDepartment);
                db.SaveChanges();
        }

        IEnumerable<UserBioDepartment> IUserBioDepartment.GetUserBioDepartments()
        {
           var userBioDepartment= from u in db.Bio_UserDepartments
            join d in db.Departments on u.ud_depart_id equals d.d_id
            join e in db.Users
            on u.ud_user_id equals e.u_id
            join b in db.Users
            on u.ud_cre_by equals b.u_id
            select new UserBioDepartment
            {
                ud_id = u.ud_id,
                ud_user_id = u.ud_user_id,
                ud_user_name = e.u_full_name,
                ud_depart_id = u.ud_depart_id,
                ud_depart_desc = d.d_description,
                ud_active_yn = u.ud_active_yn,
                ud_cre_by = u.ud_cre_by,
                ud_cre_by_name = b.u_full_name,
                ud_cre_date = u.ud_cre_date,
            };
            return userBioDepartment;
        }
    }
}
