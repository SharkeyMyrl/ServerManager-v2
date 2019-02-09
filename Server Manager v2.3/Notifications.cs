using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Console;
using static ServerManager.SteamInterface;
using static ServerManager.Manager;
using System.Net.Mail;

namespace ServerManager
{
    partial class Notifications
    {
        public static void Check(ref NotifSettings settings)
        {
            int delay = 5;
            DateTime tmp = System.DateTime.Now;
            if (settings.prevDay == 0)
            {
                settings.prevDay = tmp.DayOfYear;
                settings.prevTime = tmp.TimeOfDay.Seconds;
                settings.prevYear = tmp.Year;
            }

            int dayDiff = settings.prevDay - tmp.DayOfYear;
            int timeDiff = settings.prevTime - tmp.TimeOfDay.Seconds;
            int yearDiff = settings.prevYear - tmp.Year;

            if (dayDiff == -1)
            {
                if (timeDiff <= 2359 - delay)
                {
                    Notification(ref settings, 1);
                    return;
                }
                Notification(ref settings, 0);
                return;
            }

            if (dayDiff == 0)
            {
                if (timeDiff <= delay)
                {
                    Notification(ref settings, 1);
                    return;
                }
                Notification(ref settings, 0);
                return;
            }

            if (yearDiff == -1)
            {
                if (dayDiff == new DateTime(year: settings.prevYear, month: 12, day: 30).DayOfYear && timeDiff <= 2359 - delay)
                {
                    Notification(ref settings, 1);
                    return;
                }
                Notification(ref settings, 0);
                return;
            }
            Notification(ref settings, 0);
        }
        public static void Notification(ref NotifSettings settings, int var)
        {
            if (var != 1 && !settings.msgSent)
            {
                MailMessage mail = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential("sharkgaming.Notifications@gmail.com", "rootboot");
                client.UseDefaultCredentials = false;
                client.Host = "smtp.gmail.com";
                mail.To.Add(new MailAddress(settings.address)); // <-- this one
                mail.From = new MailAddress("sharkgaming.Notifications@gmail.com");
                switch (var)
                {
                    case 0: mail.Subject = "Server Crashed/Shutdown"; settings.msgSent = false; break;
                    case 1: mail.Subject = "Server May Be in a Crash Loop"; settings.msgSent = true; break;
                }
                mail.Body = "";
                client.Send(mail);
            }
        }
    }

    public struct NotifSettings
    {
        public String address { get; set; }
        public int prevDay { get; set; }
        public int prevTime { get; set; }
        public int prevYear { get; set; }
        public bool msgSent { get; set; }
    }
}
