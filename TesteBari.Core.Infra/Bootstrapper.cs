using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using TesteBari.Core.Infra.Configuracoes;
using TesteBari.Core.Infra.Log;

namespace TesteBari.Core.Infra
{
    public static class Bootstrapper
    {
        public static void ConfiguracaoCoreInfra(this IServiceCollection services, IConfiguration Configuration)
        {
            var mqConfiguracao = new MQConfiguracao();
            var section = Configuration.GetSection("configuracaoFila");
            new ConfigureFromConfigurationOptions<MQConfiguracao>(section).Configure(mqConfiguracao);
            services.AddSingleton(mqConfiguracao);

            var identificadorServico = new IdentificadorServico {
                Identificador = Guid.NewGuid()
            };

            Console.WriteLine($"Id do serviço - {identificadorServico.Identificador}");

            services.AddSingleton(identificadorServico);

            services.AddSingleton<IBariLogger, BariLogger>();

        }
    }
}
