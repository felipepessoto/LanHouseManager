using System;
using System.Runtime.InteropServices;

namespace LanManager.OSNativeUtils.Security.Windows
{
    internal class Desktop
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        private Desktop() { }

        protected static IntPtr Handle
        {
            get
            {
                return FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Progman", null);
            }
        }

        public static void Desativar()
        {
            ShowWindow(Handle, SW_SHOW);
        }

        public static void Ativar()
        {
            ShowWindow(Handle, SW_HIDE);
        }
    }
}