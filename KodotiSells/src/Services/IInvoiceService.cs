using Models;
using System.Collections.Generic;

namespace Services
{
    public interface IInvoiceService
    {
        IEnumerable<Invoice> GetAll();
        
        Invoice Get(int id);

        void Create(Invoice model);

        void Update(Invoice model);        

        void Delete(int id);        
    }
}
