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
            throw new NotImplementedException();
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
