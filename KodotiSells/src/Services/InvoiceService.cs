using Common;
using Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace Services
{
    public class InvoiceService
    {
        public List<Invoice> GetAll()
        {
            List<Invoice> result = null;
            Invoice registro = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Parameters.ConnectionString))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Invoices", cn))
                    {
                        cmd.CommandTimeout = 0;
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            result = new List<Invoice>();
                            while (dr.Read())
                            {
                                registro = new Invoice();
                                registro.Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id"));
                                registro.Iva = dr.IsDBNull(dr.GetOrdinal("Iva")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Iva"));
                                registro.SubTotal = dr.IsDBNull(dr.GetOrdinal("SubTotal")) ? 0 : dr.GetDecimal(dr.GetOrdinal("SubTotal"));
                                registro.Total = dr.IsDBNull(dr.GetOrdinal("Total")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Total"));
                                registro.ClienteId = dr.IsDBNull(dr.GetOrdinal("ClienteId")) ? 0 : dr.GetInt32(dr.GetOrdinal("ClienteId"));
                                result.Add(registro);
                            }
                        }
                    }
                    cn.Close();

                    foreach (var invoice in result)
                    {
                        SetClient(invoice, cn);
                        SetDetail(invoice, cn);
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }

            return result;
        }

        public Invoice Get(int id) 
        {
            Invoice registro = null;
            try
            {
                using (SqlConnection cn = new SqlConnection(Parameters.ConnectionString))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Invoices WHERE id = @Id", cn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.CommandTimeout = 0;
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {                            
                            while (dr.Read())
                            {
                                registro = new Invoice();
                                registro.Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id"));
                                registro.Iva = dr.IsDBNull(dr.GetOrdinal("Iva")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Iva"));
                                registro.SubTotal = dr.IsDBNull(dr.GetOrdinal("SubTotal")) ? 0 : dr.GetDecimal(dr.GetOrdinal("SubTotal"));
                                registro.Total = dr.IsDBNull(dr.GetOrdinal("Total")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Total"));
                                registro.ClienteId = dr.IsDBNull(dr.GetOrdinal("ClienteId")) ? 0 : dr.GetInt32(dr.GetOrdinal("ClienteId"));                                
                            }
                        }
                    }
                    cn.Close();

                    SetClient(registro, cn);
                    SetDetail(registro, cn);                    
                }
            }
            catch (System.Exception)
            {

                throw;
            }
            return registro;
        }

        public void Create(Invoice model) 
        {
            PrepareOrder(model);
            try
            {
                //using (TransactionScope transaction = new TransactionScope())
                //{
                    using (SqlConnection cn = new SqlConnection(Parameters.ConnectionString))
                    {
                        cn.Open();
                        AddHeader(model, cn);
                        AddDetail(model, cn);
                        cn.Close();
                    }
                //    transaction.Complete();
                //}
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
                using (SqlConnection cn = new SqlConnection(Parameters.ConnectionString))
                {
                    cn.Open();
                    UpdateHeader(model, cn);
                    RemoveDetail(model.Id, cn);
                    AddDetail(model, cn);
                    cn.Close();
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public void Delete(int Id)
        {            
            try
            {
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
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        private void AddHeader(Invoice model, SqlConnection sqlConnection)
        {
            var query = "insert into Invoices (ClienteId, Iva, SubTotal, Total) output inserted.Id values(@ClienteId, @Iva, @SubTotal, @Total)";
            using (SqlConnection cn = new SqlConnection(Parameters.ConnectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@ClienteId", model.ClienteId);
                    cmd.Parameters.AddWithValue("@Iva", model.Iva);
                    cmd.Parameters.AddWithValue("@SubTotal", model.SubTotal);
                    cmd.Parameters.AddWithValue("@Total", model.Total);
                    cmd.CommandTimeout = 0;

                    model.Id = (int)cmd.ExecuteScalar();
                }
                cn.Close();
            }
        }

        private void UpdateHeader(Invoice model, SqlConnection sqlConnection)
        {
            var query = "Update a SET a.ClienteId = @ClienteId, a.Iva = @Iva, a.SubTotal = @SubTotal, a.Total = @Total FROM Invoices a WHERE a.Id = @Id";
            using (SqlConnection cn = new SqlConnection(Parameters.ConnectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@ClienteId", model.ClienteId);
                    cmd.Parameters.AddWithValue("@Iva", model.Iva);
                    cmd.Parameters.AddWithValue("@SubTotal", model.SubTotal);
                    cmd.Parameters.AddWithValue("@Total", model.Total);
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                }
                cn.Close();
            }
        }

        private void AddDetail(Invoice model, SqlConnection sqlConnection)
        {
            foreach (var detail in model.Detail)
            {
                var query = "insert into InvoiceDetail (InvoiceId, ProductId, Quantity, Price, Iva, SubTotal, Total) output inserted.Id values(@InvoiceId, @ProductId, @Quantity, @Price, @Iva, @SubTotal, @Total)";
                using (SqlConnection cn = new SqlConnection(Parameters.ConnectionString))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@InvoiceId", model.Id);
                        cmd.Parameters.AddWithValue("@ProductId", detail.ProductId);
                        cmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
                        cmd.Parameters.AddWithValue("@Price", detail.Price);
                        cmd.Parameters.AddWithValue("@Iva", detail.Iva);
                        cmd.Parameters.AddWithValue("@SubTotal", detail.SubTotal);
                        cmd.Parameters.AddWithValue("@Total", detail.Total);
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                    }
                    cn.Close();
                }
            }            
        }

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
                        cmd.Parameters.AddWithValue("@ClienteId", invoice.ClienteId);
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
                                detail.Add(registro);
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
    }
}
