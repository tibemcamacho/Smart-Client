using CefSharp;
using CefSharp.WinForms;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Services.Services.Interface;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Splat;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.FaceCF
{
    public partial class FaceCFControl : UserControl
    {

        private ChromiumWebBrowser browser;
        private string _userToken;
        private long _userEntity;
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();

        private readonly Configuration _config;

        public FaceCFControl(string token, long entidad)
        {
            _userToken = token;
            _userEntity = entidad;
            InitializeComponent();

            _config = SmartClientEnvironmentUtils.GetConfiguration();

            // Creamos cargamos la instancia del Chromium
            //this.browser = CreateBrowser();

            //// Agregamos la instancia al Panel
            //AddElementToPanel(this.browser);

            // Creamos cargamos la instancia del WebView
            _ = InitializeBrowserAsync();
            this.Disposed += FaceCFControl_Disposed;
        }

        private void FaceCFControl_Disposed(object sender, System.EventArgs e)
        {
            var currentControls = this.Controls;
            foreach (Control item in currentControls)
            {
                item.Dispose();
                this.Controls.Remove(item);
            }

            this.Disposed -= FaceCFControl_Disposed;
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
            bool seeLists = false;
            bool modifyLists = false;
            bool seeFace = false;
            bool modifyFace = false;
            if (appAuthorization.Exist("auth.app.apps.face.seeLists"))
            {
                seeLists = true;
            }
            if (appAuthorization.Exist("auth.app.apps.face.modifyLists"))
            {
                modifyLists = true;
            }
            if (appAuthorization.Exist("auth.app.apps.face.seeFace"))
            {
                seeFace = true;
            }
            if (appAuthorization.Exist("auth.app.apps.face.modifyFace"))
            {
                modifyFace = true;
            }

            string pageUrl = $@"{_config.AppSettings.Settings["FaceUrl"].Value}?Token-Session={_userToken}&Lang=es" +
                "&P1=" + seeLists.ToString().ToLower() +
                "&P2=" + modifyLists.ToString().ToLower() +
                "&P3=" + seeFace.ToString().ToLower() +
                "&P4=" + modifyFace.ToString().ToLower() +
                "&P5=true&VcaId={2}&showMenuHeader=false";

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
        /// Previus instance for Chromium
        /// </summary>
        /// <param name="control"></param>
        protected virtual ChromiumWebBrowser CreateBrowser()
        {
            CefSettings settings = new CefSettings()
            {
                RemoteDebuggingPort = 8005
            };

            settings.CefCommandLineArgs.Add("timezone", "America/Mexico_City");
            settings.CefCommandLineArgs.Add("enable-media-stream", "1");

            ChromiumWebBrowser chromiumWebBrowser;

            if (!Cef.IsInitialized)
            {
                Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
            }
            bool seeLists = false;
            bool modifyLists = false;
            bool seeFace = false;
            bool modifyFace = false;
            if (appAuthorization.Exist("auth.app.apps.face.seeLists"))
            {
                seeLists = true;
            }
            if (appAuthorization.Exist("auth.app.apps.face.modifyLists"))
            {
                modifyLists = true;
            }
            if (appAuthorization.Exist("auth.app.apps.face.seeFace"))
            {
                seeFace = true;
            }
            if (appAuthorization.Exist("auth.app.apps.face.modifyFace"))
            {
                modifyFace = true;
            }
            string page = string.Format(@"{0}index.html#/load?Token-Session={1}&Lang=es" +
                "&P1=" + seeLists.ToString().ToLower() +
                "&P2=" + modifyLists.ToString().ToLower() +
                "&P3=" + seeFace.ToString().ToLower() +
                "&P4=" + modifyFace.ToString().ToLower() +
                "&P5=true&VcaId={2}&showMenuHeader=false", _config.AppSettings.Settings["FaceUrl"].Value, _userToken, _userEntity);

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
            control.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.Controls.Add(control);
            control.BringToFront();
        }
    }
}
