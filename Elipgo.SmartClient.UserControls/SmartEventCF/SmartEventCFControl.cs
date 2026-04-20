using CefSharp;
using CefSharp.WinForms;
using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.Helpers;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Splat;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.SmartEventCF
{
    public partial class SmartEventCFControl : UserControl
    {
        private WebView2 _browser;
        private string _userToken;
        private readonly Configuration _config;
        //private readonly ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();

        public SmartEventCFControl(string token)
        {
            _userToken = token;
            InitializeComponent();
            _config = SmartClientEnvironmentUtils.GetConfiguration();

            this._browser = CreateBrowser();

            AddElementToPanel(this._browser);

            this.Disposed += SmartEventCFControl_Disposed;
        }

        private void SmartEventCFControl_Disposed(object sender, EventArgs e)
        {
            foreach (Control item in this.Controls)
            {
                item.Dispose();
            }

            this.Controls.Clear();
            this.Disposed -= SmartEventCFControl_Disposed;
        }

        protected virtual WebView2 CreateBrowser()
        {
            var webView = new WebView2
            {
                Dock = DockStyle.Fill
            };

            InitializeAsync(webView);

            return webView;
        }

        private async void InitializeAsync(WebView2 webView)
        {
            var env = await CoreWebView2Environment.CreateAsync(
                null,
                @"cache", // cache local (importante)
                new CoreWebView2EnvironmentOptions()
                {
                    AdditionalBrowserArguments = "--disable-web-security --allow-running-insecure-content"
                });

            await webView.EnsureCoreWebView2Async(env);

            // 🔥 USER AGENT (opcional)
            webView.CoreWebView2.Settings.UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                "(KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";

            // 🔥 Habilitar cosas útiles
            webView.CoreWebView2.Settings.IsScriptEnabled = true;
            webView.CoreWebView2.Settings.AreDevToolsEnabled = true;

            string page = _config.AppSettings.Settings["SmartEventUrl"].Value;

            webView.CoreWebView2.Navigate(page);
        }

        protected virtual void AddElementToPanel(Control control)
        {
            this.Controls.Add(control);
            control.BringToFront();
        }
    }
}
