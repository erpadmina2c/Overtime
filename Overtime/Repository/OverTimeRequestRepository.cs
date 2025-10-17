using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NHibernate.Util;
using Overtime.Models;
using Overtime.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Overtime.Repository
{
    public class OverTimeRequestRepository : IOverTimeRequest
    {
        private DBContext db;
        private IWorkingHour iworkinghour;

        public OverTimeRequestRepository(DBContext _db,IWorkingHour _iworkinghour)
        {
            db = _db;
            iworkinghour = _iworkinghour;
        }

        public IEnumerable<OverTimeRequest> GetOvertimeRequests => db.OverTimeRequest;

        public IEnumerable<OverTimeRequest> GetMyOvertimeRequests(int Currentuserid)
        {
            var query = from u in db.OverTimeRequest
                        join d in db.Departments
                        on u.rq_dep_id equals d.d_id
                        join j in db.Users
                        on u.rq_cre_by equals j.u_id
                        join x in db.Users
                        on u.rq_cre_for equals x.u_id
                        where u.rq_status == 0
                        where u.rq_cre_by== Currentuserid
                        select new OverTimeRequest
                        {
                            rq_id = u.rq_id,
                            rq_cre_for = u.rq_cre_for,
                            rq_cre_for_name = x.u_full_name,
                            rq_cre_for_emp_id = x.u_name,
                            rq_workflow_id = u.rq_workflow_id,
                            rq_doc_id = u.rq_doc_id,
                            rq_description = u.rq_description,
                            rq_dep_id = u.rq_dep_id,
                            rq_dep_description = d.d_description,
                            rq_end_time=u.rq_end_time,
                            rq_active_yn = u.rq_active_yn,
                            rq_start_time = u.rq_start_time,
                            rq_status = u.rq_status,
                            rq_remarks = u.rq_remarks,
                            rq_hold_yn = u.rq_hold_yn,
                            rq_hold_date = u.rq_hold_date,
                            rq_hold_by = u.rq_hold_by,
                            rq_cre_by = u.rq_cre_by,
                            rq_cre_by_name = j.u_full_name,
                            rq_cre_date = u.rq_cre_date,
                        };
            return query;
        }


        public IEnumerable<OverTimeRequest> GetRequestForApprovals(int id)
        {
            var qry= from u in db.Users
                        where u.u_id == id
                        select u;
            User currentUser = qry.FirstOrDefault();

            int workflow = (from u in db.Documents
                            where u.dc_id == 1
                            select u.dc_workflow_id).FirstOrDefault();
           

            List<OverTimeRequest> result = new List<OverTimeRequest>();
            var conn = db.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {

                    string query = @"select OverTimeRequest.*,d_description rq_dep_description,b.u_name rq_cre_for_emp_id,b.u_full_name rq_cre_for_name,
                                    a.u_name+' - '+a.u_full_name rq_cre_by_name,
                                    [dbo].[getRoleByWorkflowPriority](rq_workflow_id,rq_status) rq_current_position,
                                    [dbo].[get_deduction_from_overtime](rq_id,rq_doc_id) rq_deductions,
                                    ([dbo].[fn_GetTotalWorkingHours](rq_start_time, rq_end_time)) eligible_hour
                                    from OverTimeRequest
                                    join Departments on rq_dep_id=d_id
                                    join workflowDetails on rq_workflow_id=wd_workflow_id
                                    join UserDepartments on rq_dep_id=ud_depart_id
                                    join users m on m.u_role_id=wd_role_id
                                    join Users a on rq_cre_by=a.u_id
                                    join Users b on rq_cre_for=b.u_id
                                    where 1=1  and ud_user_id = m.u_id and rq_status = wd_priority and m.u_id=" + id;
                    /*var query = from u in db.OverTimeRequest
                                join w in db.WorkflowDetails
                                  on u.rq_workflow_id equals w.wd_workflow_id
                                join d in db.Departments
                                on u.rq_dep_id equals d.d_id
                                join ud in db.UserDepartments on u.rq_dep_id equals ud.ud_depart_id
                                join m in db.Users on
                                 w.wd_role_id equals m.u_role_id
                               
                                where u.rq_status == w.wd_priority
                                where m.u_id == id
                                where ud.ud_user_id == m.u_id*/

                    command.CommandText = query;
                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            OverTimeRequest overTimeRequest = new OverTimeRequest();
                            overTimeRequest.rq_id             =Int32.Parse(reader["rq_id"].ToString()) ;
                            overTimeRequest.rq_cre_for        =Int32.Parse(reader["rq_cre_for"].ToString()) ;
                            overTimeRequest.rq_cre_for_name   =reader["rq_cre_for_name"].ToString() ;
                            overTimeRequest.rq_cre_for_emp_id =reader["rq_cre_for_emp_id"].ToString() ;
                            overTimeRequest.rq_workflow_id    = Int32.Parse(reader["rq_workflow_id"].ToString()) ;
                            overTimeRequest.rq_doc_id         = Int32.Parse(reader["rq_doc_id"].ToString()) ;
                            overTimeRequest.rq_description    =reader["rq_description"].ToString() ;
                            overTimeRequest.rq_dep_id         = Int32.Parse(reader["rq_dep_id"].ToString()) ;
                            overTimeRequest.rq_dep_description=reader["rq_dep_description"].ToString() ;
                            overTimeRequest.rq_active_yn      =reader["rq_active_yn"].ToString() ;
                            if (!reader["rq_end_time"].ToString().Equals(""))
                            {
                                overTimeRequest.rq_end_time = DateTime.Parse(reader["rq_end_time"].ToString());
                            }
                            overTimeRequest.rq_start_time     = DateTime.Parse(reader["rq_start_time"].ToString()) ;
                            if (!reader["eligible_hour"].ToString().Equals(""))
                            {
                                overTimeRequest.rq_working_hours = Decimal.Parse(reader["eligible_hour"].ToString());
                            }
                           
                            if (!reader["rq_deductions"].ToString().Equals(""))
                            {
                                overTimeRequest.rq_deductions = Decimal.Parse(reader["rq_deductions"].ToString());
                            }
                            overTimeRequest.rq_status         = Int32.Parse(reader["rq_status"].ToString()) ;
                            overTimeRequest.rq_current_position = reader["rq_current_position"].ToString();
                            
                            if (!reader["rq_hold_by"].ToString().Equals(""))
                            {
                                overTimeRequest.rq_hold_by = Int32.Parse(reader["rq_hold_by"].ToString());
                                overTimeRequest.rq_hold_yn = reader["rq_hold_yn"].ToString();
                                overTimeRequest.rq_hold_date = DateTime.Parse(reader["rq_hold_date"].ToString());
                            }
                           
                            overTimeRequest.rq_remarks        =reader["rq_remarks"].ToString() ;
                            overTimeRequest.rq_cre_by         = Int32.Parse(reader["rq_cre_by"].ToString()) ;
                            overTimeRequest.rq_cre_by_name    =reader["rq_cre_by_name"].ToString() ;
                            overTimeRequest.rq_cre_date = DateTime.Parse(reader["rq_cre_date"].ToString());



                            result.Add(overTimeRequest);
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

        public void Add(OverTimeRequest overTimeRequest)
        {
            db.OverTimeRequest.Add(overTimeRequest);
            db.SaveChanges();
        }

        public void Approve(int id)
        {

        }

        public OverTimeRequest GetOverTimeRequest(int id)
        {
           
            OverTimeRequest overTimeRequest = db.OverTimeRequest.Find(id);
            return overTimeRequest;
        }

        public void Remove(int id)
        {
            OverTimeRequest overTimeRequest = db.OverTimeRequest.Find(id);
            db.OverTimeRequest.Remove(overTimeRequest);
            db.SaveChanges();
        }

        public void Update(OverTimeRequest overTimeRequest)
        {
            db.OverTimeRequest.Update(overTimeRequest);
            db.SaveChanges();

        }

        public IEnumerable<OverTimeRequest> GetReportsold(int rq_dep_id, DateTime rq_start_time, DateTime rq_end_time, int rq_role_id, int rq_cre_by, DateTime rq_cre_date, string approve)
        {
            var query = from u in db.OverTimeRequest
                        join d in db.Departments
                          on u.rq_dep_id equals d.d_id
                        join k in db.Users
                         on u.rq_cre_by equals k.u_id
                        join x in db.Users
                        on u.rq_cre_for equals x.u_id
                        orderby u.rq_id descending
                        select new OverTimeRequest
                        {
                            rq_id = u.rq_id,
                            rq_cre_for = u.rq_cre_for,
                            rq_cre_for_name = x.u_full_name,
                            rq_cre_for_emp_id = x.u_name,
                            rq_workflow_id = u.rq_workflow_id,
                            rq_doc_id = u.rq_doc_id,
                            rq_description = u.rq_description,
                            rq_dep_id = u.rq_dep_id,
                            rq_dep_description = d.d_description,
                            rq_active_yn = u.rq_active_yn,
                            rq_end_time = u.rq_end_time,
                            rq_start_time = u.rq_start_time,
                            rq_no_of_hours = u.rq_no_of_hours,
                            rq_status = u.rq_status,
                            rq_hold_yn = u.rq_hold_yn,
                            rq_hold_date = u.rq_hold_date,
                            rq_hold_by = u.rq_hold_by,
                            rq_remarks = u.rq_remarks,
                            rq_cre_by = u.rq_cre_by,
                            rq_cre_by_name = k.u_full_name,
                            rq_cre_date = u.rq_cre_date,
                        };
            if (rq_dep_id != 0)
                query = query.Where(x => x.rq_dep_id == rq_dep_id);
            if (!rq_start_time.ToString().Equals("1/1/0001 12:00:00 AM") && !rq_end_time.ToString().Equals("1/1/0001 12:00:00 AM"))
                query = query.Where(x => x.rq_start_time >= rq_start_time && rq_start_time<=rq_end_time );
            if (rq_cre_by != 0)
                query = query.Where(x => x.rq_cre_by == rq_cre_by);
            if (!rq_cre_date.ToString().Equals("1/1/0001 12:00:00 AM"))
                query = query.Where(x => x.rq_cre_date == rq_cre_date);
            return query;
        }

        public IEnumerable<OverTimeRequest> GetReports(int rq_dep_id, DateTime rq_start_time, DateTime rq_end_time, int rq_role_id, int rq_cre_by, DateTime rq_cre_date, string approve)
        {
            List<OverTimeRequest> result = new List<OverTimeRequest>();
            var conn = db.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {

                    string query = @"select OverTimeRequest.*,d_description rq_dep_description,b.u_name rq_cre_for_emp_id,b.u_full_name rq_cre_for_name,
                                    a.u_name+' - '+a.u_full_name rq_cre_by_name,
                                    [dbo].[getRoleByWorkflowPriority](rq_workflow_id,rq_status) rq_current_position,
                                    [dbo].[get_deduction_from_overtime](rq_id,rq_doc_id) rq_deductions,
                                    ([dbo].[fn_GetTotalWorkingHours](rq_start_time, rq_end_time)) eligible_hour
                                    from OverTimeRequest
                                    join Departments on rq_dep_id=d_id
                                    join Users a on rq_cre_by=a.u_id
                                    join Users b on rq_cre_for=b.u_id
                                    where 1=1 ";


                    if (!rq_start_time.ToString().Equals("1/1/0001 12:00:00 AM") && !rq_end_time.ToString().Equals("1/1/0001 12:00:00 AM"))
                        query += " and rq_start_time between '" + rq_start_time + @"' and '" + rq_end_time + @"' ";
                    
                    if (rq_dep_id != 0) query += " and rq_dep_id='" + rq_dep_id + @"'";

                    if (rq_cre_by != 0)
                        query += " and rq_cre_by='" + rq_cre_by + @"'";


                    if (!rq_cre_date.ToString().Equals("1/1/0001 12:00:00 AM"))
                        query += " and CONVERT(date, rq_cre_date) ='" + rq_cre_date.Date + @"'";

                    
                  
                    command.CommandText = query;
                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            OverTimeRequest overTimeRequest = new OverTimeRequest();
                            overTimeRequest.rq_id             =Int32.Parse(reader["rq_id"].ToString()) ;
                            overTimeRequest.rq_cre_for        =Int32.Parse(reader["rq_cre_for"].ToString()) ;
                            overTimeRequest.rq_cre_for_name   =reader["rq_cre_for_name"].ToString() ;
                            overTimeRequest.rq_cre_for_emp_id =reader["rq_cre_for_emp_id"].ToString() ;
                            overTimeRequest.rq_workflow_id    = Int32.Parse(reader["rq_workflow_id"].ToString()) ;
                            overTimeRequest.rq_doc_id         = Int32.Parse(reader["rq_doc_id"].ToString()) ;
                            overTimeRequest.rq_description    =reader["rq_description"].ToString() ;
                            overTimeRequest.rq_dep_id         = Int32.Parse(reader["rq_dep_id"].ToString()) ;
                            overTimeRequest.rq_dep_description=reader["rq_dep_description"].ToString() ;
                            overTimeRequest.rq_active_yn      =reader["rq_active_yn"].ToString() ;
                            if (!reader["rq_end_time"].ToString().Equals(""))
                            {
                                overTimeRequest.rq_end_time = DateTime.Parse(reader["rq_end_time"].ToString());
                            }
                            overTimeRequest.rq_start_time     = DateTime.Parse(reader["rq_start_time"].ToString()) ;
                            overTimeRequest.rq_working_hours = Decimal.Parse(reader["rq_no_of_hours"].ToString()) ;
                            overTimeRequest.rq_status         = Int32.Parse(reader["rq_status"].ToString()) ;
                            overTimeRequest.rq_current_position = reader["rq_current_position"].ToString();
                            
                            if (!reader["rq_hold_by"].ToString().Equals(""))
                            {
                                overTimeRequest.rq_hold_by = Int32.Parse(reader["rq_hold_by"].ToString());
                                overTimeRequest.rq_hold_yn = reader["rq_hold_yn"].ToString();
                                overTimeRequest.rq_hold_date = DateTime.Parse(reader["rq_hold_date"].ToString());
                            }

                            if (!reader["eligible_hour"].ToString().Equals(""))
                            {
                                overTimeRequest.rq_working_hours = Decimal.Parse(reader["eligible_hour"].ToString());
                            }

                            if (!reader["rq_deductions"].ToString().Equals(""))
                            {
                                overTimeRequest.rq_deductions = Decimal.Parse(reader["rq_deductions"].ToString());
                            }
                            overTimeRequest.rq_remarks        =reader["rq_remarks"].ToString() ;
                            overTimeRequest.rq_cre_by         = Int32.Parse(reader["rq_cre_by"].ToString()) ;
                            overTimeRequest.rq_cre_by_name    =reader["rq_cre_by_name"].ToString() ;
                            overTimeRequest.rq_cre_date = DateTime.Parse(reader["rq_cre_date"].ToString());



                            result.Add(overTimeRequest);
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
        public IEnumerable<OvCustomModel> getConsolidatedAsync(int rq_dep_id, DateTime startDate, DateTime endDate, int rq_cre_for)
        {
            List<OvCustomModel> result = new List<OvCustomModel>();
            var conn = db.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {

                    string query = @"select rq_cre_for,u_name,u_full_name,rq_dep_id,d_description,
                                     sum(CASE
                                     WHEN   rq_status>=[dbo].get_approved_priority(rq_workflow_id) THEN
                                     ([dbo].[fn_GetTotalWorkingHours](rq_start_time, rq_end_time)-[dbo].[get_deduction_from_overtime](rq_id,rq_doc_id))
                                     ELSE 0
                                     END) approved,
                                     sum(CASE
                                     WHEN   rq_status>=[dbo].get_approved_priority(rq_workflow_id)  THEN 0
                                     ELSE (([dbo].[fn_GetTotalWorkingHours](rq_start_time, rq_end_time)-[dbo].[get_deduction_from_overtime](rq_id,rq_doc_id)))
                                     END) unaproved,
                                     sum(
                                     [dbo].[fn_GetTotalWorkingHours](rq_start_time, rq_end_time)-[dbo].[get_deduction_from_overtime](rq_id,rq_doc_id)) hours
                                     from OverTimeRequest
                                     join Users on rq_cre_for = u_id
                                     join Departments on rq_dep_id = d_id
                                     where 1=1 and rq_end_time is not null and rq_status!=0  and  rq_description !='automation test'";

                   
                    if (!startDate.ToString().Equals("1/1/0001 12:00:00 AM") && !endDate.ToString().Equals("1/1/0001 12:00:00 AM"))
                        query += " and rq_start_time between '" + startDate + @"' and '" + endDate + @"' ";
                    if (rq_dep_id != 0) query += " and rq_dep_id='" + rq_dep_id + @"'";
                    if (rq_cre_for != 0) query += " and rq_cre_for='" + rq_cre_for + @"'";

                    query += " group by rq_cre_for,u_name,u_full_name,rq_dep_id,d_description";
                 
                    command.CommandText = query;
                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            OvCustomModel ovCustomModel = new OvCustomModel();
                            ovCustomModel.emp_id = reader.GetInt32(0);
                            ovCustomModel.emp_name = reader.GetString(1);
                            ovCustomModel.emp_full_name = reader.GetString(2);
                            ovCustomModel.emp_dep_id = reader.GetInt32(3);
                            ovCustomModel.emp_dep_name = reader.GetString(4);
                            ovCustomModel.work_hours = reader.GetDecimal(5);
                            ovCustomModel.emp_unapproved = reader.GetDecimal(6);
                            ovCustomModel.emp_total = reader.GetDecimal(7);

                            result.Add(ovCustomModel);
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

        public dynamic GetMyOnProcessRequests(int u_id)
        {
            var query = from u in db.OverTimeRequest
                        join d in db.Departments
                        on u.rq_dep_id equals d.d_id
                        join j in db.Users
                        on u.rq_cre_by equals j.u_id
                        join x in db.Users
                        on u.rq_cre_for equals x.u_id
                        where u.rq_cre_date>=DateTime.Now.AddDays(- 30)
                        where u.rq_cre_by == u_id
                        select new OverTimeRequest
                        {
                            rq_id = u.rq_id,
                            rq_cre_for = u.rq_cre_for,
                            rq_cre_for_name = x.u_full_name,
                            rq_cre_for_emp_id = x.u_name,
                            rq_workflow_id = u.rq_workflow_id,
                            rq_doc_id = u.rq_doc_id,
                            rq_description = u.rq_description,
                            rq_dep_id = u.rq_dep_id,
                            rq_dep_description = d.d_description,
                            rq_active_yn = u.rq_active_yn ??"Y",
                            rq_start_time = u.rq_start_time,
                            rq_no_of_hours = u.rq_no_of_hours,
                            rq_status = u.rq_status,
                            rq_end_time = u.rq_end_time,
                            rq_remarks = u.rq_remarks ??"",
                            rq_cre_by = u.rq_cre_by,
                            rq_hold_yn = u.rq_hold_yn,
                            rq_hold_date = u.rq_hold_date,
                            rq_hold_by = u.rq_hold_by,
                            rq_cre_by_name = j.u_full_name,
                            rq_cre_date = u.rq_cre_date,
                        };
            return query;
        }

        public IEnumerable<OverTimeRequest> GetMyLiveOvertimeRequest(int u_id)
        {
            var query = from u in db.OverTimeRequest
                        join d in db.Departments
                        on u.rq_dep_id equals d.d_id
                        join j in db.Users
                        on u.rq_cre_by equals j.u_id
                        join x in db.Users
                        on u.rq_cre_for equals x.u_id
                        where u.rq_status == 0
                        where u.rq_cre_by == u_id
                        where u.rq_end_time == null
                        select new OverTimeRequest
                        {
                            rq_id = u.rq_id,
                            rq_cre_for = u.rq_cre_for,
                            rq_cre_for_name = x.u_full_name,
                            rq_cre_for_emp_id = x.u_name,
                            rq_workflow_id = u.rq_workflow_id,
                            rq_doc_id = u.rq_doc_id,
                            rq_description = u.rq_description,
                            rq_dep_id = u.rq_dep_id,
                            rq_dep_description = d.d_description,
                            rq_active_yn = u.rq_active_yn ?? "Y",
                            rq_start_time = u.rq_start_time,
                            rq_no_of_hours = u.rq_no_of_hours,
                            rq_status = u.rq_status,
                            rq_end_time = u.rq_end_time,
                            rq_remarks = u.rq_remarks?? "...",
                            rq_hold_yn = u.rq_hold_yn,
                            rq_hold_date = u.rq_hold_date,
                            rq_hold_by = u.rq_hold_by,
                            rq_cre_by = u.rq_cre_by,
                            rq_cre_by_name = j.u_full_name,
                            rq_cre_date = u.rq_cre_date,
                        };
            return query;
        }

        public object GetAllLiveOvertimeRequest(int u_id)
        {
            var query = from u in db.OverTimeRequest
                        join d in db.Departments
                        on u.rq_dep_id equals d.d_id
                        join j in db.Users
                        on u.rq_cre_by equals j.u_id
                        join x in db.Users
                        on u.rq_cre_for equals x.u_id
                        where u.rq_status == 0
                        where u.rq_end_time == null
                        select new OverTimeRequest
                        {
                            rq_id = u.rq_id,
                            rq_cre_for = u.rq_cre_for,
                            rq_cre_for_name = x.u_full_name,
                            rq_cre_for_emp_id = x.u_name,
                            rq_workflow_id = u.rq_workflow_id,
                            rq_doc_id = u.rq_doc_id,
                            rq_description = u.rq_description,
                            rq_dep_id = u.rq_dep_id,
                            rq_dep_description = d.d_description,
                            rq_active_yn = u.rq_active_yn,
                            rq_start_time = u.rq_start_time,
                            rq_no_of_hours = u.rq_no_of_hours,
                            rq_status = u.rq_status,
                            rq_end_time = u.rq_end_time,
                            rq_remarks = u.rq_remarks,
                            rq_hold_yn=u.rq_hold_yn,
                            rq_hold_date=u.rq_hold_date,
                            rq_hold_by=u.rq_hold_by,
                            rq_cre_by = u.rq_cre_by,
                            rq_cre_by_name = j.u_full_name,
                            rq_cre_date = u.rq_cre_date,
                        };
            return query;
        }

        public IEnumerable<OverTimeRequest> getAllHoldDocuments()
        {
            var query = from u in db.OverTimeRequest
                        join d in db.Departments
                        on u.rq_dep_id equals d.d_id
                        join j in db.Users
                        on u.rq_cre_by equals j.u_id
                        join x in db.Users
                        on u.rq_cre_for equals x.u_id
                        where u.rq_status != 0
                        where u.rq_hold_yn=="Y"
                        select new OverTimeRequest
                        {
                            rq_id = u.rq_id,
                            rq_cre_for = u.rq_cre_for,
                            rq_cre_for_name = x.u_full_name,
                            rq_cre_for_emp_id = x.u_name,
                            rq_workflow_id = u.rq_workflow_id,
                            rq_doc_id = u.rq_doc_id,
                            rq_description = u.rq_description,
                            rq_dep_id = u.rq_dep_id,
                            rq_dep_description = d.d_description,
                            rq_active_yn = u.rq_active_yn,
                            rq_start_time = u.rq_start_time,
                            rq_no_of_hours = u.rq_no_of_hours,
                            rq_status = u.rq_status,
                            rq_end_time = u.rq_end_time,
                            rq_remarks = u.rq_remarks,
                            rq_hold_yn = u.rq_hold_yn,
                            rq_hold_date = u.rq_hold_date,
                            rq_hold_by = u.rq_hold_by,
                            rq_cre_by = u.rq_cre_by,
                            rq_cre_by_name = j.u_full_name,
                            rq_cre_date = u.rq_cre_date,
                        };
            return query;
        }

        public object getConsolidateByType(DateTime startDate, DateTime endDate, string type)
        {
            List<OvCustomModel> result = new List<OvCustomModel>();
            var conn = db.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string daterange = "";
                    if (!startDate.ToString().Equals("1/1/0001 12:00:00 AM") && !endDate.ToString().Equals("1/1/0001 12:00:00 AM"))
                    {
                        daterange = " and rq_start_time between '" + startDate + @"' and '" + endDate + @"' ";
                    }


                    string query = "";
                    if (type.Equals("Department"))
                    {
                        query = @"select rq_dep_id,d_description,
                            sum(CASE
                            WHEN   rq_status=[dbo].get_approved_priority(rq_workflow_id)  THEN
	                        ([dbo].[fn_GetTotalWorkingHours](rq_start_time, rq_end_time)-[dbo].[get_deduction_from_overtime](rq_id,rq_doc_id))
                            ELSE 0
                            END) approved,
                            sum(CASE
                                     WHEN   rq_status>=[dbo].get_approved_priority(rq_workflow_id)  THEN 0
                                    ELSE ([dbo].[fn_GetTotalWorkingHours](rq_start_time, rq_end_time)-[dbo].[get_deduction_from_overtime](rq_id,rq_doc_id))
                            END) unaproved,
                            sum(
						    [dbo].[fn_GetTotalWorkingHours](rq_start_time, rq_end_time)-[dbo].[get_deduction_from_overtime](rq_id,rq_doc_id)) hours
                            from OverTimeRequest
                            join Users on rq_cre_for = u_id
                            join Departments on rq_dep_id = d_id
                            where 1=1 " + daterange + @" and  rq_end_time is not null and rq_status!=0   and  rq_description !='automation test'
                            group by rq_dep_id,d_description";

                    }else
                    {
                        query = @"select rq_cre_for,u_name,u_full_name,
                            sum(CASE
                            WHEN   rq_status>=[dbo].get_approved_priority(rq_workflow_id)  THEN
	                        ([dbo].[fn_GetTotalWorkingHours](rq_start_time, rq_end_time)-[dbo].[get_deduction_from_overtime](rq_id,rq_doc_id))
                            ELSE 0
                            END) approved,
                            sum(CASE
                            WHEN  rq_status>=[dbo].get_approved_priority(rq_workflow_id)  THEN 0
                            ELSE ([dbo].[fn_GetTotalWorkingHours](rq_start_time, rq_end_time)-[dbo].[get_deduction_from_overtime](rq_id,rq_doc_id))
                            END) unaproved,
                            sum([dbo].[fn_GetTotalWorkingHours](rq_start_time, rq_end_time)-[dbo].[get_deduction_from_overtime](rq_id,rq_doc_id)) hours
                            from OverTimeRequest
                            join Users on rq_cre_for = u_id
                            join Departments on rq_dep_id = d_id
                            where 1=1 " + daterange + @" and rq_end_time is not null and rq_status!=0  and   rq_description !='automation test'
							group by rq_cre_for,u_name,u_full_name";
                    }

                
                   
                    
                    command.CommandText = query;
                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            OvCustomModel ovCustomModel = new OvCustomModel();
                            if (type.Equals("Department")){
                                ovCustomModel.emp_dep_id = int.Parse(reader["rq_dep_id"].ToString());
                                ovCustomModel.emp_dep_name = reader["d_description"].ToString();
                            }
                            else
                            {
                                ovCustomModel.emp_id = int.Parse(reader["rq_cre_for"].ToString());
                                ovCustomModel.emp_name = reader["u_name"].ToString();
                                ovCustomModel.emp_full_name = reader["u_full_name"].ToString();
                            }
                            
                            
                            ovCustomModel.work_hours = decimal.Parse(reader["approved"].ToString());
                            ovCustomModel.emp_unapproved = decimal.Parse(reader["unaproved"].ToString());
                            ovCustomModel.emp_total = decimal.Parse(reader["hours"].ToString());

                            result.Add(ovCustomModel);
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

        public decimal getWorkingHourByDocument(int fun_id, int doc_no)
        {
            decimal workinghour = 0;
            var conn = db.Database.GetDbConnection();
            try
            {
                
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                   

                    string  query = @"select
	                        [dbo].[fn_GetTotalWorkingHours](rq_start_time, rq_end_time) hour
                            from OverTimeRequest
                            where rq_id="+fun_id+ " and rq_doc_id=" + doc_no+"";
                   
                    command.CommandText = query;
                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            workinghour = decimal.Parse(reader["hour"].ToString());
                           
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

        public IEnumerable<OverTimeRequest> getApprovalsBySearch(int rq_dep_id, DateTime rq_start_time, DateTime rq_end_time, int role_id, int rq_cre_by, DateTime rq_cre_date, string approve,int id)
        {
            var qry = from u in db.Users
                      where u.u_id == id
                      select u;
            User currentUser = qry.FirstOrDefault();

            int workflow = (from u in db.Documents
                            where u.dc_id == 1
                            select u.dc_workflow_id).FirstOrDefault();


            List<OverTimeRequest> result = new List<OverTimeRequest>();
            var conn = db.Database.GetDbConnection();
            try
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {

                    string query = @"select OverTimeRequest.*,d_description rq_dep_description,b.u_name rq_cre_for_emp_id,b.u_full_name rq_cre_for_name,
                                    a.u_name+' - '+a.u_full_name rq_cre_by_name,
                                    [dbo].[getRoleByWorkflowPriority](rq_workflow_id,rq_status) rq_current_position,
                                    [dbo].[get_deduction_from_overtime](rq_id,rq_doc_id) rq_deductions,
                                    ([dbo].[fn_GetTotalWorkingHours](rq_start_time, rq_end_time)) eligible_hour
                                    from OverTimeRequest
                                    join Departments on rq_dep_id=d_id
                                    join workflowDetails on rq_workflow_id=wd_workflow_id
                                    join UserDepartments on rq_dep_id=ud_depart_id
                                    join users m on m.u_role_id=wd_role_id
                                    join Users a on rq_cre_by=a.u_id
                                    join Users b on rq_cre_for=b.u_id
                                    where 1=1  and ud_user_id = m.u_id and rq_status = wd_priority and m.u_id=" + id;

                    if (!rq_start_time.ToString().Equals("1/1/0001 12:00:00 AM") && !rq_end_time.ToString().Equals("1/1/0001 12:00:00 AM"))
                        query += " and rq_start_time between '" + rq_start_time + @"' and '" + rq_end_time + @"' ";

                    if (rq_dep_id != 0) query += " and rq_dep_id='" + rq_dep_id + @"'";

                    if (rq_cre_by != 0)
                        query += " and rq_cre_by='" + rq_cre_by + @"'";


                    if (!rq_cre_date.ToString().Equals("1/1/0001 12:00:00 AM"))
                        query += " and CONVERT(date, rq_cre_date) ='" + rq_cre_date.Date + @"'";

                    command.CommandText = query;
                    DbDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            OverTimeRequest overTimeRequest = new OverTimeRequest();
                            overTimeRequest.rq_id = Int32.Parse(reader["rq_id"].ToString());
                            overTimeRequest.rq_cre_for = Int32.Parse(reader["rq_cre_for"].ToString());
                            overTimeRequest.rq_cre_for_name = reader["rq_cre_for_name"].ToString();
                            overTimeRequest.rq_cre_for_emp_id = reader["rq_cre_for_emp_id"].ToString();
                            overTimeRequest.rq_workflow_id = Int32.Parse(reader["rq_workflow_id"].ToString());
                            overTimeRequest.rq_doc_id = Int32.Parse(reader["rq_doc_id"].ToString());
                            overTimeRequest.rq_description = reader["rq_description"].ToString();
                            overTimeRequest.rq_dep_id = Int32.Parse(reader["rq_dep_id"].ToString());
                            overTimeRequest.rq_dep_description = reader["rq_dep_description"].ToString();
                            overTimeRequest.rq_active_yn = reader["rq_active_yn"].ToString();
                            if (!reader["rq_end_time"].ToString().Equals(""))
                            {
                                overTimeRequest.rq_end_time = DateTime.Parse(reader["rq_end_time"].ToString());
                            }
                            overTimeRequest.rq_start_time = DateTime.Parse(reader["rq_start_time"].ToString());
                            if (!reader["eligible_hour"].ToString().Equals(""))
                            {
                                overTimeRequest.rq_working_hours = Decimal.Parse(reader["eligible_hour"].ToString());
                            }

                            if (!reader["rq_deductions"].ToString().Equals(""))
                            {
                                overTimeRequest.rq_deductions = Decimal.Parse(reader["rq_deductions"].ToString());
                            }
                            overTimeRequest.rq_status = Int32.Parse(reader["rq_status"].ToString());
                            overTimeRequest.rq_current_position = reader["rq_current_position"].ToString();

                            if (!reader["rq_hold_by"].ToString().Equals(""))
                            {
                                overTimeRequest.rq_hold_by = Int32.Parse(reader["rq_hold_by"].ToString());
                                overTimeRequest.rq_hold_yn = reader["rq_hold_yn"].ToString();
                                overTimeRequest.rq_hold_date = DateTime.Parse(reader["rq_hold_date"].ToString());
                            }

                            overTimeRequest.rq_remarks = reader["rq_remarks"].ToString();
                            overTimeRequest.rq_cre_by = Int32.Parse(reader["rq_cre_by"].ToString());
                            overTimeRequest.rq_cre_by_name = reader["rq_cre_by_name"].ToString();
                            overTimeRequest.rq_cre_date = DateTime.Parse(reader["rq_cre_date"].ToString());



                            result.Add(overTimeRequest);
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

        public DbResult jq_Approve(int id,int u_id)
        {
            DbResult dbResult = new DbResult();
            try
            {
                var _id = new SqlParameter("id", id + "");
                var _u_id = new SqlParameter("u_id", u_id + "");

                 dbResult = db.DbResult.FromSqlRaw<DbResult>("EXECUTE dbo.jq_Approve @id,@u_id", _id, _u_id).ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                dbResult.Message = ex.Message + " " + ex.InnerException;
            }
            return dbResult;
        }

        public DbResult JQ_Reject(int id, string reason, int u_id)
        {
            DbResult dbResult = new DbResult();
            try
            {
                var _id = new SqlParameter("id", id + "");
                var _reason = new SqlParameter("reason", reason + "");
                var _u_id = new SqlParameter("u_id", u_id + "");

                dbResult = db.DbResult.FromSqlRaw<DbResult>("EXECUTE dbo.JQ_Reject @id,@reason,@u_id", _id, _reason, _u_id).ToList().FirstOrDefault();

            }
            catch (Exception ex)
            {
                dbResult.Message = ex.Message + " " + ex.InnerException;
            }
            return dbResult;
        }

        public DataTable getOvertimeFullDetails(string reportrange, string type,int user)
        {
            DataTable dt = new DataTable();
            var conn = db.Database.GetDbConnection();
            try
            {

                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = @"exec [dbo].[getOvertimeFullDetails] @reportrange = '" + reportrange + "',@type = '" + type + "',@user ='" + user + "'";
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

        public DataTable getApprovedOtDetails(string reportrange, string type, int user)
        {
            DataTable dt = new DataTable();
            var conn = db.Database.GetDbConnection();
            try
            {

                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = @"exec [dbo].[getApprovedOtDetails] @reportrange = '" + reportrange + "',@type = '" + type + "',@user ='" + user + "'";
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

        public DataTable getOvertimeReportNew(string reportrange, string type, int user)
        {
            DataTable dt = new DataTable();
            var conn = db.Database.GetDbConnection();
            try
            {

                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    string query = @"exec [dbo].[getOvertimeReportNew] @reportrange = '" + reportrange + "',@type = '" + type + "',@user ='" + user + "'";
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
