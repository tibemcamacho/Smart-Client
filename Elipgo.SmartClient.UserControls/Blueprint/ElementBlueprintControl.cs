using CefSharp;
using CefSharp.WinForms;
using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Reflections;
using Elipgo.SmartClient.Drivers;
using Elipgo.SmartClient.UserControls.ElementContainer;
using Elipgo.SmartClient.ViewModels;
using Splat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Blueprint
{
    public partial class ElementBlueprintControl : UserControl, IDisposable
    {
        public event ObjectSelectedEventHandler ObjectSelected;
        public event ObjectChangeEventHandler ObjectOnClick;
        public event ObjectOnDragEventHandler ObjectOnDrag;
        public event ObjectDoubleClickSelectedEventHandler ObjectSelectedDoubleClick;

        private ISmartNotification _notification = Locator.Current.GetService<ISmartNotification>();
        private readonly IDriverFactory _driverFactory = Locator.Current.GetService<IDriverFactory>();

        public SidebarElementDTO _element { get; set; }
        public BlueprintDTO _dtoElement { get; set; }

        private List<BlueprintElementDTO> _doElements = new List<BlueprintElementDTO>();
        private Image _blueprintImage { get; set; }
        private readonly LiveViewModel _viewModel;
        private bool _painted = false;
        private ChromiumWebBrowser _browser;

        public ElementBlueprintControl(SidebarElementDTO element, BlueprintDTO blueprint, LiveViewModel viewModel)
        {
            InitializeComponent();

            _element = element;
            _dtoElement = blueprint;
            _viewModel = viewModel;

            CefSettings settings = new CefSettings();
            if (!Cef.IsInitialized)
            {
                Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
            }
            this.Click += ElementBlueprintControl_Click;

            this.Paint += ElementBlueprintControl_Paint;
        }

        public void InitBrowser()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    InitBrowser();
                });
                return;
            }

            try
            {
                CefSharpSettings.LegacyJavascriptBindingEnabled = true;
                CefSharpSettings.ShutdownOnExit = false;
                string page = $@"{Application.StartupPath}\Resources\Html\location.html";
                _browser = new ChromiumWebBrowser(page)
                {
                    RequestContext = new RequestContext(),
                    BrowserSettings = new BrowserSettings
                    {
                        FileAccessFromFileUrls = CefState.Enabled,
                        UniversalAccessFromFileUrls = CefState.Enabled,
                        BackgroundColor = Cef.ColorSetARGB(255, 34, 34, 34)
                    },
                    MenuHandler = new CustomCefMenuHandler()
                };
                var bound = new BoundObject();

                //browser.RegisterJsObject("bound", bound);
                _browser.JavascriptObjectRepository.Register("bound", bound, true);
                bound.OnClickObject += Bound_OnClickObject;
                bound.OnClickDocument += Bound_OnClickDocument;
                bound.OnDrag += Bound_OnDrag;
                bound.OnDoubleClickObject += Bound_OnDoubleClickObject;
                _browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;
                _browser.LoadingStateChanged += Browser_LoadingStateChanged;
                _browser.AllowDrop = true;
                this.Controls.Add(_browser);
            }
            catch (Exception e)
            {
                _notification.Show(e.Message, null);
            }
        }

        private void ElementBlueprintControl_Paint(object sender, PaintEventArgs e)
        {
            if (this._painted)
                return;

            Threads.RunInOtherThread(new Action[]
            {
                () => InitBrowser()
            }, null);

            this._painted = true;
        }

        private void ElementBlueprintControl_Click(object sender, EventArgs e)
        {
            ObjectSelected(this, _element);
        }

        private void Bound_OnClickDocument(object sender)
        {
            ObjectSelected(this, _element);
        }

        private async void Bound_OnClickObject(object sender, string json)
        {
            BlueprintElementDTO blueprintElement = Newtonsoft.Json.JsonConvert.DeserializeObject<BlueprintElementDTO>(json);

            switch (blueprintElement.ObjectType)
            {
                case "DO":
                    ChangeDOElementStatus(blueprintElement.DeviceFeatureId);
                    break;
                case "VIN":
                case "MAP":
                case "KPI":
                case "OFFLINE":
                    await ShowCamera(blueprintElement);
                    break;
                default:
                    break;
            }
        }

        private async void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading && _browser?.IsDisposed == false)
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(_dtoElement);
                _browser?.GetMainFrame().ExecuteJavaScriptAsync($"addMarkers('{json}')");

                var elementsDo = _dtoElement.Elements.Where(x => x.ObjectType == "DO").ToList();
                _doElements.AddRange(elementsDo);

                _browser?.GetMainFrame().ExecuteJavaScriptAsync("showLoader()");

                var outputTasks = elementsDo.Select(item => GetOutputState(item.DeviceFeatureId)).ToList();
                await Task.WhenAll(outputTasks);
                var DOElementsSerialized = Newtonsoft.Json.JsonConvert.SerializeObject(_doElements);
                _browser?.GetMainFrame().ExecuteJavaScriptAsync($"changeDOCamerasStatus('{DOElementsSerialized}')");
                _browser?.GetMainFrame().ExecuteJavaScriptAsync("hideLoader()");
            }
        }

        private void Browser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            if (((ChromiumWebBrowser)sender).IsBrowserInitialized)
            {

            }
        }

        private void ElementBlueprintControl_Load(object sender, EventArgs e)
        {

        }

        private void Bound_OnDrag(object sender)
        {
            if (((Control)ObjectSelected.Target).Parent != null)
            {
                string panel = ((Control)ObjectSelected.Target).Parent.Name;
                ObjectOnDrag.Invoke(this, panel);
            }
        }
        private void Bound_OnDoubleClickObject(object sender, string json)
        {
            ObjectSelectedDoubleClick?.Invoke(this);
        }

        private async Task ShowCamera(BlueprintElementDTO blueprintElement)
        {
            object data = null;
            if (blueprintElement.ObjectType == "MAP")
            {
                data = blueprintElement;
            }
            else
            {
                data =await  _viewModel.GetCatalogCamera(_dtoElement.SiteId, blueprintElement.DeviceFeatureId);
            }

            if (data != null)
            {
                ObjectOnClick(this, _element, data);
            }
            else
            {
                _notification.Show(@"❌404🔴 - Camera not found", null);
            }
        }

        private async void ChangeDOElementStatus(int outputId)
        {
            var elementSelected = _doElements.FirstOrDefault(x => x.DeviceFeatureId == outputId);
            if (elementSelected != null)
            {
                var currentState = elementSelected.PortEstate;
                var newState = (currentState == IOPortState.Offline || currentState == IOPortState.Inactive) ? IOPortState.Active : IOPortState.Offline;

                _browser.GetMainFrame().ExecuteJavaScriptAsync("showLoader()");

                Previus_ToChangeState(elementSelected.DeviceFeatureId);

                await Element_ItemChangeState(elementSelected.DeviceFeatureId, newState);

                After_ToChangeState(elementSelected.DeviceFeatureId);

                _browser.GetMainFrame().ExecuteJavaScriptAsync("hideLoader()");
            }
        }

        private void Previus_ToChangeState(int outputId)
        {
            var itemToUpdate = _doElements.FirstOrDefault(x => x.DeviceFeatureId == outputId);
            itemToUpdate.PortEstate = IOPortState.Offline;
            var jsonDO_itemToUpdate = Newtonsoft.Json.JsonConvert.SerializeObject(itemToUpdate);
            _browser.GetMainFrame().ExecuteJavaScriptAsync($"changeDOElementStatus('{jsonDO_itemToUpdate}')");
        }

        private void After_ToChangeState(int outputId)
        {
            var updateItem = _doElements.FirstOrDefault(x => x.DeviceFeatureId == outputId);
            var jsonDO_updateItem = Newtonsoft.Json.JsonConvert.SerializeObject(updateItem);
            _browser.GetMainFrame().ExecuteJavaScriptAsync($"changeDOElementStatus('{jsonDO_updateItem}')");
        }


        private Task GetOutputState(int outputId)
        {
            return Task.Run(async () =>
            {
                CameraDTO device = await _viewModel.GetCamera(outputId);
                if (device != null)
                {
                    IManufactureUri driver = _driverFactory.GetDriverApiCgi(device);
                    var state = driver.OuputPortState();

                    // Actualizar el estado de la cámara en la interfaz de usuario
                    var item = _doElements.FirstOrDefault(x => x.DeviceFeatureId == outputId);
                    item.PortEstate = state;
                }
            });
        }

        private Task Element_ItemChangeState(int outputId, IOPortState newState)
        {
            return Task.Run(async () =>
            {
                CameraDTO device = await _viewModel.GetCamera(outputId);
                if (device != null)
                {
                    IManufactureUri driver = _driverFactory.GetDriverApiCgi(device);
                    driver.OuputPortChangeState(newState);
                    var updateState = driver.OuputPortState();

                    var item = _doElements.FirstOrDefault(x => x.DeviceFeatureId == outputId);
                    item.PortEstate = updateState;

                    _viewModel.SendNotificationIot(new IotDTO { active = updateState == IOPortState.Active });
                    _notification.Show(@"👷 - Output state changed to " + updateState.ToString(), null);
                }
            });
        }

    }
}
