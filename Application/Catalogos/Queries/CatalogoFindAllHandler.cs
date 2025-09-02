using Application._Core;
using Domain.Catalogos.DTO;
using Domain.Catalogos.Entities;
using Domain.Catalogos.Filters;
using Domain.Catalogos.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Catalogos.Queries
{
    public class CatalogoFindAllHandler : IQueryHandler<CatalogoFindAllParams, List<CatalogoDTO>>
    {
        private readonly ICatalogoFind _catalogoFind;
        private readonly ICatalogoSearchService _searchService;

        public CatalogoFindAllHandler(ICatalogoFind catalogoFind, ICatalogoSearchService searchService)
        {
            _catalogoFind = catalogoFind;
            _searchService = searchService;
        }

        public async Task<List<CatalogoDTO>> handle(CatalogoFindAllParams query)
        {
            // Se houver termo de busca, usa o Elasticsearch
            if (!string.IsNullOrWhiteSpace(query.searchValue))
            {
                var resultados = await _searchService.SearchCatalogosAsync(query.searchValue);
                return MapToDtoList(resultados);
            }

            // Caso contrário, busca normal no banco de dados
            var filter = new CatalogoFilter
            {
                searchValue = query.searchValue
            };

            return await _catalogoFind.FindAll(filter);
        }

        private List<CatalogoDTO> MapToDtoList(IEnumerable<Catalogo> catalogos)
        {
            return catalogos.Select(c => new CatalogoDTO
            {
                id = c.id,
                name = c.name,
                description = c.description,
                created_at = c.created_at,
                alter_at = c.alter_at
            }).ToList();
        }
    }

    public class CatalogoFindAllParams
    {
        public string? searchValue { get; set; }
    }
}
