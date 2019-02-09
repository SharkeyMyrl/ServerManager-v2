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
        private IComponent components;
        private MenuStrip menuStrip;
        private ToolStripMenuItem toolStripTextBox;
        private TabControl tabControl;
        private OpenFileDialog openFileDialog;
        private CheckBox reboot;
        private CheckBox safeClose;
        private CheckBox notif;
        private CheckBox checkBox4;
        private TextBox file;
        private TextBox address;
        private TextBox shutdown;
        private Button stop;
        private Button start;
        private Label label1;
        private Label label2;
        private Label label3;
        private TabPage tabPage;
        private Panel panel1;
        private Panel panel2;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerManager));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolStripTextBox = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.menuStrip.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox});
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
            // tabControl
            // 
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(684, 437);
            this.tabControl.TabIndex = 0;
            // 
            // FormManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "FormManager";
            this.Text = "Server Manager v2.1";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        //Have START and STOP base the index of server as the tab index

        private void Start(object sender, EventArgs e)
        {
            Console.WriteLine("Starting");
            int i = tabControl.SelectedIndex;
            ThreadStart tmp = new ThreadStart(() => run(i));
            Thread thread = new Thread(tmp);

            tabs[tabControl.SelectedIndex].thread = thread;
            tabs[tabControl.SelectedIndex].file = file;
            thread.Start();
        }

        private void Stop(object sender, EventArgs e)
        {
            Console.WriteLine("Stopping");
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
                tabs[tabControl.SelectedIndex].file.Text = openFileDialog.FileName;
            }
        }

        private void AddTab(object sender, EventArgs e)
        {
            if (tabControl.TabCount == Environment.ProcessorCount)
            {
                MessageBox.Show("Max tabs reached for your device.", "Safety Sake", MessageBoxButtons.OK);
            }
            else
            {
                Console.WriteLine("Adding tab");
                this.reboot = new System.Windows.Forms.CheckBox();
                this.safeClose = new System.Windows.Forms.CheckBox();
                this.notif = new System.Windows.Forms.CheckBox();
                this.checkBox4 = new System.Windows.Forms.CheckBox();
                this.file = new System.Windows.Forms.TextBox();
                this.address = new System.Windows.Forms.TextBox();
                this.shutdown = new System.Windows.Forms.TextBox();
                this.stop = new System.Windows.Forms.Button();
                this.start = new System.Windows.Forms.Button();
                this.label1 = new System.Windows.Forms.Label();
                this.label2 = new System.Windows.Forms.Label();
                this.label3 = new System.Windows.Forms.Label();
                this.tabPage = new System.Windows.Forms.TabPage();
                this.panel1 = new System.Windows.Forms.Panel();
                this.panel2 = new System.Windows.Forms.Panel();
                this.panel1.SuspendLayout();
                this.panel2.SuspendLayout();

                tabControl.Controls.Add(tabPage);

                // 
                // tabPage
                // 
                this.tabPage.Controls.Add(this.panel1);
                this.tabPage.Controls.Add(this.panel2);
                this.tabPage.Location = new System.Drawing.Point(4, 22);
                this.tabPage.Name = "tabPage";
                this.tabPage.Padding = new System.Windows.Forms.Padding(3);
                this.tabPage.Size = new System.Drawing.Size(676, 411);
                this.tabPage.TabIndex = 0;
                this.tabPage.Text = "tabPage";
                this.tabPage.UseVisualStyleBackColor = true;
                // 
                // panel
                // 
                this.panel2.Controls.Add(this.label3);
                this.panel2.Controls.Add(this.label2);
                this.panel2.Controls.Add(this.label1);
                this.panel2.Controls.Add(this.shutdown);
                this.panel2.Controls.Add(this.address);
                this.panel2.Controls.Add(this.file);
                this.panel2.Controls.Add(this.checkBox4);
                this.panel2.Controls.Add(this.notif);
                this.panel2.Controls.Add(this.safeClose);
                this.panel2.Controls.Add(this.reboot);
                this.panel2.Controls.Add(this.start);
                this.panel2.Controls.Add(this.stop);
                this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
                this.panel2.Location = new System.Drawing.Point(3, 258);
                this.panel2.Name = "panel";
                this.panel2.Size = new System.Drawing.Size(670, 150);
                this.panel2.TabIndex = 1;
                // 
                // start
                // 
                this.start.Location = new System.Drawing.Point(3, 124);
                this.start.Name = "start";
                this.start.Size = new System.Drawing.Size(75, 23);
                this.start.TabIndex = 1;
                this.start.Text = "Start";
                this.start.UseVisualStyleBackColor = true;
                this.start.Click += new System.EventHandler(this.Start);
                // 
                // stop
                // 
                this.stop.Location = new System.Drawing.Point(590, 122);
                this.stop.Name = "stop";
                this.stop.Size = new System.Drawing.Size(75, 23);
                this.stop.TabIndex = 0;
                this.stop.Text = "Stop";
                this.stop.UseVisualStyleBackColor = true;
                this.stop.Click += new System.EventHandler(this.Stop);
                // 
                // label3
                // 
                this.label3.Location = new System.Drawing.Point(357, 111);
                this.label3.Name = "label3";
                this.label3.Size = new System.Drawing.Size(75, 20);
                this.label3.TabIndex = 10;
                this.label3.Text = "notif email";
                // 
                // label2
                // 
                this.label2.Location = new System.Drawing.Point(357, 85);
                this.label2.Name = "label2";
                this.label2.Size = new System.Drawing.Size(75, 20);
                this.label2.TabIndex = 11;
                this.label2.Text = "shutdown CMD";
                // 
                // label1
                // 
                this.label1.Location = new System.Drawing.Point(357, 59);
                this.label1.Name = "label1";
                this.label1.Size = new System.Drawing.Size(75, 20);
                this.label1.TabIndex = 9;
                this.label1.Text = "EXE";
                // 
                // shutdown
                // 
                this.shutdown.Location = new System.Drawing.Point(251, 111);
                this.shutdown.Name = "shutdown";
                this.shutdown.Size = new System.Drawing.Size(100, 20);
                this.shutdown.TabIndex = 8;
                // 
                // address
                // 
                this.address.Location = new System.Drawing.Point(251, 85);
                this.address.Name = "address";
                this.address.Size = new System.Drawing.Size(100, 20);
                this.address.TabIndex = 7;
                // 
                // file
                // 
                this.file.Location = new System.Drawing.Point(251, 59);
                this.file.Name = "file";
                this.file.Size = new System.Drawing.Size(100, 20);
                this.file.TabIndex = 6;
                this.file.Click += new System.EventHandler(this.SelectFile);
                // 
                // checkBox4
                // 
                this.checkBox4.AutoSize = true;
                this.checkBox4.Location = new System.Drawing.Point(352, 29);
                this.checkBox4.Name = "checkBox4";
                this.checkBox4.Size = new System.Drawing.Size(80, 17);
                this.checkBox4.TabIndex = 5;
                this.checkBox4.Text = "checkBox4";
                this.checkBox4.UseVisualStyleBackColor = true;
                // 
                // notif
                // 
                this.notif.AutoSize = true;
                this.notif.Location = new System.Drawing.Point(352, 6);
                this.notif.Name = "notif";
                this.notif.Size = new System.Drawing.Size(46, 17);
                this.notif.TabIndex = 4;
                this.notif.Text = "notif";
                this.notif.UseVisualStyleBackColor = true;
                // 
                // safeClose
                // 
                this.safeClose.AutoSize = true;
                this.safeClose.Location = new System.Drawing.Point(251, 29);
                this.safeClose.Name = "safeClose";
                this.safeClose.Size = new System.Drawing.Size(72, 17);
                this.safeClose.TabIndex = 3;
                this.safeClose.Text = "safeClose";
                this.safeClose.UseVisualStyleBackColor = true;
                // 
                // reboot
                // 
                this.reboot.AutoSize = true;
                this.reboot.Location = new System.Drawing.Point(251, 6);
                this.reboot.Name = "reboot";
                this.reboot.Size = new System.Drawing.Size(56, 17);
                this.reboot.TabIndex = 2;
                this.reboot.Text = "reboot";
                this.reboot.UseVisualStyleBackColor = true;
                // 
                // panel1
                // 
                this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
                this.panel1.Location = new System.Drawing.Point(0, 0);
                this.panel1.Name = "panel1";
                this.panel1.Size = new System.Drawing.Size(670, 255);
                this.panel1.TabIndex = 2;

                tabControl.SelectedIndex = tabControl.TabCount - 1;

                tabs.Add(tabs.Count, new Server());
                tabs[tabs.Count - 1].tabPage = tabPage;
                tabs[tabs.Count - 1].file = file;
                tabs[tabs.Count - 1].reboot = reboot;
                tabs[tabs.Count - 1].notif = notif;
                tabs[tabs.Count - 1].safeClose = safeClose;
                tabs[tabs.Count - 1].address = address;
                tabs[tabs.Count - 1].shutdown = shutdown;

            }
        }


        private void run(int name)
        {
            ProcessStartInfo test = new ProcessStartInfo();
            //Execution of Processes and Monitoring of them
            //test.RedirectStandardOutput = true;
            test.CreateNoWindow = false;
            test.FileName = tabs[name].file.Text.Replace(@"\", @"\\");
            test.WorkingDirectory = Path.GetDirectoryName(test.FileName);
            //test.Arguments = "-nogaphics -batchmode +secureserver/Server 1";
            test.Verb = "runas";
            test.WindowStyle = ProcessWindowStyle.Maximized;
            //test.UseShellExecute = false;

            Process proc = Process.Start(test);
            Thread.Sleep(100);
            panel1.BeginInvoke((MethodInvoker)delegate () { SetParent(proc.MainWindowHandle, panel1.Handle); });





            //tabs[name].richTextBox.Text = proc.StandardOutput.ReadToEnd();

            tabs[name].id = proc.Id;


            try
            {
                while (true)
                {
                    Console.WriteLine("Waiting for Close");
                    proc.WaitForExit();
                    if (tabs[name].notif.CheckState == CheckState.Checked)
                    {
                        Check(ref tabs[name].Settings);
                    }

                    if (tabs[name].reboot.CheckState == CheckState.Unchecked)
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

            if (tabs[name].safeClose.CheckState == CheckState.Checked)
            {
                try
                {
                    proc.StandardInput.WriteLine(shutdown); //Should send the shutdown string to the game console
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
    }

    class Server
    {
        public Thread thread;
        public int id;
        public TextBox file;
        public Boolean foreground;
        public NotifSettings Settings;

        public TabPage tabPage;
        public RichTextBox richTextBox;
        public CheckBox reboot;
        public CheckBox window;
        public CheckBox notif;
        public CheckBox safeClose;
        public TextBox address;
        public TextBox shutdown;
    }
}
