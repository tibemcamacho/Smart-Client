using CefSharp;
using CefSharp.WinForms;
using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Services.Services.Interface;
using Splat;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.DACCF
{
    public partial class DACCFControl : UserControl
    {
        private ChromiumWebBrowser browser;
        private string UserToken;
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        private readonly Configuration config;

        //[DllImport("user32.dll", SetLastError = true)]
        //static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        //[DllImport("User32.dll", CharSet = CharSet.Auto)]
        //private static extern int SetForegroundWindow(IntPtr points);

        public DACCFControl(string token)
        {
            UserToken = token;
            InitializeComponent();

            config = SmartClientEnvironmentUtils.GetConfiguration();

            // Creamos cargamos la instancia del Chromium
            this.browser = CreateBrowser();

            // Agregamos la instancia al Panel
            AddElementToPanel(this.browser);
            this.Disposed += DACCFControl_Disposed;
        }

        private void DACCFControl_Disposed(object sender, EventArgs e)
        {
            var currentControls = this.Controls;
            foreach (Control item in currentControls)
            {
                item.Dispose();
                this.Controls.Remove(item);
            }

            this.Disposed -= DACCFControl_Disposed;
        }

        protected virtual ChromiumWebBrowser CreateBrowser()
        {
            CefSettings settings = new CefSettings();
            settings.RemoteDebuggingPort = 8088;
            ChromiumWebBrowser chromiumWebBrowser;

            if (!Cef.IsInitialized)
            {
                Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
            }

            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            CefSharpSettings.ShutdownOnExit = false;

            //string page = string.Format(@"{0}?Token-Session={1}", config.AppSettings.Settings["DACUrl"].Value, UserToken);
            string page = string.Format(@"{0}", config.AppSettings.Settings["DACUrl"].Value);
            //string page = string.Format(@"http://localhost:4200/#/home/?Token-Session=eyJhbGciOiJSUzI1NiIsImtpZCI6IjExODgyODgzMjVDMDJGMzkwNjJCODY2NEU3NTM3NzY3QjM5MjgxRkJSUzI1NiIsInR5cCI6ImF0K2p3dCIsIng1dCI6IkVZZ29neVhBTHprR0s0Wms1MU4zWjdPU2dmcyJ9.eyJuYmYiOjE2OTE2Nzg0NDAsImV4cCI6MTcyMzIxNDQ0MCwiaXNzIjoiaHR0cHM6Ly9hdXRoLnZtb25pdG9yaW5nLmNvbTo1MDAxIiwiYXVkIjpbImNhZF9hcGkiLCJJZGVudGl0eVNlcnZlckFwaSIsInZtb25fYXBpIiwiSW52b2ljZV9hcGkiLCJiYWNrb2ZmaWNlX2FwaSIsImF2bF9hcGkiLCJhdmxfZnJvbnQiLCJhcHBfbW9iaWxlIiwiSW52b2ljZUZyb250IiwiQ2FkRnJvbnRlbmQiLCJodHRwczovL2F1dGgudm1vbml0b3JpbmcuY29tOjUwMDEvcmVzb3VyY2VzIl0sImNsaWVudF9pZCI6IkJhY2tvZmZpY2VGcm9udCIsImNsaWVudF9zdWIiOiJlYTY5MTc4Mi0zM2UwLTRiMTUtOGRkNS1iODFlMjUzZGRhNWMiLCJjbGllbnRfaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoibW9uaXRvckBlbGlwZ28uY29tIiwiY2xpZW50X0FzcE5ldC5JZGVudGl0eS5TZWN1cml0eVN0YW1wIjoiVVlZM04zN1NHVkhXWTM1WDdXTkxPM0dNNEpCUkE3WjYiLCJjbGllbnRfcHJlZmVycmVkX3VzZXJuYW1lIjoibW9uaXRvci1lbGlwZ28iLCJjbGllbnRfbmFtZSI6Im1vbml0b3ItZWxpcGdvIiwiY2xpZW50X2VtYWlsIjoibW9uaXRvckBlbGlwZ28uY29tIiwiY2xpZW50X2VtYWlsX3ZlcmlmaWVkIjp0cnVlLCJzdWIiOiJlYTY5MTc4Mi0zM2UwLTRiMTUtOGRkNS1iODFlMjUzZGRhNWMiLCJhdXRoX3RpbWUiOjE2OTE2Nzg0NDAsImlkcCI6ImxvY2FsIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoibW9uaXRvckBlbGlwZ28uY29tIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiJVWVkzTjM3U0dWSFdZMzVYN1dOTE8zR000SkJSQTdaNiIsInByZWZlcnJlZF91c2VybmFtZSI6Im1vbml0b3ItZWxpcGdvIiwibmFtZSI6Im1vbml0b3ItZWxpcGdvIiwiZW1haWwiOiJtb25pdG9yQGVsaXBnby5jb20iLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiZW50aXR5SWQiOiI0MCIsImFwcFVzZXJJZCI6IjAiLCJhcHBVc2VyUm9sZXMiOiJbNTksNjAsOTYsMTA5LDI1Nyw0MjUsNDQ1LDQ0Niw1NTgsNTY3XSIsImFwcFJvbGVJZCI6IlN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkhhc2hTZXRgMVtTZXJ2ZXIuTW9kZWxzLlVzZXJSb2xlXSIsImVtcGxveWVlTnVtYmVyIjoiMTIzIiwiYXBwUm9sZXNJZHMiOiJbXCJDYWRGcm9udGVuZFwiLFwiQmFja29mZmljZUZyb250XCIsXCJBdmxGcm9udFwiLFwiU21hcnRDbGllbnRGcm9udFwiLFwiUmVwb3J0VXNlckZyb250XCIsXCJGYWNlXCIsXCJMUFJcIixcIlJldGFpbFwiLFwiU3luY3JvQmFja0Zyb250XCJdIiwiaXNQYXNzd29yZEVkaXRhYmxlIjoiRmFsc2UiLCJpc0VuYWJsZWQiOiJUcnVlIiwiaXNXb3JraW5nSG91cnMiOiJUcnVlIiwicGhvbmVOdW1iZXIiOiIiLCJtYXRyaXhIb3VycyI6IiIsImZpcnN0TmFtZSI6IiB0ZXN0MSIsImxhc3ROYW1lIjoiIHRlc3QyIiwianRpIjoiRjRFQzg4RTcwQkNEN0VDN0U4Rjc4RkE1MkE3M0RGNTkiLCJpYXQiOjE2OTE2Nzg0NDAsInNjb3BlIjpbIm9wZW5pZCIsInByb2ZpbGUiLCJlbWFpbCIsIklkZW50aXR5U2VydmVyQXBpIiwiQXV0aC5yZWFkIiwiQXV0aC53cml0ZSJdfQ.itK9EwcsCf-QwJ5BgpW4QCCE3spML8MH5IxV9OeAdaBO6q4UGmKRhM9QCV_71YVKRJMIWx6Aa_dhGsgMmdt_JjeNkmg9JW0YFa4D1GnC0y6uC7Eqz2ttgNNL3gLCgctiHpVaHU3JqIwrd6gSMB4rIzjuqDbumCAB8j7XNAqpwCItedUgR_ntIQoc1iiaSDge-Py5TbggE5guCl8XMkME_MymymN-SumaGaa6yHz1G9QAC4zNogL96gWjf484Q73SLJ0RZ5rzkYxgOUATklbK_eRUOYoUDXekn0EYTNoIoZyzEbPyqEzPqv237SNWnDuZBvbL4-u-JTFMd5h5gZ5Uvg");

            chromiumWebBrowser = new ChromiumWebBrowser(page)
            {
                RequestContext = new RequestContext(),
                BrowserSettings = new BrowserSettings
                {
                    FileAccessFromFileUrls = CefState.Enabled,
                    UniversalAccessFromFileUrls = CefState.Enabled,
                    BackgroundColor = Cef.ColorSetARGB(255, 34, 34, 34),
                    Javascript = CefState.Enabled
                },
                MenuHandler = new CustomCefMenuHandler(),
                DownloadHandler = new CustomCefDownloadHandler()
            };
            var bound = new BoundObject();
            chromiumWebBrowser.JavascriptObjectRepository.Register("bound", bound, true);
            chromiumWebBrowser.AllowDrop = true;

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
}
