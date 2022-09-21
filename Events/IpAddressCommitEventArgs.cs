using System.Net.NetworkInformation;

namespace Indicon.Api.IpManager.Events
{
    internal class IpAddressCommitEventArgs : EventArgs
    {
        public string Name = String.Empty;
        public string[] IpAddress = Array.Empty<string>();
        public string[] SubnetMask = Array.Empty<string>();
        public string[] Gateway = Array.Empty<string>();
        public NetworkInterface? Adapter = null;
    }
}
