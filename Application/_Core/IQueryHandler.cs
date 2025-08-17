using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application._Core
{
    public interface IQueryHandler<TQuery, TResult>
    {
        Task<TResult> handle(TQuery query);
    }
}
