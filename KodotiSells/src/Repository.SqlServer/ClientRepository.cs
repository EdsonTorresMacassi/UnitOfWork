using Models;
using Repository.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Repository.SqlServer
{
    public class ClientRepository : Repository, IClientRepository
    {
        public ClientRepository(SqlConnection cn, SqlTransaction transaction)
        {
            this._cn = cn;
            this._transaction = transaction;
        }

        public Client Get (int id)
        {
            Client result = null;
            using (var cmd = CreateCommand("SELECT * FROM Clients WITH(NOLOCK) WHERE id = @ClienteId"))
            {
                cmd.Parameters.AddWithValue("@ClienteId", id);
                cmd.CommandTimeout = 0;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        result = new Client();
                        result.Id = dr.IsDBNull(dr.GetOrdinal("Id")) ? 0 : dr.GetInt32(dr.GetOrdinal("Id"));
                        result.Name = dr.IsDBNull(dr.GetOrdinal("Name")) ? "" : dr.GetString(dr.GetOrdinal("Name"));
                    }
                }
            }
            return result;
        }

        public IEnumerable<Client> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}
