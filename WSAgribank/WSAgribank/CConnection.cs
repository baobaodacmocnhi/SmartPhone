﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace WSAgribank
{
    public class CConnection
    {
        private string connectionString;          // Chuỗi kết nối
        private SqlConnection connection;         // Đối tượng kết nối
        private SqlDataAdapter adapter;           // Đối tượng adapter chứa dữ liệu
        private SqlCommand command;               // Đối tượng command thực thi truy vấn
        private SqlTransaction transaction;       // Đối tượng transaction

        public CConnection(String connectionString)
        {
            try
            {
                this.connectionString = connectionString;
                connection = new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SqlConnection Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        public void Connect()
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
        }

        public void Disconnect()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

        public void BeginTransaction()
        {
            try
            {
                Connect();
                transaction = connection.BeginTransaction();
            }
            catch (Exception ex) { throw ex; }
        }

        public void CommitTransaction()
        {
            try
            {
                transaction.Commit();
                transaction.Dispose();
                Disconnect();
            }
            catch (Exception ex) { throw ex; }
        }

        public void RollbackTransaction()
        {
            try
            {
                transaction.Rollback();
                transaction.Dispose();

                Disconnect();
            }
            catch (Exception ex) { throw ex; }
        }

        public bool ExecuteNonQuery(string sql)
        {
            try
            {
                Connect();
                command = new SqlCommand(sql, connection);
                int rowsAffected = command.ExecuteNonQuery();
                Disconnect();
                if (rowsAffected >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Disconnect();
                throw ex;
            }
        }

        public bool ExecuteNonQuery_Transaction(string sql)
        {
            try
            {
                command = new SqlCommand(sql, connection, transaction);
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ExecuteNonQuery_Transaction(SqlCommand command)
        {
            try
            {
                command.Connection = connection;
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ExecuteNonQuery(SqlCommand command)
        {
            try
            {
                Connect();
                command.Connection = connection;
                int rowsAffected = command.ExecuteNonQuery();
                Disconnect();
                if (rowsAffected >= 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Disconnect();
                throw ex;
            }
        }

        public object ExecuteQuery_ReturnOneValue(string sql)
        {
            try
            {
                Connect();
                command = new SqlCommand(sql, connection);
                object result = command.ExecuteScalar();
                Disconnect();
                return result;
            }
            catch (Exception ex)
            {
                Disconnect();
                throw ex;
            }
        }

        public object ExecuteQuery_ReturnOneValue_Transaction(string sql)
        {
            try
            {
                command = new SqlCommand(sql, connection, transaction);
                object result = command.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ExecuteQuery_DataSet(string sql)
        {
            try
            {
                this.Connect();
                DataSet dataset = new DataSet();
                command = new SqlCommand();
                command.Connection = this.connection;
                adapter = new SqlDataAdapter(sql, connection);
                try
                {
                    adapter.Fill(dataset);
                }
                catch (SqlException e)
                {
                    throw e;
                }
                this.Disconnect();
                return dataset;
            }
            catch (Exception ex)
            {
                Disconnect();
                throw ex;
            }
        }

        /// <summary>
        /// Thực thi câu truy vấn SQL trả về một đối tượng DataSet chứa kết quả trả về
        /// </summary>
        /// <param name="strSelect">Câu truy vấn cần thực thi lấy dữ liệu</param>
        /// <returns>Đối tượng dataset chứa dữ liệu kết quả câu truy vấn</returns>
        public DataTable ExecuteQuery_DataTable(string sql)
        {
            try
            {
                Connect();
                DataTable dt = new DataTable();
                command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                try
                {
                    adapter.Fill(dt);
                }
                catch (SqlException e)
                {
                    throw e;
                }
                Disconnect();
                return dt;
            }
            catch (Exception ex)
            {
                Disconnect();
                throw ex;
            }
        }

        public DataTable ExecuteQuery_DataTable_Transaction(string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                command = new SqlCommand(sql, connection, transaction);
                adapter = new SqlDataAdapter(command);
                try
                {
                    adapter.Fill(dt);
                }
                catch (SqlException e)
                {
                    throw e;
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Chuyển đối tượng Reader sang Entity Class
        /// </summary>
        /// <typeparam name="T">Tên Class</typeparam>
        /// <param name="reader">kết quả thực thi</param>
        /// <returns></returns>
        public T MapToClass<T>(SqlDataReader reader) where T : class
        {
            T obj = default(T);
            try
            {
                //T returnedObject = Activator.CreateInstance<T>();
                //List<PropertyInfo> modelProperties = returnedObject.GetType().GetProperties().OrderBy(p => p.MetadataToken).ToList();
                //for (int i = 0; i < modelProperties.Count; i++)
                //    modelProperties[i].SetValue(returnedObject, Convert.ChangeType(reader.GetValue(i), modelProperties[i].PropertyType), null);
                //return returnedObject;

                while (reader.Read())
                {
                    obj = Activator.CreateInstance<T>();
                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        if (!object.Equals(reader[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, reader[prop.Name], null);
                        }
                    }
                }
                return obj;
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.Message, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return obj;
            }

        }

        #region ConvertMoneyToWord

        public string unit(int n)
        {
            string chuoi = "";
            if (n == 1) chuoi = " đồng ";
            else if (n == 2) chuoi = " nghìn ";
            else if (n == 3) chuoi = " triệu ";
            else if (n == 4) chuoi = " tỷ ";
            else if (n == 5) chuoi = " nghìn tỷ ";
            else if (n == 6) chuoi = " triệu tỷ ";
            else if (n == 7) chuoi = " tỷ tỷ ";
            return chuoi;
        }

        public string convert_number(string n)
        {
            string chuoi = "";
            if (n == "0") chuoi = "không";
            else if (n == "1") chuoi = "một";
            else if (n == "2") chuoi = "hai";
            else if (n == "3") chuoi = "ba";
            else if (n == "4") chuoi = "bốn";
            else if (n == "5") chuoi = "năm";
            else if (n == "6") chuoi = "sáu";
            else if (n == "7") chuoi = "bảy";
            else if (n == "8") chuoi = "tám";
            else if (n == "9") chuoi = "chín";
            return chuoi;
        }

        public string join_number(string n)
        {
            string chuoi = "";
            int i = 1, j = n.Length;
            while (i <= j)
            {
                if (i == 1) chuoi = convert_number(n.Substring(j - i, 1)) + chuoi;
                else if (i == 2) chuoi = convert_number(n.Substring(j - i, 1)) + " mươi " + chuoi;
                else if (i == 3) chuoi = convert_number(n.Substring(j - i, 1)) + " trăm " + chuoi;
                i += 1;
            }
            return chuoi;
        }

        public string join_unit(string n)
        {
            int sokytu = n.Length;
            int sodonvi = (sokytu % 3 > 0) ? (sokytu / 3 + 1) : (sokytu / 3);
            n = n.PadLeft(sodonvi * 3, '0');
            sokytu = n.Length;
            string chuoi = "";
            int i = 1;
            while (i <= sodonvi)
            {
                if (i == sodonvi) chuoi = join_number((int.Parse(n.Substring(sokytu - (i * 3), 3))).ToString()) + unit(i) + chuoi;
                else chuoi = join_number(n.Substring(sokytu - (i * 3), 3)) + unit(i) + chuoi;
                i += 1;
            }
            return chuoi;
        }

        public string replace_special_word(string chuoi)
        {
            chuoi = chuoi.Replace("không mươi không ", "");
            chuoi = chuoi.Replace("không mươi", "lẻ");
            chuoi = chuoi.Replace("i không", "i");
            chuoi = chuoi.Replace("i năm", "i lăm");
            chuoi = chuoi.Replace("một mươi", "mười");
            chuoi = chuoi.Replace("mươi một", "mươi mốt");
            return chuoi;
        }

        public string ConvertMoneyToWord(string money)
        {
            string str = replace_special_word(join_unit(money));
            if (str.Length > 1)
                return str.Substring(0, 1).ToUpper() + str.Substring(1).ToLower();
            else
                return "";
        }

        #endregion
    }
}