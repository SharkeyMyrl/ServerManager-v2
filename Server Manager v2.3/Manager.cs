using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Console;
using static ServerManager.Notifications;
using static ServerManager.SteamInterface;


namespace ServerManager
{
    partial class Manager
    {
        [STAThread]
        public static void Main(String[] args)
        {
            Thread main = Thread.CurrentThread;
            main.Name = "MainThread";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ServerManager());
        }
    }
}
