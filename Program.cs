using System.Threading;
using System.Windows.Forms;

namespace MainScreenCurtain;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        using var mutex = new Mutex(
            initiallyOwned: true,
            name: @"Local\MainScreenCurtain",
            createdNew: out bool createdNew);

        if (!createdNew)
            return;

        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new CurtainForm());
    }
}
