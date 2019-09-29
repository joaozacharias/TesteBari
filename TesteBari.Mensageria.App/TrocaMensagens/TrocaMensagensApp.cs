using System;
using System.Threading.Tasks;
using TesteBari.Core.Domain.Entidades;
using TesteBari.Core.Infra.Configuracoes;
using TesteBari.Core.Infra.Log;
using TesteBari.Mensageria.Domain.Interface;

namespace TesteBari.Mensageria.App.TrocaMensagens
{
    class TrocaMensagensApp : ITrocaMensagensApp
    {
        private readonly IMensageriaServico _mensageriaService;
        private readonly IBariLogger _logger;
        private readonly IdentificadorServico _identificador;

        public TrocaMensagensApp(IMensageriaServico mensageriaService, IBariLogger logger, IdentificadorServico identificador)
        {
            _mensageriaService = mensageriaService;
            _logger = logger;
            _identificador = identificador;
        }

        public Task EnvioMensagens(string fila)
        {
            var mensagem = new Mensagem
            {
                HorarioEnvio = DateTime.Now.TimeOfDay,
                Id = Guid.NewGuid(),
                Texto = "Hello World",
                IdServico = _identificador.Identificador
            };

            _mensageriaService.EnviarMensagem(fila, mensagem);

            Console.WriteLine($"{DateTime.Now.TimeOfDay} - Enviando - {mensagem.Id}");

            return Task.FromResult(true);
        }

        public Task RecebimentoMensagens(string fila)
        {
            _mensageriaService.ReceberMensagem<Mensagem>(fila, msg => ImprimirMensagem(Newtonsoft.Json.JsonConvert.DeserializeObject<Mensagem>(msg)));
            return Task.FromResult(true);
        }

        private bool ImprimirMensagem(Mensagem mensagem)
        {
            if(mensagem.IdServico != _identificador.Identificador)
            {
                Console.WriteLine($"\nId:{mensagem.Id} \nIdServiço: {mensagem.IdServico} \nTimeStamp: {mensagem.HorarioEnvio} \nTexto: {mensagem.Texto} \n");
                return true;
            }
            return false;
            
        }
    }
}
