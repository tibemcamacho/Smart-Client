using CefSharp;
using CefSharp.WinForms;
using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Splat;
using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.ElementContainer
{
    public partial class ElementKPIControl : UserControl, IDisposable
    {
        public event ObjectSelectedEventHandler ObjectSelected;
        public event ObjectOnDragEventHandler ObjectOnDrag;
        public event ObjectDoubleClickSelectedEventHandler ObjectSelectedDoubleClick;

        private ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        private readonly Configuration config;

        private SidebarElementDTO Element { get; set; }
        private KpiDTO Kpi { get; set; }
        public SidebarElementDTO _element;
        public KpiDTO _dtoElement;
        public ElementKPIControl(SidebarElementDTO element, KpiDTO kpi)
        {
            Element = element;
            Kpi = kpi;
            _element = element;
            _dtoElement = kpi;

            InitializeComponent();

            config = SmartClientEnvironmentUtils.GetConfiguration();
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND);
            this.Click += ElementKPIControl_Click;

            InitBrowser();
        }

        private void ElementKPIControl_Click(object sender, EventArgs e)
        {
            ObjectSelected(this, Element);
        }

        private ChromiumWebBrowser browser;
        public void InitBrowser()
        {
            try
            {
                CefSettings settings = new CefSettings() { RemoteDebuggingPort = 8088 };
                if (!Cef.IsInitialized)
                {
                    Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
                }
                CefSharpSettings.LegacyJavascriptBindingEnabled = true;
                CefSharpSettings.ShutdownOnExit = false;
                string page = string.Format(@"{0}index.card.html#/load?Token-Session={1}&sitId={2}&kpiId={3}&kpiType={4}&Lang=es", config.AppSettings.Settings["RetailUrl"].Value, Kpi.Token, Kpi.SiteId, Kpi.ObjectId, Kpi.Type);
                browser = new ChromiumWebBrowser(page)
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
                browser.JavascriptObjectRepository.Register("bound", bound, true);
                bound.OnClickDocument += Bound_OnClickDocument;
                bound.OnDrag += Bound_OnDrag;
                bound.OnDoubleClickObject += Bound_OnDoubleClickObject;
                browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;
                browser.LoadingStateChanged += Browser_LoadingStateChanged;
                browser.AllowDrop = true;
                this.Controls.Add(browser);
            }
            catch (Exception e)
            {
                notification.Show(e.Message, null);
            }
        }

        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {

            }
        }

        private void Browser_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            if (((ChromiumWebBrowser)sender).IsBrowserInitialized)
            {
            }
        }

        private void Bound_OnClickDocument(object sender)
        {
            ObjectSelected(this, Element);
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

        public new void Dispose()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Dispose();
                });
                return;
            }
            base.Dispose(true);
        }

    }
}
