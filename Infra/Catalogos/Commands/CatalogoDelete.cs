using Dapper;
using Domain.Catalogos.Services;
using System.Threading.Tasks;

namespace Infra.Catalogos.Commands
{
    public class CatalogoDelete : ICatalogoDelete
    {
        private readonly CatalogoDBConnection _connection;
        
        public CatalogoDelete(CatalogoDBConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Delete(int id)
        {
            var query = "DELETE FROM Catalogo WHERE id = @Id";
            var parameters = new { Id = id };
            
            await _connection.GetConnection().ExecuteAsync(query, parameters);
            
            return id;
        }
    }
}
