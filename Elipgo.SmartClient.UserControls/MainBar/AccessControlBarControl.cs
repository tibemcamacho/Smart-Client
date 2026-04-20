using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.UserControls.MainBar;
using Elipgo.SmartClient.UserControls.VaultBar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.AccessControlBar
{
    public partial class AccessControlBarControl : MainBarBaseControl
    {
        public event EventHandler<OptionObjectDTO> ItemSelectedChanged;

        private bool _resizeLoad = false;
        private bool isSetting_SelectedOption = false;
        private OptionObjectDTO currentSelectedToValidate;

        public OptionObjectDTO DropDownListSelect
        {
            get
            {
                var selectedItem = this.ddlServersAC.SelectedItem as OptionObjectDTO;
                var pos = this.ddlServersAC.SelectedItem != null ? selectedItem : this.ddlServersAC.Items[0] as OptionObjectDTO;
                return pos;
            }
        }

        public AccessControlBarControl()
        {
            InitializeComponent();
            _resizeLoad = true;
            BuildBar();
            this.Resize += AccessControlBarControl_Resize; ;
            this.ddlServersAC.SelectedIndexChanged += DdlServersAC_SelectedValueChanged;
            this.Disposed += AccessControlBarControl_Disposed;
        }

        private void AccessControlBarControl_Disposed(object sender, EventArgs e)
        {
            this.Resize -= AccessControlBarControl_Resize; ;
            this.ddlServersAC.SelectedIndexChanged -= DdlServersAC_SelectedValueChanged;
            this.Disposed -= AccessControlBarControl_Disposed;
        }

        private void DdlServersAC_SelectedValueChanged(object sender, EventArgs e)
        {
            if (!isSetting_SelectedOption && currentSelectedToValidate != DropDownListSelect)
            {
                ItemSelectedChanged.Invoke(sender, DropDownListSelect);
            }
            isSetting_SelectedOption = false;
            currentSelectedToValidate = DropDownListSelect;
        }

        private void AccessControlBarControl_Resize(object sender, EventArgs e)
        {
            AccessControlBarResize();
        }

        public override void LoadButtons()
        {
            //throw new NotImplementedException();
        }

        public override void SetImageButtons()
        {
            //throw new NotImplementedException();
        }

        public override void SetTooltips()
        {
            //throw new NotImplementedException();
        }

        public override void ShowButtons()
        {
            //throw new NotImplementedException();
        }

        public void loadOptions(List<OptionObjectDTO> list)
        {
            foreach (var item in list)
            {
                this.ddlServersAC.Items.Add(item);
                this.ddlServersAC.ValueMember = "Key";
                this.ddlServersAC.DisplayMember = "Name";
            }

            isSetting_SelectedOption = true;
            if (this.ddlServersAC.Items.Count > 0)
                this.ddlServersAC.SelectedIndex = 0;

        }

        private void AccessControlBarResize(ContentListView currentContentList = ContentListView.Bookmarks)
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var workingArea = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                if (workingArea.Width > 1400 && workingArea.Width < 2000)
                {
                    ddlLabel.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (workingArea.Width > 1366 && workingArea.Width <= 1400)
                {
                }
                else if (workingArea.Width <= 1366)
                {
                    ddlLabel.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (workingArea.Width >= 2000)
                {
                }

                var accessControlBarControl = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.490M), 2));
                this.Width = accessControlBarControl;

                var ddlServersACWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.125M), 2));
                var ddlServersACHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.042M), 2));
                ddlServersAC.Size = new Size(ddlServersACWidth, ddlServersACHeight);

                var ddlServersACGridX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.364M), 2));
                var ddlServersACGridY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.016M), 2));
                if (workingArea.Width > 1400 && workingArea.Width < 2000)
                    ddlServersACGridY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.020M), 2));
                ddlServersAC.Location = new Point(ddlServersACGridX, ddlServersACGridY);

                var labelVaultBarTitleWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.040M), 2));
                var labelVaultBarTitleHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.042M), 2));
                this.ddlLabel.Size = new Size(labelVaultBarTitleWidth, labelVaultBarTitleHeight);

                var labelVaultBarTitleX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.ddlServersAC.Location.X - this.ddlLabel.Width), 2));
                var labelVaultBarTitleY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.014M), 2));
                this.ddlLabel.Location = new System.Drawing.Point(labelVaultBarTitleX, labelVaultBarTitleY);

                _resizeLoad = false;
            }
        }

    }
}
