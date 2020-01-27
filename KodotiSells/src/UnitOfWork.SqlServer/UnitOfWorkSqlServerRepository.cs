using System.Data.SqlClient;
using UnitOfWork.Interfaces;

namespace UnitOfWork.SqlServer
{
    public class UnitOfWorkSqlServerRepository : IUnitOfWorkRepository
    {
        public UnitOfWorkSqlServerRepository(SqlConnection cn, SqlTransaction transaction)
        {
            
        }
    }
}
