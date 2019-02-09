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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage = new System.Windows.Forms.TabPage();
            this.stop = new System.Windows.Forms.Button();
            this.start = new System.Windows.Forms.Button();
            this.reboot = new System.Windows.Forms.CheckBox();
            this.notif = new System.Windows.Forms.CheckBox();
            this.shutdown = new System.Windows.Forms.TextBox();
            this.safeClose = new System.Windows.Forms.CheckBox();
            this.address = new System.Windows.Forms.TextBox();
            this.file = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.steamBox = new System.Windows.Forms.CheckBox();
            this.SteamLabel = new System.Windows.Forms.TextBox();
            this.loginBox = new System.Windows.Forms.CheckBox();
            this.user = new System.Windows.Forms.TextBox();
            this.passw = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.menuStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox,
            this.addSteamToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(684, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
            // 
            // toolStripTextBox
            // 
            this.toolStripTextBox.Name = "toolStripTextBox";
            this.toolStripTextBox.Size = new System.Drawing.Size(63, 20);
            this.toolStripTextBox.Text = "Add Tab";
            this.toolStripTextBox.Click += new System.EventHandler(this.AddTab);
            // 
            // addSteamToolStripMenuItem
            // 
            this.addSteamToolStripMenuItem.Name = "addSteamToolStripMenuItem";
            this.addSteamToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.addSteamToolStripMenuItem.Text = "Add Steam";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(684, 437);
            this.tabControl.TabIndex = 0;
            // 
            // tabPage
            // 
            this.tabPage.BackColor = System.Drawing.Color.Cyan;
            this.tabPage.Controls.Add(this.panel2);
            this.tabPage.Controls.Add(this.panel1);
            this.tabPage.Location = new System.Drawing.Point(4, 22);
            this.tabPage.Name = "tabPage";
            this.tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage.Size = new System.Drawing.Size(676, 411);
            this.tabPage.TabIndex = 0;
            this.tabPage.Text = "tabPage";
            // 
            // stop
            // 
            this.stop.Location = new System.Drawing.Point(595, 77);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(75, 23);
            this.stop.TabIndex = 0;
            this.stop.Text = "Stop";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.Stop);
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(0, 77);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(75, 23);
            this.start.TabIndex = 1;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.Start);
            // 
            // reboot
            // 
            this.reboot.AutoSize = true;
            this.reboot.Location = new System.Drawing.Point(81, 53);
            this.reboot.Name = "reboot";
            this.reboot.Size = new System.Drawing.Size(56, 17);
            this.reboot.TabIndex = 2;
            this.reboot.Text = "reboot";
            this.reboot.UseVisualStyleBackColor = true;
            // 
            // notif
            // 
            this.notif.AutoSize = true;
            this.notif.Location = new System.Drawing.Point(81, 9);
            this.notif.Name = "notif";
            this.notif.Size = new System.Drawing.Size(84, 17);
            this.notif.TabIndex = 4;
            this.notif.Text = "Notifications";
            this.notif.UseVisualStyleBackColor = true;
            // 
            // shutdown
            // 
            this.shutdown.Location = new System.Drawing.Point(186, 6);
            this.shutdown.Name = "shutdown";
            this.shutdown.Size = new System.Drawing.Size(100, 20);
            this.shutdown.TabIndex = 8;
            // 
            // safeClose
            // 
            this.safeClose.AutoSize = true;
            this.safeClose.Location = new System.Drawing.Point(81, 32);
            this.safeClose.Name = "safeClose";
            this.safeClose.Size = new System.Drawing.Size(99, 17);
            this.safeClose.TabIndex = 3;
            this.safeClose.Text = "Safe Shutdown";
            this.safeClose.UseVisualStyleBackColor = true;
            // 
            // address
            // 
            this.address.Location = new System.Drawing.Point(186, 30);
            this.address.Name = "address";
            this.address.Size = new System.Drawing.Size(100, 20);
            this.address.TabIndex = 7;
            // 
            // file
            // 
            this.file.Location = new System.Drawing.Point(81, 79);
            this.file.Name = "file";
            this.file.Size = new System.Drawing.Size(100, 20);
            this.file.TabIndex = 6;
            this.toolTip.SetToolTip(this.file, "Click to open file dialog.");
            this.file.Click += new System.EventHandler(this.SelectFile);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(670, 405);
            this.panel1.TabIndex = 2;
            // 
            // steamBox
            // 
            this.steamBox.AutoSize = true;
            this.steamBox.Location = new System.Drawing.Point(408, 8);
            this.steamBox.Name = "steamBox";
            this.steamBox.Size = new System.Drawing.Size(78, 17);
            this.steamBox.TabIndex = 24;
            this.steamBox.Text = "Use Steam";
            this.toolTip.SetToolTip(this.steamBox, "Does server file download require login?");
            this.steamBox.UseVisualStyleBackColor = true;
            // 
            // SteamLabel
            // 
            this.SteamLabel.Location = new System.Drawing.Point(492, 6);
            this.SteamLabel.Name = "SteamLabel";
            this.SteamLabel.Size = new System.Drawing.Size(100, 20);
            this.SteamLabel.TabIndex = 22;
            this.toolTip.SetToolTip(this.SteamLabel, "Steam Game ID Number");
            // 
            // loginBox
            // 
            this.loginBox.AutoSize = true;
            this.loginBox.Location = new System.Drawing.Point(408, 31);
            this.loginBox.Name = "loginBox";
            this.loginBox.Size = new System.Drawing.Size(73, 17);
            this.loginBox.TabIndex = 19;
            this.loginBox.Text = "Login Info";
            this.toolTip.SetToolTip(this.loginBox, "Does server file download require login?");
            this.loginBox.UseVisualStyleBackColor = true;
            // 
            // user
            // 
            this.user.Location = new System.Drawing.Point(492, 29);
            this.user.Name = "user";
            this.user.Size = new System.Drawing.Size(100, 20);
            this.user.TabIndex = 21;
            this.toolTip.SetToolTip(this.user, "Steam Username");
            // 
            // passw
            // 
            this.passw.Location = new System.Drawing.Point(492, 53);
            this.passw.Name = "passw";
            this.passw.Size = new System.Drawing.Size(100, 20);
            this.passw.TabIndex = 20;
            this.toolTip.SetToolTip(this.passw, "Steam Password");
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Crimson;
            this.panel2.Controls.Add(this.steamBox);
            this.panel2.Controls.Add(this.start);
            this.panel2.Controls.Add(this.reboot);
            this.panel2.Controls.Add(this.stop);
            this.panel2.Controls.Add(this.file);
            this.panel2.Controls.Add(this.notif);
            this.panel2.Controls.Add(this.SteamLabel);
            this.panel2.Controls.Add(this.passw);
            this.panel2.Controls.Add(this.address);
            this.panel2.Controls.Add(this.shutdown);
            this.panel2.Controls.Add(this.loginBox);
            this.panel2.Controls.Add(this.user);
            this.panel2.Controls.Add(this.safeClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 308);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(670, 100);
            this.panel2.TabIndex = 3;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
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
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "ServerManager";
            this.Text = "Server Manager v2.0";
            this.Load += new System.EventHandler(this.ServerManager_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        //Have START and STOP base the index of server as the tab index

        private void Start(object sender, EventArgs e)
        {
            
            Console.WriteLine("Starting Index " +tabControl.SelectedIndex);
            int i = tabControl.SelectedIndex;
            ThreadStart tmp = new ThreadStart(() => run(i));
            Thread thread = new Thread(tmp);

            tabs[tabControl.SelectedIndex].thread = thread;
            thread.Start();
        }

        private void Stop(object sender, EventArgs e)
        {
            Console.WriteLine("Stopping Index " +tabControl.SelectedIndex);
            Server tmp = tabs[tabControl.SelectedIndex];
            tmp.thread.Abort();
        }

        //Add text to richtext, switch tabControl.SelectedIndex with thread index
        //tabs[tabControl.SelectedIndex].richTextBox.AppendText(Environment.NewLine + "Hello");


        //Broken Currently
        private void SelectFile(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tabs[tabControl.SelectedIndex].tab.file.Text = openFileDialog.FileName;
            }
        }

        private void AddTab(object sender, EventArgs e)
        {
            String name;
            Boolean steam;
            int id;

            if (tabControl.TabCount == Environment.ProcessorCount)
            {
                MessageBox.Show("Max tabs reached for your device.", "Safety Sake", MessageBoxButtons.OK);
            }
            else
            {
                
                PopUp pu = new PopUp();
                pu.ShowDialog();
                Console.WriteLine("Leaving Dialog");
                if (pu.DialogResult == DialogResult.OK)
                {
                    name = pu.name;
                    steam = pu.steam;
                    id = pu.id;
                    if (steam == true) {
                        Install();
                    }

                    if (id != 0)
                    {
                        //Update(user, passw, id);
                    }
                }
                else
                {
                    return;
                }
                

                Console.WriteLine("Adding tab");
                CheckBox reboot = new System.Windows.Forms.CheckBox();
                CheckBox safeClose = new System.Windows.Forms.CheckBox();
                CheckBox notif = new System.Windows.Forms.CheckBox();
                CheckBox checkBox4 = new System.Windows.Forms.CheckBox();
                TextBox file = new System.Windows.Forms.TextBox();
                TextBox address = new System.Windows.Forms.TextBox();
                TextBox shutdown = new System.Windows.Forms.TextBox();
                Button stop = new System.Windows.Forms.Button();
                Button start = new System.Windows.Forms.Button();
                Label label1 = new System.Windows.Forms.Label();
                Label label2 = new System.Windows.Forms.Label();
                Label label3 = new System.Windows.Forms.Label();
                TabPage tabPage = new System.Windows.Forms.TabPage();
                Panel panel1 = new System.Windows.Forms.Panel();
                Panel panel2 = new System.Windows.Forms.Panel();
                panel1.SuspendLayout();
                panel2.SuspendLayout();

                tabControl.Controls.Add(tabPage);

                // 
                // tabPage
                // 
                tabPage.Controls.Add(panel1);
                tabPage.Controls.Add(panel2);
                tabPage.Location = new System.Drawing.Point(4, 22);
                tabPage.Name = "tabPage";
                tabPage.Padding = new System.Windows.Forms.Padding(3);
                tabPage.Size = new System.Drawing.Size(676, 411);
                tabPage.TabIndex = 0;
                tabPage.Text = "tabPage";
                tabPage.UseVisualStyleBackColor = true;
                // 
                // panel
                // 
                panel2.Controls.Add(label3);
                panel2.Controls.Add(label2);
                panel2.Controls.Add(label1);
                panel2.Controls.Add(shutdown);
                panel2.Controls.Add(address);
                panel2.Controls.Add(file);
                panel2.Controls.Add(checkBox4);
                panel2.Controls.Add(notif);
                panel2.Controls.Add(safeClose);
                panel2.Controls.Add(reboot);
                panel2.Controls.Add(start);
                panel2.Controls.Add(stop);
                panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
                panel2.Location = new System.Drawing.Point(3, 258);
                panel2.Name = "panel";
                panel2.Size = new System.Drawing.Size(670, 150);
                panel2.TabIndex = 1;
                // 
                // start
                // 
                start.Location = new System.Drawing.Point(3, 124);
                start.Name = "start";
                start.Size = new System.Drawing.Size(75, 23);
                start.TabIndex = 1;
                start.Text = "Start";
                start.UseVisualStyleBackColor = true;
                start.Click += new System.EventHandler(Start);
                // 
                // stop
                // 
                stop.Location = new System.Drawing.Point(590, 122);
                stop.Name = "stop";
                stop.Size = new System.Drawing.Size(75, 23);
                stop.TabIndex = 0;
                stop.Text = "Stop";
                stop.UseVisualStyleBackColor = true;
                stop.Click += new System.EventHandler(Stop);
                // 
                // label3
                // 
                label3.Location = new System.Drawing.Point(357, 111);
                label3.Name = "label3";
                label3.Size = new System.Drawing.Size(75, 20);
                label3.TabIndex = 10;
                label3.Text = "notif email";
                // 
                // label2
                // 
                label2.Location = new System.Drawing.Point(357, 85);
                label2.Name = "label2";
                label2.Size = new System.Drawing.Size(75, 20);
                label2.TabIndex = 11;
                label2.Text = "shutdown CMD";
                // 
                // label1
                // 
                label1.Location = new System.Drawing.Point(357, 59);
                label1.Name = "label1";
                label1.Size = new System.Drawing.Size(75, 20);
                label1.TabIndex = 9;
                label1.Text = "EXE";
                // 
                // shutdown
                // 
                shutdown.Location = new System.Drawing.Point(251, 111);
                shutdown.Name = "shutdown";
                shutdown.Size = new System.Drawing.Size(100, 20);
                shutdown.TabIndex = 8;
                // 
                // address
                // 
                address.Location = new System.Drawing.Point(251, 85);
                address.Name = "address";
                address.Size = new System.Drawing.Size(100, 20);
                address.TabIndex = 7;
                // 
                // file
                // 
                file.Location = new System.Drawing.Point(251, 59);
                file.Name = "file";
                file.Size = new System.Drawing.Size(100, 20);
                file.TabIndex = 6;
                file.Click += new System.EventHandler(SelectFile);
                // 
                // checkBox4
                // 
                checkBox4.AutoSize = true;
                checkBox4.Location = new System.Drawing.Point(352, 29);
                checkBox4.Name = "checkBox4";
                checkBox4.Size = new System.Drawing.Size(80, 17);
                checkBox4.TabIndex = 5;
                checkBox4.Text = "checkBox4";
                checkBox4.UseVisualStyleBackColor = true;
                // 
                // notif
                // 
                notif.AutoSize = true;
                notif.Location = new System.Drawing.Point(352, 6);
                notif.Name = "notif";
                notif.Size = new System.Drawing.Size(46, 17);
                notif.TabIndex = 4;
                notif.Text = "notif";
                notif.UseVisualStyleBackColor = true;
                // 
                // safeClose
                // 
                safeClose.AutoSize = true;
                safeClose.Location = new System.Drawing.Point(251, 29);
                safeClose.Name = "safeClose";
                safeClose.Size = new System.Drawing.Size(72, 17);
                safeClose.TabIndex = 3;
                safeClose.Text = "safeClose";
                safeClose.UseVisualStyleBackColor = true;
                // 
                // reboot
                // 
                reboot.AutoSize = true;
                reboot.Location = new System.Drawing.Point(251, 6);
                reboot.Name = "reboot";
                reboot.Size = new System.Drawing.Size(56, 17);
                reboot.TabIndex = 2;
                reboot.Text = "reboot";
                reboot.UseVisualStyleBackColor = true;
                // 
                // panel1
                // 
                panel1.Dock = System.Windows.Forms.DockStyle.Fill;
                panel1.Location = new System.Drawing.Point(0, 0);
                panel1.Name = "panel1";
                panel1.Size = new System.Drawing.Size(670, 255);
                panel1.TabIndex = 2;

                tabControl.SelectedIndex = tabControl.TabCount - 1;

                tabs.Add(tabs.Count, new Server());

                Tab tmp = new Tab();
                tmp.tabPage = tabPage;
                tmp.file = file;
                tmp.reboot = reboot;
                tmp.notif = notif;
                tmp.safeClose = safeClose;
                tmp.address = address;
                tmp.shutdown = shutdown;
                tmp.panel = panel1;

                tabs[tabs.Count - 1].tab = tmp;

            }
        }


        private void run(int name)
        {
            Console.WriteLine("Name = " + name);
            ProcessStartInfo test = new ProcessStartInfo();
            //Execution of Processes and Monitoring of them
            //test.RedirectStandardOutput = true;
            test.CreateNoWindow = false;
            test.FileName = tabs[name].tab.file.Text.Replace(@"\", @"\\");
            Console.WriteLine("Starting " + test.FileName);
            test.WorkingDirectory = Path.GetDirectoryName(test.FileName);
            //test.Arguments = "-nogaphics -batchmode +secureserver/Server 1";
            test.Verb = "runas";
            test.WindowStyle = ProcessWindowStyle.Maximized;
            //test.UseShellExecute = false;

            Process proc = Process.Start(test);
            Thread.Sleep(100);
            Panel p = tabs[name].tab.panel;
            p.BeginInvoke((MethodInvoker)delegate () { SetParent(proc.MainWindowHandle, p.Handle); });





            //tabs[name].richTextBox.Text = proc.StandardOutput.ReadToEnd();

            tabs[name].id = proc.Id;


            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for Close");
                    proc.WaitForExit();
                    if (tabs[name].tab.notif.CheckState == CheckState.Checked)
                    {
                        Check(ref tabs[name].Settings);
                    }

                    if (tabs[name].tab.reboot.CheckState == CheckState.Unchecked)
                    {
                        break;
                    }
                }
            }
            catch (ThreadAbortException e) //Catches if MainThread terminates childthread, for clean up before actually terminating
            {
                Console.WriteLine("Abort Caught");
                Thread.ResetAbort();
            }

            if (tabs[name].tab.safeClose.CheckState == CheckState.Checked)
            {
                try
                {
                    proc.StandardInput.WriteLine(tabs[name].tab.shutdown.Text); //Should send the shutdown string to the game console
                    Console.WriteLine("Shutting down " + name + ".");
                    proc.WaitForExit(60000);
                }
                catch { }
            }
            else
            {
                try { proc.Kill(); } catch { }
            }

        }

        private TabPage tabPage;
        private Button stop;
        private Button start;
        private CheckBox reboot;
        private CheckBox notif;
        private TextBox shutdown;
        private CheckBox safeClose;
        private TextBox address;
        private TextBox file;
        private Panel panel1;
        private IContainer components;
        private ToolTip toolTip;
        private ToolStripMenuItem addSteamToolStripMenuItem;
        private Panel panel2;
        private CheckBox steamBox;
        private TextBox SteamLabel;
        private TextBox passw;
        private CheckBox loginBox;
        private TextBox user;
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
        public TextBox file { get; set; }
        public TabPage tabPage { get; set; }
        public RichTextBox richTextBox { get; set; }
        public CheckBox reboot { get; set; }
        public CheckBox window { get; set; }
        public CheckBox notif { get; set; }
        public CheckBox safeClose { get; set; }
        public TextBox address { get; set; }
        public TextBox shutdown { get; set; }
        public Panel panel { get; set; }
    }
}
