using Elipgo.SmartClient.Common.Helpers;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.ClientManager
{
    public partial class ClientManagerControl : UserControl
    {
        private string _userToken;
        private Configuration _config;
        private string _clientManagerUrl;

        public ClientManagerControl(string token)
        {
            _userToken = token;
            _config = SmartClientEnvironmentUtils.GetConfiguration();
            _clientManagerUrl = _config.AppSettings.Settings["ClientManagerUrl"].Value;
            InitializeComponent();

            _ = InitializeBrowserAsync();
            this.Disposed += ClientManagerControl_Disposed;
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
            //var temp = GetTokenAsync();
            string pageUrl = $"{_clientManagerUrl}?lang=ES";
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

        private void ClientManagerControl_Disposed(object sender, EventArgs e)
        {
            var currentControls = this.Controls;
            foreach (Control item in currentControls)
            {
                item.Dispose();
                this.Controls.Remove(item);
            }

            this.Disposed -= ClientManagerControl_Disposed;
        }

        //public TokenResponse GetTokenAsync()
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        var disco = httpClient.GetDiscoveryDocumentAsync(_authority).Result;
        //        if (disco != null)
        //        {
        //            var tokenResponse = httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
        //            {
        //                Address = disco.TokenEndpoint,
        //                ClientId = "ClientManager",
        //                ClientSecret = _clientSecret,
        //                Scope = "IdentityServerApi"
        //            });
        //            var token = tokenResponse.Result;
        //            return token;
        //        }
        //    }
        //    return null;
        //}
    }
}