using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using static ServerManager.Manager;
using static ServerManager.Notifications;
using static ServerManager.SteamInterface;
namespace ServerManager
{
    partial class ServerManager
    {
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        public int i = 0;
        private Dictionary<int, Server> tabs = new Dictionary<int, Server>();
        private MenuStrip menuStrip;
        private ToolStripMenuItem toolStripTextBox;
        private TabControl tabControl;
        private OpenFileDialog openFileDialog;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerManager));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolStripTextBox = new System.Windows.Forms.ToolStripMenuItem();
            this.addSteamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox,
            this.addSteamToolStripMenuItem,
            this.installGameToolStripMenuItem,
            this.removeTabToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.ShowItemToolTips = true;
            this.menuStrip.Size = new System.Drawing.Size(684, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
            // 
            // toolStripTextBox
            // 
            this.toolStripTextBox.Name = "toolStripTextBox";
            this.toolStripTextBox.Size = new System.Drawing.Size(63, 20);
            this.toolStripTextBox.Text = "Add Tab";
            this.toolStripTextBox.ToolTipText = "Add new server tab.";
            this.toolStripTextBox.Click += new System.EventHandler(this.AddTab);
            // 
            // addSteamToolStripMenuItem
            // 
            this.addSteamToolStripMenuItem.Name = "addSteamToolStripMenuItem";
            this.addSteamToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.addSteamToolStripMenuItem.Text = "Add Steam";
            this.addSteamToolStripMenuItem.ToolTipText = "Installs steamcmd folder into current working directory.";
            // 
            // installGameToolStripMenuItem
            // 
            this.installGameToolStripMenuItem.Name = "installGameToolStripMenuItem";
            this.installGameToolStripMenuItem.Size = new System.Drawing.Size(120, 20);
            this.installGameToolStripMenuItem.Text = "Install Steam Game";
            this.installGameToolStripMenuItem.ToolTipText = "Installs the current selected tabs steam game.";
            this.installGameToolStripMenuItem.Click += new System.EventHandler(this.TabInstall);
            // 
            // removeTabToolStripMenuItem
            // 
            this.removeTabToolStripMenuItem.Name = "removeTabToolStripMenuItem";
            this.removeTabToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.removeTabToolStripMenuItem.Text = "Delete Tab";
            this.removeTabToolStripMenuItem.ToolTipText = "Deletes current tab.";
            this.removeTabToolStripMenuItem.Click += new System.EventHandler(this.removeTab);
            // 
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(684, 437);
            this.tabControl.TabIndex = 0;
            // 
            // ServerManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MaximumSize = new System.Drawing.Size(700, 500);
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "ServerManager";
            this.Text = "Server Manager v2.0";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private void removeTab(object sender, EventArgs e)
        {
            tabControl.TabPages.Remove(tabs[tabControl.SelectedIndex].tab.tabPage);
            tabs.Remove(tabControl.SelectedIndex);
        }
        //Have START and STOP base the index of server as the tab index
        private void Start(object sender, EventArgs e)
        {
            int i = tabControl.SelectedIndex;
            ThreadStart tmp = new ThreadStart(() => run(i));
            Thread thread = new Thread(tmp);
            tabs[tabControl.SelectedIndex].thread = thread;
            thread.Start();
        }
        private void Stop(object sender, EventArgs e)
        {
            Server tmp = tabs[tabControl.SelectedIndex];
            tmp.thread.Abort();
        }
        private void TabInstall(object sender, EventArgs e)
        {
            Tab tmp = tabs[tabControl.SelectedIndex].tab;
            if (tmp.steamBox.CheckState == CheckState.Checked)
            {
                try
                {
                    SteamInterface.Update(tmp.user.Text.ToString(), tmp.passw.Text.ToString(), Convert.ToInt32(tmp.SteamLabel.Text));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Something failed check your login info and the steam game id.", "ServerManager", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Enable steam for this tab and enter the needed info.", "ServerManager", MessageBoxButtons.OK);
            }
        }
        //Add text to richtext, switch tabControl.SelectedIndex with thread index
        //tabs[tabControl.SelectedIndex].richTextBox.AppendText(Environment.NewLine + "Hello");
        //Broken Currently
        private void SelectFile(object sender, EventArgs e)
        {
            Server tmp = tabs[tabControl.SelectedIndex];
            if (tmp.tab.steamBox.CheckState == CheckState.Checked)
            {
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory() + @"\steamcmd\steamapps\common\";
            }
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tmp.tab.file.Text = openFileDialog.FileName;
            }
        }
        private void AddTab(object sender, EventArgs e)
        {
            String name;
            if (tabControl.TabCount == Environment.ProcessorCount)
            {
                MessageBox.Show("Max tabs reached for your device.", "Safety Sake", MessageBoxButtons.OK);
            }
            else
            {
                PopUp pu = new PopUp();
                pu.ShowDialog();
                if (pu.DialogResult == DialogResult.OK)
                {
                    name = pu.name;
                }
                else
                {
                    return;
                }
                TabPage tabPage = new System.Windows.Forms.TabPage();
                Button stop = new System.Windows.Forms.Button();
                Button start = new System.Windows.Forms.Button();
                CheckBox reboot = new System.Windows.Forms.CheckBox();
                CheckBox notif = new System.Windows.Forms.CheckBox();
                TextBox arguments = new System.Windows.Forms.TextBox();
                CheckBox argumentsCheck = new System.Windows.Forms.CheckBox();
                TextBox address = new System.Windows.Forms.TextBox();
                TextBox file = new System.Windows.Forms.TextBox();
                Panel panel1 = new System.Windows.Forms.Panel();
                CheckBox steamBox = new System.Windows.Forms.CheckBox();
                TextBox SteamLabel = new System.Windows.Forms.TextBox();
                CheckBox loginBox = new System.Windows.Forms.CheckBox();
                TextBox user = new System.Windows.Forms.TextBox();
                TextBox passw = new System.Windows.Forms.TextBox();
                Panel panel2 = new System.Windows.Forms.Panel();
                panel1.SuspendLayout();
                panel2.SuspendLayout();
                tabControl.Controls.Add(tabPage);
                // 
                // tabPage
                // 
                tabPage.BackColor = System.Drawing.Color.Cyan;
                tabPage.Controls.Add(panel2);
                tabPage.Controls.Add(panel1);
                tabPage.Name = "tabPage";
                tabPage.Padding = new System.Windows.Forms.Padding(3);
                tabPage.Size = new System.Drawing.Size(676, 411);
                tabPage.Text = name;
                // 
                // stop
                // 
                stop.Location = new System.Drawing.Point(595, 77);
                stop.Name = "stop";
                stop.Size = new System.Drawing.Size(75, 23);
                stop.TabIndex = 0;
                stop.Text = "Stop";
                stop.UseVisualStyleBackColor = true;
                stop.Click += new System.EventHandler(Stop);
                // 
                // start
                // 
                start.Location = new System.Drawing.Point(0, 77);
                start.Name = "start";
                start.Size = new System.Drawing.Size(75, 23);
                start.TabIndex = 1;
                start.Text = "Start";
                start.UseVisualStyleBackColor = true;
                start.Click += new System.EventHandler(Start);
                // 
                // reboot
                // 
                reboot.AutoSize = true;
                reboot.Location = new System.Drawing.Point(81, 53);
                reboot.Name = "reboot";
                reboot.Size = new System.Drawing.Size(90, 17);
                reboot.TabIndex = 2;
                reboot.Text = "Auto Reboot";
                reboot.UseVisualStyleBackColor = true;
                toolTip.SetToolTip(reboot, "Should the server auto restart upon shutdown/crash?");
                // 
                // notif
                // 
                notif.AutoSize = true;
                notif.Location = new System.Drawing.Point(81, 9);
                notif.Name = "notif";
                notif.Size = new System.Drawing.Size(84, 17);
                notif.TabIndex = 4;
                notif.Text = "Notifications";
                notif.UseVisualStyleBackColor = true;
                toolTip.SetToolTip(notif, "Do you want to get notifications on shutdown/crash/crashloop?");
                // 
                // arguments
                // 
                arguments.Location = new System.Drawing.Point(186, 30);
                arguments.Name = "arguments";
                arguments.Size = new System.Drawing.Size(100, 20);
                arguments.TabIndex = 8;
                toolTip.SetToolTip(arguments, "Arguments to pass to the server on start.");
                // 
                // argumentsCheck
                // 
                argumentsCheck.AutoSize = true;
                argumentsCheck.Location = new System.Drawing.Point(81, 32);
                argumentsCheck.Name = "argumentsCheck";
                argumentsCheck.Size = new System.Drawing.Size(99, 17);
                argumentsCheck.TabIndex = 3;
                argumentsCheck.Text = "Arguments";
                argumentsCheck.UseVisualStyleBackColor = true;
                toolTip.SetToolTip(argumentsCheck, "Pass arguements to the server on start?");
                // 
                // address
                // 
                address.Location = new System.Drawing.Point(186, 6);
                address.Name = "address";
                address.Size = new System.Drawing.Size(100, 20);
                address.TabIndex = 7;
                toolTip.SetToolTip(address, "What email should the notification be sent to?\n Ex:\n sharkgaming.notifications@gmail.com\n ##########@mms.att.net");
                // 
                // file
                // 
                file.Location = new System.Drawing.Point(81, 79);
                file.Name = "file";
                file.Size = new System.Drawing.Size(100, 20);
                file.TabIndex = 6;
                toolTip.SetToolTip(file, "Click to open file dialog.");
                file.Click += new System.EventHandler(SelectFile);
                // 
                // panel1
                // 
                panel1.BackColor = System.Drawing.Color.Black;
                panel1.Dock = System.Windows.Forms.DockStyle.Fill;
                panel1.Location = new System.Drawing.Point(3, 3);
                panel1.Name = "panel1";
                panel1.Size = new System.Drawing.Size(670, 405);
                panel1.TabIndex = 2;
                // 
                // steamBox
                // 
                steamBox.AutoSize = true;
                steamBox.Location = new System.Drawing.Point(408, 8);
                steamBox.Name = "steamBox";
                steamBox.Size = new System.Drawing.Size(78, 17);
                steamBox.TabIndex = 24;
                steamBox.Text = "Use Steam";
                toolTip.SetToolTip(steamBox, "Uses Steam?");
                steamBox.UseVisualStyleBackColor = true;
                // 
                // SteamLabel
                // 
                SteamLabel.Location = new System.Drawing.Point(492, 6);
                SteamLabel.Name = "SteamLabel";
                SteamLabel.Size = new System.Drawing.Size(100, 20);
                SteamLabel.TabIndex = 22;
                toolTip.SetToolTip(SteamLabel, "Steam Game ID Number");
                // 
                // loginBox
                // 
                loginBox.AutoSize = true;
                loginBox.Location = new System.Drawing.Point(408, 31);
                loginBox.Name = "loginBox";
                loginBox.Size = new System.Drawing.Size(73, 17);
                loginBox.TabIndex = 19;
                loginBox.Text = "Login Info";
                toolTip.SetToolTip(loginBox, "Does server file download require login?");
                loginBox.UseVisualStyleBackColor = true;
                // 
                // user
                // 
                user.Location = new System.Drawing.Point(492, 29);
                user.Name = "user";
                user.Size = new System.Drawing.Size(100, 20);
                user.TabIndex = 21;
                toolTip.SetToolTip(user, "Steam Username");
                // 
                // passw
                // 
                passw.Location = new System.Drawing.Point(492, 53);
                passw.Name = "passw";
                passw.Size = new System.Drawing.Size(100, 20);
                passw.TabIndex = 20;
                toolTip.SetToolTip(passw, "Steam Password");
                // 
                // panel2
                // 
                panel2.BackColor = System.Drawing.Color.Crimson;
                panel2.Controls.Add(steamBox);
                panel2.Controls.Add(start);
                panel2.Controls.Add(reboot);
                panel2.Controls.Add(stop);
                panel2.Controls.Add(file);
                panel2.Controls.Add(notif);
                panel2.Controls.Add(SteamLabel);
                panel2.Controls.Add(passw);
                panel2.Controls.Add(address);
                panel2.Controls.Add(arguments);
                panel2.Controls.Add(loginBox);
                panel2.Controls.Add(user);
                panel2.Controls.Add(argumentsCheck);
                panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
                panel2.Location = new System.Drawing.Point(3, 308);
                panel2.Name = "panel2";
                panel2.Size = new System.Drawing.Size(670, 100);
                panel2.TabIndex = 3;

                tabs.Add(i++, new Server());
                Tab tmp = new Tab();
                tmp.tabPage = tabPage;
                tmp.reboot = reboot;
                tmp.notif = notif;
                tmp.arguments = arguments;
                tmp.argumentsCheck = argumentsCheck;
                tmp.address = address;
                tmp.file = file;
                tmp.panel1 = panel1;
                tmp.steamBox = steamBox;
                tmp.SteamLabel = SteamLabel;
                tmp.loginBox = loginBox;
                tmp.user = user;
                tmp.passw = passw;
                tmp.panel2 = panel2;
                tabs[tabs.Count - 1].tab = tmp;
            }
        }
        private void run(int name)
        {
            ProcessStartInfo test = new ProcessStartInfo();
            //Execution of Processes and Monitoring of them
            test.CreateNoWindow = false;
            test.FileName = tabs[name].tab.file.Text.Replace(@"\", @"\\");
            test.WorkingDirectory = Path.GetDirectoryName(test.FileName);
            if (tabs[name].tab.argumentsCheck.CheckState == CheckState.Checked)
            {
                test.Arguments = tabs[name].tab.arguments.Text;
            }
            //test.Verb = "runas";
            test.WindowStyle = ProcessWindowStyle.Minimized;
            test.UseShellExecute = false;
            Process proc = new Process();
            proc.StartInfo = test;
            try
            {
                do
                {
                    proc.Start();
                    Thread.Sleep(2000);
                    Panel p = tabs[name].tab.panel1;
                    var tmp = proc.MainWindowHandle;
                    p.BeginInvoke((MethodInvoker)delegate () { SetParent(tmp, p.Handle); });
                    //Thread.Sleep(2000);
                    SetWindowPos(tmp, new IntPtr(0), -8, -30, 686, 343, 0x4000);

                    tabs[name].id = proc.Id;
                    Console.WriteLine("Waiting");
                    proc.WaitForExit();
                    if (tabs[name].tab.notif.CheckState == CheckState.Checked)
                    {
                        Check(ref tabs[name].Settings);
                    }
                    if (tabs[name].tab.reboot.CheckState == CheckState.Unchecked)
                    {
                        break;
                    }
                } while (true);
            }
            catch (ThreadAbortException e) //Catches if MainThread terminates childthread, for clean up before actually terminating
            {
                Console.WriteLine("Abort Caught");
                Thread.ResetAbort();
            }
            try { proc.Kill(); } catch { }
            
        }
        
        private IContainer components;
        private ToolTip toolTip;
        private ToolStripMenuItem addSteamToolStripMenuItem;
        private ToolStripMenuItem installGameToolStripMenuItem;
        private ToolStripMenuItem removeTabToolStripMenuItem;
        public object SteamInferface { get; private set; }
    }
    class Server
    {
        public Thread thread;
        public int id;
        public NotifSettings Settings;
        public Tab tab;
    }
    struct Tab
    {
        public TabPage tabPage { get; set; }
        public CheckBox reboot { get; set; }
        public CheckBox notif { get; set; }
        public TextBox arguments { get; set; }
        public CheckBox argumentsCheck { get; set; }
        public TextBox address { get; set; }
        public TextBox file { get; set; }
        public Panel panel1 { get; set; }
        public CheckBox steamBox { get; set; }
        public TextBox SteamLabel { get; set; }
        public CheckBox loginBox { get; set; }
        public TextBox user { get; set; }
        public TextBox passw { get; set; }
        public Panel panel2 { get; set; }
    }
}
