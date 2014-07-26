using System;
using Microsoft.Win32;

namespace LanManager.OSNativeUtils.Security.Windows
{
    internal class Explorer
    {
        private const string explorerSubKey = @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";

        public static void Ativar()
        {
            const string keyValueInt = "67108863";

            try
            {
                RegistryKey regkey = Registry.CurrentUser.CreateSubKey(explorerSubKey);
                regkey.SetValue("NoDrives", keyValueInt, RegistryValueKind.DWord);
                regkey.SetValue("NoViewOnDrive", keyValueInt, RegistryValueKind.DWord);
                regkey.Close();
            }
            catch{}
        }

        public static void Desativar()
        {
            try { Registry.CurrentUser.DeleteSubKeyTree(explorerSubKey); }
            catch { }
        }
    }
}
