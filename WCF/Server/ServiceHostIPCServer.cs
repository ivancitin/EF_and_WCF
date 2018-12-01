using System.ServiceModel;
using WCF.Service;

namespace WCF.Server
{
    public class ServiceHostIPCServer<S> : IPCServer<S, int> where S : IIPCService
    {
        public S Service
        {
            get; private set;
        }

        readonly object semaphore = new object();
        readonly ServiceHost serviceHost;

        public ServiceHostIPCServer(S singletonInstance)
        {
            Service = singletonInstance;
            serviceHost = new ServiceHost(singletonInstance);
        }

        public int Start()
        {
            lock (semaphore)
            {
                int port = Constants.PORT;
                string serviceAddress = Constants.GetServiceAddress(port);

                // Start new server host
                serviceHost.AddServiceEndpoint(typeof(S), new NetTcpBinding(), serviceAddress);
                serviceHost.Open();

                return port;
            }
        }

        public void Dispose()
        {
            lock (semaphore)
            {
                if (serviceHost.State != CommunicationState.Closed)
                {
                    serviceHost.Close();
                }
            }
        }
    }
}
