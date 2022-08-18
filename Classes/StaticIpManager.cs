using Indicon.Api.IpManager.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicon.Api.IpManager.Classes
{
    internal static class StaticIpManager
    {
        public static List<Ipv4Address>? Ips { get; private set; } = new();
        public static string AppDirPath { get; private set; }
        public static string ThisToolPath { get; private set; }
        private static string FilePath { get { return Path.Combine(ThisToolPath, FILE_NAME) + FILE_EXT; } }
        private const string APP_NAME = "IndiconStudio";
        private const string THIS_TOOL = "IpManager";
        private const string FILE_NAME = "IpMetaData";
        private const string FILE_EXT = ".bin";
        private static IpManagerForm BoundForm;
        public static void Init(ref IpManagerForm oForm)
        {
            /// Add init functions here as required
            AppDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), APP_NAME);
            /// Check for Indicon Studio folder
            if (!Directory.Exists(AppDirPath))
            {
                Directory.CreateDirectory(AppDirPath);
            }
            ThisToolPath = Path.Combine(AppDirPath, THIS_TOOL);
            if(!Directory.Exists(ThisToolPath))
            {
                Directory.CreateDirectory(ThisToolPath);
            }
            BoundForm = oForm;
            Load();
            oForm.UpdateIpList(Ips.ToArray());
        }
        public static void CreateNewIp(object sender, IpAddressCommitEventArgs oArgs)
        {
            Ipv4Address oIpAddress = new Ipv4Address();
            oIpAddress.SetName(oArgs.Name);
            oIpAddress.SetIpAddress(oArgs.IpAddress);
            oIpAddress.SetSubnetMask(oArgs.SubnetMask);
            oIpAddress.SetGateway(oArgs.Gateway);
            oIpAddress.NetworkInterfaceName = oArgs.Adapter?.Name;
            oIpAddress.NetworkInterfaceMAC = oArgs.Adapter?.GetPhysicalAddress().ToString();
            Ipv4Address oExistingIP = Ips.Find(x => x.Equals(oIpAddress));
            if(oExistingIP != null)
            {
                Ips.Remove(oExistingIP);
            }
            Ips.Add(oIpAddress);
            UpdateFormIpList();
            Save();
        }
        public static void DeleteEntry(Ipv4Address oAddress)
        {
            Ips.Remove(oAddress);
            Save();
        }
        public static void SetIpScheme(Ipv4Address oAddress)
        {
            string[] sIP = new string[1] { oAddress.IpAddressString };
            string[] sSubnet = new string[1] { oAddress.SubnetMaskString };
            string[] sGateway = new string[1] { oAddress.GatewayString };
            bool bSuccess = NetworkManager.SetNICStatic(oAddress.NetworkInterfaceMAC, sIP, sSubnet, sGateway, string.Empty);
            if (bSuccess)
            {
                MessageBox.Show("Success!", "Set Static");
            }
            else
            {
                MessageBox.Show("Failure!", "Set Static");
            }
        }
        public static void SetDHCPScheme(string sMAC)
        {
            (uint, uint) oResponse = NetworkManager.SetNICDHCP(sMAC);
            if (oResponse.Item1 == 0 && oResponse.Item2 == 0)
            {
                MessageBox.Show("Success!", "Set DHCP");
            }
            else
            {
                MessageBox.Show("Failure!", "Set DHCP");
            }
        }
        public static void OpenNetworkConnectionsPanel()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("NCPA.cpl");
            startInfo.UseShellExecute = true;
            Process.Start(startInfo);
        }
        private static void UpdateFormIpList()
        {
            BoundForm.UpdateIpList(Ips.ToArray());
        }
        private static void Save()
        {
            Ipv4Address[] oAddress = new Ipv4Address[Ips.Count];
            Ips.CopyTo(oAddress, 0);
            BinarySerialization.WriteToBinaryFile<Ipv4Address[]>(FilePath, oAddress, false);
        }
        private static void Load()
        {
            Ips = new List<Ipv4Address>();
            try
            {
                Ipv4Address[] oAddress = BinarySerialization.ReadFromBinaryFile<Ipv4Address[]>(FilePath);
                for (int i = 0; i < oAddress.Length; i++)
                {
                    Ips.Add(oAddress[i]);
                }
            }
            catch (FileNotFoundException)
            {
                return;
            }
            
        }
    }
}
