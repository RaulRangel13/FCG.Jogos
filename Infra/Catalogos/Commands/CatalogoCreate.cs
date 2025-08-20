using Dapper;
using Domain.Catalogos.Commands;
using Domain.Catalogos.Entities;
using Domain.Catalogos.Services;
using System;
using System.Threading.Tasks;

namespace Infra.Catalogos.Commands
{
    public class CatalogoCreate : ICatalogoCreate
    {
        private readonly CatalogoDBConnection _connection;
        
        public CatalogoCreate(CatalogoDBConnection connection)
        {
            _connection = connection;
        }

        public async Task<Catalogo> Create(CatalogoNewCmd cmd)
        {
            cmd.itemCatalogo.created_at = DateTime.Now;
            
            var query = @"
                INSERT INTO Catalogo (name, description, created_at)
                VALUES (@Name, @Description, @CreatedAt)
                RETURNING id, name, description, created_at, alter_at";
            
            var parameters = new
            {
                Name = cmd.itemCatalogo.name,
                Description = cmd.itemCatalogo.description,
                CreatedAt = cmd.itemCatalogo.created_at
            };
            
            var result = await _connection.GetConnection().QueryFirstAsync<Catalogo>(query, parameters);
            
            return result;
        }
    }
}
