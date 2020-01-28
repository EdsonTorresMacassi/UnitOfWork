using Models;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Repository.SqlServer
{
    public class InvoiceDetailRepository : Repository, IInvoiceDetailRepository
    {
        public InvoiceDetailRepository(SqlConnection cn, SqlTransaction transaction)
        {
            this._cn = cn;
            this._transaction = transaction;
        }

        public void Create(IEnumerable<InvoiceDetail> model, int invoiceId) 
        {
            foreach (var detail in model)
            {
                var query = "insert into InvoiceDetail (InvoiceId, ProductId, Quantity, Price, Iva, SubTotal, Total) values(@InvoiceId, @ProductId, @Quantity, @Price, @Iva, @SubTotal, @Total)";                
                using (var cmd = CreateCommand(query))
                {
                    cmd.Parameters.AddWithValue("@InvoiceId", invoiceId);
                    cmd.Parameters.AddWithValue("@ProductId", detail.ProductId);
                    cmd.Parameters.AddWithValue("@Quantity", detail.Quantity);
                    cmd.Parameters.AddWithValue("@Price", detail.Price);
                    cmd.Parameters.AddWithValue("@Iva", detail.Iva);
                    cmd.Parameters.AddWithValue("@SubTotal", detail.SubTotal);
                    cmd.Parameters.AddWithValue("@Total", detail.Total);
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                }                
            }
        }

        public InvoiceDetail Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<InvoiceDetail> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<InvoiceDetail> GetAllByInvoiceId(int InvoiceId)
        {
            List<InvoiceDetail> result = null;
            InvoiceDetail detail = null;            
            using (var cmd = CreateCommand("SELECT * FROM InvoiceDetail WITH(NOLOCK) WHERE InvoiceId = @Id"))
            {
                cmd.Parameters.AddWithValue("@Id", InvoiceId);
                cmd.CommandTimeout = 0;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    result = new List<InvoiceDetail>();
                    while (dr.Read())
                    {
                        detail = new InvoiceDetail();
                        detail.Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id"));
                        detail.InvoiceId = dr.IsDBNull(dr.GetOrdinal("InvoiceId")) ? 0 : dr.GetInt32(dr.GetOrdinal("InvoiceId"));
                        detail.ProductId = dr.IsDBNull(dr.GetOrdinal("ProductId")) ? 0 : dr.GetInt32(dr.GetOrdinal("ProductId"));
                        detail.Quantity = dr.IsDBNull(dr.GetOrdinal("Quantity")) ? 0 : dr.GetInt32(dr.GetOrdinal("Quantity"));
                        detail.Price = dr.IsDBNull(dr.GetOrdinal("Price")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Price"));
                        detail.Iva = dr.IsDBNull(dr.GetOrdinal("Iva")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Iva"));
                        detail.SubTotal = dr.IsDBNull(dr.GetOrdinal("SubTotal")) ? 0 : dr.GetDecimal(dr.GetOrdinal("SubTotal"));
                        detail.Total = dr.IsDBNull(dr.GetOrdinal("Total")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Total"));                        
                        result.Add(detail);
                    }
                }
            }
            return result;
        }
    }
}
