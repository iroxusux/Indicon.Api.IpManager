using System.Management;
using System.Net.NetworkInformation;
using System.Diagnostics;

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
        private static ManagementObject GetnetworkInterfaceManagementObject(string sMAC)
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
                ManagementObject oManager = GetnetworkInterfaceManagementObject(sMAC);
                if(oManager == null)
                {
                    return false;
                }
                /// Set IP Protocol Address
                ManagementBaseObject oIP = oManager.GetMethodParameters("EnableStatic");
                oIP["IPAddress"] = sIP;
                oIP["SubnetMask"] = sSubnet;
                var oIPResults = oManager.InvokeMethod("EnableStatic", oIP, null)["ReturnValue"];
                /// Set Gateway
                ManagementBaseObject oGateway = oManager.GetMethodParameters("SetGateways");
                if (sGateway[0] == "0.0.0.0")
                {
                    oGateway["DefaultIPGateway"] = oIP["IPAddress"];
                    var oNewResults = oManager.InvokeMethod("SetGateways", oGateway, null)["ReturnValue"];
                }
                else
                {
                    oGateway["DefaultIPGateway"] = sGateway;
                    oGateway["GatewayCostMetric"] = new int[] { 1 };
                }
                var oGatewayResults = oManager.InvokeMethod("SetGateways", oGateway, null)["ReturnValue"];
                /// Set DNS
                if(sDNS != String.Empty)
                {
                    ManagementBaseObject oDNS = oManager.GetMethodParameters("SetDNSServerSearchOrder");
                    oDNS["DNSServerSearchOrder"] = new string[] { sDNS };
                    oManager.InvokeMethod("SetDNSServerSearchOrder", oDNS, null);
                }
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
                ManagementObject oManager = GetnetworkInterfaceManagementObject(sMAC);
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
    }
}
