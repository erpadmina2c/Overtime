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
    }
}
