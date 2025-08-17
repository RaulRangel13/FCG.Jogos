using Domain.Catalogos.Services;
using Microsoft.EntityFrameworkCore;

namespace Infra.Catalogos.Queries
{
    public class CatalogoFind : ICatalogoFind
    {
        private readonly CatalogoDBContext _context;

        public CatalogoFind(CatalogoDBContext context)
        {
            _context = context;
        }

        public async Task<Domain.Catalogos.Entities.Catalogo> FindById(Guid id)
        {

            var result = await this._context.Catalogo.FirstOrDefaultAsync(x => x.id == id);

            return result;

        }
    }
}
