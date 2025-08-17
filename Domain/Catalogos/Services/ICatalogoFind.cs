using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Catalogos.Services
{
    public interface ICatalogoFind
    {
        Task<Entities.Catalogo> FindById(Guid id);

    }
}
