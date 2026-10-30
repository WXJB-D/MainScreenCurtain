using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace MainScreenCurtain;

internal sealed class CurtainForm : Form
{
    private const int ArmDelayMs = 700;
    private const int RefitDelayMs = 300;

    private readonly Stopwatch _sinceShown = Stopwatch.StartNew();
    private readonly System.Windows.Forms.Timer _refitTimer = new() { Interval = RefitDelayMs };
    private readonly string _primaryDevice;

    public CurtainForm()
    {
        MonitorInfo primary = GetPrimaryMonitor();

        _primaryDevice = primary.DeviceName;

        AutoScaleMode = AutoScaleMode.None;
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.Manual;
        Bounds = primary.Bounds;
        BackColor = Color.Black;
        TopMost = true;
        ShowInTaskbar = false;
        MinimizeBox = false;
        MaximizeBox = false;
        DoubleBuffered = true;

        _refitTimer.Tick += (_, _) => Refit();
    }

    protected override bool ShowWithoutActivation => true;

    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams parameters = base.CreateParams;
            parameters.ExStyle |= NativeMethods.WS_EX_TOOLWINDOW | NativeMethods.WS_EX_NOACTIVATE;
            return parameters;
        }
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        _sinceShown.Restart();
        ReassertTopMost();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && _sinceShown.ElapsedMilliseconds >= ArmDelayMs)
        {
            Close();
            return;
        }

        base.OnMouseDown(e);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);

        if (WindowState == FormWindowState.Minimized)
            Close();
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == NativeMethods.WM_DISPLAYCHANGE)
        {
            _refitTimer.Stop();
            _refitTimer.Start();
        }

        base.WndProc(ref m);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _refitTimer.Stop();
        _refitTimer.Dispose();
        base.OnFormClosing(e);
    }

    private void Refit()
    {
        _refitTimer.Stop();

        MonitorInfo primary = GetPrimaryMonitor();

        if (_primaryDevice.Length > 0 &&
            primary.DeviceName.Length > 0 &&
            !string.Equals(_primaryDevice, primary.DeviceName, StringComparison.OrdinalIgnoreCase))
        {
            Close();
            return;
        }

        Bounds = primary.Bounds;
        ReassertTopMost();
    }

    private void ReassertTopMost()
    {
        NativeMethods.SetWindowPos(
            Handle,
            NativeMethods.HWND_TOPMOST,
            0,
            0,
            0,
            0,
            NativeMethods.SWP_NOMOVE | NativeMethods.SWP_NOSIZE | NativeMethods.SWP_NOACTIVATE);
    }

    private static MonitorInfo GetPrimaryMonitor()
    {
        MonitorInfo monitor = NativeMethods.GetPrimaryMonitor();

        if (monitor.Bounds.Width <= 0 || monitor.Bounds.Height <= 0)
        {
            Rectangle bounds = Screen.PrimaryScreen!.Bounds;
            monitor = new MonitorInfo(bounds, string.Empty);
        }

        return monitor;
    }
}
