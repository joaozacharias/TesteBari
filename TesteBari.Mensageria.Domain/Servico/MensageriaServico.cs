using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using TesteBari.Core.Infra.Configuracoes;
using TesteBari.Core.Infra.Log;
using TesteBari.Mensageria.Domain.Interface;

namespace TesteBari.Mensageria.Domain.Servico
{
    class MensageriaServico : IMensageriaServico
    {
        private readonly MQConfiguracao _mqConfiguracao;
        private readonly IBariLogger _logger;
        private ConnectionFactory _factory;

        public MensageriaServico(MQConfiguracao mqConfiguracao, IBariLogger logger)
        {
            _mqConfiguracao = mqConfiguracao;
            _logger = logger;
        }

        public Task<bool> EnviarMensagem(string queue, object sendObject)
        {
            _factory = new ConnectionFactory() { HostName = _mqConfiguracao.HostName, UserName = _mqConfiguracao.UserName, Password = _mqConfiguracao.Password };

            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    try
                    {
                        channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);

                        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(sendObject));

                        channel.BasicPublish(exchange: "",
                                             routingKey: queue,
                                             basicProperties: null,
                                             body: body);

                        return Task.FromResult(true);

                    }
                    catch(Exception e)
                    {
                        _logger.Error(e);
                        return Task.FromResult(false);
                    }

                }
            }
        }

        public Task<bool> ReceberMensagem<T>(string queue, Func<string, bool> func)
        {
            _factory = new ConnectionFactory() { HostName = _mqConfiguracao.HostName, UserName = _mqConfiguracao.UserName, Password = _mqConfiguracao.Password };

            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {

                        try
                        {
                            var message = Encoding.UTF8.GetString(ea.Body);
                            
                            var retornoAcao = func?.Invoke(message);
                            if (retornoAcao ?? false)
                                channel.BasicAck(ea.DeliveryTag, true);
                            else
                                channel.BasicReject(ea.DeliveryTag, true);

                        }
                        catch (Exception e)
                        {
                            _logger.Error(e);
                        }
                    };

                    channel.BasicConsume(queue: queue,
                                            autoAck: false,
                                            consumer: consumer);

                    Console.ReadLine();
                }

            }

            return Task.FromResult(true);
        }

    }
}
