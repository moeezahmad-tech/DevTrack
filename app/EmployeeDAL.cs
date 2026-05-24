using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using CompanyManagement.Models;

namespace CompanyManagement.DAL
{
    public class EmployeeDAL
    {
        public bool Insert(EmployeeModel e)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "INSERT INTO Employee (ID, Role, Dept_ID) VALUES (@id, @role, @dept)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id",   e.ID);
                cmd.Parameters.AddWithValue("@role", e.Role);
                cmd.Parameters.AddWithValue("@dept", string.IsNullOrEmpty(e.DeptID) ? (object)DBNull.Value : e.DeptID);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex) { throw new Exception("Insert Employee: " + ex.Message); }
            finally { conn.Close(); }
        }

        public bool Update(EmployeeModel e)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "UPDATE Employee SET Role = @role, Dept_ID = @dept WHERE ID = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@role", e.Role);
                cmd.Parameters.AddWithValue("@dept", string.IsNullOrEmpty(e.DeptID) ? (object)DBNull.Value : e.DeptID);
                cmd.Parameters.AddWithValue("@id",   e.ID);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally { conn.Close(); }
        }

        public bool Delete(int id)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "DELETE FROM Employee WHERE ID = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally { conn.Close(); }
        }

        public bool IsEmployee(int id)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Employee WHERE ID = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                return (long)cmd.ExecuteScalar() > 0;
            }
            finally { conn.Close(); }
        }

        public List<EmployeeModel> GetAll()
        {
            List<EmployeeModel> list = new List<EmployeeModel>();
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT e.ID, p.Name, p.Email, e.Role, e.Dept_ID, " +
                             "COALESCE(d.Dept_Name,'—') AS Dept_Name " +
                             "FROM Employee e " +
                             "JOIN Person p ON e.ID = p.ID " +
                             "LEFT JOIN Department d ON e.Dept_ID = d.Dept_ID " +
                             "ORDER BY e.ID";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    EmployeeModel em = new EmployeeModel();
                    em.ID       = reader.GetInt32("ID");
                    em.Name     = reader.GetString("Name");
                    em.Email    = reader.GetString("Email");
                    em.Role     = reader.GetString("Role");
                    em.DeptID   = reader.IsDBNull(reader.GetOrdinal("Dept_ID")) ? "" : reader.GetString("Dept_ID");
                    em.DeptName = reader.GetString("Dept_Name");
                    list.Add(em);
                }
                reader.Close();
            }
            finally { conn.Close(); }
            return list;
        }

        public EmployeeModel GetByID(int id)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT e.ID, p.Name, p.Email, e.Role, e.Dept_ID, " +
                             "COALESCE(d.Dept_Name,'—') AS Dept_Name " +
                             "FROM Employee e " +
                             "JOIN Person p ON e.ID = p.ID " +
                             "LEFT JOIN Department d ON e.Dept_ID = d.Dept_ID " +
                             "WHERE e.ID = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                EmployeeModel em = null;
                if (reader.Read())
                {
                    em          = new EmployeeModel();
                    em.ID       = reader.GetInt32("ID");
                    em.Name     = reader.GetString("Name");
                    em.Email    = reader.GetString("Email");
                    em.Role     = reader.GetString("Role");
                    em.DeptID   = reader.IsDBNull(reader.GetOrdinal("Dept_ID")) ? "" : reader.GetString("Dept_ID");
                    em.DeptName = reader.GetString("Dept_Name");
                }
                reader.Close();
                return em;
            }
            finally { conn.Close(); }
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    public class HeadOfDeptDAL
    {
        public bool Insert(HeadOfDeptModel h)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "INSERT INTO Head_of_Dept (ID, Office_No, Dept_ID) VALUES (@id, @office, @dept)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id",     h.ID);
                cmd.Parameters.AddWithValue("@office", h.OfficeNo);
                cmd.Parameters.AddWithValue("@dept",   string.IsNullOrEmpty(h.DeptID) ? (object)DBNull.Value : h.DeptID);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex) { throw new Exception("Insert HOD: " + ex.Message); }
            finally { conn.Close(); }
        }

        public bool IsHOD(int id)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Head_of_Dept WHERE ID = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                return (long)cmd.ExecuteScalar() > 0;
            }
            finally { conn.Close(); }
        }

        public HeadOfDeptModel GetByID(int id)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT h.ID, p.Name, p.Email, h.Office_No, h.Dept_ID, " +
                             "COALESCE(d.Dept_Name,'—') AS Dept_Name " +
                             "FROM Head_of_Dept h " +
                             "JOIN Person p ON h.ID = p.ID " +
                             "LEFT JOIN Department d ON h.Dept_ID = d.Dept_ID " +
                             "WHERE h.ID = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();
                HeadOfDeptModel hm = null;
                if (reader.Read())
                {
                    hm          = new HeadOfDeptModel();
                    hm.ID       = reader.GetInt32("ID");
                    hm.Name     = reader.GetString("Name");
                    hm.Email    = reader.GetString("Email");
                    hm.OfficeNo = reader.GetString("Office_No");
                    hm.DeptID   = reader.IsDBNull(reader.GetOrdinal("Dept_ID")) ? "" : reader.GetString("Dept_ID");
                    hm.DeptName = reader.GetString("Dept_Name");
                }
                reader.Close();
                return hm;
            }
            finally { conn.Close(); }
        }

        public List<HeadOfDeptModel> GetAll()
        {
            List<HeadOfDeptModel> list = new List<HeadOfDeptModel>();
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT h.ID, p.Name, p.Email, h.Office_No, h.Dept_ID, " +
                             "COALESCE(d.Dept_Name,'—') AS Dept_Name " +
                             "FROM Head_of_Dept h " +
                             "JOIN Person p ON h.ID = p.ID " +
                             "LEFT JOIN Department d ON h.Dept_ID = d.Dept_ID " +
                             "ORDER BY h.ID";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    HeadOfDeptModel hm = new HeadOfDeptModel();
                    hm.ID       = reader.GetInt32("ID");
                    hm.Name     = reader.GetString("Name");
                    hm.Email    = reader.GetString("Email");
                    hm.OfficeNo = reader.GetString("Office_No");
                    hm.DeptID   = reader.IsDBNull(reader.GetOrdinal("Dept_ID")) ? "" : reader.GetString("Dept_ID");
                    hm.DeptName = reader.GetString("Dept_Name");
                    list.Add(hm);
                }
                reader.Close();
            }
            finally { conn.Close(); }
            return list;
        }
    }
}
