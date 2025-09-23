using Domain.Catalogos.Entities;
using Domain.Catalogos.Services;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elastic.Transport;

namespace Infra.Catalogos.Services
{
    public class ElasticsearchCatalogoService : ICatalogoSearchService
    {
        private readonly ElasticsearchClient _client;
        private readonly string _indexName;
        private const string UserHistoryIndex = "user_history";

        public ElasticsearchCatalogoService(IConfiguration configuration)
        {
            var uri = new Uri(configuration["Elasticsearch:Uri"]);
            
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
            
            EnsureUserHistoryIndexExists().Wait();
        }

        private async Task EnsureUserHistoryIndexExists()
        {
            var exists = await _client.Indices.ExistsAsync(UserHistoryIndex);
            if (!exists.Exists)
            {
                await _client.Indices.CreateAsync(UserHistoryIndex);
            }
        }

        public async Task SaveUserHistoryAsync(int userId, int gameId)
        {
            var historyEntry = new
            {
                userId,
                gameId,
                timestamp = DateTime.UtcNow
            };

            var response = await _client.IndexAsync(historyEntry, idx => 
                idx.Index(UserHistoryIndex));
            
            if (!response.IsValidResponse)
            {
                throw new Exception($"Falha ao salvar histórico do usuário: {response.DebugInformation}");
            }
        }
        
        public async Task<IEnumerable<Catalogo>> GetGameRecommendationsAsync(int userId, int limit = 5)
        {
            // Buscar documentos de histórico do usuário e calcular top N via LINQ (evita agregações Terms)
            var userHistoryResponse = await _client.SearchAsync<UserHistoryDoc>(s => s
                .Index(UserHistoryIndex)
                .Size(1000)
                .Query(q => q.Term(t => t.Field("userId").Value(userId)))
            );

            if (!userHistoryResponse.IsValidResponse)
            {
                throw new Exception($"Falha ao buscar histórico do usuário: {userHistoryResponse.DebugInformation}");
            }
            
            // Pega os 5 jogos favoritos
            var topGames = userHistoryResponse.Documents
                .GroupBy(d => d.gameId)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => g.Key)
                .ToList();

// Pega TODOS os jogos que o usuário já jogou para excluir
            var allUserGamesIds = userHistoryResponse.Documents
                .Select(d => d.gameId)
                .Distinct()
                .ToList();

// (Opcional, mas necessário para a busca híbrida)
// var distinctTopCategories = await GetCategoriesForGamesAsync(topGames);

// ✨ PASSO NOVO: Crie a lista de documentos para o MLT
            var likeDocuments = topGames
                .Select(gameId => new Like(new LikeDocument { Id = new Id(gameId) }))
                .ToList();

            var searchResponse = await _client.SearchAsync<Catalogo>(s => s
                .Index(_indexName)
                .Size(limit)
                .Query(q => q
                    .Bool(b =>
                    {
                        b.Should(
                            // 1. Relevância por texto (MLT) com a sintaxe corrigida e mais limpa
                            sh => sh.MoreLikeThis(mlt => mlt
                                .Fields(f => f.title, f => f.description)
                                .Like(likeDocuments) // Passando a lista pronta
                                .MinTermFreq(1)
                                .MinDocFreq(1)
                            )
                            // ,
                            // // 2. Relevância por categoria com boost
                            // sh => sh.Terms(t => t
                            //     .Field(p => p.idCategoria) // Define o campo
                            //     .Terms(new TermsQueryField(distinctTopCategories)) // Passa os valores dentro de um TermsQueryField
                            //     .Boost(2.0)
                            // )
                        );

                        b.MinimumShouldMatch(1); 

                        b.MustNot(mn => mn
                            .Terms(t => t
                                .Field(p => p.id) // Define o campo
                                .Terms(new TermsQueryField(allUserGamesIds.Select(id => (FieldValue)id).ToList()))
                            )
                        );
                    })
                )
            );
            
            
            
            
            
            

            // // Busca os jogos recomendados baseados nos jogos mais acessados
            // var searchResponse = await _client.SearchAsync<Catalogo>(s => 
            // {
            //     s
            //         .Index(_indexName)
            //         .Size(limit)
            //         .Query(q => q.Bool(b =>
            //         {
            //             foreach (var gameId in topGames)
            //             {
            //                 b.Should(sh => sh.Term(t => t.Field("id").Value(gameId)));
            //             }
            //             b.MinimumShouldMatch(1);
            //         }));
            // });

            if (!searchResponse.IsValidResponse)
            {
                throw new Exception($"Falha ao buscar recomendações: {searchResponse.DebugInformation}");
            }

            return searchResponse.Documents;
        }

        private async Task<IEnumerable<Catalogo>> GetPopularGames(int limit)
        {
            var response = await _client.SearchAsync<Catalogo>(s => s
                .Index(_indexName)
                .Size(limit)
                .Sort(srt => srt
                    .Field(f => f.Field("created_at").Order(SortOrder.Desc))
                )
            );

            return response.Documents;
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

        private class UserHistoryDoc
        {
            public int userId { get; set; }
            public int gameId { get; set; }
            public DateTime timestamp { get; set; }
        }
    }
}
