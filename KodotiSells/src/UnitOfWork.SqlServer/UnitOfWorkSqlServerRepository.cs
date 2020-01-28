using Repository.Interfaces;
using Repository.SqlServer;
using System.Data.SqlClient;
using UnitOfWork.Interfaces;

namespace UnitOfWork.SqlServer
{
    public class UnitOfWorkSqlServerRepository : IUnitOfWorkRepository
    {
        public IProductRepository ProductRepository { get; }
        public IClientRepository ClientRepository { get; }
        public IInvoiceRepository InvoiceRepository { get; }
        public IInvoiceDetailRepository InvoiceDetailRepository { get; }
        public UnitOfWorkSqlServerRepository(SqlConnection cn, SqlTransaction transaction)
        {
            ProductRepository = new ProductRepository(cn, transaction);
            ClientRepository = new ClientRepository(cn, transaction);
            InvoiceRepository = new InvoiceRepository(cn, transaction);
            InvoiceDetailRepository = new InvoiceDetailRepository(cn, transaction);
        }
    }
}
