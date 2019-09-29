using System;

namespace TesteBari.Core.Infra.Log
{
    class BariLogger : IBariLogger
    {
        private Serilog.ILogger _logger;

        public BariLogger(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public Guid Error(string msg, params object[] parametros)
        {
            var id = Guid.NewGuid();
            this.NewLogger(id).Error(msg, parametros);
            return id;
        }

        public Guid Error(Exception ex)
        {
            var id = Guid.NewGuid();
            NewLogger(id).Error(ex, ex.Message);
            return id;
        }

        public Guid Info(string msg, params object[] parametros)
        {
            var id = Guid.NewGuid();
            NewLogger(id).Information(msg, parametros);
            return id;
        }

        private Serilog.ILogger NewLogger(Guid id)
        {
            return _logger.ForContext("EventId", id);
        }
    }
}
