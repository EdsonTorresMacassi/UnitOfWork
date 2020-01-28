using Common;
using System;
using System.Data.SqlClient;

namespace Services
{
    public class TestService
    {
        public static void TestConecction()
        {
            try
            {
                /*
                using (var cn = new SqlConnection(Parameters.ConnectionString))
                {
                    cn.Open();
                    Console.WriteLine("Sql Connection successful");
                    cn.Close();
                }
                */
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"SQL Server: { ex.Message }");
            }
        }
    }
}
