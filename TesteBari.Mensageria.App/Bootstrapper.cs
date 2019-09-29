using Microsoft.Extensions.DependencyInjection;
using TesteBari.Mensageria.App.TrocaMensagens;
using TesteBari.Mensageria.Domain;

namespace TesteBari.Mensageria.App
{
    public static class Bootstrapper
    {
        public static void ConfiguracaoMensageriaApp(this IServiceCollection services)
        {
            services.AddScoped<ITrocaMensagensApp, TrocaMensagensApp>();

            services.ConfiguracaoMensageriaDomain();
        }
    }
}
