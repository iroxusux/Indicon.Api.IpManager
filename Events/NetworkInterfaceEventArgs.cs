using System.Net.NetworkInformation;

namespace Indicon.Api.IpManager.Events
{
    internal class NetworkInterfaceEventArgs : EventArgs
    {
        public NetworkInterface? Interface = null;
    }
}
