using Domain.Catalogos.Entities;
using Domain.Catalogos.Services;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elastic.Transport;

namespace Infra.Catalogos.Services
{
    public class ElasticsearchCatalogoService : ICatalogoSearchService
    {
        private readonly ElasticsearchClient _client;
        private readonly string _indexName;

        public ElasticsearchCatalogoService(IConfiguration configuration)
        {
            var uri = new Uri(configuration["Elasticsearch:Uri"]);
            
            // Configuração básica do cliente
            var settings = new ElasticsearchClientSettings(uri)
                .DefaultIndex("catalogos")
                .DisableDirectStreaming()
                .PrettyJson()
                .DefaultMappingFor<Catalogo>(m => m
                    .IdProperty(p => p.id)
                    .IndexName("catalogos")
                );

            _client = new ElasticsearchClient(settings);
            _indexName = "catalogos";
        }

        public async Task IndexCatalogoAsync(Catalogo catalogo)
        {
            var response = await _client.IndexAsync(catalogo, idx => 
                idx.Index(_indexName).Id(catalogo.id));
            
            if (!response.IsValidResponse)
            {
                throw new Exception($"Falha ao indexar catálogo: {response.DebugInformation}");
            }
        }

        public async Task UpdateCatalogoAsync(Catalogo catalogo)
        {
            var response = await _client.UpdateAsync<Catalogo, object>(
                _indexName, 
                catalogo.id, 
                u => u.Doc(catalogo));
                
            if (!response.IsValidResponse)
            {
                throw new Exception($"Falha ao atualizar catálogo: {response.DebugInformation}");
            }
        }

        public async Task DeleteCatalogoAsync(int id)
        {
            var response = await _client.DeleteAsync<Catalogo>(id, idx => idx.Index(_indexName));
            if (!response.IsValidResponse && response.Result != Result.NotFound)
            {
                throw new Exception($"Falha ao remover catálogo: {response.DebugInformation}");
            }
        }

        public async Task<IEnumerable<Catalogo>> SearchCatalogosAsync(string searchTerm)
        {
            var response = await _client.SearchAsync<Catalogo>(s => s
                .Index(_indexName)
                .Query(q => q
                    .MultiMatch(mm => mm
                        .Query(searchTerm)
                        .Fields(new[] { "name^3", "description" })
                        .Fuzziness("AUTO")
                    )
                )
            );

            if (!response.IsValidResponse)
            {
                throw new Exception($"Falha na busca: {response.DebugInformation}");
            }

            return response.Documents;
        }

        public async Task EnsureIndexExists()
        {
            var exists = await _client.Indices.ExistsAsync(_indexName);
            if (!exists.Exists)
            {
                var response = await _client.Indices.CreateAsync(_indexName);
                
                if (!response.IsValidResponse)
                {
                    throw new Exception($"Falha ao criar índice: {response.DebugInformation}");
                }
            }
        }
    }
}
