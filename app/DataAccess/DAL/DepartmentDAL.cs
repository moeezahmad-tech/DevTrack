using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using DevTrack.DataAccess.Models;
using DevTrack.DataAccess.Database;

namespace DevTrack.DataAccess.DAL
{
    public class DepartmentDAL
    {
        public List<DepartmentModel> GetAll()
        {
            List<DepartmentModel> list = new List<DepartmentModel>();
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT Dept_ID, Dept_Name FROM Department ORDER BY Dept_ID";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DepartmentModel d = new DepartmentModel();
                    d.DeptID   = reader.GetString("Dept_ID");
                    d.DeptName = reader.GetString("Dept_Name");
                    list.Add(d);
                }
                reader.Close();
            }
            finally { conn.Close(); }
            return list;
        }

        public bool Insert(DepartmentModel d)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "INSERT INTO Department (Dept_ID, Dept_Name) VALUES (@id, @name)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id",   d.DeptID);
                cmd.Parameters.AddWithValue("@name", d.DeptName);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex) { throw new Exception("Insert Dept: " + ex.Message); }
            finally { conn.Close(); }
        }

        public bool Update(DepartmentModel d)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "UPDATE Department SET Dept_Name = @name WHERE Dept_ID = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", d.DeptName);
                cmd.Parameters.AddWithValue("@id",   d.DeptID);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally { conn.Close(); }
        }

        public bool Delete(string deptID)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "DELETE FROM Department WHERE Dept_ID = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", deptID);
                return cmd.ExecuteNonQuery() > 0;
            }
            finally { conn.Close(); }
        }

        public bool IDExists(string deptID)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Department WHERE Dept_ID = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", deptID);
                return (long)cmd.ExecuteScalar() > 0;
            }
            finally { conn.Close(); }
        }
    }
}
