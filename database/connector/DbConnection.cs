using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.database.connector
{
    class DbConnection
    {
        private readonly string connectionString = "Server=localhost;Port=3306;Database=projektni;Uid=root;Pwd=root;";

        public MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
    }
}
