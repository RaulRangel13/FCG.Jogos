using AdminBFF._Core.Injectors;

namespace AdminBFF._Core
{
    public static class AppServiceInjectors
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {

            CatalogoInjector.Register(services);

        }
    }
}
