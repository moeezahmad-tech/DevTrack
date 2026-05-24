using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using DevTrack.DataAccess.Models;
using DevTrack.DataAccess.Database;

namespace DevTrack.DataAccess.DAL
{
    public class ProjectDAL
    {
        public List<ProjectModel> GetAll()
        {
            List<ProjectModel> list = new List<ProjectModel>();
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT p.Project_ID, p.Project_Name, p.Dept_ID, " +
                             "COALESCE(d.Dept_Name,'—') AS Dept_Name, p.Start_Date, p.Status " +
                             "FROM Project p LEFT JOIN Department d ON p.Dept_ID = d.Dept_ID " +
                             "ORDER BY p.Project_ID";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ProjectModel pm = new ProjectModel();
                    pm.ProjectID   = reader.GetString("Project_ID");
                    pm.ProjectName = reader.GetString("Project_Name");
                    pm.DeptID      = reader.IsDBNull(reader.GetOrdinal("Dept_ID")) ? "" : reader.GetString("Dept_ID");
                    pm.DeptName    = reader.GetString("Dept_Name");
                    pm.StartDate   = reader.GetDateTime("Start_Date");
                    pm.Status      = reader.GetString("Status");
                    list.Add(pm);
                }
                reader.Close();
            }
            finally { conn.Close(); }
            return list;
        }

        public bool Insert(ProjectModel pm)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "INSERT INTO Project (Project_ID, Project_Name, Dept_ID, Start_Date, Status) " +
                             "VALUES (@pid, @name, @dept, @date, @status)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@pid",    pm.ProjectID);
                cmd.Parameters.AddWithValue("@name",   pm.ProjectName);
                cmd.Parameters.AddWithValue("@dept",   string.IsNullOrEmpty(pm.DeptID) ? (object)DBNull.Value : pm.DeptID);
                cmd.Parameters.AddWithValue("@date",   pm.StartDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@status", pm.Status);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex) { throw new Exception("Insert Project: " + ex.Message); }
            finally { conn.Close(); }
        }

        public bool Update(ProjectModel pm)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "UPDATE Project SET Project_Name=@name, Dept_ID=@dept, " +
                             "Start_Date=@date, Status=@status WHERE Project_ID=@pid";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name",   pm.ProjectName);
                cmd.Parameters.AddWithValue("@dept",   string.IsNullOrEmpty(pm.DeptID) ? (object)DBNull.Value : pm.DeptID);
                cmd.Parameters.AddWithValue("@date",   pm.StartDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@status", pm.Status);
                cmd.Parameters.AddWithValue("@pid",    pm.ProjectID);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally { conn.Close(); }
        }

        public bool Delete(string projectID)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "DELETE FROM Project WHERE Project_ID = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", projectID);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally { conn.Close(); }
        }

        public bool IDExists(string projectID)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Project WHERE Project_ID = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", projectID);
                return (long)cmd.ExecuteScalar() > 0;
            }
            finally { conn.Close(); }
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    public class TaskDAL
    {
        public List<TaskItemModel> GetAll()
        {
            List<TaskItemModel> list = new List<TaskItemModel>();
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT t.Task_ID, t.Title, t.Task_description, t.Priority, t.Deadline, " +
                             "t.Project_ID, COALESCE(pr.Project_Name,'—') AS Project_Name, " +
                             "t.Employee_ID, COALESCE(p.Name,'—') AS Employee_Name " +
                             "FROM Task t " +
                             "LEFT JOIN Project pr ON t.Project_ID = pr.Project_ID " +
                             "LEFT JOIN Person p ON t.Employee_ID = p.ID " +
                             "ORDER BY t.Task_ID";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TaskItemModel tm = new TaskItemModel();
                    tm.TaskID          = reader.GetString("Task_ID");
                    tm.Title           = reader.GetString("Title");
                    tm.TaskDescription = reader.IsDBNull(reader.GetOrdinal("Task_description")) ? "" : reader.GetString("Task_description");
                    tm.Priority        = reader.GetString("Priority");
                    tm.Deadline        = reader.GetDateTime("Deadline");
                    tm.ProjectID       = reader.IsDBNull(reader.GetOrdinal("Project_ID")) ? "" : reader.GetString("Project_ID");
                    tm.ProjectName     = reader.GetString("Project_Name");
                    tm.EmployeeID      = reader.IsDBNull(reader.GetOrdinal("Employee_ID")) ? 0 : reader.GetInt32("Employee_ID");
                    tm.EmployeeName    = reader.GetString("Employee_Name");
                    list.Add(tm);
                }
                reader.Close();
            }
            finally { conn.Close(); }
            return list;
        }

        public List<TaskItemModel> GetByEmployee(int employeeID)
        {
            List<TaskItemModel> list = new List<TaskItemModel>();
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT t.Task_ID, t.Title, t.Task_description, t.Priority, t.Deadline, " +
                             "t.Project_ID, COALESCE(pr.Project_Name,'—') AS Project_Name, " +
                             "t.Employee_ID, COALESCE(p.Name,'—') AS Employee_Name " +
                             "FROM Task t " +
                             "LEFT JOIN Project pr ON t.Project_ID = pr.Project_ID " +
                             "LEFT JOIN Person p ON t.Employee_ID = p.ID " +
                             "WHERE t.Employee_ID = @eid ORDER BY t.Deadline";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@eid", employeeID);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TaskItemModel tm = new TaskItemModel();
                    tm.TaskID          = reader.GetString("Task_ID");
                    tm.Title           = reader.GetString("Title");
                    tm.TaskDescription = reader.IsDBNull(reader.GetOrdinal("Task_description")) ? "" : reader.GetString("Task_description");
                    tm.Priority        = reader.GetString("Priority");
                    tm.Deadline        = reader.GetDateTime("Deadline");
                    tm.ProjectID       = reader.IsDBNull(reader.GetOrdinal("Project_ID")) ? "" : reader.GetString("Project_ID");
                    tm.ProjectName     = reader.GetString("Project_Name");
                    tm.EmployeeID      = employeeID;
                    tm.EmployeeName    = reader.GetString("Employee_Name");
                    list.Add(tm);
                }
                reader.Close();
            }
            finally { conn.Close(); }
            return list;
        }

        public bool Insert(TaskItemModel tm)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "INSERT INTO Task (Task_ID, Title, Task_description, Priority, Deadline, Project_ID, Employee_ID) " +
                             "VALUES (@tid, @title, @desc, @prio, @dl, @pid, @eid)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@tid",   tm.TaskID);
                cmd.Parameters.AddWithValue("@title", tm.Title);
                cmd.Parameters.AddWithValue("@desc",  tm.TaskDescription);
                cmd.Parameters.AddWithValue("@prio",  tm.Priority);
                cmd.Parameters.AddWithValue("@dl",    tm.Deadline.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@pid",   string.IsNullOrEmpty(tm.ProjectID) ? (object)DBNull.Value : tm.ProjectID);
                cmd.Parameters.AddWithValue("@eid",   tm.EmployeeID == 0 ? (object)DBNull.Value : tm.EmployeeID);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex) { throw new Exception("Insert Task: " + ex.Message); }
            finally { conn.Close(); }
        }

        public bool Update(TaskItemModel tm)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "UPDATE Task SET Title=@title, Task_description=@desc, Priority=@prio, " +
                             "Deadline=@dl, Project_ID=@pid, Employee_ID=@eid WHERE Task_ID=@tid";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@title", tm.Title);
                cmd.Parameters.AddWithValue("@desc",  tm.TaskDescription);
                cmd.Parameters.AddWithValue("@prio",  tm.Priority);
                cmd.Parameters.AddWithValue("@dl",    tm.Deadline.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@pid",   string.IsNullOrEmpty(tm.ProjectID) ? (object)DBNull.Value : tm.ProjectID);
                cmd.Parameters.AddWithValue("@eid",   tm.EmployeeID == 0 ? (object)DBNull.Value : tm.EmployeeID);
                cmd.Parameters.AddWithValue("@tid",   tm.TaskID);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally { conn.Close(); }
        }

        public bool Delete(string taskID)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "DELETE FROM Task WHERE Task_ID = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", taskID);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally { conn.Close(); }
        }

        public bool IDExists(string taskID)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Task WHERE Task_ID = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", taskID);
                return (long)cmd.ExecuteScalar() > 0;
            }
            finally { conn.Close(); }
        }
    }
}
