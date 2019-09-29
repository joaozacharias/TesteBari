using System;
using System.Collections.Generic;
using System.Text;

namespace TesteBari.Core.Infra.Log
{
    public interface IBariLogger
    {
        Guid Info(string msg, params object[] parametros);
        Guid Error(string msg, params object[] parametros);
        Guid Error(Exception ex);
    }
}
