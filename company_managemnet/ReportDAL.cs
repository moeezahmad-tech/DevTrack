using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using CompanyManagement.Models;

namespace CompanyManagement.DAL
{
    public class ReportDAL
    {
        public List<ReportModel> GetAll()
        {
            List<ReportModel> list = new List<ReportModel>();
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT Report_ID, Report_Title, Report_Description, " +
                             "Submission_Date, Status_Update FROM Reports ORDER BY Report_ID";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ReportModel r = new ReportModel();
                    r.ReportID          = reader.GetString("Report_ID");
                    r.ReportTitle       = reader.GetString("Report_Title");
                    r.ReportDescription = reader.IsDBNull(reader.GetOrdinal("Report_Description")) ? "" : reader.GetString("Report_Description");
                    r.SubmissionDate    = reader.GetDateTime("Submission_Date");
                    r.StatusUpdate      = reader.GetString("Status_Update");
                    list.Add(r);
                }
                reader.Close();
            }
            finally { conn.Close(); }
            return list;
        }

        public bool Insert(ReportModel r)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "INSERT INTO Reports (Report_ID, Report_Title, Report_Description, " +
                             "Submission_Date, Status_Update) VALUES (@rid, @title, @desc, @date, @status)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@rid",    r.ReportID);
                cmd.Parameters.AddWithValue("@title",  r.ReportTitle);
                cmd.Parameters.AddWithValue("@desc",   r.ReportDescription);
                cmd.Parameters.AddWithValue("@date",   r.SubmissionDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@status", r.StatusUpdate);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex) { throw new Exception("Insert Report: " + ex.Message); }
            finally { conn.Close(); }
        }

        public bool UpdateStatus(string reportID, string newStatus)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "UPDATE Reports SET Status_Update = @status WHERE Report_ID = @rid";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@status", newStatus);
                cmd.Parameters.AddWithValue("@rid",    reportID);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally { conn.Close(); }
        }

        public bool Delete(string reportID)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "DELETE FROM Reports WHERE Report_ID = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", reportID);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally { conn.Close(); }
        }

        public bool IDExists(string reportID)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Reports WHERE Report_ID = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", reportID);
                return (long)cmd.ExecuteScalar() > 0;
            }
            finally { conn.Close(); }
        }
    }
}
