using Domain.Catalogos.Commands;
using Domain.Catalogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Catalogos.Services
{
    public interface ICatalogoChange
    {
        Task<Catalogo> Change(CatalogoChangeCmd cmd);

    }
}
