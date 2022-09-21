using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using Indicon.Api.IpManager.Classes;
using Indicon.Api.IpManager.Events;

namespace Indicon.Api.IpManager.Forms
{
    public partial class IpManagerForm : Form
    {
        /// <summary>
        /// Local Variables
        /// </summary>
        private const int IP_LOW = 0;
        private const int IP_HIGH = 255;
        private bool bHintShown = false;

        /// <summary>
        /// Event Delegates
        /// </summary>
        internal EventHandler<IpAddressCommitEventArgs> IpSchemeCommit = delegate { };
        internal EventHandler<Ipv4AddressEventArgs> IpSchemeDelete = delegate { };
        internal EventHandler<Ipv4AddressEventArgs> NetworkInterfaceApplyStatic = delegate { };
        internal EventHandler<NetworkInterfaceEventArgs> NetworkInterfaceApplyDhcp = delegate { };
        internal EventHandler<NetworkInterfaceEventArgs> NetworkInterfacePing = delegate { };
        /// <summary>
        /// Event Delegation Methods
        /// </summary>
        /// <param name="oArgs"></param>
        private void OnIpSchemeCommit(IpAddressCommitEventArgs oArgs)
        {
            EventHandler<IpAddressCommitEventArgs> oHandler = IpSchemeCommit;
            oHandler?.Invoke(this, oArgs);
        }
        private void OnIpSchemeDelete(Ipv4AddressEventArgs oArgs)
        {
            EventHandler<Ipv4AddressEventArgs> oHandler = IpSchemeDelete;
            oHandler?.Invoke(this, oArgs);
        }
        private void OnNetworkInterfaceApplyStatic(Ipv4AddressEventArgs oArgs)
        {
            EventHandler<Ipv4AddressEventArgs> oHandler = NetworkInterfaceApplyStatic;
            oHandler?.Invoke(this, oArgs);
        }
        private void OnNetworkInterfaceApplyDhcp(NetworkInterfaceEventArgs oArgs)
        {
            EventHandler<NetworkInterfaceEventArgs> oHandler = NetworkInterfaceApplyDhcp;
            oHandler?.Invoke(this, oArgs);
        }
        private void OnNetworkInterfacePing(NetworkInterfaceEventArgs oArgs)
        {
            EventHandler<NetworkInterfaceEventArgs> oHandler = NetworkInterfacePing;
            oHandler?.Invoke(this, oArgs);
        }
        /// <summary>
        /// Event Drivers
        /// </summary>
        private void OnSchemeCommit_Click(object oSender, EventArgs oArgs)
        {
            if (!ValidateEntry()) return;
            ComboBoxItem? oItem = AdapterComboBox.SelectedItem as ComboBoxItem;
            if (oItem == null) return;
            NetworkInterface? oInterface = oItem.Tag as NetworkInterface;
            if (oInterface == null) return;
            IpAddressCommitEventArgs oAddressEventArgs = new()
            {
                Name = NameTextBox.Text,
                IpAddress = new string[4] { IPTextBox1.Text, IPTextBox2.Text, IPTextBox3.Text, IPTextBox4.Text },
                SubnetMask = new string[4] { SubnetTextBox1.Text, SubnetTextBox2.Text, SubnetTextBox3.Text, SubnetTextBox4.Text },
                Gateway = new string[4] { GatewayTextBox1.Text, GatewayTextBox2.Text, GatewayTextBox3.Text, GatewayTextBox4.Text },
                Adapter = oInterface,
            };
            OnIpSchemeCommit(oAddressEventArgs);
        }
        private void OnDeleteEntry_Click(object oSender, EventArgs oArgs)
        {
            Ipv4Address oAddress = GetSelectedIpScheme();
            if (oAddress == null) return;
            var oConfirmResult = MessageBox.Show($"Are you sure you wish to delete {oAddress.Name}?", "Confirm IP Scheme Deletion", MessageBoxButtons.YesNo);
            if (oConfirmResult == DialogResult.Yes)
            {
                Ipv4AddressEventArgs oAddressEventArgs = new() { Address = oAddress };
                OnIpSchemeDelete(oAddressEventArgs);
            }
        }
        private void OnApplyStatic_Click(object oSender, EventArgs oArgs)
        {
            if (IpConfigListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("No network configuration selected!", "Failed To Apply Network Configuration!");
                return;
            }
            Ipv4Address? oAddress = IpConfigListView.SelectedItems[0].Tag as Ipv4Address;
            if (oAddress == null) return;
            Ipv4AddressEventArgs oAddressEventArgs = new() { Address = oAddress };
            OnNetworkInterfaceApplyStatic(oAddressEventArgs);
        }
        private void OnApplyDHCP_Click(object oSender, EventArgs oArgs)
        {
            ComboBoxItem? oItem = AdapterComboBoxDHCP.SelectedItem as ComboBoxItem;
            if(oItem == null) return;
            NetworkInterface? oInterface = oItem.Tag as NetworkInterface;
            if(oInterface == null) return;
            NetworkInterfaceEventArgs oInterfaceArgs = new() { Interface = oInterface };
            OnNetworkInterfaceApplyDhcp(oInterfaceArgs);
        }
        /// <summary>
        /// Private Method - System Tray Implimentation Of Static Ip Scheme Set
        /// </summary>
        /// <param name="oSender"></param>
        /// <param name="oArgs"></param>
        private void OnSystemTraySetStaticIPAddress_Click(object oSender, EventArgs oArgs)
        {
            ToolStripItem? oItem = oSender as ToolStripItem;
            if (oItem == null) return;
            Ipv4Address? oAddress = oItem.Tag as Ipv4Address;
            if (oAddress == null) return;
            Ipv4AddressEventArgs oAddressEventArgs = new() { Address = oAddress };
            OnNetworkInterfaceApplyStatic(oAddressEventArgs);
        }
        /// <summary>
        /// Private Method - System Tray Implimentation Of Dhcp Ip Scheme Set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSystemTraySetDhcpIpAddress_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem? oItem = sender as ToolStripMenuItem;
            if (oItem != null) return;
            NetworkInterface? oInterface = oItem.Tag as NetworkInterface;
            if (oInterface == null) return;
            NetworkInterfaceEventArgs oInterfaceArgs = new() { Interface = oInterface };
            OnNetworkInterfaceApplyDhcp(oInterfaceArgs);
        }
        /// <summary>
        /// Public & Default Constructor
        /// </summary>
        public IpManagerForm()
        {
            InitializeComponent();
            Init();
        }
        /// <summary>
        /// Detached Initialization Method
        /// Windows Forms Create Their Own Methods For Init, So Instead Of Modifying Those, A Seperate Init Method Was Created For User Initialization
        /// </summary>
        private void Init()
        {
            /// Prepare list view
            IpConfigListView.View = View.Details;
            IpConfigListView.GridLines = true;
            IpConfigListView.FullRowSelect = true;
            IpConfigListView.Columns.Add("Nickname", -2, HorizontalAlignment.Left);
            IpConfigListView.Columns.Add("IP", -2, HorizontalAlignment.Left);
            IpConfigListView.Columns.Add("Subnet", -2, HorizontalAlignment.Left);
            IpConfigListView.Columns.Add("Gateway", -2, HorizontalAlignment.Left);
            IpConfigListView.Columns.Add("Network Interface", -2, HorizontalAlignment.Left);

            /// Prepare list view (Network Pinging)
            NetworkPingResultsListView.View = View.Details;
            NetworkPingResultsListView.GridLines = true;
            NetworkPingResultsListView.FullRowSelect = true;
            NetworkPingResultsListView.Columns.Add("IP Address", -2, HorizontalAlignment.Left);
            NetworkPingResultsListView.Columns.Add("MAC Address", -2, HorizontalAlignment.Left);
            NetworkPingResultsListView.Columns.Add("Vendor", -2, HorizontalAlignment.Left);

            /// Setup context menu strip
            ContextMenuStrip oStripBuilder = new ContextMenuStrip();
            var oEditEntry = oStripBuilder.Items.Add("Edit Entry", null, new EventHandler(OnEditEntry_Click));
            var oDeleteEntry = oStripBuilder.Items.Add("Delete Entry", null, new EventHandler(OnDeleteEntry_Click));
            IpConfigListView.ContextMenuStrip = oStripBuilder;

            /// Setup combo box
            AdapterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            AdapterComboBoxDHCP.DropDownStyle = ComboBoxStyle.DropDownList;

            /// Setup "checked" state of run on startup
            //launchOnWindowsStartToolStripMenuItem.Checked = StaticIpManager.GetStartup();
        }
        /// <summary>
        /// Public Method To Clear Ip List
        /// </summary>
        public void ClearIpList()
        {
            IpConfigListView.Items.Clear();
        }
        /// <summary>
        /// Public Method To Clear Any Controls Containing Network Adapters
        /// </summary>
        public void ClearNetworkAdapters()
        {
            AdapterComboBox.Items.Clear();
            AdapterComboBoxDHCP.Items.Clear();
        }
        /// <summary>
        /// Public Method To Add Network Adapters Into This Form's Controls
        /// </summary>
        /// <param name="oInterface"></param>
        public void AddNetworkAdapter(NetworkInterface oInterface)
        {
            ComboBoxItem oItem = new ComboBoxItem() { Text = oInterface.Name, Tag = oInterface };
            AdapterComboBox.Items.Add(oItem);
            AdapterComboBoxDHCP.Items.Add(oItem);
        }
        /// <summary>
        /// Public Method To Auto-Clear & Reset Ip List View Contents
        /// </summary>
        /// <param name="oElements"></param>
        public void SetIpList(Ipv4Address[] oElements)
        {
            IpConfigListView.Items.Clear();
            for (int i = 0; i < oElements.Length; i++)
            {
                ListViewItem oItem = new ListViewItem() { Name = oElements[i].Name, Text = oElements[i].Name, Tag = oElements[i] };
                oItem.SubItems.Add(oElements[i].IpAddressString);
                oItem.SubItems.Add(oElements[i].SubnetMaskString);
                oItem.SubItems.Add(oElements[i].GatewayString);
                oItem.SubItems.Add(oElements[i].NetworkInterfaceName);
                IpConfigListView.Items.Add(oItem);
            }
            IpConfigListView.Sorting = SortOrder.Ascending;
            IpConfigListView.Sort();
            UpdateTrayContextMenuStrip(oElements);
        }
        /// <summary>
        /// Build And Manage Context Menu Strip Of System Tray For This Application
        /// </summary>
        /// <param name="oElements"></param>
        private void UpdateTrayContextMenuStrip(Ipv4Address[] oElements)
        {
            ContextMenuStrip oStripBuilder = new ContextMenuStrip();
            oStripBuilder.Items.Add("Open", null, new EventHandler(RestoreWindow));
            oStripBuilder.Items.Add("-");
            /// Setup static IP configuration hotkeys for the drop down
            ToolStripMenuItem oSetStaticItem = new ToolStripMenuItem() { Name = "SetStatic", Text = "Set Static" };
            oSetStaticItem.DropDownDirection = ToolStripDropDownDirection.Left;
            for (int i = 0; i < oElements.Length; i++)
            {
                string sDisplayString = $"{oElements[i].Name} {oElements[i].IpAddressString}";
                ToolStripMenuItem oNewStaticItem = new ToolStripMenuItem() { Name = oElements[i].Name, Text = sDisplayString, Tag = oElements[i] };
                oNewStaticItem.Click += new EventHandler(OnSystemTraySetStaticIPAddress_Click);
                oSetStaticItem.DropDownItems.Add(oNewStaticItem);
            }
            /// Setup DHCP configuration hotkeys for the drop down
            ToolStripMenuItem oSetDHCPItem = new ToolStripMenuItem() { Name = "SetDHCP", Text = "Set DHCP" };
            oSetDHCPItem.DropDownDirection = ToolStripDropDownDirection.Left;
            foreach (NetworkInterface oAdapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                ToolStripMenuItem oNewDHCPItem = new ToolStripMenuItem() { Name = oAdapter.Name, Text = oAdapter.Name, Tag = oAdapter };
                oNewDHCPItem.Click += new EventHandler(OnSystemTraySetDhcpIpAddress_Click);
                oSetDHCPItem.DropDownItems.Add(oNewDHCPItem);
            }
            oStripBuilder.Items.Add(oSetStaticItem);
            oStripBuilder.Items.Add(oSetDHCPItem);
            oStripBuilder.Items.Add("-");
            oStripBuilder.Items.Add("Open Network Connections", null, new EventHandler(OpenNetworkConnections_Click));
            oStripBuilder.Items.Add("-");
            oStripBuilder.Items.Add("Exit", null, new EventHandler(OnExit_Click));
            SystemTrayNotifyIcon.ContextMenuStrip = oStripBuilder;
        }
        /// <summary>
        /// Add Ip Address Into Ping Result Window
        /// </summary>
        /// <param name="oAddress"></param>
        public void AddPingResult(string sIpAddress, string sMac, string? sVendor)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    AddPingResult(sIpAddress, sMac, sVendor);
                });
                return;
            }
            ListViewItem oItem = new ListViewItem() { Name = sIpAddress, Text = sIpAddress };
            oItem.SubItems.Add(sMac);
            oItem.SubItems.Add(sVendor);
            NetworkPingResultsListView.Items.Add(oItem);
            NetworkPingResultsListView.Sorting = SortOrder.Ascending;
            NetworkPingResultsListView.Sort();

        }
        /// <summary>
        /// Private Method - Any IP Text Box Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IpSchemeTextBoxChanged(object sender, EventArgs e)
        {
            TextBox oTextBox = sender as TextBox;
            if (oTextBox == null)
            {
                return;
            }
            try
            {
                int iText = Convert.ToInt32(oTextBox.Text);
                if (iText < IP_LOW)
                {
                    oTextBox.Text = IP_LOW.ToString();
                }
                if (iText > IP_HIGH)
                {
                    oTextBox.Text = IP_HIGH.ToString();
                }
                if (iText >= 100 || oTextBox.Text.Length >= 3)
                {
                    NextIpSchemeControl(oTextBox);
                }
            }
            catch (Exception)
            {
                oTextBox.Text = String.Empty;
            }
        }
        /// <summary>
        /// Private Method - Any IP Text Box Key Down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IpSchemeTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            /// Determine if object is valid for parsing
            TextBox? oTextBox = sender as TextBox;
            if (oTextBox == null) return;
            if (e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Decimal)
            {
                NextIpSchemeControl(oTextBox);
                e.SuppressKeyPress = true;
            }
            /// Check for non-numeric value being entered (and also not "back" or backspace)
            if(e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    if (e.KeyCode != Keys.Back)
                    {
                        e.SuppressKeyPress = true;
                    }
                }
            }
            /// Shift being pressed here will also be NaN
            if(e.Modifiers == Keys.Shift)
            {
                e.SuppressKeyPress = true;
            }
        }
        /// <summary>
        /// Private Method
        /// User Interface Helper To Auto Scroll To Next Entry Box While Entering An Address
        /// </summary>
        /// <param name="oControl"></param>
        private void NextIpSchemeControl(Control oControl)
        {
            foreach (Control oNextControl in IpSchemeGroupBox.Controls)
            {
                if (oNextControl.TabIndex == oControl.TabIndex + 1)
                {
                    oNextControl.Focus();
                }
            }
        }
        /// <summary>
        /// Private Helper Method To Validate All Text Box Controls Related To Ip Address Scheme
        /// </summary>
        /// <returns></returns>
        private bool ValidateEntry()
        {
            foreach(Control oControl in IpSchemeGroupBox.Controls)
            {
                TextBox? oTextBox = oControl as TextBox;
                if (oTextBox == null) continue;
                if(oTextBox.Text == String.Empty)
                {
                    MessageBox.Show("All fields must be complete before committing a new IP scheme!", "Failed To Add Scheme!");
                    return false;
                }
                if(AdapterComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Adapter must be selected before committing a new IP scheme!", "Failed To Add Scheme!");
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Private Method - Set Existing Ip Scheme Back To Editing Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEditEntry_Click(object sender, EventArgs e)
        {
            Ipv4Address oAddress = GetSelectedIpScheme();
            if (oAddress == null) return;
            SetEditingAddress(oAddress);
        }
        /// <summary>
        /// Re-Populate Editing Field(s) With Provided Address
        /// </summary>
        /// <param name="oAddress"></param>
        private void SetEditingAddress(Ipv4Address oAddress)
        {
            /// Set IP strings back into Text Boxes
            NameTextBox.Text = oAddress.Name;

            IPTextBox1.Text = oAddress.IpAddress[0].ToString();
            IPTextBox2.Text = oAddress.IpAddress[1].ToString();
            IPTextBox3.Text = oAddress.IpAddress[2].ToString();
            IPTextBox4.Text = oAddress.IpAddress[3].ToString();

            SubnetTextBox1.Text = oAddress.SubnetMask[0].ToString();
            SubnetTextBox2.Text = oAddress.SubnetMask[1].ToString();
            SubnetTextBox3.Text = oAddress.SubnetMask[2].ToString();
            SubnetTextBox4.Text = oAddress.SubnetMask[3].ToString();

            GatewayTextBox1.Text = oAddress.Gateway[0].ToString();
            GatewayTextBox2.Text = oAddress.Gateway[1].ToString();
            GatewayTextBox3.Text = oAddress.Gateway[2].ToString();
            GatewayTextBox4.Text = oAddress.Gateway[3].ToString();

            foreach(ComboBoxItem oItem in AdapterComboBox.Items)
            {
                if (oItem.Text == oAddress.NetworkInterfaceName)
                {
                    AdapterComboBox.SelectedIndex = AdapterComboBox.Items.IndexOf(oItem);
                }
            }
        }
        /// <summary>
        /// Helper Method To Get "Tag" Of List Item
        /// </summary>
        /// <returns></returns>
        private Ipv4Address? GetSelectedIpScheme()
        {
            if (IpConfigListView.SelectedItems.Count == 0) return null;
            return IpConfigListView.SelectedItems[0].Tag as Ipv4Address;

        }
        /// <summary>
        /// Private Method - If Form Is Minimized, Set To System Tray Instead
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnForm_Resize(object sender, EventArgs e)
        {
            /// If the form is minimized, hide to system tray
            if(WindowState == FormWindowState.Minimized)
            {
                Hide();
                SystemTrayNotifyIcon.Visible = true;
                if (!bHintShown)
                {
                    SystemTrayNotifyIcon.ShowBalloonTip(1500);
                    bHintShown = true;
                }
            }
        }
        /// <summary>
        /// Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        /// <summary>
        /// If The User Attempted To Close This Application - Override And Minimize Instead (Which Will Be Overriden Again)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnForm_Closing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                WindowState = FormWindowState.Minimized;
            }
        }
        private void SystemTrayNotifyIcon_OnMouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                Point oPoint = NativeWindowsFunctions.GetCursorPosition();
                SystemTrayNotifyIcon.ContextMenuStrip.Show(oPoint);
            }
        }
        private void SystemTrayNotifyIcon_OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            RestoreWindow(null, EventArgs.Empty);
        }
        private void RestoreWindow(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            SystemTrayNotifyIcon.Visible = false;
        }
        private void OpenNetworkConnections_Click(object sender, EventArgs e)
        {
            Engine.Core.NetshCmds.OpenNetworkConnectionsPanel();
        }
        private async void PingNetworkButtonClick(object sender, MouseEventArgs e)
        {
            NetworkPingResultsListView.Items.Clear();
            NetworkInterface? oInterface = null;
            ComboBoxItem? oItem = AdapterComboBoxDHCP.SelectedItem as ComboBoxItem;
            if (oItem == null)
            {
                MessageBox.Show("No network adapter selected!", "Failed Ping Request!");
                return;
            }
            oInterface = oItem.Tag as NetworkInterface;
            if (oInterface == null)
            {
                MessageBox.Show("No network adapter selected!", "Failed Ping Request!");
                return;
            }
            NetworkInterfaceEventArgs oNetworkArgs = new() { Interface = oInterface };
            await Task.Factory.StartNew(() => OnNetworkInterfacePing(oNetworkArgs));
        }
        private void LaunchOnWindowsStart_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem? oItem = sender as ToolStripMenuItem;
            if(oItem == null)
            {
                return;
            }
            //StaticIpManager.SetStartup(!oItem.Checked);
            oItem.Checked = !oItem.Checked;
        }
    }
}
