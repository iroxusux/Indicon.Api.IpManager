using Indicon.Api.IpManager.Notify;
using Engine.Tool;
using Indicon.Api.IpManager.Forms;
using System.Xml;
using System.Net.NetworkInformation;
using Indicon.Api.IpManager.Events;
using Engine.Forms.Classes;

namespace Indicon.Api.IpManager.Classes
{
    internal class NewIpManager : Tool
    {
        private const bool USE_NETSH = true;
        private const string TOOL_NAME = "Ip Manager";
        private IpManagerForm? Form;
        private List<Ipv4Address> _Ips { get; set; } = new();
        private List<VendorOUI> _VendorMacRelations { get; set; } = new();
        private string _IpListFilePath { get { return Path.Combine(DirectoryPath, "IpMetaData.bin"); } }
        protected override void Load()
        {
            _Ips = new();
            try
            {
                Ipv4Address[]? oAddresses = Engine.IO.BinarySerialization.ReadFromBinaryFile<Ipv4Address[]>(_IpListFilePath);
                if (oAddresses == null) return;
                for (int i = 0; i < oAddresses.Length; i++)
                {
                    _Ips.Add(oAddresses[i]);
                }
                _Ips.Sort();
            }
            catch (FileNotFoundException)
            {
                Engine.Notify.NotifyHandler.General(CodeNotify.FileNotFound);
            }
        }
        protected override void Save()
        {
            Ipv4Address[] oAddresses = new Ipv4Address[_Ips.Count];
            _Ips.CopyTo(oAddresses, 0);
            Engine.IO.BinarySerialization.WriteToBinaryFile(_IpListFilePath, oAddresses);
        }
        public override void Init()
        {
            ToolName = TOOL_NAME;
            if (!Initialized)
            {
                base.Init();
                ReadVendorFile();
            }
            if(Form == null)
            {
                return;
            }
            ClearFormIpList();
            ClearFormNetworkAdapters();
            SetFormIpList();
            SetFormNetworkAdapters();
        }
        public void BindToForm(ref IpManagerForm oForm)
        {
            if (oForm == null)
            {
                Engine.Notify.NotifyHandler.Fatal(CodeFatal.NullFormError);
                return;
            }
            Form = oForm;
            Form.IpSchemeCommit += new EventHandler<IpAddressCommitEventArgs>(CreateNewIpScheme);
            Form.IpSchemeDelete += new EventHandler<Ipv4AddressEventArgs>(DeleteIpScheme);
            Form.NetworkInterfaceApplyStatic += new EventHandler<Ipv4AddressEventArgs>(ApplyStaticNetworkScheme);
            Form.NetworkInterfaceApplyDhcp += new EventHandler<NetworkInterfaceEventArgs>(ApplyDynamicNetworkScheme);
            Form.NetworkInterfacePing += new EventHandler<NetworkInterfaceEventArgs>(PingNetworkInterface);
        }
        private void CreateNewIpScheme(object? oSender, IpAddressCommitEventArgs oArgs)
        {
            Ipv4Address oAddress = new();
            oAddress.SetName(oArgs.Name);
            oAddress.SetIpAddress(oArgs.IpAddress);
            oAddress.SetSubnetMask(oArgs.SubnetMask);
            oAddress.SetGateway(oArgs.Gateway);
            oAddress.NetworkInterfaceName = oArgs.Adapter?.Name;
            oAddress.NetworkInterfaceMAC = oArgs.Adapter?.GetPhysicalAddress().ToString();
            AddIp(oAddress);
        }
        private void DeleteIpScheme(object? oSender, Ipv4AddressEventArgs oArgs)
        {
            Ipv4Address oAddress = oArgs.Address;
            DeleteIp(oAddress);
        }
        private void ApplyStaticNetworkScheme(object? oSender, Ipv4AddressEventArgs oArgs)
        {
            Ipv4Address oAddress = oArgs.Address;
            if (oAddress.NetworkInterfaceMAC == null) return;
            if (!USE_NETSH)
            {
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
                    MessageBox.Show($"Failure!\nIP Code: {oResponse.Item1}\nSubnet Code: {oResponse.Item2}\nGateway Code: {oResponse.Item3}", "Set Static");
                }
                return;
            }
            else
            {
                bool bSuccess = NetworkManager.SetNICStaticNetsh(oAddress.NetworkInterfaceMAC, oAddress.IpAddressString, oAddress.SubnetMaskString, oAddress.GatewayString);
                if (bSuccess)
                {
                    MessageBox.Show("Success!", "Set Static");
                }
                else
                {
                    MessageBox.Show("Failure!", "Set Static");
                }
            }
            
        }
        private void ApplyDynamicNetworkScheme(object? oSender, NetworkInterfaceEventArgs oArgs)
        {
            NetworkInterface? oInterface = oArgs.Interface;
            if(oInterface == null) return;
            if (!USE_NETSH)
            {
                (uint, uint) oResponse = NetworkManager.SetNICDHCP(oInterface.GetPhysicalAddress().ToString());
                if (oResponse.Item1 == 0 && oResponse.Item2 == 0)
                {
                    MessageBox.Show("Success!", "Set DHCP");
                }
                else
                {
                    MessageBox.Show("Failure!", "Set DHCP");
                }
                return;
            }
            else
            {
                bool bSuccess = NetworkManager.SetNICDhcpNetsh(oInterface.GetPhysicalAddress().ToString());
                if (bSuccess)
                {
                    MessageBox.Show("Success!", "Set DHCP");
                }
                else
                {
                    MessageBox.Show("Failure!", "Set DHCP");
                }
            }
            
        }
        private void AddIp(Ipv4Address oAddress, bool bOverwrite=true)
        {
            Ipv4Address? oExistingIP = _Ips?.Find(x => x.Equals(oAddress));
            if (bOverwrite && oExistingIP != null)
            {
                _Ips?.Remove(oExistingIP);
            }
            if (!bOverwrite && oExistingIP != null)
            {
                return;
            }
            _Ips?.Add(oAddress);
            _Ips?.Sort();
            SetFormIpList();
            Save();
        }
        private void DeleteIp(Ipv4Address oAddress)
        {
            _Ips?.Remove(oAddress);
            _Ips?.Sort();
            SetFormIpList();
            Save();
        }
        private void PingNetworkInterface(object? oSender, NetworkInterfaceEventArgs oArgs)
        {
            NetworkInterface? oInterface = oArgs.Interface;
            if (oInterface == null) return;
            (string sIP, string sSubnet) = NetworkManager.GetNetworkInterfaceIpAddress(oInterface.GetPhysicalAddress().ToString());
            if(sIP == null || sSubnet == null)
            {
                Engine.Notify.NotifyHandler.General(CodeNotify.NoPingAdapter);
            }
            NetworkManager.PingUpdated += new EventHandler<NetworkManager.PingResultEventArgs>(OnPingUpdated);
            NetworkManager.PingNetwork(sIP, sSubnet);
        }
        private void OnPingUpdated(object sender, NetworkManager.PingResultEventArgs e)
        {
            if (e.Address == null) return;
            byte[] aMacBytes = NativeWindowsFunctions.GetMacAddress(e.Address);
            if (aMacBytes == null) return;
            string sMac = string.Join(":", aMacBytes.Select(x => x.ToString("X")));
            string? sVendor = GetVendorByMac(sMac);
            Form?.AddPingResult(e.Address.MapToIPv4().ToString(), sMac, sVendor);
        }
        private void ClearFormIpList()
        {
            Form?.ClearIpList();
        }
        private void SetFormIpList()
        {
            Form?.SetIpList(_Ips.ToArray());
        }
        private void ClearFormNetworkAdapters()
        {
            Form?.ClearNetworkAdapters();
        }
        private void SetFormNetworkAdapters()
        {
            foreach(NetworkInterface oAdapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                Form?.AddNetworkAdapter(oAdapter);
            }
        }
        private void ReadVendorFile()
        {
            XmlDocument oDoc = new();
            oDoc.Load(@"vendorMacs.xml");
            foreach(XmlNode oNode in oDoc.DocumentElement.ChildNodes)
            {
                if(oNode.Attributes == null) { continue; }
                _VendorMacRelations.Add(new VendorOUI() { MAC = oNode.Attributes["mac_prefix"]?.InnerText, Name = oNode.Attributes["vendor_name"]?.InnerText });
            }
        }
        internal string? GetVendorByMac(string sMAC)
        {
            string[] sSubs = sMAC.Split(":");
            if (sSubs.Length != 6)
            {
                return string.Empty;
            }
            string sOctetOne = NetworkManager.ValidateMacOctet(sSubs[0]);
            string sOctetTwo = NetworkManager.ValidateMacOctet(sSubs[1]);
            string sOctetThree = NetworkManager.ValidateMacOctet(sSubs[2]);
            string sMACSearch = $"{sOctetOne}:{sOctetTwo}:{sOctetThree}";
            VendorOUI? oVendor = _VendorMacRelations.Find(x => x.MAC == sMACSearch);
            if (oVendor != null)
            {
                return oVendor.Name;
            }
            return string.Empty;
        }
    }
}
