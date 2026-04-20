using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Drivers;
using Splat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.ElementContainer
{

    public partial class ElementCameraControl : UserControl, IDisposable
    {
        public SidebarElementDTO _element;
        private bool _painted = false;
        private readonly IDriverFactory DriverFactory = Locator.Current.GetService<IDriverFactory>();
        public event ObjectSelectedEventHandler ObjectSelected;
        public event ObjectDoubleClickSelectedEventHandler ObjectSelectedDoubleClick;
        public event OnVideoEventHandler OnVideo;
        public event ButtonPressedEventHandler PressedButtons;
        public event OnSequecingClick OnSequencingClick;
        public event OnAddExtraProfilesEventHandler OnAddExtraProfiles;
        public CameraDTO Camera { get; set; }
        private Profile Profile { get; set; }
        private PlayMode _playMode { get; set; }
        private string _nameTab = string.Empty;
        private bool InitAudio { get; set; }
        private bool enabledPlaybackSync = false;

        public ElementCameraControl(SidebarElementDTO elementDTO, CameraDTO camera, Profile profile, bool initAudio, string nameTab, PlayMode playMode = PlayMode.Live, bool enabledPlaybackSync = false)
        {
            try
            {
                InitializeComponent();
                this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND);
                this.enabledPlaybackSync = enabledPlaybackSync;
                _element = elementDTO;
                Camera = camera;
                Profile = profile;
                InitAudio = initAudio;
                _playMode = playMode;
                _nameTab = nameTab;
                this.Click += ElementCameraControl_Click;
                LoadDriver();
            }
            catch (Exception ex)
            {
                Logger.Log($"Elipgo.SmartClient.UserControls.ElementContainer.ElementCameraControl, Exception --> {ex.Message}", LogPriority.Fatal);
            }
        }

        public new void Dispose()
        {
            foreach (Control c in this.Controls)
            { // modifique esto por que observe que los driver estan directamente debajo del elemnent camara controller, pero no se en que escenario Airan 
                //realizo el dispose de la camara cuando esto esta en los hijos, para no romper nada realizo el dispose en ambos escenarios
                if (c is IDriverLive)
                {
                    (c as IDriverLive).CameraSelected -= ElementCameraControl_CameraSelected;
                    (c as IDriverLive).CameraSelectedDoubleClick -= ElementCameraControl_CameraSelectedDoubleClick;
                    (c as IDriverLive).OnVideo -= ElementCameraControl_OnVideo;
                    (c as IDriverLive).PressedButtons -= ElementCameraControlPressedButtons;
                    (c as IDriverLive).OnSequencing -= ElementCameraControlOnSequencing;
                    (c as IDriverLive).OnAddExtraProfiles -= ElementCameraControl_OnAddExtraProfiles;
                    //(c as IDriverLive).Dispose();
                }
                else if (c is IDriverInstantPlayback)
                {
                    (c as IDriverInstantPlayback).CameraSelected -= ElementCameraControl_CameraSelected;
                    (c as IDriverInstantPlayback).CameraSelectedDoubleClick -= ElementCameraControl_CameraSelectedDoubleClick;
                    (c as IDriverInstantPlayback).OnVideo -= ElementCameraControl_OnVideo;


                    //(c as IDriverInstantPlayback).Dispose();
                }
                else
                {
                    foreach (Control p in c.Controls)
                    {
                        if (p is IDriverLive)
                        {
                            (p as IDriverLive).CameraSelected -= ElementCameraControl_CameraSelected;
                            (p as IDriverLive).CameraSelectedDoubleClick -= ElementCameraControl_CameraSelectedDoubleClick;
                            (p as IDriverLive).OnVideo -= ElementCameraControl_OnVideo;
                            (p as IDriverLive).PressedButtons -= ElementCameraControlPressedButtons;
                            (p as IDriverLive).OnSequencing -= ElementCameraControlOnSequencing;
                            (p as IDriverLive).OnAddExtraProfiles -= ElementCameraControl_OnAddExtraProfiles;

                            (p as IDriverLive).Dispose();
                        }
                        else if (p is IDriverInstantPlayback)
                        {
                            (p as IDriverInstantPlayback).CameraSelected -= ElementCameraControl_CameraSelected;
                            (p as IDriverInstantPlayback).CameraSelectedDoubleClick -= ElementCameraControl_CameraSelectedDoubleClick;
                            (p as IDriverInstantPlayback).OnVideo -= ElementCameraControl_OnVideo;
                            (p as IDriverInstantPlayback).Dispose();
                        }
                        else
                        {
                            if (!p.IsDisposed)
                                p.Dispose();
                        }

                    }
                }
                if (!c.IsDisposed)
                    c.Dispose();
                c.Controls.Clear();
            }
            this.Controls.Clear();
            this.Click -= ElementCameraControl_Click;
            if (ObjectSelected != null)
            {
                foreach (Delegate d in ObjectSelected?.GetInvocationList())
                {
                    ObjectSelected -= (ObjectSelectedEventHandler)d;
                }
            }
            if (ObjectSelectedDoubleClick != null)
            {
                foreach (Delegate d in ObjectSelectedDoubleClick?.GetInvocationList())
                {
                    ObjectSelectedDoubleClick -= (ObjectDoubleClickSelectedEventHandler)d;
                }
            }
            if (OnVideo != null)
            {
                foreach (Delegate d in OnVideo?.GetInvocationList())
                {
                    OnVideo -= (OnVideoEventHandler)d;
                }
            }
            if (PressedButtons != null)
            {
                foreach (Delegate d in PressedButtons?.GetInvocationList())
                {
                    PressedButtons -= (ButtonPressedEventHandler)d;
                }
            }
            if (OnSequencingClick != null)
            {
                foreach (Delegate d in OnSequencingClick?.GetInvocationList())
                {
                    OnSequencingClick -= (OnSequecingClick)d;
                }
            }
            if (OnAddExtraProfiles != null)
            {
                foreach (Delegate d in OnAddExtraProfiles?.GetInvocationList())
                {
                    OnAddExtraProfiles -= (OnAddExtraProfilesEventHandler)d;
                }
            }
        }

        public IDriverInstantPlayback GetInstantDriver()
        {
            return this.Controls.Count > 0 ? this.Controls[0] as IDriverInstantPlayback : null;
        }

        public IDriverLive GetDriverLive()
        {
            return this.Controls[0] as IDriverLive;
        }

        public void DisposeDraggued()
        {
            var controls = this.Controls.OfType<IDriverLive>().ToList();
            if (controls.Count == 1)
            {
                controls[0].DisposeDragged();
            }

            var cls = this.Controls.OfType<IDriverInstantPlayback>().ToList();
            if (cls.Count == 1)
            {
                cls[0].Dispose();
            }
        }
        //este metodo es para ser utilizado en modo secuenciamiento
        private void LoadDriver()
        {
            /*if (this.InvokeRequired)
           {
               this.Invoke((MethodInvoker)delegate
               {
                   LoadDriver();
               });
               return;
           }*/

            Control control = new Control();
            switch (_playMode)
            {
                case PlayMode.Live:
                    control = DriverFactory.GetDriverLive(Camera, Profile, InitAudio, _nameTab) as Control;
                    (control as IDriverLive).CameraSelected += ElementCameraControl_CameraSelected;
                    (control as IDriverLive).CameraSelectedDoubleClick += ElementCameraControl_CameraSelectedDoubleClick;
                    (control as IDriverLive).Play();
                    (control as IDriverLive).OnVideo += ElementCameraControl_OnVideo;
                    (control as IDriverLive).PressedButtons += ElementCameraControlPressedButtons;
                    (control as IDriverLive).OnSequencing += ElementCameraControlOnSequencing;
                    (control as IDriverLive).OnAddExtraProfiles += ElementCameraControl_OnAddExtraProfiles;

                    break;
                case PlayMode.Playback:
                    control = DriverFactory.GetDriverInstantPlayback(Camera, Profile, this._element.GetRecorderDTO(), DateTime.UtcNow, _nameTab, true) as Control;
                    control.Dock = DockStyle.Fill;
                    (control as IDriverInstantPlayback).CameraSelected += ElementCameraControl_CameraSelected;
                    (control as IDriverInstantPlayback).CameraSelectedDoubleClick += ElementCameraControl_CameraSelectedDoubleClick;
                    if (!enabledPlaybackSync)
                    {
                        (control as IDriverInstantPlayback).Play();
                    } (control as IDriverInstantPlayback).OnVideo += ElementCameraControl_OnVideo;
                    break;
            }
            //switch (_playMode)
            //{
            //    case PlayMode.Live:
            //        control = DriverFactory.GetDriverLive(Camera, Profile) as Control;
            //        (control as IDriverLive).CameraSelected += ElementCameraControl_CameraSelected;
            //        break;
            //    case PlayMode.Playback:
            //        control = DriverFactory.GetDriverInstantPlayback(Camera, Profile, this._element.RecorderDriver, DateTime.UtcNow, true) as Control;
            //        control.Dock = DockStyle.Fill;
            //        (control as IDriverInstantPlayback).CameraSelected += ElementCameraControl_CameraSelected;
            //        break;
            //}
            control.Name = Guid.NewGuid().ToString();
            control.Height = this.Height - 2;
            control.Width = this.Width - 2;
            control.Location = new Point(1, 1);
            control.Visible = true;
            control.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right;

            this.Controls.Add(control);
            //  control.Paint += Control_Paint;
        }

        private void ElementCameraControl_OnAddExtraProfiles(object sender, Profile profile)
        {
            OnAddExtraProfiles?.Invoke(sender, profile);
        }

        private void ElementCameraControlOnSequencing(int dvfID)
        {
            OnSequencingClick?.Invoke(dvfID);
        }

        private void ElementCameraControlPressedButtons(List<ActionCommand> pressedButton)
        {
            PressedButtons?.Invoke(pressedButton);
        }

        private void Control_Paint(object sender, PaintEventArgs e)
        {
            if (this._painted)
            {
                return;
            }

            this._painted = true;

            Task.Run(() =>
            {
                switch (_playMode)
                {
                    case PlayMode.Live:
                        (sender as IDriverLive).Play();
                        (sender as IDriverLive).OnVideo += ElementCameraControl_OnVideo;
                        break;
                    case PlayMode.Playback:
                        //(sender as IDriverInstantPlayback).Play();
                        (sender as IDriverInstantPlayback).OnVideo += ElementCameraControl_OnVideo;
                        break;
                }
            });
        }

        private void ElementCameraControl_OnVideo(bool video, object parent)
        {/*este evento es utilizado unicamente para detectar 
            cuando la camara comienza a reproducir
            la primera vez, y poder sincronizar las velocidades de reproducion
            con la camara master en caso de que este funcionando en este modo*/

            this.OnVideo?.Invoke(true, parent);
            if (_playMode == PlayMode.Playback)
            {
                bool bFound = false;
                for (int i = 0; i < this.Controls.Count && !bFound; i++)
                {
                    if (this.Controls[i] is IDriverInstantPlayback)
                    {
                        bFound = true;
                        (this.Controls[i] as IDriverInstantPlayback).OnVideo -= ElementCameraControl_OnVideo;
                    }
                }
            }
        }

        private void ElementCameraControl_CameraSelected(object sender, CameraDTO element)
        {
            ObjectSelected?.Invoke(this, _element);
        }

        private void ElementCameraControl_CameraSelectedDoubleClick(object sender)
        {
            ObjectSelectedDoubleClick?.Invoke(this);
        }

        private void ElementCameraControl_Click(object sender, EventArgs e)
        {
            ObjectSelected?.Invoke(this, _element);
        }

        protected override void Dispose(bool disposing)
        {
            this.Click -= ElementCameraControl_Click;
            foreach (var control in this.Controls)
            {
                if (control is IDriverLive)
                {

                    (control as IDriverLive).CameraSelected -= ElementCameraControl_CameraSelected;
                    (control as IDriverLive).CameraSelectedDoubleClick -= ElementCameraControl_CameraSelectedDoubleClick;
                    (control as IDriverLive).OnVideo -= ElementCameraControl_OnVideo;
                    (control as IDriverLive).PressedButtons -= ElementCameraControlPressedButtons;
                    (control as IDriverLive).OnSequencing -= ElementCameraControlOnSequencing;
                    (control as IDriverLive).OnAddExtraProfiles -= ElementCameraControl_OnAddExtraProfiles;
                }
                else
                    if (control is IDriverInstantPlayback)
                {
                    (control as IDriverInstantPlayback).CameraSelected -= ElementCameraControl_CameraSelected;
                    (control as IDriverInstantPlayback).CameraSelectedDoubleClick -= ElementCameraControl_CameraSelectedDoubleClick;
                    (control as IDriverInstantPlayback).OnVideo -= ElementCameraControl_OnVideo;
                }
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            if (base.IsHandleCreated)
            {
                base.Dispose(disposing);
            }
            this.Dispose();
        }
    }
}
