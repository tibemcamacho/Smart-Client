using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Services.Services.Interface;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Splat;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.CAD
{
    public partial class CADControl : UserControl
    {
        private string _userToken;
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();

        private readonly Configuration config;

        public CADControl(string token)
        {
            _userToken = token;
            config = SmartClientEnvironmentUtils.GetConfiguration();

            InitializeComponent();

            _ = InitializeBrowserAsync();
            this.Disposed += CADControl_Disposed;
        }

        private async Task InitializeBrowserAsync()
        {
            var browser = await CreateBrowserAsync();
            browser.Dock = DockStyle.Fill;
            this.Controls.Add(browser);
            browser.BringToFront();
        }

        /// <summary>
        /// Llega con la instancia del webview
        /// </summary>
        /// <param name="control"></param>
        private async Task<WebView2> CreateBrowserAsync()
        {
            string pageUrl = $@"{config.AppSettings.Settings["CADUrl"].Value}?Token-Session={_userToken}&Lang=es";
            string instanceId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
            string cachePath = System.IO.Path.Combine(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
            var browserExecutablePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libraries", "WebView2.131.0.2903.86.x64");
            PathUtils.CleanOldWebView2Caches(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
            var environment = await CoreWebView2Environment.CreateAsync(browserExecutablePath, cachePath);

            var webView = new WebView2();
            await webView.EnsureCoreWebView2Async(environment);
            webView.Source = new Uri(pageUrl, UriKind.Absolute);
            webView.CoreWebView2.NavigationCompleted += async (sender, args) =>
            {
                if (args.IsSuccess)
                {
                    // Polling: espera hasta que Angular haya establecido appIsReady = true
                    for (int i = 0; i < 10; i++)
                    {
                        var result = await webView.CoreWebView2.ExecuteScriptAsync("window.appIsReady === true");
                        if (result == "true")
                        {
                            string messageJson = $"{{ \"type\": \"token\", \"accessToken\": \"{_userToken}\" }}";
                            webView.CoreWebView2.PostWebMessageAsJson(messageJson);
                            break;
                        }

                        await Task.Delay(100);
                    }
                }
            };
            return webView;
        }

        private void CADControl_Disposed(object sender, System.EventArgs e)
        {
            var currentControls = this.Controls;
            foreach (Control item in currentControls)
            {
                item.Dispose();
                this.Controls.Remove(item);
            }

            this.Disposed -= CADControl_Disposed;
        }
    }
}
