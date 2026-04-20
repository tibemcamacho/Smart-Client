using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.UserControls.GenericForm;
using Elipgo.SmartClient.ViewModels;
using Splat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Cameras
{
    public partial class PresetDahuaControl : GenericContentComponent
    {
        private PresetDTO _entity;
        private readonly ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        public PresetDahuaControl()
        {
            base.Configuration = new ConfigGenericForm
            {
                ObjectBarSelected = LiveBarButtom.preset,
                NameEntity = Resources.preset,
                IconEntity = FileResources.icon_presets,
                CanEditOrCreate = false,
                CanPrivate = false,
                CanMultiSelect = false,
                ShowAddButton = false
            };

            InitializeComponent();
            tilteForm.Text = Resources.preset;
            ControlsResize();
        }

        public PresetViewModel PresetViewModel => (ViewModel as PresetViewModel);

        public override void Clear()
        {
            txtName.Text = string.Empty;
        }

        public override void DobleClick(GenericForm.ContentFormDTO element)
        {
            PresetViewModel.Driver.CallPreset(element.ObjectOrigin as PresetDTO);
        }

        public override Task<List<GenericForm.ContentFormDTO>> GetDataSource(Action<List<GenericForm.ContentFormDTO>> callback)
        {
            return Task.Run(() =>
            {
                var data = PresetViewModel.Driver.ListPresets();
                return data?.Select
                     (
                       p => new GenericForm.ContentFormDTO
                       {
                           Label1 = p.Name,
                           EntityIcon = FileResources.icon_presets,
                           IsPrivate = false,
                           Id = p.Id,
                           ObjectOrigin = p
                       }
                     ).ToList() ?? new List<GenericForm.ContentFormDTO>();
            });
        }

        public override async Task<bool> Edit()
        {
            if (SelectedItem != null)
            {
                if (PresetViewModel.Driver.Camera.ManufactureCode == Common.Enum.Manufacturer.Dahua)
                {
                    _entity = new PresetDTO
                    {
                        Id = SelectedItem.Item.Id,
                        Name = SelectedItem.Item.Label1
                    };
                    PresetViewModel.Driver.SavePreset(_entity);
                    notification.Show(string.Format(Resources.PresetSaved, _entity.Name), null);
                    return false;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public override async Task<bool> Delete()
        {
            var result = false;
            if (SelectedItem != null)
            {
                result = PresetViewModel.Driver.RemovePreset(SelectedItem.Item.ObjectOrigin as PresetDTO);
            }

            return false;

        }

        public void ControlsResize()
        {
            if (Screen.AllScreens.Length >= 2 && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                #region Estilos

                if (main.Width > 1400 && main.Width < 2000)
                {

                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width >= 1366 && main.Width < 1400)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);

                }
                else if (main.Width >= 2000 && main.Width < 2560)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width >= 2560 && main.Width <= 3440)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Large_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                }


                #endregion

                //922, 495
                var panel1Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.4802M), 2));
                var panel1Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.4583M), 2));
                panel1.Size = new Size(panel1Width, panel1Height);

                ////4, 2
                var tilteFormX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.002M), 2));
                var tilteFormY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.002M), 2));
                tilteForm.Location = new Point(tilteFormX, tilteFormY);


                //44, 13
                var labelNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.02291M), 2));
                var labelNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.012M), 2));
                labelName.Size = new Size(panel1Width, panel1Height);
                //20, 50
                var labelNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0104M), 2));
                var labelNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0462M), 2));
                labelName.Location = new Point(labelNameX, labelNameY);

                //363, 35
                var txtNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1890M), 2));
                var txtNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0324M), 2));
                txtName.Size = new Size(txtNameWidth, txtNameHeight);
                txtName.MinimumSize = new Size(txtNameWidth, txtNameHeight);
                //23, 66
                var txtNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0119M), 2));
                var txtNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0611M), 2));
                txtName.Location = new Point(txtNameX, txtNameY);

            }
        }

    }
}
