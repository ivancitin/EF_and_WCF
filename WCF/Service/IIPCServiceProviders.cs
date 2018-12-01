using System;

namespace WCF.Service
{
    public interface IIPCServiceProviders
    {
        Action ExitAppProvider
        {
            set;
        }
    }
}
