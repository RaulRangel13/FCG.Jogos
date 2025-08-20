using Application._Core;
using Domain.Catalogos.DTO;
using Domain.Catalogos.Filters;
using Domain.Catalogos.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalogos.Queries
{
    public class CatalogoFindAllHandler : IQueryHandler<CatalogoFindAllParams, List<CatalogoDTO>>
    {
        private readonly ICatalogoFind _catalogoFind;

        public CatalogoFindAllHandler(ICatalogoFind catalogoFind)
        {
            _catalogoFind = catalogoFind;
        }

        public async Task<List<CatalogoDTO>> handle(CatalogoFindAllParams query)
        {
            var filter = new CatalogoFilter
            {
                searchValue = query.searchValue
            };

            var result = await this._catalogoFind.FindAll(filter);

            return result;
        }
    }

    public class CatalogoFindAllParams
    {
        public string? searchValue { get; set; }
    }
}
