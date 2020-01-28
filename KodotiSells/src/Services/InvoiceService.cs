using Common;
using Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using UnitOfWork.Interfaces;

namespace Services
{
    public class InvoiceService
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
                    //cn.Repositories.InvoiceDetailRepository.RemoveByInvoiceId(model.Id);
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
                    //cn.Repositories.InvoiceRepository.Remove(id);
                    //cn.Repositories.InvoiceRepository.Remove(id);
                    cn.SaveChanges();
                }

                /*
                var query2 = "delete from InvoiceDetail where InvoiceId = @Id";
                using (SqlConnection cn = new SqlConnection(Parameters.ConnectionString))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(query2, cn))
                    {
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                    }
                    cn.Close();
                }

                var query = "delete from Invoices where Id = @Id";
                using (SqlConnection cn = new SqlConnection(Parameters.ConnectionString))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                    }
                    cn.Close();
                }
                */
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        
        /*
        private void RemoveDetail(int invoceId, SqlConnection sqlConnection)
        {
            var query = "delete from InvoiceDetail where InvoiceId = @InvoiceId";
            using (SqlConnection cn = new SqlConnection(Parameters.ConnectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@InvoiceId", invoceId);                      
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                }
                cn.Close();
            }         
        }
        */

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

        /*
        private void SetClient(Invoice invoice, SqlConnection sqlConnection) 
        {
            Client registro = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Parameters.ConnectionString))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Clients WHERE id = @ClienteId", cn))
                    {
                        cmd.Parameters.AddWithValue("@ClienteId", invoice.ClientId);
                        cmd.CommandTimeout = 0;
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {   
                            if (dr.Read())
                            {
                                registro = new Client();
                                registro.Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id"));
                                registro.Name = dr.IsDBNull(dr.GetOrdinal("Name")) ? "" : dr.GetString(dr.GetOrdinal("Name"));
                            }
                        }
                    }
                    cn.Close();
                }
            }
            catch (System.Exception)
            {

                throw;
            }

            invoice.Client = registro;
        }
        */

        /*
        private void SetDetail(Invoice invoice, SqlConnection sqlConnection)
        {
            List<InvoiceDetail> detail = null;
            InvoiceDetail registro = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Parameters.ConnectionString))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM InvoiceDetail WHERE InvoiceId = @Id", cn))
                    {
                        cmd.Parameters.AddWithValue("@Id", invoice.Id);
                        cmd.CommandTimeout = 0;
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            detail = new List<InvoiceDetail>();
                            while (dr.Read())
                            {
                                registro = new InvoiceDetail();
                                registro.Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id"));
                                registro.InvoiceId = dr.IsDBNull(dr.GetOrdinal("InvoiceId")) ? 0 : dr.GetInt32(dr.GetOrdinal("InvoiceId"));
                                registro.ProductId = dr.IsDBNull(dr.GetOrdinal("ProductId")) ? 0 : dr.GetInt32(dr.GetOrdinal("ProductId"));
                                registro.Quantity = dr.IsDBNull(dr.GetOrdinal("Quantity")) ? 0 : dr.GetInt32(dr.GetOrdinal("Quantity"));
                                registro.Price = dr.IsDBNull(dr.GetOrdinal("Price")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Price"));
                                registro.Iva = dr.IsDBNull(dr.GetOrdinal("Iva")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Iva"));
                                registro.SubTotal = dr.IsDBNull(dr.GetOrdinal("SubTotal")) ? 0 : dr.GetDecimal(dr.GetOrdinal("SubTotal"));
                                registro.Total = dr.IsDBNull(dr.GetOrdinal("Total")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Total"));
                                //registro.Invoice = invoice;
                                //detail.Add(registro);
                            }
                        }
                    }
                    cn.Close();
                }
            }
            catch (System.Exception)
            {

                throw;
            }

            invoice.Detail = detail;

            foreach (var details in invoice.Detail)
            {
                SetProduct(details, sqlConnection);
            }
        }
        */

        /*
        private void SetProduct(InvoiceDetail invoiceDetail, SqlConnection sqlConnection)
        {
            Product registro = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Parameters.ConnectionString))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE id = @ProductId", cn))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", invoiceDetail.ProductId);
                        cmd.CommandTimeout = 0;
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                registro = new Product();
                                registro.Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id"));
                                registro.Name = dr.IsDBNull(dr.GetOrdinal("Name")) ? "" : dr.GetString(dr.GetOrdinal("Name"));
                                registro.Price = dr.IsDBNull(dr.GetOrdinal("Price")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Price"));
                            }
                        }
                    }
                    cn.Close();
                }
            }
            catch (System.Exception)
            {

                throw;
            }

            invoiceDetail.Product = registro;
        }
        */
    }
}
