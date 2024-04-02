using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SV20T1020610.DataLayers.SQLSever
{
    /// <summary>
    /// Lớp cha của các lớp cài đặt các phép xử lý dữ liệu trên SQL Sever
    /// </summary>
    public abstract class _BaseDAL
    {
        protected string _connectionString = "";

        public _BaseDAL(string connectionString) 
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// Tạo và mở kết nối CSDL
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection ()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = _connectionString;    
            connection.Open();

            return connection;
        }
    }
}
