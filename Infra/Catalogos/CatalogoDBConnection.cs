using Npgsql;
using System;
using System.Data;

namespace Infra.Catalogos
{
    public class CatalogoDBConnection : IDisposable
    {
        private readonly string _connectionString;
        private IDbConnection _connection;

        public CatalogoDBConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetConnection()
        {
            if (_connection == null || _connection.State == ConnectionState.Closed)
            {
                _connection = new NpgsqlConnection(_connectionString);
                _connection.Open();
            }

            return _connection;
        }

        public void Dispose()
        {
            if (_connection != null && _connection.State != ConnectionState.Closed)
            {
                _connection.Close();
                _connection.Dispose();
            }
        }
    }
}
