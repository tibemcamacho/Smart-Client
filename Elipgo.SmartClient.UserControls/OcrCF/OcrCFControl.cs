using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.Helpers;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Splat;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.OcrCF
{
    public delegate void OnGotoPreviewEvent(object sender, string json);

    public partial class OcrCFControl : UserControl
    {
        public event OnGotoPreviewEvent GotoPreview;

        private string _userToken;
        private readonly Configuration _config;
        private readonly ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int SetForegroundWindow(IntPtr points);

        public OcrCFControl(string token)
        {
            _userToken = token;
            InitializeComponent();
            _config = SmartClientEnvironmentUtils.GetConfiguration();

            // Creamos cargamos la instancia del WebView
            _ = InitializeBrowserAsync();
            this.Disposed += OcrCFControl_Disposed;
        }

        private void OcrCFControl_Disposed(object sender, EventArgs e)
        {
            var currentControls = this.Controls;
            foreach (Control item in currentControls)
            {
                item.Dispose();
                this.Controls.Remove(item);
            }

            this.Disposed -= OcrCFControl_Disposed;
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
            string pageUrl = $@"{_config.AppSettings.Settings["OcrUrl"].Value}?Token-Session={_userToken}";
            string instanceId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
            string cachePath = System.IO.Path.Combine(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
            var browserExecutablePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libraries", "WebView2.131.0.2903.86.x64");
            PathUtils.CleanOldWebView2Caches(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
            var environment = await CoreWebView2Environment.CreateAsync(browserExecutablePath, cachePath);

            var webView = new WebView2();
            await webView.EnsureCoreWebView2Async(environment);
            webView.Source = new Uri(pageUrl, UriKind.Absolute);

            webView.WebMessageReceived += WebView_WebMessageReceived;
            return webView;
        }

        private void WebView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            var path = e.TryGetWebMessageAsString();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            LoadForegroundWindow(path);
        }

        private void LoadForegroundWindow(string path)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    LoadForegroundWindow(path);
                });
                return;
            }

            try
            {
                var winHandle = FindWindow("TkTopLevel", "UI");
                if (winHandle != IntPtr.Zero)
                {
                    SetForegroundWindow(winHandle);
                }
                else
                {
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = path;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.Verb = "runas";
                        process.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"OCR Operation Fail, No se puede iniciar OCR App Error: {ex.Message}", LogPriority.Information);
                notification.Show($"No se puede inciar OCR App", null);
            }
        }
    }
}
