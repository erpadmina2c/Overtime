using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Overtime.Models;
using Overtime.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Repository
{
    public class LeavetypeRepository : ILeavetype
    {
        private readonly DBContext db;

        public LeavetypeRepository(DBContext _db)
        {
            db = _db;
        }

        public IEnumerable<Leavetypetbl> GetLeavetypes => db.Leavetypetbl;

        public void Add(Leavetypetbl leavetype)
        {
            throw new NotImplementedException();
        }

        public Leavetypetbl GetLeavetype(int id)
        {
            var _id = new SqlParameter("id", id + "");
            var LeaveType = db.Leavetypetbl.FromSqlRaw<Leavetypetbl>("EXECUTE dbo.GetLeavetype @id", _id).ToList().FirstOrDefault();

            return LeaveType;
        }


        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Leavetypetbl leavetype)
        {
            throw new NotImplementedException();
        }
    }
}
