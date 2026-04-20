using CefSharp;
using CefSharp.Handler;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.UserControls.Spinner;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.AccessControl
{
    public partial class AccessControlControl : UserControl
    {
        public AccessControlControl(string token, string url)
        {
            InitializeComponent();
            _ = InitializeBrowserAsync(url);
            this.Disposed += AccessControlControl_Disposed;
        }

        private async Task InitializeBrowserAsync(string url)
        {
            await RecreateAndNavigate(url);
        }

        public async void NavigateTo(string url)
        {
            var existingWebView = this.Controls.OfType<WebView2>().FirstOrDefault();
            if (existingWebView != null && !existingWebView.IsDisposed && existingWebView.CoreWebView2 != null)
            {
                LoadSpinner();
                EventHandler<CoreWebView2NavigationCompletedEventArgs> handler = null;
                handler = (s, e) =>
                {
                    existingWebView.NavigationCompleted -= handler;
                    RemoveSpinner();
                };
                existingWebView.NavigationCompleted += handler;
                existingWebView.CoreWebView2.Navigate(url);
            }
            else
            {
                await RecreateAndNavigate(url);
            }
        }

        private async Task RecreateAndNavigate(string url)
        {
            var oldWebViews = this.Controls.OfType<WebView2>().ToList();
            foreach (var old in oldWebViews)
            {
                this.Controls.Remove(old);
                old.Dispose();
            }

            var browser = await CreateBrowserAsync(url);
            browser.Source = new Uri(url, UriKind.Absolute);
            browser.Dock = DockStyle.Fill;
            this.Controls.Add(browser);
            browser.BringToFront(); ;
        }

        private void AccessControlControl_Disposed(object sender, EventArgs e)
        {
            var currentControls = this.Controls;
            foreach (Control item in currentControls)
            {
                item.Dispose();
                this.Controls.Remove(item);
            }

            this.Disposed -= AccessControlControl_Disposed;
        }

        /// <summary>
        /// Llega con la instancia del webview
        /// </summary>
        /// <param name="control"></param>
        private async Task<WebView2> CreateBrowserAsync(string url)
        {
            string instanceId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
            string cachePath = System.IO.Path.Combine(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
            var browserExecutablePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libraries", "WebView2.131.0.2903.86.x64");
            PathUtils.CleanOldWebView2Caches(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
            var environment = await CoreWebView2Environment.CreateAsync(browserExecutablePath, cachePath);

            var webView = new WebView2();
            await webView.EnsureCoreWebView2Async(environment);
            return webView;
        }

        private void LoadSpinner()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { LoadSpinner(); });
                return;
            }

            var existingSpinner = this.Controls.Find("Spinerloading", false);
            if (existingSpinner.Length > 0)
            {
                this.Controls.Remove(existingSpinner[0]);
                existingSpinner[0].Dispose();
            }

            FlowLayoutPanel dynamicFlowLayoutPanel = new FlowLayoutPanel
            {
                Location = new Point(0, 0),
                Name = "FlowLayoutPanel1",
                Size = this.Size,
                Dock = DockStyle.Fill,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                Margin = new Padding(10, 0, 0, 0),
            };
            dynamicFlowLayoutPanel.Scroll += (s, e) => { Application.DoEvents(); };

            var spinner = new SpinnerThreePoint(dynamicFlowLayoutPanel)
            {
                Name = "Spinerloading",
                Dock = DockStyle.Fill,
                Visible = true,
                Location = new Point(0, 0)
            };

            this.Controls.Add(spinner);
            spinner.BringToFront();
        }

        private void RemoveSpinner()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { RemoveSpinner(); });
                return;
            }

            var existingSpinner = this.Controls.Find("Spinerloading", true);
            foreach (var spinner in existingSpinner)
            {
                this.Controls.Remove(spinner);
                spinner.Dispose();
            }
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
