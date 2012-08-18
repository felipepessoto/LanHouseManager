namespace LanManager.OSNativeUtils.Security.Windows
{
    public class WinSecureEnvironment
    {
        public static void Ativar()
        {
            //Desktop.Ativar();
            //Taskbar.Ativar();
            TaskManager.Ativar();
            //WinHotKeys.Ativar();
            Explorer.Ativar();
        }

        public static void Desativar()
        {
            //Desktop.Desativar();
            //Taskbar.Desativar();
            TaskManager.Desativar();
            //WinHotKeys.Desativar();
            Explorer.Desativar();
        }
    }
}