using Domain.Catalogos.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Catalogos.Services
{
    public interface ICatalogoSearchService
    {
        Task IndexCatalogoAsync(Catalogo catalogo);
        Task UpdateCatalogoAsync(Catalogo catalogo);
        Task DeleteCatalogoAsync(int id);
        Task<IEnumerable<Catalogo>> SearchCatalogosAsync(string searchTerm);
        
        // New methods for user history and recommendations
        Task SaveUserHistoryAsync(int userId, int gameId);
        Task<IEnumerable<Catalogo>> GetGameRecommendationsAsync(int userId, int limit = 5);
    }
}
