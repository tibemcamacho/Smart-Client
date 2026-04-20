using CefSharp.DevTools.Audits;
using CefSharp.DevTools.CSS;
using CefSharp.DevTools.Profiler;
using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services.Services.Interface;
using Elipgo.SmartClient.UserControls.GenericForm;
using Elipgo.SmartClient.ViewModels;
using Splat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Elipgo.SmartClient.Common.Enum.Enums;
using static Elipgo.SmartClient.Common.Enum.TypeAlarms;


namespace Elipgo.SmartClient.UserControls.Groups
{
    public partial class FormGroups : GenericContentComponent
    {
        //private List<CatalogSite> Sitios;
        private readonly IGridService _gridService = Locator.Current.GetService<IGridService>();
        private readonly ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        public List<ObjectStateFilter> typeObject = new List<ObjectStateFilter>();
        public Action<Control> Event;
        public event EventHandler<FormGroups> SelectObject;
        public event EventHandler<FormGroups> EventGetObject;
        public event EventHandler<FormGroups> SelectSiteObject;
        public event EventHandler<FormGroups> SelectSiteObjectName;
        public List<DataViewGroup> dataView = new List<DataViewGroup>();
        public Action<Element> OnSaveOrUpdate;
        public int cantSeleccionados;
        private ObjectGroupEntity _entity;
        public ObjectGroupEntity ItemGroupSelected { get; set; }
        private List<OptionObjectDTO> optionObjectsName;
        public List<OptionObjectDTO> optionAllSites;
        private List<OptionObjectDTO> optionAllObjects;
        private List<OptionObjectDTO> optionAllDevices;
        private List<OptionObjectDTO> optionAllAnalytics;
        private List<OptionObjectDTO> optionAllCarousels;
        private List<OptionObjectDTO> optionAllLocations;
        private List<OptionObjectDTO> optionAllGrids;
        private List<string> itemsSelected;       /* Elementos seleccionados previamente. */
        private List<OptionObjectDTO> _listOptionObjectNames = new List<OptionObjectDTO>();
        private List<OptionObjectDTO> _listSiteOptionObjectNames = new List<OptionObjectDTO>();

        Image img = null;
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private bool startDragAndDrop = false;
        public bool _painted = false;
        private bool selectedValue = false;
        private string previousSelected;
        private GenericFormPagination controlPag;
        private List<DataViewGroup> devices = new List<DataViewGroup>();
        public short pageOptionSiteObject = 1;
        public short pageOptionDeviceObject = 0;
        public string textsearch = string.Empty;
        public string textSiteSearch = string.Empty;
        public SidebarElement type;
        public string filter = string.Empty;
        private short _takeDropdown = 0;
        public string SiteNameSelectValue
        {
            get => this.ucOptionSitesName.SelectedValue?.ToString() ?? string.Empty;
            set => this.ucOptionSitesName.SelectedValue = value;
        }

        public FormGroups()
        {
            base.Configuration = new ConfigGenericForm
            {
                ObjectBarSelected = LiveBarButtom.groups,
                NameEntity = Resources.group,
                IconEntity = FileResources.icon_groups,
                CanEditOrCreate = true,
                CanPrivate = false,
                CanMultiSelect = false,
                ShowFilterControls = true
            };

            /* Inicializamos componentes. */
            InitializeComponent();
            var _config = SmartClientEnvironmentUtils.GetConfiguration();
            _takeDropdown = Int16.Parse(_config.AppSettings.Settings["takeDropdown"].Value);
            tilteForm.Text = Resources.newGroup;
            labelName.Text = Resources.Name;
            labelNameObject.Text = Resources.objectName;
            labelObject.Text = Resources.Object;
            labelTypeObject.Text = Resources.typeObject;
            labelTypeGrid.Text = Resources.typeGrid;
            elementsAvailable.Text = Resources.elementsAvailable;
            labelSitesGroup.Text = Resources.lblTypeGroup;
            ButtonAdd.Text = Resources.buttonAdd;
            elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);
            txtName.BackColor = Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            txtName.FillColor = Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            bunifuDataObject.RowHeadersVisible = false;
            bunifuDataObject.RowsDefaultCellStyle.SelectionBackColor = Color.Transparent;
            bunifuDataObject.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold);
            bunifuDataObject.RowsDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 7.8f);

            StreamColumn.DisplayMember = "Name";
            StreamColumn.ValueMember = "Key";

            bunifuDataObject.Columns[1].HeaderText = Resources.Object;
            bunifuDataObject.Columns[2].HeaderText = Resources.type;
            bunifuDataObject.Columns[3].HeaderText = Resources.Name;
            bunifuDataObject.Columns[4].HeaderText = Resources.Stream;
            bunifuDataObject.Columns[5].HeaderText = Resources.delete;
            bunifuDataObject.Columns[6].HeaderText = Resources.order;
            bunifuDataObject.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            bunifuDataObject.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ucOptionSitesName.SearchRequested += UcOptionSiteName_Search;
            ucOptionObjectName.SearchRequested += UcOptionObjectName_Search;

            LoadValidations(); _entity = new ObjectGroupEntity { Type = 1 };
            bindingSourceGroup.DataSource = _entity; // this.Paint += FormGroups_Paint;
            _gridService.PathGridResources = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "grid.json");
            controlPag = new GenericFormPagination(1);
            controlPag.OnClickNextPage += OnClickNextPage;
            controlPag.OnClickBackPage += OnClickBackPage;
            controlPag.OnClickStartPage += OnClickStartPage;
            controlPag.OnClickEndPage += OnClickEndPage;
            panelPag.Controls.Add(controlPag);
            panelPag.Visible = false;
            this.Load += FormGroups_Load;
        }

        private void FormGroups_Load(object sender, EventArgs e)
        {
            StreamColumn.DataSource = GroupViewModel.GetProfilesStream();
        }

        private void FormGroups_Paint(object sender, PaintEventArgs e)
        {
            if (this._painted)
            {
                return;
            }

            this._painted = true; Threads.RunInOtherThread(new Action[] { () => SetInitOptionsGrid() }, null);
        }

        public override void GoBack()
        {
            this._painted = false; txtName.DataBindings.Clear();
        }

        public void SetInitOptionsGrid()
        {
            txtName.DataBindings.Clear();
            txtName.DataBindings.Add(new Binding("Text", this.bindingSourceGroup, "Name", true));
            tipoObject.DataBindings.Add(new Binding("SelectedValue", this.bindingSourceGroup, "IsPrivate", true));

            GridFilterDTO gridFilter = new GridFilterDTO();

            if (this.ParentApps == Apps.Playback)
            {
                gridFilter.MaximumQuantity = Convert.ToInt32(VariableResources.grid_maximum_quantity);
            }

            List<GridDTO> grids = _gridService.Get(gridFilter);

            optionAllGrids = new List<OptionObjectDTO>();

            foreach (GridDTO grid in grids)
            {
                optionAllGrids.Add(new OptionObjectDTO
                {
                    Key = grid.Id,
                    Name = Resources.Grid + " " + grid.Id
                });
            }
        }

        public void SetOptionAllObjects(List<OptionObjectDTO> option1, List<OptionObjectDTO> option2, List<OptionObjectDTO> option3, List<OptionObjectDTO> option4)
        {
            var _AllObj = new List<OptionObjectDTO>();

            /* Dispositivos. */
            foreach (var item in option1)
            {
                _AllObj.Add(item);
            }

            /* Analiticos. */
            foreach (var item in option2)
            {
                _AllObj.Add(item);
            }

            /* Carruseles. */
            foreach (var item in option3)
            {
                _AllObj.Add(item);
            }

            /* Ubicaciones. */
            foreach (var item in option4)
            {
                _AllObj.Add(item);
            }

            /* Cargamos todo. */
            this.optionAllDevices = option1; this.optionAllAnalytics = option2; this.optionAllCarousels = option3;
            this.optionAllLocations = option4; this.optionAllObjects = _AllObj;
        }

        public void SetOptionObject(List<OptionObjectDTO> option)
        {
            this.optionObject.DataSource = option;
        }

        public void SetSitesObject(List<OptionObjectDTO> option)
        {

            if (pageOptionSiteObject == 1)
            {
                this.ucOptionObjectName.DataSource = new List<OptionObjectDTO>();
                this.ucOptionObjectName.SwitchToSelectionMode();
                this.textsearch = string.Empty;
                if (this.ucOptionSitesName.SelectedValue != null)
                    if (this.ucOptionSitesName.SelectedValue.ToString() != "0")
                        _listSiteOptionObjectNames.Clear();

            }
            this.ucOptionSitesName.DataSource = new List<OptionObjectDTO>();
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
                this.ucOptionSitesName.DataSource = optionAllSites ?? _listSiteOptionObjectNames;
                this.ucOptionSitesName.DisplayMember = "Name";
                this.ucOptionSitesName.ValueMember = "Key";
            }
        }

        public void SetOptionTypeObject(ElementSidebar type, string keyObjectType = null)
        {
            ShowTypeObject();
            if (type != ElementSidebar.Device && type != ElementSidebar.Analytics)
            {
                HideTypeObject();
                GetNames();
                return;
            }
            var option = new List<OptionObjectDTO>();

            var data = typeObject.Where(t => t.Type == type).FirstOrDefault();

            if (!string.IsNullOrEmpty(keyObjectType))
            {
                CheckElementDTO element = data.options.SingleOrDefault(ot => ot.Key == keyObjectType);

                option.Add(new OptionObjectDTO { Key = element.Key, Name = element.Name });
            }
            else
            {
                foreach (var item in data.options)
                {
                    option.Add(new OptionObjectDTO
                    {
                        Key = item.Key,
                        Name = item.Name
                    });
                }
            }
            this.pageOptionDeviceObject = 1;
            this.pageOptionSiteObject = 1;
            this.optionTypeObject.DataSource = option;
            GetNames();
        }

        public void SetOptionSitesObject(ElementSidebar type)
        {
            if (optionObjectsName != null)
            {
                if (type != ElementSidebar.Device && type != ElementSidebar.Analytics && type != ElementSidebar.Locations && type != ElementSidebar.AlarmsMap)
                {
                    HideSitesObject(false);
                    GetNames();
                    return;
                }
                else
                {

                    HideSitesObject(true);
                }
            }
        }

        public void SetOptionObjectNames(List<OptionObjectDTO> options)
        {
            if (optionAllSites != null)
            {
                this.ucOptionObjectName.DataSource = optionObjectsName = options;
            }
        }

        public void SetOptionSiteObjectNames(List<OptionObjectDTO> options)
        {
            if (pageOptionDeviceObject == 1)
            {
                this.ucOptionObjectName.SwitchToSelectionMode();
                if (this.ucOptionObjectName.SelectedValue != null)
                    if (this.ucOptionObjectName.SelectedValue.ToString() != "0")
                        _listOptionObjectNames.Clear();

                if (this.ucOptionSitesName.isSearchingMode)
                {
                    this.ucOptionSitesName.SwitchToSelectionMode();
                }
            }
            this.ucOptionObjectName.DataSource = new List<OptionObjectDTO>();
            _listOptionObjectNames.RemoveAll(x => x.Key == "0");
            if (options.Count != 0 && options[0].count > _takeDropdown)
            {
                float totalpage = (int)Math.Ceiling((double)options[0].count / _takeDropdown);
                if (totalpage > pageOptionDeviceObject)
                {
                    var seemore = new OptionObjectDTO { Key = "0", Name = Resources.ViewMore };
                    options.Add(seemore);
                }
            }
            _listOptionObjectNames.AddRange(options);
            this.ucOptionObjectName.DataSource = optionObjectsName = _listOptionObjectNames.ToList();
            this.ucOptionObjectName.DisplayMember = "Name";
            this.ucOptionObjectName.ValueMember = "Key";

        }

        public void SetOptionSitesNames(List<OptionObjectDTO> options)
        {
            if (optionObjectsName != null)
            {
                var list = new List<OptionObjectDTO>();                 /* Objeto devuelto. */
                var listAll = options.GroupBy(u => u.Tag).Select(x => x.First()).OrderByDescending(u => u.Tag).ToList();
                var listSitesStr = listAll.Select(u => u.Tag.ToString()).ToList();

                /* Genero la lista de sucursales. */
                var AllSites = this.optionAllSites.Select(u => new { u.Tag, u.Name }).Where(u => listSitesStr.Contains(u.Tag)).ToList();

                /* Limpio la lista. */
                list.Clear();
                foreach (var item in AllSites)
                {
                    list.Add(new OptionObjectDTO { Name = item.Name, Key = item.Tag.ToString(), Tag = item.Tag.ToString() });
                }

                this.ucOptionSitesName.DataSource = list; // GetNames();
            }
        }

        public void SetAllOptionGrids()
        {
            if (optionAllGrids != null)
            {
                Clear(); this.optionGrid.DataSource = optionAllGrids; this.optionGrid.SelectedValue = ItemGroupSelected.GridId;
                selectedValue = true;
            }
        }

        private void HideTypeObject()
        {
            this.optionTypeObject.Visible = false;
            labelTypeObject.Visible = false;
            bunifuSeparatorType.Visible = false;
        }

        private void HideSitesObject(bool flag)
        {
            this.ucOptionSitesName.Visible = flag;
            this.labelSitesGroup.Visible = flag;
            this.bunifuSeparator2.Visible = flag;
        }

        private void HideObjects(bool flag)
        {
            this.ucOptionObjectName.Visible = flag;
            this.labelNameObject.Visible = flag;
            this.bunifuSeparator3.Visible = flag;
            this.ButtonAdd.Enabled = flag;
        }

        private void ShowTypeObject()
        {
            this.optionTypeObject.Visible = true;
            labelTypeObject.Visible = true;
            bunifuSeparatorType.Visible = true;
        }

        private void optionSitesGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ucOptionSitesName.SelectedValue.ToString() == "0")
            {
                pageOptionSiteObject++;
                GetNames();
            }
            else
            {
                if (optionAllSites != null && this.optionObjectsName != null)
                {
                    var valueSiteObject = ((ListControl)sender).SelectedValue.ToString();
                    var valueTypeObject = (this.optionTypeObject.SelectedValue != null) ? this.optionTypeObject.SelectedValue.ToString() : "All";
                    var valueObject = this.optionObject.SelectedValue.ToString();

                    /* Cargo la lista de objetos, según el sitio y el tipo de objeto. */
                    if (valueObject == "Carousels")
                    {
                        return;
                    }

                    /* Objeto devuelto. */
                    var list = (this.itemsSelected != null) ? FilterObjects(valueObject, valueTypeObject, valueSiteObject).Where(u => !itemsSelected.Contains(u.Key)).ToList() :
                                                              FilterObjects(valueObject, valueTypeObject, valueSiteObject).ToList();

                    /* Finalmente cargamos el set de datos. */
                    if (list.Count == 0)
                    {
                        HideObjects(false);
                    }
                    else
                    {
                        HideObjects(true); this.ucOptionObjectName.DataSource = list;
                    }
                    // GetNames();        
                }
                else if (this.ucOptionSitesName.SelectedValue != null && this.ucOptionSitesName.SelectedValue.ToString() != "0")
                {
                    var valueSiteObject = this.ucOptionSitesName.SelectedValue.ToString();
                    pageOptionDeviceObject = 1;
                    this.ucOptionObjectName.DataSource = null;
                    _listOptionObjectNames.Clear();
                    this.textSiteSearch = string.Empty;
                    this.ucOptionSitesName.SwitchToSelectionMode();
                    GetSiteObjectNames();
                    //SetOptionSiteObjectNames(LiveViewModel.GetSiteDevicesName("All", 1, pageOptionSiteObject, 50));
                    //SetOptionSiteObjectNames(LiveViewModel.GetSitesName("All", pageOptionSiteObject, 50));
                }
            }
        }

        private void optionObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                var value = ((ListControl)sender).SelectedValue.ToString();
                switch (value)
                {
                    case "AlarmsMap":
                    case "Locations":
                        if (value == "Locations")
                        {
                            SetOptionTypeObject(ElementSidebar.Locations);
                            SetOptionSitesObject(ElementSidebar.Locations);
                        }
                        else
                        {
                            SetOptionTypeObject(ElementSidebar.AlarmsMap);
                            SetOptionSitesObject(ElementSidebar.AlarmsMap);
                        }

                        //178, 216
                        var optionSitesNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0926M), 2));
                        var optionSitesNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));
                        ucOptionSitesName.Location = new Point(optionSitesNameX, optionSitesNameY);

                        //178, 197
                        var labelSitesGroupX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0926M), 2));
                        var labelSitesGroupY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1826M), 2));
                        labelSitesGroup.Location = new Point(labelSitesGroupX, labelSitesGroupY);

                        //178, 244
                        var bunifuSeparator2X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0926M), 2));
                        var bunifuSeparator2Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2));
                        bunifuSeparator2.Location = new Point(bunifuSeparator2X, bunifuSeparator2Y);

                        //390, 216
                        var optionObjectNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.203M), 2));
                        var optionObjectNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));
                        ucOptionObjectName.Location = new Point(optionObjectNameX, optionObjectNameY);

                        //390, 197
                        var labelNameObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.203M), 2));
                        var labelNameObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1826M), 2));
                        labelNameObject.Location = new Point(labelNameObjectX, labelNameObjectY);

                        //390, 244
                        var bunifuSeparator3X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.203M), 2));
                        var bunifuSeparator3Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2));
                        bunifuSeparator3.Location = new Point(bunifuSeparator3X, bunifuSeparator3Y);

                        break;
                    case "Devices":
                        SetOptionTypeObject(ElementSidebar.Device);
                        SetOptionSitesObject(ElementSidebar.Device);

                        if (!(main.Width == 1024 && main.Height == 768))
                        {
                            //292, 216
                            var optionSitesNameDX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1520M), 2));
                            var optionSitesNameDY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));
                            ucOptionSitesName.Location = new Point(optionSitesNameDX, optionSitesNameDY);

                            //290, 202
                            var labelSitesGroupDX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1510M), 2));
                            var labelSitesGroupDY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1870M), 2));
                            labelSitesGroup.Location = new Point(labelSitesGroupDX, labelSitesGroupDY);

                            //292, 244
                            var bunifuSeparator2DX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1520M), 2));
                            var bunifuSeparator2DY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2));
                            bunifuSeparator2.Location = new Point(bunifuSeparator2DX, bunifuSeparator2DY);

                            //451, 216  QQQ
                            var optionObjectNameDX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2348M), 2));
                            var optionObjectNameDY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));
                            ucOptionObjectName.Location = new Point(optionObjectNameDX, optionObjectNameDY);

                            //449, 202
                            var labelNameObjectDX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2338M), 2));
                            var labelNameObjectDY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1870M), 2));
                            labelNameObject.Location = new Point(labelNameObjectDX, labelNameObjectDY);

                            //451, 244
                            var bunifuSeparator3DX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2348M), 2));
                            var bunifuSeparator3DY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2));
                            bunifuSeparator3.Location = new Point(bunifuSeparator3DX, bunifuSeparator3DY);
                        }

                        break;
                    case "Analytics":
                        SetOptionTypeObject(ElementSidebar.Analytics);
                        SetOptionSitesObject(ElementSidebar.Analytics);
                        this.ucOptionSitesName.SelectedIndex = 0;
                        //292, 216
                        var optionSitesNameAX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1520M), 2));
                        var optionSitesNameAY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));
                        ucOptionSitesName.Location = new Point(optionSitesNameAX, optionSitesNameAY);

                        //290, 202
                        var labelSitesGroupAX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1510M), 2));
                        var labelSitesGroupAY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1870M), 2));
                        labelSitesGroup.Location = new Point(labelSitesGroupAX, labelSitesGroupAY);

                        //292, 244
                        var bunifuSeparator2AX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1520M), 2));
                        var bunifuSeparator2AY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2));
                        bunifuSeparator2.Location = new Point(bunifuSeparator2AX, bunifuSeparator2AY);

                        //451, 216
                        var optionObjectNameAX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2348M), 2));
                        var optionObjectNameAY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));
                        ucOptionObjectName.Location = new Point(optionObjectNameAX, optionObjectNameAY);

                        //449, 202
                        var labelNameObjectAX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2338M), 2));
                        var labelNameObjectAY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1870M), 2));
                        labelNameObject.Location = new Point(labelNameObjectAX, labelNameObjectAY);

                        //451, 244
                        var bunifuSeparator3AX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2348M), 2));
                        var bunifuSeparator3AY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2));
                        bunifuSeparator3.Location = new Point(bunifuSeparator3AX, bunifuSeparator3AY);

                        break;
                    case "Carousels":
                        SetOptionTypeObject(ElementSidebar.Carousel);
                        SetOptionSitesObject(ElementSidebar.Carousel);
                        //178, 216
                        var optionObjectNameCX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0926M), 2));
                        var optionObjectNameCY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));
                        ucOptionObjectName.Location = new Point(optionObjectNameCX, optionObjectNameCY);

                        //178, 197
                        var labelNameObjectCX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0926M), 2));
                        var labelNameObjectCY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1826M), 2));
                        labelNameObject.Location = new Point(labelNameObjectCX, labelNameObjectCY);

                        //178, 244
                        var bunifuSeparator3CX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0926M), 2));
                        var bunifuSeparator3CY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2));
                        bunifuSeparator3.Location = new Point(bunifuSeparator3CX, bunifuSeparator3CY);

                        break;
                    default:
                        break;
                }
            }
        }

        private void OptionTypeObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.optionObjectsName != null)
            {
                var valueTypeObject = ((ListControl)sender).SelectedValue.ToString();
                //var valueSiteObject = this.ucOptionSitesName.SelectedValue.ToString();
                var valueObject = this.optionObject.SelectedValue.ToString();

                /* Cargo la lista de objetos, según el sitio y el tipo de objeto. */
                if (valueObject == "Carousels")
                    return;

                /* Objeto devuelto. */

                //var list = (this.itemsSelected != null) ? FilterObjects(valueObject, valueTypeObject, valueSiteObject).Where(u => !itemsSelected.Contains(u.Key)).ToList() :
                //                                          FilterObjects(valueObject, valueTypeObject, valueSiteObject).ToList();

                GetNames();
                //GetSiteObjectNames();

                /* Finalmente cargamos el set de datos. */
                //if (list.Count == 0)
                //{
                //    HideObjects(false);
                //}
                //else
                //{
                //    HideObjects(true); this.optionObjectName.DataSource = list;
                //}
                // GetNames();        
            }
        }

        private void OptionObjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ucOptionObjectName.SelectedValue != null && this.ucOptionObjectName.SelectedValue.ToString() == "0")
            {
                GetMoreSiteObjectNames();
            }
        }

        public string GetSelectObject()
        {
            return optionObject.SelectedValue.ToString();
        }

        public SidebarElement GetSelectObjectType()
        {
            var ob = optionObject.SelectedValue.ToString();
            SidebarElement _type = SidebarElement.all;
            switch (ob)
            {
                case "Locations":
                    _type = SidebarElement.Locations;
                    break;
                case "Devices":
                    _type = SidebarElement.devices;
                    break;
                case "Analytics":
                    _type = SidebarElement.Analytics;
                    break;
                case "Carousels":
                    _type = SidebarElement.Carousels;
                    break;
                case "AlarmsMap":
                case "Alarms_Map":
                    _type = SidebarElement.GeoAlarms;
                    break;
            }
            return _type;
        }

        public string GetSelectTypeObject()
        {
            return optionTypeObject.SelectedValue?.ToString() ?? "All";
        }

        public int GetSelectElement()
        {
            return (this.ucOptionObjectName.SelectedValue == null ? 0 : int.Parse(this.ucOptionObjectName.SelectedValue.ToString())); ;
        }

        private void GetNames()
        {
            this.SelectObject?.Invoke(null, this);
        }
        private void GetSiteObjectNames()
        {
            this.SelectSiteObject?.Invoke(null, this);
        }

        private void GetMoreSiteObjectNames()
        {
            pageOptionDeviceObject++;
            this.SelectSiteObjectName?.Invoke(null, this);
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            //var id = this.ucOptionObjectName.SelectedValue.ToString();

            OptionObjectDTO typeGridSelected = (OptionObjectDTO)this.optionGrid.SelectedItem;

            if (typeGridSelected == null)
            {
                cantSeleccionados--; MessageBox.Show(Resources.GridNotSelect.Trim(), Resources.action, MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }
            else
            {
                //cantSeleccionados++; elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);

                List<GridDTO> gridTypes = _gridService.Get(new GridFilterDTO
                {
                    Id = typeGridSelected.Key
                });

                if (gridTypes[0].Elements.Count <= cantSeleccionados)
                {
                    ButtonAdd.Visible = false;
                }
                EventGetObject?.Invoke(null, this);
            }
        }

        public void AddElement(TypesFilters type, int idElement)
        {
            if (!ucOptionObjectName.isSearchingMode && !ucOptionSitesName.isSearchingMode
                && ucOptionObjectName.Items.Count > 0 && ucOptionSitesName.Items.Count > 0)
            {
                if (!itemsSelected.Contains(idElement.ToString()))
                {
                    var objeto = ((OptionObjectDTO)optionObject.SelectedItem).Name;
                    var name = ((OptionObjectDTO)ucOptionObjectName.SelectedItem).Name;
                    var valueTypeObject = this.optionTypeObject.SelectedValue.ToString();
                    var valueSiteObject = this.ucOptionSitesName.SelectedValue.ToString();
                    var valueObject = this.optionObject.SelectedValue.ToString();

                    var data = new DataViewGroup
                    {
                        Id = "0",
                        objectTitle = objeto,
                        nameObject = name,
                        type = type.ToString(),//element.Type,
                        delete = false,
                        siteId = int.Parse(valueSiteObject), //((OptionObjectDTO)optionObjectName.SelectedItem).Key != null ? ((OptionObjectDTO)optionObjectName.SelectedItem).Key : (((OptionObjectDTO)optionObjectName.SelectedItem).Item as CatalogObjectGroup).Elements.FirstOrDefault().Element.SiteId,
                        item = (type.ToString() == ElementType.AlarmsMap.ToString() ? (OptionObjectDTO)ucOptionObjectName.SelectedItem : ((OptionObjectDTO)ucOptionObjectName.SelectedItem).Item)
                    };

                    dataView.Add(data);
                    bunifuDataObject.DataSource = typeof(List<DataViewGroup>);
                    bunifuDataObject.DataSource = dataView;
                    bunifuDataObject.EndEdit();
                    devices.Add(data);


                    /* Anterior. */
                    var idx = this.ucOptionObjectName.SelectedIndex;

                    this.itemsSelected.Add((idElement).ToString());
                    cantSeleccionados = this.itemsSelected.Count();
                    elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);


                    /* NOTA: Cuando ya no hay elementos que elegir, se desaparece el combo de elementos. */
                    if (this.ucOptionObjectName.SelectedIndex == -1)
                    {
                        HideObjects(false);
                    }
                    else
                    {
                        HideObjects(true); this.ucOptionObjectName.SelectedIndex = idx > this.ucOptionObjectName.Items.Count ? this.ucOptionObjectName.Items.Count : idx;
                    }
                    panelPag.Visible = (devices.Count > 0);
                    controlPag.UpdatePage(devices.Count, 1, true);
                    if (controlPag.PropTotalPage > 1)
                    {
                        controlPag.endPage();
                    }

                    pagination(controlPag.PropTotalPage);
                    GenericFormControl.EnabledButtonOkEvent(cantSeleccionados >= 1);
                }
                else
                {
                    //MessageBox.Show(Resources.registered_Item, Resources.action, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    notification.Show(Resources.registered_Item, null);
                }
            }
        }

        //public Element GetElement(TypesFilters type, int idElement)
        //{
        //    var list = new List<OptionObjectDTO>();

        //    Element element = new Element();
        //    if (type == TypesFilters.Device)
        //    {
        //        element.SiteId = int.Parse(this.optionSitesName.SelectedValue);
        //        switch (ElementType)    
        //        {
        //            case ElementType.None:
        //                break;
        //            case ElementType.Camera:
        //                break;
        //            case ElementType.Iot:
        //                element.Type = iot.Type == "DI" ? ElementType.Iot_In.ToString() : ElementType.Iot_Out.ToString();
        //                break;
        //            case ElementType.Kpi:
        //                break;
        //            case ElementType.Face:
        //                break;
        //            case ElementType.Lpr:
        //                break;
        //            case ElementType.Geomap:
        //                break;
        //            case ElementType.Carousel:
        //                break;
        //            case ElementType.Blueprint:
        //                break;
        //            case ElementType.Iot_In:
        //                break;
        //            case ElementType.Iot_Out:
        //                break;
        //            case ElementType.Location:
        //                break;
        //            case ElementType.AlarmsMap:
        //                break;
        //            case ElementType.Geolocation_Alarm:
        //                break;
        //            default:
        //                break;
        //        }

        //        foreach (var item in _listOptionObjectNames)
        //            {
        //                //var camera = item.Cameras.Where(c => c.ObjectId == idElement).FirstOrDefault();
        //                if (item != null)
        //                {
        //                    element.SiteId = item.;
        //                    element.Type = ElementType.Camera.ToString();
        //                }
        //                if (element.Type != null)
        //                {
        //                    return element;
        //                }

        //                var iot = item.Iots.Where(c => c.ObjectId == idElement).FirstOrDefault();
        //                if (iot != null)
        //                {
        //                    element.SiteId = item.Id;
        //                    element.Type = iot.Type == "DI" ? ElementType.Iot_In.ToString() : ElementType.Iot_Out.ToString();
        //                }
        //                if (element.Type != null)
        //                {
        //                    return element;
        //                }
        //            }

        //    }

        //    else if (type == TypesFilters.Locations)
        //    {
        //        if (_catalog != null && _catalog.Sites != null)
        //        {
        //            foreach (var item in _catalog.Sites)
        //            {
        //                var location = item.Locations.Where(c => c.ObjectId == idElement).FirstOrDefault();
        //                if (location != null)
        //                {
        //                    element.SiteId = item.Id;
        //                    element.Type = ElementType.Location.ToString();
        //                }
        //                if (element.Type != null)
        //                {
        //                    return element;
        //                }
        //            }
        //        }
        //    }
        //    else if (type == TypesFilters.AlarmsMap)
        //    {
        //        if (_catalog != null && _catalog.Sites != null)
        //        {
        //            foreach (var item in _catalog.Sites)
        //            {
        //                if (item != null)
        //                {
        //                    element.SiteId = item.Id;
        //                    element.Type = ElementType.AlarmsMap.ToString();
        //                }
        //                if (element.Type != null)
        //                {
        //                    return element;
        //                }
        //            }
        //        }
        //    }

        //    else if (type == TypesFilters.Analytics)
        //    {
        //        if (_catalog != null && _catalog.Sites != null)
        //        {
        //            foreach (var item in _catalog.Sites)
        //            {
        //                var kpi = item.Kpis.Where(c => c.ObjectId == idElement).FirstOrDefault();
        //                if (kpi != null)
        //                {
        //                    element.SiteId = item.Id;
        //                    element.Type = ElementType.Kpi.ToString();
        //                }
        //                if (element.Type != null)
        //                {
        //                    return element;
        //                }

        //                var face = item.Faces.Where(c => c.ObjectId == idElement).FirstOrDefault();
        //                if (face != null)
        //                {
        //                    element.SiteId = item.Id;
        //                    element.Type = ElementType.Face.ToString();
        //                }
        //                if (element.Type != null)
        //                {
        //                    return element;
        //                }

        //                var lpr = item.Lprs.Where(c => c.ObjectId == idElement).FirstOrDefault();
        //                if (lpr != null)
        //                {
        //                    element.SiteId = item.Id;
        //                    element.Type = ElementType.Lpr.ToString();
        //                }
        //                if (element.Type != null)
        //                {
        //                    return element;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (_catalog != null && _catalog.Carousels != null)
        //        {
        //            var carousel = _catalog.Carousels.Where(c => c.Id == idElement).FirstOrDefault();
        //            if (carousel != null)
        //            {
        //                element.Type = ElementType.Carousel.ToString();
        //            }
        //            if (element.Type != null)
        //            {
        //                return element;
        //            }
        //        }
        //    }
        //    return element;
        //}

        public override async Task<GenericForm.ContentFormDTO> SaveOrUpdate()
        {
            if (!IsValid()) { return null; }
            bindingSourceGroup.EndEdit();
            _entity.UserIdGuid = GroupViewModel.UserIdGuid;
            _entity.UserId = (int)GroupViewModel.UserId;
            _entity.Type = ItemGroupSelected.Type;
            _entity.IsPrivate = Convert.ToBoolean(tipoObject.SelectedValue);
            var gridSelected = (OptionObjectDTO)this.optionGrid.SelectedItem;
            if (gridSelected != null)
                _entity.GridId = gridSelected.Key;

            var device = new List<ObjectGroupElementEntity>();
            var dataSource = this.devices; // bunifuDataObject.DataSource as List<DataViewGroup>;
            var index = 1;
            if (dataSource != null)
            {
                dataSource.ForEach(i =>
                {
                    var item = i.item;
                    var type = string.Empty;
                    if (i.type != "Alarms_Map" && i.type != "AlarmsMap")
                    {
                        if (item.GetType().GetProperty("DeviceTypeStr") != null)
                            type = ((SidebarElementDTO)item).DeviceTypeStr;
                        else if (item.GetType().GetProperty("Type") != null)
                            type = Convert.ToString(item.GetType().InvokeMember("Type", System.Reflection.BindingFlags.GetProperty, null, i.item, null));
                        else if (item is CatalogSite)
                            type = "GEO";
                    }
                    else
                        type = "AlarmsMap";

                    if (i.type == "Carousel")
                    {
                        type = "CAR";
                    }

                    int objectId;
                    if (item.GetType().GetProperty("ObjectId") != null)
                        objectId = (int)item.GetType().InvokeMember("ObjectId", System.Reflection.BindingFlags.GetProperty, null, i.item, null);
                    else if (item is CatalogSite catalogSite)
                        objectId = catalogSite.Id;
                    else
                        objectId = ((SidebarElementDTO)i.item).ElementId;

                    // Para MAP/GEO: preservar el ObjectId original de _entity.Elements
                    // porque GetSiteLocationInfo puede devolver un ObjectId diferente al vmap ID
                    if (new string[] { "MAP", "GEO" }.Contains(type) && _entity.Elements != null)
                    {
                        var originalElement = _entity.Elements.FirstOrDefault(e => e.Type == type && e.SiteId == (int)(i.siteId ?? 0) && !e.IsDeleted);
                        if (originalElement != null)
                        {
                            objectId = originalElement.ObjectId;
                        }
                    }


                    var lat = string.Empty;
                    var lng = string.Empty;

                    if (new string[] { "GEO", "MAP" }.Contains(type))
                    {
                        lat = "0";//item.GetType().InvokeMember("LocationLatitude", System.Reflection.BindingFlags.GetProperty, null, i.item, null)?.ToString();
                        lng = "0";// item.GetType().InvokeMember("LocationLongitude", System.Reflection.BindingFlags.GetProperty, null, i.item, null)?.ToString();
                    }

                    device.Add(new ObjectGroupElementEntity
                    {
                        ObjectGroupId = _entity.Id,
                        ContainerId = index++,
                        IsDeleted = false,
                        Type = type,
                        SiteId = (type == "Alarms_Map" || type == "AlarmsMap" ? int.Parse(i.siteId.ToString()) : (int)(i.siteId ?? 0)),
                        ObjectId = objectId,
                        Latitude = lat,
                        Longitude = lng,
                        Id = Convert.ToInt32(i.Id ?? "0"),
                        ProfileStream = GroupViewModel.ConvertToProfile(i.streamProfile)
                    });
                });

                _entity.Elements = device.ToArray();
            }

            var obj = await GroupViewModel.SaveOrUpdate(_entity);
            ItemGroupSelected.Elements = null;
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

        public override ConfigGenericForm Configuration
        {
            get => base.Configuration;
            internal set => base.Configuration = value;
        }

        public override Task<List<GenericForm.ContentFormDTO>> GetDataSource(Action<List<GenericForm.ContentFormDTO>> callback)
        {
            return Task.Run(async () =>
            {
                var data = await GroupViewModel.GetObjectGroup(ItemGroupSelected.UserId, ItemGroupSelected.Type);
                return data?.Where(p => !p.IsDeleted).Select
               (
                 p => new GenericForm.ContentFormDTO
                 {
                     Label1 = p.Name,
                     EntityIcon = FileResources.icon_groups,
                     IsPrivate = true,
                     Id = p.Id
                 }
               ).ToList() ?? new List<GenericForm.ContentFormDTO>();
            });
        }

        public override void Clear()
        {
            this.tilteForm.Text = Resources.newGroup;
            dataView.Add(new DataViewGroup { });
            bunifuDataObject.DataSource = typeof(List<DataViewGroup>);
            bunifuDataObject.DataSource = dataView;

            bindingSourceGroup.Clear();
            errorManager.Clear();
            dataView.Clear();

            _entity = new ObjectGroupEntity();
            bindingSourceGroup.DataSource = _entity;
            bunifuDataObject.DataSource = typeof(List<DataViewGroup>);
            bunifuDataObject.DataSource = dataView;

            errorManager.SetErrorWithCount(txtName, Resources.required);
            //errorManager.SetErrorWithCount(optionGrid, Resources.required);

            cantSeleccionados = 0; itemsSelected = new List<string>();
            elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);
            //this.optionObjectName.DataSource = FilterObjects(this.optionObject.SelectedValue?.ToString(), optionTypeObject.SelectedValue?.ToString() ?? "All", this.optionSitesName.SelectedValue != null ? this.optionSitesName.SelectedValue.ToString() : "");
            ButtonAdd.Visible = true;
            this.txtName.Text = string.Empty;
            this.optionGrid.SelectedValue = ItemGroupSelected.GridId; this._painted = false;
            panelPag.Visible = false;
            devices = new List<DataViewGroup>();
            controlPag.UpdatePage(0, 1);
            //ControlsResize();
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

        private void LoadValidations()
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                errorManager.SetErrorWithCount(txtName, Resources.required);
            }
        }

        private bool ValidateSelectedDevices()
        {
            OptionObjectDTO typeGridSelected = (OptionObjectDTO)this.optionGrid.SelectedItem;

            if (typeGridSelected == null)
            {
                return false;
            }
            List<GridDTO> gridTypes = _gridService.Get(new GridFilterDTO
            {
                Id = typeGridSelected.Key
            });
            return (gridTypes != null && gridTypes.Any() && gridTypes[0].Elements.Count >= cantSeleccionados);
        }

        public override async Task<bool> Edit()
        {
            if (SelectedItemOption != null)
            {
                this.tilteForm.Text = Resources.editGroup;
                var item = await GroupViewModel.GetGroup(SelectedItemOption.Id);

                if (item != null)
                {
                    bindingSourceGroup.Clear();
                    _entity = item;
                    bindingSourceGroup.DataSource = _entity;
                    bindingSourceGroup.ResetBindings(false);
                    errorManager.Clear();

                    var valueTypeObject = this.optionTypeObject.SelectedValue.ToString();
                    var valueSiteObject = this.ucOptionSitesName.SelectedValue.ToString();
                    var valueObject = this.optionObject.SelectedValue.ToString();

                    //var devices = _entity.Elements?.Where(p => !p.IsDeleted).Select(p => (GroupViewModel.GetElement(p))).Where(p => p != null).ToList();
                    var tasks = _entity.Elements?.Where(p => !p.IsDeleted).Select(p => GroupViewModel.GetElement(p));

                    if (tasks != null)
                    {
                        var results = await Task.WhenAll(tasks);
                        var devices = results.Where(p => p != null).ToList();
                        this.devices = devices;

                        this.devices = devices;
                        dataView.Clear();
                        dataView.AddRange(PagePagination(1));
                        bunifuDataObject.DataSource = typeof(List<DataViewGroup>);
                        bunifuDataObject.DataSource = dataView;

                        var idx = this.ucOptionObjectName.SelectedIndex;
                        var items = dataView.Select(i => i.item.GetType().GetProperty("ObjectId") != null ?
                                                ((int)i.item.GetType().InvokeMember("ObjectId", System.Reflection.BindingFlags.GetProperty, null, i.item, null)).ToString() :
                                                ((int)i.item.GetType().InvokeMember("Id", System.Reflection.BindingFlags.GetProperty, null, i.item, null)).ToString());

                        /* Agrego el elemento a la lista de elementos seleccionados. */
                        this.itemsSelected = items.ToList();
                        cantSeleccionados = devices.Count();
                        elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);

                        /* Actualizo el arreglo general de todos los elementos con el filtro del Id seleccionado. */
                        //var filteredObjects = optionAllObjects.Where(t => !items.Contains(t.Key)).ToList();
                        //var localfilteredObjects = (valueObject == "Devices") ? optionAllDevices.Where(t => !items.Contains(t.Key)).ToList() :
                        //                           (valueObject == "Analytics") ? optionAllAnalytics.Where(t => !items.Contains(t.Key)).ToList() :
                        //                           (valueObject == "Locations") ? optionAllLocations.Where(t => !items.Contains(t.Key)).ToList() :
                        //                           optionAllCarousels.Where(t => !items.Contains(t.Key)).ToList();

                        /* Actualizo el "DataSource" del objeto "this.optionObjectName" con los datos del objeto, tipo de objeto y sitio seleccionados. */
                        //this.optionObjectName.DataSource = ChangeObjectFiltered(ref localfilteredObjects, valueObject, valueTypeObject, valueSiteObject);

                        /* NOTA: Cuando ya no hay elementos que elegir, se desaparece el combo de elementos. */
                        if (this.ucOptionObjectName.SelectedIndex == -1)
                        {
                            HideObjects(false);
                        }
                        else
                        {
                            HideObjects(true); this.ucOptionObjectName.SelectedIndex = idx > this.ucOptionObjectName.Items.Count ? this.ucOptionObjectName.Items.Count : idx;
                        }

                        // this.optionObjectName.DataSource = optionObjectsName.Where(t => !items.Contains(t.Key)).ToList();
                        OptionObjectDTO typeGridSelected = (OptionObjectDTO)this.optionGrid.SelectedItem;
                        if (typeGridSelected != null)
                        {
                            List<GridDTO> gridTypes = _gridService.Get(new GridFilterDTO
                            {
                                Id = typeGridSelected.Key
                            });

                            if (gridTypes[0].Elements.Count > cantSeleccionados)
                            {
                                ButtonAdd.Visible = true;
                            }
                        }
                        SetOptionType(item.IsPrivate);

                        panelPag.Visible = (devices.Count > 1);
                        controlPag.UpdatePage(devices.Count, 1);
                        GenericFormControl.EnabledButtonOkEvent(cantSeleccionados >= 1);
                        return true;
                    }
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
                    bindingSourceGroup.Clear();
                    _entity = item;
                    bindingSourceGroup.DataSource = _entity;
                    bindingSourceGroup.ResetBindings(true);
                    errorManager.Clear();

                    //var devices = _entity.Elements?.Where(p => !p.IsDeleted).Select(p => GroupViewModel.GetElement(p)).Where(p => p != null).ToList();
                    var tasks = _entity.Elements?.Where(p => !p.IsDeleted).Select(p => GroupViewModel.GetElement(p));
                    if (tasks != null)
                    {
                        var results = await Task.WhenAll(tasks);
                        var devicesList = results.Where(p => p != null).ToList();
                        this.devices = devicesList;

                        dataView.Clear();
                        if (devices != null && devices.Count() > 0)
                        {
                            cantSeleccionados = devices.Count();
                            this.itemsSelected.AddRange(_entity.Elements.Select(d => d.ObjectId.ToString()));
                            devices = devices;
                            dataView.AddRange(PagePagination(1));
                        }
                        else
                        {
                            cantSeleccionados = 0;
                        }

                        bunifuDataObject.DataSource = typeof(List<DataViewGroup>);
                        bunifuDataObject.DataSource = dataView;
                        elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);

                        /* NOTA: Se corrige aquí el bug generado cuando se aprieta el botón "Desde grilla" muchas veces para refrescar las variables "Nombre del grupo" y "Grilla seleccionada". */
                        this.txtName.Text = item.Name; this.optionGrid.SelectedValue = ItemGroupSelected.GridId;
                        panelPag.Visible = (devices.Count > 1);
                        controlPag.UpdatePage(devices.Count, 1);
                        GenericFormControl.EnabledButtonOkEvent(cantSeleccionados >= 1);
                        return true;
                    }
                }
            }

            return false;
        }

        public override async Task<bool> Delete()
        {
            if (SelectedItemOption != null)
            {
                int id = SelectedItemOption.Id;
                return await GroupViewModel.DeleteGroup(id);

                //devices.RemoveAt(0);
            }



            return true;
        }

        public override bool IsValid()
        {
            // Aquí se deben agregar las validaciones necesarias para crear o editar un grupo.           
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show(string.Format(Resources.FieldRequired, Resources.Name), Resources.action, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus(); return false;
            }

            if (optionGrid.SelectedValue == null)
            {
                MessageBox.Show(Resources.GridTypeRequired, Resources.action, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                optionGrid.Focus(); return false;
            }

            if (!ValidateSelectedDevices())
            {
                MessageBox.Show(Resources.GridTypeToDevicesNotEquals, Resources.action, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        public GroupsViewModel GroupViewModel => (ViewModel as GroupsViewModel);

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
            bool showMessage = false;

            bunifuDataObject.EndEdit();

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
                        var item = bunifuDataObject.Rows[i].DataBoundItem as DataViewGroup;
                        var id = Convert.ToInt32(item.Id);
                        cantSeleccionados--;
                        elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);
                        if (id == 0 || (await GroupViewModel.DeleteElementOfGroup(id)))
                        {
                            dataView.Remove(bunifuDataObject.Rows[i].DataBoundItem as DataViewGroup);
                            try
                            {
                                this.itemsSelected.Remove((((SidebarElementDTO)item.item).ElementId).ToString());
                                devices.Remove(devices.SingleOrDefault(d => d.Id == id.ToString()));
                            }
                            catch
                            {
                                Logger.Log("Error on deleted item device", LogPriority.Warning);
                            }
                        }
                    }
                }

                var valueTypeObject = this.optionTypeObject.SelectedValue.ToString();
                var valueSiteObject = this.ucOptionSitesName.SelectedValue.ToString();
                var valueObject = this.optionObject.SelectedValue.ToString();


                //var items = dataView.Select(i => i.item.GetType().GetProperty("ObjectId") != null ?
                //        ((int)i.item.GetType().InvokeMember("ObjectId", System.Reflection.BindingFlags.GetProperty, null, i.item, null)).ToString() :
                //        ((int)i.item.GetType().InvokeMember("Id", System.Reflection.BindingFlags.GetProperty, null, i.item, null)).ToString());

                /* Agrego el elemento a la lista de elementos seleccionados. */
                //this.itemsSelected = items.ToList();
                cantSeleccionados = this.itemsSelected.Count(); elementosSeleccionados.Text = string.Format(Resources.elementSelected, cantSeleccionados);

                /* Actualizo el arreglo general de todos los elementos con el filtro del Id seleccionado. */
                //var filteredObjects = optionAllObjects.Where(t => !items.Contains(t.Key)).ToList();
                //var localfilteredObjects = (valueObject == "Devices") ? optionAllDevices.Where(t => !items.Contains(t.Key)).ToList() :
                //                           (valueObject == "Analytics") ? optionAllAnalytics.Where(t => !items.Contains(t.Key)).ToList() :
                //                           (valueObject == "Locations") ? optionAllLocations.Where(t => !items.Contains(t.Key)).ToList() :
                //                           optionAllCarousels.Where(t => !items.Contains(t.Key)).ToList();

                /* Actualizo el "DataSource" del objeto "this.optionObjectName" con los datos del objeto, tipo de objeto y sitio seleccionados. */
                //this.optionObjectName.DataSource = ChangeObjectFiltered(ref localfilteredObjects, valueObject, valueTypeObject, valueSiteObject);
                var idx = this.ucOptionObjectName.SelectedIndex;
                /* NOTA: Cuando ya no hay elementos que elegir, se desaparece el combo de elementos. */
                if (this.ucOptionObjectName.SelectedIndex == -1)
                {
                    HideObjects(false);
                }
                else
                {
                    HideObjects(true); this.ucOptionObjectName.SelectedIndex = idx > this.ucOptionObjectName.Items.Count ? this.ucOptionObjectName.Items.Count : idx;
                }

                /* this.optionObjectName.DataSource = optionObjectsName.Where(t => !items.Contains(t.Key)).ToList(); */
                OptionObjectDTO typeGridSelected = (OptionObjectDTO)this.optionGrid.SelectedItem;
                if (typeGridSelected == null)
                {
                    return;
                }

                List<GridDTO> gridTypes = _gridService.Get(new GridFilterDTO
                {
                    Id = typeGridSelected.Key
                });

                if (gridTypes[0].Elements.Count > cantSeleccionados)
                {
                    ButtonAdd.Visible = true;
                }
                if (showMessage)
                {
                    bunifuDataObject.DataSource = typeof(List<DataViewGroup>);
                    bunifuDataObject.DataSource = dataView;
                }
                GenericFormControl.EnabledButtonOkEvent(cantSeleccionados >= 1);
            }
        }

        private void BunifuDataObject_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex == -1 && e.ColumnIndex == deleteDataGridViewCheckBoxColumn.Index)
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

        private void OptionGrid_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (optionGrid.SelectedValue == null)
            {
                errorManager.SetErrorWithCount(optionGrid, Resources.required);
            }
            else
            {
                errorManager.SetErrorWithCount(optionGrid, string.Empty);
            }
        }

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

            if (e.Effect == DragDropEffects.Move && rowIndexOfItemUnderMouseToDrop != -1)
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

        private void BunifuDataObject_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == StreamColumn.Index)
                {
                    var otem = (DataViewGroup)bunifuDataObject.Rows[e.RowIndex].DataBoundItem;
                    var dt = (List<OptionObjectDTO>)((System.Windows.Forms.DataGridViewComboBoxCell)bunifuDataObject.Rows[e.RowIndex].Cells[e.ColumnIndex]).DataSource;
                    var newProfileStream = bunifuDataObject.Rows[e.RowIndex].Cells[e.ColumnIndex].EditedFormattedValue.ToString();
                    var profileStream = bunifuDataObject.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue.ToString();
                    if (newProfileStream != profileStream)
                    {
                        var oProfileStream = dt.SingleOrDefault(a => a.Name == newProfileStream);
                        if (oProfileStream != null)
                        {

                            if (devices.Count() > 0)
                            {
                                var devicesToUpdate = devices.Where(d => d.Id == otem.Id.ToString());

                                foreach (var d in devicesToUpdate)
                                {
                                    var id = (d.item as SidebarElementDTO).ElementId;
                                    if ((d.item as SidebarElementDTO).ElementId == (otem.item as SidebarElementDTO).ElementId)
                                    {
                                        d.streamProfile = oProfileStream.Key;
                                    }

                                }
                            }

                            //var device = devices.FirstOrDefault(d => d.Id == otem.Id.ToString());

                            //// Verificamos que realmente se encontró algo
                            //if (device != null)
                            //{
                            //    device.streamProfile = oProfileStream.Key;
                            //}

                            //if (devices.Count() > 0)
                            //{
                            //    devices.SingleOrDefault(d => d.Id == otem.Id.ToString()).streamProfile = oProfileStream.Key;
                            //}
                        }
                    }
                }
            }
        }

        private List<OptionObjectDTO> FilterObjects(string valueObject, string valueTypeObject, string valueSiteObject)
        {
            var list = new List<OptionObjectDTO>();                      /* Objeto devuelto. */
            var listObjectsFiltered = new List<OptionObjectDTO>();       /* Objetos filtrados. */
            var listAll = (valueObject == "Devices") ? optionAllDevices?.ToList() :
                          (valueObject == "Analytics") ? optionAllAnalytics?.ToList() :
                          (valueObject == "Locations") ? optionAllLocations?.ToList() :
                          (valueObject == "AlarmsMap") ? optionAllSites?.ToList() :
                          optionAllCarousels?.ToList();

            if (listAll == null)
            {
                Logger.Log($"FilterObjects: La lista para '{valueObject}' es null", LogPriority.Warning);
                return list;
            }

            /* Filtramos los objetos por sucursal. */
            var AllSites = listAll.Where(u => u.Tag != null && u.Tag.ToString() == valueSiteObject).ToList();

            switch (valueTypeObject)
            {
                case "Camera":
                    foreach (var Items in AllSites)
                    {
                        dynamic u = Items.Item;

                        if (u.Type != "DO" && u.Type != "DI")
                        {
                            listObjectsFiltered.Add(Items);
                        }
                    }
                    break;
                case "Iot_In":
                    foreach (var Items in AllSites)
                    {
                        dynamic u = Items.Item;

                        if (u.Type == "DI")
                        {
                            listObjectsFiltered.Add(Items);
                        }
                    }
                    break;
                case "Iot_Out":
                    foreach (var Items in AllSites)
                    {
                        dynamic u = Items.Item;

                        if (u.Type == "DO")
                        {
                            listObjectsFiltered.Add(Items);
                        }
                    }
                    break;
                case "Kpi":
                    foreach (var Items in AllSites)
                    {
                        dynamic u = Items.Item;

                        if (u.Type == valueTypeObject.ToUpper())
                        {
                            listObjectsFiltered.Add(Items);
                        }
                    }
                    break;
                case "Lpr":
                    foreach (var Items in AllSites)
                    {
                        dynamic u = Items.Item;

                        if (u.Type == valueTypeObject.ToUpper())
                        {
                            listObjectsFiltered.Add(Items);
                        }
                    }
                    break;
                case "Face":
                    foreach (var Items in AllSites)
                    {
                        dynamic u = Items.Item;

                        if (u.Type == valueTypeObject.ToUpper())
                        {
                            listObjectsFiltered.Add(Items);
                        }
                    }
                    break;
                default:
                    /* Todos. */
                    listObjectsFiltered = AllSites;
                    break;
            }

            /* Filtramos la lista final. */
            foreach (var item in listObjectsFiltered)
            {
                list.Add(item);
            }

            /* Finalmente cargamos el set de datos. */
            return list;
        }

        private List<OptionObjectDTO> ChangeObjectFiltered(ref List<OptionObjectDTO> items, string valueObject, string valueTypeObject, string valueSiteObject)
        {
            var list = new List<OptionObjectDTO>();                      /* Objeto devuelto. */
            var listObjectsFiltered = new List<OptionObjectDTO>();       /* Objetos filtrados. */
            List<OptionObjectDTO> AllSites = new List<OptionObjectDTO>();
            if (valueObject == "Carousels")
            {
                AllSites = items;
            }
            else
            {
                AllSites = items.Where(u => u.Tag != null && u.Tag.ToString() == valueSiteObject).ToList();
            }

            switch (valueTypeObject)
            {
                case "Camera":
                    foreach (var Items in AllSites)
                    {
                        dynamic u = Items.Item;

                        if (u.Type != "DO" && u.Type != "DI")
                        {
                            listObjectsFiltered.Add(Items);
                        }
                    }
                    break;
                case "Iot_In":
                    foreach (var Items in AllSites)
                    {
                        dynamic u = Items.Item;

                        if (u.Type == "DI")
                        {
                            listObjectsFiltered.Add(Items);
                        }
                    }
                    break;
                case "Iot_Out":
                    foreach (var Items in AllSites)
                    {
                        dynamic u = Items.Item;

                        if (u.Type == "DO")
                        {
                            listObjectsFiltered.Add(Items);
                        }
                    }
                    break;
                case "Kpi":
                    foreach (var Items in AllSites)
                    {
                        dynamic u = Items.Item;

                        if (u.Type == valueTypeObject.ToUpper())
                        {
                            listObjectsFiltered.Add(Items);
                        }
                    }
                    break;
                case "Lpr":
                    foreach (var Items in AllSites)
                    {
                        dynamic u = Items.Item;

                        if (u.Type == valueTypeObject.ToUpper())
                        {
                            listObjectsFiltered.Add(Items);
                        }
                    }
                    break;
                case "Face":
                    foreach (var Items in AllSites)
                    {
                        dynamic u = Items.Item;

                        if (u.Type == valueTypeObject.ToUpper())
                        {
                            listObjectsFiltered.Add(Items);
                        }
                    }
                    break;
                default:
                    /* Todos. */
                    listObjectsFiltered = AllSites;
                    break;
            }

            /* Filtramos la lista final. */
            foreach (var item in listObjectsFiltered)
            {
                list.Add(item);
            }

            items.Clear(); items = new List<OptionObjectDTO>(list);
            return items;
        }

        private void optionGrid_SelectedValueChanged(object sender, EventArgs e)
        {
            if (optionGrid.SelectedValue != null && selectedValue)
            {

                OptionObjectDTO typeGridSelected = (OptionObjectDTO)this.optionGrid.SelectedItem;
                if (typeGridSelected != null)
                {
                    List<GridDTO> gridTypes = _gridService.Get(new GridFilterDTO
                    {
                        Id = typeGridSelected.Key
                    });

                    if (gridTypes[0].Elements.Count == cantSeleccionados)
                    {
                        previousSelected = ((OptionObjectDTO)this.optionGrid.SelectedItem).Key;
                        optionGrid.SelectedValue = previousSelected;
                        ButtonAdd.Visible = false;
                    }
                    else if (gridTypes[0].Elements.Count < cantSeleccionados)
                    {
                        if (!string.IsNullOrEmpty(previousSelected))
                        {
                            optionGrid.SelectedValue = previousSelected;
                        }

                        notification.Show(Resources.ErrorSelectedTypeGrilla, null);
                        ButtonAdd.Visible = false;
                    }
                    else
                    {
                        ButtonAdd.Visible = true;
                    }

                }
            }
        }

        public void ControlsResize()
        {
            if (Screen.AllScreens.Length >= 1 && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                #region Estilos

                if (main.Width > 1400 && main.Width < 2000)
                {

                    //lblTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Large_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    //IndexEntity.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    //txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel
                    //

                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Bold, GraphicsUnit.Pixel);
                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    optionGrid.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    tipoObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    optionObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    optionTypeObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    ucOptionSitesName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    ucOptionObjectName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
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
                    optionGrid.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    tipoObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementsAvailable.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    optionObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    optionTypeObject.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    ucOptionSitesName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    ucOptionObjectName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.Font = FontHelper.GetRobotoRegular(FontSizes.Small_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersDefaultCellStyle.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Bold, GraphicsUnit.Pixel);
                    bunifuDataObject.RowsDefaultCellStyle.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    bunifuDataObject.ColumnHeadersHeight = 30;
                    bunifuDataObject.RowTemplate.Height = 30;
                    ButtonAdd.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    elementosSeleccionados.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAdd.IdleBorderRadius = 20;
                    ButtonAdd.OnIdleState.BorderRadius = 20;

                    //GroupName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);


                }
                else if (main.Width == 1024 && main.Height == 768)
                {
                    tilteForm.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Bold, GraphicsUnit.Pixel);
                    tilteForm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

                    txtName.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    optionGrid.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    tipoObject.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    optionObject.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    optionTypeObject.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    ucOptionSitesName.Font = FontHelper.GetRobotoRegular(FontSizes.Small_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    ucOptionObjectName.Font = FontHelper.GetRobotoRegular(FontSizes.Small_2, FontStyle.Regular, GraphicsUnit.Pixel);

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
                panel1.Size = new Size(panel1Width, panel1Height);

                //15, 11 4, 3
                var tilteFormX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0078M), 2));
                var tilteFormY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0101M), 2));

                //41, 12
                var labelNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0215M), 2));
                var labelNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                //5, 58
                var labelNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0027M), 2));
                var labelNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.054M), 2));

                //218, 20
                var txtNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1135M), 2));
                var txtNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0185M), 2));

                //8, 90
                var txtNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0041M), 2));
                var txtNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0833M), 2));

                //62, 12
                var labelTypeGridWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0323M), 2));
                var labelTypeGridHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                //262, 58
                var labelTypeGridX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1364M), 2));
                var labelTypeGridY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0537M), 2));

                //218, 32
                var optionGridWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1135M), 2));
                var optionGridHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

                //264, 78
                var optionGridX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1375M), 2));
                var optionGridY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0722M), 2));

                //205, 10
                var bunifuSeparator4Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1067M), 2));
                var bunifuSeparator4Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.009M), 2));

                //264, 104
                var bunifuSeparator4X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1375M), 2));
                var bunifuSeparator4Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0963M), 2));

                //165, 20
                var elementsAvailableWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.086M), 2));
                var elementsAvailableHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0186M), 2));

                //4, 152
                var elementsAvailableX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.002M), 2));
                var elementsAvailableY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.141M), 2));

                //35, 12
                var labelObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0183M), 2));
                var labelObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                //6, 197
                var labelObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.003M), 2));
                var labelObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1826M), 2));

                //69, 12
                var labelTypeObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.036M), 2));
                var labelTypeObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                //149, 202
                var labelTypeObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0776M), 2));
                var labelTypeObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1870M), 2));

                //27, 12
                var labelSitesGroupWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.014M), 2));
                var labelSitesGroupHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                //290, 202
                var labelSitesGroupX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1510M), 2));
                var labelSitesGroupY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1870M), 2));

                //88, 12
                var labelNameObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.046M), 2));
                var labelNameObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.011M), 2));

                //449, 202
                var labelNameObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2338M), 2));
                var labelNameObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1870M), 2));

                //134, 32
                var optionObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0697M), 2));
                var optionObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

                //4, 216
                var optionObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.002M), 2));
                var optionObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));

                //134, 32
                var optionTypeObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0697M), 2));
                var optionTypeObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

                //144, 216
                var optionTypeObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.075M), 2));
                var optionTypeObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));

                //145, 32
                var optionSitesNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0755M), 2));
                var optionSitesNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

                //292, 216
                var optionSitesNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1520M), 2));
                var optionSitesNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));

                //145, 32
                var optionObjectNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0807M), 2));
                var optionObjectNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

                //451, 216
                var optionObjectNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2348M), 2));
                var optionObjectNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));

                //121, 37
                var ButtonAddWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.063M), 2));
                var ButtonAddHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0345M), 2));

                //601, 216
                var ButtonAddX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3130M), 2));
                var ButtonAddY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2));

                //127, 13
                var elementosSeleccionadosWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.066M), 2));
                var elementosSeleccionadosHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.012M), 2));

                //562, 263
                var elementosSeleccionadosX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2890M), 2));
                var elementosSeleccionadosY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2435M), 2));

                //117, 10
                var bunifuSeparator1Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0609M), 2));
                var bunifuSeparator1Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.009M), 2));

                //7, 244
                var bunifuSeparator1X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0037M), 2));
                var bunifuSeparator1Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2));

                //137, 10
                var bunifuSeparatorTypeWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0609M), 2));
                var bunifuSeparatorTypeHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.009M), 2));

                //151, 244
                var bunifuSeparatorTypeX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0786M), 2));
                var bunifuSeparatorTypeY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2));

                //146, 10
                var bunifuSeparator2Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.07604M), 2));
                var bunifuSeparator2Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.009M), 2));

                //292, 244
                var bunifuSeparator2X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1520M), 2));
                var bunifuSeparator2Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2));

                //148, 5
                var bunifuSeparator3Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0770M), 2));
                var bunifuSeparator3Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0046M), 2));

                //451, 244
                var bunifuSeparator3X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2348M), 2));
                var bunifuSeparator3Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2));

                //720, 170
                var bunifuDataObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.375M), 2));
                var bunifuDataObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1574M), 2));

                //4, 286
                var bunifuDataObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.002M), 2));
                var bunifuDataObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.265M), 2));

                //41,16
                var label1Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0213M), 2));
                var label1Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0148M), 2));

                //510, 58
                var label1X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2656M), 2));
                var label1Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0537M), 2));

                //218, 32
                var tipoObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1135M), 2));
                var tipoObjectHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0296M), 2));

                //614, 75 512, 78
                var tipoObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2666M), 2));
                var tipoObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0694M), 2));

                //205, 10
                var bunifuSeparator5Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1067M), 2));
                var bunifuSeparator5Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0092M), 2));

                //512, 104
                var bunifuSeparator5X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2666M), 2));
                var bunifuSeparator5Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0962M), 2));

                //264, 463
                var panelPagX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1375M), 2));
                var panelPagY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.42870M), 2));

                this.optionGrid.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                this.optionObject.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                this.optionTypeObject.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                this.ucOptionSitesName.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                this.ucOptionObjectName.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

                // Edición de propiedades segun resolución
                if (main.Width == 1024 && main.Height == 768)
                {
                    tilteFormX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0078M), 2)) + 5;
                    label1Height = 12;
                    label1Width = 25;

                    tipoObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1135M), 2)) - 6;
                    bunifuSeparator5Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1067M), 2)) - 10;

                    int displY = -18;
                    elementsAvailableY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.141M), 2)) - 15;

                    labelObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.003M), 2));
                    labelObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1826M), 2)) + displY;
                    optionObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.002M), 2));
                    optionObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2)) + displY;
                    optionObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0732M), 2));
                    bunifuSeparator1X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0037M), 2));
                    bunifuSeparator1Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2)) + displY;

                    labelTypeObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0801M), 2));
                    labelTypeObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1826M), 2)) + displY;
                    optionTypeObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0802M), 2));
                    optionTypeObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2)) + displY;
                    optionTypeObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0732M), 2));
                    bunifuSeparatorTypeX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0808M), 2));
                    bunifuSeparatorTypeY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2)) + displY;

                    labelSitesGroupX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1600M), 2));
                    labelSitesGroupY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1826M), 2)) + displY;
                    optionSitesNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1601M), 2));
                    optionSitesNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2)) + displY;
                    optionSitesNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1050M), 2));
                    bunifuSeparator2Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.07604M), 2)) + 20;
                    bunifuSeparator2Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.009M), 2));
                    bunifuSeparator2X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1607M), 2));
                    bunifuSeparator2Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2)) + displY;

                    labelNameObjectX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2700M), 2));
                    labelNameObjectY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1826M), 2)) + displY;
                    optionObjectNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2701M), 2));
                    optionObjectNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2)) + displY;
                    optionObjectNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0998M), 2));
                    bunifuSeparator3Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0770M), 2)) + 15;
                    bunifuSeparator3Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0046M), 2));
                    bunifuSeparator3X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2707M), 2));
                    bunifuSeparator3Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2261M), 2)) + displY;

                    elementosSeleccionadosX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.002M), 2));

                    ButtonAddX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3130M), 2)) - 15;
                    ButtonAddY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.200M), 2)) + 15;

                    bunifuDataObjectWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.375M), 2)) - 15;
                }
                else if ((main.Width > 2020 && main.Width < 2100) && (main.Height > 1200 && main.Height < 1300))
                {
                    ButtonAddX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3130M), 2)) - 10;
                    ButtonAddWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.063M), 2)) - 20;
                }

                // Asignación de propiedades a elementos del form
                tilteForm.Location = new Point(tilteFormX, tilteFormY);
                labelName.Size = new Size(panel1Width, panel1Height);
                labelName.Location = new Point(labelNameX, labelNameY);
                txtName.Size = new Size(txtNameWidth, txtNameHeight);
                txtName.MinimumSize = new Size(txtNameWidth, txtNameHeight);
                txtName.Location = new Point(txtNameX, txtNameY);

                // Grid Type
                labelTypeGrid.Size = new Size(labelTypeGridWidth, labelTypeGridHeight);
                labelTypeGrid.Location = new Point(labelTypeGridX, labelTypeGridY);
                optionGrid.Size = new Size(optionGridWidth, optionGridHeight);
                optionGrid.Location = new Point(optionGridX, optionGridY);
                bunifuSeparator4.Size = new Size(bunifuSeparator4Width, bunifuSeparator4Height);
                bunifuSeparator4.Location = new Point(bunifuSeparator4X, bunifuSeparator4Y);

                // Tipo
                label1.Size = new Size(label1Width, label1Height);
                label1.Location = new Point(label1X, label1Y);
                tipoObject.Size = new Size(tipoObjectWidth, tipoObjectHeight);
                tipoObject.Location = new Point(tipoObjectX, tipoObjectY);
                bunifuSeparator5.Size = new Size(bunifuSeparator5Width, bunifuSeparator5Height);
                bunifuSeparator5.Location = new Point(bunifuSeparator5X, bunifuSeparator5Y);

                elementsAvailable.Size = new Size(elementsAvailableWidth, elementsAvailableHeight);
                elementsAvailable.Location = new Point(elementsAvailableX, elementsAvailableY);

                // Object
                labelObject.Size = new Size(labelObjectWidth, labelObjectHeight);
                labelObject.Location = new Point(labelObjectX, labelObjectY);
                optionObject.Size = new Size(optionObjectWidth, optionObjectHeight);
                optionObject.Location = new Point(optionObjectX, optionObjectY);
                bunifuSeparator1.Size = new Size(bunifuSeparator1Width, bunifuSeparator1Height);
                bunifuSeparator1.Location = new Point(bunifuSeparator1X, bunifuSeparator1Y);

                // Object Type
                labelTypeObject.Size = new Size(labelTypeObjectWidth, labelTypeObjectHeight);
                labelTypeObject.Location = new Point(labelTypeObjectX, labelTypeObjectY);
                optionTypeObject.Size = new Size(optionTypeObjectWidth, optionTypeObjectHeight);
                optionTypeObject.Location = new Point(optionTypeObjectX, optionTypeObjectY);
                bunifuSeparatorType.Size = new Size(bunifuSeparatorTypeWidth, bunifuSeparatorTypeHeight);
                bunifuSeparatorType.Location = new Point(bunifuSeparatorTypeX, bunifuSeparatorTypeY);

                // Site
                labelSitesGroup.Size = new Size(labelSitesGroupWidth, labelSitesGroupHeight);
                labelSitesGroup.Location = new Point(labelSitesGroupX, labelSitesGroupY);
                ucOptionSitesName.Size = new Size(optionSitesNameWidth, optionSitesNameHeight);
                ucOptionSitesName.Location = new Point(optionSitesNameX, optionSitesNameY);
                bunifuSeparator2.Size = new Size(bunifuSeparator2Width, bunifuSeparator2Height);
                bunifuSeparator2.Location = new Point(bunifuSeparator2X, bunifuSeparator2Y);

                // Object Name
                labelNameObject.Size = new Size(labelNameObjectWidth, labelNameObjectHeight);
                labelNameObject.Location = new Point(labelNameObjectX, labelNameObjectY);
                ucOptionObjectName.Size = new Size(optionObjectNameWidth, optionObjectNameHeight);
                ucOptionObjectName.Location = new Point(optionObjectNameX, optionObjectNameY);
                bunifuSeparator3.Size = new Size(bunifuSeparator3Width, bunifuSeparator3Height);
                bunifuSeparator3.Location = new Point(bunifuSeparator3X, bunifuSeparator3Y);

                ButtonAdd.Size = new Size(ButtonAddWidth, ButtonAddHeight);
                ButtonAdd.Location = new Point(ButtonAddX, ButtonAddY);
                elementosSeleccionados.Size = new Size(elementosSeleccionadosWidth, elementosSeleccionadosHeight);
                elementosSeleccionados.Location = new Point(elementosSeleccionadosX, elementosSeleccionadosY);

                bunifuDataObject.Size = new Size(bunifuDataObjectWidth, bunifuDataObjectHeight);
                bunifuDataObject.Location = new Point(bunifuDataObjectX, bunifuDataObjectY);

                panelPag.Location = new Point(panelPagX, panelPagY);
            }
        }

        public void SetAllOptionType()
        {
            List<OptionObjectDTO> optionObjectDTO = new List<OptionObjectDTO>()
            {
                new OptionObjectDTO { IsPrivate = false ,Name ="public" },
                new OptionObjectDTO { IsPrivate = true ,Name ="private" },
            };
            tipoObject.DataSource = optionObjectDTO;
        }

        public void SetOptionType(bool type)
        {
            List<OptionObjectDTO> optionObjectDTO = new List<OptionObjectDTO>();
            if (type)
            {
                optionObjectDTO.Add(new OptionObjectDTO { IsPrivate = true, Name = "private" });
                optionObjectDTO.Add(new OptionObjectDTO { IsPrivate = false, Name = "public" });
            }
            else
            {
                optionObjectDTO.Add(new OptionObjectDTO { IsPrivate = false, Name = "public" });
                optionObjectDTO.Add(new OptionObjectDTO { IsPrivate = true, Name = "private" });
            }
            tipoObject.DataSource = optionObjectDTO;
        }

        private List<DataViewGroup> PagePagination(int Page)
        {
            var pageItem = new List<DataViewGroup>();
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
            dataView.Clear();
            dataView.AddRange(PagePagination(page));
            bunifuDataObject.DataSource = typeof(List<DataViewGroup>);
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

        private void optionGrid_DrawItem(object sender, DrawItemEventArgs e)
        {

        }
        private void UcOptionObjectName_Search(object sender, string textSearchs)
        {
            //List<OptionObjectDTO> listaFiltrada;
            this.textsearch = textSearchs;
            this.pageOptionDeviceObject = 1;
            GetSiteObjectNames();
        }

        private void UcOptionSiteName_Search(object sender, string textSearchs)
        {
            this.textSiteSearch = textSearchs;
            this.pageOptionSiteObject = 1;
            GetNames();
        }
    }
}
