using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodTrackerApp.Data
{
    public class DatabaseHelper
    {
        private string connectionString = @"Data Source=(local);Initial Catalog=MoodTrackerDB;Integrated Security=True";

        /// <summary>
        /// Получает соединение с базой данных
        /// </summary>
        /// <returns>Объект SqlConnection</returns>
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}