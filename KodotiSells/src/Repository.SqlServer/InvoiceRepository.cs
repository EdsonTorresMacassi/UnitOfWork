using Models;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Repository.SqlServer
{
    public class InvoiceRepository : Repository, IInvoiceRepository
    {
        public InvoiceRepository(SqlConnection cn, SqlTransaction transaction) 
        {
            this._cn = cn;
            this._transaction = transaction;
        }
        public void Create(Invoice model)
        {
            var query = "insert into Invoices (ClientId, Iva, SubTotal, Total) output inserted.Id values(@ClientId, @Iva, @SubTotal, @Total)";            
            using (var cmd = CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@ClientId", model.ClientId);
                cmd.Parameters.AddWithValue("@Iva", model.Iva);
                cmd.Parameters.AddWithValue("@SubTotal", model.SubTotal);
                cmd.Parameters.AddWithValue("@Total", model.Total);
                cmd.CommandTimeout = 0;

                model.Id = (int)cmd.ExecuteScalar();
            }
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Invoice Get(int id)
        {
            Invoice result = null;
            using (var cmd = CreateCommand("SELECT * FROM Invoices WITH(NOLOCK) WHERE id = @Id"))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.CommandTimeout = 0;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        result = new Invoice();
                        result.Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id"));
                        result.Iva = dr.IsDBNull(dr.GetOrdinal("Iva")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Iva"));
                        result.SubTotal = dr.IsDBNull(dr.GetOrdinal("SubTotal")) ? 0 : dr.GetDecimal(dr.GetOrdinal("SubTotal"));
                        result.Total = dr.IsDBNull(dr.GetOrdinal("Total")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Total"));
                        result.ClientId = dr.IsDBNull(dr.GetOrdinal("ClientId")) ? 0 : dr.GetInt32(dr.GetOrdinal("ClientId"));
                    }
                }
            }
            return result;
        }

        public IEnumerable<Invoice> GetAll()
        {
            List<Invoice> results = null;
            Invoice result = null;
            using (var cmd = CreateCommand("SELECT * FROM Invoices WITH(NOLOCK)"))
            {                
                cmd.CommandTimeout = 0;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        result = new Invoice();
                        result.Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id"));
                        result.Iva = dr.IsDBNull(dr.GetOrdinal("Iva")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Iva"));
                        result.SubTotal = dr.IsDBNull(dr.GetOrdinal("SubTotal")) ? 0 : dr.GetDecimal(dr.GetOrdinal("SubTotal"));
                        result.Total = dr.IsDBNull(dr.GetOrdinal("Total")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Total"));
                        result.ClientId = dr.IsDBNull(dr.GetOrdinal("ClientId")) ? 0 : dr.GetInt32(dr.GetOrdinal("ClientId"));
                    }
                }
            }
            return results;
        }

        public void Update(Invoice model)
        {
            var query = "Update a SET a.ClienteId = @ClienteId, a.Iva = @Iva, a.SubTotal = @SubTotal, a.Total = @Total FROM Invoices a WHERE a.Id = @Id";            
            using (var cmd = CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@ClienteId", model.ClientId);
                cmd.Parameters.AddWithValue("@Iva", model.Iva);
                cmd.Parameters.AddWithValue("@SubTotal", model.SubTotal);
                cmd.Parameters.AddWithValue("@Total", model.Total);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }            
        }
    }
}
