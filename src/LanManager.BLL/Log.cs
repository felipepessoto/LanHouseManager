using System;
using System.IO;

namespace LanManager.BLL
{
    public class Log
    {
        public static void WriteAdminLog(string message, string adminLogin)
        {
            try
            {
                if (!Directory.Exists("log"))
                {
                    Directory.CreateDirectory("log");
                }

                using (StreamWriter log = File.AppendText(@"log\" + DateTime.Now.ToString("yyyyMMdd") + "Admin.log"))
                {
                    log.WriteLine(string.Format("{0} - {1} - {2}", adminLogin, DateTime.Now.ToString("dd/MM/yyyy"), message));
                    log.Close();
                }
            }
            catch(Exception ex) { }
        }

        public static void Write(string message)
        {
            try
            {
                using (StreamWriter log = File.CreateText("log.log"))
                {
                    log.WriteLine(string.Format("{0} - {1}", DateTime.Now.ToString("dd/MM/yyyy"), message));
                    log.Close();
                }
            }
            catch { }
        }
    }
}
