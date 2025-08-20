using Application._Core;
using Domain.Catalogos.Entities;
using Domain.Catalogos.Services;

namespace Application.Catalogos.Queries
{
    public class CatalogoFindOneHandler : IQueryHandler<CatalogoFindOneParams, Domain.Catalogos.Entities.Catalogo>
    {
        private readonly ICatalogoFind _finder;

        public CatalogoFindOneHandler(ICatalogoFind finder  )
        {
            _finder = finder;
        }

        public async Task<Domain.Catalogos.Entities.Catalogo> handle(CatalogoFindOneParams request)
        {

            var result = await this._finder.FindById(request.id);

            return result;
        }
    }

    public class CatalogoFindOneParams
    {

        public int id { get; set; }

    }
}
