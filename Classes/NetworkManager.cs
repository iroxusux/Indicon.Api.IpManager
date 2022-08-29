using System.Management;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Net;

namespace Indicon.Api.IpManager.Classes
{
    internal static class NetworkManager
    {
        /// <summary>
        /// Public Event Handler
        /// Notify Any Calling Object That A Ping Result Has Been Published
        /// </summary>
        public static event EventHandler<PingResultEventArgs> PingUpdated = delegate { };
        /// <summary>
        /// Static Delegated Method
        /// Asyncronously Call Delegate Methods On Ping Results Complete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnPingUpdated(object sender, PingResultEventArgs e)
        {
            EventHandler<PingResultEventArgs> oHandler = PingUpdated;
            oHandler?.Invoke(sender, e);
        }
        /// <summary>
        /// Get Network Interface By MAC Of NIC
        /// </summary>
        /// <param name="sMAC"></param>
        /// <returns></returns>
        private static NetworkInterface? GetNetworkInterface(string sMAC)
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
        /// <summary>
        /// Get Management Object Of NIC
        /// </summary>
        /// <param name="sMAC"></param>
        /// <returns></returns>
        private static ManagementObject? GetNetworkInterfaceManagementObject(string sMAC)
        {
            NetworkInterface? oNetwork = GetNetworkInterface(sMAC);
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
        /// <summary>
        /// Set IP Address (Ipv4) Properties Of NIC To Static Values Passed
        /// </summary>
        /// <param name="sMAC"></param>
        /// <param name="sIP"></param>
        /// <param name="sSubnet"></param>
        /// <param name="sGateway"></param>
        /// <param name="sDNS"></param>
        /// <returns></returns>
        public static (uint, uint, uint) SetNICStatic(string sMAC, string[] sIP, string[] sSubnet, string[] sGateway, string sDNS)
        {
            try
            {
                ManagementObject? oManager = GetNetworkInterfaceManagementObject(sMAC);
                if(oManager == null)
                {
                    return (997, 997, 997);
                }
                if (!IsNetworkEnabled(oManager))
                {
                    MessageBox.Show("This adapter cannot be configured!\n'IP NOT ENABLED'\nPlease plug a cable into the adapter or enable IP!", "Cannot update adapter!");
                    return (998, 998, 998);
                }
                /// Set IP Address To NIC Card
                uint iIpRetVal = SetNetworkInterfaceIpAddress(oManager, sIP, sSubnet);
                /// Set Gateway Address To NIC Card
                uint iGatewayRetVal = SetNetworkInterfaceGateway(oManager, sIP, sGateway);
                /// Set DNS To NIC Card - NOT USED
                uint iDnsRetVal = SetNetworkInterfaceDNS(oManager, sDNS);
                return (iIpRetVal, iGatewayRetVal, iDnsRetVal);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return (999, 999, 999);
            }
        }
        /// <summary>
        /// Set IP Address (Ipv4) Properties Of NIC To DHCP
        /// </summary>
        /// <param name="sMAC"></param>
        /// <returns></returns>
        public static (uint, uint) SetNICDHCP(string sMAC)
        {
            try
            {
                ManagementObject? oManager = GetNetworkInterfaceManagementObject(sMAC);
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
        /// <summary>
        /// Validate MAC Octet - If Length Is Oversize, Return Empty String
        /// If Length Is Too Small, Add Length And Return
        /// </summary>
        /// <param name="sMacOctet"></param>
        /// <returns></returns>
        public static string ValidateMacOctet(string sMacOctet)
        {
            if(sMacOctet == null || sMacOctet.Length > 2)
            {
                return string.Empty;
            }
            if(sMacOctet.Length < 2)
            {
                return new string($"0{sMacOctet}");
            }
            return sMacOctet;
        }
        /// <summary>
        /// Common Function
        /// Set IP Address Of NIC
        /// </summary>
        /// <param name="oManager"></param>
        /// <param name="sIP"></param>
        /// <param name="sSubnet"></param>
        /// <returns></returns>
        private static uint SetNetworkInterfaceIpAddress(ManagementObject oManager, string[] sIP, string[] sSubnet)
        {
            /// Set IP Protocol Address
            ManagementBaseObject oIP = oManager.GetMethodParameters("EnableStatic");
            oIP["IPAddress"] = sIP;
            oIP["SubnetMask"] = sSubnet;
            return (uint)oManager.InvokeMethod("EnableStatic", oIP, null)["ReturnValue"];
        }
        /// <summary>
        /// Common Function
        /// Set Gateway Of NIC
        /// </summary>
        /// <param name="oManager"></param>
        /// <param name="sIP"></param>
        /// <param name="sGateway"></param>
        /// <returns></returns>
        private static uint SetNetworkInterfaceGateway(ManagementObject oManager, string[] sIP, string[] sGateway)
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
        /// <summary>
        /// Common Function
        /// Set DNS Of NIC
        /// (Unused At This Time)
        /// </summary>
        /// <param name="oManager"></param>
        /// <param name="sDNS"></param>
        /// <returns></returns>
        private static uint SetNetworkInterfaceDNS(ManagementObject oManager, string sDNS)
        {
            return 0;  /// This method is not used nor has it been debugged / tested - do not modify DNS as of now
            ManagementBaseObject oDNS = oManager.GetMethodParameters("SetDNSServerSearchOrder");
            oDNS["DNSServerSearchOrder"] = new string[] { sDNS };
            return (uint)oManager.InvokeMethod("SetDNSServerSearchOrder", oDNS, null)["ReturnValue"];
        }
        /// <summary>
        /// Retreive Ipv4 Address Of NIC Card
        /// </summary>
        /// <param name="sMAC"></param>
        /// <returns></returns>
        public static (string, string) GetNetworkInterfaceIpAddress(string sMAC)
        {
            ManagementObject oManager = GetNetworkInterfaceManagementObject(sMAC);
            return GetNetworkInterfaceIpAddress(oManager);
            
        }
        /// <summary>
        /// Retreive Ipv4 Address Of NIC Card
        /// </summary>
        /// <param name="oManager"></param>
        /// <returns></returns>
        public static (string, string) GetNetworkInterfaceIpAddress(ManagementObject oManager)
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
        /// <summary>
        /// With A Provided IP & Subnet, Ping A Network For Devices
        /// </summary>
        /// <param name="sIP"></param>
        /// <param name="sSubnet"></param>
        public static void PingNetwork(string sIP, string sSubnet)
        {
            NetworkScanner.TryPingNetwork(sIP, sSubnet);
        }
        /// <summary>
        /// Helper Function - Check If NIC Network Is Enabled
        /// </summary>
        /// <param name="oManager"></param>
        /// <returns></returns>
        public static bool IsNetworkEnabled(ManagementObject oManager)
        {
            if(oManager == null)
            {
                return false;
            }
            return (bool)oManager["IPEnabled"];
        }        
        /// <summary>
        /// Private Class To Manage Scanning Of Network For Pings
        /// </summary>
        private static class NetworkScanner
        {
            private static List<Pinger> Pingers = new();
            private static int Instances = 0;
            private static object @lock = new();
            private static int TimeOut = 250;
            private const int OCTET_COUNT = 4;
            private static bool Busy = false;

            /// <summary>
            /// Attempt To Ping Provided Network
            /// </summary>
            /// <param name="sIP"></param>
            /// <param name="sSubnet"></param>
            internal static void TryPingNetwork(string sIP, string sSubnet)
            {
                if (Busy)
                {
                    return;
                }
                Busy = true;
                Pingers = new List<Pinger>();
                CreatePings(GetPingableAddressesFromNetwork(sIP, sSubnet));
                Instances = Pingers.Count;
                foreach(Pinger oPinger in Pingers)
                {
                    oPinger.Ping.SendAsync(oPinger.NetworkAddress, TimeOut, new object());
                }
            }
            /// <summary>
            /// Given An Ipv4 Addrss And Subnet - Calculate Available Addresses To Attempt To Ping
            /// </summary>
            /// <param name="sIP"></param>
            /// <param name="sSubnet"></param>
            /// <returns></returns>
            private static string[] GetPingableAddressesFromNetwork(string sIP, string sSubnet)
            {
                string[] sIpOctets = sIP.Split(".");
                string[] sSubnetOctets = sSubnet.Split(".");
                int[] iIpOctets = new int[OCTET_COUNT];
                int[] iSubnetOctets = new int[OCTET_COUNT];
                List<string> oObserableIps = new();
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
            /// <summary>
            /// Compile Array Of Addresses Into Pingers
            /// </summary>
            /// <param name="sIPAddresses"></param>
            private static void CreatePings(string[] sIPAddresses)
            {
                for (int i = 0; i < sIPAddresses.Length; i++)
                {
                    Pinger oPinger = new() { Ping = new Ping(), NetworkAddress = sIPAddresses[i] };
                    oPinger.Ping.PingCompleted += new PingCompletedEventHandler(OnPingComplete);
                    Pingers.Add(oPinger);
                }
            }
            /// <summary>
            /// Unbind Delegates And Remove Pinger From List
            /// </summary>
            private static void DestroyPings()
            {
                foreach(Pinger oPinger in Pingers)
                {
                    oPinger.Ping.PingCompleted -= new PingCompletedEventHandler(OnPingComplete);
                    oPinger.Ping.Dispose();
                }
                Pingers.Clear();
            }
            private static void Finish()
            {
                Busy = false;
            }
            /// <summary>
            /// Thread Target Function
            /// Buffer Ping Replies If Successful, Manage Instances
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="oArgs"></param>
            private static void OnPingComplete(object sender, PingCompletedEventArgs oArgs)
            {
                lock (@lock)
                {
                    Instances -= 1;
                    if (oArgs.Reply?.Status == IPStatus.Success)
                    {
                        OnPingUpdated(null, new PingResultEventArgs() { Address = oArgs.Reply.Address});
                    }
                    if (Instances == 0)
                    {
                        DestroyPings();
                        Finish();
                    }
                }  
            }
        }
        /// <summary>
        /// Helper Struct
        /// </summary>
        private struct Pinger
        {
            public Ping Ping { get; set; }
            public string NetworkAddress { get; set; }
        }
        /// <summary>
        /// Event Class For Asnycronous Publishing Of Ping Results
        /// </summary>
        public class PingResultEventArgs : EventArgs
        {
            public IPAddress Address { get; set; }
        }
    }
}
