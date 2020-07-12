using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cream
{

    public enum State
    {
        Recording,
        NotRecording
    }

    public partial class Cream : Form
    {
        internal const int CTRL_C_EVENT = 0;
        [DllImport("kernel32.dll")]
        internal static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AttachConsole(uint dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool FreeConsole();
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);
        delegate Boolean ConsoleCtrlDelegate(uint CtrlType);

        DefaultSetting setting;

        private State state = State.NotRecording;
        private Process process = null;


        public Cream()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            setting = DefaultSetting.GetDefaultSetting();
        }

        private void Cream_Load(object sender, EventArgs e)
        {

        }

        private void buttonRecord_Click(object sender, EventArgs e)
        {
            if(state==State.Recording)
            {
                (sender as Button).Text = "Start";
                state = State.NotRecording;
                if (process != null)
                {
                    if (AttachConsole((uint)process.Id))
                    {
                        SetConsoleCtrlHandler(null, true);
                        try
                        {
                            if (!GenerateConsoleCtrlEvent(CTRL_C_EVENT, 0))
                                return;
                            process.WaitForExit();
                        }
                        finally
                        {
                            FreeConsole();
                            SetConsoleCtrlHandler(null, false);
                        }
                        return;
                    }
                }
            }
            else
            {
                (sender as Button).Text = "Stop";
                state = State.Recording;
                ProcessStartInfo psi = new ProcessStartInfo("ffmpeg");
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                string fileName = "creamy_" + DateTime.Now.Ticks + ".mp4";
                psi.Arguments = "-f gdigrab -i desktop -vcodec libx264 " + setting.Location + fileName;
                psi.UseShellExecute = false;
                process = Process.Start(psi);
            }
        }

        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            Process.Start(setting.Location);
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            new Settings().Show();
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            new About().Show();
        }
    }
}
