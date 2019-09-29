using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TesteBari.Core.App.Interface;
using TesteBari.Core.App.Servico;
using TesteBari.Core.Infra;
using TesteBari.Mensageria.App;

namespace TesteBari.Core.App.Extensoes
{
    public static class IServiceCollectionExtensions
    {
        public static void InicializarServicos(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IControleServicos, ControleServicos>();
            services.ConfiguracaoCoreInfra(configuration);
            services.ConfiguracaoMensageriaApp();
        }
    }
}
