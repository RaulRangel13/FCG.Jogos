using Application._Core;
using Domain.Catalogos.Entities;
using Domain.Catalogos.Services;

namespace Application.Catalogos.Queries
{
    public class CatalogoFindOneHandler : IQueryHandler<CatalogoFindOneParams, Domain.Catalogos.Entities.Catalogo>
    {
        private readonly ICatalogoFind _finder;
        private readonly ICatalogoSearchService _searchService;

        public CatalogoFindOneHandler(ICatalogoFind finder, ICatalogoSearchService searchService)
        {
            _finder = finder;
            _searchService = searchService;
        }

        public async Task<Domain.Catalogos.Entities.Catalogo> handle(CatalogoFindOneParams request)
        {

            var result = await this._finder.FindById(request.id);
            
            if(result?.id > 0)
                await _searchService.SaveUserHistoryAsync(request.idUsuario, request.id);

            return result;
        }
    }

    public class CatalogoFindOneParams
    {

        public int id { get; set; }
        public int idUsuario { get; set; }
        

    }
}
