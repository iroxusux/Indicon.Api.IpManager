using System.Net.NetworkInformation;
using Indicon.Api.IpManager.Classes;

namespace Indicon.Api.IpManager.Forms
{
    public partial class IpManagerForm : Form
    {
        private const int IP_LOW = 0;
        private const int IP_HIGH = 255;
        private bool bHintShown = false;
        public EventHandler<IpAddressCommitEventArgs> IpCommited = delegate { };
        public IpManagerForm()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            /// Subscribe our functions to the system StaticIpManager
            IpCommited += new EventHandler<IpAddressCommitEventArgs>(StaticIpManager.CreateNewIp);
            /// Compile network interface adapters into combo box for user selection (Static & DHCP Configurations)
            foreach (NetworkInterface oAdapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                ComboBoxItem oItem = new ComboBoxItem() { Text = oAdapter.Name, Tag = oAdapter };
                AdapterComboBox.Items.Add(oItem);
                AdapterComboBoxDHCP.Items.Add(oItem);
            }
            /// Prepare list view
            IpConfigListView.View = View.Details;
            IpConfigListView.GridLines = true;
            IpConfigListView.FullRowSelect = true;
            IpConfigListView.Columns.Add("Nickname", -2, HorizontalAlignment.Left);
            IpConfigListView.Columns.Add("IP", -2, HorizontalAlignment.Left);
            IpConfigListView.Columns.Add("Subnet", -2, HorizontalAlignment.Left);
            IpConfigListView.Columns.Add("Gateway", -2, HorizontalAlignment.Left);
            IpConfigListView.Columns.Add("Network Interface", -2, HorizontalAlignment.Left);

            /// Setup context menu strip
            ContextMenuStrip oStripBuilder = new ContextMenuStrip();
            var oEditEntry = oStripBuilder.Items.Add("Edit Entry", null, new EventHandler(CmEditEntry));
            var oDeleteEntry = oStripBuilder.Items.Add("Delete Entry", null, new EventHandler(CmDeleteEntry));
            IpConfigListView.ContextMenuStrip = oStripBuilder;

            /// Setup combo box
            AdapterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            AdapterComboBoxDHCP.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        public void UpdateIpList(Ipv4Address[] oElements)
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
        private void IpSchemeTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            TextBox oTextBox = sender as TextBox;
            if (oTextBox == null)
            {
                return;
            }
            if (e.KeyCode == Keys.OemPeriod || e.KeyCode == Keys.Decimal)
            {
                NextIpSchemeControl(oTextBox);
                e.SuppressKeyPress = true;
            }
        }
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
        private void OnCommit_Click(object sender, EventArgs e)
        {
            if (!ValidateEntry())
            {
                return;
            }
            NetworkInterface? oInterface = null;
            ComboBoxItem? oItem = AdapterComboBox.SelectedItem as ComboBoxItem;
            if (oItem != null)
            {
                oInterface = oItem.Tag as NetworkInterface;
            }
            IpAddressCommitEventArgs oArgs = new()
            {
                Name = NameTextBox.Text,
                IpAddress = new string[4] { IPTextBox1.Text, IPTextBox2.Text, IPTextBox3.Text, IPTextBox4.Text },
                SubnetMask = new string[4] { SubnetTextBox1.Text, SubnetTextBox2.Text, SubnetTextBox3.Text, SubnetTextBox4.Text },
                Gateway = new string[4] { GatewayTextBox1.Text, GatewayTextBox2.Text, GatewayTextBox3.Text, GatewayTextBox4.Text },
                Adapter = oInterface
            };
            IpAddressCommited(oArgs);
        }
        private bool ValidateEntry()
        {
            foreach(Control oControl in IpSchemeGroupBox.Controls)
            {
                TextBox oTextBox = oControl as TextBox;
                if(oTextBox != null)
                {
                    if(oTextBox.Text == String.Empty)
                    {
                        MessageBox.Show("All fields must be complete before committing a new IP scheme!");
                        return false;
                    }
                }
                if(AdapterComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Adapter must be selected before committing a new IP scheme!");
                    return false;
                }
            }
            return true;
        }
        private void IpAddressCommited(IpAddressCommitEventArgs e)
        {
            EventHandler<IpAddressCommitEventArgs> oHandler = IpCommited;
            oHandler?.Invoke(this, e);
        }
        private void OnApply_Click(object sender, EventArgs e)
        {
            if (IpConfigListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("No network configuration selected!");
                return;
            }
            Ipv4Address? oAddress = IpConfigListView.SelectedItems[0].Tag as Ipv4Address;
            if (oAddress != null)
            {
                StaticIpManager.SetIpScheme(oAddress);
            }
        }
        private void CmDeleteEntry(object sender, EventArgs e)
        {
            Ipv4Address oTag = GetSelectedTag();
            if(oTag != null)
            {
                var oConfirmResult = MessageBox.Show($"Are you sure you wish to delete {oTag.Name}?", "Confirm IP Scheme Deletion", MessageBoxButtons.YesNo);
                if (oConfirmResult == DialogResult.Yes)
                {
                    StaticIpManager.DeleteEntry(oTag);
                    UpdateIpList(StaticIpManager.Ips.ToArray());
                }
            }
        }
        private void CmEditEntry(object sender, EventArgs e)
        {
            Ipv4Address oTag = GetSelectedTag();
            if(oTag != null)
            {
                SetEditingAddress(oTag);
            }
            
        }
        private Ipv4Address GetSelectedTag()
        {
            if (IpConfigListView.SelectedItems.Count == 0)
            {
                return null;
            }
            return IpConfigListView.SelectedItems[0].Tag as Ipv4Address;
            
        }
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
        private void OnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        private void OnForm_Closing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                WindowState = FormWindowState.Minimized;
            }
        }
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
                oNewStaticItem.Click += new EventHandler(SystemTray_SetStaticIPAddress);
                oSetStaticItem.DropDownItems.Add(oNewStaticItem);
            }
            /// Setup DHCP configuration hotkeys for the drop down
            ToolStripMenuItem oSetDHCPItem = new ToolStripMenuItem() { Name = "SetDHCP", Text = "Set DHCP" };
            oSetDHCPItem.DropDownDirection = ToolStripDropDownDirection.Left;
            foreach (NetworkInterface oAdapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                ToolStripMenuItem oNewDHCPItem = new ToolStripMenuItem() { Name = oAdapter.Name, Text = oAdapter.Name, Tag = oAdapter };
                oNewDHCPItem.Click += new EventHandler(SystemTray_SetDHCPIPAddress);
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
        private void SystemTray_SetStaticIPAddress(object sender, EventArgs e)
        {
            ToolStripItem? oItem = sender as ToolStripItem;
            if(oItem != null)
            {
                Ipv4Address? oAddress = oItem.Tag as Ipv4Address;
                if (oAddress != null)
                {
                    StaticIpManager.SetIpScheme(oAddress);
                }
            }
        }
        private void SystemTray_SetDHCPIPAddress(object sender, EventArgs e)
        {
            ToolStripMenuItem oItem = sender as ToolStripMenuItem;
            if(oItem != null)
            {
                NetworkInterface oInterface = oItem.Tag as NetworkInterface;
                if(oInterface != null)
                {
                    StaticIpManager.SetDHCPScheme(oInterface.GetPhysicalAddress().ToString());
                }
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
        private void SetDHCPButton_Click(object sender, EventArgs e)
        {
            ComboBoxItem? oItem = AdapterComboBoxDHCP.SelectedItem as ComboBoxItem;
            if (oItem != null)
            {
                NetworkInterface oInterface = oItem.Tag as NetworkInterface;
                if(oInterface != null)
                {
                    StaticIpManager.SetDHCPScheme(oInterface.GetPhysicalAddress().ToString());
                }
            }
        }
        private void RestoreWindow(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            SystemTrayNotifyIcon.Visible = false;
        }
        private void OpenNetworkConnections_Click(object sender, EventArgs e)
        {
            StaticIpManager.OpenNetworkConnectionsPanel();
        }
    }
    public class IpAddressCommitEventArgs : EventArgs
    {
        public string Name;
        public string[] IpAddress;
        public string[] SubnetMask;
        public string[] Gateway;
        public NetworkInterface Adapter;
    }
}
