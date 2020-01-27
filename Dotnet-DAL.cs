using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DB
    {
        public SqlConnection Connection { get; set; }

        private SqlCommand command;
        private bool isTrans;           // indication - if we are in a transaction
        private SqlTransaction trans;   // the actual transaction


        public DB(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
            isTrans = false; // by default - not trans
        }

        public void Open(bool openTransaction = false)
        {
            Connection.Open();

            if (openTransaction) // if we required to open a transaction
            {
                trans = Connection.BeginTransaction(); // open the transaction
                isTrans = true; // set the flag as true
            }
        }

        public void Close()
        {
            if (isTrans) // cehck if we are in a transaction
            {
                trans.Commit();
                isTrans = false;
            }

            if (Connection.State == ConnectionState.Open)
                Connection.Close();
        }

        private void RollBack()
        {
            if (isTrans)  // cehck if we are in a transaction
            {
                trans.Rollback();
                isTrans = false;
            }
        }

        public string ExecuteToJson (string query, CommandType type = CommandType.Text, params SqlParameter[] parameters)
        {
            try
            {
                command = InitCommand(query, type, parameters);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet);
                DataTable dt = dataSet.Tables[0];
                string tmp = JsonConvert.SerializeObject(dt);
                dt.Clear();
                return tmp;

            }
            catch (Exception ex)
            {
                RollBack();
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
        }
        public string ExecuteToJsonArr(string query, CommandType type = CommandType.Text, params SqlParameter[] parameters)
        {
            try
            {
                command = InitCommand(query, type, parameters);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet);
                string[] arr = new string[dataSet.Tables.Count];
                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    arr[i] = JsonConvert.SerializeObject(dataSet.Tables[i]);
                }
                return JsonConvert.SerializeObject(arr);

            }
            catch (Exception ex)
            {
                RollBack();
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
        }

        public SqlDataReader ExecuteReader(string query, CommandType type = CommandType.Text, params SqlParameter[] parameters)
        {
            try
            {
                command = InitCommand(query, type, parameters);

                return command.ExecuteReader();
            }
            catch (Exception)
            {
                RollBack();
                throw;
            }
        }

        public int ExecuteNonQuery(string query, CommandType type = CommandType.Text, params SqlParameter[] parameters)
        {
            try
            {
                command = InitCommand(query, type, parameters);

                return command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                RollBack();
                throw;
            }
        }

        public object ExecuteScalar(string query, CommandType type = CommandType.Text, params SqlParameter[] parameters)
        {
            try
            {
                command = InitCommand(query, type, parameters);

                return command.ExecuteScalar();
            }
            catch (Exception)
            {
                RollBack();
                throw;
            }
        }

        public SqlParameter AddParameter(string paramName, object paramValue, SqlDbType type = SqlDbType.NVarChar, ParameterDirection direction = ParameterDirection.Input)
        {
            SqlParameter param = new SqlParameter()
            {
                ParameterName = paramName,
                SqlDbType = type,
                Value = paramValue,
                Direction = direction
            };

            return param;
        }

        public object GetParameter(string paramName)
        {
            return command.Parameters[paramName].Value;
        }


        // set the command properties
        private SqlCommand InitCommand(string query, CommandType type, SqlParameter[] parameters)
        {
            command = new SqlCommand(query);
            command.Connection = Connection;
            command.Transaction = trans;
            command.CommandType = type;
            command.Parameters.AddRange(parameters);
            return command;
        }
    }
}
