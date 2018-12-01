using System;
using System.ServiceModel;
using WCF.Service;

namespace WCF.Client
{
    public static class IPCClientFactory
    {
        static readonly TimeSpan DEFAULT_OPERATION_TIMEOUT = TimeSpan.FromSeconds(10);
        static readonly NetTcpBinding DEFAULT_BINDING = CreateNetTcpBinding(DEFAULT_OPERATION_TIMEOUT);

        public static S Instantiate<S>(int port) where S : IIPCService
        {
            string address = Constants.GetServiceAddress(port);

            return ChannelFactory<S>.CreateChannel(DEFAULT_BINDING, new EndpointAddress(address));
        }

        static NetTcpBinding CreateNetTcpBinding(TimeSpan sendTimeout)
        {
            return new NetTcpBinding()
            {
                OpenTimeout = TimeSpan.FromSeconds(1),
                CloseTimeout = TimeSpan.FromSeconds(1),
                SendTimeout = sendTimeout
            };
        }
    }
}
