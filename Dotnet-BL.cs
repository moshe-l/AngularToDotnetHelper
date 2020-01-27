using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Models;
namespace BL
{
    public class BL
    {
        private DB DB { get; set; }

        public BL(string databaseName)
        {
            string con = ConfigurationManager.ConnectionStrings[databaseName].ConnectionString;
            DB = new DB(con);
        }

        public string GetAll(Person p)
        {          

            try
            {
                DB.Open(true);
                var param = DB.AddParameter("@Identification", p.ID);
                return DB.ExecuteToJson("sp_Last_Login", System.Data.CommandType.StoredProcedure, param);
              
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                DB.Close();
            }

        }
        
        /*
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            try
            {
                DB.Open();

                // 1) call the DAL to run the query
                SqlDataReader reader = DB.ExecuteReader("Select * from Employees");

                // 2) map the result to list of objects
                while (reader.Read())
                {
                    employees.Add(new Employee()
                    {
                        ID = int.Parse(reader["EmployeeID"].ToString()),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Title = reader["Title"].ToString()
                    });
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                DB.Close();
            }

            return employees;
        }
        */

    }
}
