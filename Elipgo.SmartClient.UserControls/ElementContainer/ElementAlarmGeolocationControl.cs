using CefSharp;
using CefSharp.WinForms;
using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.ViewModels;
using Splat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.ElementContainer
{
    public partial class ElementAlarmGeolocationControl : UserControl
    {
        private ISmartNotification _notification = Locator.Current.GetService<ISmartNotification>();
        public event ObjectSelectedEventHandler ObjectSelected;
        public event ObjectChangeEventHandler ObjectOnClick;
        public event ObjectOnDragEventHandler ObjectOnDrag;
        public event ObjectDoubleClickSelectedEventHandler ObjectSelectedDoubleClick;

        public SidebarElementDTO _element;
        public CardDto _dtoElement;

        private MapConfigDTO _mapConfig;
        private CardDto _card;
        protected LiveViewModel _viewModel;
        protected AlarmDTO _alarmDTO;

        private ChromiumWebBrowser _browser;
        private BoundObject _bound;
        private volatile bool _disposed = false;
        private volatile bool _isDataInitialized = false;
        private volatile bool _isBrowserReady = false;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public ElementAlarmGeolocationControl(SidebarElementDTO element, CardDto dto, LiveViewModel viewModel)
        {
            InitializeComponent();
            _element = element;
            _card = dto;
            _dtoElement = dto;
            _viewModel = viewModel;

            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND);
            InitBrowser();
            InitializeDataAsync(_cts.Token);
        }

        private async void InitializeDataAsync(CancellationToken ct)
        {
            try
            {
                if (_disposed || ct.IsCancellationRequested) return;

                // Primero obtener la alarma
                _alarmDTO = await _viewModel.GetAlarm(_card.IdAlarm);

                if (_disposed || ct.IsCancellationRequested || _alarmDTO == null) return;

                // Luego crear la configuración del mapa
                await CreateMapConfigAsync(_element.DeviceTypeStr);

                if (_disposed || ct.IsCancellationRequested) return;

                _isDataInitialized = true;

                // Si el browser ya está listo, ejecutar addMarkers
                TryExecuteAddMarkers();
            }
            catch (OperationCanceledException)
            {
                // Esperado al disponer el control durante la carga
            }
            catch (Exception ex)
            {
                Logger.Log($"ElementAlarmGeolocationControl.InitializeDataAsync error: {ex.Message}", LogPriority.Warning);
            }
        }

        public void InitBrowser()
        {
            try
            {
                CefSettings settings = new CefSettings();
                if (!Cef.IsInitialized)
                {
                    Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
                }
                CefSharpSettings.LegacyJavascriptBindingEnabled = true;
                string page = string.Format(@"{0}\Resources\Html\gmaps.html", Application.StartupPath);
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
                _bound = new BoundObject();
                _browser.JavascriptObjectRepository.Register("bound", _bound, true);
                _bound.OnClickObject += Bound_OnClickObject;
                _bound.OnClickDocument += Bound_OnClickDocument;
                _bound.OnDrag += Bound_OnDrag;
                _browser.LoadingStateChanged += Browser_LoadingStateChanged;
                _browser.AllowDrop = true;
                this.Controls.Add(_browser);
            }
            catch (Exception e)
            {
                _notification.Show(e.Message, null);
            }
        }

        private void Bound_OnClickDocument(object sender)
        {
            if (_disposed) return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Bound_OnClickDocument(sender);
                });
                return;
            }

            ObjectSelected?.Invoke(this, _element);
        }

        private void Bound_OnClickObject(object sender, string json)
        {
            // TODO: Implementar si se necesita funcionalidad al hacer clic en el marcador
        }

        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (_disposed) return;

            if (!e.IsLoading && _browser != null && _browser.IsBrowserInitialized)
            {
                _isBrowserReady = true;

                // Este evento se dispara desde un hilo de CEF (no-UI).
                // Marshalear al hilo UI para acceder a controles de forma segura.
                if (this.IsHandleCreated && !this.IsDisposed)
                {
                    try
                    {
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            TryExecuteAddMarkers();
                        });
                    }
                    catch (ObjectDisposedException)
                    {
                        // Control fue disposed entre el check y el BeginInvoke
                    }
                }
            }
        }

        private void TryExecuteAddMarkers()
        {
            if (_disposed || !_isBrowserReady || !_isDataInitialized || _mapConfig == null)
                return;

            try
            {
                if (_browser == null || _browser.IsDisposed || !_browser.IsBrowserInitialized)
                    return;

                var frame = _browser.GetMainFrame();
                if (frame != null && frame.IsValid)
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(new List<MapConfigDTO>() { _mapConfig });
                    frame.ExecuteJavaScriptAsync(string.Format("addMarkers('{0}')", json));
                }
            }
            catch (ObjectDisposedException)
            {
                // Browser was disposed, ignore
            }
            catch (Exception ex)
            {
                Logger.Log($"ElementAlarmGeolocationControl.TryExecuteAddMarkers error: {ex.Message}", LogPriority.Warning);
            }
        }

        private void Bound_OnDrag(object sender)
        {
            if (_disposed) return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Bound_OnDrag(sender);
                });
                return;
            }

            try
            {
                if (ObjectSelected?.Target is Control control && control.Parent != null)
                {
                    string panel = control.Parent.Name;
                    ObjectOnDrag?.Invoke(this, panel);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"ElementAlarmGeolocationControl.Bound_OnDrag error: {ex.Message}", LogPriority.Warning);
            }
        }

        private async Task CreateMapConfigAsync(string type)
        {
            double LocationLatitude = 0;
            double LocationLongitude = 0;

            if (!string.IsNullOrEmpty(_alarmDTO?.Latitude) && !string.IsNullOrEmpty(_alarmDTO?.Longitude))
            {
                LocationLatitude = Convert.ToDouble(_alarmDTO.Latitude, System.Globalization.CultureInfo.InvariantCulture);
                LocationLongitude = Convert.ToDouble(_alarmDTO.Longitude, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (_alarmDTO != null)
            {
                var locationChannel = await _viewModel.GetDeviceFeature(_alarmDTO.ObjectId);
                if (!string.IsNullOrEmpty(locationChannel?.Latitude) && !string.IsNullOrEmpty(locationChannel?.Longitude))
                {
                    LocationLatitude = Convert.ToDouble(locationChannel.Latitude, System.Globalization.CultureInfo.InvariantCulture);
                    LocationLongitude = Convert.ToDouble(locationChannel.Longitude, System.Globalization.CultureInfo.InvariantCulture);
                }
                else if (_alarmDTO.SiteId != 0)
                {
                    var sites = await _viewModel.GetGeomapCatalog(_alarmDTO.SiteId);
                    var location = sites?.FirstOrDefault(x => x.Id.Equals(_alarmDTO.SiteId));
                    if (location != null)
                    {
                        LocationLatitude = location.LocationLatitude;
                        LocationLongitude = location.LocationLongitude;
                    }
                }
            }

            _mapConfig = new MapConfigDTO()
            {
                Type = type,
                Name = "Alert",
                LocationLatitude = LocationLatitude,
                LocationLongitude = LocationLongitude
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;

            if (disposing)
            {
                // Cancelar operaciones async en curso
                try
                {
                    _cts?.Cancel();
                    _cts?.Dispose();
                    _cts = null;
                }
                catch (Exception ex)
                {
                    Logger.Log($"ElementAlarmGeolocationControl.Dispose CTS error: {ex.Message}", LogPriority.Warning);
                }

                // Desuscribir eventos de BoundObject
                try
                {
                    if (_bound != null)
                    {
                        _bound.OnClickObject -= Bound_OnClickObject;
                        _bound.OnClickDocument -= Bound_OnClickDocument;
                        _bound.OnDrag -= Bound_OnDrag;
                        _bound = null;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"ElementAlarmGeolocationControl.Dispose BoundObject error: {ex.Message}", LogPriority.Warning);
                }

                // Desuscribir eventos y disponer el browser
                try
                {
                    if (_browser != null)
                    {
                        _browser.LoadingStateChanged -= Browser_LoadingStateChanged;

                        if (this.Controls.Contains(_browser))
                        {
                            this.Controls.Remove(_browser);
                        }

                        if (!_browser.IsDisposed)
                        {
                            _browser.Dispose();
                        }
                        _browser = null;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"ElementAlarmGeolocationControl.Dispose Browser error: {ex.Message}", LogPriority.Warning);
                }

                // Limpiar eventos públicos para evitar leaks de suscriptores
                ObjectSelected = null;
                ObjectOnClick = null;
                ObjectOnDrag = null;
                ObjectSelectedDoubleClick = null;

                // Dispose de components del Designer
                try
                {
                    components?.Dispose();
                }
                catch (Exception ex)
                {
                    Logger.Log($"ElementAlarmGeolocationControl.Dispose Components error: {ex.Message}", LogPriority.Warning);
                }
            }

            base.Dispose(disposing);
        }
    }
}
