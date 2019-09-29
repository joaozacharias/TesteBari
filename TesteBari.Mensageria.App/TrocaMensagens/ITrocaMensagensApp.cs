using System.Threading.Tasks;

namespace TesteBari.Mensageria.App.TrocaMensagens
{
    public interface ITrocaMensagensApp
    {
        Task EnvioMensagens(string fila);
        Task RecebimentoMensagens(string fila);
    }
}
