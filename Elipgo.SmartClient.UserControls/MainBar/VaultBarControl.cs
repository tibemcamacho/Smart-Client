using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.UserControls.Groups;
using Elipgo.SmartClient.UserControls.MainBar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.VaultBar
{
    public enum ContentListView
    {
        Bookmarks,
        ExportedBookmarks
    }

    public class ToggleEventArgs : EventArgs
    {
        public ContentListView CurrentContentList { get; set; }

        public ToggleEventArgs(ContentListView contentListView)
        {
            CurrentContentList = contentListView;
        }
    }

    public delegate void TextBoxFilterKeyUp(object sender, KeyEventArgs args);
    public delegate void ButtonFindClick(object sender, EventArgs e);
    public delegate void ButtonClearTextClick(object sender, EventArgs e);
    public delegate void ButtonToggleBookmarksClick(object sender, ToggleEventArgs e);
    public delegate void SelectDeviceObjectName(object sender, EventArgs e);
    public delegate void SearchDeviceObjectName(object sender, string textSearchs);

    public partial class VaultBarControl : MainBarBaseControl
    {
        public event TextBoxFilterKeyUp TextBoxFilterKeyUp;
        public event ButtonFindClick ButtonFindClick;
        public event ButtonClearTextClick ButtonClearTextClick;
        public event ButtonToggleBookmarksClick ButtonToggleBookmarksClick;
        public event SelectDeviceObjectName SelectDeviceObjectName;
        public event SearchDeviceObjectName SearchDeviceObjectName;
        private ContentListView? contentListView = null;
        private bool _resizeLoad = false;
        public string FilterTextBoxValue { get => this.TextboxFilter.Text; set => this.TextboxFilter.Text = value; }
        public DateTime? Filter_StartDateValue { get; set; }
        public DateTime? Filter_EndDateValue;
        private bool exportedBookMarkMode;
        private string defaultText;
        private List<SidebarElementDTO> _listOptionCameraObject = new List<SidebarElementDTO>();
        private short _takeDropdown = 0;
        public short pageDevicesObject = 0;
        public SidebarElementDTO DropDownListSelect
        {
            get
            {
                var selectedItem = this.ucddlDevices.SelectedItem as SidebarElementDTO;
                var pos = this.ucddlDevices.SelectedItem != null ? selectedItem : this.ucddlDevices.Items[0] as SidebarElementDTO;
                return pos;
            }
        }

        public VaultBarControl()
        {
            InitializeComponent();
            var _config = SmartClientEnvironmentUtils.GetConfiguration();
            _takeDropdown = Int16.Parse(_config.AppSettings.Settings["takeDropdown"].Value);
            _resizeLoad = true;
            BuildBar();
            exportedBookMarkMode = true;
            this.Resize += VaultBarControl_Resize;
            this.Disposed += VaultBarControl_Disposed;
            CultureInfo ci = CultureInfo.InstalledUICulture;
            defaultText = ci.Name.Contains("es") ? ButtonsContextBar.Devices.GetDescription() : ButtonsContextBar.Devices.GetAttribute<DescriptionEN>().Descripcion;
            ButtonStartDatetime.DateTimeClick += ButtonStartDatetime_DateTimeSelected;
            ButtonEndDatetime.DateTimeClick += ButtonEndDatetime_DateTimeSelected;
            ButtonStartDatetime.HideProgressBar();
            ButtonEndDatetime.HideProgressBar();
            ucddlDevices.SearchRequested += ucDeviceName_Search;
        }

        private void VaultBarControl_Disposed(object sender, EventArgs e)
        {
            this.buttonRefresh.Click -= ButtonRefresh_Click;
            this.ButtonToggleBookmarkGrid.Click -= ButtonToggleBookmarkGrid_Click;
            this.ButtonClearTextFilter.Click -= ButtonClearText_Click;
            this.ButtonFind.Click -= ButtonFind_Click;
            this.TextboxFilter.KeyUp -= TextboxFilter_KeyUp;
            ButtonStartDatetime.DateTimeClick -= ButtonStartDatetime_DateTimeSelected;
            ButtonEndDatetime.DateTimeClick -= ButtonEndDatetime_DateTimeSelected;
            this.Resize -= VaultBarControl_Resize;
            this.Disposed -= VaultBarControl_Disposed;
        }

        private void VaultBarControl_Resize(object sender, EventArgs e)
        {
            VaultBarControlResize();
        }

        public override void LoadButtons()
        {
            this.Buttons.Add(buttonRefresh);
        }

        public override void SetImageButtons()
        {
            this.buttonRefresh.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.icon_refresh;
        }

        public override void ShowButtons()
        {
            buttonRefresh.Visible = true;
        }

        public override void SetPositionButtons()
        {
            this.buttonRefresh.Location = new System.Drawing.Point(430, 27);
        }

        public override void SetTooltips()
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            bunifuToolTip1.SetToolTip(buttonRefresh, ci.Name.Contains("es") ? ButtonsContextBar.Refresh.GetDescription() : ButtonsContextBar.Refresh.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip2.SetToolTip(ButtonFind, ci.Name.Contains("es") ? ButtonsContextBar.Search.GetDescription() : ButtonsContextBar.Search.GetAttribute<DescriptionEN>().Descripcion);
        }

        public void LoadFilterControls(ContentListView currentContentList)
        {
            contentListView = currentContentList;
            VaultBarControlResize();
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            this.ClearSelectedFilters();
            base.OnObjectSelectedChanged(sender, new ObjectSelectedArgs(new LiveBarItemDTO(LiveBarButtom.refresh)));
        }

        private void TextboxFilter_KeyUp(object sender, KeyEventArgs e)
        {
            TextBoxFilterKeyUp(sender, e);
        }

        private void ButtonClearText_Click(object sender, EventArgs e)
        {
            ButtonClearTextClick(sender, e);
            ShowClearButton(false);
        }

        private void ButtonFind_Click(object sender, EventArgs e)
        {
            if (!ucddlDevices.isSearchingMode)
            {
                ButtonFindClick(sender, e);
            }
        }

        public bool IsExpotedBookMarkMode()
        {
            return exportedBookMarkMode;
        }

        private void ButtonToggleBookmarkGrid_Click(object sender, EventArgs e)
        {
            Bunifu.UI.WinForms.BunifuButton.BunifuButton btnToggleBookmarks = (Bunifu.UI.WinForms.BunifuButton.BunifuButton)sender;
            ToggleEventArgs toggleEventArgs;

            if (btnToggleBookmarks.Text.ToLower() == Resources.ExportedBookmarkGridTitle.ToLower())
            {
                toggleEventArgs = new ToggleEventArgs(ContentListView.ExportedBookmarks);

                ButtonToggleBookmarksClick(sender, toggleEventArgs);
                btnToggleBookmarks.Text = Resources.BookmarkGridTitle;
                exportedBookMarkMode = false;
            }
            else
            {
                toggleEventArgs = new ToggleEventArgs(ContentListView.Bookmarks);

                ButtonToggleBookmarksClick(sender, toggleEventArgs);
                btnToggleBookmarks.Text = Resources.ExportedBookmarkGridTitle;
                exportedBookMarkMode = true;
            }

            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var workingArea = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                var buttonToggleBookmarkGridWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.100M), 2));
                var buttonToggleBookmarkGridHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.034M), 2));
                btnToggleBookmarks.Size = new Size(buttonToggleBookmarkGridWidth, buttonToggleBookmarkGridHeight);

                var buttonToggleBookmarkGridX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.610M), 2));
                var buttonToggleBookmarkGridY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.016M), 2));
                btnToggleBookmarks.Location = new Point(buttonToggleBookmarkGridX, buttonToggleBookmarkGridY);
            }
        }

        public void ShowClearButton(bool show)
        {
            if (show)
            {
                ButtonClearTextFilter.BringToFront();
                ButtonClearTextFilter.Show();
                return;
            }

            ButtonClearTextFilter.Hide();
        }

        private void VaultBarControlResize(ContentListView currentContentList = ContentListView.Bookmarks)
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var workingArea = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                if (workingArea.Width > 1400 && workingArea.Width < 2000)
                {
                    LabelVaultBarTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Small_8, FontStyle.Regular, GraphicsUnit.Pixel);
                    TextboxFilter.BorderRadius = 25;

                    ButtonToggleBookmarkGrid.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonToggleBookmarkGrid.IdleBorderRadius = 30;
                    ButtonToggleBookmarkGrid.OnIdleState.BorderRadius = 30;
                    ucddlDevices.Font = FontHelper.GetRobotoRegular(FontSizes.Small_8, FontStyle.Regular, GraphicsUnit.Pixel);
                    LabelStartDateTime.Font = FontHelper.GetRobotoRegular(FontSizes.Small_8, FontStyle.Regular, GraphicsUnit.Pixel);
                    LabelEndDateTime.Font = FontHelper.GetRobotoRegular(FontSizes.Small_8, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (workingArea.Width > 1366 && workingArea.Width <= 1400)
                {
                }
                else if (workingArea.Width <= 1366)
                {
                    LabelVaultBarTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    TextboxFilter.BorderRadius = 15;
                    ButtonToggleBookmarkGrid.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonToggleBookmarkGrid.IdleBorderRadius = 20;
                    ButtonToggleBookmarkGrid.OnIdleState.BorderRadius = 20;
                    ucddlDevices.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    LabelStartDateTime.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    LabelEndDateTime.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (workingArea.Width >= 2000 && workingArea.Width < 2560)
                {
                    LabelVaultBarTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Small_9, FontStyle.Regular, GraphicsUnit.Pixel);
                    TextboxFilter.BorderRadius = 35;

                    ButtonToggleBookmarkGrid.Font = FontHelper.GetRobotoRegular(FontSizes.Small_8, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonToggleBookmarkGrid.IdleBorderRadius = 30;
                    ButtonToggleBookmarkGrid.OnIdleState.BorderRadius = 30;
                    ucddlDevices.Font = FontHelper.GetRobotoRegular(FontSizes.Small_9, FontStyle.Regular, GraphicsUnit.Pixel);
                    LabelStartDateTime.Font = FontHelper.GetRobotoRegular(FontSizes.Small_9, FontStyle.Regular, GraphicsUnit.Pixel);
                    LabelEndDateTime.Font = FontHelper.GetRobotoRegular(FontSizes.Small_9, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (workingArea.Width >= 2560 && workingArea.Width <= 3440)
                {
                    LabelVaultBarTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    TextboxFilter.BorderRadius = 35;

                    ButtonToggleBookmarkGrid.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonToggleBookmarkGrid.IdleBorderRadius = 30;
                    ButtonToggleBookmarkGrid.OnIdleState.BorderRadius = 30;
                    ucddlDevices.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    LabelStartDateTime.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    LabelEndDateTime.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                }

                var vaultBarControl = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.725M), 2));
                this.Width = vaultBarControl;

                int labelVaultBarTitleWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.103M), 2));
                int labelVaultBarTitleHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.042M), 2));
                this.LabelVaultBarTitle.Size = new Size(labelVaultBarTitleWidth, labelVaultBarTitleHeight);

                int labelVaultBarTitleX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0010M), 2));
                int labelVaultBarTitleY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.014M), 2));
                this.LabelVaultBarTitle.Location = new System.Drawing.Point(labelVaultBarTitleX, labelVaultBarTitleY);
                this.LabelVaultBarTitle.Text = Resources.BookmarkGridTitle;

                int textboxFilterWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.110M), 2));
                int textboxFilterHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.027M), 2));
                TextboxFilter.Size = new Size(textboxFilterWidth, textboxFilterHeight);

                int textboxFilterX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.110M), 2));
                int textboxFilterY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.022M), 2));
                this.TextboxFilter.Location = new System.Drawing.Point(textboxFilterX, textboxFilterY);

                var buttonClearTextFilterWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0125M), 2));
                var buttonClearTextFilterHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.022M), 2));
                ButtonClearTextFilter.Size = new Size(buttonClearTextFilterWidth, buttonClearTextFilterHeight);

                int buttonClearTextFilterX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.200M), 2));
                int buttonClearTextFilterY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.025M), 2));
                this.ButtonClearTextFilter.Location = new System.Drawing.Point(buttonClearTextFilterX, buttonClearTextFilterY);

                int ddlDevicesX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.225M), 2));
                int ddlDevicesY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.014M), 2));
                this.ucddlDevices.Location = new System.Drawing.Point(ddlDevicesX, ddlDevicesY);

                int ddlDevicesWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.100M), 2));
                int ddlDevicesHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.027M), 2));
                ucddlDevices.Size = new Size(ddlDevicesWidth, ddlDevicesHeight);

                int buttonStartDatetimeX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.325M), 2));
                int buttonStartDatetimeY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.014M), 2));
                this.ButtonStartDatetime.Location = new System.Drawing.Point(buttonStartDatetimeX, buttonStartDatetimeY);

                int labelStartDateTimeWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.103M), 2));
                int labelStartDateTimeHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.042M), 2));
                this.LabelStartDateTime.Size = new Size(labelStartDateTimeWidth, labelStartDateTimeHeight);

                int labelStartDateTimeX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.340M), 2));
                int labelStartDateTimeY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.014M), 2));
                this.LabelStartDateTime.Location = new System.Drawing.Point(labelStartDateTimeX, labelStartDateTimeY);

                int buttonEndDatetimeX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.445M), 2));
                int buttonEndDatetimeY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.014M), 2));
                this.ButtonEndDatetime.Location = new System.Drawing.Point(buttonEndDatetimeX, buttonEndDatetimeY);

                int labelEndDateTimeWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.103M), 2));
                int labelEndDateTimeHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.042M), 2));
                this.LabelEndDateTime.Size = new Size(labelEndDateTimeWidth, labelEndDateTimeHeight);

                int labelEndDateTimeX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.460M), 2));
                int labelEndDateTimeY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.014M), 2));
                this.LabelEndDateTime.Location = new System.Drawing.Point(labelEndDateTimeX, labelEndDateTimeY);

                var buttonFindWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0125M), 2));
                var buttonFindHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.022M), 2));
                ButtonFind.Size = new Size(buttonFindWidth, buttonFindHeight);

                int ButtonFindX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.225M), 2));
                int ButtonFindY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.025M), 2));
                this.ButtonFind.Location = new System.Drawing.Point(ButtonFindX, ButtonFindY);

                var buttonRefreshWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0125M), 2));
                var buttonRefreshHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.022M), 2));
                this.buttonRefresh.Size = new Size(buttonRefreshWidth, buttonRefreshWidth);

                int buttonRefreshX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.585M), 2));
                int buttonRefreshY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.026M), 2));
                this.buttonRefresh.Location = new System.Drawing.Point(buttonRefreshX, buttonRefreshY);
                this.buttonRefresh.Refresh();

                var buttonToggleBookmarkGridWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.100M), 2));
                var buttonToggleBookmarkGridHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.034M), 2));
                ButtonToggleBookmarkGrid.Size = new Size(buttonToggleBookmarkGridWidth, buttonToggleBookmarkGridHeight);

                var buttonToggleBookmarkGridX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.610M), 2));
                var buttonToggleBookmarkGridY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.016M), 2));
                ButtonToggleBookmarkGrid.Location = new Point(buttonToggleBookmarkGridX, buttonToggleBookmarkGridY);

                if (workingArea.Width == 1024 && workingArea.Height == 768)
                {
                    this.buttonRefresh.Location = new System.Drawing.Point(buttonRefreshX - 65, buttonRefreshY);
                    ButtonToggleBookmarkGrid.Size = new Size(buttonToggleBookmarkGridWidth + 30, buttonToggleBookmarkGridHeight);
                    ButtonToggleBookmarkGrid.Location = new Point(buttonToggleBookmarkGridX - 65, buttonToggleBookmarkGridY);
                }

                switch (contentListView)
                {
                    case null:
                    case ContentListView.Bookmarks:
                        this.LabelVaultBarTitle.Text = Resources.BookmarkGridTitle;
                        break;
                    case ContentListView.ExportedBookmarks:
                        this.LabelVaultBarTitle.Text = Resources.ExportedBookmarkGridTitle;
                        break;
                    default:
                        break;

                }
                _resizeLoad = false;
            }
        }

        public void loadDevices(CatalogObjectDetails devices)
        {
            if (pageDevicesObject == 1)
            {
                this.ucddlDevices.Items.Clear();
                _listOptionCameraObject.Clear();
                //this.textsearch = string.Empty;
            }
            this.ucddlDevices.Items.Clear();
            _listOptionCameraObject.RemoveAll(x => x.ElementId == 0);
            if (devices.Count != 0 && devices.Count > _takeDropdown)
            {
                float totalpage = (int)Math.Ceiling((double)devices.Count / _takeDropdown);
                if (totalpage > pageDevicesObject)
                {
                    var seemore = new SidebarElementDTO { ElementId = 0, Name = Resources.ViewMore };
                    devices.Elements.Add(seemore);
                }
            }

            _listOptionCameraObject.AddRange(devices.Elements);
            foreach (var item in _listOptionCameraObject)
            {
                this.ucddlDevices.Items.Add(item);
                this.ucddlDevices.ValueMember = "ElementId";
                this.ucddlDevices.DisplayMember = "Name";
            }

            if (pageDevicesObject == 1)
            {
                this.ucddlDevices.Items.Insert(0, new SidebarElementDTO
                {
                    ElementId = -1,
                    Name = defaultText
                });
            }
            this.ucddlDevices.SelectedIndex = 0;
        }

        private void ButtonStartDatetime_DateTimeSelected(object sender, Dictionary<string, DateTime> e)
        {
            e.TryGetValue("date", out DateTime date);
            e.TryGetValue("time", out DateTime time);
            if (date != null && time != null)
            {
                DateTime combinedDateTime = date.Date + time.TimeOfDay;
                LabelStartDateTime.Text = combinedDateTime.ToString("yyyy/MM/dd HH:mm:ss");
                Filter_StartDateValue = combinedDateTime;
            }
        }

        private void ButtonEndDatetime_DateTimeSelected(object sender, Dictionary<string, DateTime> e)
        {
            e.TryGetValue("date", out DateTime date);
            e.TryGetValue("time", out DateTime time);
            if (date != null && time != null)
            {
                DateTime combinedDateTime = date.Date + time.TimeOfDay;
                LabelEndDateTime.Text = combinedDateTime.ToString("yyyy/MM/dd HH:mm:ss");
                Filter_EndDateValue = combinedDateTime;
            }
        }

        private void ClearSelectedFilters()
        {
            Filter_StartDateValue = null;
            this.LabelStartDateTime.Text = "0000/00/00 00:00:00";

            Filter_EndDateValue = null;
            this.LabelEndDateTime.Text = "0000/00/00 00:00:00";

            this.ucddlDevices.SelectedIndex = 0;
        }

        private void ucddlDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedDevice = this.ucddlDevices.SelectedItem as SidebarElementDTO;
            if (selectedDevice != null && selectedDevice.ElementId == 0)
            {
                SelectDeviceObjectName(sender, e);
            }
        }
        private void ucDeviceName_Search(object sender, string textSearchs)
        {
            //this.textSiteSearch = textSearchs;
            this.pageDevicesObject = 1;
            SearchDeviceObjectName(sender, textSearchs);
        }
    }
}
