using CefSharp;
using CefSharp.WinForms;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Services.Services.Interface;
using Splat;
using System.Configuration;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.SecondViewCFControl
{
    public partial class SecondViewCFControl : UserControl
    {
        private ChromiumWebBrowser browser;
        private string UserToken;
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();

        private readonly Configuration config;

        public SecondViewCFControl(string token)
        {
            UserToken = token;
            InitializeComponent();

            config = SmartClientEnvironmentUtils.GetConfiguration();

            // Creamos cargamos la instancia del Chromium
            this.browser = CreateBrowser();

            // Agregamos la instancia al Panel
            AddElementToPanel(this.browser);
            this.Disposed += SecondViewCFControl_Disposed;
        }

        private void SecondViewCFControl_Disposed(object sender, System.EventArgs e)
        {
            var currentControls = this.Controls;
            foreach (Control item in currentControls)
            {
                item.Dispose();
                this.Controls.Remove(item);
            }

            this.Disposed -= SecondViewCFControl_Disposed;
        }

        protected virtual ChromiumWebBrowser CreateBrowser()
        {
            CefSettings settings = new CefSettings();
            ChromiumWebBrowser chromiumWebBrowser;

            if (!Cef.IsInitialized)
            {
                Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
            }

            string page = string.Format(@"https://vms-dev.vmonitoring.com/secondview/#/?Header=true&Token-Session={1}&Lang=es", config.AppSettings.Settings["SecondUrl"].Value, UserToken);

            chromiumWebBrowser = new ChromiumWebBrowser(page)
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
