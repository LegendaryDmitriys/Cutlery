
using System;  
using System.Collections.Generic;  
using System.Linq;  
using System.Text;  
using MySql.Data.MySqlClient;  
using System.Windows.Forms;  

namespace WindowsFormsApp4
{
    public class connection
    {
        MySql.Data.MySqlClient.MySqlConnection conn;  
        string myConnectionString;  
        static string host = "localhost";  
        static string database = "trade";  
        static string userDB = "root";  
        static string password = "Dima@1213";  
        public static string strProvider = "server=" + host + ";Database=" + database + ";User ID=" + userDB + ";Password=" + password; 
        
        public connection()
        {
            Initialize();
        }
        
        public MySqlConnection GetConnection()
        {
            return conn;
        }
        
        private void Initialize()
        {
            myConnectionString = $"server={host};database={database};user={userDB};password={password};";
            conn = new MySqlConnection(myConnectionString);
        }
        
        public void Open()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        public void Close()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public MySqlDataReader ExecuteQuery(string query)
        {
            try
            {
                Open();

                MySqlCommand cmd = new MySqlCommand(query, conn);
                return cmd.ExecuteReader();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
        