using System;
using System.Threading.Tasks;
using TesteBari.Core.Domain.Entidades;

namespace TesteBari.Mensageria.Domain.Interface
{
    public interface IMensageriaServico
    {
        Task<bool> EnviarMensagem(string queue, object sendObject);
        Task<bool> ReceberMensagem<T>(string queue, Func<string, bool> func);
    }
}
