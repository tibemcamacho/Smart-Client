using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.UserControls.GenericForm;
using Elipgo.SmartClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Cameras
{
    public partial class PresetControl : GenericContentComponent
    {
        private PresetDTO _entity;

        public bool FromPreset { get; set; }

        public PresetControl()
        {
            base.Configuration = new ConfigGenericForm
            {
                ObjectBarSelected = LiveBarButtom.preset,
                NameEntity = Resources.preset,
                IconEntity = FileResources.icon_presets,
                CanEditOrCreate = true,
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
                var data = PresetViewModel.Driver.ListPresets().Where(x => x.Enabled == true);
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
            if (SelectedItemOption != null)
            {
                if (PresetViewModel.Driver.Camera.Driver == Common.Enum.Drivers.AMC_741
                    || PresetViewModel.Driver.Camera.Driver == Common.Enum.Drivers.HCNetSDK_616
                    || PresetViewModel.Driver.Camera.Driver == Common.Enum.Drivers.HCNetSDK_619)
                {

                    _entity = new PresetDTO
                    {
                        Id = SelectedItemOption.Id,
                        Name = SelectedItemOption.Label1
                    };

                    txtName.Text = _entity.Name;

                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public override async Task<GenericForm.ContentFormDTO> SaveOrUpdate()
        {
            if (!IsCompleted())
            {
                return null;
            }

            if (_entity == null)
            {//if is it new
                _entity = new PresetDTO();
                if (mancode == Common.Enum.Manufacturer.Hikvision)
                {
                    _entity.Id = GetNextHikVisionPresetId();
                }
            }
            else
            {//if is it updated
                _entity.OldName = _entity.Name;
            }
            _entity.Name = txtName.Text;

            if (PresetViewModel.Driver.SavePreset(_entity))
            {
                var obj = new GenericForm.ContentFormDTO
                {
                    EntityIcon = Configuration.IconEntity,
                    Id = _entity.Id,
                    IsActive = true,
                    IsPrivate = false,
                    Label1 = _entity.Name,
                    ObjectOrigin = _entity
                };

                _entity = null;
                return obj;
            }
            else
            {
                _entity = null;
                return null;
            }

        }
        private int GetNextHikVisionPresetId()
        {
            var array = PresetViewModel.Driver.ListPresets();
            int minId = 1;
            bool bfound = false;
            for (int i = 0; i < array.Length; i++)
            {
                if (minId < array[i].Id || array[i].Enabled == false)
                {
                    bfound = true;
                    break;
                }
                else
                {
                    minId++;
                }
            }
            if (bfound == false)
            {
                return array[array.Length - 1].Id + 1;
            }
            return minId;
        }
        public override async Task<bool> Delete()
        {
            var result = false;
            if (SelectedItemOption != null)
            {
                result = PresetViewModel.Driver.RemovePreset(SelectedItemOption.ObjectOrigin as PresetDTO);
            }

            _entity = null;
            if (PresetViewModel.Driver.Camera.Driver == Common.Enum.Drivers.AMC_741
                 || PresetViewModel.Driver.Camera.Driver == Common.Enum.Drivers.HCNetSDK_616
                 || PresetViewModel.Driver.Camera.Driver == Common.Enum.Drivers.HCNetSDK_619)
            {
                return result;
            }
            else
            {
                return false;
            }

        }

        private void TxtName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                errorManager.SetErrorWithCount(txtName, Resources.required);
            }
            else
            {
                errorManager.SetErrorWithCount(txtName, string.Empty);
            }
        }

        private bool IsCompleted()
        {
            return !(string.IsNullOrEmpty(txtName.Text));
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
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);

                }
                else if (main.Width >= 1366 && main.Width < 1400)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);

                }
                else if (main.Width >= 2000 && main.Width < 2560)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width >= 2560 && main.Width <= 3440)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Large_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width == 1024 && main.Height == 768)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width < 1366)
                {


                }
                else if (main.Width > 2000)
                {

                }

                #endregion

                //744,250 274
                var panel1Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3875M), 2));
                var panel1Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2314M), 2));
                panel1.Size = new Size(panel1Width, panel1Height);

                ////39, 20
                var tilteFormX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.020339M), 2));
                var tilteFormY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0185M), 2));
                tilteForm.Location = new Point(tilteFormX, tilteFormY);

                //44, 13
                var labelNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.02291M), 2));
                var labelNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.012M), 2));

                //20, 50
                var labelNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0104M), 2));
                var labelNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0777M), 2));

                //696, 20
                var txtNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3625M), 2));
                var txtNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0185M), 2));

                //23, 66
                var txtNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0119M), 2));
                var txtNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0953M), 2));

                if (main.Width == 1024 && main.Height == 768)
                {
                    txtNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3625M), 2)) - 15;
                    txtNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0185M), 2)) + 4;
                }

                labelName.Size = new Size(panel1Width, panel1Height);
                labelName.Location = new Point(labelNameX, labelNameY);

                txtName.Size = new Size(txtNameWidth, txtNameHeight);
                txtName.MinimumSize = new Size(txtNameWidth, txtNameHeight);

                txtName.Location = new Point(txtNameX, txtNameY);
            }
        }

    }
}
