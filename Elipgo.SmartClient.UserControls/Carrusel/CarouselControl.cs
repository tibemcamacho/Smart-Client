using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.UserControls.GenericForm;
using Elipgo.SmartClient.UserControls.Groups;
using Elipgo.SmartClient.ViewModels;
using Splat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Carrusel
{
    public partial class CarouselControl : GenericContentComponent
    {
        private readonly ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        public event EventHandler<CarouselControl> SelectObject;
        public event EventHandler<CarouselControl> SelectCameraObject;
        public List<DataViewCarousel> dataView = new List<DataViewCarousel>();
        private bool _resizeLoad = false;
        private ObjectGroupEntity _entity;
        public ObjectGroupEntity ItemGroupSelected { get; set; }

        public int cantSeleccionados = 0;
        private GenericFormPagination controlPag;

        private List<DataViewCarousel> devices = new List<DataViewCarousel>();
        private bool _isprivate = false;
        public short pageOptionSiteObject = 1;
        public short pageOptionDeviceObject = 0;
        public List<OptionObjectDTO> optionAllSites;
        private List<OptionObjectDTO> _listOptionCameraObject = new List<OptionObjectDTO>();
        private List<OptionObjectDTO> _listSiteOptionObjectNames = new List<OptionObjectDTO>();
        private List<OptionObjectDTO> optionCameraObject;
        public string textsearch = string.Empty;
        public string textSiteSearch = string.Empty;
        private short _takeDropdown = 0;
        public string LocationNameSelectValue
        {
            get => this.ucLocationObject.SelectedValue?.ToString() ?? string.Empty;
            set => this.ucLocationObject.SelectedValue = value;
        }
        public CarouselControl()
        {
            base.Configuration = new ConfigGenericForm
            {
                ObjectBarSelected = LiveBarButtom.carousel,
                NameEntity = Resources.carousel,
                IconEntity = FileResources.icon_carruseles,
                CanEditOrCreate = true,
                CanPrivate = false,
                CanMultiSelect = false,
                ShowSwitch = false

            };

            InitializeComponent();
            _resizeLoad = true;
            var _config = SmartClientEnvironmentUtils.GetConfiguration();
            _takeDropdown = Int16.Parse(_config.AppSettings.Settings["takeDropdown"].Value);
            bunifuDataObject.AutoGenerateColumns = false;
            txtName.DataBindings.Add(new Binding("Text", this.bindingSourceCarousel, "Name", true));
            tipoObject.DataBindings.Add(new Binding("SelectedValue", this.bindingSourceCarousel, "IsPrivate", true));
            elementosSeleccionados.Text = String.Format(Resources.elementSelected, cantSeleccionados);
            tilteForm.Text = Resources.newCarousel;
            labelObject.Text = Resources.Object;
            labelName.Text = Resources.Name;
            labelTipo.Text = Resources.type;
            elementsAvailable.Text = Resources.camerasAvailables;
            ButtonAdd.Text = Resources.buttonAdd;
            labelLocation.Text = Resources.location;
            labelCamera.Text = Resources.cameras;

            TimeColumn.DisplayMember = "Name";
            TimeColumn.ValueMember = "Key";
            StreamColumn.DisplayMember = "Name";
            StreamColumn.ValueMember = "Key";

            bunifuDataObject.RowHeadersVisible = false;
            bunifuDataObject.RowsDefaultCellStyle.SelectionBackColor = Color.Transparent;
            bunifuDataObject.RowTemplate.Height = 28;
            bunifuDataObject.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold);
            bunifuDataObject.CurrentTheme.HeaderStyle.SelectionBackColor = Color.FromArgb(15, 16, 18);
            bunifuDataObject.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(15, 16, 18);

            bunifuDataObject.Columns[1].HeaderText = Resources.location;
            bunifuDataObject.Columns[2].HeaderText = Resources.Camera;
            bunifuDataObject.Columns[3].HeaderText = Resources.duration;
            bunifuDataObject.Columns[4].HeaderText = Resources.Stream;
            bunifuDataObject.Columns[5].HeaderText = Resources.delete;
            bunifuDataObject.Columns[6].HeaderText = Resources.order;
            bunifuDataObject.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            bunifuDataObject.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            controlPag = new GenericFormPagination(1);
            controlPag.OnClickNextPage += OnClickNextPage;
            controlPag.OnClickBackPage += OnClickBackPage;
            controlPag.OnClickStartPage += OnClickStartPage;
            controlPag.OnClickEndPage += OnClickEndPage;
            ucLocationObject.SearchRequested += UcOptionlocationName_Search;
            ucCamerasObject.SearchRequested += UcOptionObjectName_Search;
            panelPag.Controls.Add(controlPag);
            panelPag.Visible = false;
            ControlsResize();
        }

        public GroupsViewModel GroupsViewModel => (ViewModel as GroupsViewModel);

        public void SetOptionObject(List<OptionObjectDTO> option)
        {
            //Se eliminan las opciones que no son necesarias para carruceles
            this.optionObject.DataSource = option;
        }

        public void SetLocationObject(List<OptionObjectDTO> option)
        {
            if (pageOptionSiteObject == 1)
            {
                this.ucCamerasObject.DataSource = new List<OptionObjectDTO>();
                this.ucCamerasObject.SwitchToSelectionMode();
                this.textsearch = string.Empty;
                _listSiteOptionObjectNames.Clear();
            }
            this.ucLocationObject.DataSource = new List<OptionObjectDTO>();
            _listSiteOptionObjectNames.RemoveAll(x => x.Key == "0");
            if (option.Count != 0 && option[0].count > _takeDropdown)
            {
                float totalpage = (int)Math.Ceiling((double)option[0].count / _takeDropdown);
                if (totalpage > pageOptionSiteObject)
                {
                    var seemore = new OptionObjectDTO { Key = "0", Name = Resources.ViewMore };
                    option.Add(seemore);
                }
            }
            if (option.Count > 0)
            {
                _listSiteOptionObjectNames.AddRange(option);
                this.ucLocationObject.DataSource = optionAllSites ?? _listSiteOptionObjectNames.ToList();
                this.ucLocationObject.DisplayMember = "Name";
                this.ucLocationObject.ValueMember = "Key";
            }
        }

        public void SetCamerasObject(List<OptionObjectDTO> options)
        {
            if (pageOptionDeviceObject == 1)
            {
                this.ucCamerasObject.SwitchToSelectionMode();
                _listOptionCameraObject.Clear();
                if (this.ucLocationObject.isSearchingMode)
                    this.ucLocationObject.SwitchToSelectionMode();
            }
            this.ucCamerasObject.DataSource = new List<OptionObjectDTO>();
            _listOptionCameraObject.RemoveAll(x => x.Key == "0");
            if (options.Count != 0 && options[0].count > _takeDropdown)
            {
                float totalpage = (int)Math.Ceiling((double)options[0].count / _takeDropdown);
                if (totalpage > pageOptionDeviceObject)
                {
                    var seemore = new OptionObjectDTO { Key = "0", Name = Resources.ViewMore };
                    options.Add(seemore);
                }
            }
            _listOptionCameraObject.AddRange(options);
            this.ucCamerasObject.DataSource = optionCameraObject = _listOptionCameraObject.ToList();
            this.ucCamerasObject.DisplayMember = "Name";
            this.ucCamerasObject.ValueMember = "Key";

        }

        private void GetLocationNames()
        {
            this.SelectObject?.Invoke(null, this);
        }
        private void GetCameraObjectNames()
        {
            this.SelectCameraObject?.Invoke(null, this);
        }

        public override Task<List<GenericForm.ContentFormDTO>> GetDataSource(Action<List<GenericForm.ContentFormDTO>> callback)
        {
            return Task.Run(async () =>
            {
                var data = await GroupsViewModel.GetsCarousel();
                return data?.Select
                     (
                       p => new GenericForm.ContentFormDTO
                       {
                           Label1 = p.Name,
                           EntityIcon = FileResources.icon_carruseles,
                           Switch = p.IsPrivate,
                           Id = p.Id
                       }
                     ).ToList() ?? new List<GenericForm.ContentFormDTO>();
            });
        }

        public override void Clear()
        {
            dataView.Add(new DataViewCarousel { });
            bunifuDataObject.DataSource = typeof(List<DataViewCarousel>);
            bunifuDataObject.DataSource = dataView;

            bindingSourceCarousel.Clear();
            errorManager.Clear();
            dataView.Clear();

            _entity = new ObjectGroupEntity();
            bindingSourceCarousel.DataSource = _entity;
            bunifuDataObject.DataSource = typeof(List<DataViewCarousel>);
            bunifuDataObject.DataSource = dataView;
            cantSeleccionados = 0;
            elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);
            panelPag.Visible = false;
            devices = new List<DataViewCarousel>();
            controlPag.UpdatePage(0, 1);
            errorManager.SetError(txtName, Resources.required);
            optionObject.Enabled = true;
            optionObject.SelectedIndex = 0;
            DisplayCotrolGroup(false);
            TimeColumn.DataSource = GroupsViewModel.GetDuration();
            StreamColumn.DataSource = GroupsViewModel.GetProfilesStream();
            this.tilteForm.Text = Resources.newCarousel;
        }

        public void AddElement()
        {
            var data = new DataViewCarousel();
            var bExistItem = false;

            if (optionObject.SelectedValue.ToString() == "group")
            {

                var groupData = (List<ObjectGroupEntity>)groupObject.DataSource;
                var group = ((ObjectGroupEntity)groupObject.SelectedItem);


                data = new DataViewCarousel
                {
                    Id = devices.Count,
                    Duration = "40",
                    Delete = false,
                    DeviceName = group.Name,
                    Item = group.Id,
                    Order = devices.Count + 1,
                    StreamProfile = ((int)Profile.SubStream).ToString()
                };
                dataView.Add(data);
                devices.Add(data);

                /*
                var groupSelect = (ObjectGroupEntity)groupObject.SelectedValue;
                var item = GroupsViewModel.GetGroup(groupSelect.Id);
                var devicesGroup = item.Elements?.Where(p => !p.IsDeleted).Select(p => GroupsViewModel.GetElement(p)).Where(p => p != null).ToList();

                foreach (var devGroup in devicesGroup)
                {
                    data = new DataViewCarousel
                    {
                        Id = 0,
                        Duration = "15",
                        Delete = false,
                        DeviceName = devGroup.nameObject,
                        Item = devGroup.Id,
                        SiteId = devGroup.siteId,
                        SiteStr = devGroup.siteName,
                        Order = devices.Count + 1,
                        Type = devGroup.type,
                    };
                    dataView.Add(data);
                    devices.Add(data);
                }
                */

                ucCamerasObject.SwitchToSelectionMode();
                ucLocationObject.SwitchToSelectionMode();

                groupObject.DataSource = new List<ObjectGroupEntity>();
                groupData.Remove(group);
                groupObject.DataSource = groupData.ToList();
                groupObject.SelectedIndex = 0;
                GenericFormControl.EnabledButtonOkEvent(dataView.Count > 1);
            }
            else
            {
                if (!ucCamerasObject.isSearchingMode && !ucLocationObject.isSearchingMode
                    && ucCamerasObject.Items.Count > 0 && ucLocationObject.Items.Count > 0)
                {
                    var location = ((OptionObjectDTO)ucLocationObject.SelectedItem);

                    var device = ((OptionObjectDTO)ucCamerasObject.SelectedItem);
                    if (device != null && !devices.Exists(d => d.Item == device.Item))
                    {
                        data = new DataViewCarousel
                        {
                            Id = devices.Count,
                            Duration = "15",
                            Delete = false,
                            DeviceName = device.Name,
                            Item = device.Item,
                            SiteId = location.Key,
                            SiteStr = location.Name,
                            Order = devices.Count + 1,
                            StreamProfile = ((int)Profile.SubStream).ToString()
                        };
                        dataView.Add(data);
                        devices.Add(data);
                        GenericFormControl.EnabledButtonOkEvent(dataView.Count > 1);
                    }
                    else
                        bExistItem = true;
                }
            }

            if (!bExistItem && (!ucCamerasObject.isSearchingMode && !ucLocationObject.isSearchingMode))
            {
                optionObject.Enabled = false;

                bunifuDataObject.DataSource = typeof(List<DataViewCarousel>);
                bunifuDataObject.DataSource = dataView;
                bunifuDataObject.EndEdit();

                cantSeleccionados = devices.Count;
                elementosSeleccionados.Text = String.Format(Resources.elementSelected, cantSeleccionados);

                panelPag.Visible = (devices.Count > controlPag.PropTake);
                controlPag.UpdatePage(devices.Count, 1, true);
                if (controlPag.PropTotalPage > 1)
                {
                    controlPag.endPage();
                }

                pagination(controlPag.PropTotalPage);
            }
            else if (bExistItem)
            {
                //MessageBox.Show(Resources.registered_Item, Resources.action, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                notification.Show(Resources.registered_Item, null);
            }

        }

        private async void CarouselControl_Load(object sender, EventArgs e)
        {
            if (SelectedItem == null)
            {
                tipoObject.DataSource = GroupsViewModel.GetTypeCarousel();
                if (_isprivate)
                {
                    tipoObject.SelectedIndex = tipoObject.FindString("private");
                }
                else
                {
                    tipoObject.SelectedIndex = tipoObject.FindString("public");
                }
            }

            //locationObject.DataSource = GroupsViewModel.GetLocations();
            if (optionObject.SelectedValue.ToString() != "group")
            {
                TimeColumn.DataSource = GroupsViewModel.GetDuration();
                StreamColumn.DataSource = GroupsViewModel.GetProfilesStream();
            }

            groupObject.DataSource = await GroupsViewModel.GetObjectGroup(ItemGroupSelected.UserId, ItemGroupSelected.Type);
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

            bindingSourceCarousel.EndEdit();

            var dataSource = devices;//(bunifuDataObject.DataSource as List<DataViewCarousel>);
            var device = new List<ObjectGroupElementEntity>();

            if (dataSource != null)
            {
                var order = 0;
                dataSource.ForEach(i =>
                {
                    if (optionObject.SelectedValue.ToString() == "group")
                    {
                        device.Add
                        (
                            new ObjectGroupElementEntity
                            {
                                ContainerId = order,
                                Id = i.Id,
                                IsDeleted = false,
                                ObjectId = (int)i.Item,//(i.Item as CatalogCamera).,
                                ObjectGroupId = _entity.Id,
                                //SiteId = Convert.ToInt32(i.SiteId),
                                Time = Convert.ToInt32(i.Duration),
                                Type = "GROUP", //(i.Item as CatalogCamera).Type,
                                Order = order++,
                                ProfileStream = GroupsViewModel.ConvertToProfile(i.StreamProfile)
                            }
                        );
                        order++;

                    }
                    else
                    {
                        device.Add
                        (
                            new ObjectGroupElementEntity
                            {
                                ContainerId = order,
                                Id = i.Id,
                                IsDeleted = false,
                                ObjectId = i.Item.GetType().GetProperty("ObjectId") != null ? (i.Item as CatalogCamera).ObjectId : ((SidebarElementDTO)i.Item).ElementId,
                                ObjectGroupId = _entity.Id,
                                SiteId = Convert.ToInt32(i.SiteId),
                                Time = Convert.ToInt32(i.Duration),
                                Type = i.Item.GetType().GetProperty("DeviceTypeStr") != null ? ((SidebarElementDTO)i.Item).DeviceTypeStr : (i.Item as CatalogCamera).Type,
                                Order = order++
                            }
                        );
                        order++;
                    }
                });

                if (device.ToArray().Length == 0)
                {
                    return null;
                }
                if (_entity == null)
                    _entity = new ObjectGroupEntity();
                _entity.UserIdGuid = GroupsViewModel.UserIdGuid;
                _entity.UserId = (int)GroupsViewModel.UserId;
                _entity.IsPrivate = Convert.ToBoolean(tipoObject.SelectedValue);
                _entity.Type = 2;
                _entity.GridId = "";
                _entity.Elements = device.ToArray();
            }

            var obj = await GroupsViewModel.SaveOrUpdate(_entity);

            if (obj == null)
            {
                return null;
            }

            //if (_entity.Id == 0)
            //{
            //    if (GroupsViewModel.Catalog.Carousels == null)
            //        GroupsViewModel.Catalog.Carousels = new List<CatalogObjectGroup>(); 
            //    GroupsViewModel.Catalog.Carousels.Add(GroupsViewModel.GetsCarouselObject(obj));
            //}
            //else
            //{
            //    var el = GroupsViewModel.Catalog.Carousels.FindIndex(p => p.Id == obj.Id);
            //    GroupsViewModel.Catalog.Carousels.RemoveAt(el);
            //    GroupsViewModel.Catalog.Carousels.Insert(el, GroupsViewModel.GetsCarouselObject(obj));
            //}

            return new GenericForm.ContentFormDTO
            {
                EntityIcon = Configuration.IconEntity,
                Id = obj.Id,
                IsActive = true,
                IsPrivate = obj.IsPrivate,
                Switch = !obj.IsPrivate,
                Label1 = obj.Name,
                DeviceType = Common.Enum.ElementType.Carousel
            };
        }

        private bool IsCompleted()
        {
            return !(String.IsNullOrEmpty(txtName.Text));
        }


        public override async Task<bool> Edit()
        {
            if (SelectedItemOption != null)
            {
                var item = await GroupsViewModel.GetGroup(SelectedItemOption.Id);

                if (item != null)
                {
                    bindingSourceCarousel.Clear();
                    errorManager.Clear();

                    var lisGrupo = item.Elements.Where(g => g.Type == "GROUP").ToList();
                    if (lisGrupo.Count > 0)
                    {
                        var i = item.Elements.Where(g => g.Type == "GROUP").ToArray();
                        item.Elements = (ObjectGroupElementEntity[])item.Elements.Where(g => g.Type == "GROUP").ToArray();
                        //item = (ObjectGroupEntity)lisGrupo;
                        DisplayCotrolGroup(true);
                        optionObject.SelectedIndex = 1;
                        GenericFormControl.EnabledButtonOkEvent(item.Elements.Length > 1);
                    }
                    else
                    {
                        DisplayCotrolGroup(false);
                        optionObject.SelectedIndex = 0;
                        GenericFormControl.EnabledButtonOkEvent(item.Elements.Length > 1);
                    }

                    optionObject.Enabled = false;
                    _entity = item;
                    bindingSourceCarousel.DataSource = _entity;
                    bindingSourceCarousel.ResetBindings(false);

                    dataView.Clear();
                    devices = await GroupsViewModel.GetElementCarousel(_entity.Elements.Where(p => !p.IsDeleted).ToList());


                    cantSeleccionados = devices.Count();
                    elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);

                    dataView.AddRange(await GroupsViewModel.GetElementCarousel(item.Elements
                               .Where(p => !p.IsDeleted)
                               .OrderBy(g => g.ContainerId)
                               .ToList()
                       )
                    );

                    if (dataView.Count == 0)
                    {
                        dataView.Add(new DataViewCarousel { });
                        bunifuDataObject.DataSource = dataView;
                        dataView.Clear();
                    }

                    bunifuDataObject.DataSource = typeof(List<DataViewCarousel>);
                    bunifuDataObject.DataSource = dataView;


                    var ls = new List<OptionObjectDTO>();
                    tipoObject.DataSource = ls;
                    tipoObject.DataSource = GroupsViewModel.GetTypeCarousel(item.IsPrivate);

                    panelPag.Visible = (devices.Count > controlPag.PropTake);
                    controlPag.UpdatePage(devices.Count, 1);
                    pagination(1);
                    if (item.IsPrivate)
                    {
                        _isprivate = true;
                    }
                    else
                    {
                        _isprivate = false;
                    }

                    this.tilteForm.Text = Resources.EditCarousel;
                    return true;
                }
            }

            return false;
        }

        public override async Task<bool> EditSelected()
        {
            /* Esto se activa desde el botón "Desde Grilla". */
            if (ItemGroupSelected != null)
            {
                var item = ItemGroupSelected;
                if (item != null)
                {
                    bindingSourceCarousel.Clear();
                    _entity = item;
                    bindingSourceCarousel.DataSource = _entity;
                    bindingSourceCarousel.ResetBindings(true);
                    errorManager.Clear();

                    //devices = _entity.Elements?.Where(p => !p.IsDeleted).Select(p => GroupsViewModel.GetElementCarousel(p)).Where(p => p != null).ToList();
                    var tasks = _entity.Elements?
                                .Where(p => !p.IsDeleted)
                                .Select(p => GroupsViewModel.GetElementCarousel(p));

                    if (tasks != null)
                    {
                        // 2. Esperamos a que todas terminen en paralelo (usando await)
                        var results = await Task.WhenAll(tasks);

                        // 3. Filtramos los nulos y convertimos a lista final
                        devices = results.Where(p => p != null).ToList();
                    }
                    else
                    {
                        devices = new List<DataViewCarousel>();
                    }

                    dataView.Clear();
                    if (devices != null && devices.Count() > 0)
                    {
                        cantSeleccionados = devices.Count();
                        dataView.AddRange(PagePagination(1));
                    }
                    else
                    {
                        cantSeleccionados = 0;
                    }

                    bunifuDataObject.DataSource = typeof(List<DataViewGroup>);
                    bunifuDataObject.DataSource = dataView;
                    elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);
                    GenericFormControl.EnabledButtonOkEvent(item.Elements.Length > 1);
                    /* NOTA: Se corrige aquí el bug generado cuando se aprieta el botón "Desde grilla" muchas veces para refrescar las variables "Nombre del grupo" y "Grilla seleccionada". */
                    //this.txtName.Text = item.Name; this.optionGrid.SelectedValue = ItemGroupSelected.GridId;
                    panelPag.Visible = (devices.Count > controlPag.PropTake);
                    controlPag.UpdatePage(devices.Count, 1);
                    return true;
                }
            }

            return false;
        }

        public override async Task<bool> Delete()
        {
            if (SelectedItemOption != null)
            {
                var ret = await GroupsViewModel.DeleteGroup(SelectedItemOption.Id);
                //if (ret)
                //{
                //    var el = GroupsViewModel.Catalog.Carousels.FindIndex(p => p.Id == SelectedItemOption.Id);
                //    GroupsViewModel.Catalog.Carousels.RemoveAt(el);
                //}
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
                    if ((bool)(bunifuDataObject.Rows[i].Cells[deleteDataGridViewCheckBoxColumn.Index].Value ?? false) == true)
                    {
                        if (!showMessage)
                        {
                            if (MessageBox.Show(Resources.deleteItemsGrid, Resources.delete, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                            {
                                return;
                            }

                            showMessage = true;
                        }
                        var item = bunifuDataObject.Rows[i].DataBoundItem as DataViewCarousel;
                        int id = 0;
                        if (optionObject.SelectedValue.ToString() == "group")
                        {
                            id = Convert.ToInt32(item.Item);
                        }
                        else
                        {
                            id = Convert.ToInt32(item.Id);
                        }

                        cantSeleccionados--;
                        elementosSeleccionados.Text = String.Format(Resources.elementSelected, cantSeleccionados);
                        if (id == 0 || (await GroupsViewModel.DeleteElementOfGroup(id)))
                        {
                            dataView.Remove(bunifuDataObject.Rows[i].DataBoundItem as DataViewCarousel);
                            try
                            {
                                if (optionObject.SelectedValue.ToString() == "group")
                                {
                                    devices.Remove(devices.SingleOrDefault(d => (int)d.Item == id));
                                }
                                else
                                {
                                    devices.Remove(devices.SingleOrDefault(d => d.Id == id));
                                }
                            }
                            catch
                            {
                                Logger.Log("Error on deleted element in carrousel", LogPriority.Warning);
                            }
                        }
                    }
                }
                if (showMessage)
                {
                    bunifuDataObject.DataSource = typeof(List<DataViewGroup>);
                    bunifuDataObject.DataSource = dataView;
                }
                if (dataView.Count == 0)
                {
                    optionObject.Enabled = true;
                }

                if (optionObject.SelectedValue.ToString() == "group")
                {
                    GenericFormControl.EnabledButtonOkEvent(dataView.Count > 1);
                }
                else
                {
                    GenericFormControl.EnabledButtonOkEvent(dataView.Count > 1);
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

        private void LocationObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ucLocationObject.SelectedValue != null && this.ucLocationObject.SelectedValue.ToString() != "0")
            {
                var valueSiteObject = this.ucLocationObject.SelectedValue.ToString();
                pageOptionDeviceObject = 1;
                this.ucCamerasObject.DataSource = null;
                _listOptionCameraObject.Clear();
                this.textSiteSearch = string.Empty;
                this.ucLocationObject.SwitchToSelectionMode();
                //GetLocationNames();
                GetCameraObjectNames();
            }
            else
            {
                pageOptionSiteObject++;
                GetLocationNames();
            }
        }

        private void CameraObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ucCamerasObject.SelectedValue != null && this.ucCamerasObject.SelectedValue.ToString() == "0")
            {
                pageOptionDeviceObject++;
                GetCameraObjectNames();
            }
        }


        private void TxtName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text))
            {
                errorManager.SetErrorWithCount(txtName, Resources.required);
            }
            else
            {
                errorManager.SetErrorWithCount(txtName, string.Empty);
            }
        }

        private void BunifuDataObject_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        public void ControlsResize()
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                #region Estilos

                if (main.Width > 1400 && main.Width < 2000)
                {

                    //lblTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Large_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    //IndexEntity.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    tipoObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementsAvailable.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    optionObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    groupObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    ucLocationObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    ucCamerasObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    lblWarninMessage.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
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
                    tipoObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementsAvailable.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    optionObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    groupObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    ucLocationObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    ucCamerasObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    lblWarninMessage.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    //optionObjectName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
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
                else if (main.Width >= 2000 && main.Width < 2560)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    tipoObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementsAvailable.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Small_8, FontStyle.Regular, GraphicsUnit.Pixel);
                    optionObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    groupObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    ucLocationObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    ucCamerasObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    lblWarninMessage.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersHeight = 40;
                    //bunifuDataObject.RowTemplate.Height = 40;
                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Small_8, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 30;
                    ButtonAdd.OnIdleState.BorderRadius = 30;
                }
                else if (main.Width >= 2560 && main.Width <= 3440)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    tipoObject.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementsAvailable.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    optionObject.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    groupObject.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    ucLocationObject.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    ucCamerasObject.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    lblWarninMessage.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersHeight = 40;
                    //bunifuDataObject.RowTemplate.Height = 40;
                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 40;
                    ButtonAdd.OnIdleState.BorderRadius = 40;
                }
                else if (main.Width == 1024 && main.Height == 768)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Bold, GraphicsUnit.Pixel);
                    tilteForm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 20;
                    ButtonAdd.OnIdleState.BorderRadius = 20;

                    elementsAvailable.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Bold, GraphicsUnit.Pixel);
                    lblWarninMessage.Font = FontHelper.GetRobotoRegular(FontSizes.Small_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);

                    bunifuDataObject.Font = FontHelper.GetRobotoRegular(FontSizes.Small_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersDefaultCellStyle.Font = FontHelper.GetRobotoRegular(FontSizes.Small_2, FontStyle.Bold, GraphicsUnit.Pixel);
                    bunifuDataObject.RowsDefaultCellStyle.Font = FontHelper.GetRobotoRegular(FontSizes.Small_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersHeight = 30;
                    bunifuDataObject.RowTemplate.Height = 30;
                }

                #endregion

                //744, 493
                var panel1Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3875M), 2));
                var panel1Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.458M), 2));


                //2560 x 1600  125 % 2048  1280
                //15, 11 
                var tilteFormX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0078M), 2));
                var tilteFormY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0101M), 2));
                if ((main.Width > 2020 && main.Width < 2100) && (main.Height > 1200 && main.Height < 1300))
                {
                    tilteForm.Location = new Point(45, 25);
                }
                else
                {
                    tilteForm.Location = new Point(tilteFormX, tilteFormY);
                }

                //41, 12
                var labelNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0215M), 2));
                var labelNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                //5, 58
                var labelNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0027M), 2));
                var labelNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.054M), 2));

                //336, 28
                var txtNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.175M), 2));
                var txtNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0259M), 2));

                //7, 75
                var txtNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0037M), 2));
                var txtNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0696M), 2));

                //24, 12
                var labelTipoWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0125M), 2));
                var labelTipoHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                //401, 58
                var labelTipoX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2088M), 2));
                var labelTipoY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0537M), 2));

                //336, 32
                var tipoObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.175M), 2));
                var tipoObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0296M), 2));

                //398, 74
                var tipoObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2072M), 2));
                var tipoObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0666M), 2));

                //325, 5
                var bunifuSeparator2Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1692M), 2));
                var bunifuSeparator2Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0046M), 2));

                //395, 100
                var bunifuSeparator2X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2057M), 2));
                var bunifuSeparator2Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0925M), 2));

                //152, 20
                var elementsAvailableWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0793M), 2));
                var elementsAvailableHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0186M), 2));

                //4, 152
                var elementsAvailableX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.002M), 2));
                var elementsAvailableY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.141M), 2));

                //35, 12
                var labelObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0182M), 2));
                var labelObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                //11, 197
                var labelObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0057M), 2));
                var labelObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1826M), 2));

                //49, 12
                var labelLocationWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0256M), 2));
                var labelLocationHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                //155, 197
                var labelLocationX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0807M), 2));
                var labelLocationY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1826M), 2));

                //labelCarrusel.
                //35, 12
                var labelCarruselWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0256M), 2));
                var labelCarruselHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                //11, 197
                var labelCarruselX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0807M), 2));
                var labelCarruselY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1826M), 2));

                //42, 12
                var labelCameraWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.022M), 2));
                var labelCameraHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                //383, 197
                var labelCameraX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1994M), 2));
                var labelCameraY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1824M), 2));

                //134, 32
                var optionObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0697M), 2));
                var optionObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0296M), 2));

                //9, 216
                var optionObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0046M), 2));
                var optionObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));

                //214, 32
                var locationObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1114M), 2));
                var locationObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

                //157, 216
                var locationObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0817M), 2));
                var locationObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));

                //carruselObject
                //214, 32
                var carruselObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1114M), 2));
                var carruselObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

                //157, 216
                var carruselObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0817M), 2));
                var carruselObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));

                //198, 32
                var camerasObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1031M), 2));
                var camerasObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

                //385, 216
                var camerasObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2005M), 2));
                var camerasObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));

                //121, 37
                var ButtonAddWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.063M), 2));
                var ButtonAddHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0345M), 2));

                //599, 215
                var ButtonAddX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3119M), 2));
                var ButtonAddY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.199M), 2));

                //117, 8
                var bunifuSeparator4Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0609M), 2));
                var bunifuSeparator4Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0074M), 2));

                //13, 241
                var bunifuSeparator4X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0067M), 2));
                var bunifuSeparator4Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2231M), 2));

                //210, 8
                var bunifuSeparator1Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1114M), 2));
                var bunifuSeparator1Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0074M), 2));

                //157, 241
                var bunifuSeparator1X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0817M), 2));
                var bunifuSeparator1Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2231M), 2));

                //189, 8
                var bunifuSeparator3Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1026M), 2));
                var bunifuSeparator3Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0074M), 2));

                //385, 241
                var bunifuSeparator3X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2005M), 2));
                var bunifuSeparator3Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2231M), 2));

                //lblWarninMessage
                //340, 13
                var lblWarninMessageWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.177M), 2));
                var lblWarninMessageHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.012M), 2));

                //6, 266
                var lblWarninMessageX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0031M), 2));
                var lblWarninMessageY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2466M), 2));

                ////135, 13 -->148
                //var elementosSeleccionadosWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0715M), 2));
                //var elementosSeleccionadosHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.012M), 2));
                //elementosSeleccionados.Size = new Size(elementosSeleccionadosWidth, elementosSeleccionadosHeight);
                ////704, 266
                //var elementosSeleccionadosX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3667M), 2));
                //var elementosSeleccionadosY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2466M), 2));
                //elementosSeleccionados.Location = new Point(elementosSeleccionadosX, elementosSeleccionadosY);

                //127, 13
                var elementosSeleccionadosWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.066M), 2));
                var elementosSeleccionadosHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.012M), 2));

                //562, 263
                var elementosSeleccionadosX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2890M), 2));
                var elementosSeleccionadosY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2435M), 2));

                //720, 170
                var bunifuDataObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.375M), 2));
                var bunifuDataObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1574M), 2));

                //0, 287
                var bunifuDataObjectX = 0;
                var bunifuDataObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.266M), 2));

                //248, 463
                var panelPagX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1291M), 2));
                var panelPagY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.42870M), 2));

                // Edición de propiedades segun resolución
                if (main.Width == 1024 && main.Height == 768)
                {
                    int displY = -25;
                    elementsAvailableY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.141M), 2)) - 20;

                    labelObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0057M), 2));
                    labelObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1826M), 2)) + displY;
                    optionObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0058M), 2));
                    optionObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2)) + displY; ;
                    optionObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0997M), 2));
                    bunifuSeparator4Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0897M), 2));
                    bunifuSeparator4X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0058M), 2));
                    bunifuSeparator4Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2290M), 2)) + displY;

                    labelLocationX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1207M), 2));
                    labelLocationY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1826M), 2)) + displY;
                    locationObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1208M), 2));
                    locationObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2)) + displY;
                    locationObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1200M), 2));
                    bunifuSeparator1Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1100M), 2));
                    bunifuSeparator1X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1208M), 2));
                    bunifuSeparator1Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2290M), 2)) + displY;

                    labelCameraX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2394M), 2));
                    labelCameraY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1824M), 2)) + displY;
                    camerasObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2395M), 2));
                    camerasObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2)) + displY;
                    camerasObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1331M), 2));
                    bunifuSeparator3Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1231M), 2));
                    bunifuSeparator3X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2395M), 2));
                    bunifuSeparator3Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2290M), 2)) + displY;

                    lblWarninMessageX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0031M), 2));
                    lblWarninMessageY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2466M), 2)) - 28;

                    elementosSeleccionadosX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0031M), 2));
                    elementosSeleccionadosY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2435M), 2));

                    ButtonAddX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3130M), 2)) - 15;
                    ButtonAddY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2)) + 15;

                    bunifuDataObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.375M), 2)) - 15;
                }
                else if ((main.Width > 2020 && main.Width < 2100) && (main.Height > 1200 && main.Height < 1300))
                {
                    ButtonAddX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3119M), 2)) - 20;
                    ButtonAddWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.063M), 2)) - 20;
                }

                panel1.Size = new Size(panel1Width, panel1Height);
                tilteForm.Location = new Point(tilteFormX, tilteFormY);
                labelName.Size = new Size(panel1Width, panel1Height);
                labelName.Location = new Point(labelNameX, labelNameY);
                txtName.Size = new Size(txtNameWidth, txtNameHeight);
                txtName.MinimumSize = new Size(txtNameWidth, txtNameHeight);
                txtName.Location = new Point(txtNameX, txtNameY);
                labelTipo.Size = new Size(labelTipoWidth, labelTipoHeight);
                labelTipo.Location = new Point(labelTipoX, labelTipoY);
                tipoObject.Size = new Size(tipoObjectWidth, tipoObjectHeight);
                tipoObject.Location = new Point(tipoObjectX, tipoObjectY);
                bunifuSeparator2.Size = new Size(bunifuSeparator2Width, bunifuSeparator2Height);
                bunifuSeparator2.Location = new Point(bunifuSeparator2X, bunifuSeparator2Y);
                elementsAvailable.Size = new Size(elementsAvailableWidth, elementsAvailableHeight);
                elementsAvailable.Location = new Point(elementsAvailableX, elementsAvailableY);
                labelObject.Size = new Size(labelObjectWidth, labelObjectHeight);
                labelObject.Location = new Point(labelObjectX, labelObjectY);
                labelLocation.Size = new Size(labelLocationWidth, labelLocationHeight);
                labelLocation.Location = new Point(labelLocationX, labelLocationY);
                labelGroup.Size = new Size(labelCarruselWidth, labelCarruselHeight);
                labelGroup.Location = new Point(labelCarruselX, labelCarruselY);
                labelCamera.Size = new Size(labelCameraWidth, labelCameraHeight);
                labelCamera.Location = new Point(labelCameraX, labelCameraY);
                optionObject.Size = new Size(optionObjectWidth, optionObjectHeight);
                optionObject.Location = new Point(optionObjectX, optionObjectY);
                ucLocationObject.Size = new Size(locationObjectWidth, locationObjectHeight);
                ucLocationObject.Location = new Point(locationObjectX, locationObjectY);
                groupObject.Size = new Size(carruselObjectWidth, carruselObjectHeight);
                groupObject.Location = new Point(carruselObjectX, carruselObjectY);
                ucCamerasObject.Size = new Size(camerasObjectWidth, camerasObjectHeight);
                ucCamerasObject.Location = new Point(camerasObjectX, camerasObjectY);
                ButtonAdd.Size = new Size(ButtonAddWidth, ButtonAddHeight);
                ButtonAdd.Location = new Point(ButtonAddX, ButtonAddY);
                bunifuSeparator4.Size = new Size(bunifuSeparator4Width, bunifuSeparator4Height);
                bunifuSeparator4.Location = new Point(bunifuSeparator4X, bunifuSeparator4Y);
                bunifuSeparator1.Size = new Size(bunifuSeparator1Width, bunifuSeparator1Height);
                bunifuSeparator1.Location = new Point(bunifuSeparator1X, bunifuSeparator1Y);
                bunifuSeparator3.Size = new Size(bunifuSeparator3Width, bunifuSeparator3Height);
                bunifuSeparator3.Location = new Point(bunifuSeparator3X, bunifuSeparator3Y);
                lblWarninMessage.Size = new Size(lblWarninMessageWidth, lblWarninMessageHeight);
                lblWarninMessage.Location = new Point(lblWarninMessageX, lblWarninMessageY);
                elementosSeleccionados.Size = new Size(elementosSeleccionadosWidth, elementosSeleccionadosHeight);
                elementosSeleccionados.Location = new Point(elementosSeleccionadosX, elementosSeleccionadosY);
                bunifuDataObject.Size = new Size(bunifuDataObjectWidth, bunifuDataObjectHeight);
                bunifuDataObject.Location = new Point(bunifuDataObjectX, bunifuDataObjectY);
                panelPag.Location = new Point(panelPagX, panelPagY);

                this.optionObject.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                this.groupObject.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                this.tipoObject.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                this.ucLocationObject.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                this.ucCamerasObject.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                _resizeLoad = false;
            }
        }

        private void DisplayCotrolGroup(bool show)
        {
            labelGroup.Visible = show;
            groupObject.Visible = show;

            labelLocation.Visible = !show;
            ucLocationObject.Visible = !show;
            labelCamera.Visible = !show;
            ucCamerasObject.Visible = !show;
            bunifuSeparator3.Visible = !show;
            lblWarninMessage.Visible = !show;
            //separatorPresetGuarda.Visible = show;

            if (show)
            {
                bunifuDataObject.Columns["Location"].Visible = false;
                bunifuDataObject.Columns["Device"].HeaderText = "Grupo";
                var duration = GroupsViewModel.GetDuration();
                TimeColumn.DataSource = duration.Where(d => d.Key == "40" || d.Key == "50" || d.Key == "60" || d.Key == "120").ToList();
                StreamColumn.Visible = false;
            }
            else
            {
                bunifuDataObject.Columns["Location"].Visible = true;
                bunifuDataObject.Columns["Device"].HeaderText = Resources.Camera;
                StreamColumn.Visible = true;
            }
        }

        private List<DataViewCarousel> PagePagination(int Page)
        {
            var pageItem = new List<DataViewCarousel>();
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
            dataView.Add(new DataViewCarousel { });
            dataView.Clear();
            bunifuDataObject.DataSource = typeof(List<DataViewCarousel>);
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
                if (e.ColumnIndex == TimeColumn.Index)
                {
                    var otem = (DataViewCarousel)bunifuDataObject.Rows[e.RowIndex].DataBoundItem;
                    var dt = (List<OptionObjectDTO>)((System.Windows.Forms.DataGridViewComboBoxCell)bunifuDataObject.Rows[e.RowIndex].Cells[e.ColumnIndex]).DataSource;
                    var newDuration = bunifuDataObject.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString();
                    var Duration = bunifuDataObject.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue.ToString();
                    if (newDuration != Duration)
                    {
                        var oDuration = dt.SingleOrDefault(a => a.Name == newDuration);
                        if (oDuration != null)
                        {
                            if (otem.Id == 0)
                            {
                                devices.SingleOrDefault(d => d.Order == otem.Order).Duration = oDuration.Key;
                            }
                            else
                            {
                                devices.SingleOrDefault(d => d.Id == otem.Id).Duration = oDuration.Key;
                            }
                        }
                    }
                }

                if (e.ColumnIndex == StreamColumn.Index)
                {
                    var otem = (DataViewCarousel)bunifuDataObject.Rows[e.RowIndex].DataBoundItem;
                    var dt = (List<OptionObjectDTO>)((System.Windows.Forms.DataGridViewComboBoxCell)bunifuDataObject.Rows[e.RowIndex].Cells[e.ColumnIndex]).DataSource;
                    var newProfileStream = bunifuDataObject.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString();
                    var profileStream = bunifuDataObject.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue.ToString();
                    if (newProfileStream != profileStream)
                    {
                        var oProfileStream = dt.SingleOrDefault(a => a.Name == newProfileStream);
                        if (oProfileStream != null)
                        {
                            if (otem.Id == 0)
                            {
                                devices.SingleOrDefault(d => d.StreamProfile == otem.StreamProfile).StreamProfile = oProfileStream.Key;
                            }
                            else
                            {
                                devices.SingleOrDefault(d => d.Id == otem.Id).StreamProfile = oProfileStream.Key;
                            }
                        }
                    }
                }
            }
        }

        private void optionObject_SelectedValueChanged(object sender, EventArgs e)
        {

            if (optionObject.SelectedValue.ToString() == "group")
            {
                DisplayCotrolGroup(true);
            }
            else
            {
                DisplayCotrolGroup(false);
            }

        }

        private void UcOptionObjectName_Search(object sender, string textSearchs)
        {
            //List<OptionObjectDTO> listaFiltrada;
            this.textsearch = textSearchs;
            this.pageOptionDeviceObject = 1;
            GetCameraObjectNames();
        }

        private void UcOptionlocationName_Search(object sender, string textSearchs)
        {
            this.textSiteSearch = textSearchs;
            this.pageOptionSiteObject = 1;
            GetLocationNames();
        }
    }
}
