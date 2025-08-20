using Domain.Catalogos.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Catalogos.Commands
{
    public class CatalogoDelete : ICatalogoDelete
    {
        private readonly CatalogoDBContext _context;
        public CatalogoDelete(CatalogoDBContext context)
        {
            _context = context;
        }

        public async Task<int> Delete(int id)
        {

            this._context.Catalogo.AsNoTracking().Where(x => x.id == id).ExecuteDelete();

            return id;
        }
    }
}
