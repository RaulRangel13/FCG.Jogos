using Infra.Catalogos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Domain.Catalogos.Services;
using Infra.Catalogos.Queries;
using Infra.Catalogos.Commands;

namespace AdminBFF._Core
{
    public static class AppDatabase
    {
        public static void AddContext(this IServiceCollection services, ConfigurationManager config)
        {
            //Conexão com o banco de dados
            var connectionString = config.GetConnectionString("DefaultConnection");

            // Registrar a conexão do Dapper como singleton
            services.AddSingleton<CatalogoDBConnection>(provider => new CatalogoDBConnection(connectionString));

            // Registrar os repositórios usando factory para garantir que recebam a conexão corretamente
            services.AddTransient<ICatalogoFind>(provider => 
                new CatalogoFind(provider.GetRequiredService<CatalogoDBConnection>()));
                
            services.AddTransient<ICatalogoCreate>(provider => 
                new CatalogoCreate(provider.GetRequiredService<CatalogoDBConnection>()));
                
            services.AddTransient<ICatalogoChange>(provider => 
                new CatalogoChange(provider.GetRequiredService<CatalogoDBConnection>()));
                
            services.AddTransient<ICatalogoDelete>(provider => 
                new CatalogoDelete(provider.GetRequiredService<CatalogoDBConnection>()));
        }
    }
}
