using Application._Core;
using Application.Catalogos.Queries;
using Domain.Catalogos.Services;
using Infra.Catalogos.Queries;

namespace AdminBFF._Core.Injectors
{
    public static class CatalogoInjector
    {
        public static void Register(IServiceCollection services)
        {

            //Queries
            services.AddTransient<ICatalogoFind, CatalogoFind>();

            //Commands

            //Handlers
            services.AddTransient<IQueryHandler<CatalogoFindOneParams, Domain.Catalogos.Entities.Catalogo>, CatalogoFindOneHandler>();

            //Validators

        }
    }
}
