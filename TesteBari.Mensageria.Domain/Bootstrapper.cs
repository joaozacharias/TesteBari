using Microsoft.Extensions.DependencyInjection;
using TesteBari.Mensageria.Domain.Interface;
using TesteBari.Mensageria.Domain.Servico;

namespace TesteBari.Mensageria.Domain
{
    public static class Bootstrapper
    {
        public static void ConfiguracaoMensageriaDomain(this IServiceCollection services)
        {
            services.AddScoped<IMensageriaServico, MensageriaServico>();
        }
    }
}
