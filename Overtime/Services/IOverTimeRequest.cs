﻿using Overtime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Overtime.Services
{
   public interface IOverTimeRequest
    {
        IEnumerable<OverTimeRequest> GetOvertimeRequests { get; }
        IEnumerable<OverTimeRequest> GetMyOvertimeRequests(int Currentuserid);
        IEnumerable<OverTimeRequest> GetRequestForApprovals(int id);

        IEnumerable<OverTimeRequest> GetReports(int rq_dep_id, DateTime rq_start_time, DateTime rq_end_time, int role_id, int rq_cre_by, DateTime rq_cre_date, string approve);
        OverTimeRequest GetOverTimeRequest(int id);

        void Add(OverTimeRequest overTimeRequest);

        void Remove(int id);

        void Approve(int id);
        void Update(OverTimeRequest overTimeRequest);
        IEnumerable<OvCustomModel> getConsolidatedAsync(int rq_dep_id, DateTime startDate, DateTime endDate, int rq_cre_by);
        dynamic GetMyOnProcessRequests(int u_id);
        IEnumerable<OverTimeRequest> GetMyLiveOvertimeRequest(int u_id);
        object GetAllLiveOvertimeRequest(int u_id);
        IEnumerable<OverTimeRequest> getAllHoldDocuments();
        object getConsolidateByType(DateTime startDate, DateTime endDate, string type);
        decimal getWorkingHourByDocument(int v1, int v2);
        IEnumerable<OverTimeRequest> getApprovalsBySearch(int rq_dep_id, DateTime rq_start_time, DateTime rq_end_time, int role_id, int rq_cre_by, DateTime rq_cre_date, string approve,int id);
        DbResult jq_Approve(int id,int u_id);
        DbResult JQ_Reject(int id, string reason, int u_id);
    }
}
