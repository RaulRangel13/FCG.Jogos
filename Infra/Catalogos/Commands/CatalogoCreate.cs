using Domain.Catalogos.Commands;
using Domain.Catalogos.Entities;
using Domain.Catalogos.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Catalogos.Commands
{
    public class CatalogoCreate : ICatalogoCreate
    {
        private readonly CatalogoDBContext _context;
                public CatalogoCreate(CatalogoDBContext context)
        {
            _context = context;
        }

        public async Task<Catalogo> Create(CatalogoNewCmd cmd)
        {
            cmd.itemCatalogo.created_at = DateTime.Now;
            await this._context.Catalogo.AddAsync(cmd.itemCatalogo);

            _ = await _context.SaveChangesAsync();

            return cmd.itemCatalogo;
        }
    }
}
