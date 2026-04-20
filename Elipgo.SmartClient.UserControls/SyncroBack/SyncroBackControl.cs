using CefSharp;
using CefSharp.Handler;
using Elipgo.SmartClient.Common.Helpers;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.SyncroBack
{
    public partial class SyncroBackControl : UserControl
    {
        private string _userToken;
        private readonly Configuration _config;

        public SyncroBackControl(string token)
        {
            _userToken = token;
            InitializeComponent();
            _config = SmartClientEnvironmentUtils.GetConfiguration();
            
            _ = InitializeBrowserAsync();
            this.Disposed += SyncroBackControl_Disposed;
        }

        private void SyncroBackControl_Disposed(object sender, EventArgs e)
        {
            var currentControls = this.Controls;
            foreach (Control item in currentControls)
            {
                item.Dispose();
                this.Controls.Remove(item);
            }

            this.Disposed -= SyncroBackControl_Disposed;
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
            string pageUrl = $@"{_config.AppSettings.Settings["SyncroBackUrl"].Value}#/?Token-Session={_userToken}&Lang=es";
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

    public class CustomRequesthandle : RequestHandler
    {
        public event EventHandler<OnCertificateErrorEventArgs> OnCertificateErrorEvent;

        protected override bool OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
        {
            var args = new OnCertificateErrorEventArgs(chromiumWebBrowser, browser, errorCode, requestUrl, sslInfo, callback);

            OnCertificateErrorEvent?.Invoke(this, args);

            callback.Continue(true);
            return args.ContinueAsync;
        }
    }

    public class OnCertificateErrorEventArgs : BaseRequestEventArgs
    {
        public OnCertificateErrorEventArgs(IWebBrowser chromiumWebBrowser, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
            : base(chromiumWebBrowser, browser)
        {
            ErrorCode = errorCode;
            RequestUrl = requestUrl;
            SSLInfo = sslInfo;
            Callback = callback;

            ContinueAsync = false; // default
        }

        public CefErrorCode ErrorCode { get; private set; }
        public string RequestUrl { get; private set; }
        public ISslInfo SSLInfo { get; private set; }

        /// <summary>
        ///     Callback interface used for asynchronous continuation of url requests.
        ///     If empty the error cannot be recovered from and the request will be canceled automatically.
        /// </summary>
        public IRequestCallback Callback { get; private set; }

        /// <summary>
        ///     Set to false to cancel the request immediately. Set to true and use <see cref="T:CefSharp.IRequestCallback" /> to
        ///     execute in an async fashion.
        /// </summary>
        public bool ContinueAsync { get; set; }
    }

    public abstract class BaseRequestEventArgs : System.EventArgs
    {
        protected BaseRequestEventArgs(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            ChromiumWebBrowser = chromiumWebBrowser;
            Browser = browser;
        }

        public IWebBrowser ChromiumWebBrowser { get; private set; }
        public IBrowser Browser { get; private set; }
    }
}