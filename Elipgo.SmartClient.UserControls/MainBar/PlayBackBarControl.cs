using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Services.Services.Interface;
using Elipgo.SmartClient.UserControls.MainBar;
using Splat;
using System;
using System.Globalization;

namespace Elipgo.SmartClient.UserControls.PlayBackBar
{
    public delegate void ObjectSelectedEventHandler(object sender, LiveBarItemDTO element);

    public partial class PlayBackBarControl : MainBarBaseControl
    {
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();

        public PlayBackBarControl()
        {
            InitializeComponent();
            BuildBar();
            this.Resize += PlayBackBarControl_Resize;
            this.Disposed += PlayBackBarControl_Disposed;
        }

        private void PlayBackBarControl_Disposed(object sender, EventArgs e)
        {
            this.buttonGridClear.Click -= ButtonGridClear_Click;
            this.buttonGroups.Click -= ButtonGroups_Click;
            this.buttonCarruseles.Click -= ButtonCarruseles_Click;
            this.buttonScene.Click -= ButtonScene_Click;
            this.buttonGrid.Click -= ButtonGrid_Click;
            this.buttonSaveGroup.Click -= ButtonSaveGroup_Click;
            this.Resize -= PlayBackBarControl_Resize;
            this.Disposed -= PlayBackBarControl_Disposed;
        }

        private void PlayBackBarControl_Resize(object sender, EventArgs e)
        {
            ResizeControls();
        }

        public override void LoadButtons()
        {
            this.Buttons.Add(buttonGridClear);
            this.Buttons.Add(buttonGrid);
            this.Buttons.Add(buttonScene);
            this.Buttons.Add(buttonCarruseles);
            this.Buttons.Add(buttonGroups);
            this.Buttons.Add(buttonSaveGroup);
        }

        public override void SetImageButtons()
        {
            this.buttonGridClear.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.contextBarMain_clear_grids;
            this.buttonGroups.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.contextBarMain_groups;
            this.buttonCarruseles.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.contextBarMain_carousel;
            this.buttonScene.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.contextBarMain_scenes;
            this.buttonGrid.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.contextBarMain_grids;
            this.buttonSaveGroup.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.contextBarMain_groups;
        }

        public override void ShowButtons()
        {
            buttonCarruseles.Visible = false;
            buttonScene.Visible = false;
            buttonGroups.Visible = appAuthorization.Exist(ButtonsContextBar.Group.GetAttribute<PermissionPlayback>().PermissionKey);
            buttonGridClear.Visible = appAuthorization.Exist(ButtonsContextBar.GridCleaner.GetAttribute<PermissionPlayback>().PermissionKey);
            buttonGrid.Visible = appAuthorization.Exist(ButtonsContextBar.Grid.GetAttribute<PermissionPlayback>().PermissionKey);
        }

        public override void SetTooltips()
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            bunifuToolTip1.SetToolTip(this.buttonCarruseles, ci.Name.Contains("es") ? ButtonsContextBar.ActiveCarousel.GetDescription() : ButtonsContextBar.ActiveCarousel.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(buttonScene, ci.Name.Contains("es") ? ButtonsContextBar.Scene.GetDescription() : ButtonsContextBar.Scene.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(buttonGroups, ci.Name.Contains("es") ? ButtonsContextBar.Group.GetDescription() : ButtonsContextBar.Group.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(buttonGridClear, ci.Name.Contains("es") ? ButtonsContextBar.GridCleaner.GetDescription() : ButtonsContextBar.GridCleaner.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(buttonGrid, ci.Name.Contains("es") ? ButtonsContextBar.Grid.GetDescription() : ButtonsContextBar.Grid.GetAttribute<DescriptionEN>().Descripcion);
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
    }
}
