using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Services.Services.Interface;
using Elipgo.SmartClient.UserControls.MainBar;
using Splat;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.LiveBar
{
    public delegate void ObjectSelectedEventHandler(object sender, LiveBarItemDTO element);

    public partial class LiveBarControl : MainBarBaseControl
    {
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        private bool _talkStatus = false;
        private bool _digitalZoomStatus = false;

        public LiveBarControl()
        {
            InitializeComponent();
            BuildBar();
            this.Resize += LiveBarControl_Resize;
            this.Disposed += LiveBarControl_Disposed;
        }

        private void LiveBarControl_Disposed(object sender, EventArgs e)
        {
            this.buttonGridClear.Click -= ButtonGridClear_Click;
            this.buttonGroups.Click -= ButtonGroups_Click;
            this.buttonCarruseles.Click -= ButtonCarruseles_Click;
            this.buttonScene.Click -= ButtonScene_Click;
            this.buttonGrid.Click -= ButtonGrid_Click;
            this.buttonSaveGroup.Click -= ButtonSaveGroup_Click;
            this.buttonMicrophone.Click -= ButtonMicrophone_Click;
            this.Resize -= LiveBarControl_Resize;
            this.Disposed -= LiveBarControl_Disposed;
        }

        private void LiveBarControl_Resize(object sender, EventArgs e)
        {
            ResizeControls();
            SetSizes();
        }

        public override void LoadButtons()
        {
            this.Buttons.Add(buttonGridClear);
            this.Buttons.Add(buttonGrid);
            this.Buttons.Add(buttonScene);
            this.Buttons.Add(buttonCarruseles);
            this.Buttons.Add(buttonGroups);
            this.Buttons.Add(buttonSaveGroup);
            this.Buttons.Add(buttonMicrophone);
        }

        public override void SetImageButtons()
        {
            this.buttonGridClear.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.contextBarMain_clear_grids;
            this.buttonGroups.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.contextBarMain_groups;
            this.buttonCarruseles.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.contextBarMain_carousel;
            this.buttonScene.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.contextBarMain_scenes;
            this.buttonGrid.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.contextBarMain_grids;
            this.buttonSaveGroup.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.contextBarMain_groups;
            this.buttonMicrophone.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.contextBarMain_mic_on;
        }

        public override void ShowButtons()
        {
            buttonCarruseles.Visible = appAuthorization.Exist(ButtonsContextBar.ActiveCarousel.GetAttribute<PermissionLive>().PermissionKey);
            buttonScene.Visible = appAuthorization.Exist(ButtonsContextBar.Scene.GetAttribute<PermissionLive>().PermissionKey);
            buttonGroups.Visible = appAuthorization.Exist(ButtonsContextBar.Group.GetAttribute<PermissionLive>().PermissionKey);
            buttonGridClear.Visible = appAuthorization.Exist(ButtonsContextBar.GridCleaner.GetAttribute<PermissionLive>().PermissionKey);
            buttonGrid.Visible = appAuthorization.Exist(ButtonsContextBar.Grid.GetAttribute<PermissionLive>().PermissionKey);
            buttonMicrophone.Visible = appAuthorization.Exist(ButtonsContextBar.TalkAll.GetAttribute<PermissionLive>().PermissionKey);
        }

        public override void SetTooltips()
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            bunifuToolTip1.SetToolTip(this.buttonCarruseles, ci.Name.Contains("es") ? ButtonsContextBar.ActiveCarousel.GetDescription() : ButtonsContextBar.ActiveCarousel.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(buttonScene, ci.Name.Contains("es") ? ButtonsContextBar.Scene.GetDescription() : ButtonsContextBar.Scene.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(buttonGroups, ci.Name.Contains("es") ? ButtonsContextBar.Group.GetDescription() : ButtonsContextBar.Group.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(buttonGridClear, ci.Name.Contains("es") ? ButtonsContextBar.GridCleaner.GetDescription() : ButtonsContextBar.GridCleaner.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(buttonGrid, ci.Name.Contains("es") ? ButtonsContextBar.Grid.GetDescription() : ButtonsContextBar.Grid.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(buttonMicrophone, ci.Name.Contains("es") ? ButtonsContextBar.Talk.GetDescription() : ButtonsContextBar.Talk.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(bunifuToggleSwitch, ci.Name.Contains("es") ? ButtonsContextBar.ActiveCarousel.GetDescription() : ButtonsContextBar.ActiveCarousel.GetAttribute<DescriptionEN>().Descripcion);
        }

        public void SwitchCarouselVisible(bool visible)
        {
            bunifuToggleSwitch.Visible = visible;

            if (visible)
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                var btnHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.022M), 2));
                this.bunifuToggleSwitch.Location = new Point(Buttons.LastOrDefault().Left - 96, btnHeight);
            }
        }

        public void setSwitchCarousel(bool check)
        {
            bunifuToggleSwitch.Checked = check;
        }

        private void ButtonGridClear_Click(object sender, EventArgs e)
        {
            base.OnObjectSelectedChanged(sender, new ObjectSelectedArgs(new LiveBarItemDTO(LiveBarButtom.removeGrids)));
        }

        private void ButtonCarruseles_Click(object sender, EventArgs e)
        {
            base.OnObjectSelectedChanged(sender, new ObjectSelectedArgs(new LiveBarItemDTO(LiveBarButtom.carousel)));
        }

        private void ButtonGroups_Click(object sender, EventArgs e)
        {
            base.OnObjectSelectedChanged(sender, new ObjectSelectedArgs(new LiveBarItemDTO(LiveBarButtom.groups)));
        }

        private void ButtonGrid_Click(object sender, EventArgs e)
        {
            base.OnObjectSelectedChanged(sender, new ObjectSelectedArgs(new LiveBarItemDTO(LiveBarButtom.grids)));
        }

        private void ButtonScene_Click(object sender, EventArgs e)
        {
            base.OnObjectSelectedChanged(sender, new ObjectSelectedArgs(new LiveBarItemDTO(LiveBarButtom.scenes)));
        }

        private void ButtonSaveGroup_Click(object sender, EventArgs e)
        {
            base.OnObjectSelectedChanged(sender, new ObjectSelectedArgs(new LiveBarItemDTO(LiveBarButtom.saveGroups)));
        }

        private void ButtonMicrophone_Click(object sender, EventArgs e)
        {
            _talkStatus = !_talkStatus;
            if (_talkStatus)
            {
                this.buttonMicrophone.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.contextBarMain_mic_on;
            }
            else
            {
                this.buttonMicrophone.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.contextBarMain_mic_off;
            }

            base.OnObjectSelectedTalkChanged(sender, _talkStatus);
        }

        private void ButtonDigitalZoom_Click(object sender, EventArgs e)
        {
            _digitalZoomStatus = !_digitalZoomStatus;
            if (_digitalZoomStatus)
            {
                this.buttonDigitalZoom.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.icon_digital_zoom_on;
            }
            else
            {
                this.buttonDigitalZoom.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.icon_digital_zoom_off;
            }

            buttonDigitalZoom.Refresh();

            base.OnObjectSelectedDigitalZoomChanged(sender, _digitalZoomStatus);
        }

        private void BunifuToggleSwitch_Click(object sender, EventArgs e)
        {
            base.OnObjectSelectedChanged(sender, new ObjectSelectedArgs(new LiveBarItemDTO(LiveBarButtom.activeCarousel)));
        }


        private void SetSizes()
        {
            if (Screen.AllScreens.Length >= 2 && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                //var space = 0;
                //var startPositionButton = 1154;
                var btnHeight = 20;
                if (main.Width > 1400 && this.Width < 1700)
                {
                    btnHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                }
                else
                {
                    btnHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.022M), 2));
                }
                this.Buttons.ForEach(x =>
                {
                    if (x.Visible)
                    {
                        x.Size = new System.Drawing.Size(btnHeight, btnHeight);
                        //startPositionButton = (startPositionButton - space);
                        //space = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.044M), 2));
                    }
                });
            }
        }
    }
}
