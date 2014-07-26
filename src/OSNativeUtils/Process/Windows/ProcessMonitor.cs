using System;
using System.Runtime.InteropServices;

namespace LanManager.OSNativeUtils.Process.Windows
{
    public class ProcessMonitor
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        public static System.Diagnostics.Process GetForegroundProcess()
        {
            int processId;
            GetWindowThreadProcessId(GetForegroundWindow(), out processId);
            return System.Diagnostics.Process.GetProcessById(processId);
        }
    }
}
