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
    public class LeaveRepository : ILeave
    {
        private readonly DBContext db;

        public LeaveRepository(DBContext _db)
        {
            db = _db;
        }

        public DbResult ApproveLeave(int id, string type, int u_id)
        {
            var _id = new SqlParameter("id", id + "");
            var _type = new SqlParameter("type", type + "");
            var _u_id = new SqlParameter("u_id", u_id + "");
            var dbResults = db.DbResult.FromSqlRaw<DbResult>("EXECUTE dbo.ApproveLeave @id,@type,@u_id", _id, _type, _u_id)
             .ToList().FirstOrDefault();

            return dbResults;
        }

        public DbResult AuthorizeLeave(int l_id, string l_authorization,int u_id)
        {
            var _l_id = new SqlParameter("l_id", l_id + "");
            var _l_authorization = new SqlParameter("l_authorization", l_authorization + "");
            var _u_id = new SqlParameter("u_id", u_id + "");
            var dbResults = db.DbResult.FromSqlRaw<DbResult> ("EXECUTE dbo.AuthorizeLeave @l_id,@l_authorization,@u_id", _l_id, _l_authorization, _u_id)
             .ToList().FirstOrDefault();

            return dbResults;
        }


        public DbResultWithObject createOrUpdateLeave(Leave leave)
        {
            var l_id = new SqlParameter("l_id", leave.l_id + "");
            var l_dep_id = new SqlParameter("l_dep_id", leave.l_dep_id + "");
            var l_designation = new SqlParameter("l_designation", leave.l_designation + "");
            var l_type = new SqlParameter("l_type", leave.l_type + "");
            var l_reason = new SqlParameter("l_reason", leave.l_reason + "");
            var l_leave_for = new SqlParameter("l_leave_for", leave.l_leave_for + "");
            var l_leave_from = new SqlParameter("l_leave_from", leave.l_leave_from + "");
            var l_leave_to = new SqlParameter("l_leave_to", leave.l_leave_to + "");
            var l_salary_required = new SqlParameter("l_salary_required", leave.l_salary_required + "");
            var l_salary_month = new SqlParameter("l_salary_month", leave.l_salary_month + "");
            var l_required_amount = new SqlParameter("l_required_amount", leave.l_required_amount + "");
            var l_required_date = new SqlParameter("l_required_date", leave.l_required_date + "");
            var l_address = new SqlParameter("l_address", leave.l_address + "");
            var l_contact_no1 = new SqlParameter("l_contact_no1", leave.l_contact_no1 + "");
            var l_contact_no2 = new SqlParameter("l_contact_no2", leave.l_contact_no2 + "");
            var l_cre_by = new SqlParameter("l_cre_by", leave.l_cre_by + "");

            var dbResults = db.DbResultWithObject.FromSqlRaw<DbResultWithObject>
                ("EXECUTE dbo.createOrUpdateLeave @l_id,@l_dep_id,@l_designation,@l_type,@l_reason,@l_leave_for,@l_leave_from," +
                "@l_leave_to,@l_salary_required,@l_salary_month,@l_required_amount,@l_required_date,@l_address,@l_contact_no1,@l_contact_no2,@l_cre_by",
                l_id,l_dep_id, l_designation, l_type, l_reason, l_leave_for, l_leave_from, l_leave_to, l_salary_required, l_salary_month,
                l_required_amount, l_required_date, l_address, l_contact_no1, l_contact_no2, l_cre_by)
                .ToList().FirstOrDefault();

            return dbResults;
        }

        public DbResult deleteLeaveApplication(int l_id, int u_id)
        {
            var _l_id = new SqlParameter("l_id", l_id + "");
            var _u_id = new SqlParameter("u_id", u_id + "");
            var dbResults = db.DbResult.FromSqlRaw<DbResult>("EXECUTE dbo.deleteLeaveApplication @l_id,@u_id", _l_id, _u_id)
             .ToList().FirstOrDefault();

            return dbResults;
        }

        public Leave getLeave(int l_id)
        {
            var _l_id = new SqlParameter("l_id", l_id + "");
            var leaves = db.Leaves.FromSqlRaw<Leave>("EXECUTE dbo.getLeave @l_id", _l_id).ToList().FirstOrDefault();

            return leaves;
        }

        public List<Leave> getLeaveApplicationsForHrApproval(int u_id)
        {
            var _u_id = new SqlParameter("u_id", u_id + "");
            var leaves = db.Leaves.FromSqlRaw<Leave>("EXECUTE dbo.getLeaveApplicationsForHrApproval @u_id", _u_id).ToList();

            return leaves;
        }

        public List<Leave> getLeaveApplicationsForReview(int u_id)
        {
            var _u_id = new SqlParameter("u_id", u_id + "");
            var leaves = db.Leaves.FromSqlRaw<Leave>("EXECUTE dbo.getLeaveApplicationsForReview @u_id", _u_id).ToList();

            return leaves;
        }

        public List<Leave> getLeaveDetailsOfAEmployee(int u_id)
        {
            var _u_id = new SqlParameter("u_id", u_id + "");
            var leaves = db.Leaves.FromSqlRaw<Leave>("EXECUTE dbo.getLeaveDetailsOfAEmployee @u_id", _u_id).ToList();

            return leaves;
        }

        public DataTable getLeaveReport(int u_id, DateTime from, DateTime to, string type, string fullhistory)
        {
            DataTable dt = new DataTable();
            var conn = db.Database.GetDbConnection();
            string from2 = "";
            string to2 = "";


            if (fullhistory == "true")
            {
                from2 = ""; to2 = "";
            }
            else
            { 
                from2 = "" + from; to2 = "" + to;
            }
            try
            {

                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = @"exec [dbo].[getLeaveReport] 
                    @u_id='" + u_id + "',@from='" + from2 + "',@to='" + to2 + "'," +
                    "@type='" + type + "',@fullhistory='" + fullhistory + "'";
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

      

        public List<Leave> getLeaveRequests(int u_id)
        {
            var _u_id = new SqlParameter("u_id", u_id + "");
            var leaves = db.Leaves.FromSqlRaw<Leave>("EXECUTE dbo.getLeaveRequests @u_id", _u_id).ToList();

            return leaves;
        }

        public RemainingLeave getRemainingLeave(int u_id)
        {
            var _u_id = new SqlParameter("u_id", u_id + "");
            var leave = db.RemainingLeaves.FromSqlRaw<RemainingLeave>("EXECUTE dbo.getRemainingLeave @u_id", _u_id).ToList().FirstOrDefault();

            return leave;
        }
    }
}
