using Elipgo.SmartClient.Common.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.UserProfile
{
    public partial class ResourceMonitor : UserControl
    {

        // Movable Form
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        List<float> CPUpoints;
        List<float> RAMpoints;
        List<float> Diskpoints;
        public ResourceMonitor()
        {
            InitializeComponent();
            LoadConfiguration(true);
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            ((Form)this.TopLevelControl).Close();
        }

        private void lblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Parent.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void PanelHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Parent.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void LoadConfiguration(bool configMode)
        {
            this.lblTitle.Text = Resources.ResourceMonitor;

            CPUpoints = new List<float>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            RAMpoints = new List<float>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Diskpoints = new List<float>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        }

        private void tmrMonitor_Tick(object sender, EventArgs e)
        {
            //float cpuOs = pfcCPU.NextValue();
            float cpuSc = pfCPUSC.NextValue();
            float diskSc = pfcDisk.NextValue();

            //lblCpuOs.Text = String.Format("{0:0.00} %", cpuOs);
            lblCpuSc.Text = String.Format("{0:0.00} %", cpuSc);
            lblDiskSc.Text = String.Format("{0:0.00} %", diskSc);

            CPUpoints.RemoveAt(0);
            CPUpoints.Add(cpuSc);
            Diskpoints.RemoveAt(0);
            Diskpoints.Add(diskSc);

            chrMonitorCPU.Series["CPU"].Points.Clear();
            chrMonitorDisk.Series["DISK"].Points.Clear();

            for (int i = 0; i < 11; i++)
            {
                float pointCpu = CPUpoints[i];
                float pointDisk = Diskpoints[i];
                chrMonitorCPU.Series["CPU"].Points.AddXY(i, pointCpu);
                chrMonitorDisk.Series["DISK"].Points.AddXY(i, pointDisk);
            }

            Process myProcess = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).FirstOrDefault();
            if (myProcess != null)
            {
                float ramSc = (myProcess.PrivateMemorySize64 / 1024) / 1024;
                lblRamSc.Text = String.Format("{0} MBs", ramSc);

                RAMpoints.RemoveAt(0);
                RAMpoints.Add(ramSc);
                chrMonitorRAM.Series["RAM"].Points.Clear();
                int j = 0;
                foreach (var point in RAMpoints)
                {
                    chrMonitorRAM.Series["RAM"].Points.AddXY(j, point);
                    j++;
                }
            }
        }
    }
}
