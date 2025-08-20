using Infra.Catalogos;
using Microsoft.EntityFrameworkCore;


namespace AdminBFF._Core
{
    public static class AppDatabase
    {
        public static void AddContext(this IServiceCollection services, ConfigurationManager config)
        {

            //Contexts (separar em outro arquivo)
            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<CatalogoDBContext>(options => {

                options.UseNpgsql(connectionString,
                    b => {
                        b.MigrationsAssembly(typeof(CatalogoDBContext).Assembly.FullName);
                        //b.MinPoolSize(1);
                        //b.MaxPoolSize(20);
                        //b.ConnectionIdleLifetime(300); // 5 minutos
                        //b.CommandTimeout(120);
                        //b.EnableRetryOnFailure(5);
                    });

            });

        }
    }
}
