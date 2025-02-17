﻿using Indicon.Api.IpManager.Forms;
using System.Diagnostics;
using System.Net.NetworkInformation;
using Engine;
using Engine.Notify;
using Indicon.Api.IpManager.Events;

namespace Indicon.Api.IpManager.Classes
{
    internal static class StaticIpManager
    {
        private struct NotifyMessages
        {
            public NotifyMessages() { }
            internal static readonly ErrorCode NoFileFound = new(10_000, "No Save File Found", "No save file was found for this tool. A new save file is being created at the target directory!");
            internal static readonly ErrorCode NoPingAdapter = new(10_000, "Cannot ping network!", "No valid IP address associated with selected Network Adapter. Please verify network is enabled and cable is plugged in!");
        }
        private struct ErrorCodes
        {
            public ErrorCodes() { }
            internal static readonly ErrorCode AppFault = new(1_000, "Application Directory Fault", "Application Directory Could Not Be Found Or Created!");
            internal static readonly ErrorCode ToolDirFault = new(1_001, "Application Tool Directory Fault", "Application Tool Directory Could Not Be Found Or Created!");
            internal static readonly ErrorCode NullFormFault = new ErrorCode(1_002, "Null Form Fault", "Null Form Was Passed To Ip Manager, Cannot Continue!");
        }
        public static List<Ipv4Address>? Ips { get; private set; } = new();
        public static string? AppDirPath { get; private set; }
        public static string? ThisToolPath { get; private set; }
        private static string FilePath { get { return Path.Combine(ThisToolPath, FILE_NAME) + FILE_EXT; } }
        private const string APP_NAME = "IndiconStudio";
        private const string THIS_TOOL = "IpManager";
        private const string FILE_NAME = "IpMetaData";
        private const string FILE_EXT = ".bin";
        private static IpManagerForm? BoundForm;
        private static Engine.Tool.Tool Tool;

        public static void Init(ref IpManagerForm oForm)
        {
            /// Bind form
            if(oForm != null)
            {
                BoundForm = oForm;
            }
            else
            {
                NotifyHandler.FatalError(ErrorCodes.NullFormFault);
            }
            /// This is a test - we will continue if the test goes well?
            StudioEngine.Init(THIS_TOOL);
            /// Validate directories are ok
            CheckDirs();
            /// Load from file (if exists) else we will recompile whatever is needed
            Load();
            /// Initialize Xml Vendor Static Class
            XmlReader.ReadVendorFile(string.Empty);
            /// After a load (successful or not) update the GUI
            oForm.SetIpList(Ips.ToArray());
        }
        public static void CreateNewIp(object sender, IpAddressCommitEventArgs oArgs)
        {
            Ipv4Address oIpAddress = new();
            oIpAddress.SetName(oArgs.Name);
            oIpAddress.SetIpAddress(oArgs.IpAddress);
            oIpAddress.SetSubnetMask(oArgs.SubnetMask);
            oIpAddress.SetGateway(oArgs.Gateway);
            oIpAddress.NetworkInterfaceName = oArgs.Adapter?.Name;
            oIpAddress.NetworkInterfaceMAC = oArgs.Adapter?.GetPhysicalAddress().ToString();
            Ipv4Address? oExistingIP = Ips?.Find(x => x.Equals(oIpAddress));
            if(oExistingIP != null)
            {
                Ips?.Remove(oExistingIP);
            }
            Ips?.Add(oIpAddress);
            Ips?.Sort();
            UpdateFormIpList();
            Save();
        }
        public static void DeleteEntry(Ipv4Address oAddress)
        {
            Ips?.Remove(oAddress);
            Save();
        }
        public static void SetIpScheme(Ipv4Address oAddress)
        {
            if (oAddress.NetworkInterfaceMAC == null)
            {
                return;
            }
            string[] sIP = new string[1] { oAddress.IpAddressString };
            string[] sSubnet = new string[1] { oAddress.SubnetMaskString };
            string[] sGateway = new string[1] { oAddress.GatewayString };
            (uint, uint, uint) oResponse = NetworkManager.SetNICStatic(oAddress.NetworkInterfaceMAC, sIP, sSubnet, sGateway, string.Empty);
            if (oResponse.Item1 == 0 && oResponse.Item2 == 0 && oResponse.Item3 == 0)
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
        public static void PingAdapterNetwork(NetworkInterface oInterface)
        {
            
            (string sIP, string sSubnet) = NetworkManager.GetNetworkInterfaceIpAddress(oInterface.GetPhysicalAddress().ToString());
            if (sIP == null || sSubnet == null)
            {
                NotifyHandler.GeneralNotification(NotifyMessages.NoPingAdapter);
                return;
            }
            NetworkManager.PingUpdated += new EventHandler<NetworkManager.PingResultEventArgs>(OnPingUpdated);
            NetworkManager.PingNetwork(sIP, sSubnet);
            NetworkManager.PingUpdated -= new EventHandler<NetworkManager.PingResultEventArgs>(OnPingUpdated);
        }
        public static void OpenNetworkConnectionsPanel()
        {
            ProcessStartInfo startInfo = new("NCPA.cpl") { UseShellExecute = true };
            Process.Start(startInfo);
        }
        private static void OnPingUpdated(object sender, NetworkManager.PingResultEventArgs e)
        {
            if(e.Address != null)
            {
                BoundForm?.AddPingResult(e.Address);
            }
        }
        public static void SetStartup(bool bEnable)
        {
            NativeWindowsFunctions.SetStartup(THIS_TOOL, bEnable);
        }
        public static bool GetStartup()
        {
            return NativeWindowsFunctions.StartupStatus(THIS_TOOL);
        }
        private static void CheckDirs()
        {
            /// Add init functions here as required
            AppDirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), APP_NAME);
            /// Check for Indicon Studio folder
            if (!Directory.Exists(AppDirPath))
            {
                try
                {
                    Directory.CreateDirectory(AppDirPath);
                }
                catch(Exception)
                {
                    NotifyHandler.FatalError(ErrorCodes.AppFault);
                }
                
            }
            /// Check for folder for this tool
            ThisToolPath = Path.Combine(AppDirPath, THIS_TOOL);
            if (!Directory.Exists(ThisToolPath))
            {
                try
                {
                    Directory.CreateDirectory(ThisToolPath);
                }
                catch(Exception)
                {
                    NotifyHandler.FatalError(ErrorCodes.ToolDirFault);
                }
                
            }
        }
        private static void UpdateFormIpList()
        {
            BoundForm?.SetIpList(Ips.ToArray());
        }
        private static void Save()
        {
            Ipv4Address[] oAddress = new Ipv4Address[Ips.Count];
            Ips.CopyTo(oAddress, 0);
            BinarySerialization.WriteToBinaryFile<Ipv4Address[]>(FilePath, oAddress, false);
        }
        private static void Load()
        {
            /// Recreate list (this empties out the list)
            Ips = new List<Ipv4Address>();
            try
            {
                /// Read binary serialization file locally into array
                Ipv4Address[] oAddress = BinarySerialization.ReadFromBinaryFile<Ipv4Address[]>(FilePath);
                /// Buffer the array into our newly created list
                for (int i = 0; i < oAddress.Length; i++)
                {
                    Ips.Add(oAddress[i]);
                }
                /// Sort for prettiness
                Ips.Sort();
            }
            /// No file found, notify user, create, return
            catch (FileNotFoundException)
            {
                NotifyHandler.GeneralNotification(NotifyMessages.NoFileFound);
            }
            
        }
    }
}
