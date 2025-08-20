using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Catalogos.Entities
{
    public class Catalogo : CatalogoEntity
    {
    }

    public class CatalogoEntity
{

        public int id { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public DateTime created_at { get; set; }

        public DateTime? alter_at { get; set; }


    }
}
