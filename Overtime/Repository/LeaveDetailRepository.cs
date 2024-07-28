using Microsoft.Data.SqlClient;
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
    public class LeaveDetailRepository : ILeaveDetail
    {
        private readonly DBContext db;

        public LeaveDetailRepository(DBContext _db)
        {
            db = _db;
        }

        public IEnumerable<LeaveDetail> GetLeavetypes => db.LeaveDetail;

        public string AddUpdateLeave(LeaveDetail obj, string flag, int UserId)
        {
            string returnResult = "";

            var conn = db.Database.GetDbConnection();
            try
            {

                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = @"exec spAddUpdateLeave @empId = '" + obj.EmpName + "',@Startdate = '" + obj.StartDate + "',@EndDate = '" + obj.EndDate + "',@LeavetypeId = '" + obj.LeaveType + "',@User = '" + UserId + "',@Flag = '" + flag + "'" +
                        ",@LeaveId = '" + obj.LeaveId + "'";
                    command.CommandText = query;


                    returnResult =command.ExecuteScalar().ToString();

                }
            }
            catch (Exception ex)
            {
                conn.Close();
            }
            finally
            {
                conn.Close();
            }

            return returnResult;
        }

     
        public string DeleteLeave(int LeaveId)
        {
            string returnResult = "";

            var conn = db.Database.GetDbConnection();
            try
            {

                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = @"exec spDeleteLeave @LeaveId = '" + LeaveId + "'";
                    command.CommandText = query;


                    returnResult = command.ExecuteScalar().ToString();

                }
            }
            catch (Exception ex)
            {
                conn.Close();
            }
            finally
            {
                conn.Close();
            }

            return returnResult;
        }

        public LeaveDetail GetLeavetype(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(LeaveDetail leavetype)
        {
            throw new NotImplementedException();
        }
    }
}
