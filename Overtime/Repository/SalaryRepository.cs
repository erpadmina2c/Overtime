using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Overtime.Models;
using Overtime.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Repository
{
    public class SalaryRepository : ISalary
    {
        private readonly DBContext db;

        public SalaryRepository(DBContext _db)
        {
            db = _db;
        }

        public List<Salary> GetSalaries() {
            List<Salary> salaries = new List<Salary>();
            return salaries;
        }
            

        public void Add(Salary salary)
        {
            db.Salaries.Add(salary);
            db.SaveChanges();
        }

        public Salary GetSalary(int id)
        {

            var _id = new SqlParameter("@id", id);

            var salary = db.Salaries
                           .FromSqlRaw("EXEC GetSalary @id", _id)
                           .ToList()     // materialize query
                           .FirstOrDefault();    // pick first row

            return salary;
        }

        public void Remove(int id)
        {
            Salary salary = db.Salaries.Find(id);
            if (salary != null)
            {
                db.Salaries.Remove(salary);
                db.SaveChanges();
            }
        }

        public void Update(Salary salary)
        {
            db.Update(salary);
            db.SaveChanges();
        }

        public DataTable getSalaryOftheMonth(string yearMonth,int u_id)
        {
            DataTable dt = new DataTable();
            using (var conn = db.Database.GetDbConnection())
            {
                try
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = "getSalaryOftheMonth";
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters safely
                        var paramFrom = command.CreateParameter();
                        paramFrom.ParameterName = "@yearMonth";
                        paramFrom.Value = yearMonth;
                        command.Parameters.Add(paramFrom);

                        var paramUser = command.CreateParameter();
                        paramUser.ParameterName = "@u_id";
                        paramUser.Value = u_id;
                        command.Parameters.Add(paramUser);



                        using (var reader = command.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.InnerException);
                    // optionally log exception
                }
            }
            return dt;
        }

        public DataTable uploadSalary(string yearMonth, string jsonData)
        {
            DataTable dt = new DataTable();
            using (var conn = db.Database.GetDbConnection())
            {
                try
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandText = "uploadSalary";
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters safely
                        var paramFrom = command.CreateParameter();
                        paramFrom.ParameterName = "@yearMonth";
                        paramFrom.Value = yearMonth;
                        command.Parameters.Add(paramFrom);

                        var parmjsonData = command.CreateParameter();
                        parmjsonData.ParameterName = "@jsonData";
                        parmjsonData.Value = jsonData;
                        command.Parameters.Add(parmjsonData);



                        using (var reader = command.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.InnerException);
                    // optionally log exception
                }
            }
            return dt;
        }

        public DbResult deleteSalary(int id,int u_id)
        {
            var _id = new SqlParameter("id", id + "");
            var _u_id = new SqlParameter("u_id", u_id + "");
           
            var dbResults = db.DbResult.FromSqlRaw<DbResult>("EXECUTE dbo.deleteSalary @id,@u_id", _id, _u_id)
             .ToList().FirstOrDefault();

            return dbResults;
        }

        public DbResult deleteSalaryOftheMonth(string yearMonth, int u_id)
        {
            var _yearMonth = new SqlParameter("yearMonth", yearMonth + "");
            var _u_id = new SqlParameter("u_id", u_id + "");

            var dbResults = db.DbResult.FromSqlRaw<DbResult>("EXECUTE dbo.deleteSalaryOftheMonth @yearMonth,@u_id", _yearMonth, _u_id)
             .ToList().FirstOrDefault();

            return dbResults;
        }
    }
}
