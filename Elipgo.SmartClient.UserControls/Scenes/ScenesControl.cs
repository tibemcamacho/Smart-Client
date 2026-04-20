using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Drivers;
using Elipgo.SmartClient.UserControls.GenericForm;
using Elipgo.SmartClient.UserControls.Groups;
using Elipgo.SmartClient.ViewModels;
using Splat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using static Elipgo.SmartClient.Common.Enum.TypeAlarms;
using static Elipgo.SmartClient.Services.Client.Client;
using static IdentityModel.ClaimComparer;

namespace Elipgo.SmartClient.UserControls.Scenes
{
    public partial class ScenesControl : GenericContentComponent
    {
        public List<DataViewScenes> dataView = new List<DataViewScenes>();
        public event EventHandler<ScenesControl> SelectSiteObject;
        public event EventHandler<ScenesControl> SelectDeviceObjectName;
        private readonly IDriverFactory DriverFactory = Locator.Current.GetService<IDriverFactory>();
        private readonly ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        private ScenesEntity _entity;
        public int cantSeleccionados = 0;

        private GenericFormPagination controlPag;
        //private bool _resizeLoad = false;
        private List<DataViewScenes> devices = new List<DataViewScenes>();
        public short pageOptionSiteObject = 1;
        public short pageOptionDevicesObject = 0;
        public string textSiteSearch = string.Empty;
        public string textsearch = string.Empty;
        //public string filter = string.Empty;
        private List<OptionObjectDTO> _listOptionSites = new List<OptionObjectDTO>();
        private List<OptionObjectDTO> _listIotsOptionNames = new List<OptionObjectDTO>();
        private short _takeDropdown = 0;
        public string SiteNameSelectValue
        {
            get => ((OptionObjectDTO)this.ucSitioiotObject.SelectedItem).Key.ToString() ?? string.Empty;
            //set => this.ucSitioiotObject.SelectedValue = value;
        }
        public ScenesControl()
        {
            base.Configuration = new ConfigGenericForm
            {
                ObjectBarSelected = LiveBarButtom.scenes,
                NameEntity = Resources.scenes,
                IconEntity = FileResources.icon_scenes,
                CanEditOrCreate = true,
                CanPrivate = false,
                CanMultiSelect = false,

            };
#pragma warning disable CS0612 // Type or member is obsolete
            InitializeComponent();
#pragma warning restore CS0612 // Type or member is obsolete
            //_resizeLoad = true;
            var _config = SmartClientEnvironmentUtils.GetConfiguration();
            _takeDropdown = Int16.Parse(_config.AppSettings.Settings["takeDropdown"].Value);
            LoadPresetGuard();
            bunifuDataObject.AutoGenerateColumns = false;
            txtName.DataBindings.Add(new Binding("Text", this.bindingSourceScenes, "Name", true));
            txtNote.DataBindings.Add(new Binding("Text", this.bindingSourceScenes, "Note", true));
            elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);
            tilteForm.Text = Resources.newScene;
            labelName.Text = Resources.Name;
            labelNote.Text = Resources.note;
            elementsAvailable.Text = Resources.IotAvailable;
            ButtonAdd.Text = Resources.buttonAdd;
            labelDevices.Text = Resources.Devices + " / " + Resources.Camera;
            labelAction.Text = Resources.GlobalAction;
            labelSitio.Text = Resources.SiteText;

            ActionColumn.DisplayMember = "Name";
            ActionColumn.ValueMember = "Key";

            bunifuDataObject.RowHeadersVisible = false;
            bunifuDataObject.RowsDefaultCellStyle.SelectionBackColor = Color.Transparent;
            bunifuDataObject.RowTemplate.Height = 28;
            bunifuDataObject.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold);
            bunifuDataObject.CurrentTheme.HeaderStyle.SelectionBackColor = Color.FromArgb(15, 16, 18);
            bunifuDataObject.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 16, 18);

            bunifuDataObject.Columns[1].HeaderText = Resources.Device;
            bunifuDataObject.Columns[2].HeaderText = Resources.preset + " / " + Resources.guard;
            bunifuDataObject.Columns[3].HeaderText = Resources.action;
            bunifuDataObject.Columns[4].HeaderText = Resources.delete;
            bunifuDataObject.Columns[5].HeaderText = Resources.order;
            bunifuDataObject.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            bunifuDataObject.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            controlPag = new GenericFormPagination(1);
            controlPag.OnClickNextPage += OnClickNextPage;
            controlPag.OnClickBackPage += OnClickBackPage;
            controlPag.OnClickStartPage += OnClickStartPage;
            controlPag.OnClickEndPage += OnClickEndPage;
            panelPagi.Controls.Add(controlPag);
            panelPagi.Visible = false;
            ucSitioiotObject.SearchRequested += UcOptionSiteName_Search;
            uciotObject.SearchRequested += UcOptionName_Search;
            ControlsResize();
        }

        private void FormScenes_load(object sender, EventArgs e)
        {
            if (ScenesViewModel != null)
            {
                actionsObject.DataSource = ScenesViewModel.GetActions();
                actionsObject.DisplayMember = "Name";
                actionsObject.ValueMember = "Key";
                ActionColumn.DataSource = ScenesViewModel.GetActions();
            }
        }
        public ScenesViewModel ScenesViewModel => (ViewModel as ScenesViewModel);

        public override Task<List<GenericForm.ContentFormDTO>> GetDataSource(Action<List<GenericForm.ContentFormDTO>> callback)
        {
            return Task.Run(async () =>
            {
                var data = await ScenesViewModel.GetScenes();
                return data?.Where(p => !p.IsDeleted).Select
                     (
                       p => new GenericForm.ContentFormDTO
                       {
                           Label1 = p.Name,
                           EntityIcon = FileResources.icon_scenes,
                           IsPrivate = false,
                           Id = p.Id
                       }
                     ).ToList() ?? new List<GenericForm.ContentFormDTO>();
            });
        }

        public override void Clear()
        {
            dataView.Add(new DataViewScenes { });
            bunifuDataObject.DataSource = typeof(List<DataViewScenes>);
            bunifuDataObject.DataSource = dataView;

            bindingSourceScenes.Clear();
            errorManager.Clear();
            dataView.Clear();

            _entity = new ScenesEntity();
            bindingSourceScenes.DataSource = _entity;
            bunifuDataObject.DataSource = typeof(List<DataViewScenes>);
            bunifuDataObject.DataSource = dataView;

            errorManager.SetError(txtName, Resources.required);
            cantSeleccionados = 0;
            elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);
            panelPagi.Visible = false;
            devices = new List<DataViewScenes>();
            controlPag.UpdatePage(0, 1);
            ControlsResize();
        }

        public async void AddElement()
        {
            if (!uciotObject.isSearchingMode && !ucSitioiotObject.isSearchingMode)
            {

                var iot = ((OptionObjectDTO)uciotObject.SelectedItem);
                var action = ((OptionObjectDTO)actionsObject.SelectedItem);
                if (devices != null && !devices.Exists(d => d.ObjectId == int.Parse(iot.Key)))
                {
                    string name = string.Empty;
                    int key = 0;
                    int? ObjectSubId = null;
                    if (presetGuardiaObject.Visible)
                    {
                        key = ((KeyValuePair<int, string>)presetGuardiaObject.SelectedItem).Key;

                        if ((SubType)key == SubType.Preset || (SubType)key == SubType.Guard)
                        {
                            if (presetListGuardiaObject.SelectedItem != null)
                            {
                                name = presetListGuardiaObject.SelectedItem is PresetDTO ? ((PresetDTO)presetListGuardiaObject.SelectedItem).Name : ((GuardDTO)presetListGuardiaObject.SelectedItem).Name;
                                ObjectSubId = presetListGuardiaObject.SelectedItem is PresetDTO ? ((PresetDTO)presetListGuardiaObject.SelectedItem).Id : ((GuardDTO)presetListGuardiaObject.SelectedItem).Id;
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    var data = new DataViewScenes
                    {
                        Id = 0,
                        Action = Convert.ToInt32(action.Key),
                        ActionName = action.Name,
                        DeviceName = iot.Name,
                        NameObjectSubType = !string.IsNullOrEmpty(name) ? labelListPresetGuardia.Text + " " + name : string.Empty,
                        IsDeleted = false,
                        ObjectId = Convert.ToInt32(iot.Key),
                        Order = devices.Count + 1,
                        ElementType = (iot.Item as CatalogIot).Type,
                        ObjectSubType = (SubType)key,
                        ObjectSubId = ObjectSubId
                    };

                    dataView.Add(data);
                    bunifuDataObject.DataSource = typeof(List<DataViewGroup>);
                    bunifuDataObject.DataSource = dataView;
                    bunifuDataObject.EndEdit();
                    devices.Add(data);
                    this.ucSitioiotObject.Enabled = !(dataView.Count > 0 || (SelectedItem != null && SelectedItem.Item.Id > 0));
                    separatorSitios.Visible = ucSitioiotObject.Enabled;

                    cantSeleccionados = devices.Count;
                    elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);

                    panelPagi.Visible = (devices.Count > 0);
                    controlPag.UpdatePage(devices.Count, 1, true);
                    if (controlPag.PropTotalPage > 1)
                    {
                        controlPag.endPage();
                    }

                    pagination(controlPag.PropTotalPage);
                    GenericFormControl.EnabledButtonAccep(cantSeleccionados >= 2);
                    //SetOptionsDevicesByDvfsSelected();
                    await ChangeDisplayControls();
                }
                else
                {
                    //MessageBox.Show(Resources.registered_Item, Resources.action, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    notification.Show(Resources.registered_Item, null);
                }
            }
        }

        public void ScenesControl_Load(List<OptionObjectDTO> option)
        {
            if (pageOptionSiteObject == 1)
            {
                this.ucSitioiotObject.Items.Clear();
                _listOptionSites.Clear();
                this.uciotObject.DataSource = null;
                this.uciotObject.SwitchToSelectionMode();
                this.textsearch = string.Empty;
                this.ucSitioiotObject.DataSource = null;
            }
            _listOptionSites.RemoveAll(x => x.Key == "0");
            this.ucSitioiotObject.Items.Clear();
            if (option.Count != 0 && option[0].count > _takeDropdown)
            {
                float totalpage = (int)Math.Ceiling((double)option[0].count / _takeDropdown);
                if (totalpage > pageOptionSiteObject)
                {
                    var seemore = new OptionObjectDTO { Key = "0", Name = Resources.ViewMore };
                    option.Add(seemore);
                }
            }
            //List<CatalogSite> dataSourceSite = ScenesViewModel.Catalog.Sites.Where(x => x.Iots.Count > 0 ||
            //                                                       x.Cameras.Where(c => c.LiveReadPTZ == true).Count() > 0).ToList();
            _listOptionSites.AddRange(option);
            foreach (var item in _listOptionSites)
            {
                this.ucSitioiotObject.Items.Add(item);
                this.ucSitioiotObject.ValueMember = "Id";
                this.ucSitioiotObject.DisplayMember = "Name";
            }
            //this.sitioiotObject.Items.Insert(0, new CatalogSite { Id = 0, Name = Resources.All });
            if (this.ucSitioiotObject.Items.Count > 0)
            {
                this.ucSitioiotObject.SelectedIndex = 0;
            }

            if (ScenesViewModel != null)
            {
                actionsObject.DataSource = ScenesViewModel.GetActions();
                ActionColumn.DataSource = ScenesViewModel.GetActions();
            }

            Logger.Log(string.Format("Listado de acciones disponibles: {0}", (actionsObject.DataSource as ICollection)?.Count ?? 0), LogPriority.Information);
        }

        private async void sitioiotObject_SelectedValueChanged(object sender, EventArgs e)
        {
            if (ucSitioiotObject.SelectedItem != null && ((OptionObjectDTO)ucSitioiotObject.SelectedItem).Key == "0")
            {
                pageOptionSiteObject++;
                GetSiteObjectNames();
            }
            else
            {
                pageOptionDevicesObject = 1;
                GetOptionsDevicesByDvfsSelected();

                //await ChangeDisplayControls();
            }
        }
        private void GetOptionsDevicesByDvfsSelected()
        {
            this.SelectDeviceObjectName?.Invoke(null, this);
        }

        public void SetOptionsDevicesByDvfsSelected(List<OptionObjectDTO> listIots)
        {
            uciotObject.DataSource = null;
            var siteSelected = (OptionObjectDTO)ucSitioiotObject.SelectedItem;
            List<OptionObjectDTO> optionObjectDTO = new List<OptionObjectDTO>();
            List<CatalogSite> filterSites = new List<CatalogSite>();
            optionObjectDTO = listIots;
            //if (siteSelected.Key != "0")
            //{
            //    optionObjectDTO = ScenesViewModel.GetIots(int.Parse(siteSelected.Key),"DO").ToList();
            //    //filterSites = ScenesViewModel.Catalog.Sites.Where(s => s.Id == int.Parse(siteSelected.Key)).ToList();
            //}
            //else
            //{
            //    optionObjectDTO = ScenesViewModel.GetIots(0,"DO");
            //    //filterSites = ScenesViewModel.Catalog.Sites.ToList();
            //}

            //Si el tipo no es DO en primera opción de SITIO en el catalogo, se buscan cuales sitios si tiene tipo DO
            //se hacen los filtros y se rellena nuevamente el combo de sitios
            //if (optionObjectDTO.Count == 0)
            //{
            //    optionObjectDTO = ScenesViewModel.GetIots();
            //    List<int> sitesWithDO = new List<int>();
            //    int _countSiteDO = 0;
            //    foreach (var item in optionObjectDTO)
            //    {
            //        sitesWithDO.Add((int)item.Tag);
            //        _countSiteDO++;
            //    }
            //    filterSites = ScenesViewModel.Catalog.Sites.Where(o => sitesWithDO.Contains(o.Id)).ToList();
            //    this.ucSitioiotObject.Items.Clear();
            //    foreach (var item in filterSites)
            //    {
            //        this.ucSitioiotObject.Items.Add(item);
            //        this.ucSitioiotObject.ValueMember = "Id";
            //        this.ucSitioiotObject.DisplayMember = "Name";
            //    }
            //    this.ucSitioiotObject.SelectedIndex = 0;
            //}


            //foreach (var item in filterSites)
            //{
            //    var camerasPtz = item.Cameras.Where(c => c.LiveReadPTZ == true).ToList();
            //    if (camerasPtz != null && camerasPtz.Count > 0)
            //    {
            //        var optionObjectDTOPtz = camerasPtz.Select(c => new OptionObjectDTO
            //        {
            //            Key = c.ObjectId.ToString(),
            //            Name = c.DeviceName,
            //            Item = new CatalogIot()
            //            {
            //                ObjectId = c.ObjectId,
            //                Name = c.DeviceName,
            //                Type = c.Type,
            //                SubType = c.Type,
            //                LocationLatitude = 0,
            //                LocationLongitude = 0
            //            },
            //            Tag = item.Id,
            //            IsPtz = true
            //        }).ToList();
            //        optionObjectDTO.AddRange(optionObjectDTOPtz);
            //    }
            //}

            //var optionsItemDto = optionObjectDTO.Select(x => x.Item as CatalogIot).Where(x => devices.Select(dvf => dvf.ObjectId).Contains(x.ObjectId)).ToList();

            if (pageOptionDevicesObject == 1)
            {
                this.uciotObject.DataSource = new List<OptionObjectDTO>();
                this.uciotObject.SwitchToSelectionMode();
                _listIotsOptionNames.Clear();
                if (this.ucSitioiotObject.isSearchingMode)
                    this.ucSitioiotObject.SwitchToSelectionMode();
            }
            _listIotsOptionNames.RemoveAll(x => x.Key == "0");
            if (listIots.Count != 0 && listIots[0].count > _takeDropdown)
            {
                float totalpage = (int)Math.Ceiling((double)listIots[0].count / _takeDropdown);
                if (totalpage > pageOptionDevicesObject)
                {
                    var seemore = new OptionObjectDTO { Key = "0", Name = Resources.ViewMore };
                    listIots.Add(seemore);
                }
            }
            _listIotsOptionNames.AddRange(listIots);
            uciotObject.DataSource = _listIotsOptionNames;// optionObjectDTO.Where(option => !optionsItemDto.Contains(option.Item as CatalogIot)).ToList();
            this.uciotObject.DisplayMember = "Name";
            this.uciotObject.ValueMember = "Key";
        }

        private async void iotObject_SelectedValueChanged(object sender, EventArgs e)
        {
            if (uciotObject.SelectedItem != null && ((OptionObjectDTO)uciotObject.SelectedItem).Key == "0")
            {
                pageOptionDevicesObject++;
                GetOptionsDevicesByDvfsSelected();
            }
            else
            {
                await ChangeDisplayControls();
            }
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            AddElement();
        }

        public override async Task<GenericForm.ContentFormDTO> SaveOrUpdate()
        {
            if (!IsCompleted())
            {
                return null;
            }

            bindingSourceScenes.EndEdit();

            _entity.EntityId = (int)ScenesViewModel.EntityId;

            //"devices": es la variable donde se almacenan todos los dispositivos de las diferentes paginas.
            var dataSource = devices; //(bunifuDataObject.DataSource as List<DataViewScenes>);
            var device = new List<SceneElementEntity>();

            if (dataSource != null)
            {

                var order = 0;
                dataSource.ForEach(i =>
                {

                    device.Add
                    (
                        new SceneElementEntity
                        {
                            Action = i.Action,
                            Id = i.Id,
                            ObjectId = i.ObjectId,
                            IsDeleted = false,
                            ObjectType = i.ElementType,
                            Order = order++,
                            SceneId = _entity.Id,
                            ObjectSubType = i.ObjectSubType,
                            NameObjectSubType = i.NameObjectSubType,
                            ObjectSubId = i.ObjectSubId
                        }
                    );
                });

                _entity.Elements = device.ToArray();
            }

            var obj = await ScenesViewModel.SaveOrUpdate(_entity);

            if (obj == null)
            {
                return null;
            }

            return new GenericForm.ContentFormDTO
            {
                EntityIcon = Configuration.IconEntity,
                Id = obj.Id,
                IsActive = true,
                IsPrivate = false,
                Label1 = obj.Name
            };
        }

        private bool IsCompleted()
        {
            return !(string.IsNullOrEmpty(txtName.Text));
        }

        public override async Task<bool> Edit()
        {
            if (SelectedItemOption != null)
            {
                var item = await ScenesViewModel.GetScene(SelectedItemOption.Id);

                if (item != null)
                {
                    bindingSourceScenes.Clear();
                    errorManager.Clear();

                    _entity = item;
                    bindingSourceScenes.DataSource = _entity;
                    bindingSourceScenes.ResetBindings(false);

                    dataView.Clear();
                    var d = await ScenesViewModel.GetElements(_entity.Elements.Where(p => !p.IsDeleted).OrderBy(p => p.Order).ToArray());
                    devices = d.ToList();
                    dataView.AddRange(d);

                    cantSeleccionados = devices.Count();
                    elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);

                    if (dataView.Count == 0)
                    {
                        dataView.Add(new DataViewScenes { });
                        bunifuDataObject.DataSource = dataView;
                        dataView.Clear();
                    }

                    bunifuDataObject.DataSource = typeof(List<DataViewScenes>);
                    bunifuDataObject.DataSource = PagePagination(1);
                    this.ucSitioiotObject.Enabled = !(dataView.Count > 0 || (SelectedItem != null && SelectedItem.Item.Id > 0));

                    var visib = (devices.Count >= 1);
                    panelPagi.Visible = visib;
                    controlPag.UpdatePage(devices.Count, 1);
                    GenericFormControl.EnabledButtonAccep(cantSeleccionados >= 2);

                    return true;
                }
            }

            return false;
        }

        public override async Task<bool> Delete()
        {
            if (SelectedItemOption != null)
            {
                return await ScenesViewModel.Delete(SelectedItemOption.Id);
            }

            return true;
        }

        public override async Task<bool> Execute()
        {
            if (SelectedItemOption != null)
            {
                await Execute(SelectedItemOption.Id);
            }

            return true;
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

        private async void BunifuDataObject_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
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
                        var item = bunifuDataObject.Rows[i].DataBoundItem as DataViewScenes;
                        var id = Convert.ToInt32(item.Id);
                        cantSeleccionados--;
                        elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);
                        if (id == 0 || await ScenesViewModel.DeleteElementOfScene(id))
                        {
                            dataView.Remove(bunifuDataObject.Rows[i].DataBoundItem as DataViewScenes);
                            devices.Remove(item);
                        }
                    }
                }
                if (showMessage)
                {
                    bunifuDataObject.DataSource = typeof(List<DataViewGroup>);
                    bunifuDataObject.DataSource = dataView;
                }
                this.ucSitioiotObject.Enabled = !(dataView.Count > 0 || (SelectedItem != null && SelectedItem.Item.Id > 0));
                separatorSitios.Visible = ucSitioiotObject.Enabled;

                //SetOptionsDevicesByDvfsSelected();
                await ChangeDisplayControls();
                GenericFormControl.EnabledButtonAccep(cantSeleccionados >= 2);
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

        private void BunifuDataObject_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == order.Index && e.RowIndex != -1)
            {
                startDragAndDrop = true;
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

        private void LoadPresetGuard()
        {
            Dictionary<int, string> dc = new Dictionary<int, string>();
            dc.Add(1, Resources.preset);
            dc.Add(2, Resources.guard);
            presetGuardiaObject.DataSource = new BindingSource(dc, null);
            presetGuardiaObject.DisplayMember = "Value";
            presetGuardiaObject.ValueMember = "Key";
        }

        private async void presetGuardiaObject_SelectedValueChanged(object sender, EventArgs e)
        {
            await GetListPresetGuards();
        }

        private async Task GetListPresetGuards()
        {
            if (presetGuardiaObject.Visible)
            {
                errorManagerPresetGuard.Clear();
                labelListPresetGuardia.Text = ((KeyValuePair<int, string>)presetGuardiaObject.SelectedItem).Value;
                var selectedItem = (OptionObjectDTO)uciotObject.SelectedItem;


                int key = ((KeyValuePair<int, string>)presetGuardiaObject.SelectedItem).Key;
                List<OptionObjectDTO> actions = ScenesViewModel != null ? ScenesViewModel.GetActions().Where(f => key == 1 ? f.Key == "1" : (f.Key == "0" || f.Key == "1")).ToList() : new List<OptionObjectDTO>();

                switch ((SubType)key)
                {
                    case SubType.Preset:
                        List<PresetDTO> dataSourcePreset = ScenesViewModel != null ? (await ScenesViewModel.GetPresets(Convert.ToInt32(selectedItem.Key))) : null;
                        if (dataSourcePreset != null && dataSourcePreset.Count > 0)
                        {
                            presetListGuardiaObject.DataSource = new BindingSource(dataSourcePreset, null);
                            presetListGuardiaObject.DisplayMember = "Name";
                            presetListGuardiaObject.ValueMember = "Id";
                        }
                        else
                        {
                            errorManagerPresetGuard.SetError(presetListGuardiaObject, Resources.required);
                            presetListGuardiaObject.DataSource = null;
                        }

                        break;
                    case SubType.Guard:
                        List<GuardDTO> dataSourceGuards = ScenesViewModel != null ? await ScenesViewModel.GetGuards(Convert.ToInt32(selectedItem.Key)) : null;
                        if (dataSourceGuards != null && dataSourceGuards.Count > 0)
                        {
                            presetListGuardiaObject.DataSource = new BindingSource(dataSourceGuards, null);
                            presetListGuardiaObject.DisplayMember = "Name";
                            presetListGuardiaObject.ValueMember = "Id";
                        }
                        else
                        {
                            errorManagerPresetGuard.SetError(presetListGuardiaObject, Resources.required);
                            presetListGuardiaObject.DataSource = null;
                        }

                        break;
                    default:
                        break;
                }
                actionsObject.DataSource = actions;
            }
        }

        private void DisplayCotrols(bool show)
        {
            labelPresetGuardia.Visible = show;
            presetGuardiaObject.Visible = show;
            separatorPresetGuarda.Visible = show;

            labelListPresetGuardia.Visible = show;
            presetListGuardiaObject.Visible = show;
            separatorListPresetGuarda.Visible = show;
        }

        private async Task ChangeDisplayControls()
        {
            var selectedItem = (OptionObjectDTO)uciotObject.SelectedItem;
            separatorSitios.Visible = ucSitioiotObject.Enabled;

            if (Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                //if (selectedItem.IsPtz != null)
                if (selectedItem != null && (bool)selectedItem.IsPtz)
                {
                    int key = ((KeyValuePair<int, string>)presetGuardiaObject.SelectedItem).Key;

                    if (ScenesViewModel != null)
                    {
                        List<OptionObjectDTO> actions = ScenesViewModel.GetActions().Where(f => key == 1 ? f.Key == "1" : (f.Key == "0" || f.Key == "1")).ToList();
                        actionsObject.DataSource = actions;
                    }

                    //583, 192
                    var ButtonAddX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3036M), 2));
                    var ButtonAddY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1777M), 2));



                    //3, 162
                    var elementsAvailableX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0015M), 2));
                    var elementsAvailableY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.15M), 2));
                    elementsAvailable.Location = new Point(elementsAvailableX, elementsAvailableY);

                    //11, 187
                    var labelSitioX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0057M), 2));
                    var labelSitioY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1731M), 2));
                    labelSitio.Location = new Point(labelSitioX, labelSitioY);

                    //7, 196
                    var sitioiotObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0036M), 2));
                    var sitioiotObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1814M), 2));


                    //10, 222
                    var separatorSitiosX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0057M), 2));
                    var separatorSitiosY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2055M), 2));
                    separatorSitios.Location = new Point(separatorSitiosX, separatorSitiosY);

                    //194, 187
                    var labelDevicesX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1010M), 2));
                    var labelDevicesY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1731M), 2));
                    labelDevices.Location = new Point(labelDevicesX, labelDevicesY);

                    //196, 196
                    var iotObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1020M), 2));
                    var iotObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1814M), 2));
                    uciotObject.Location = new Point(iotObjectX, iotObjectY);

                    //196, 222
                    var separatorDispositivosX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1020M), 2));
                    var separatorDispositivosY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2055M), 2));
                    separatorDispositivos.Location = new Point(separatorDispositivosX, separatorDispositivosY);
                    separatorDispositivos.BringToFront();

                    //364, 183
                    var labelPresetGuardiaX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1895M), 2));
                    var labelPresetGuardiaY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1694M), 2));
                    labelPresetGuardia.Location = new Point(labelPresetGuardiaX, labelPresetGuardiaY);
                    labelPresetGuardia.BringToFront();

                    //366, 196
                    var presetGuardiaObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1906M), 2));
                    var presetGuardiaObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1814M), 2));
                    presetGuardiaObject.Location = new Point(presetGuardiaObjectX, presetGuardiaObjectY);

                    //366, 222 
                    var separatorPresetGuardaX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1906M), 2));
                    var separatorPresetGuardaY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2055M), 2));
                    separatorPresetGuarda.Location = new Point(separatorPresetGuardaX, separatorPresetGuardaY);
                    separatorPresetGuarda.BringToFront();

                    //12, 236
                    var labelListPresetGuardiaX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0062M), 2));
                    var labelListPresetGuardiaY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2185M), 2));
                    labelListPresetGuardia.Location = new Point(labelListPresetGuardiaX, labelListPresetGuardiaY);
                    labelListPresetGuardia.BringToFront();

                    //7, 250
                    var presetListGuardiaObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0036M), 2));
                    var presetListGuardiaObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2259M), 2));
                    presetListGuardiaObject.Location = new Point(presetListGuardiaObjectX, presetListGuardiaObjectY);

                    //11, 272
                    var separatorListPresetGuardaX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0057M), 2));
                    var separatorListPresetGuardaY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2518M), 2));
                    separatorListPresetGuarda.Location = new Point(separatorListPresetGuardaX, separatorListPresetGuardaY);
                    separatorListPresetGuarda.BringToFront();

                    //194, 236
                    var labelActionX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1010M), 2));
                    var labelActionY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2185M), 2));
                    labelAction.Location = new Point(labelActionX, labelActionY);

                    //196, 244
                    var actionsObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1020M), 2));
                    var actionsObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2259M), 2));
                    actionsObject.Location = new Point(actionsObjectX, actionsObjectY);

                    //196, 272
                    var separatorActionsX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1020M), 2));
                    var separatorActionsY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2518M), 2));
                    separatorActions.Location = new Point(separatorActionsX, separatorActionsY);
                    separatorActions.BringToFront();

                    DisplayCotrols(true);
                }
                else
                {
                    if (ScenesViewModel != null)
                    {
                        actionsObject.DataSource = ScenesViewModel.GetActions();
                    }

                    //11, 196
                    var labelSitioX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0059M), 2));
                    var labelSitioY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1818M), 2));

                    //11, 216
                    var sitioiotObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0058M), 2));
                    var sitioiotObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));

                    //11, 244
                    var separatorSitiosX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0058M), 2));
                    var separatorSitiosY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2));

                    //194, 201
                    var labelDevicesX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1010M), 2));
                    var labelDevicesY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1861M), 2));

                    //196, 216
                    var iotObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1020M), 2));
                    var iotObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));

                    //196, 244
                    var separatorDispositivosX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1020M), 2));
                    var separatorDispositivosY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2));

                    //378, 201
                    var labelActionX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1968M), 2));
                    var labelActionY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1861M), 2));

                    //380, 216
                    var actionsObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1979M), 2));
                    var actionsObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));

                    //380, 244
                    var separatorActionsX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1979M), 2));
                    var separatorActionsY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2));

                    ////583, 211
                    var ButtonAddX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3036M), 2));
                    var ButtonAddY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1953M), 2));

                    //3, 162
                    var elementsAvailableX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0015M), 2));
                    var elementsAvailableY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.150M), 2));
                    elementsAvailable.Location = new Point(elementsAvailableX, elementsAvailableY);

                    //7, 250
                    var presetGuardiaObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0036M), 2));
                    var presetGuardiaObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2314M), 2));
                    presetGuardiaObject.Location = new Point(presetGuardiaObjectX, presetGuardiaObjectY);

                    //11, 273
                    var separatorPresetGuardaX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0058M), 2));
                    var separatorPresetGuardaY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.253M), 2));
                    separatorPresetGuarda.Location = new Point(separatorPresetGuardaX, separatorPresetGuardaY);

                    //196, 273
                    var separatorListPresetGuardaX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1020M), 2));
                    var separatorListPresetGuardaY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2527M), 2));
                    separatorListPresetGuarda.Location = new Point(separatorListPresetGuardaX, separatorListPresetGuardaY);

                    if (main.Width == 1024 && main.Height == 768)
                    {
                        labelSitioX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0059M), 2));
                        labelSitioY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1818M), 2)) - 5;
                        sitioiotObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0060M), 2));
                        sitioiotObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2)) - 5;
                        separatorSitiosX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0066M), 2));
                        separatorSitiosY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2)) - 3;

                        labelDevicesX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1250M), 2));
                        labelDevicesY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1861M), 2)) - 5;
                        iotObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1251M), 2));
                        iotObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2)) - 5;
                        separatorDispositivosX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1257M), 2));
                        separatorDispositivosY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2)) - 3;

                        labelActionX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2598M), 2));
                        labelActionY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1861M), 2)) - 5;
                        actionsObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2599M), 2));
                        actionsObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2)) - 5;
                        separatorActionsX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2606M), 2));
                        separatorActionsY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2)) - 3;

                        ButtonAddX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3130M), 2)) - 15;
                        ButtonAddY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2)) + 23;
                    }
                    else if ((main.Width > 2020 && main.Width < 2100) && (main.Height > 1200 && main.Height < 1300))
                    {
                        ButtonAddX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3036M), 2)) - 20;
                    }

                    ButtonAdd.Location = new Point(ButtonAddX, ButtonAddY);

                    labelSitio.Location = new Point(labelSitioX, labelSitioY);
                    ucSitioiotObject.Location = new Point(sitioiotObjectX, sitioiotObjectY);
                    separatorSitios.Location = new Point(separatorSitiosX, separatorSitiosY);

                    labelDevices.Location = new Point(labelDevicesX, labelDevicesY);
                    uciotObject.Location = new Point(iotObjectX, iotObjectY);
                    separatorDispositivos.Location = new Point(separatorDispositivosX, separatorDispositivosY);

                    labelAction.Location = new Point(labelActionX, labelActionY);
                    actionsObject.Location = new Point(actionsObjectX, actionsObjectY);
                    separatorActions.Location = new Point(separatorActionsX, separatorActionsY);

                    DisplayCotrols(false);
                }
                await GetListPresetGuards();
            }
        }

        void BunifuDataObject_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void bunifuDataObject_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var row = bunifuDataObject.CurrentCell.RowIndex;
            var rowData = (List<DataViewScenes>)((DataGridViewComboBoxEditingControl)e.Control).EditingControlDataGridView.Columns[6].DataGridView.DataSource;
            if (rowData[row].ObjectSubType != SubType.Default)
            {
                var dgv = (ComboBox)e.Control;
                var DataSourceActions = ScenesViewModel.GetActions().Where(f => (rowData[row].ObjectSubType == SubType.Preset) ? f.Key == "1" : (f.Key == "0" || f.Key == "1")).ToList();
                dgv.DataSource = DataSourceActions;
                dgv.ValueMember = "Key";
                dgv.DisplayMember = "Name";
            }
        }

        public override async void DobleClick(GenericForm.ContentFormDTO element)
        {
            await Execute(element.Id);
        }

        private async Task<bool> Execute(int idScene)
        {
            var item = await ScenesViewModel.GetScene(idScene);
            foreach (var element in item.Elements)
            {
                if (element.ObjectSubType == SubType.Preset)
                {
                    var preset = new PresetDTO() { Id = Convert.ToInt32(element.ObjectSubId), Name = element.NameObjectSubType.Split(' ')[1] };
                    var cameraDTO = await ScenesViewModel.GetCamera(element.ObjectId);
                    if (cameraDTO != null)
                    {
                        DriverFactory.GetDriverLive(cameraDTO, Profile.MainStream, false, Guid.NewGuid().ToString()).CallPreset(preset);
                    }
                }
            }
            return await ScenesViewModel.Execute(idScene);
        }

        public async void ControlsResize()
        {
            if (Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                #region Estilos

                if (main.Width > 1400 && main.Width < 2000)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    txtNote.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);

                    ucSitioiotObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    uciotObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    actionsObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    presetGuardiaObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    presetListGuardiaObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersHeight = 40;
                    //bunifuDataObject.RowTemplate.Height = 40;
                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 30;
                    ButtonAdd.OnIdleState.BorderRadius = 30;

                    //lblTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Large_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    //IndexEntity.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);

                }
                else if (main.Width >= 1366 && main.Width < 1400)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    txtNote.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementsAvailable.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    ucSitioiotObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    uciotObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    actionsObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    presetGuardiaObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    presetListGuardiaObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.Font = FontHelper.GetRobotoRegular(FontSizes.Small_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersDefaultCellStyle.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Bold, GraphicsUnit.Pixel);
                    bunifuDataObject.RowsDefaultCellStyle.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersHeight = 30;
                    bunifuDataObject.RowTemplate.Height = 30;
                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 20;
                    ButtonAdd.OnIdleState.BorderRadius = 20;
                    //GroupName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);


                }
                else if (main.Width >= 2000 && main.Width < 2560)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    txtNote.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);

                    ucSitioiotObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    uciotObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    actionsObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    presetGuardiaObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    presetListGuardiaObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersHeight = 40;
                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Small_8, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 30;
                    ButtonAdd.OnIdleState.BorderRadius = 30;
                }
                else if (main.Width >= 2560 && main.Width <= 3440)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    txtNote.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);

                    ucSitioiotObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    uciotObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    actionsObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    presetGuardiaObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    presetListGuardiaObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersHeight = 40;
                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Small_9, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 40;
                    ButtonAdd.OnIdleState.BorderRadius = 40;
                }
                else if (main.Width == 1024 && main.Height == 768)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Bold, GraphicsUnit.Pixel);
                    tilteForm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    txtNote.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);

                    elementsAvailable.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Bold, GraphicsUnit.Pixel);

                    ucSitioiotObject.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    uciotObject.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    actionsObject.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);

                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 20;
                    ButtonAdd.OnIdleState.BorderRadius = 20;

                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);

                    bunifuDataObject.Font = FontHelper.GetRobotoRegular(FontSizes.Small_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersDefaultCellStyle.Font = FontHelper.GetRobotoRegular(FontSizes.Small_2, FontStyle.Bold, GraphicsUnit.Pixel);
                    bunifuDataObject.RowsDefaultCellStyle.Font = FontHelper.GetRobotoRegular(FontSizes.Small_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersHeight = 30;
                    bunifuDataObject.RowTemplate.Height = 30;
                    bunifuDataObject.AutoResizeColumns();
                }

                #endregion

                //744, 508
                var panel1Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3875M), 2));
                var panel1Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.4703M), 2));

                //4, 2
                var panel1X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.002M), 2));
                var panel1Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.002M), 2));

                //15, 11 
                var tilteFormX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0078M), 2));
                var tilteFormY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0101M), 2));

                //41, 12
                var labelNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0215M), 2));
                var labelNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                //7, 40
                var labelNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0036M), 2));
                var labelNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0370M), 2));

                //26, 12   
                var labelNoteWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0135M), 2));
                var labelNoteHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                //12, 99
                var labelNoteX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.00625M), 2));
                var labelNoteY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0916M), 2));

                //696, 28
                var txtNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3625M), 2));
                var txtNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0259M), 2));

                //txtName.MinimumSize = new Size(txtNameWidth, txtNameHeight);
                //8, 57
                var txtNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0041M), 2));
                var txtNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0527M), 2));

                //696, 28
                var txtNoteWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3625M), 2));
                var txtNoteHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0259M), 2));

                //8, 114
                var txtNoteX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0041M), 2));
                var txtNoteY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1055M), 2));

                //112, 20 
                var elementsAvailableWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0583M), 2));
                var elementsAvailableHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0185M), 2));

                ////3, 162
                //var elementsAvailableX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0015M), 2));
                //var elementsAvailableY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.150M), 2));
                //elementsAvailable.Location = new Point(elementsAvailableX, elementsAvailableY);

                //27, 12
                var labelSitioWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.014M), 2));
                var labelSitioHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                ////11, 196
                //var labelSitioX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0059M), 2));
                //var labelSitioY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1818M), 2));
                //labelSitio.Location = new Point(labelSitioX, labelSitioY);

                //56, 12 
                var labelDevicesWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0293M), 2));
                var labelDevicesHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                ////194, 201
                //var labelDevicesX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1010M), 2));
                //var labelDevicesY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1861M), 2));
                //labelDevices.Location = new Point(labelDevicesX, labelDevicesY);

                //66, 12
                var labelActionWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0345M), 2));
                var labelActionHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                ////378, 201
                //var labelActionX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1968M), 2));
                //var labelActionY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1861M), 2));
                //labelAction.Location = new Point(labelActionX, labelActionY);

                //144, 32
                var sitioiotObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.075M), 2));
                var sitioiotObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

                ////11, 216
                //var sitioiotObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0058M), 2));
                //var sitioiotObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));
                //ucSitioiotObject.Location = new Point(sitioiotObjectX, sitioiotObjectY);

                //160, 32
                var iotObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0833M), 2));
                var iotObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

                //196, 216
                //var iotObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1020M), 2));
                //var iotObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));
                //uciotObject.Location = new Point(iotObjectX, iotObjectY);

                //144, 32
                var actionsObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.075M), 2));
                var actionsObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

                ////380, 216
                //var actionsObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1979M), 2));
                //var actionsObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));
                //actionsObject.Location = new Point(actionsObjectX, actionsObjectY);

                //121, 37
                var ButtonAddWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.063M), 2));
                var ButtonAddHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0345M), 2));

                ////583, 211
                //var ButtonAddX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3036M), 2));
                //var ButtonAddY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1953M), 2));
                //ButtonAdd.Location = new Point(ButtonAddX, ButtonAddY);

                //143, 8
                var separatorSitiosWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0744M), 2));
                var separatorSitiosHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0074M), 2));

                //11, 244
                //var separatorSitiosX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0058M), 2));
                //var separatorSitiosY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2));
                //separatorSitios.Location = new Point(separatorSitiosX, separatorSitiosY);

                //158, 8
                var separatorDispositivosWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0822M), 2));
                var separatorDispositivosHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0074M), 2));

                //196, 244
                //var separatorDispositivosX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1020M), 2));
                //var separatorDispositivosY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2));
                //separatorDispositivos.Location = new Point(separatorDispositivosX, separatorDispositivosY);

                //130, 8
                var separatorActionsWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0677M), 2));
                var separatorActionsHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0074M), 2));

                //380, 244
                //var separatorActionsX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1979M), 2));
                //var separatorActionsY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2));
                //separatorActions.Location = new Point(separatorActionsX, separatorActionsY);

                //144, 32
                var presetGuardiaObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.075M), 2));
                var presetGuardiaObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.02870M), 2));

                ////7, 250
                //var presetGuardiaObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0036M), 2));
                //var presetGuardiaObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2314M), 2));
                //presetGuardiaObject.Location = new Point(presetGuardiaObjectX, presetGuardiaObjectY);

                //144, 32
                var presetListGuardiaObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.075M), 2));
                var presetListGuardiaObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.02870M), 2));

                //196, 250
                var presetListGuardiaObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1020M), 2));
                var presetListGuardiaObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2314M), 2));

                //127, 13
                var elementosSeleccionadosWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.066M), 2));
                var elementosSeleccionadosHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.012M), 2));

                //562, 263
                var elementosSeleccionadosX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2890M), 2));
                var elementosSeleccionadosY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2435M), 2));

                //130, 8
                var separatorPresetGuardaWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0677M), 2));
                var separatorPresetGuardaHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0064M), 2));

                //11, 273
                //var separatorPresetGuardaX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0058M), 2));
                //var separatorPresetGuardaY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.253M), 2));
                //separatorPresetGuarda.Location = new Point(separatorPresetGuardaX, separatorPresetGuardaY);

                //130, 10
                var separatorListPresetGuardaWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0677M), 2));
                var separatorListPresetGuardaHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.009M), 2));

                ////196, 273
                //var separatorListPresetGuardaX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1020M), 2));
                //var separatorListPresetGuardaY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2527M), 2));
                //separatorListPresetGuarda.Location = new Point(separatorListPresetGuardaX, separatorListPresetGuardaY);

                //696, 180
                var bunifuDataObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3625M), 2));
                var bunifuDataObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1574M), 2));

                //8, 287
                var bunifuDataObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0041M), 2));
                var bunifuDataObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2675M), 2));

                //252, 463
                var panelPagX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1312M), 2));
                var panelPagY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.42870M), 2));

                this.ucSitioiotObject.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                this.uciotObject.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                this.actionsObject.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                this.presetGuardiaObject.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                this.presetListGuardiaObject.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

                // Edición de propiedades segun resolución
                if (main.Width == 1024 && main.Height == 768)
                {
                    labelNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0036M), 2));
                    labelNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0370M), 2));

                    txtNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3530M), 2));
                    txtNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0259M), 2));

                    txtNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0041M), 2));
                    txtNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0527M), 2));


                    labelNoteWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0135M), 2));
                    labelNoteHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                    labelNoteX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.00625M), 2));
                    labelNoteY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0916M), 2));

                    txtNoteWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3530M), 2));
                    txtNoteHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0259M), 2));

                    txtNoteX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0036M), 2));
                    txtNoteY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1070M), 2));

                    sitioiotObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1155M), 2));
                    sitioiotObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                    separatorSitiosWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1050M), 2));

                    iotObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1315M), 2));
                    iotObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                    separatorDispositivosWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1220M), 2));

                    actionsObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1090M), 2));
                    actionsObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                    separatorActionsWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1005M), 2));

                    elementosSeleccionadosWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.066M), 2));
                    elementosSeleccionadosHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.012M), 2));
                    elementosSeleccionadosX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.002M), 2));
                    elementosSeleccionadosY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2435M), 2)) + 5;

                }
                else if (main.Width == 2048 && main.Height == 1280)
                {
                    tilteFormX = 45;
                }

                panel1.Size = new Size(panel1Width, panel1Height);
                panel1.Location = new Point(panel1X, panel1Y);
                tilteForm.Location = new Point(tilteFormX, tilteFormY);
                labelName.Size = new Size(labelNameWidth, labelNameHeight);
                labelName.Location = new Point(labelNameX, labelNameY);
                labelNote.Size = new Size(labelNoteWidth, labelNoteHeight);
                labelNote.Location = new Point(labelNoteX, labelNoteY);
                txtName.Size = new Size(txtNameWidth, txtNameHeight);
                txtName.Location = new Point(txtNameX, txtNameY);
                txtNote.Size = new Size(txtNoteWidth, txtNoteHeight);
                txtNote.Location = new Point(txtNoteX, txtNoteY);
                elementsAvailable.Size = new Size(elementsAvailableWidth, elementsAvailableHeight);
                labelSitio.Size = new Size(labelSitioWidth, labelSitioHeight);
                labelDevices.Size = new Size(labelDevicesWidth, labelDevicesHeight);
                labelAction.Size = new Size(labelActionWidth, labelActionHeight);
                ucSitioiotObject.Size = new Size(sitioiotObjectWidth, sitioiotObjectHeight);
                uciotObject.Size = new Size(iotObjectWidth, iotObjectHeight);
                actionsObject.Size = new Size(actionsObjectWidth, actionsObjectHeight);
                ButtonAdd.Size = new Size(ButtonAddWidth, ButtonAddHeight);
                separatorSitios.Size = new Size(separatorSitiosWidth, separatorSitiosHeight);
                separatorDispositivos.Size = new Size(separatorDispositivosWidth, separatorDispositivosHeight);
                separatorDispositivos.BringToFront();
                separatorActions.Size = new Size(separatorActionsWidth, separatorActionsHeight);
                presetGuardiaObject.Size = new Size(presetGuardiaObjectWidth, presetGuardiaObjectHeight);
                presetListGuardiaObject.Size = new Size(presetListGuardiaObjectWidth, presetListGuardiaObjectHeight);
                presetListGuardiaObject.Location = new Point(presetListGuardiaObjectY, presetListGuardiaObjectX);
                elementosSeleccionados.Size = new Size(elementosSeleccionadosWidth, elementosSeleccionadosHeight);
                elementosSeleccionados.Location = new Point(elementosSeleccionadosX, elementosSeleccionadosY);
                separatorPresetGuarda.Size = new Size(separatorPresetGuardaWidth, separatorPresetGuardaHeight);
                separatorListPresetGuarda.Size = new Size(separatorListPresetGuardaWidth, separatorListPresetGuardaHeight);
                bunifuDataObject.Size = new Size(bunifuDataObjectWidth, bunifuDataObjectHeight);
                bunifuDataObject.Location = new Point(bunifuDataObjectX, bunifuDataObjectY);
                panelPagi.Location = new Point(panelPagX, panelPagY);

                await ChangeDisplayControls();
                //DisplayCotrols(false);

                //_resizeLoad = false;
            }
        }

        private void separatorDispositivos_Load(object sender, EventArgs e)
        {

        }

        private void labelAction_Click(object sender, EventArgs e)
        {

        }

        private List<DataViewScenes> PagePagination(int Page)
        {
            var pageItem = new List<DataViewScenes>();
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
            dataView.Add(new DataViewScenes { });
            dataView.Clear();
            bunifuDataObject.DataSource = typeof(List<DataViewScenes>);
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
                if (e.ColumnIndex == ActionColumn.Index)
                {
                    var element = new DataViewScenes();
                    var otem = (DataViewScenes)bunifuDataObject.Rows[e.RowIndex].DataBoundItem;
                    if (otem.Id != 0)
                    {
                        element = devices.SingleOrDefault(d => d.Id == otem.Id);
                    }
                    else
                    {
                        element = devices.SingleOrDefault(d => d.Order == otem.Order);
                    }

                    var dt = (List<OptionObjectDTO>)((System.Windows.Forms.DataGridViewComboBoxCell)bunifuDataObject.Rows[e.RowIndex].Cells[e.ColumnIndex]).DataSource;
                    var newAction = bunifuDataObject.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString();
                    if (newAction != element.ActionName)
                    {
                        var oAction = dt.SingleOrDefault(a => a.Name == newAction);
                        if (oAction != null)
                        {
                            if (otem.Id == 0)
                            {
                                devices.SingleOrDefault(d => d.Order == otem.Order).ActionStr = oAction.Key;
                                devices.SingleOrDefault(d => d.Order == otem.Order).ActionName = oAction.Name;
                            }
                            else
                            {
                                devices.SingleOrDefault(d => d.Id == otem.Id).ActionStr = oAction.Key;
                                devices.SingleOrDefault(d => d.Id == otem.Id).ActionName = oAction.Name;
                            }
                        }

                    }
                }
            }
        }


        //private void GetNames()
        //{
        //    this.SelectObject?.Invoke(null, this);
        //}
        private void GetSiteObjectNames()
        {
            this.SelectSiteObject?.Invoke(null, this);
        }
        private void GetObjectNames()
        {
            this.SelectDeviceObjectName?.Invoke(null, this);
        }


        private void UcOptionSiteName_Search(object sender, string textSearchs)
        {
            this.textSiteSearch = textSearchs;
            GetSiteObjectNames();
        }

        private void UcOptionName_Search(object sender, string textSearchs)
        {
            this.textsearch = textSearchs;
            GetObjectNames();
        }
    }
}
