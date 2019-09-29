using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.Configuration;
using System;
using TesteBari.Core.App.Interface;
using TesteBari.Mensageria.App.TrocaMensagens;

namespace TesteBari.Core.App.Servico
{
    class ControleServicos : IControleServicos
    {
        private readonly IConfiguration _configuration;
        private readonly ITrocaMensagensApp _trocaMensagensApp;

        public ControleServicos(IConfiguration configuration, ITrocaMensagensApp trocaMensagensApp)
        {
            _configuration = configuration;
            _trocaMensagensApp = trocaMensagensApp;
        }

        public void Iniciar()
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                foreach (var recurringJob in connection.GetRecurringJobs())
                {
                    RecurringJob.RemoveIfExists(recurringJob.Id);
                }
            }

            var configuracaoCron = _configuration["configuracaoCron"];
            var nomeFilaMensageria = _configuration["nomeFilaMensageria"];

            if (!string.IsNullOrEmpty(configuracaoCron))
            {
                RecurringJob.AddOrUpdate<ITrocaMensagensApp>("enviomensagens", (execucao) => execucao.EnvioMensagens(nomeFilaMensageria),
                    configuracaoCron, timeZone: TimeZoneInfo.Local);
                _trocaMensagensApp.RecebimentoMensagens(nomeFilaMensageria);
            }
        }
    }
}
