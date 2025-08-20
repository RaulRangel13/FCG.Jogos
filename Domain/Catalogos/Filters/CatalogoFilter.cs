using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Catalogos.Filters
{
    public class CatalogoFilter
    {
        public int id { get; set; }
        public string name { get; set; }
        public string? searchValue { get; set; }

    }
}
