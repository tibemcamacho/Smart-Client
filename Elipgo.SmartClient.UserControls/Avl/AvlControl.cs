using CefSharp;
using CefSharp.Handler;
using CefSharp.WinForms;
using Elipgo.SmartClient.Common.Helpers;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Avl
{
    public partial class AvlControl : UserControl
    {
        private ChromiumWebBrowser browser;
        private string UserToken;
        private readonly Configuration config;

        public AvlControl(string token, string url)
        {
            UserToken = token;
            InitializeComponent();

            config = SmartClientEnvironmentUtils.GetConfiguration();

            // Creamos cargamos la instancia del Chromium
            this.browser = CreateBrowser(url);

            // Agregamos la instancia al Panel
            AddElementToPanel(this.browser);
            this.Disposed += AvlControl_Disposed;
        }

        private void AvlControl_Disposed(object sender, EventArgs e)
        {
            var currentControls = this.Controls;
            foreach (Control item in currentControls)
            {
                item.Dispose();
                this.Controls.Remove(item);
            }

            this.Disposed -= AvlControl_Disposed;
        }

        protected virtual ChromiumWebBrowser CreateBrowser(string url)
        {
            CefSettings settings = new CefSettings()
            {
                RemoteDebuggingPort = 8088//,
                //IgnoreCertificateErrors = true
            };
            ChromiumWebBrowser chromiumWebBrowser;

            if (!Cef.IsInitialized)
            {
                Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
            }
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            //string page = "http://www.google.com/"; 
            // string.Format(@"{0}", ConfigurationManager.AppSettings["SupremaUrl"]); // TOMAR ACA URL DEL CONFIG
            string page = config.AppSettings.Settings["AvlURL"].Value;
            chromiumWebBrowser = new ChromiumWebBrowser(page)
            {
                RequestContext = new RequestContext(),
                BrowserSettings = new BrowserSettings
                {
                    FileAccessFromFileUrls = CefState.Enabled,
                    UniversalAccessFromFileUrls = CefState.Enabled,
                    BackgroundColor = Cef.ColorSetARGB(255, 34, 34, 34),


                },
                RequestHandler = new CustomRequesthandle(),
                MenuHandler = new CustomCefMenuHandler()
            };

            chromiumWebBrowser.LifeSpanHandler = new MyCustomLifeSpanHandler();
            //chromiumWebBrowser.LifeSpanHandler.OpenNewTab += (sender, e) =>
            //{
            //    // Cancel opening in new tab
            //    e.Cancel = true;

            //    // Open the URL in the current tab
            //    browser.Load(e.TargetUrl);
            //};

            //chromiumWebBrowser.FrameLoadEnd += (sender, e) =>
            //{
            //    if (e.Frame.IsMain)
            //    {
            //        browser.EvaluateScriptAsync(@"
            //            var links = document.getElementsByTagName('a');
            //            for (var i = 0; i < links.length; i++) {
            //                var link = links[i];
            //                if (link.target === '_blank') {
            //                    link.target = '_self';
            //                }
            //            }
            //        ");
            //    }
            //};

            return chromiumWebBrowser;
        }


        /// <summary>
        /// Llega con la instancia del Chromium
        /// </summary>
        /// <param name="control"></param>
        protected virtual void AddElementToPanel(Control control)
        {
            this.Controls.Add(control);
            control.BringToFront();
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

    public class MyCustomLifeSpanHandler : ILifeSpanHandler
    {
        // Load new URL (when clicking a link with target=_blank) in the same frame
        public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            browser.MainFrame.LoadUrl(targetUrl);
            newBrowser = null;
            return true;
        }

        // If you don't implement all of the interface members in the custom class
        // you will find:
        // Error CS0535	'MyCustomLifeSpanHandler' does not implement interface member 'ILifeSpanHandler.OnAfterCreated(IWebBrowser, IBrowser)'
        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            // throw new NotImplementedException();
            return true;
        }

        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            // throw new NotImplementedException();
        }

        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            // throw new NotImplementedException();
        }
    }
}
