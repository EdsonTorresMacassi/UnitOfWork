using Models;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Repository.SqlServer
{
    public class ProductRepository : Repository, IProductRepository
    {
        public ProductRepository(SqlConnection cn, SqlTransaction transaction)
        {
            this._cn = cn;
            this._transaction = transaction;
        }

        public Product Get(int id)
        {
            Product result = null;            
            using (var cmd = CreateCommand("SELECT * FROM Products WITH(NOLOCK) WHERE id = @ProductId"))
            {
                cmd.Parameters.AddWithValue("@ProductId", id);
                cmd.CommandTimeout = 0;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        result = new Product();
                        result.Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id"));
                        result.Name = dr.IsDBNull(dr.GetOrdinal("Name")) ? "" : dr.GetString(dr.GetOrdinal("Name"));
                        result.Price = dr.IsDBNull(dr.GetOrdinal("Price")) ? 0 : dr.GetDecimal(dr.GetOrdinal("Price"));
                    }
                }
            }
            return result;
        }

        public IEnumerable<Product> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}
