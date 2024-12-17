using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCours.Model;

namespace WpfCours.MVVM.ViewModel
{
    public class SqlConnectionManager
    {
        private static SqlConnection _connection;

        public static SqlConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(SharedService.Instance.DataBase);
                _connection.Open();
            }
            return _connection;
        }

        public static void CloseConnection()
        {
            if (_connection != null && _connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }
    }
}