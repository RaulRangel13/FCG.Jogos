using Application._Core;
using Application.Catalogos.Commands;
using Application.Catalogos.Queries;
using Application.Catalogos.Validators;
using Domain.Catalogos.Commands;
using Domain.Catalogos.DTO;
using Domain.Catalogos.Entities;
using Domain.Catalogos.Services;
using Infra.Catalogos.Commands;
using Infra.Catalogos.Queries;

namespace AdminBFF._Core.Injectors
{
    public static class CatalogoInjector
    {
        public static void Register(IServiceCollection services)
        {
            // Nota: Os repositórios (ICatalogoFind, ICatalogoCreate, etc.) agora são registrados no AppDatabase.cs
            // para garantir que recebam a conexão do Dapper corretamente

            //Handlers
            //services.AddTransient<IQueryHandler<CatalogoFindOneParams, Domain.Catalogos.Entities.Catalogo>, CatalogoFindOneHandler>();
            services.AddTransient<IQueryHandler<CatalogoFindOneParams, Catalogo>, CatalogoFindOneHandler>();
            services.AddTransient<IQueryHandler<CatalogoFindAllParams, List<CatalogoDTO>>, CatalogoFindAllHandler>();
            services.AddTransient<ICommandHandler<CatalogoNewCmd, CatalogoNewCmd.Result>, CatalogoNewHandler>();
            services.AddTransient<ICommandHandler<CatalogoChangeCmd, CatalogoChangeCmd.Result>, CatalogoChangeHandler>();
            services.AddTransient<ICommandHandler<CatalogoDeleteCmd, CatalogoDeleteCmd.Result>, CatalogoDeleteHandler>();

            //Validators
            services.AddKeyedTransient<ValidatorBase<Catalogo>, CatalogoNewValidator>("new");
            services.AddKeyedTransient<ValidatorBase<Catalogo>, CatalogoChangeValidator>("change");
            services.AddKeyedTransient<ValidatorBase<Catalogo>, CatalogoDeleteValidator>("delete");
        }
    }
}
