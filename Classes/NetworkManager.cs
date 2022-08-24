    using System.Management;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Net;

namespace Indicon.Api.IpManager.Classes
{
    internal static class NetworkManager
    {
        private static NetworkInterface GetNetworkInterface(string sMAC)
        {
            foreach(NetworkInterface oNetwork in NetworkInterface.GetAllNetworkInterfaces())
            {
                if(sMAC == oNetwork.GetPhysicalAddress().ToString())
                {
                    return oNetwork;
                }
            }
            return null;
        }
        private static ManagementObject GetNetworkInterfaceManagementObject(string sMAC)
        {
            NetworkInterface oNetwork = GetNetworkInterface(sMAC);
            if(oNetwork == null)
            {
                return null;
            }
            ManagementClass oManagementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection oCollection = oManagementClass.GetInstances();
            foreach(ManagementObject oInstance in oCollection)
            {
                if (oInstance["settingID"].ToString() == oNetwork.Id)
                {
                    return oInstance;
                }
            }
            return null;
        }
        public static bool SetNICStatic(string sMAC, string[] sIP, string[] sSubnet, string[] sGateway, string sDNS)
        {
            try
            {
                ManagementObject oManager = GetNetworkInterfaceManagementObject(sMAC);
                if(oManager == null)
                {
                    return false;
                }
                if (!IsNetworkEnabled(oManager))
                {
                    MessageBox.Show("This adapter cannot be configured!\n'IP NOT ENABLED'\nPlease plug a cable into the adapter or enable IP!", "Cannot update adapter!");
                    return false;
                }
                /// Set IP Address To NIC Card
                uint iIpRetVal = SetNetworkInterfaceCardIpAddress(oManager, sIP, sSubnet);
                /// Set Gateway Address To NIC Card
                uint iGatewayRetVal = SetNetworkInterfaceCardGateway(oManager, sIP, sGateway);
                /// Set DNS To NIC Card - NOT USED
                uint iDnsRetVal = SetNetworkInterfaceCardDNS(oManager, sDNS);
                return true;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
        public static (uint, uint) SetNICDHCP(string sMAC)
        {
            try
            {
                ManagementObject oManager = GetNetworkInterfaceManagementObject(sMAC);
                if (oManager == null)
                {
                    Debug.WriteLine("No Management Object Found For NIC Card");
                    return (997, 997);
                }
                /// Disable DNS
                var oDNS = oManager.GetMethodParameters("SetDNSServerSearchOrder");
                oDNS["DNSServerSearchOrder"] = null;
                var oEnableDHCP = oManager.InvokeMethod("EnableDHCP", null, null)["ReturnValue"];
                var oSetDNS = oManager.InvokeMethod("SetDNSServerSearchOrder", oDNS, null)["ReturnValue"];
                try
                {
                    return ((uint)oEnableDHCP, (uint)oSetDNS);
                }
                catch (InvalidCastException)
                {
                    return (998, 998);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return (999, 999);
            }
        }
        public static string ValidateMacOctet(string sMacOctet)
        {
            if(sMacOctet == null || sMacOctet.Length > 3)
            {
                return string.Empty;
            }
            if(sMacOctet.Length < 2)
            {
                return new string($"0{sMacOctet}");
            }
            return sMacOctet;
        }
        private static uint SetNetworkInterfaceCardIpAddress(ManagementObject oManager, string[] sIP, string[] sSubnet)
        {
            /// Set IP Protocol Address
            ManagementBaseObject oIP = oManager.GetMethodParameters("EnableStatic");
            oIP["IPAddress"] = sIP;
            oIP["SubnetMask"] = sSubnet;
            return (uint)oManager.InvokeMethod("EnableStatic", oIP, null)["ReturnValue"];
        }
        private static uint SetNetworkInterfaceCardGateway(ManagementObject oManager, string[] sIP, string[] sGateway)
        {
            /// Set Gateway
            ManagementBaseObject oGateway = oManager.GetMethodParameters("SetGateways");
            if (sGateway[0] == "0.0.0.0")
            {
                oGateway["DefaultIPGateway"] = sIP;
            }
            else
            {
                oGateway["DefaultIPGateway"] = sGateway;
                oGateway["GatewayCostMetric"] = new int[] { 1 };
            }
            return (uint)oManager.InvokeMethod("SetGateways", oGateway, null)["ReturnValue"];
        }
        private static uint SetNetworkInterfaceCardDNS(ManagementObject oManager, string sDNS)
        {
            return 0;  /// This method is not used nor has it been debugged / tested - do not modify DNS as of now
            ManagementBaseObject oDNS = oManager.GetMethodParameters("SetDNSServerSearchOrder");
            oDNS["DNSServerSearchOrder"] = new string[] { sDNS };
            return (uint)oManager.InvokeMethod("SetDNSServerSearchOrder", oDNS, null)["ReturnValue"];
        }
        public static (string, string) GetNetworkInterfaceCardIpAddress(string sMAC)
        {
            ManagementObject oManager = GetNetworkInterfaceManagementObject(sMAC);
            return GetNetworkInterfaceCardIpAddress(oManager);
            
        }
        public static (string, string) GetNetworkInterfaceCardIpAddress(ManagementObject oManager)
        {
            if(oManager == null)
            {
                return (null, null);
            }
            string[] IPInformation = (string[])oManager["IPAddress"];
            string[] SubnetInformation = (string[])oManager["IPSubnet"];
            if (IPInformation == null || IPInformation.Length == 0 || SubnetInformation == null || SubnetInformation.Length == 0)
            {
                return (null, null);
            }
            return (IPInformation[0], SubnetInformation[0]);
        }
        public static bool PingNetwork(string sIP, string sSubnet)
        {
            NetworkScanner.PingNetwork(sIP, sSubnet);
            return true;
        }
        public static bool IsNetworkEnabled(ManagementObject oManager)
        {
            if(oManager == null)
            {
                return false;
            }
            return (bool)oManager["IPEnabled"];
        }        
        private static class NetworkScanner
        {
            private static List<Pinger> Pingers = new List<Pinger>();
            private static List<IPAddress> SuccessfulAddresses = new List<IPAddress>();
            private static int Instances = 0;
            private static object @lock = new object();
            private static int TimeOut = 250;
            private const int OCTET_COUNT = 4;
            private static bool Busy = false;

            internal static void PingNetwork(string sIP, string sSubnet)
            {
                if (Busy)
                {
                    return;
                }
                Busy = true;
                Pingers = new List<Pinger>();
                SuccessfulAddresses = new List<IPAddress>();
                CreatePings(GetPingableAddressesFromNetwork(sIP, sSubnet));
                Instances = Pingers.Count;
                foreach(Pinger oPinger in Pingers)
                {
                    oPinger.Ping.SendAsync(oPinger.NetworkAddress, TimeOut, new object());
                }
            }
            private static string[] GetPingableAddressesFromNetwork(string sIP, string sSubnet)
            {
                string[] sIpOctets = sIP.Split(".");
                string[] sSubnetOctets = sSubnet.Split(".");
                int[] iIpOctets = new int[OCTET_COUNT];
                int[] iSubnetOctets = new int[OCTET_COUNT];
                List<string> oObserableIps = new List<string>();
                if (sIpOctets.Length != OCTET_COUNT || sSubnetOctets.Length != OCTET_COUNT)
                {
                    return Array.Empty<string>();
                }
                try
                {
                    for(int i = 0; i < OCTET_COUNT; i++)
                    {
                        iIpOctets[i] = Convert.ToInt32(sIpOctets[i]);
                        iSubnetOctets[i] = Convert.ToInt32(sSubnetOctets[i]);
                    }
                    for(int i = 0; i < OCTET_COUNT; i++)
                    {
                        if (iSubnetOctets[i] != 255)
                        {
                            for(int j = 1; j < 254; j++)
                            {
                                oObserableIps.Add($"{(i == 0 ? j : iIpOctets[0])}.{(i == 1 ? j : iIpOctets[1])}.{(i == 2 ? j : iIpOctets[2])}.{(i == 3 ? j : iIpOctets[3])}");
                            }
                        }
                    }
                    return oObserableIps.ToArray();
                }
                catch (InvalidCastException)
                {
                    return Array.Empty<string>();
                }
            }
            private static void CreatePings(string[] sIPAddresses)
            {
                for (int i = 0; i < sIPAddresses.Length; i++)
                {
                    Pinger oPinger = new Pinger { Ping = new Ping(), NetworkAddress = sIPAddresses[i] };
                    oPinger.Ping.PingCompleted += new PingCompletedEventHandler(OnPingComplete);
                    Pingers.Add(oPinger);
                }
            }
            private static void DestroyPings()
            {
                foreach(Pinger oPinger in Pingers)
                {
                    oPinger.Ping.PingCompleted -= new PingCompletedEventHandler(OnPingComplete);
                    oPinger.Ping.Dispose();
                }
                Pingers.Clear();
            }
            private static void PublishResults()
            {
                StaticIpManager.PulishNetworkPingResults(SuccessfulAddresses.ToArray());
                Busy = false;
            }
            private static void OnPingComplete(object sender, PingCompletedEventArgs oArgs)
            {
                lock (@lock)
                {
                    Instances -= 1;
                    if (oArgs.Reply.Status == IPStatus.Success)
                    {
                        SuccessfulAddresses.Add(oArgs.Reply.Address);
                    }
                    if (Instances == 0)
                    {
                        PublishResults();
                        DestroyPings();
                    }
                }  
            }
        }
        private struct Pinger
        {
            public Ping Ping { get; set; }
            public string NetworkAddress { get; set; }
        }
    }
}
