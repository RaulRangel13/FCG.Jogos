using Domain.Catalogos.DTO;
using Domain.Catalogos.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Catalogos.Services
{
    public interface ICatalogoFind
    {
        Task<Entities.Catalogo> FindById(int id);
        Task<List<CatalogoDTO>> FindAll(CatalogoFilter filter);
        Task<bool> FindAnyByFilter(CatalogoFilter filter);

    }
}
