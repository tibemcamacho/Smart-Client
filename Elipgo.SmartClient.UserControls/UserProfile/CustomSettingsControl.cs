using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.UserProfile
{
    public delegate void UpdateUserPreferences(bool existChanges);
    public partial class CustomSettingsControl : UserControl
    {
        public event UpdateUserPreferences UpdateUserPreferences;

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

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        //// End Movable Form
        private Dictionary<string, string> CustomSettings { get; set; }
        private Dictionary<string, string> OldCustomSettings { get; set; }
        private OptionObjectDTO SelectedLanguage { get; set; }
        private string SelectedFolder { get; set; }

        public CustomSettingsControl()
        {
            InitializeComponent();

            this.CustomSettings = WorkFolderUtils.GetDictonaryParams();
            this.OldCustomSettings = new Dictionary<string, string>(CustomSettings);
            DefaultCustomerSettings();

            this.ddlLanguage.OptionSelected += DdlLanguage_OptionSelected;
            this.txbDownloadFolder.Click += TxbDownloadFolder_Click;
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            this.bunifuButtonGuardar.Enabled = true;
            this.grbQuickViewAlarms.SendToBack();
            this.grbSidebar.SendToBack();

            LoadLanguageConfig();

            this.txbDownloadFolder.Text = WorkFolderUtils.GetDefaultLocation();
            this.txbDownloadFolder.ReadOnly = true;
            this.txbDownloadFolder.Cursor = Cursors.Hand;
            this.txbDownloadFolder.SendToBack();

            int.TryParse(CustomSettings["takeObj"], out int userSidebarElements);
            int[] sidebarElementsList = { 10, 15, 20, 30, 40 };
            int takeObjIndexSelected = Array.IndexOf(sidebarElementsList, userSidebarElements);
            this.ddlSidebarElements.DataSource = sidebarElementsList;
            this.ddlSidebarElements.SelectedIndex = takeObjIndexSelected == -1 ? 0 : takeObjIndexSelected;
            this.ddlSidebarElements.SendToBack();
            this.lblStatusDev.Text = Resources.DeviceStatus;

            var stringStatus = CustomSettings["VerifyStatus"];
            bool.TryParse(stringStatus, out bool preResult);
            this.switchDevStatus.Value = !string.IsNullOrEmpty(stringStatus) && preResult;

            stringStatus = CustomSettings["ShowPanelQuickView"];
            bool.TryParse(stringStatus, out preResult);
            this.switchQViewAlarms.Value = string.IsNullOrEmpty(stringStatus) || preResult;

            int.TryParse(CustomSettings["TakeAlarmsQuickView"], out int qViewAlarmsElements);
            int[] qViewAlarmsElementsList = { 3, 5, 10 };
            int takeAlarmsIndexSelected = Array.IndexOf(qViewAlarmsElementsList, qViewAlarmsElements);
            this.ddlQViewAlarms.DataSource = qViewAlarmsElementsList;
            this.ddlQViewAlarms.SelectedIndex = takeAlarmsIndexSelected == -1 ? 0 : takeAlarmsIndexSelected;
            this.ddlQViewAlarms.SendToBack();
        }

        private void LoadLanguageConfig()
        {
            var listLanguages = new List<OptionObjectDTO>() {
                  new OptionObjectDTO() { Name = "en-US", Key = "English", Item = Properties.Resources.language_en },
                  new OptionObjectDTO() { Name = "es-ES", Key = "Espanish", Item = Properties.Resources.language_es }
                };
            var userLanguage = this.CustomSettings["UserLanguage"];
            listLanguages = listLanguages.OrderByDescending(x => x.Name == userLanguage).ToList();
            var currentLanguaje = listLanguages.FirstOrDefault();
            this.SelectedLanguage = currentLanguaje;
            this.picBoxLanguaje.Image = currentLanguaje.Item as Image;
            this.ddlLanguage.SetOptionsWhitImage(listLanguages);
            SetSizeDdlLanguage();
        }

        private void SetSizeDdlLanguage()
        {
            this.ddlLanguage.MinimumSize = new Size(this.ddlLanguage.Width, this.ddlLanguage.Height);
            var maxHeigthDDL = this.ddlLanguage.GetMaxHeigth();
            this.ddlLanguage.MaximumSize = new Size(this.ddlLanguage.Width, maxHeigthDDL);
        }


        private void TxbDownloadFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                Description = Resources.DescriptionDownload + " " + this.txbDownloadFolder.Text
            };
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (folderDlg.SelectedPath != this.txbDownloadFolder.Text)
                {
                    if (MessageBox.Show(Resources.FolderBrowserConfirm + " \"" + folderDlg.SelectedPath + "\"?", Resources.FolderBrowser, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.txbDownloadFolder.Text = folderDlg.SelectedPath;
                    }
                }
            }
        }

        private void DdlLanguage_OptionSelected(object sender, OptionObjectDTO e)
        {
            this.picBoxLanguaje.Image = e.Item as Image;
            SelectedLanguage = e;
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            ((Form)this.TopLevelControl).Close();
        }

        private void bunifuButtonGuardar_Click(object sender, EventArgs e)
        {
            CustomSettings["UserLanguage"] = SelectedLanguage.Name;

            CustomSettings["DefaultLocation"] = txbDownloadFolder.Text;
            ////La path sigue tomandose de setting en la mayoria de las referencias
            Settings.Default["DefaultLocation"] = CustomSettings["DefaultLocation"];

            CustomSettings["takeObj"] = ddlSidebarElements.SelectedValue.ToString();
            CustomSettings["VerifyStatus"] = switchDevStatus.Value.ToString();
            CustomSettings["TakeAlarmsQuickView"] = ddlQViewAlarms.SelectedValue.ToString();
            CustomSettings["ShowPanelQuickView"] = switchQViewAlarms.Value.ToString();
            WorkFolderUtils.SetDictonaryParams(CustomSettings);

            var existChanges = !CustomSettings.SequenceEqual(OldCustomSettings);
            this.UpdateUserPreferences(existChanges);
            ((Form)this.TopLevelControl).Close();
        }

        private void bunifuButtonCancelar_Click(object sender, EventArgs e)
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

        private void DefaultCustomerSettings()
        {
            var config = SmartClientEnvironmentUtils.GetConfiguration();
            CultureInfo ci = CultureInfo.InstalledUICulture;
            Dictionary<string, string> defaultSettings = new Dictionary<string, string>
            {
                { "UserLanguage", ci.Name },
                { "DefaultLocation", Settings.Default["DefaultLocation"].ToString() },
                { "takeObj", config.AppSettings.Settings["takeObj"].Value },
                { "VerifyStatus", config.AppSettings.Settings["VerifyStatus"].Value },
                { "TakeAlarmsQuickView",  config.AppSettings.Settings["TakeAlarmsQuickView"].Value },
                { "ShowPanelQuickView", config.AppSettings.Settings["ShowPanelQuickView"].Value },
            };

            foreach (var setting in defaultSettings)
            {
                if (!CustomSettings.ContainsKey(setting.Key))
                {
                    CustomSettings.Add(setting.Key, setting.Value);
                }
            }
        }
    }
}
