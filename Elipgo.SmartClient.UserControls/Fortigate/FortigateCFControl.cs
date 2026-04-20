using CefSharp;
using CefSharp.WinForms;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.UserControls.AccessControl;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Fortigate
{
    public partial class FortigateCFControl : UserControl
    {
        private ChromiumWebBrowser browser;
        private string UserToken;
        private readonly Configuration _config;

        public FortigateCFControl(string token)
        {
            UserToken = token;
            InitializeComponent();
            _config = SmartClientEnvironmentUtils.GetConfiguration();
            this.browser = CreateBrowser();
            this.Controls.Add(browser);
            browser.BringToFront();
            this.Disposed += CRMCFControl_Disposed;
        }

        private void CRMCFControl_Disposed(object sender, EventArgs e)
        {
            var currentControls = this.Controls;
            foreach (Control item in currentControls)
            {
                item.Dispose();
                this.Controls.Remove(item);
            }

            this.Disposed -= CRMCFControl_Disposed;
        }

        protected virtual ChromiumWebBrowser CreateBrowser()
        {
            CefSettings settings = new CefSettings()
            {
                IgnoreCertificateErrors = true
            };
            ChromiumWebBrowser chromiumWebBrowser = null;

            if (!Cef.IsInitialized)
            {
                Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
            }
            try
            {
                string page = string.Format(_config.AppSettings.Settings["FortigateUrl"].Value);
                chromiumWebBrowser = new ChromiumWebBrowser(page)
                {
                    RequestContext = new RequestContext(),
                    BrowserSettings = new BrowserSettings
                    {
                        FileAccessFromFileUrls = CefState.Enabled,
                        UniversalAccessFromFileUrls = CefState.Enabled,
                        BackgroundColor = Cef.ColorSetARGB(255, 34, 34, 34)
                    },
                    RequestHandler = new CustomRequesthandle(),
                    MenuHandler = new CustomCefMenuHandler()
                };
            }
            catch (Exception)
            {
            }
            return chromiumWebBrowser;
        }

    }
}
