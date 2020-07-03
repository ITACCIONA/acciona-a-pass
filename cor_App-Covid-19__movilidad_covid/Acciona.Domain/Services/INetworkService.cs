

using Acciona.Domain.Model;

namespace Acciona.Domain.Services
{
    public interface INetworkService
    {
        NetworkType GetConnectivityStatus();
        bool IsConnected();
    }
}