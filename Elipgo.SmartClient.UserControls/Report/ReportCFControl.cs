using Elipgo.SmartClient.Common.Helpers;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Report
{
    public partial class ReportCFControl : UserControl
    {
        private string _userToken;
        private long _userEntity;
        private readonly Configuration _config;

        public ReportCFControl(string token, long entidad)
        {
            _userToken = token;
            _userEntity = entidad;
            InitializeComponent();
            _config = SmartClientEnvironmentUtils.GetConfiguration();
            _ = InitializeBrowserAsync();
        }

        private async Task InitializeBrowserAsync()
        {
            var browser = await CreateBrowserAsync();
            browser.Dock = DockStyle.Fill;
            this.Controls.Add(browser);
            browser.BringToFront();
        }

        /// <summary>
        /// Llega con la instancia del WebView
        /// </summary>
        /// <param name="control"></param>
        private async Task<WebView2> CreateBrowserAsync()
        {
            string pageUrl = $@"{_config.AppSettings.Settings["ReportURL"].Value}/?Token-Session={_userToken}&showMenuHeader=false";
            string instanceId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
            string cachePath = System.IO.Path.Combine(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
            var browserExecutablePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libraries", "WebView2.131.0.2903.86.x64");
            PathUtils.CleanOldWebView2Caches(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
            var environment = await CoreWebView2Environment.CreateAsync(browserExecutablePath, cachePath);

            var webView = new WebView2();
            await webView.EnsureCoreWebView2Async(environment);
            webView.Source = new Uri(pageUrl, UriKind.Absolute);
            return webView;
        }
    }
}
