using CefSharp;
using CefSharp.WinForms;
using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Properties;
using Splat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.ElementContainer
{
    public partial class ElementAlarmGeomapControl : UserControl
    {
        private ISmartNotification _notification = Locator.Current.GetService<ISmartNotification>();
        public event ObjectSelectedEventHandler ObjectSelected;
        public event ObjectChangeEventHandler ObjectOnClick;
        public event ObjectOnDragEventHandler ObjectOnDrag;
        public event ObjectDoubleClickSelectedEventHandler ObjectSelectedDoubleClick;

        public SidebarElementDTO _element;
        public List<AlarmGeoMapDTO> _dtoElement;

        private ChromiumWebBrowser _browser;
        private BoundObject _bound;
        private volatile bool _disposed = false;

        public ElementAlarmGeomapControl(SidebarElementDTO element, List<AlarmGeoMapDTO> markers)
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND);

            _element = element;
            _dtoElement = markers;

            this.Click += ElementGMapControl_Click;
            InitBrowser();
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
                string page = string.Format(@"{0}\Resources\Html\gmapsAlarms.html", Application.StartupPath);
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

        private void ElementGMapControl_Click(object sender, EventArgs e)
        {
            if (_disposed) return;
            ObjectSelected?.Invoke(this, _element);
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
            // TODO: Implementar funcionalidad de clic en marcador de alarma.
            // Este código estaba comentado en la versión original.
            // Al habilitarlo, se dispararía ObjectOnClick cuando el usuario
            // hace clic en un marcador del mapa de alarmas.
            //
            // if (_disposed) return;
            // if (this.InvokeRequired)
            // {
            //     this.BeginInvoke((MethodInvoker)delegate { Bound_OnClickObject(sender, json); });
            //     return;
            // }
            // try
            // {
            //     AlarmGeoMapDTO data = Newtonsoft.Json.JsonConvert.DeserializeObject<AlarmGeoMapDTO>(json);
            //     ObjectOnClick?.Invoke(this, _element, data);
            // }
            // catch (Exception ex)
            // {
            //     Logger.Log($"ElementAlarmGeomapControl.Bound_OnClickObject error: {ex.Message}", LogPriority.Warning);
            // }
        }

        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (_disposed) return;

            if (!e.IsLoading && _browser != null && _browser.IsBrowserInitialized)
            {
                try
                {
                    var frame = _browser.GetMainFrame();
                    if (frame != null && frame.IsValid)
                    {
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(_dtoElement);
                        frame.ExecuteJavaScriptAsync(string.Format("addMarkers('{0}')", json));
                    }
                }
                catch (ObjectDisposedException)
                {
                    // Browser was disposed, ignore
                }
                catch (Exception ex)
                {
                    Logger.Log($"ElementAlarmGeomapControl.Browser_LoadingStateChanged error: {ex.Message}", LogPriority.Warning);
                }
            }
        }

        private void Browser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
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
                Logger.Log($"ElementAlarmGeomapControl.Bound_OnDrag error: {ex.Message}", LogPriority.Warning);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed) return;
            _disposed = true;

            if (disposing)
            {
                try
                {
                    // Desuscribir evento Click
                    this.Click -= ElementGMapControl_Click;

                    // Desuscribir eventos de BoundObject
                    if (_bound != null)
                    {
                        _bound.OnClickObject -= Bound_OnClickObject;
                        _bound.OnClickDocument -= Bound_OnClickDocument;
                        _bound.OnDrag -= Bound_OnDrag;
                        _bound = null;
                    }

                    // Desuscribir eventos del browser
                    if (_browser != null)
                    {
                        _browser.IsBrowserInitializedChanged -= Browser_IsBrowserInitializedChanged;
                        _browser.LoadingStateChanged -= Browser_LoadingStateChanged;

                        // Remover de Controls antes de disponer para evitar doble dispose
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

                    // Dispose de components del Designer
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"ElementAlarmGeomapControl.Dispose error: {ex.Message}", LogPriority.Warning);
                }
            }

            base.Dispose(disposing);
        }
    }
}
