using Domain.Catalogos.DTO;
using Domain.Catalogos.Filters;
using Domain.Catalogos.Services;
using Microsoft.EntityFrameworkCore;
using UTIL.Extensions;

namespace Infra.Catalogos.Queries
{
    public class CatalogoFind : ICatalogoFind
    {
        private readonly CatalogoDBContext _context;

        public CatalogoFind(CatalogoDBContext context)
        {
            _context = context;
        }

        public async Task<Domain.Catalogos.Entities.Catalogo> FindById(int id)
        {

            var result = await this._context.Catalogo.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);

            return result;

        }

        public async Task<List<CatalogoDTO>> FindAll(CatalogoFilter filter)
        {

            var query = from item in this._context.Catalogo select item;

            filter = filter.getTrimStringProperties();

            if (!string.IsNullOrEmpty(filter.searchValue))
            {

                query = query.Where(x =>
                    EF.Functions.ILike(x.description, $"%{filter.searchValue}%") ||
                    EF.Functions.ILike(x.name, $"%{filter.searchValue}%") 
                );

            }

            var result = await query.AsNoTracking().Select(x => new CatalogoDTO
            {
                id = x.id,
                name = x.name,
                description = x.description,
                created_at = x.created_at,
                alter_at = x.alter_at
            }).ToListAsync();

            return result;

        }

        public async Task<bool> FindAnyByFilter(CatalogoFilter filter) => await this._context.Catalogo.AsNoTracking().AnyAsync(x => x.id == filter.id);

    }
}
