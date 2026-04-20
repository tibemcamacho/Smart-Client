using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Drivers;
using Elipgo.SmartClient.Drivers.Joystick;
using Elipgo.SmartClient.UserControls.Joystick;
using Splat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.UserProfile
{
    public delegate void ContainerSelectedHandler(string keyPress);
    public delegate void ChangeDriverHandler(int ContainerSelected);
    public partial class JoystickSettings : UserControl
    {
        public event ContainerSelectedHandler ContainerSelectedEvent;
        public event ChangeDriverHandler ChangeDriverEvent;

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
        private readonly IDriverFactoryJoystick driverFactoryJoystick = Locator.Current.GetService<IDriverFactoryJoystick>();
        IDriverJoystick joystick;
        DateTime? dateWindowsKey = null;
        public bool IsConnected = false;
        //// End Movable Form

        public JoystickSettings()
        {
            joystick = driverFactoryJoystick.GetDriverJoystinck();
            if (joystick != null)
            {
                IsConnected = true;
                InitializeComponent();
                LoadConfiguration(true);
            }

        }

        private void Joystick_ContainerSelectedEvent(string keyPress)
        {
            this.ContainerSelectedEvent(keyPress);
        }

        private void joystick_MappingActionsListView(ListViewItem[] mappingActionsListView)
        {
            try
            {
                actionsMappingListView.Items.Clear();

                foreach (var item in mappingActionsListView)
                {
                    item.BackColor = Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
                    item.ForeColor = Color.Silver;
                    item.Font = new Font("Segoe UI", 8.25F);
                    actionsMappingListView.Items.Add((ListViewItem)item.Clone());
                }

                //actionsMappingListView.Items.AddRange((ListViewItem[])mappingActionsListView.Clone());
                //int selectedIndex = actionsMappingListView.SelectedItems.Count > 0 ? actionsMappingListView.SelectedIndices[0] : 0;
                if (actionsMappingListView.Items.Count > 0)
                    actionsMappingListView.Items[0].Selected = true;
            }
            catch (Exception ex)
            {
                Logger.Log("ex.Message --> " + ex.Message + " ex.StackTrace -->" + ex.StackTrace, LogPriority.Warning);
            }

        }

        private void joystick_JoystickSetting(bool invertXChecked, bool invertYChecked, bool invertZRotationChecked)
        {
            invertXCheckBox.Checked = invertXChecked;
            invertYCheckBox.Checked = invertYChecked;
            invertZRotationCheckBox.Checked = invertZRotationChecked;
        }

        private string CurrentConfiguration;
        private void Load_Configuration(object sender, EventArgs e)
        {
            CurrentConfiguration = (sender as Bunifu.UI.WinForms.BunifuDropdown).SelectedValue.ToString();
            joystick.LoadConfiguration(CurrentConfiguration);
        }

        private void joystickButtonEvent(List<ActionCommand> pressedButton)
        {
            foreach (var it in pressedButton)
            {

                handleButtonPressed(it.buttonOrAxis);
            }
        }

        private void handleButtonPressed(ButtonOrAxis button)
        {
            if (actionsMappingListView.InvokeRequired)
            {
                actionsMappingListView.Invoke((MethodInvoker)delegate
                {
                    handleButtonPressed(button);
                });
                return;
            }
            try
            {
                //Hay algun item seleccionado?
                if (actionsMappingListView.SelectedItems.Count == 0)
                    return;

                //Asigno el boton al ActionCommand seleccionado
                ActionCommand selectedActionCommand = (ActionCommand)joystick.ActionMappingsListModel[actionsMappingListView.SelectedIndices[0]];


                if (selectedActionCommand.buttonOrAxis == ButtonOrAxis.Button15)
                {
                    if (dateWindowsKey == null)
                        dateWindowsKey = DateTime.Now;

                    TimeSpan dp = DateTime.Now.Subtract(Convert.ToDateTime(dateWindowsKey));
                    if (dp.TotalSeconds > 4.1)
                    {
                        dateWindowsKey = null;
                        actionsMappingListView.KeyDown -= new KeyEventHandler(actionsMappingListView_KeyDown);
                        joystick.ConfigMode = false;
                        joystick.JoystickSetting -= joystick_JoystickSetting;
                        joystick.MappingActionsListView -= joystick_MappingActionsListView;
                        joystick.JoystickButtonSettingEvent -= joystickButtonEvent;
                        joystick.Dispose();
                        System.Threading.Tasks.Task.Delay(6000).ContinueWith(t =>
                        {
                            joystick = driverFactoryJoystick.GetDriverJoystinck();
                            LoadConfiguration(false);
                        });
                    }
                }
                else
                    dateWindowsKey = null;

                joystick.SetActionCommand(button, selectedActionCommand);

                //Actualizo la vista
                joystick.UpdateListView();
            }
            catch (Exception)
            {

            }
        }

        private void UpdateView(DriversJoystick driversJoystick)
        {
            switch (driversJoystick)
            {
                case DriversJoystick.COM:
                    pnlContainerUSB.Visible = false;
                    this.Height = 237;
                    if (this.TopLevelControl != null)
                        ((Form)this.TopLevelControl).Height = 237;
                    bunifuButtonCancelar.Location = new Point(bunifuButtonCancelar.Location.X, 160);
                    bunifuButtonGuardar.Location = new Point(bunifuButtonGuardar.Location.X, 160);
                    break;
                case DriversJoystick.USB:
                    pnlContainerUSB.Visible = true;
                    this.Height = 537;
                    if (this.TopLevelControl != null)
                        ((Form)this.TopLevelControl).Height = 537;
                    bunifuButtonCancelar.Location = new Point(bunifuButtonCancelar.Location.X, 485);
                    bunifuButtonGuardar.Location = new Point(bunifuButtonGuardar.Location.X, 485);
                    break;
                default:
                    break;


            }
        }

        private void bunifuButtonGuardar_Click(object sender, EventArgs e)
        {
            dateWindowsKey = null;
            if (string.IsNullOrEmpty(CurrentConfiguration))
            {
                CurrentConfiguration = DropDownConfig.SelectedValue.ToString();
                joystick.LoadConfiguration(CurrentConfiguration);
            }

            if (this.CurrentConfiguration.ToUpper() == "NUEVO")
            {
                ConfigName us = new ConfigName();
                using (Form form = new Form()
                {
                    StartPosition = FormStartPosition.CenterScreen,
                    AutoScaleDimensions = new SizeF(6F, 13F),
                    AutoScaleMode = AutoScaleMode.Font,
                    BackColor = Color.FromArgb(34, 34, 34),
                    ClientSize = new Size(us.Width, us.Height),
                    FormBorderStyle = FormBorderStyle.None,
                })
                {
                    form.Controls.Add(us);
                    form.TopMost = true;
                    form.ShowDialog();
                    if (!us.saved)
                        return;
                    this.CurrentConfiguration = us.configName;
                }
            }

            joystick.ConfigMode = false;
            joystick.Save(this.CurrentConfiguration);
            ((Form)this.TopLevelControl).Close();
        }

        private void invertXCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            joystick.SetJoystickSetting(GlobalJoystickSetting.InvertXAxis, invertXCheckBox.Checked.ToString());
        }

        private void invertYCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            joystick.SetJoystickSetting(GlobalJoystickSetting.InvertYAxis, invertYCheckBox.Checked.ToString());
        }

        private void invertZRotationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            joystick.SetJoystickSetting(GlobalJoystickSetting.InvertZAxisRotation, invertZRotationCheckBox.Checked.ToString());
        }

        private void actionsMappingListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (actionsMappingListView.SelectedItems.Count == 0)
                    return;

                ActionCommand selectedActionCommand = (ActionCommand)joystick.ActionMappingsListModel[actionsMappingListView.SelectedIndices[0]];
                joystick.SetActionCommand(ButtonOrAxis.None, selectedActionCommand);
                joystick.UpdateListView();
            }
        }

        private void bunifuButtonCancelar_Click(object sender, EventArgs e)
        {
            dateWindowsKey = null;
            joystick.ConfigMode = false;
            ((Form)this.TopLevelControl).Close();
        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            joystick.ConfigMode = false;
            ((Form)this.TopLevelControl).Close();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            dateWindowsKey = null;
            Process.Start("joy.cpl");
        }

        private void actionsMappingListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))))), e.Bounds);
            using (Font headerFont = new Font("Segoe UI", 8.25f, FontStyle.Bold)) //Font size!!!!
            {
                e.Graphics.DrawString(e.Header.Text, headerFont, Brushes.Silver, e.Bounds);
            }
        }

        private void actionsMappingListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
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
            if (actionsMappingListView.InvokeRequired)
            {
                actionsMappingListView.Invoke((MethodInvoker)delegate
                {
                    LoadConfiguration(configMode);
                });
                return;
            }
            joystick = driverFactoryJoystick.GetDriverJoystinck();
            this.actionsMappingListView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.lblTitle.Text = Resources.JoystickSetting;
            joystick.ContainerSelectedEvent += Joystick_ContainerSelectedEvent;
            joystick.DriverFactoryChange += Joystick_DriverFactoryChange;


            if (joystick is USBJoystick)
            {
                actionsLabel.Text = Resources.JoystickTitleUSB;
                actionsMappingListView.KeyDown += new KeyEventHandler(actionsMappingListView_KeyDown);
                joystick.ConfigMode = configMode;
                var running = driverFactoryJoystick.StartJoystick();
                if (!running)
                    return;
                joystick.JoystickSetting += joystick_JoystickSetting;
                joystick.MappingActionsListView += joystick_MappingActionsListView;
                joystick.JoystickButtonSettingEvent += joystickButtonEvent;
                UpdateView(DriversJoystick.USB);

            }
            else
            {
                actionsLabel.Text = Resources.JoystickTitleCOM;
                UpdateView(DriversJoystick.COM);
            }

            CurrentConfiguration = joystick.GetCurrentJoystickConf();
            joystick.LoadConfiguration(CurrentConfiguration);
            var list = joystick.GetCustomConfig();
            bunifuButtonGuardar.Enabled = true;
            DropDownConfig.DataSource = list;
            if (list != null && list.Count > 0)
                DropDownConfig.SelectedIndex = string.IsNullOrEmpty(CurrentConfiguration) ? 0 : list.IndexOf(CurrentConfiguration);

            DropDownConfig.SelectedIndexChanged += Load_Configuration;
        }

        private void Joystick_DriverFactoryChange()
        {
            actionsMappingListView.KeyDown -= new KeyEventHandler(actionsMappingListView_KeyDown);
            joystick.JoystickSetting += joystick_JoystickSetting;
            joystick.MappingActionsListView += joystick_MappingActionsListView;
            joystick.JoystickButtonSettingEvent += joystickButtonEvent;
            LoadConfiguration(false);
        }
    }
}
