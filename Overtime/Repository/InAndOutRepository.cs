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
    public class InAndOutRepository : IInAndOut
    {
        private readonly DBContext db;

        public InAndOutRepository(DBContext _db)
        {
            db = _db;
        }

        public IEnumerable<InAndOut> GetInAndOuts => throw new NotImplementedException();

        public DbResult AddInAndOut(InAndOut inAndOut)
        {
            var io_u_id = new SqlParameter("io_u_id", inAndOut.io_u_id + "");
            var io_punchtime = new SqlParameter("io_punchtime", inAndOut.io_punchtime + "");
            var io_punch_type = new SqlParameter("io_punch_type", inAndOut.io_punch_type + "");
            var io_remarks = new SqlParameter("io_remarks", inAndOut.io_remarks + "");
            var io_created_by = new SqlParameter("io_created_by", inAndOut.io_created_by + "");

            var dbResults = db.DbResult.FromSqlRaw<DbResult>("EXECUTE dbo.AddInAndOut @io_u_id,@io_punchtime,@io_punch_type,@io_remarks,@io_created_by", 
                io_u_id, io_punchtime, io_punch_type, io_remarks, io_created_by).ToList().FirstOrDefault();

            return dbResults;
        }

        public DataTable getAccomodationWiseInAndOut(int ac_id,string status,int u_id)
        {
            DataTable dt = new DataTable();
            var conn = db.Database.GetDbConnection();
            try
            {

                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = @"exec [dbo].[getAccomodationWiseInAndOut] @ac_id = '" + ac_id + "',@status = '" + status + "',@u_id ='" + u_id + "'";
                    command.CommandText = query;
                    command.CommandTimeout = 250;

                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }
                    reader.Dispose();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message + " " + ex.InnerException);
                conn.Close();
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        public DataTable getInAndOutLogBySearch(DateTime from, DateTime to)
        {
            DataTable dt = new DataTable();
            var conn = db.Database.GetDbConnection();
            try
            {

                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = @"exec [dbo].[getInAndOutLogBySearch] @from = '" + from + "',@to ='" + to + "'";
                    command.CommandText = query;
                    command.CommandTimeout = 250;

                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }
                    reader.Dispose();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message + " " + ex.InnerException);
                conn.Close();
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        public DataTable getInAndOutReport(int u_id, int ac_id,DateTime from, DateTime to)
        {
            DataTable dt = new DataTable();
            var conn = db.Database.GetDbConnection();
            try
            {

                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = @"exec [dbo].[getInAndOutReport] @u_id ='" + u_id + "',@ac_id ='" + ac_id + "',@from = '" + from + "',@to ='" + to + "'";
                    command.CommandText = query;
                    command.CommandTimeout = 250;

                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }
                    reader.Dispose();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message + " " + ex.InnerException);
                conn.Close();
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        public DbResult updateInAndOutPunchType(InAndOut inAndOut)
        {
            var io_id = new SqlParameter("io_id", inAndOut.io_id + "");
            var io_punch_type = new SqlParameter("io_punch_type", inAndOut.io_punch_type + "");
            var io_modified_by = new SqlParameter("io_modified_by", inAndOut.io_modified_by + "");

            var dbResults = db.DbResult.FromSqlRaw<DbResult>("EXECUTE dbo.updateInAndOutPunchType @io_id,@io_punch_type,@io_modified_by",
                io_id, io_punch_type, io_modified_by).ToList().FirstOrDefault();

            return dbResults;
        }

        public DbResult updateInAndOutPunchTypeUserWise(InAndOut inAndOut)
        {
            var io_u_id = new SqlParameter("io_u_id", inAndOut.io_u_id + "");
            var io_punch_type = new SqlParameter("io_punch_type", inAndOut.io_punch_type + "");
            var io_created_by = new SqlParameter("io_created_by", inAndOut.io_created_by + "");

            var dbResults = db.DbResult.FromSqlRaw<DbResult>("EXECUTE dbo.updateInAndOutPunchTypeUserWise @io_u_id,@io_punch_type,@io_created_by",
                io_u_id, io_punch_type, io_created_by).ToList().FirstOrDefault();

            return dbResults;
        }
    }
}
