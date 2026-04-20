using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services;
using Elipgo.SmartClient.UserControls.GenericForm;
using Elipgo.SmartClient.ViewModels;
using Splat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Cameras
{
    public partial class GuardControl : GenericContentComponent
    {
        private GuardDTO _entity;

        public List<DataViewGuard> dataView = new List<DataViewGuard>();

        private List<GuardDTO> _guardList = new List<GuardDTO>();

        public int cantSeleccionados = 0;
        private List<PresetDTO> m_presets;

        private GenericFormPagination controlPag;

        private List<DataViewGuard> devices = new List<DataViewGuard>();
        private readonly ISmartNotification _notification = Locator.Current.GetService<ISmartNotification>();

        public GuardControl()
        {
            base.Configuration = new ConfigGenericForm
            {
                ObjectBarSelected = LiveBarButtom.guard,
                NameEntity = Resources.guard,
                IconEntity = FileResources.icon_guards,
                CanEditOrCreate = true,
                CanPrivate = false,
                CanMultiSelect = false,
                ShowAddButton = true,
                ShowSwitch = true
            };

            InitializeComponent();

            bunifuDataObject.AutoGenerateColumns = false;
            elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);
            tilteForm.Text = Resources.newGuard;
            labelName.Text = Resources.Name;
            elementsAvailable.Text = Resources.presetAvailable;
            ButtonAdd.Text = Resources.buttonAdd;
            labelTime.Text = Resources.timeSequence;
            lblPreset.Text = Resources.preset;

            SpeedColumn.DisplayMember = "Name";
            SpeedColumn.ValueMember = "Key";

            TimeColumn.DisplayMember = "Name";
            TimeColumn.ValueMember = "Key";

            UnitTimeColumn.DisplayMember = "Name";
            UnitTimeColumn.ValueMember = "Key";

            bunifuDataObject.RowHeadersVisible = false;
            bunifuDataObject.RowsDefaultCellStyle.SelectionBackColor = Color.Transparent;
            bunifuDataObject.RowTemplate.Height = 28;
            bunifuDataObject.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold);
            bunifuDataObject.CurrentTheme.HeaderStyle.SelectionBackColor = Color.FromArgb(15, 16, 18);
            bunifuDataObject.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 16, 18);

            bunifuDataObject.Columns[1].HeaderText = Resources.font;
            bunifuDataObject.Columns[2].HeaderText = Resources.speed;
            bunifuDataObject.Columns[3].HeaderText = Resources.duration;
            bunifuDataObject.Columns[5].HeaderText = Resources.delete;
            bunifuDataObject.Columns[6].HeaderText = Resources.order;
            controlPag = new GenericFormPagination(1);
            controlPag.OnClickNextPage += OnClickNextPage;
            controlPag.OnClickBackPage += OnClickBackPage;
            controlPag.OnClickStartPage += OnClickStartPage;
            controlPag.OnClickEndPage += OnClickEndPage;
            panelPag.Controls.Add(controlPag);
            panelPag.Visible = false;
            // LoadPreset();
            ControlsResize();

        }
        private void LoadPreset()
        {
            if (m_presets == null || m_presets.Count == 0)
            {
                m_presets = GuardViewModel.Driver.ListPresets().ToList();
            }
            //solucion tranitoria hasta que se implemente present en vmon5
            //if (GuardViewModel.Driver.Camera.ManufactureCode == Common.Enum.Manufacturer.Dahua
            //   && m_presets[0].Id == 0)
            //{
            //    // si comienza desde cero entonces se esta conectando desde un nvr
            //    foreach (var it in m_presets)
            //    {
            //        it.Id = it.Id + 1;
            //    }
            //}
        }
        public GuardViewModel GuardViewModel => (ViewModel as GuardViewModel);

        public override void Clear()
        {
            cantSeleccionados = 0;

            if (timeObject.SelectedIndex > -1)
            {
                timeObject.SelectedIndex = 0;
            }

            if (unitObject.SelectedIndex > -1)
            {
                unitObject.SelectedIndex = 0;
            }

            if (presetObject.SelectedIndex > -1)
            {
                unitObject.SelectedIndex = 0;
            }

            txtName.Text = string.Empty;

            dataView.Add(new DataViewGuard { });
            bunifuDataObject.DataSource = typeof(List<DataViewGuard>);
            bunifuDataObject.DataSource = dataView;
            dataView.Clear();

            _entity = new GuardDTO();
            _entity.Id = -1;
            bunifuDataObject.DataSource = typeof(List<DataViewGuard>);
            bunifuDataObject.DataSource = dataView;
            cantSeleccionados = 0;
            elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);
            panelPag.Visible = false;
            errorManager.SetError(txtName, Resources.required);
            devices = new List<DataViewGuard>();
            controlPag.UpdatePage(0, 1);
            m_presets?.Clear();
        }

        public override async Task<bool> Delete()
        {
            if (SelectedItemOption != null)
            {
                // return GuardViewModel.Driver.RemoveGuard(SelectedItem.Item.ObjectOrigin as GuardDTO);
                var currentGuard = _guardList.Where(x => x.isActivated).Select(x => new ActivateGuardDTO { Id = x.Id, isActivated = x.isActivated }).FirstOrDefault();
                if (currentGuard != null)
                {
                    (currentGuard as ActivateGuardDTO).isActivated = false;
                    bool bSucess = await Vmon5Service.StopGuard(GuardViewModel.Driver.Camera.Id, ((IGenericViewModel)this.ViewModel).Token, currentGuard as ActivateGuardDTO);
                    bSucess = bSucess && await Vmon5Service.DeleteGuard(GuardViewModel.Driver.Camera.Id, ((IGenericViewModel)this.ViewModel).Token, SelectedItemOption.ObjectOrigin as GuardDTO);
                    if ((SelectedItemOption.ObjectOrigin as GuardDTO).Id != currentGuard.Id)
                    {
                        (currentGuard as ActivateGuardDTO).isActivated = true;
                        bSucess = bSucess && await Vmon5Service.CallGuard(GuardViewModel.Driver.Camera.Id, ((IGenericViewModel)this.ViewModel).Token, currentGuard as ActivateGuardDTO);
                    }
                    return bSucess;
                }
                else
                {
                    return await Vmon5Service.DeleteGuard(GuardViewModel.Driver.Camera.Id, ((IGenericViewModel)this.ViewModel).Token, SelectedItemOption.ObjectOrigin as GuardDTO);
                }

            }

            return true;
        }

        public void AddElement()
        {
            var time = ((OptionItemDTO<int>)timeObject.SelectedItem);
            var unit = ((OptionItemDTO<int>)unitObject.SelectedItem);
            var preset = presetObject.SelectedItem as PresetDTO;

            var data = new DataViewGuard
            {
                Id = 0,
                Preset = preset,
                Speed = 10,
                Time = time.Key,
                UnitTime = unit.Key,
                IsDeleted = false,
                Order = devices.Count + 1,
            };

            dataView.Add(data);
            devices.Add(data);
            bunifuDataObject.DataSource = typeof(List<DataViewGuard>);
            bunifuDataObject.DataSource = dataView;
            bunifuDataObject.EndEdit();

            cantSeleccionados = devices.Count;
            elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);

            panelPag.Visible = (devices.Count > 0);
            controlPag.UpdatePage(devices.Count, 1, true);
            if (controlPag.PropTotalPage > 1)
            {
                controlPag.endPage();
            }

            pagination(controlPag.PropTotalPage);
            GenericFormControl.EnabledButtonOkEvent(devices.Count > 0);

        }

        public override async Task<bool> Edit()
        {
            if (SelectedItemOption != null)
            {
                errorManager.Clear();
                // var editEntity = GuardViewModel.Driver.GetGuard(SelectedItem.Item.Id);
                var editEntity = await Vmon5Service.GetGuard(GuardViewModel.Driver.Camera.Id, SelectedItemOption.Id, ((IGenericViewModel)this.ViewModel).Token);
                this._entity = new GuardDTO() { Id = editEntity.Id, isActivated = editEntity.isActivated };
                if (m_presets == null || m_presets.Count == 0)
                {
                    LoadPreset();
                }

                dataView.Clear();
                devices.Clear();
                foreach (var item in editEntity.GuardTours.OrderBy(x => x.ViewOrder))
                {
                    var preset = m_presets.FirstOrDefault(m => m.Id == item.PresetId);
                    if (preset != null)
                    {
                        preset.Id = item.PresetId;

                        var d = new DataViewGuard()
                        {
                            Id = item.PresetId,
                            Preset = preset,
                            Speed = item.Speed,
                            UnitTime = GuardViewModel.GetUnitTime(item.Time),
                            Time = GuardViewModel.GetTime(item.Time),
                            Order = item.ViewOrder
                        };
                        devices.Add(d);

                    }
                }
                dataView.AddRange(PagePagination(1));

                if (dataView.Count == 0)
                {
                    dataView.Add(new DataViewGuard { });
                    bunifuDataObject.DataSource = dataView;
                    dataView.Clear();
                }

                cantSeleccionados = devices.Count();
                elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);

                bunifuDataObject.DataSource = typeof(List<DataViewGuard>);
                bunifuDataObject.DataSource = dataView;
                txtName.Text = editEntity.Name;



                panelPag.Visible = (devices.Count > 1);
                controlPag.UpdatePage(devices.Count, 1);
                GenericFormControl.EnabledButtonOkEvent(devices.Count > 0);

                return true;

            }

            return false;
        }

        private bool IsCompleted()
        {
            return !(string.IsNullOrEmpty(txtName.Text));
        }

        public override async Task<GenericForm.ContentFormDTO> SaveOrUpdate()
        {
            if (!IsCompleted())
            {
                return null;
            }

            bunifuDataObject.EndEdit();

            var source = devices; //(bunifuDataObject.DataSource as List<DataViewGuard>);
            var preset = new List<GuardTourForCreationDTO>();

            var newEntity = new GuardForCreationDTO()
            {
                Name = txtName.Text,
                Id = -1
            };
            //newEntity.TimeBetweenSequences
            int timeBetweenSequences = int.Parse(timeObject.SelectedValue.ToString());
            newEntity.TimeBetweenSequences = timeBetweenSequences;

            if (source != null)
            {
                int j = 1;
                source.ForEach(i =>
                {
                    preset.Add
                    (
                        new GuardTourForCreationDTO
                        {
                            PresetId = i.Preset.Id,
                            Speed = i.Speed,
                            Time = i.Time,
                            ViewOrder = j++,
                            WaitTimeViewType = i.UnitTime == 0 ? WaitTimeViewType.Seconds : i.UnitTime == 1 ? WaitTimeViewType.Minutes : WaitTimeViewType.Hours
                        }
                    );
                });
                newEntity.GuardTours = preset.ToArray();
            }
            //var result1 = GuardViewModel.Driver.SaveGuard(newEntity);
            bool result;
            string message = "";
            if (_entity.Id == -1)
            {
                (result, message) = await Vmon5Service.SaveGuard(GuardViewModel.Driver.Camera.Id, ((IGenericViewModel)this.ViewModel).Token, newEntity, message);
            }
            else
            {
                newEntity.Id = _entity.Id;
                newEntity.isActivated = _entity.isActivated;

                //tmc
                result = await Vmon5Service.UpdateGuard(GuardViewModel.Driver.Camera.Id, ((IGenericViewModel)this.ViewModel).Token, newEntity);
            }
            if (!result)
            {
                MessageBox.Show(string.Format(Resources.NoGuardAvailable, GuardViewModel.Driver.Camera.Name), Resources.preset, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            return new GenericForm.ContentFormDTO
            {
                EntityIcon = Configuration.IconEntity,
                Id = newEntity.Id,
                Switch = newEntity.isActivated,
                IsPrivate = false,
                Label1 = newEntity.Name,
                ObjectOrigin = newEntity
            };
        }

        public override Task<List<GenericForm.ContentFormDTO>> GetDataSource(Action<List<GenericForm.ContentFormDTO>> callback)
        {
            return Task.Run(async () =>
            {
                try
                {
                    //var data = GuardViewModel.Driver.ListGuards();
                    var data = await Vmon5Service.GetGuards(GuardViewModel.Driver.Camera.Id, ((IGenericViewModel)this.ViewModel).Token);
                    _guardList.AddRange(data.ToList());
                    return data?.Select
                             (
                               p => new GenericForm.ContentFormDTO
                               {
                                   Label1 = p.Name,
                                   EntityIcon = FileResources.icon_guards,
                                   IsPrivate = false,
                                   Id = p.Id,
                                   ObjectOrigin = p,
                                   Switch = p.isActivated
                               }
                             ).ToList() ?? new List<GenericForm.ContentFormDTO>();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        private void GuardControl_Load(object sender, EventArgs e)
        {
            var times = GuardViewModel.GetTime();
            var unitTimes = GuardViewModel.GetUnitTime();
            if (m_presets == null)
            {
                LoadPreset();
            }

            if (GuardViewModel.Driver.Camera.ManufactureCode == Common.Enum.Manufacturer.Dahua
                || GuardViewModel.Driver.Camera.ManufactureCode == Common.Enum.Manufacturer.Hikvision)
            {//para el caso de dahua solo existe un tiempo de rotacion 15 segundos es por esto que los combobox que con este unico valor
                timeObject.DataSource = times.Where(x => x.Key == 15).ToList();
                unitObject.DataSource = unitTimes.Where(x => x.Name == "Seg" || x.Name == "Sec").ToList();
                presetObject.DataSource = m_presets;
                SpeedColumn.DataSource = GuardViewModel.GetSpeed();
                TimeColumn.DataSource = times;
                UnitTimeColumn.DataSource = unitTimes;
                this.bunifuToolTip1.AllowAutoClose = true;
                // Units for time are in milliseconds.
                bunifuToolTip1.AutoCloseDuration = 2000;
                CultureInfo ci = CultureInfo.InstalledUICulture;
                bunifuToolTip1.SetToolTip(this.timeObject, ci.Name.Contains("es") ? "Tiempo soportado por Dahua" : "Time supported by Dahua");
            }
            else
            {
                timeObject.DataSource = times;
                unitObject.DataSource = unitTimes;
                presetObject.DataSource = m_presets;
                SpeedColumn.DataSource = GuardViewModel.GetSpeed();
                TimeColumn.DataSource = times;
                UnitTimeColumn.DataSource = unitTimes;
            }

        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            AddElement();
        }

        private void bunifuDataObject_DataError(object sender, System.Windows.Forms.DataGridViewDataErrorEventArgs e)
        {

        }

        Image img = null;

        void BunifuDataObject_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == -1 && e.ColumnIndex == deleteDataGridViewCheckBoxColumn.Index)
                {
                    if (img != null)
                    {
                        img.Dispose();
                    }

                    img = FileResources.icon_papelera.Clone() as Image;

                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                    e.Graphics.DrawImage(img, e.CellBounds.X + (e.CellBounds.Width / 2 - 12), e.CellBounds.Y + (e.CellBounds.Height / 2 - 12), 24, 24);
                    e.Handled = true;
                }
            }
            catch { }
        }

        private void BunifuDataObject_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            bunifuDataObject.EndEdit();

            bool showMessage = false;
            if (e.RowIndex == -1 && e.ColumnIndex == deleteDataGridViewCheckBoxColumn.Index)
            {
                for (int i = bunifuDataObject.Rows.Count - 1; i >= 0; i--)
                {
                    if ((bool)bunifuDataObject.Rows[i].Cells[deleteDataGridViewCheckBoxColumn.Index].Value == true)
                    {
                        if (!showMessage)
                        {
                            if (MessageBox.Show(Resources.deleteItemsGrid, Resources.delete, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                            {
                                return;
                            }

                            showMessage = true;
                        }
                        var item = bunifuDataObject.Rows[i].DataBoundItem as DataViewGuard;
                        cantSeleccionados--;
                        elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);
                        dataView.Remove(item);
                        devices.Remove(item);

                    }
                }
                if (showMessage)
                {
                    bunifuDataObject.DataSource = typeof(List<DataViewGuard>);
                    bunifuDataObject.DataSource = dataView;
                }
            }
        }

        private void BunifuDataObject_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if ((e.RowIndex == -1 && e.ColumnIndex == deleteDataGridViewCheckBoxColumn.Index) || (e.RowIndex != -1 && e.ColumnIndex == order.Index))
                {
                    bunifuDataObject.Cursor = Cursors.Hand;
                }
                else
                {
                    bunifuDataObject.Cursor = Cursors.Default;
                }
            }
            catch { }
        }

        public override async Task<bool> SwitchChange(bool value, GenericForm.ContentFormDTO element)
        {
            bool result = true;
            var guard = new ActivateGuardDTO()
            {
                Id = (element.ObjectOrigin as GuardDTO).Id
            };
            guard.isActivated = value;
            if (value)
            {
                // return GuardViewModel.Driver.CallGuard(guard);
                result = await Vmon5Service.CallGuard(GuardViewModel.Driver.Camera.Id, ((IGenericViewModel)this.ViewModel).Token, guard);
            }
            else
            {
                //return GuardViewModel.Driver.StopGuard(guard);
                result = await Vmon5Service.StopGuard(GuardViewModel.Driver.Camera.Id, ((IGenericViewModel)this.ViewModel).Token, guard);
            }

            ////return GuardViewModel.Driver.SaveGuard(element.ObjectOrigin as GuardForCreationDTO);
            //return Vmon5Service.SaveGuard(GuardViewModel.Driver.Camera.Id, ((IGenericViewModel)this.ViewModel).Token, element.ObjectOrigin as GuardForCreationDTO);
            return result;
        }

        private Rectangle dragBoxFromMouseDown;

        private int rowIndexFromMouseDown;

        private int rowIndexOfItemUnderMouseToDrop;

        private bool startDragAndDrop = false;

        private void BunifuDataObject_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left && startDragAndDrop)
            {
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    DragDropEffects dropEffect = bunifuDataObject.DoDragDrop(
                             bunifuDataObject.Rows[rowIndexFromMouseDown],
                             DragDropEffects.Move);

                }
            }
        }

        private void BunifuDataObject_MouseDown(object sender, MouseEventArgs e)
        {
            rowIndexFromMouseDown = bunifuDataObject.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1)
            {
                Size dragSize = SystemInformation.DragSize;
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),

                                                               e.Y - (dragSize.Height / 2)),
                                                        dragSize);
            }

            else
            {
                dragBoxFromMouseDown = Rectangle.Empty;
            }
        }

        private void BunifuDataObject_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void BunifuDataObject_DragDrop(object sender, DragEventArgs e)

        {
            Point clientPoint = bunifuDataObject.PointToClient(new Point(e.X, e.Y));

            rowIndexOfItemUnderMouseToDrop = bunifuDataObject.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            if (e.Effect == DragDropEffects.Move)
            {
                var item = dataView.ElementAt(rowIndexFromMouseDown);
                dataView.RemoveAt(rowIndexFromMouseDown);
                dataView.Insert(rowIndexOfItemUnderMouseToDrop, item);
                bunifuDataObject.DataSource = typeof(List<DataViewGroup>);
                bunifuDataObject.DataSource = dataView;
            }

            startDragAndDrop = false;
        }

        public override async void DobleClick(GenericForm.ContentFormDTO element)
        {
            var activeGuard = _guardList.Where(x => x.isActivated).Select(x => new ActivateGuardDTO { Id = x.Id, isActivated = x.isActivated }).FirstOrDefault() as ActivateGuardDTO;
            if (activeGuard != null)
            {
                // GuardViewModel.Driver.StopGuard(activeGuard);
                await Vmon5Service.StopGuard(GuardViewModel.Driver.Camera.Id, ((IGenericViewModel)this.ViewModel).Token, activeGuard);
            }

            var selectedGuard = element.ObjectOrigin as GuardDTO;
            if (activeGuard == null || (selectedGuard.Id != activeGuard.Id))
            {
                //GuardViewModel.Driver.CallGuard(element.ObjectOrigin as GuardDTO);
                await Vmon5Service.CallGuard(GuardViewModel.Driver.Camera.Id, ((IGenericViewModel)this.ViewModel).Token, activeGuard);
            }
        }

        private void BunifuDataObject_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == order.Index && e.RowIndex != -1)
            {
                startDragAndDrop = true;
            }
        }

        public void ControlsResize()
        {
            if (Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                #region Estilos

                if (main.Width > 1400 && main.Width < 2000)
                {

                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    presetObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    timeObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    unitObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersHeight = 40;
                    //bunifuDataObject.RowTemplate.Height = 40;
                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 30;
                    ButtonAdd.OnIdleState.BorderRadius = 30;

                }
                else if (main.Width >= 1366 && main.Width < 1400)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    presetObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    timeObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    unitObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementsAvailable.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.Font = FontHelper.GetRobotoRegular(FontSizes.Small_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersDefaultCellStyle.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Bold, GraphicsUnit.Pixel);
                    bunifuDataObject.RowsDefaultCellStyle.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersHeight = 30;
                    bunifuDataObject.RowTemplate.Height = 30;
                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 20;
                    ButtonAdd.OnIdleState.BorderRadius = 20;
                    //GroupName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);


                }
                else if (main.Width >= 2000 && main.Width <= 2560)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    presetObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    timeObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    unitObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Small_8, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersHeight = 40;
                    //bunifuDataObject.RowTemplate.Height = 40;
                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 30;
                    ButtonAdd.OnIdleState.BorderRadius = 30;

                }
                else if (main.Width > 2560 && main.Width <= 3440)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    presetObject.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    timeObject.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    unitObject.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersHeight = 40;
                    //bunifuDataObject.RowTemplate.Height = 40;
                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 40;
                    ButtonAdd.OnIdleState.BorderRadius = 40;
                }
                else if (main.Width == 1024 && main.Height == 768)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Bold, GraphicsUnit.Pixel);
                    tilteForm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);

                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 20;
                    ButtonAdd.OnIdleState.BorderRadius = 20;

                    elementsAvailable.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Bold, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);

                    bunifuDataObject.Font = FontHelper.GetRobotoRegular(FontSizes.Small_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersDefaultCellStyle.Font = FontHelper.GetRobotoRegular(FontSizes.Small_2, FontStyle.Bold, GraphicsUnit.Pixel);
                    bunifuDataObject.RowsDefaultCellStyle.Font = FontHelper.GetRobotoRegular(FontSizes.Small_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersHeight = 30;
                    bunifuDataObject.RowTemplate.Height = 30;
                }
                else if (main.Width < 1366)
                {


                }
                else if (main.Width > 2000)
                {

                }

                #endregion

                //745, 495
                var panel1Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3880M), 2));
                var panel1Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.458M), 2));

                //34, 11
                var tilteFormX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0177M), 2));
                var tilteFormY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0101M), 2));

                //41, 12
                var labelNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0213M), 2));
                var labelNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                //5, 49
                var labelNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0027M), 2));
                var labelNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0453M), 2));

                //709, 20
                var txtNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3692M), 2));
                var txtNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0185M), 2));

                //7, 68
                var txtNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0036M), 2));
                var txtNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0629M), 2));

                //136, 20
                var elementsAvailableWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0708M), 2));
                var elementsAvailableHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0186M), 2));

                //5, 163
                var elementsAvailableX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0026M), 2));
                var elementsAvailableY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1509M), 2));

                //31, 12
                var lblPresetWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0161M), 2));
                var lblPresetHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                //6, 205
                var lblPresetX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0031M), 2));
                var lblPresetY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1898M), 2));

                //99, 12
                var labelTimeWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0515M), 2));
                var labelTimeHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0111M), 2));

                //6, 100 
                var labelTimeX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0031M), 2));
                var labelTimeY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0925M), 2));

                //480, 32
                var presetObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.25M), 2));
                var presetObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.02962M), 2));

                //4, 224
                var presetObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0020M), 2));
                var presetObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2074M), 2));

                //328, 32
                var timeObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1708M), 2));
                var timeObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0296M), 2));

                //7, 114
                var timeObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0064M), 2));
                var timeObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1055M), 2));

                //328, 32
                var unitObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1708M), 2));
                var unitObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0296M), 2));

                //402, 114
                var unitObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2093M), 2));
                var unitObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1055M), 2));

                //121, 37
                var ButtonAddWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.063M), 2));
                var ButtonAddHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0345M), 2));

                //599, 224
                var ButtonAddX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3119M), 2));
                var ButtonAddY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2074M), 2));

                //460, 8
                var bunifuSeparator2Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2395M), 2));
                var bunifuSeparator2Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0074M), 2));

                //7, 252
                var bunifuSeparator2X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0036M), 2));
                var bunifuSeparator2Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2333M), 2));

                //311, 8
                var bunifuSeparator1Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1619M), 2));
                var bunifuSeparator1Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0074M), 2));

                //9, 144
                var bunifuSeparator1X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0046M), 2));
                var bunifuSeparator1Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1333M), 2));

                //311, 8
                var bunifuSeparator3Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1619M), 2));
                var bunifuSeparator3Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0074M), 2));

                //405, 144
                var bunifuSeparator3X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2109M), 2));
                var bunifuSeparator3Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1333M), 2));

                //127, 13
                var elementosSeleccionadosWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0661M), 2));
                var elementosSeleccionadosHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.012M), 2));

                //562, 263
                var elementosSeleccionadosX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2890M), 2));
                var elementosSeleccionadosY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2435M), 2));

                //720, 170
                var bunifuDataObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.375M), 2));
                var bunifuDataObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1574M), 2));

                //0, 283
                var bunifuDataObjectX = 0;
                var bunifuDataObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2620M), 2));

                //277, 460
                var panelPagX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1442M), 2));
                var panelPagY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.4259M), 2));

                // Edición de propiedades segun resolución
                if (main.Width == 1024 && main.Height == 768)
                {
                    txtNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3692M), 2)) - 15;
                    txtNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0185M), 2)) + 2;

                    ButtonAddX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3130M), 2)) - 15;

                    bunifuDataObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.375M), 2)) - 15;
                }

                panel1.Size = new Size(panel1Width, panel1Height);
                tilteForm.Location = new Point(tilteFormX, tilteFormY);
                labelName.Size = new Size(panel1Width, panel1Height);
                labelName.Location = new Point(labelNameX, labelNameY);
                txtName.Size = new Size(txtNameWidth, txtNameHeight);
                txtName.MinimumSize = new Size(txtNameWidth, txtNameHeight);
                txtName.Location = new Point(txtNameX, txtNameY);
                elementsAvailable.Size = new Size(elementsAvailableWidth, elementsAvailableHeight);
                elementsAvailable.Location = new Point(elementsAvailableX, elementsAvailableY);
                lblPreset.Size = new Size(lblPresetWidth, lblPresetHeight);
                lblPreset.Location = new Point(lblPresetX, lblPresetY);
                labelTime.Size = new Size(labelTimeWidth, labelTimeHeight);
                labelTime.Location = new Point(labelTimeX, labelTimeY);
                presetObject.Size = new Size(presetObjectWidth, presetObjectHeight);
                presetObject.Location = new Point(presetObjectX, presetObjectY);
                timeObject.Size = new Size(timeObjectWidth, timeObjectHeight);
                timeObject.Location = new Point(timeObjectX, timeObjectY);
                unitObject.Size = new Size(unitObjectWidth, unitObjectHeight);
                unitObject.Location = new Point(unitObjectX, unitObjectY);
                ButtonAdd.Size = new Size(ButtonAddWidth, ButtonAddHeight);
                ButtonAdd.Location = new Point(ButtonAddX, ButtonAddY);
                bunifuSeparator2.Size = new Size(bunifuSeparator2Width, bunifuSeparator2Height);
                bunifuSeparator2.Location = new Point(bunifuSeparator2X, bunifuSeparator2Y);
                bunifuSeparator1.Size = new Size(bunifuSeparator1Width, bunifuSeparator1Height);
                bunifuSeparator1.Location = new Point(bunifuSeparator1X, bunifuSeparator1Y);
                bunifuSeparator3.Size = new Size(bunifuSeparator3Width, bunifuSeparator3Height);
                bunifuSeparator3.Location = new Point(bunifuSeparator3X, bunifuSeparator3Y);
                elementosSeleccionados.Size = new Size(elementosSeleccionadosWidth, elementosSeleccionadosHeight);
                elementosSeleccionados.Location = new Point(elementosSeleccionadosX, elementosSeleccionadosY);
                bunifuDataObject.Size = new Size(bunifuDataObjectWidth, bunifuDataObjectHeight);
                bunifuDataObject.Location = new Point(bunifuDataObjectX, bunifuDataObjectY);
                panelPag.Location = new Point(panelPagX, panelPagY);

                this.presetObject.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                this.timeObject.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                this.unitObject.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

            }
        }

        private List<DataViewGuard> PagePagination(int Page)
        {
            var pageItem = new List<DataViewGuard>();
            var item = controlPag.PropTake;
            var items = item * Page;
            int index = (Page == 1 ? 0 : (items - item));

            if (devices.Count > 0)
            {
                for (int i = index; i < items && i <= (devices.Count - 1); i++)
                {
                    pageItem.Add(devices[i]);
                }
            }

            return pageItem;

        }

        private void pagination(int page)
        {
            dataView.Add(new DataViewGuard { });
            dataView.Clear();
            bunifuDataObject.DataSource = typeof(List<DataViewGuard>);
            dataView.AddRange(PagePagination(page));
            bunifuDataObject.DataSource = dataView;
        }
        private void OnClickNextPage(int page)
        {
            pagination(page);
        }
        private void OnClickBackPage(int page)
        {

            pagination(page);
        }
        private void OnClickStartPage(int page)
        {

            pagination(page);
        }
        private void OnClickEndPage(int page)
        {
            pagination(page);
        }

        private void bunifuDataObject_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == SpeedColumn.Index || e.ColumnIndex == TimeColumn.Index || e.ColumnIndex == UnitTimeColumn.Index)
                {
                    var otem = (DataViewGuard)bunifuDataObject.Rows[e.RowIndex].DataBoundItem;
                    var dt = (List<OptionItemDTO<int>>)((System.Windows.Forms.DataGridViewComboBoxCell)bunifuDataObject.Rows[e.RowIndex].Cells[e.ColumnIndex]).DataSource;
                    var newSelect = bunifuDataObject.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString();
                    var Select = bunifuDataObject.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue.ToString();
                    var oSpeed = dt.SingleOrDefault(a => a.Name == newSelect);
                    if (e.ColumnIndex == SpeedColumn.Index)
                    {
                        if (newSelect != Select)
                        {

                            if (oSpeed != null)
                            {
                                if (otem.Id == 0)
                                {
                                    devices.SingleOrDefault(d => d.Order == otem.Order).Speed = oSpeed.Key;
                                }
                                else
                                {
                                    devices.SingleOrDefault(d => d.Id == otem.Id).Speed = oSpeed.Key;
                                }
                            }

                        }
                    }

                    if (e.ColumnIndex == TimeColumn.Index)
                    {
                        if (newSelect != Select)
                        {
                            if (oSpeed != null)
                            {
                                if (otem.Id == 0)
                                {
                                    devices.SingleOrDefault(d => d.Order == otem.Order).Time = oSpeed.Key;
                                }
                                else
                                {
                                    devices.SingleOrDefault(d => d.Id == otem.Id).Time = oSpeed.Key;
                                }
                            }

                        }
                    }

                    if (e.ColumnIndex == UnitTimeColumn.Index)
                    {
                        if (newSelect != Select)
                        {
                            if (oSpeed != null)
                            {
                                if (otem.Id == 0)
                                {
                                    devices.SingleOrDefault(d => d.Order == otem.Order).UnitTime = oSpeed.Key;
                                }
                                else
                                {
                                    devices.SingleOrDefault(d => d.Id == otem.Id).UnitTime = oSpeed.Key;
                                }
                            }

                        }
                    }
                }
            }
        }
    }
}
