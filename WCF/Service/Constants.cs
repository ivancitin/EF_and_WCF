namespace WCF.Service
{
    public static class Constants
    {
        const string SERVICE_URI_PATTERN = "net.tcp://localhost:{0}/{1}";
        const string SERVICE_PATH = "ExitAppService";
        public const int PORT = 8989;

        public static string GetServiceAddress(int portNumber)
        {
            return string.Format(SERVICE_URI_PATTERN, portNumber, SERVICE_PATH);
        }
    }
}
