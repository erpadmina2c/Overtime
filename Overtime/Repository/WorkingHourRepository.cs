using Microsoft.EntityFrameworkCore;
using Overtime.Models;
using Overtime.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Repository
{
    public class WorkingHourRepository : IWorkingHour
    {
        private DBContext db;

        public WorkingHourRepository(DBContext _db)
        {
            db = _db;
        }
        public IEnumerable<WorkingHour> GetWorkingHours => db.WorkingHour;

        public void Add(WorkingHour workingHour)
        {
            db.WorkingHour.Add(workingHour);
            db.SaveChanges();
        }

        public WorkingHour GetWorkingHour(int id)
        {
            WorkingHour workingHour = db.WorkingHour.Find(id);
            return workingHour;
        }

        public IEnumerable<WorkingHour> GetWorkingHourByDocument(int rowid, int doc_id)
        {
            var query = from u in db.WorkingHour
                        join d in db.Users
                         on u.wh_cre_by equals d.u_id
                        where u.wh_doc_id == doc_id && u.wh_fun_doc_id == rowid
                        orderby u.wh_cre_date descending
                        select new WorkingHour
                        {
                            wh_id = u.wh_id,
                            wh_doc_id = u.wh_doc_id,
                            wh_fun_doc_id = u.wh_fun_doc_id,
                            wh_hours = u.wh_hours,
                            wh_remarks= u.wh_remarks,
                            wh_cre_by = u.wh_cre_by,
                            wh_cre_by_name = d.u_name + '-' + d.u_full_name,
                            wh_cre_date = u.wh_cre_date

                        };


            return query;
        }

        public decimal GetWorkingHourConsolidateByDocument(int v1, int v2)
        {
            decimal workinghour = 0;
            var conn = db.Database.GetDbConnection();
            try
            {

                conn.Open();
                using (var command = conn.CreateCommand())
                {


                    string query = @"select sum(wh_hours) hour from WorkingHour
                                    where wh_doc_id="+ v2 + " and wh_fun_doc_id="+ v1 + "";
                  
                    command.CommandText = query;
                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if(!reader["hour"].ToString().Equals(""))  workinghour = decimal.Parse(reader["hour"].ToString());

                        }
                    }
                    reader.Dispose();

                }
            }
            finally
            {
                conn.Close();
            }
            return workinghour;
        }

        public void Remove(int id)
        {
            WorkingHour workingHour = db.WorkingHour.Find(id);
            db.WorkingHour.Remove(workingHour);
            db.SaveChanges();
        }

        public void Update(WorkingHour workingHour)
        {
            db.WorkingHour.Update(workingHour);
            db.SaveChanges();
        }
    }
}
