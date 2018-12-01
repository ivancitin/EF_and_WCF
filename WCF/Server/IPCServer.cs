using System;
using WCF.Service;

namespace WCF.Server
{
    public interface IPCServer<S, R> : IDisposable where S : IIPCService
    {
        S Service
        {
            get;
        }

        R Start();
    }
}
