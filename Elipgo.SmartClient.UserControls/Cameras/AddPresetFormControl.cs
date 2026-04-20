using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Drivers;
using Splat;
using System;
using System.Drawing;
using System.Windows.Forms;
using EnumDrivers = Elipgo.SmartClient.Common.Enum.Drivers;

namespace Elipgo.SmartClient.UserControls.Cameras
{

    public partial class AddPresetFormControl : UserControl
    {
        public IDriverLive Driver { get; set; }

        private readonly ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();

        public AddPresetFormControl()
        {
            InitializeComponent();

            ButtonOK.Text = Resources.ButtonOK;
            ButtonCancel.Text = Resources.cancel;
            ButtonOK.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
            ButtonCancel.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
            LabelTitle.ForeColor = ColorTranslator.FromHtml(VariableResources.COLOR_TITLE_FONT);
            LabelName.ForeColor = ColorTranslator.FromHtml(VariableResources.COLOR_TITLE_FONT);
            TextBoxPresetName.Visible = false;
            DropdownPreset.Visible = false;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            var preset = new PresetDTO();
            if (this.TextBoxPresetName.Visible)
            {
                if (string.IsNullOrEmpty(TextBoxPresetName.Text))
                {
                    return;
                }

                preset = new PresetDTO()
                {
                    Name = TextBoxPresetName.Text
                };
            }

            if (this.DropdownPreset.Visible)
            {
                preset = (PresetDTO)this.DropdownPreset.SelectedItem;

                if (preset == null)
                {
                    return;
                }
            }

            if (Driver.SavePreset(preset))
            {
                notification.Show(string.Format(Resources.PresetSaved, preset.Name), null);
            }

            this.ParentForm.DialogResult = DialogResult.OK;
            this.ParentForm.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.ParentForm.DialogResult = DialogResult.Cancel;
            this.ParentForm.Close();
        }

        private void TextBoxName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxPresetName.Text))
            {
                errorManager.SetErrorWithCount(TextBoxPresetName, Resources.required);
            }
            else
            {
                errorManager.SetErrorWithCount(TextBoxPresetName, string.Empty);
            }
        }

        private void LoadDriverComponents()
        {
            switch (Driver.Camera.Driver)
            {
                case EnumDrivers.AMC_741:
                    TextBoxPresetName.Visible = true;
                    break;
                default:
                    DropdownPreset.Visible = true;
                    var presets = Driver.ListPresets() ?? new PresetDTO[] { };
                    DropdownPreset.DataSource = new BindingSource(presets, null);
                    DropdownPreset.DisplayMember = "Name";
                    DropdownPreset.ValueMember = "Id";
                    DropdownPreset.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                    break;
            }
        }

        private void AddPresetFormControl_Load(object sender, EventArgs e)
        {
            LoadDriverComponents();
        }
    }
}
