using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Overtime.Models;
using Overtime.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Repository
{
    public class ArchivedLeavesRepository : IArchivedLeave
    {
        private readonly DBContext db;

        public ArchivedLeavesRepository(DBContext _db)
        {
            db = _db;
        }

        public DbResult createArchivedLeave(int u_id, int leave_days, int cre_by)
        {
            var _u_id = new SqlParameter("u_id", u_id + "");
            var _leave_days = new SqlParameter("leave_days", leave_days + "");
            var _cre_by = new SqlParameter("cre_by", cre_by + "");

            var dbResults = db.DbResult.FromSqlRaw<DbResult>
                ("EXECUTE dbo.createArchivedLeave @u_id,@leave_days,@cre_by", _u_id, _leave_days, _cre_by).ToList().FirstOrDefault();
            return dbResults;
        }

        public List<ArchivedLeave> getArchivedLeaves()
        {
            var archivedLeaves = db.ArchivedLeaves.FromSqlRaw<ArchivedLeave>("EXECUTE dbo.getArchivedLeaves").ToList();
            return archivedLeaves;
        }

        public ArchivedLeave getEmployeeArchivedLeave(int u_id)
        {
            var _u_id = new SqlParameter("u_id", u_id + "");

            var ArchivedLeave = db.ArchivedLeaves.FromSqlRaw<ArchivedLeave>
                ("EXECUTE dbo.getEmployeeArchivedLeave @u_id",  _u_id).ToList().FirstOrDefault();
            return ArchivedLeave;
        }

        public DbResult updateArchivedLeave(int al_id, int leave_days, int u_id)
        {
            var _al_id = new SqlParameter("al_id", al_id + "");
            var _leave_days = new SqlParameter("leave_days", leave_days + "");
            var _u_id = new SqlParameter("u_id", u_id + "");

            var dbResults = db.DbResult.FromSqlRaw<DbResult>
                ("EXECUTE dbo.updateArchivedLeave @al_id,@leave_days,@u_id", _al_id, _leave_days, _u_id).ToList().FirstOrDefault();
            return dbResults;
        }
    }
}
