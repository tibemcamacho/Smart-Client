using CefSharp;
using CefSharp.WinForms;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Services.Services.Interface;
using Splat;
using System.Configuration;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.InvoiceCFControl
{
    public partial class InvoiceCFControl : UserControl
    {
        private ChromiumWebBrowser browser;
        private string UserToken;
        private CatalogDTO UserCatalog;
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();

        private readonly Configuration config;

        public InvoiceCFControl(string token, CatalogDTO catalog)
        {
            UserToken = token;
            UserCatalog = catalog;
            InitializeComponent();

            config = SmartClientEnvironmentUtils.GetConfiguration();

            // Creamos cargamos la instancia del Chromium
            this.browser = CreateBrowser();

            // Agregamos la instancia al Panel
            AddElementToPanel(this.browser);
            this.Disposed += InvoiceCFControl_Disposed;
        }

        private void InvoiceCFControl_Disposed(object sender, System.EventArgs e)
        {
            var currentControls = this.Controls;
            foreach (Control item in currentControls)
            {
                item.Dispose();
                this.Controls.Remove(item);
            }

            this.Disposed -= InvoiceCFControl_Disposed;
        }

        protected virtual ChromiumWebBrowser CreateBrowser()
        {
            CefSettings settings = new CefSettings();
            settings.RemoteDebuggingPort = 8088;
            ChromiumWebBrowser chromiumWebBrowser;

            if (!Cef.IsInitialized)
            {
                Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
            }

            string page = string.Format(@"{0}index.html#/load?Token-Session={1}&Lang=es&backendUrl=" + UserCatalog.InvoiceModule.ApiUrl, config.AppSettings.Settings["InvoiceUrl"].Value, UserToken);
            chromiumWebBrowser = new ChromiumWebBrowser(page)
            {
                RequestContext = new RequestContext(),
                BrowserSettings = new BrowserSettings
                {
                    FileAccessFromFileUrls = CefState.Enabled,
                    UniversalAccessFromFileUrls = CefState.Enabled,
                    BackgroundColor = Cef.ColorSetARGB(255, 34, 34, 34)
                },
                MenuHandler = new CustomCefMenuHandler(),
                DownloadHandler = new CustomCefDownloadHandler()
            };

            return chromiumWebBrowser;
        }

        /// <summary>
        /// Llega con la instancia del Chromium
        /// </summary>
        /// <param name="control"></param>
        protected virtual void AddElementToPanel(Control control)
        {
            this.Controls.Add(control);
            control.BringToFront();
        }
    }
}
