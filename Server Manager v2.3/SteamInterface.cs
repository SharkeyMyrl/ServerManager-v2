using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;

using static System.Console;
using static ServerManager.Notifications;
using static ServerManager.Manager;
using System.Windows.Forms;

namespace ServerManager
{
    static partial class SteamInterface
    {
        public static bool Install()
        {
            //Check if it is installed
            if (Directory.Exists(@".\steamcmd"))
            {
                MessageBox.Show("SteamCMD already installed.", "SteamCMD Installer", MessageBoxButtons.OK);
                return false;
            }

            //Download and Unzip
            using (WebClient Client = new WebClient()) {
                Client.DownloadFile("https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip", Directory.GetCurrentDirectory() + @"\steamcmd.zip");
            }
            ZipFile.ExtractToDirectory(@".\steamcmd.zip", @".\steamcmd\");

            //First time run
            ProcessStartInfo steam = new ProcessStartInfo();
            steam.Arguments = "+quit";
            steam.CreateNoWindow = false;
            steam.WindowStyle = ProcessWindowStyle.Hidden;
            steam.FileName = @".\steamcmd\steamcmd.exe";
            Process.Start(steam);
            
            return true;
        }

        //Update Game and Mods
        public static bool Update(string user, string pass, int game)
        {
            ProcessStartInfo steam = new ProcessStartInfo();
            steam.Arguments = "+login " +user+  " " +pass+ " +app_update " +game+ " +quit";
            steam.CreateNoWindow = false;
            steam.WindowStyle = ProcessWindowStyle.Hidden;
            steam.FileName = @".\steamcmd\steamcmd.exe";
            return true;
        }

        //Move Mods
        public static bool MoveMods(int game, string dest, params string[] args)
        {
            foreach (string j in args)
            {
                DirectoryInfo source = new DirectoryInfo(Directory.GetCurrentDirectory() + @".\steamcmd\steamapps\workshop\content\" +game+ @"\" +j);
                DirectoryInfo target = new DirectoryInfo(dest+ @"\" +j);
                CopyAll(source, target);
            }
            return true;
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
