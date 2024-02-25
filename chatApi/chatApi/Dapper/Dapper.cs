using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace chatApi.Dapper
{
    public class Dapper : IDapper
    {
        private readonly IConfiguration configuration;
        private const string connectionString = "ChatConnection";
        public Dapper(IConfiguration configuration) => this.configuration = configuration;
        public T Get<T>(string sp, DynamicParameters parameters)
        {
            T res;
            using IDbConnection db = new SqlConnection(configuration.GetConnectionString(connectionString));
            try
            {
                res = db.Query<T>(sp, parameters, null, true, null, CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.HResult = 0; throw;
            }
            return res;
        }

        public List<T> GetAll<T>(string sp, DynamicParameters parameters)
        {
            List<T> res;
            using IDbConnection db = new SqlConnection(configuration.GetConnectionString(connectionString));
            try
            {
                res = db.Query<T>(sp, parameters, null, true, null, CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                ex.HResult = 0; throw;
            }
            return res;
        }

        public List<T> GetAllList<T>(string sp)
        {
            List<T> res;
            using IDbConnection db = new SqlConnection(configuration.GetConnectionString(connectionString));
            try
            {
                res = db.Query<T>(sp, null, null, true, null, CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                ex.HResult = 0; throw;
            }
            return res;
        }

        public T Post<T>(string sp, DynamicParameters parameters)
        {
            T result;
            using IDbConnection db = new SqlConnection(configuration.GetConnectionString(connectionString));
            try
            {
                if (db.State == ConnectionState.Closed) db.Open();
                using var transaction = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parameters, transaction, true, null, CommandType.StoredProcedure).FirstOrDefault();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ex.HResult = 0;
                    throw;
                }
            }
            catch (Exception ex)
            {
                ex.HResult = 0;
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open) db.Close();
            }
            return result;

        }
    }
}
