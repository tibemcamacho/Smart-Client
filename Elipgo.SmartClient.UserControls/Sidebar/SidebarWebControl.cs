using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Sidebar
{
    public delegate void ObjectDoubleClickWebEventHandler(object sender, string element);
    public delegate void ObjectGroupDoubleClickWebEventHandler(object sender, string element);
    public delegate void ObjectOnClickTabBarWebEventHandler(object sender, bool show);
    public partial class SidebarWebControl : UserControl
    {
        public event ObjectDoubleClickWebEventHandler ObjectDoubleClick;
        public event ObjectGroupDoubleClickWebEventHandler ObjectGroupDoubleClick;
        public event ObjectOnClickTabBarWebEventHandler ObjectOnClickTabBar;

        public string Token { get; set; }
        public long UserId { get; set; }
        public Apps App { get; set; }

        private readonly Configuration _config;
        private bool _painted = false;
        private WebView2 _webViewBrowser;

        public SidebarWebControl()
        {
            InitializeComponent();
            _config = SmartClientEnvironmentUtils.GetConfiguration();
            this.Paint += SidebarWebControl_Paint;
        }

        private void SidebarWebControl_Paint(object sender, PaintEventArgs e)
        {
            if (_painted)
                return;
            _painted = true;

            // Creamos cargamos la instancia del WebView
            _ = InitializeBrowserAsync();
        }

        private async Task InitializeBrowserAsync()
        {
            var browser = await CreateBrowserAsync();
            browser.Dock = DockStyle.Fill;
            this.Controls.Add(browser);
            browser.BringToFront();
            _webViewBrowser = browser;
        }

        /// <summary>
        /// Llega con la instancia del WebView
        /// </summary>
        /// <param name="control"></param>
        private async Task<WebView2> CreateBrowserAsync()
        {
            var language = WorkFolderUtils.GetUserSettings("UserLanguage", true);
            var takeObj = WorkFolderUtils.GetUserSettings("takeObj", true);
            var verifyStatus = bool.TryParse(WorkFolderUtils.GetUserSettings("VerifyStatus", true), out bool preResult) && preResult;

            string pageUrl = $@"{_config.AppSettings.Settings["SidebarURL"].Value}?Token-Session={Token}&Apps={(int)App}&takeObj={takeObj}&language={language}&showStatus={(verifyStatus ? 1 : 0)}&UserId={UserId}";
            string instanceId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
            string cachePath = System.IO.Path.Combine(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
            var browserExecutablePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libraries", "WebView2.131.0.2903.86.x64");
            PathUtils.CleanOldWebView2Caches(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
            var environment = await CoreWebView2Environment.CreateAsync(browserExecutablePath, cachePath);

            var webView = new WebView2();
            await webView.EnsureCoreWebView2Async(environment);
            webView.Source = new Uri(pageUrl, UriKind.Absolute);

            var bridge = new BridgeObject();
            bridge.OnDoubleClickObject += Bound_OnDoubleClickObject;
            bridge.OnClickObject += Bound_OnDoubleClickGroupObject;
            bridge.OnClickTabBar += Bound_OnClickTabBar;
            webView.CoreWebView2.AddHostObjectToScript("bridge", bridge);

            return webView;
        }

        private void Bound_OnClickTabBar(object sender, bool show)
        {
            ObjectOnClickTabBar(sender, show);
        }

        private void Bound_OnDoubleClickObject(object sender, string json)
        {
            ObjectDoubleClick(sender, json);
        }
        private void Bound_OnDoubleClickGroupObject(object sender, string json)
        {
            ObjectGroupDoubleClick(sender, json);
        }

        public void ResetItem(int elementId)
        {
            _webViewBrowser.ExecuteScriptAsync($"funjsResetItem({elementId})");
        }
    }
}

