using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Reflections;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using Splat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.Drivers.Kurento
{
    public partial class KurentoLiveUserControl : UserControl, IDriverLive, IDisposable
    {
        public struct JsonObject
        {
            public string Key;
            public string Value;
        }

        public event OnDriverDispose OnDispose;
        private bool _painted = false;
        private IManufactureUri manufactureUri;
        public bool IsSequencingEnabled => this.Camera.Sequencing != null;

        public KurentoLiveUserControl(CameraDTO camera, Profile profile, bool initAudio)
        {
            try
            {
                Camera = camera;
                Profile = profile;
                IsInitAudio = initAudio;
                InitializeComponent();
                manufactureUri = ManufactureUriFactory.Instance(this.Camera, this.Profile);
                this.Paint += KurentoLiveUserControl_Paint;
                this.Click += KurentoLiveClick;
                this.DoubleClick += KurentoLiveUserControl_DoubleClick;
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("Error: {0}  ", ex.Message));
                throw ex;
            }
        }

        private void KurentoLiveClick(object sender, EventArgs e)
        {
            CameraSelected(this, Camera);
        }

        private void KurentoLiveUserControl_DoubleClick(object sender, EventArgs e)
        {
            if (CameraSelectedDoubleClick != null)
            {
                CameraSelectedDoubleClick(this);
            }
        }

        private void KurentoLiveUserControl_Paint(object sender, PaintEventArgs e)
        {
            if (this._painted)
            {
                return;
            }

            this._painted = true;
            PaintBrowser();
        }

        private async void PaintBrowser()
        {
            try
            {
                string page = string.Format("file:///{0}/Resources/Html/kurento/index.html?destination={1}&ws_uri={2}", Application.StartupPath.Replace("\\", "/"), manufactureUri.StreamLiveKurentoUri(), manufactureUri.KurentoServer());

                string instanceId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
                string cachePath = System.IO.Path.Combine(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
                var browserExecutablePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libraries", "WebView2.131.0.2903.86.x64");
                PathUtils.CleanOldWebView2Caches(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
                var environment = await CoreWebView2Environment.CreateAsync(browserExecutablePath, cachePath);
                await browser.EnsureCoreWebView2Async(environment);
                browser.SourceChanged += WebView2Control_SourceChanged;
                browser.Click += KurentoLiveClick;
                browser.Source = new Uri(page);
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("Error: {0}  ", ex.Message));
                //throw ex;
            }
        }

        private void WebView2Control_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
        }

        private void WebView2Control_SourceChanged(object sender, CoreWebView2SourceChangedEventArgs e)
        {

        }

        private void WebView2Control_NavigationStarted(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
        }

        private void WebView2Control_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {

        }

        public Profile Profile { get; set; }
        public List<Profile> Profiles => new List<Profile> { Profile.MainStream, Profile.SubStream };
        public CameraDTO Camera { get; set; }
        public List<ButtonsContextBar> Commands => GetButtons();
        public List<ButtonsContextBar> CommandsAudioPtz => GetButtonsAudioPtz();
        public bool ListenStatus { get; set; }
        public bool ClipStatus { get; set; }
        public bool TalkStatus { get; set; }
        public bool PtzStatus { get; set; }
        public bool SequencingStatus { get; set; }
        public bool DigitalZoomStatus { get; set; }
        public bool InstantPlaybackStatus { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsInitAudio { get; set; }

        public event CameraSelectedEventHandler CameraSelected;
        public event CameraSelectedDoubleClickEventHandler CameraSelectedDoubleClick;
        public event OnVideoEventHandler OnVideo;
        public event ButtonPressedEventHandler PressedButtons;
        public event OnSequecingClick OnSequencing;
        public event OnInitializeAudioEventHandler OnInitializeAudio;
        public event OnAddExtraProfilesEventHandler OnAddExtraProfiles;

        private List<ButtonsContextBar> GetButtonsAudioPtz()
        {
            return new List<ButtonsContextBar>();
        }
        private List<ButtonsContextBar> GetButtons()
        {
            return new List<ButtonsContextBar>();
        }
        public bool CallGuard(ActivateGuardDTO guard)
        {
            return false;
        }

        public bool CallPreset(PresetDTO preset)
        {
            return false;
        }

        public bool ChangeProfile(Profile profile, bool autoSwitching = false)
        {
            return false;
        }

        public void DisposeDragged()
        {
        }

        public GuardForCreationDTO GetGuard(int guardId)
        {
            return null;
        }

        public GuardDTO[] ListGuards()
        {
            return null;
        }

        public PresetDTO[] ListPresets()
        {
            return null;
        }

        public bool Play()
        {
            return false;
        }

        public bool RemoveGuard(GuardDTO guard)
        {
            return false;
        }

        public bool RemovePreset(PresetDTO preset)
        {
            return false;
        }

        public bool SaveGuard(GuardForCreationDTO guard)
        {
            return false;
        }

        public bool SavePreset(PresetDTO preset)
        {
            return false;
        }

        public async Task<bool> Snapshot(string path, string token, int id)
        {
            return false;
        }

        public bool StateGuard(GuardDTO guard)
        {
            return false;
        }

        public bool Stop()
        {
            return false;
        }

        public bool StopGuard(ActivateGuardDTO guard)
        {
            return false;
        }

        public void SubcribePTZEvent()
        {
        }

        public void ToggleFullScreen()
        {
        }

        public bool ToggleInstantPlayback()
        {
            return false;
        }

        public bool ToggleListen(bool Listen)
        {
            return false;
        }

        public bool ToggleTalk()
        {
            return false;
        }

        public bool ToogleDigitalZoom()
        {
            return false;
        }

        public bool TooglePtz()
        {
            return false;
        }

        public void UnsubcribePTZEvent()
        {

        }

        public bool VideoClipStart(string path)
        {
            return false;
        }

        public bool VideoClipStop()
        {
            return false;
        }

        public bool Volume(int value)
        {
            return false;
        }

        private void Browser_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            JsonObject jsonObject = JsonConvert.DeserializeObject<JsonObject>(e.WebMessageAsJson);
            switch (jsonObject.Key)
            {
                case "click":
                    CameraSelected(this, Camera);
                    break;
            }
        }

        public bool ToggleTalk(bool talkStatus)
        {
            return false;
        }

        public IOPortState InputPortState()
        {
            throw new NotImplementedException();
        }

        public IOPortState OuputPortState()
        {
            throw new NotImplementedException();
        }

        public void OuputPortChangeState(IOPortState state)
        {
            throw new NotImplementedException();
        }

        public bool ToogleSequencing(bool value)
        {
            throw new NotImplementedException();
        }
    }
}
