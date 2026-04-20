using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services;
using Elipgo.SmartClient.UserControls.GenericForm;
using Elipgo.SmartClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Cameras
{
    public partial class GuardDahuaControl : GenericContentComponent
    {
        private GuardDTO _entity;

        public List<DataViewGuard> dataView = new List<DataViewGuard>();

        private List<GuardDTO> _guardList = new List<GuardDTO>();

        public int cantSeleccionados = 0;

        //private GenericFormPagination controlPag;

        private List<DataViewGuard> devices = new List<DataViewGuard>();

        public GuardDahuaControl()
        {
            base.Configuration = new ConfigGenericForm
            {
                ObjectBarSelected = LiveBarButtom.guard,
                NameEntity = Resources.guard,
                IconEntity = FileResources.icon_guards,
                CanEditOrCreate = false,
                CanDelete = false,
                CanPrivate = false,
                CanMultiSelect = false,
                ShowAddButton = false,
                ShowSwitch = true
            };

            InitializeComponent();

            DataGridViewItems.AutoGenerateColumns = false;
            LabelSelectedItems.Text = string.Format(Resources.elementSelected, cantSeleccionados);
            LabelTitle.Text = Resources.editGuard;
            labelName.Text = Resources.Name;
            elementsAvailable.Text = Resources.presetAvailable;
            ButtonAdd.Text = Resources.buttonAdd;
            LabelPreset.Text = Resources.preset;
            LabelDahuaGuardNote.Text = Resources.GuardTourDahuaNote;

            DataGridViewItems.RowHeadersVisible = false;
            DataGridViewItems.RowsDefaultCellStyle.SelectionBackColor = Color.Transparent;
            DataGridViewItems.RowTemplate.Height = 28;
            DataGridViewItems.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold);
            DataGridViewItems.CurrentTheme.HeaderStyle.SelectionBackColor = Color.FromArgb(15, 16, 18);
            DataGridViewItems.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 16, 18);

            DataGridViewItems.Columns[1].HeaderText = Resources.font;
            DataGridViewItems.Columns[2].HeaderText = Resources.delete;
            DataGridViewItems.Columns[3].HeaderText = Resources.order;
            ControlsResize();
        }

        public GuardViewModel GuardViewModel => (ViewModel as GuardViewModel);

        public override void Clear()
        {
            cantSeleccionados = 0;

            LabelGuardName.Text = string.Empty;

            dataView.Add(new DataViewGuard { });
            DataGridViewItems.DataSource = typeof(List<DataViewGuard>);
            DataGridViewItems.DataSource = dataView;
            dataView.Clear();

            _entity = new GuardDTO();
            DataGridViewItems.DataSource = typeof(List<DataViewGuard>);
            DataGridViewItems.DataSource = dataView;

            errorManager.SetError(LabelGuardName, Resources.required);
        }

        public override async Task<bool> Delete()
        {
            if (SelectedItem != null)
            {
                //GuardViewModel.Driver.RemoveGuard(SelectedItem.Item.ObjectOrigin as GuardDTO);
                await Vmon5Service.DeleteGuard(GuardViewModel.Driver.Camera.Id, ((IGenericViewModel)this.ViewModel).Token, SelectedItem.Item.ObjectOrigin as GuardDTO);
            }
            return false;
        }

        public void AddElement()
        {
            var preset = DropdownPresets.SelectedItem as PresetDTO;

            var data = new DataViewGuard
            {
                Id = 0,
                Preset = preset,
                IsDeleted = false,
                Order = dataView.Count,
            };

            dataView.Add(data);
            DataGridViewItems.DataSource = typeof(List<DataViewGuard>);
            DataGridViewItems.DataSource = dataView;
            DataGridViewItems.EndEdit();
            devices.Add(data);
        }

        public override async Task<bool> Edit()
        {
            if (SelectedItem != null)
            {
                errorManager.Clear();
                //var editEntity = GuardViewModel.Driver.GetGuard(SelectedItem.Item.Id);
                var editEntity = await Vmon5Service.GetGuard(GuardViewModel.Driver.Camera.Id, SelectedItem.Item.Id, ((IGenericViewModel)this.ViewModel).Token);
                this._entity = new GuardDTO() { Id = editEntity.Id };

                dataView.Clear();
                DataGridViewItems.DataSource = typeof(List<DataViewGuard>);
                LabelGuardName.Text = editEntity.Name;
                return true;
            }
            return false;
        }

        private bool IsCompleted()
        {
            return !(string.IsNullOrEmpty(LabelGuardName.Text));
        }

        public override async Task<GenericForm.ContentFormDTO> SaveOrUpdate()
        {
            if (!IsCompleted())
            {
                return null;
            }

            DataGridViewItems.EndEdit();

            var source = devices;//(DataGridViewItems.DataSource as List<DataViewGuard>);
            var preset = new List<GuardTourForCreationDTO>();

            var newEntity = new GuardForCreationDTO();

            newEntity.Name = LabelGuardName.Text;
            if (source != null)
            {
                source.ForEach(i =>
                {
                    preset.Add
                    (
                        new GuardTourForCreationDTO
                        {
                            PresetId = i.Preset.Id,
                            Speed = i.Speed,
                            Time = i.Time,
                            ViewOrder = i.Order + 1,
                            WaitTimeViewType = i.UnitTime == 0 ? WaitTimeViewType.Seconds : WaitTimeViewType.Minutes
                        }
                    );
                });
                newEntity.GuardTours = preset.ToArray();
            }

            if (_entity.Id > -1)
            {
                newEntity.Id = _entity.Id;
            }
            string message = "";
            //var result = GuardViewModel.Driver.SaveGuard(newEntity);
            var (result, msg) = await Vmon5Service.SaveGuard(GuardViewModel.Driver.Camera.Id, ((IGenericViewModel)this.ViewModel).Token, newEntity, message);
            message = msg.ToString();
            if (!result)
            {
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
            });
        }

        private void GuardControl_Load(object sender, EventArgs e)
        {
            DropdownPresets.DataSource = GuardViewModel.Driver.ListPresets();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            cantSeleccionados++;
            LabelSelectedItems.Text = string.Format(Resources.elementSelected, cantSeleccionados);
            AddElement();
        }

        private void DataGridViewItems_DataError(object sender, System.Windows.Forms.DataGridViewDataErrorEventArgs e)
        {

        }

        Image img = null;

        void DataGridViewItems_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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

        private void DataGridViewItems_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewItems.EndEdit();

            bool showMessage = false;
            if (e.RowIndex == -1 && e.ColumnIndex == deleteDataGridViewCheckBoxColumn.Index)
            {
                for (int i = DataGridViewItems.Rows.Count - 1; i >= 0; i--)
                {
                    if ((bool)DataGridViewItems.Rows[i].Cells[deleteDataGridViewCheckBoxColumn.Index].Value == true)
                    {
                        if (!showMessage)
                        {
                            if (MessageBox.Show(Resources.deleteItemsGrid, Resources.delete, MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.Cancel)
                            {
                                return;
                            }

                            showMessage = true;
                        }
                        var item = DataGridViewItems.Rows[i].DataBoundItem as DataViewGuard;
                        cantSeleccionados--;
                        LabelSelectedItems.Text = string.Format(Resources.elementSelected, cantSeleccionados);
                        dataView.Remove(item);

                    }
                }
                if (showMessage)
                {
                    DataGridViewItems.DataSource = typeof(List<DataViewGuard>);
                    DataGridViewItems.DataSource = dataView;
                }
            }
        }

        private void DataGridViewItems_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if ((e.RowIndex == -1 && e.ColumnIndex == deleteDataGridViewCheckBoxColumn.Index) || (e.RowIndex != -1 && e.ColumnIndex == order.Index))
                {
                    DataGridViewItems.Cursor = Cursors.Hand;
                }
                else
                {
                    DataGridViewItems.Cursor = Cursors.Default;
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
            return result;
        }

        private Rectangle dragBoxFromMouseDown;

        private int rowIndexFromMouseDown;

        private int rowIndexOfItemUnderMouseToDrop;

        private bool startDragAndDrop = false;

        private void DataGridViewItems_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left && startDragAndDrop)
            {
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    DragDropEffects dropEffect = DataGridViewItems.DoDragDrop(
                             DataGridViewItems.Rows[rowIndexFromMouseDown],
                             DragDropEffects.Move);

                }
            }
        }

        private void DataGridViewItems_MouseDown(object sender, MouseEventArgs e)
        {
            rowIndexFromMouseDown = DataGridViewItems.HitTest(e.X, e.Y).RowIndex;
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

        private void DataGridViewItems_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void DataGridViewItems_DragDrop(object sender, DragEventArgs e)

        {
            Point clientPoint = DataGridViewItems.PointToClient(new Point(e.X, e.Y));

            rowIndexOfItemUnderMouseToDrop = DataGridViewItems.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            if (e.Effect == DragDropEffects.Move)
            {
                var item = dataView.ElementAt(rowIndexFromMouseDown);
                dataView.RemoveAt(rowIndexFromMouseDown);
                dataView.Insert(rowIndexOfItemUnderMouseToDrop, item);
                DataGridViewItems.DataSource = typeof(List<DataViewGroup>);
                DataGridViewItems.DataSource = dataView;
            }

            startDragAndDrop = false;
        }

        public override void DobleClick(GenericForm.ContentFormDTO element)
        {
        }

        private void DataGridViewItems_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == order.Index && e.RowIndex != -1)
            {
                startDragAndDrop = true;
            }
        }

        public void ControlsResize()
        {
            if (Screen.AllScreens.Length >= 2 && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                #region Estilos

                if (main.Width > 1400 && main.Width < 2000)
                {

                    LabelTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Bold, GraphicsUnit.Pixel);
                    LabelDahuaGuardNote.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    DropdownPresets.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);

                    elementsAvailable.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);

                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 30;
                    ButtonAdd.OnIdleState.BorderRadius = 30;

                }
                else if (main.Width >= 1366 && main.Width < 1400)
                {
                    LabelTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    LabelDahuaGuardNote.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    DropdownPresets.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);

                    elementsAvailable.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Bold, GraphicsUnit.Pixel);

                    DataGridViewItems.Font = FontHelper.GetRobotoRegular(FontSizes.Small_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    DataGridViewItems.ColumnHeadersDefaultCellStyle.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Bold, GraphicsUnit.Pixel);
                    DataGridViewItems.RowsDefaultCellStyle.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    DataGridViewItems.ColumnHeadersHeight = 30;
                    DataGridViewItems.RowTemplate.Height = 30;
                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 20;
                    ButtonAdd.OnIdleState.BorderRadius = 20;


                }
                else if (main.Width >= 2000 && main.Width < 2560)
                {
                    LabelTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Bold, GraphicsUnit.Pixel);
                    LabelDahuaGuardNote.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    DropdownPresets.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementsAvailable.Font = FontHelper.GetRobotoRegular(FontSizes.Small_8, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 30;
                    ButtonAdd.OnIdleState.BorderRadius = 30;
                }
                else if (main.Width >= 2560 && main.Width <= 3440)
                {
                    LabelTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Large_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    LabelDahuaGuardNote.Font = FontHelper.GetRobotoRegular(FontSizes.Large_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    DropdownPresets.Font = FontHelper.GetRobotoRegular(FontSizes.Large_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementsAvailable.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Large_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 30;
                    ButtonAdd.OnIdleState.BorderRadius = 30;
                }

                #endregion

                //922, 495
                var panel1Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.480M), 2));
                var panel1Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.458M), 2));
                panel1.Size = new Size(panel1Width, panel1Height);

                //4, 3
                var tilteFormX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.002M), 2));
                var tilteFormY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0027M), 2));
                LabelTitle.Location = new Point(tilteFormX, tilteFormY);


                //41, 12
                var labelNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0213M), 2));
                var labelNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));
                labelName.Size = new Size(panel1Width, panel1Height);
                //5, 84
                var labelNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0027M), 2));
                var labelNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0777M), 2));
                labelName.Location = new Point(labelNameX, labelNameY);

                //460, 23
                var LabelGuardNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2395M), 2));
                var LabelGuardNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0212M), 2));
                LabelGuardName.Size = new Size(LabelGuardNameWidth, LabelGuardNameHeight);

                //7, 101
                var LabelGuardNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0036M), 2));
                var LabelGuardNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0935M), 2));
                LabelGuardName.Location = new Point(LabelGuardNameX, LabelGuardNameY);

                //136, 20
                var elementsAvailableWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0708M), 2));
                var elementsAvailableHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0186M), 2));
                elementsAvailable.Size = new Size(elementsAvailableWidth, elementsAvailableHeight);
                //4, 169
                var elementsAvailableX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.002M), 2));
                var elementsAvailableY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1568M), 2));
                elementsAvailable.Location = new Point(elementsAvailableX, elementsAvailableY);

                //31, 12
                var lblPresetWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0161M), 2));
                var lblPresetHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));
                LabelPreset.Size = new Size(lblPresetWidth, lblPresetHeight);
                //6, 197
                var lblPresetX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0031M), 2));
                var lblPresetY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1824M), 2));
                LabelPreset.Location = new Point(lblPresetX, lblPresetY);

                //121, 37
                var ButtonAddWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.063M), 2));
                var ButtonAddHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0345M), 2));
                ButtonAdd.Size = new Size(ButtonAddWidth, ButtonAddHeight);
                //762, 216
                var ButtonAddX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3968M), 2));
                var ButtonAddY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));
                ButtonAdd.Location = new Point(ButtonAddX, ButtonAddY);

                //460, 8
                var bunifuSeparator2Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2395M), 2));
                var bunifuSeparator2Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0074M), 2));
                bunifuSeparator2.Size = new Size(bunifuSeparator2Width, bunifuSeparator2Height);
                //7, 128
                var bunifuSeparator2X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0036M), 2));
                var bunifuSeparator2Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1185M), 2));
                bunifuSeparator2.Location = new Point(bunifuSeparator2X, bunifuSeparator2Y);

                //460, 8
                var bunifuSeparator1Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2395M), 2));
                var bunifuSeparator1Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0074M), 2));
                bunifuSeparator1.Size = new Size(bunifuSeparator1Width, bunifuSeparator1Height);
                //422, 244
                var bunifuSeparator1X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2197M), 2));
                var bunifuSeparator1Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2259M), 2));
                bunifuSeparator1.Location = new Point(bunifuSeparator1X, bunifuSeparator1Y);


                //480, 32
                var DropdownPresetsWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.25M), 2));
                var DropdownPresetsHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.02962M), 2));
                DropdownPresets.Size = new Size(DropdownPresetsWidth, DropdownPresetsHeight);
                //4, 224
                var DropdownPresetsX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0020M), 2));
                var DropdownPresetsY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2074M), 2));
                DropdownPresets.Location = new Point(DropdownPresetsX, DropdownPresetsY);

                //720, 175
                var DataGridViewItemsWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.375M), 2));
                var DataGridViewItemsHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1620M), 2));
                DataGridViewItems.Size = new Size(DataGridViewItemsWidth, DataGridViewItemsHeight);
                //0, 287
                var DataGridViewItemsX = 0;
                var DataGridViewItemsY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2657M), 2));
                DataGridViewItems.Location = new Point(DataGridViewItemsX, DataGridViewItemsY);

                //247, 467
                var panelPagX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1286M), 2));
                var panelPagY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.4324M), 2));
                panelPag.Location = new Point(DataGridViewItemsX, DataGridViewItemsY);



                this.DropdownPresets.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

            }
        }
    }
}
