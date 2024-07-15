using Microsoft.EntityFrameworkCore;
using Overtime.Models;
using Overtime.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Repository
{
    public class DesignationRepository : IDesignation
    {
        private DBContext db;

        public DesignationRepository(DBContext _db)
        {
            db = _db;
        }

        public List<Designation> GetDesignations()
        {
            var designations = db.Designations.FromSqlRaw<Designation>("EXECUTE dbo.GetDesignations").ToList();
            return designations;
        }
    }
}
