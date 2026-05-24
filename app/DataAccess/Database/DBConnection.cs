using System;
using MySql.Data.MySqlClient;

namespace DevTrack.DataAccess.Database
{
    public static class DBConnection
    {
        // Change Uid and Pwd to match your MySQL setup
        private static string connectionString = "Server=localhost;Database=company_management_db;User ID=root;Password= Minerooh.1";

        public static MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
    
            return conn;
        }
    }
}
