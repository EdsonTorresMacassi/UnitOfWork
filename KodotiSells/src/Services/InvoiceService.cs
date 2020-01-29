using Common;
using Models;
using System.Collections.Generic;
using System.Linq;
using UnitOfWork.Interfaces;

namespace Services
{
    public class InvoiceService : IInvoiceService
    {
        private IUnitOfWork _unitOfWork;

        public InvoiceService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Invoice> GetAll()
        {
            IEnumerable<Invoice> results = null;            
            try
            {
                using (var cn = _unitOfWork.Create())
                {
                    results = cn.Repositories.InvoiceRepository.GetAll();
                    foreach (var result in results)
                    {
                        result.Client = cn.Repositories.ClientRepository.Get(result.ClientId);
                        result.Detail = cn.Repositories.InvoiceDetailRepository.GetAllByInvoiceId(result.Id);

                        foreach (var item in result.Detail)
                        {
                            item.Product = cn.Repositories.ProductRepository.Get(item.ProductId);
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }

            return results;
        }

        public Invoice Get(int id) 
        {
            Invoice result = null;
            try
            {
                using (var cn = _unitOfWork.Create())
                {
                    result = cn.Repositories.InvoiceRepository.Get(id);
                    result.Client = cn.Repositories.ClientRepository.Get(result.ClientId);
                    result.Detail = cn.Repositories.InvoiceDetailRepository.GetAllByInvoiceId(result.Id);

                    foreach (var item in result.Detail)
                    {
                        item.Product = cn.Repositories.ProductRepository.Get(item.ProductId);
                    }
                }                
            }
            catch (System.Exception)
            {
                throw;
            }
            return result;
        }

        public void Create(Invoice model) 
        {
            PrepareOrder(model);
            try
            {
                using (var cn = _unitOfWork.Create())
                {
                    cn.Repositories.InvoiceRepository.Create(model);
                    cn.Repositories.InvoiceDetailRepository.Create(model.Detail, model.Id);
                    cn.SaveChanges();
                }              
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void Update(Invoice model)
        {
            PrepareOrder(model);
            try
            {
                using (var cn = _unitOfWork.Create())
                {
                    cn.Repositories.InvoiceRepository.Update(model);
                    cn.Repositories.InvoiceDetailRepository.RemoveByInvoiceId(model.Id);
                    cn.Repositories.InvoiceDetailRepository.Create(model.Detail, model.Id);
                    cn.SaveChanges();
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void Delete(int id)
        {            
            try
            {
                using (var cn = _unitOfWork.Create())
                {
                    cn.Repositories.InvoiceDetailRepository.RemoveByInvoiceId(id);
                    cn.Repositories.InvoiceRepository.Remove(id);
                    cn.SaveChanges();
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void PrepareOrder(Invoice model)
        {
            foreach (var detail in model.Detail)
            {
                detail.Total = detail.Quantity * detail.Price;
                detail.Iva = detail.Total * Parameters.IvaRate;
                detail.SubTotal = detail.Total - detail.Iva;
            }

            model.Total = model.Detail.Sum(x => x.Total);
            model.Iva = model.Detail.Sum(x => x.Iva);
            model.SubTotal = model.Detail.Sum(x => x.SubTotal);
        }
    }
}
