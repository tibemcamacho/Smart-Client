using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Services.Services.Interface;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Splat;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.RetailCFControl
{
    public partial class RetailCFControl : UserControl
    {
        private string _userToken;
        private IAppAuthorization _appAuthorization = Locator.Current.GetService<IAppAuthorization>();

        private readonly Configuration _config;
        private UserDTO _currentUser;
        private string _retailUrl;
        private RetailVersion _retailVersion = RetailVersion.V1;

        public RetailCFControl(string token, UserDTO user)
        {
            _userToken = token;
            _currentUser = user;
            _config = SmartClientEnvironmentUtils.GetConfiguration();
            _retailUrl = _config.AppSettings.Settings["RetailUrl"].Value;
            if (!Enum.TryParse(_config.AppSettings.Settings["RetailVersion"].Value, out _retailVersion))
            {
                _retailVersion = RetailVersion.V1;
            }
            InitializeComponent();

            _ = InitializeBrowserAsync();
            this.Disposed += RetailCFControl_Disposed;
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

            bool seeReport = false;
            bool seeSetting = false;
            bool seeTools = false;
            if (_appAuthorization.Exist("auth.app.apps.retail.reports"))
            {
                seeReport = true;
            }
            if (_appAuthorization.Exist("auth.app.apps.retail.settings"))
            {
                seeSetting = true;
            }
            if (_appAuthorization.Exist("auth.app.apps.retail.tools"))
            {
                seeTools = true;
            }
           
            string pageUrl = string.Empty;
            if (_retailVersion == RetailVersion.V1)
            {
                //string page = string.Format(@"{0}index.html#/load?Token-Session={1}&Lang=es" +
                //     "&P1=true" +
                //     "&P2=" + seeReport.ToString().ToLower() +
                //     "&P3=true" +
                //     "&P4=" + seeSetting.ToString().ToLower() +
                //     "&P5=" + seeTools.ToString().ToLower(), config.AppSettings.Settings["RetailUrl"].Value, UserToken);
                pageUrl = $@"{_retailUrl}index.html#/load?Token-Session={_userToken}&Lang=es" +
                        $@"&P1=true&P2={seeReport.ToString().ToLower()}&P3=true&P4={seeSetting.ToString().ToLower()}&P5={seeTools.ToString().ToLower()}";
            }
            else
            {
                pageUrl = $@"{_retailUrl}";
            }

            string instanceId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
            string cachePath = System.IO.Path.Combine(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
            var browserExecutablePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libraries", "WebView2.131.0.2903.86.x64");
            PathUtils.CleanOldWebView2Caches(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
            var environment = await CoreWebView2Environment.CreateAsync(browserExecutablePath, cachePath);

            var webView = new WebView2();
            await webView.EnsureCoreWebView2Async(environment);
            webView.Source = new Uri(pageUrl, UriKind.Absolute);

            if (_retailVersion == RetailVersion.V2)
            {
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
            }
            return webView;
        }

        private void RetailCFControl_Disposed(object sender, System.EventArgs e)
        {
            var currentControls = this.Controls;
            foreach (Control item in currentControls)
            {
                item.Dispose();
                this.Controls.Remove(item);
            }

            this.Disposed -= RetailCFControl_Disposed;
        }
    }
}
