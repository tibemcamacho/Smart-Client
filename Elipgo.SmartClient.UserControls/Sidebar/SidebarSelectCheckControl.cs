using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Services.Services.Interface;
using Splat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Resources = Elipgo.SmartClient.Common.Properties.Resources;

namespace Elipgo.SmartClient.UserControls.Sidebar
{

    public delegate void SelectSiteObjectName(object sender, EventArgs e);
    public delegate void SearchSiteObjectName(object sender, string textSearchs);
    public partial class SidebarSelectCheckControl : UserControl
    {
        private readonly IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        public event SelectSiteObjectName SelectSiteObjectName;
        public event SearchSiteObjectName SearchSiteObjectName;
        //public List<OptionObjectDTO> Parent { get; set; }

        public List<OptionObjectDTO> Child { get; set; }
        private string defaultText;
        //  private bool bRemove = false;
        private List<OptionObjectDTO> _listOptionSites = new List<OptionObjectDTO>();
        private short _takeDropdown = 0;
        public short pageSiteObject = 0;

        public bool isSearchingMode { get => this.ucbddParent.isSearchingMode; }

        public List<string> ChkListChecked
        {
            get
            {
                var listChecked = this.itemCheckList1.GetItemChecked().Select(x => x.Key).ToList();
                return listChecked.SelectMany(key =>
                {
                    return TypeAlarms.EnumAlarmsCluster.TryGetValue(key, out var replacement)
                        ? replacement.Split(',').Select(x => x.Trim()).ToList()
                        : new List<string> { key };
                }).ToList();
            }
        }

        public int DropDownListSelect
        {
            get
            {
                var item = this.ucbddParent.SelectedItem;
                string pos = (this.ucbddParent.SelectedItem != null && ((item as OptionObjectDTO)?.Name ?? (item as CatalogSite)?.Name) != defaultText) ? ((item as OptionObjectDTO)?.Key ?? (item as CatalogSite)?.Id.ToString()) : "0";
                return int.Parse(pos);
            }
        }



        private string path = AppDomain.CurrentDomain.BaseDirectory;

        public SidebarSelectCheckControl(List<string> typesAlarms = null)
        {
            InitializeComponent();
            var _config = SmartClientEnvironmentUtils.GetConfiguration();
            _takeDropdown = Int16.Parse(_config.AppSettings.Settings["takeDropdown"].Value);
            this.CreateList(typesAlarms);
            CultureInfo ci = CultureInfo.InstalledUICulture;
            defaultText = ci.Name.Contains("es") ? ButtonsContextBar.defaultSiteText.GetDescription() : ButtonsContextBar.defaultSiteText.GetAttribute<DescriptionEN>().Descripcion;
            ucbddParent.SearchRequested += ucSiteName_Search;
        }

        private void BddParent_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedDevice = this.ucbddParent.SelectedItem as OptionObjectDTO;
            if (selectedDevice != null && selectedDevice.Key == "0")
            {
                SelectSiteObjectName(sender, e);
            }
        }

        private void CreateList(List<string> typesAlarms)
        {
            var lstEnum = this.getTypeAlarm(typesAlarms);
            this.itemCheckList1.loadSource(lstEnum);

        }

        public void UpdateList(List<string> typesAlarms)
        {
            var checkList = this.ChkListChecked;

            this.CreateList(typesAlarms);
        }

        public List<CheckElementDTO> getTypeAlarm(List<string> typesAlarms)
        {
            List<CheckElementDTO> lst = new List<CheckElementDTO>();

            Type t = typeof(AlarmType);
            foreach (var item in t.GetMembers().Where(m => m.GetCustomAttribute(typeof(AlarmTypeOf)) != null))
            {
                Enum.TryParse(item.Name, out AlarmType alarmTypeValue);
                var displayName = TypeAlarms.GetDisplayName(alarmTypeValue);
                AlarmTypeOf subAttr = (AlarmTypeOf)item.GetCustomAttribute(typeof(AlarmTypeOf));
                GroupAlarmType groupAlarmType = subAttr.AlarmType;
                string authorization = groupAlarmType.GetDescription();
                if (appAuthorization.Exist(authorization))
                {
                    var translatedAlarmType = Resources.ResourceManager.GetString(
                              item.Name,
                              Thread.CurrentThread.CurrentUICulture
                          )
                          ?? item.Name;
                    lst.Add(new CheckElementDTO()
                    {
                        Key = item.Name.ToUpper(),
                        Name = translatedAlarmType,
                        State = true
                    });
                }

            }
            lst.Add(new CheckElementDTO()
            {
                Key = "Generic".ToUpper(),
                Name = "Generic",
                State = true
            });

            //foreach (var item in Enum.GetValues(typeof(AlarmType)))
            //{
            //    var eType = (AlarmType)item;
            //    var itemEnum = eType.GetAttribute<DisplayAttribute>();

            //    if (itemEnum != null)
            //    {
            //        lst.Add(new CheckElementDTO()
            //        {
            //            Key = eType.convertToString(),
            //            Name = itemEnum.Name,
            //            State = true
            //        });
            //    }
            //}
            return lst;
        }

        public void loadSites(List<OptionObjectDTO> list)
        {
            if (pageSiteObject == 1)
            {
                _listOptionSites.Clear();
            }
            this.ucbddParent.Items.Clear();
            _listOptionSites.RemoveAll(x => x.Key == "0");
            if (list.Count != 0 && list[0].count > _takeDropdown)
            {
                float totalpage = (int)Math.Ceiling((double)list[0].count / _takeDropdown);
                if (totalpage > pageSiteObject)
                {
                    var seemore = new OptionObjectDTO { Key = "0", Name = Resources.ViewMore };
                    list.Add(seemore);
                }
            }
            _listOptionSites.AddRange(list);
            //list = list ?? new List<CatalogSite>();
            foreach (var item in _listOptionSites)
            {
                this.ucbddParent.Items.Add(item);
            //this.ucbddParent.DataSource = list;
                this.ucbddParent.DisplayMember = "Name";
                this.ucbddParent.ValueMember = "Key";
            }


            this.ucbddParent.Items.Insert(0, new CatalogSite
            {
                Id = -1,
                Name = defaultText
            });
            this.ucbddParent.SelectedIndex = 0;
            //    this.bddParent.SelectedIndexChanged += BddParent_SelectedIndexChanged;
        }

        private void chkList_ChangeCheckFilter(List<CheckElementDTO> list)
        {
        }

        private void ucSiteName_Search(object sender, string textSearchs)
        {
            //this.textSiteSearch = textSearchs;
            this.pageSiteObject = 1;
            SearchSiteObjectName(sender, textSearchs);
        }
    }
}
