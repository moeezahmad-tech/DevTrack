using System;
using System.Collections.Generic;
using DevTrack.DataAccess.DAL;
using DevTrack.DataAccess.Models;

namespace DevTrack.BusinessLogic.Managers
{
    public class ReportManager
    {
        ReportDAL dal = new ReportDAL();

        public List<ReportModel> GetAll()
        {
            return dal.GetAll();
        }

        public void Submit(ReportModel r)
        {
            if (string.IsNullOrWhiteSpace(r.ReportID))
                throw new Exception("Report ID is required.");
            if (string.IsNullOrWhiteSpace(r.ReportTitle))
                throw new Exception("Report Title is required.");
            if (dal.IDExists(r.ReportID))
                throw new Exception("Report ID already exists.");
            r.StatusUpdate = "Pending";
            dal.Insert(r);
        }

        public void Approve(string reportID)
        {
            dal.UpdateStatus(reportID, "Approved");
        }

        public void Reject(string reportID)
        {
            dal.UpdateStatus(reportID, "Rejected");
        }

        public void Delete(string reportID)
        {
            dal.Delete(reportID);
        }
    }
}
