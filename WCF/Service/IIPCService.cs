using System.Net.Security;
using System.ServiceModel;

namespace WCF.Service
{
    [ServiceContract(
        Namespace = "WCF.Service.IPC",
        Name = "ExitApp Communication Service",
        ProtectionLevel = ProtectionLevel.EncryptAndSign
    )]
    public interface IIPCService
    {
        IIPCServiceProviders Providers
        {
            get;
        }

        [OperationContract]
        void ExitApp();
    }
}
