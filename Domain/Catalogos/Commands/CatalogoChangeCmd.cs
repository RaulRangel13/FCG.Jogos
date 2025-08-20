using Domain._Core;
using Domain.Catalogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Catalogos.Commands
{
    public class CatalogoChangeCmd
    {
        public CatalogoChangeCmd()
        {
            itemCatalogo = new Catalogo();
        }

        public Catalogo itemCatalogo { get; set; }

        public class Result : ResultBaseCmd
        {

            public int id { get; set; }

        }
    }
}
