using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Services.Services.Interface;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Splat;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.LprCF
{
    public delegate void OnGotoPreviewEvent(object sender, string json);
    public partial class LprCFControl : UserControl
    {
        public event OnGotoPreviewEvent GotoPreview;

        private string _userToken;
        private readonly Configuration _config;
        //private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();

        public LprCFControl(string token)
        {
            _userToken = token;
            InitializeComponent();
            _config = SmartClientEnvironmentUtils.GetConfiguration();

            // Creamos cargamos la instancia del WebView
            _ = InitializeBrowserAsync();
            this.Disposed += LprCFControl_Disposed;
        }

        private void LprCFControl_Disposed(object sender, System.EventArgs e)
        {
            var currentControls = this.Controls;
            foreach (Control item in currentControls)
            {
                item.Dispose();
                this.Controls.Remove(item);
            }

            this.Disposed -= LprCFControl_Disposed;
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
            string pageUrl = $@"{_config.AppSettings.Settings["LprUrl"].Value}?Token-Session={_userToken}";
            string instanceId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
            string cachePath = System.IO.Path.Combine(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
            var browserExecutablePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libraries", "WebView2.131.0.2903.86.x64");
            PathUtils.CleanOldWebView2Caches(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
            var environment = await CoreWebView2Environment.CreateAsync(browserExecutablePath, cachePath);

            var webView = new WebView2();
            await webView.EnsureCoreWebView2Async(environment);
            webView.Source = new Uri(pageUrl, UriKind.Absolute);

            //webView.WebMessageReceived += WebView_WebMessageReceived;
            return webView;
        }

        /// <summary>
        /// Llega con la instancia del Chromium
        /// </summary>
        /// <param name="control"></param>
        protected virtual void AddElementToPanel(Control control)
        {
            control.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.Controls.Add(control);
            control.BringToFront();
        }
    }
}
