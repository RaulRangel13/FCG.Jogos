using Dapper;
using Domain.Catalogos.DTO;
using Domain.Catalogos.Entities;
using Domain.Catalogos.Filters;
using Domain.Catalogos.Services;
using System.Data;
using UTIL.Extensions;

namespace Infra.Catalogos.Queries
{
    public class CatalogoFind : ICatalogoFind
    {
        private readonly CatalogoDBConnection _connection;

        public CatalogoFind(CatalogoDBConnection connection)
        {
            _connection = connection;
        }

        public async Task<Domain.Catalogos.Entities.Catalogo> FindById(int id)
        {
            var query = "SELECT id, name, description, created_at, alter_at FROM Catalogo WHERE id = @Id";
            
            var parameters = new { Id = id };
            
            var result = await _connection.GetConnection().QueryFirstOrDefaultAsync<Catalogo>(query, parameters);

            return result;
        }

        public async Task<List<CatalogoDTO>> FindAll(CatalogoFilter filter)
        {
            filter = filter.getTrimStringProperties();

            var query = "SELECT id, name, description, created_at, alter_at FROM Catalogo WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(filter.searchValue))
            {
                query += " AND (description ILIKE @SearchValue OR name ILIKE @SearchValue)";
                parameters.Add("SearchValue", $"%{filter.searchValue}%");
            }

            var result = await _connection.GetConnection().QueryAsync<CatalogoDTO>(query, parameters);

            return result.ToList();
        }

        public async Task<bool> FindAnyByFilter(CatalogoFilter filter)
        {
            var query = "SELECT COUNT(1) FROM Catalogo WHERE id = @Id";
            var parameters = new { Id = filter.id };
            
            var count = await _connection.GetConnection().ExecuteScalarAsync<int>(query, parameters);
            
            return count > 0;
        }
    }
}
