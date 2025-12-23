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
    public class CampusRepository : ICampus
    {
        private readonly DBContext db;

        public CampusRepository(DBContext _db)
        {
            db = _db;
        }
       
        public List<Campus> getCampuses()
        {

            return db.Set<Campus>()
                     .FromSqlRaw("EXEC dbo.getCampuses")
                     .ToList();
        }

       
        public DbResult addOrUpdateCampus(Campus campus)
        {
            var parameters = new[]
            {
                new SqlParameter("@c_id", campus.c_id),
                new SqlParameter("@c_name", campus.c_name ?? ""),
                new SqlParameter("@c_active_yn", campus.c_active_yn ?? "Y"),
                new SqlParameter("@c_cre_by", campus.c_cre_by)
            };

            return db.Set<DbResult>()
                     .FromSqlRaw(
                         "EXEC dbo.addOrUpdateCampus @c_id, @c_name, @c_active_yn, @c_cre_by",
                         parameters)
                     .AsEnumerable()
                     .FirstOrDefault() ?? new DbResult();
        }

       
        public DbResult deleteCampus(int c_id, int u_id)
        {

            var  _c_id=new SqlParameter("@c_id", c_id);
            var  _u_id= new SqlParameter("@u_id", u_id);
           
            DbResult dbResult= db.Set<DbResult>()
                     .FromSqlRaw(
                         "EXEC dbo.deleteCampus @c_id, @u_id", _c_id, _u_id)
                     .ToList()
                     .FirstOrDefault() ?? new DbResult();
            return dbResult;
        }

        public Campus getCampus(int c_id)
        {
            var _c_id = new SqlParameter("@c_id", c_id);
            return db.Set<Campus>()
                     .FromSqlRaw("EXEC dbo.getCampus @c_id", _c_id)
                     .ToList()
                     .FirstOrDefault() ?? new Campus();
        }
    }
}
