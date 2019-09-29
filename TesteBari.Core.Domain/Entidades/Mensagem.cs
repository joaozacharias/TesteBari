using System;

namespace TesteBari.Core.Domain.Entidades
{
    public class Mensagem
    {
        public Guid Id { get; set; }
        public string Texto { get; set; }
        public Guid IdServico { get; set; }
        public TimeSpan HorarioEnvio { get; set; }
    }
}
