using Models;
using Repository.Interfaces.Actions;
using System.Collections.Generic;

namespace Repository.Interfaces
{
    public interface IInvoiceDetailRepository : IReadRepository<InvoiceDetail, int>
    {
        void Create(IEnumerable<InvoiceDetail> model, int invoiceId);
        void RemoveByInvoiceId(int invoiceId);
        IEnumerable<InvoiceDetail> GetAllByInvoiceId(int invoiceId);
    }
}
