using Elipgo.SmartClient.Services.Services.Interface;
using Elipgo.SmartClient.UserControls.MainBar;
using Splat;
using System;

namespace Elipgo.SmartClient.UserControls.EmtpyBar
{
    public partial class EmtpyBarControl : MainBarBaseControl
    {
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();

        public EmtpyBarControl()
        {
            InitializeComponent();
            BuildBar();
            this.Resize += PlayBackBarControl_Resize;
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
            //this.buttonGridClear.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.icons_remover_grillas;
            //this.buttonGroups.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.icons_grupos;
            //this.buttonCarruseles.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.icons_carruseles;
            //this.buttonScene.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.icons_escenas;
            //this.buttonGrid.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.icons_grillas;
            //this.buttonSaveGroup.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.icons_grupos;
        }

        public override void ShowButtons()
        {
            buttonCarruseles.Visible = false;
            buttonScene.Visible = false;
            buttonGroups.Visible = false;
            buttonGridClear.Visible = false;
            buttonGrid.Visible = false;
        }

        public override void SetTooltips()
        {
            //CultureInfo ci = CultureInfo.InstalledUICulture;
            //bunifuToolTip1.SetToolTip(this.buttonCarruseles, ci.Name.Contains("es") ? ButtonsContextBar.ActiveCarousel.GetDescription() : ButtonsContextBar.ActiveCarousel.GetAttribute<DescriptionEN>().Descripcion);
            //bunifuToolTip1.SetToolTip(buttonScene, ci.Name.Contains("es") ? ButtonsContextBar.Scene.GetDescription() : ButtonsContextBar.Scene.GetAttribute<DescriptionEN>().Descripcion);
            //bunifuToolTip1.SetToolTip(buttonGroups, ci.Name.Contains("es") ? ButtonsContextBar.Group.GetDescription() : ButtonsContextBar.Group.GetAttribute<DescriptionEN>().Descripcion);
            //bunifuToolTip1.SetToolTip(buttonGridClear, ci.Name.Contains("es") ? ButtonsContextBar.GridCleaner.GetDescription() : ButtonsContextBar.GridCleaner.GetAttribute<DescriptionEN>().Descripcion);
            //bunifuToolTip1.SetToolTip(buttonGrid, ci.Name.Contains("es") ? ButtonsContextBar.Grid.GetDescription() : ButtonsContextBar.Grid.GetAttribute<DescriptionEN>().Descripcion);
        }

        private void ButtonGridClear_Click(object sender, EventArgs e)
        {
            //base.OnObjectSelectedChanged(sender, new ObjectSelectedArgs(new LiveBarItemDTO(LivaBarButtom.removeGrids)));
        }

        private void ButtonCarruseles_Click(object sender, EventArgs e)
        {
            //base.OnObjectSelectedChanged(sender, new ObjectSelectedArgs(new LiveBarItemDTO(LivaBarButtom.carousel)));
        }

        private void ButtonGroups_Click(object sender, EventArgs e)
        {
            //base.OnObjectSelectedChanged(sender, new ObjectSelectedArgs(new LiveBarItemDTO(LivaBarButtom.groups)));
        }

        private void ButtonGrid_Click(object sender, EventArgs e)
        {
            //base.OnObjectSelectedChanged(sender, new ObjectSelectedArgs(new LiveBarItemDTO(LivaBarButtom.grids)));
        }

        private void ButtonScene_Click(object sender, EventArgs e)
        {
            //base.OnObjectSelectedChanged(sender, new ObjectSelectedArgs(new LiveBarItemDTO(LivaBarButtom.scenes)));
        }

        private void ButtonSaveGroup_Click(object sender, EventArgs e)
        {
            //base.OnObjectSelectedChanged(sender, new ObjectSelectedArgs(new LiveBarItemDTO(LivaBarButtom.saveGroups)));
        }
    }
}
