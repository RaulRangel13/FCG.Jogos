using Dapper;
using Domain.Catalogos.Commands;
using Domain.Catalogos.Entities;
using Domain.Catalogos.Services;
using System;
using System.Threading.Tasks;

namespace Infra.Catalogos.Commands
{
    public class CatalogoChange : ICatalogoChange
    {
        private readonly CatalogoDBConnection _connection;
        
        public CatalogoChange(CatalogoDBConnection connection)
        {
            _connection = connection;
        }
        
        public async Task<Catalogo> Change(CatalogoChangeCmd cmd)
        {
            cmd.itemCatalogo.alter_at = DateTime.Now;
            
            var query = @"
                UPDATE Catalogo 
                SET name = @Name, 
                    description = @Description, 
                    alter_at = @AlterAt 
                WHERE id = @Id
                RETURNING id, name, description, created_at, alter_at";
            
            var parameters = new
            {
                Id = cmd.itemCatalogo.id,
                Name = cmd.itemCatalogo.name,
                Description = cmd.itemCatalogo.description,
                AlterAt = cmd.itemCatalogo.alter_at
            };
            
            var result = await _connection.GetConnection().QueryFirstAsync<Catalogo>(query, parameters);
            
            return result;
        }
    }
}
