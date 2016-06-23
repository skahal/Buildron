namespace Buildron.Domain
{
    public interface ICIServerService
    {
        CIServer GetCIServer();

        void SaveCIServer(CIServer server);
    }
}