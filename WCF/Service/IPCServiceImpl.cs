using System;
using System.ServiceModel;

namespace WCF.Service
{
    public class BaseIPCServiceProviders : IIPCServiceProviders
    {
        public Action ExitAppProvider
        {
            protected get; set;
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class IPCServiceImpl : BaseIPCServiceProviders, IIPCService
    {
        public IIPCServiceProviders Providers
        {
            get => this;
        }

        public void ExitApp()
        {
            ExecuteProvider("ExitApp", ExitAppProvider);
        }

        void ExecuteProvider(string name, Action provider)
        {
            if (provider == null)
            {
                throw new NoProviderException($"{name} - No provider registered");
            }
            provider();
        }
    }

    public class NoProviderException : FaultException
    {
        public NoProviderException(string message) : base(message) { }
    }
}
