using Microsoft.EntityFrameworkCore;
using Overtime.Models;
using Overtime.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Repository
{
    public class BioMatrixRepository : IBioMatrix
    {
        private readonly DBContext db;

        public BioMatrixRepository(DBContext _db)
        {
            db = _db;
        }

       

        public IEnumerable<Attendance> GetAttendance(string date)
        {
            List<Attendance> result = new List<Attendance>();
            var conn = db.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = @"exec GetAttendance @date = '" + date + "'";
                    command.CommandText = query;
                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Attendance attendance = new Attendance();
                            attendance.id = int.Parse(reader["id"].ToString());
                            attendance.u_department = reader["u_department"].ToString();
                            attendance.u_full_name = reader["u_full_name"].ToString();
                            attendance.u_name = reader["u_name"].ToString();
                            attendance.u_email = reader["u_email"].ToString();
                            attendance.u_status = reader["u_status"].ToString();
                            attendance.u_remark = reader["u_remark"].ToString();
                            attendance.u_attendance_date = DateTime.Parse(reader["u_attendance_date"].ToString());
                            attendance.OfficeIn = reader["OfficeIn"].ToString();

                            result.Add(attendance);
                        }
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }

            return result;
        }


       

        public void Add(MachineDetail machine)
        {
            db.MachineDetails.Add(machine);
            db.SaveChanges();
        }
        public MachineDetail GetMachine(int machine_Id)
        {
            MachineDetail machine = db.MachineDetails.Find(machine_Id);
            return machine;
        }
        public DataTable GetMonthReport(DateTime rq_start_time, DateTime rq_end_time)
        {
            DataTable dt = new DataTable();
            var conn = db.Database.GetDbConnection();
            try
            {

                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = @"exec GET_ATTENDANCEREPORT @STARTDATE = '" + rq_start_time + "',@ENDDATE = '" + rq_end_time + "'";
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
                conn.Close();
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }


        public IEnumerable<LeaveDetail> GetLeaveDetail()
        {
            List<LeaveDetail> result = new List<LeaveDetail>();
            var conn = db.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = @"exec spGetLeaveDetail";
                    command.CommandText = query;
                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            LeaveDetail leave = new LeaveDetail();
                            leave.LeaveId = Convert.ToInt32(reader["LeaveId"]);
                            leave.EmpName = reader["EmpName"].ToString();
                            System.Diagnostics.Trace.WriteLine(reader["StartDate"].ToString());
                            if(!reader["StartDate"].ToString().Equals("")) leave.StartDate =DateTime.Parse(reader["StartDate"].ToString());
                            if (!reader["EndDate"].ToString().Equals("")) leave.EndDate = DateTime.Parse(reader["EndDate"].ToString());
                            leave.MarkedBy = reader["MarkedBy"].ToString();
                            leave.MarkedDate = reader["MarkedDate"].ToString();
                            leave.LastModifiedBy = reader["LastModifiedBy"].ToString();
                            leave.LastModfiedDate = reader["LastModfiedDate"].ToString();
                            leave.LeaveType = reader["LeaveType"].ToString();
                            leave.CurrentStatus = reader["CurrentStatus"].ToString();

                            result.Add(leave);
                        }
                    }
                    reader.Dispose();
                }
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

    }
}
