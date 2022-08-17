namespace Indicon.Api.IpManager.Forms
{
    partial class IpManagerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IpManagerForm));
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SubnetTextBox4 = new System.Windows.Forms.TextBox();
            this.GatewayTextBox1 = new System.Windows.Forms.TextBox();
            this.SubnetTextBox3 = new System.Windows.Forms.TextBox();
            this.GatewayTextBox2 = new System.Windows.Forms.TextBox();
            this.SubnetTextBox2 = new System.Windows.Forms.TextBox();
            this.GatewayTextBox3 = new System.Windows.Forms.TextBox();
            this.SubnetTextBox1 = new System.Windows.Forms.TextBox();
            this.GatewayTextBox4 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.IPTextBox4 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.IPTextBox3 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.IPTextBox2 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.IPTextBox1 = new System.Windows.Forms.TextBox();
            this.IpSchemeGroupBox = new System.Windows.Forms.GroupBox();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.CommitButton = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.AdapterComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.IpConfigListView = new System.Windows.Forms.ListView();
            this.SystemTrayNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.IpSchemeGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(255, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 16);
            this.label5.TabIndex = 20;
            this.label5.Text = ".";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(153, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 16);
            this.label6.TabIndex = 19;
            this.label6.Text = ".";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(357, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 16);
            this.label4.TabIndex = 21;
            this.label4.Text = ".";
            // 
            // SubnetTextBox4
            // 
            this.SubnetTextBox4.Location = new System.Drawing.Point(374, 95);
            this.SubnetTextBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SubnetTextBox4.Name = "SubnetTextBox4";
            this.SubnetTextBox4.Size = new System.Drawing.Size(79, 27);
            this.SubnetTextBox4.TabIndex = 8;
            this.SubnetTextBox4.TextChanged += new System.EventHandler(this.IpSchemeTextBoxChanged);
            this.SubnetTextBox4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IpSchemeTextBoxKeyDown);
            // 
            // GatewayTextBox1
            // 
            this.GatewayTextBox1.Location = new System.Drawing.Point(68, 130);
            this.GatewayTextBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GatewayTextBox1.Name = "GatewayTextBox1";
            this.GatewayTextBox1.Size = new System.Drawing.Size(79, 27);
            this.GatewayTextBox1.TabIndex = 9;
            this.GatewayTextBox1.TextChanged += new System.EventHandler(this.IpSchemeTextBoxChanged);
            this.GatewayTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IpSchemeTextBoxKeyDown);
            // 
            // SubnetTextBox3
            // 
            this.SubnetTextBox3.Location = new System.Drawing.Point(272, 95);
            this.SubnetTextBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SubnetTextBox3.Name = "SubnetTextBox3";
            this.SubnetTextBox3.Size = new System.Drawing.Size(79, 27);
            this.SubnetTextBox3.TabIndex = 7;
            this.SubnetTextBox3.TextChanged += new System.EventHandler(this.IpSchemeTextBoxChanged);
            this.SubnetTextBox3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IpSchemeTextBoxKeyDown);
            // 
            // GatewayTextBox2
            // 
            this.GatewayTextBox2.Location = new System.Drawing.Point(170, 130);
            this.GatewayTextBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GatewayTextBox2.Name = "GatewayTextBox2";
            this.GatewayTextBox2.Size = new System.Drawing.Size(79, 27);
            this.GatewayTextBox2.TabIndex = 10;
            this.GatewayTextBox2.TextChanged += new System.EventHandler(this.IpSchemeTextBoxChanged);
            this.GatewayTextBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IpSchemeTextBoxKeyDown);
            // 
            // SubnetTextBox2
            // 
            this.SubnetTextBox2.Location = new System.Drawing.Point(170, 95);
            this.SubnetTextBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SubnetTextBox2.Name = "SubnetTextBox2";
            this.SubnetTextBox2.Size = new System.Drawing.Size(79, 27);
            this.SubnetTextBox2.TabIndex = 6;
            this.SubnetTextBox2.TextChanged += new System.EventHandler(this.IpSchemeTextBoxChanged);
            this.SubnetTextBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IpSchemeTextBoxKeyDown);
            // 
            // GatewayTextBox3
            // 
            this.GatewayTextBox3.Location = new System.Drawing.Point(272, 130);
            this.GatewayTextBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GatewayTextBox3.Name = "GatewayTextBox3";
            this.GatewayTextBox3.Size = new System.Drawing.Size(79, 27);
            this.GatewayTextBox3.TabIndex = 11;
            this.GatewayTextBox3.TextChanged += new System.EventHandler(this.IpSchemeTextBoxChanged);
            this.GatewayTextBox3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IpSchemeTextBoxKeyDown);
            // 
            // SubnetTextBox1
            // 
            this.SubnetTextBox1.Location = new System.Drawing.Point(68, 95);
            this.SubnetTextBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SubnetTextBox1.Name = "SubnetTextBox1";
            this.SubnetTextBox1.Size = new System.Drawing.Size(79, 27);
            this.SubnetTextBox1.TabIndex = 5;
            this.SubnetTextBox1.TextChanged += new System.EventHandler(this.IpSchemeTextBoxChanged);
            this.SubnetTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IpSchemeTextBoxKeyDown);
            // 
            // GatewayTextBox4
            // 
            this.GatewayTextBox4.Location = new System.Drawing.Point(374, 130);
            this.GatewayTextBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GatewayTextBox4.Name = "GatewayTextBox4";
            this.GatewayTextBox4.Size = new System.Drawing.Size(79, 27);
            this.GatewayTextBox4.TabIndex = 12;
            this.GatewayTextBox4.TextChanged += new System.EventHandler(this.IpSchemeTextBoxChanged);
            this.GatewayTextBox4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IpSchemeTextBoxKeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(357, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = ".";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label9.Location = new System.Drawing.Point(153, 138);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(11, 16);
            this.label9.TabIndex = 26;
            this.label9.Text = ".";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(255, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = ".";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(255, 138);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(11, 16);
            this.label8.TabIndex = 27;
            this.label8.Text = ".";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(153, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 16);
            this.label1.TabIndex = 13;
            this.label1.Text = ".";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(357, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(11, 16);
            this.label7.TabIndex = 28;
            this.label7.Text = ".";
            // 
            // IPTextBox4
            // 
            this.IPTextBox4.Location = new System.Drawing.Point(374, 60);
            this.IPTextBox4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.IPTextBox4.Name = "IPTextBox4";
            this.IPTextBox4.Size = new System.Drawing.Size(79, 27);
            this.IPTextBox4.TabIndex = 4;
            this.IPTextBox4.TextChanged += new System.EventHandler(this.IpSchemeTextBoxChanged);
            this.IPTextBox4.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IpSchemeTextBoxKeyDown);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(43, 64);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(21, 20);
            this.label10.TabIndex = 29;
            this.label10.Text = "IP";
            // 
            // IPTextBox3
            // 
            this.IPTextBox3.Location = new System.Drawing.Point(272, 60);
            this.IPTextBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.IPTextBox3.Name = "IPTextBox3";
            this.IPTextBox3.Size = new System.Drawing.Size(79, 27);
            this.IPTextBox3.TabIndex = 3;
            this.IPTextBox3.TextChanged += new System.EventHandler(this.IpSchemeTextBoxChanged);
            this.IPTextBox3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IpSchemeTextBoxKeyDown);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 99);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 20);
            this.label11.TabIndex = 30;
            this.label11.Text = "Subnet";
            // 
            // IPTextBox2
            // 
            this.IPTextBox2.Location = new System.Drawing.Point(170, 60);
            this.IPTextBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.IPTextBox2.Name = "IPTextBox2";
            this.IPTextBox2.Size = new System.Drawing.Size(79, 27);
            this.IPTextBox2.TabIndex = 2;
            this.IPTextBox2.TextChanged += new System.EventHandler(this.IpSchemeTextBoxChanged);
            this.IPTextBox2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IpSchemeTextBoxKeyDown);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(2, 134);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(66, 20);
            this.label12.TabIndex = 31;
            this.label12.Text = "Gateway";
            // 
            // IPTextBox1
            // 
            this.IPTextBox1.Location = new System.Drawing.Point(68, 60);
            this.IPTextBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.IPTextBox1.Name = "IPTextBox1";
            this.IPTextBox1.Size = new System.Drawing.Size(79, 27);
            this.IPTextBox1.TabIndex = 1;
            this.IPTextBox1.TextChanged += new System.EventHandler(this.IpSchemeTextBoxChanged);
            this.IPTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.IpSchemeTextBoxKeyDown);
            // 
            // IpSchemeGroupBox
            // 
            this.IpSchemeGroupBox.Controls.Add(this.NameTextBox);
            this.IpSchemeGroupBox.Controls.Add(this.label14);
            this.IpSchemeGroupBox.Controls.Add(this.CommitButton);
            this.IpSchemeGroupBox.Controls.Add(this.label13);
            this.IpSchemeGroupBox.Controls.Add(this.AdapterComboBox);
            this.IpSchemeGroupBox.Controls.Add(this.IPTextBox1);
            this.IpSchemeGroupBox.Controls.Add(this.GatewayTextBox4);
            this.IpSchemeGroupBox.Controls.Add(this.label12);
            this.IpSchemeGroupBox.Controls.Add(this.label3);
            this.IpSchemeGroupBox.Controls.Add(this.IPTextBox4);
            this.IpSchemeGroupBox.Controls.Add(this.SubnetTextBox1);
            this.IpSchemeGroupBox.Controls.Add(this.IPTextBox2);
            this.IpSchemeGroupBox.Controls.Add(this.label9);
            this.IpSchemeGroupBox.Controls.Add(this.label5);
            this.IpSchemeGroupBox.Controls.Add(this.GatewayTextBox3);
            this.IpSchemeGroupBox.Controls.Add(this.label11);
            this.IpSchemeGroupBox.Controls.Add(this.label2);
            this.IpSchemeGroupBox.Controls.Add(this.label6);
            this.IpSchemeGroupBox.Controls.Add(this.SubnetTextBox2);
            this.IpSchemeGroupBox.Controls.Add(this.IPTextBox3);
            this.IpSchemeGroupBox.Controls.Add(this.label8);
            this.IpSchemeGroupBox.Controls.Add(this.label4);
            this.IpSchemeGroupBox.Controls.Add(this.GatewayTextBox2);
            this.IpSchemeGroupBox.Controls.Add(this.label10);
            this.IpSchemeGroupBox.Controls.Add(this.label1);
            this.IpSchemeGroupBox.Controls.Add(this.SubnetTextBox4);
            this.IpSchemeGroupBox.Controls.Add(this.SubnetTextBox3);
            this.IpSchemeGroupBox.Controls.Add(this.GatewayTextBox1);
            this.IpSchemeGroupBox.Controls.Add(this.label7);
            this.IpSchemeGroupBox.Location = new System.Drawing.Point(12, 26);
            this.IpSchemeGroupBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.IpSchemeGroupBox.Name = "IpSchemeGroupBox";
            this.IpSchemeGroupBox.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.IpSchemeGroupBox.Size = new System.Drawing.Size(461, 232);
            this.IpSchemeGroupBox.TabIndex = 32;
            this.IpSchemeGroupBox.TabStop = false;
            this.IpSchemeGroupBox.Text = "New IP Scheme";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(68, 25);
            this.NameTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(385, 27);
            this.NameTextBox.TabIndex = 0;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(15, 28);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(49, 20);
            this.label14.TabIndex = 35;
            this.label14.Text = "Name";
            // 
            // CommitButton
            // 
            this.CommitButton.Location = new System.Drawing.Point(380, 177);
            this.CommitButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CommitButton.Name = "CommitButton";
            this.CommitButton.Size = new System.Drawing.Size(73, 39);
            this.CommitButton.TabIndex = 34;
            this.CommitButton.Text = "Commit";
            this.CommitButton.UseVisualStyleBackColor = true;
            this.CommitButton.Click += new System.EventHandler(this.OnCommit_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 166);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 20);
            this.label13.TabIndex = 33;
            this.label13.Text = "Adapter";
            // 
            // AdapterComboBox
            // 
            this.AdapterComboBox.FormattingEnabled = true;
            this.AdapterComboBox.Location = new System.Drawing.Point(6, 188);
            this.AdapterComboBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.AdapterComboBox.Name = "AdapterComboBox";
            this.AdapterComboBox.Size = new System.Drawing.Size(306, 28);
            this.AdapterComboBox.TabIndex = 32;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ApplyButton);
            this.groupBox2.Controls.Add(this.IpConfigListView);
            this.groupBox2.Location = new System.Drawing.Point(12, 263);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(694, 344);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "IP Configurations";
            // 
            // ApplyButton
            // 
            this.ApplyButton.Location = new System.Drawing.Point(615, 297);
            this.ApplyButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(73, 39);
            this.ApplyButton.TabIndex = 35;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.OnApply_Click);
            // 
            // IpConfigListView
            // 
            this.IpConfigListView.Location = new System.Drawing.Point(0, 26);
            this.IpConfigListView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.IpConfigListView.Name = "IpConfigListView";
            this.IpConfigListView.Size = new System.Drawing.Size(688, 263);
            this.IpConfigListView.TabIndex = 0;
            this.IpConfigListView.UseCompatibleStateImageBehavior = false;
            // 
            // SystemTrayNotifyIcon
            // 
            this.SystemTrayNotifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.SystemTrayNotifyIcon.BalloonTipText = "Double Click This Icon To Return To IP Manager";
            this.SystemTrayNotifyIcon.BalloonTipTitle = "IP Manager";
            this.SystemTrayNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("SystemTrayNotifyIcon.Icon")));
            this.SystemTrayNotifyIcon.Text = "IP Manager";
            this.SystemTrayNotifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SystemTrayNotifyIcon_OnMouseDoubleClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(718, 28);
            this.menuStrip1.TabIndex = 34;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(116, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.OnExit_Click);
            // 
            // IpManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 610);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.IpSchemeGroupBox);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "IpManagerForm";
            this.Text = "IP Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnForm_Closing);
            this.Resize += new System.EventHandler(this.OnForm_Resize);
            this.IpSchemeGroupBox.ResumeLayout(false);
            this.IpSchemeGroupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SubnetTextBox4;
        private System.Windows.Forms.TextBox GatewayTextBox1;
        private System.Windows.Forms.TextBox SubnetTextBox3;
        private System.Windows.Forms.TextBox GatewayTextBox2;
        private System.Windows.Forms.TextBox SubnetTextBox2;
        private System.Windows.Forms.TextBox GatewayTextBox3;
        private System.Windows.Forms.TextBox SubnetTextBox1;
        private System.Windows.Forms.TextBox GatewayTextBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox IPTextBox4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox IPTextBox3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox IPTextBox2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox IPTextBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button CommitButton;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox AdapterComboBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.ListView IpConfigListView;
        private GroupBox IpSchemeGroupBox;
        private TextBox NameTextBox;
        private Label label14;
        private NotifyIcon SystemTrayNotifyIcon;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
    }
}