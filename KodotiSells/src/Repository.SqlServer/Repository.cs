using System.Data.SqlClient;

namespace Repository.SqlServer
{
    public abstract class Repository
    {
        protected SqlConnection _cn;
        protected SqlTransaction _transaction;

        protected SqlCommand CreateCommand(string query)
        {
            return new SqlCommand(query, _cn, _transaction);
        }
    }
}
