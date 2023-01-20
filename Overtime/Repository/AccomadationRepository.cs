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
    public class AccomadationRepository : IAccomadation
    {
        private readonly DBContext db;

        public AccomadationRepository(DBContext _db)
        {
            db = _db;
        }
  
        public IEnumerable<Accomadation> GetAccomadationslist =>db.Accomadations;

        public void Add(Accomadation accomadation)
        {
            db.Accomadations.Add(accomadation);
            db.SaveChanges();
        }

        public DbResult AddOrUpdateAccomodation(Accomadation accomadation)
        {
            var ac_id = new SqlParameter("ac_id", accomadation.ac_id + "");
            var ac_name = new SqlParameter("ac_name", accomadation.ac_name + "");
            var ac_first_punch_type = new SqlParameter("ac_first_punch_type", accomadation.ac_first_punch_type + "");
            var ac_active_yn = new SqlParameter("ac_active_yn", accomadation.ac_active_yn + "");
            var ac_cre_by = new SqlParameter("ac_cre_by", accomadation.ac_cre_by + "");

            var dbResults  = db.DbResult.FromSqlRaw<DbResult>("EXECUTE dbo.AddOrUpdateAccomodation @ac_id,@ac_name,@ac_first_punch_type,@ac_active_yn,@ac_cre_by", ac_id, ac_name, ac_first_punch_type, ac_active_yn, ac_cre_by).ToList().FirstOrDefault();

            return dbResults;
        }

        public DbResult DeleteAccomodation(int id, int u_id)
        {
            var _id = new SqlParameter("id", id + "");
            var _u_id = new SqlParameter("u_id", u_id + "");

            var dbResult = db.DbResult.FromSqlRaw<DbResult>("EXECUTE dbo.DeleteAccomodation @id,@u_id", _id, _u_id).ToList().FirstOrDefault();

            return dbResult;
        }

        public Accomadation GetAccomadation(int id)
        {
            Accomadation accomadation = db.Accomadations.Find(id);
            return accomadation;
        }

        public IEnumerable<Accomadation> GetAccomadations(string name)
        {
            var _name = new SqlParameter("name", name + "");

            var accomadations = db.Accomadations.FromSqlRaw<Accomadation>("EXECUTE dbo.GetAccomadations @name", _name).ToList();

            return accomadations;
        }

        public void Remove(int id)
        {
            Accomadation accomadation = db.Accomadations.Find(id);
            db.Accomadations.Remove(accomadation);
            db.SaveChanges();
        }

        public void Update(Accomadation accomadation)
        {
            db.Update(accomadation);
            db.SaveChanges();
        }

       
    }
}
