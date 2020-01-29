using Common;
using System.Data.SqlClient;
using UnitOfWork.Interfaces;

namespace UnitOfWork.SqlServer
{
    public class UnitOfWorkSqlServerAdapter : IUnitOfWorkAdapter
    {
        private SqlConnection _cn { get; set; }
        private SqlTransaction _transaction { get; set; }
        public IUnitOfWorkRepository Repositories { get; set; }

        public UnitOfWorkSqlServerAdapter(string connectionString)
        {
            _cn = new SqlConnection(connectionString);
            _cn.Open();
            _transaction = _cn.BeginTransaction();
            Repositories = new UnitOfWorkSqlServerRepository(_cn, _transaction);
        }
        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }

            if (_cn != null)
            {
                _cn.Close();
                _cn.Dispose();
            }

            Repositories = null;
        }

        public void SaveChanges()
        {
            _transaction.Commit();
        }
    }
}
