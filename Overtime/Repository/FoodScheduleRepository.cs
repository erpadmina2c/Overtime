using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
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

    public class FoodScheduleRepository : IFoodSchedule
    {
        private readonly DBContext db;
        public FoodScheduleRepository(DBContext _db)
        {
            db = _db;
        }

        public Result AddFoodSchedules(FoodSchedule foodSchedule)
        {
            Result result = new Result();
            try
            {
                db.Foodschedule.Add(foodSchedule);
                db.SaveChanges();
                result.Message = "Success";
            }
            catch (Exception ex)
            {
                result.Message = ex.InnerException.ToString();
            }
         
            return result;
        }

        public Result GetUserFoodDetails(FoodSchedule foodSchedule)
        {
            Result result = new Result();
            try
            {
                var valuedate = db.Foodschedule.Where(t => t.F_userid == foodSchedule.F_userid && t.f_date.Value.Date == foodSchedule.f_date.Value.Date).ToList();

                if(valuedate != null)
                {
                    if (valuedate.Count == 0)
                    {
                        result.Message = "NotExist";
                    }
                    else
                    {
                        result.Message = "Exist";
                    }
                }
                else
                {
                    result.Message = "null";
                }
            }
            catch(Exception ex)
            {
                result.Message = ex.InnerException.ToString();
            }
            return result;
        }

        public DataTable GetFoodFeedBackReportByDate(DateTime startDate, DateTime endDate,int u_id)
        {
            DataTable dt = new DataTable();
            var conn = db.Database.GetDbConnection();
            try
            {

                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = @"exec usp_Feedback_Report @startDate = '" + startDate + "',@endDate='" + endDate + "', @u_id = '" + u_id + "'";
                    command.CommandText = query;

                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {

                Trace.WriteLine(ex.InnerException);
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
