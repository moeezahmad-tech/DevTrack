using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using DevTrack.DataAccess.Models;
using DevTrack.DataAccess.Database;

namespace DevTrack.DataAccess.DAL
{
    public class PersonDAL
    {
        // Returns PersonModel if email+password match, else null
        public PersonModel Login(string email, string password)
        {
            PersonModel person = null;
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT ID, Name, Email, Salary, Password, Joining_Date " +
                             "FROM Person WHERE Email = @email AND Password = @pwd";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@pwd",   password);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    person = new PersonModel();
                    person.ID          = reader.GetInt32("ID");
                    person.Name        = reader.GetString("Name");
                    person.Email       = reader.GetString("Email");
                    person.Salary      = reader.GetDecimal("Salary");
                    person.Password    = reader.GetString("Password");
                    person.JoiningDate = reader.GetDateTime("Joining_Date");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Login failed: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return person;
        }

        public bool InsertPerson(PersonModel p)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "INSERT INTO Person (ID, Name, Email, Salary, Password, Joining_Date) " +
                             "VALUES (@id, @name, @email, @salary, @pwd, @date)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id",     p.ID);
                cmd.Parameters.AddWithValue("@name",   p.Name);
                cmd.Parameters.AddWithValue("@email",  p.Email);
                cmd.Parameters.AddWithValue("@salary", p.Salary);
                cmd.Parameters.AddWithValue("@pwd",    p.Password);
                cmd.Parameters.AddWithValue("@date",   p.JoiningDate.ToString("yyyy-MM-dd"));
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Insert Person failed: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public bool EmailExists(string email)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Person WHERE Email = @email";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@email", email);
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public bool IDExists(int id)
        {
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM Person WHERE ID = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                long count = (long)cmd.ExecuteScalar();
                return count > 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<PersonModel> GetAllPersons()
        {
            List<PersonModel> list = new List<PersonModel>();
            MySqlConnection conn = DBConnection.GetConnection();
            try
            {
                conn.Open();
                string sql = "SELECT ID, Name, Email, Salary, Joining_Date FROM Person ORDER BY ID";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PersonModel p = new PersonModel();
                    p.ID          = reader.GetInt32("ID");
                    p.Name        = reader.GetString("Name");
                    p.Email       = reader.GetString("Email");
                    p.Salary      = reader.GetDecimal("Salary");
                    p.JoiningDate = reader.GetDateTime("Joining_Date");
                    list.Add(p);
                }
                reader.Close();
            }
            finally
            {
                conn.Close();
            }
            return list;
        }
    }
}
