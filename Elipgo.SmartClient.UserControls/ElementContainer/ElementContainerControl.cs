using Bunifu.Framework.UI;
using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Drivers;
using Elipgo.SmartClient.UserControls.Blueprint;
using Elipgo.SmartClient.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.ElementContainer
{
    public delegate void ObjectSelectedEventHandler(object sender, SidebarElementDTO element);
    public delegate void ObjectDoubleClickSelectedEventHandler(object sender);
    public delegate void ObjectChangeEventHandler(object sender, SidebarElementDTO element, object data);
    public delegate void IOPortStateEventHandler(object sender, SidebarElementDTO element, IOPortState state);
    public delegate void ButtonBackEventHandler(object sender);
    public delegate void ObjectPaint(ElementContainerControl container, SidebarElementDTO element, bool showContextbar, bool ptzStatus);
    public delegate void ObjectOnDragEventHandler(object sender, string namePanel);
    public delegate void ButtonPressedEventHandler(List<ActionCommand> pressedButton);
    public delegate void ObjectSelectedTalkEventHandler(object sender, bool e);

    public partial class ElementContainerControl : UserControl, IDisposable
    {
        public event ObjectSelectedEventHandler ObjectSelected;
        public event ObjectDoubleClickSelectedEventHandler ObjectDoubleClickSelected;
        public event ObjectSelectedTalkEventHandler ObjectSelectedTalk;
        //public event ObjectSelectedTalkEventHandler ObjectSelectedDigitalZoom;
        public event ObjectChangeEventHandler ObjectChange;
        public event ButtonBackEventHandler OnClickBackButton;
        public event IOPortStateEventHandler OnIOPortState;
        public event ObjectPaint OnObjectLoaded;
        public event ObjectOnDragEventHandler ObjectOnDrag;
        public event ButtonPressedEventHandler PressedButtons;
        public event OnVideoEventHandler OnVideo;
        public event OnSequecingClick OnSequencingClick;
        public event OnAddExtraProfilesEventHandler OnAddExtraProfiles;

        public List<CarouselDTO> carousels = new List<CarouselDTO>();
        public List<CarouselDTO> originCarousels = new List<CarouselDTO>();
        private int act_carousel = 0;
        private Profile _profile = Profile.None;

        private System.Timers.Timer aTimer = new System.Timers.Timer();
        private bool aTimerExist = true;
        public bool ActiveCarousel = true;
        public readonly SidebarElementDTO originElement;
        private bool _loaded = false;
        private bool _showContextbar;
        private string _nameTab = string.Empty;

        public object _dto;
        private bool _showButtonBack = false;

        private LiveViewModel _viewModelLive;
        private PlaybackViewModel _viewModelPlayback;

        public SidebarElementDTO Element = null;
        public PlayMode _playMode = PlayMode.Live;
        public ElementGroupingMode ElementGroupingMode;
        private bool enabledPlaybackSync = false;
        public ElementContainerControl(SidebarElementDTO element, object dto, Profile profile, IRoutableViewModel viewModel, bool showContextBar, bool showButtonBack = false, bool enabledPlaybackSync = false)
        {

            InitializeComponent();
            string t = viewModel.GetType().Name;
            this.enabledPlaybackSync = enabledPlaybackSync;
            switch (t)
            {
                case nameof(LiveViewModel):
                    _viewModelLive = viewModel as LiveViewModel;
                    _playMode = PlayMode.Live;
                    _nameTab = _viewModelLive.MainView.ApplicationTitle;
                    break;
                case nameof(PlaybackViewModel):
                    _viewModelPlayback = viewModel as PlaybackViewModel;
                    _playMode = PlayMode.Playback;
                    _nameTab = _viewModelPlayback.MainView.ApplicationTitle;
                    break;
            }
            //viewModel = liveView;
            originElement = element;
            _dto = dto;
            _showButtonBack = showButtonBack;
            _profile = profile;
            _showContextbar = showContextBar;
            _sequencingMode = false;
            this.Paint += ElementContainerControl_Paint;

            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND);
        }

        public new void Dispose()
        {
            this.Paint -= ElementContainerControl_Paint;
            aTimer.Elapsed -= new ElapsedEventHandler(OnTimedEvent);
            aTimer.Dispose();
            aTimerExist = false;
            StopCarrusel();
            if (ObjectSelected != null)
            {
                foreach (Delegate d in ObjectSelected.GetInvocationList())
                {
                    ObjectSelected -= (ObjectSelectedEventHandler)d;
                }
            }
            if (ObjectDoubleClickSelected != null)
            {
                foreach (Delegate d in ObjectDoubleClickSelected.GetInvocationList())
                {
                    ObjectDoubleClickSelected -= (ObjectDoubleClickSelectedEventHandler)d;
                }
            }

            if (ObjectSelectedTalk != null)
            {
                foreach (Delegate d in ObjectSelectedTalk.GetInvocationList())
                {
                    ObjectSelectedTalk -= (ObjectSelectedTalkEventHandler)d;
                }
            }

            if (ObjectChange != null)
            {
                foreach (Delegate d in ObjectChange.GetInvocationList())
                {
                    ObjectChange -= (ObjectChangeEventHandler)d;
                }
            }

            if (OnClickBackButton != null)
            {
                foreach (Delegate d in OnClickBackButton.GetInvocationList())
                {
                    OnClickBackButton -= (ButtonBackEventHandler)d;
                }
            }

            if (OnIOPortState != null)
            {
                foreach (Delegate d in OnIOPortState.GetInvocationList())
                {
                    OnIOPortState -= (IOPortStateEventHandler)d;
                }
            }

            //if (OnObjectLoaded != null)
            //    foreach (Delegate d in OnObjectLoaded.GetInvocationList())
            //    {
            //        OnObjectLoaded -= (ObjectPaint)d;
            //    }

            if (ObjectOnDrag != null)
            {
                foreach (Delegate d in ObjectOnDrag.GetInvocationList())
                {
                    ObjectOnDrag -= (ObjectOnDragEventHandler)d;
                }
            }

            if (PressedButtons != null)
            {
                foreach (Delegate d in PressedButtons.GetInvocationList())
                {
                    PressedButtons -= (ButtonPressedEventHandler)d;
                }
            }

            if (OnVideo != null)
            {
                foreach (Delegate d in OnVideo.GetInvocationList())
                {
                    OnVideo -= (OnVideoEventHandler)d;
                }
            }

            if (OnSequencingClick != null)
            {
                foreach (Delegate d in OnSequencingClick.GetInvocationList())
                {
                    OnSequencingClick -= (OnSequecingClick)d;
                }
            }

            if (OnAddExtraProfiles != null)
            {
                foreach (Delegate d in OnAddExtraProfiles.GetInvocationList())
                {
                    OnAddExtraProfiles -= (OnAddExtraProfilesEventHandler)d;
                }
            }
        }

        private void ElementContainerControl_Paint(object sender, PaintEventArgs e)
        {
            if (_loaded)
            {
                return;
            }

            _loaded = true;
            if (originElement == null || originElement.DeviceType == ElementType.None || originElement.Key == Guid.Empty)
            {
                PanelElement.Controls.Add(new Label
                {
                    Name = "LabelError",
                    Text = "Element not set",
                    Visible = true,
                    AutoSize = false,
                    ForeColor = Color.White,
                    Size = PanelElement.Size,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right
                });
            }
            else
            {
                LoadElement(originElement, _dto, _profile, _showButtonBack);
            }
            //ObjectSelected(this, originE  lement);
            this.Focus();
        }

        private void LoadElement(SidebarElementDTO element, object dto, Profile profile, bool showButtonBack = false)
        {
            try
            {
                Control control;

                if (PanelElement.InvokeRequired)
                {
                    PanelElement.Invoke((MethodInvoker)delegate
                    {
                        LoadElement(element, dto, profile, showButtonBack);
                    });
                    return;
                }

                // Suspender layout para evitar flickering durante carga
                PanelElement.SuspendLayout();

                switch (element.DeviceType)
                {
                    case ElementType.Camera:
                        control = new ElementCameraControl(element, dto as CameraDTO, profile, false, _nameTab, _playMode, enabledPlaybackSync)
                        {
                            Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right
                        };
                        (control as ElementCameraControl).ObjectSelected += ElementContainerControl_ObjectSelected;
                        (control as ElementCameraControl).ObjectSelectedDoubleClick += ElementContainerControl_ObjectSelectedDoubleClick;
                        (control as ElementCameraControl).OnVideo += ElementContainerControl_OnVideo;
                        (control as ElementCameraControl).PressedButtons += ElementContainerControlPressedButtons;
                        (control as ElementCameraControl).OnSequencingClick += ElementContainerSequencingClick;
                        (control as ElementCameraControl).OnAddExtraProfiles += ElementContainerControl_OnAddExtraProfiles;
                        break;
                    case ElementType.Geomap:
                        control = new ElementGeomapControl(element, dto as List<CatalogCamera>)
                        {
                            Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right
                        };
                        (control as ElementGeomapControl).ObjectSelected += ElementContainerControl_ObjectSelected;
                        (control as ElementGeomapControl).ObjectOnClick += ElementContainerControl_ObjectOnClick;
                        (control as ElementGeomapControl).ObjectOnDrag += ElementContainerControl_ObjectOnDrag;
                        (control as ElementGeomapControl).ObjectSelectedDoubleClick += ElementContainerControl_ObjectSelectedDoubleClick;
                        break;
                    case ElementType.AlarmsMap:
                        control = new ElementAlarmGeomapControl(element, dto as List<AlarmGeoMapDTO>)
                        {
                            Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right
                        };
                        (control as ElementAlarmGeomapControl).ObjectSelected += ElementContainerControl_ObjectSelected;
                        (control as ElementAlarmGeomapControl).ObjectOnClick += ElementContainerControl_ObjectOnClick;
                        (control as ElementAlarmGeomapControl).ObjectOnDrag += ElementContainerControl_ObjectOnDrag;
                        (control as ElementAlarmGeomapControl).ObjectSelectedDoubleClick += ElementContainerControl_ObjectSelectedDoubleClick;
                        break;
                    case ElementType.Iot_In:
                    case ElementType.Iot_Out:
                        control = new ElementIotControl(element, dto as CameraDTO)
                        {
                            Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right
                        };
                        (control as ElementIotControl).ObjectSelected += ElementContainerControl_ObjectSelected;
                        (control as ElementIotControl).OnIOPortState += ElementContainerControl_OnIOPortState;
                        (control as ElementIotControl).ObjectSelectedDoubleClick += ElementContainerControl_ObjectSelectedDoubleClick;
                        break;
                    case ElementType.Kpi:
                        control = new ElementKPIControl(element, dto as KpiDTO)
                        {
                            Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right
                        };
                        (control as ElementKPIControl).ObjectSelected += ElementContainerControl_ObjectSelected;
                        (control as ElementKPIControl).ObjectOnDrag += ElementContainerControl_ObjectOnDrag;
                        (control as ElementKPIControl).ObjectSelectedDoubleClick += ElementContainerControl_ObjectSelectedDoubleClick;
                        break;
                    case ElementType.Carousel:
                        PanelElement.ResumeLayout(false); // Reanudar antes del return
                        LaunchCarousel(element, dto, profile);
                        return;
                    case ElementType.Face:
                        control = new ElementFaceControl(element, dto as List<FaceAlarmsDTO>, _viewModelLive)
                        {
                            BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND)
                        };
                        (control as ElementFaceControl).ObjectSelected += ElementContainerControl_ObjectSelected;
                        (control as ElementFaceControl).ObjectSelectedDoubleClick += ElementContainerControl_ObjectSelectedDoubleClick;
                        break;
                    case ElementType.Lpr:
                        control = new ElementLprControl(element, dto as List<LprAlarmDTO>, _viewModelLive)
                        {
                            BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU)
                        };
                        (control as ElementLprControl).ObjectSelected += ElementContainerControl_ObjectSelected;
                        (control as ElementLprControl).ObjectSelectedDoubleClick += ElementContainerControl_ObjectSelectedDoubleClick;
                        break;
                    case ElementType.Location:
                    case ElementType.Blueprint:
                        control = new ElementBlueprintControl(element, dto as BlueprintDTO, _viewModelLive)
                        {
                            Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right
                        };
                        (control as ElementBlueprintControl).ObjectSelected += ElementContainerControl_ObjectSelected;
                        (control as ElementBlueprintControl).ObjectOnClick += ElementContainerControl_ObjectOnClick;
                        (control as ElementBlueprintControl).ObjectOnDrag += ElementContainerControl_ObjectOnDrag;
                        (control as ElementBlueprintControl).ObjectSelectedDoubleClick += ElementContainerControl_ObjectSelectedDoubleClick;
                        break;
                    case ElementType.Geolocation_Alarm:
                        control = new ElementAlarmGeolocationControl(element, dto as CardDto, _viewModelLive)
                        {
                            Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right
                        };
                        (control as ElementAlarmGeolocationControl).ObjectSelected += ElementContainerControl_ObjectSelected;
                        (control as ElementAlarmGeolocationControl).ObjectOnClick += ElementContainerControl_ObjectOnClick;
                        (control as ElementAlarmGeolocationControl).ObjectOnDrag += ElementContainerControl_ObjectOnDrag;
                        (control as ElementAlarmGeolocationControl).ObjectSelectedDoubleClick += ElementContainerControl_ObjectSelectedDoubleClick;
                        break;
                    default:
                        control = new Label
                        {
                            Text = "Element not set",
                            AutoSize = false,
                            ForeColor = Color.White,
                            TextAlign = ContentAlignment.MiddleCenter
                        };
                        break;
                }
                control.Name = element.Key.ToString();
                control.Visible = true;

                if (showButtonBack && element.DeviceTypeStr != "VIN")
                {
                    BunifuImageButton buttonBack = new BunifuImageButton
                    {
                        BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_SELECTED),
                        Location = new Point(4, 4),
                        Size = new Size(24, 24),
                        Image = FileResources.icon_back,
                        Cursor = Cursors.Hand
                    };
                    buttonBack.Click += ButtonBack_Click;
                    this.PanelElement.Controls.Add(buttonBack);
                }
                AddButtonSwapContainer("swap", FileResources.icon_drag_drop);
                if (_playMode == PlayMode.Playback)
                {
                    AddButtonMasterCamara();
                }

                control.Location = new Point(1, 1);
                control.Height = PanelElement.Height - 2;
                control.Width = PanelElement.Width - 2;
                control.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right;
                PanelElement.Controls.Add(control);

                // Reanudar layout después de agregar controles
                PanelElement.ResumeLayout(true);

                this.OnObjectLoaded?.Invoke(this, originElement, _showContextbar, showButtonBack);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error to LoadElement, Exception --> {ex.Message} {ex.StackTrace}", LogPriority.Fatal);
                // Asegurar ResumeLayout incluso en caso de error
                try { PanelElement.ResumeLayout(false); } catch { }
            }
        }

        private void ElementContainerControl_OnAddExtraProfiles(object sender, Profile profile)
        {
            OnAddExtraProfiles?.Invoke(this, profile);
        }

        private void ElementContainerSequencingClick(int dvfID)
        {
            OnSequencingClick?.Invoke(dvfID);
        }

        private void ElementContainerControlPressedButtons(List<ActionCommand> pressedButton)
        {
            PressedButtons?.Invoke(pressedButton);
        }

        public void StopCarrusel()
        {
            aTimer.Stop();
            carousels = null;
        }

        private void LaunchCarousel(SidebarElementDTO element, object dto, Profile profile)
        {
            carousels = dto as List<CarouselDTO>;
            _profile = profile;

            aTimer.Interval = carousels.Count == 0 ? originCarousels[0].Time * 1000 : carousels[0].Time * 1000;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //LoadElement(carousels[0], carousels[0].Data, _profile);

            if (carousels.Count > 0)
            {
                var elementStart = carousels[0];
                LoadElement(elementStart, elementStart.Data, _profile);
            }
            else if (carousels.Count == 0 && originCarousels.Count > 0)
            {
                PanelElement.Controls.Add(new Label
                {
                    Name = "LabelError",
                    Text = Resources.CarouselText,
                    Visible = true,
                    AutoSize = false,
                    ForeColor = Color.White,
                    Size = PanelElement.Size,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right
                });
            }

            act_carousel++;
            ActiveCarousel = true;
            aTimer.Start();
        }

        private async void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        OnTimedEvent(source, e);
                    });
                    return;
                }

                var controlsToRemove = new List<Control>(PanelElement.Controls.Cast<Control>());
                if (carousels.Count >= 1)
                {
                    aTimer.Stop();
                    var element = carousels[act_carousel];
                    if (PanelElement.Controls.Count > 0)
                    {
                        for (int i = PanelElement.Controls.Count - 1; i >= 0; i--)
                        {

                            if (PanelElement.Controls[i] is BunifuImageButton)
                            {
                                (PanelElement.Controls[i] as BunifuImageButton).Click -= ButtonBack_Click;
                                (PanelElement.Controls[i] as BunifuImageButton).Dispose();
                                //this.PanelElement.Controls.Remove((ctl as Control));
                            }
                            else if (PanelElement.Controls[i] is ElementCameraControl)
                            {
                                (PanelElement.Controls[i] as ElementCameraControl).ObjectSelected -= ElementContainerControl_ObjectSelected;
                                (PanelElement.Controls[i] as ElementCameraControl).ObjectSelectedDoubleClick -= ElementContainerControl_ObjectSelectedDoubleClick;
                                (PanelElement.Controls[i] as ElementCameraControl).OnVideo -= ElementContainerControl_OnVideo;
                                (PanelElement.Controls[i] as ElementCameraControl).PressedButtons -= ElementContainerControlPressedButtons;
                                (PanelElement.Controls[i] as ElementCameraControl).OnSequencingClick -= ElementContainerSequencingClick;
                                (PanelElement.Controls[i] as ElementCameraControl).OnAddExtraProfiles -= ElementContainerControl_OnAddExtraProfiles;
                                (PanelElement.Controls[i] as ElementCameraControl).Dispose();
                            }
                            else if (PanelElement.Controls[i] is ElementBlueprintControl)
                            {
                                (PanelElement.Controls[i] as ElementBlueprintControl).ObjectSelected -= ElementContainerControl_ObjectSelected;
                                (PanelElement.Controls[i] as ElementBlueprintControl).ObjectOnClick -= ElementContainerControl_ObjectOnClick;
                                (PanelElement.Controls[i] as ElementBlueprintControl).ObjectOnDrag -= ElementContainerControl_ObjectOnDrag;

                            }
                            else if (PanelElement.Controls[i] is ElementLprControl)
                            {
                                (PanelElement.Controls[i] as ElementLprControl).ObjectSelected -= ElementContainerControl_ObjectSelected;
                            }
                            else if (PanelElement.Controls[i] is ElementFaceControl)
                            {
                                (PanelElement.Controls[i] as ElementFaceControl).ObjectSelected -= ElementContainerControl_ObjectSelected;
                            }
                            else if (PanelElement.Controls[i] is ElementKPIControl)
                            {
                                (PanelElement.Controls[i] as ElementKPIControl).ObjectSelected -= ElementContainerControl_ObjectSelected;
                                (PanelElement.Controls[i] as ElementKPIControl).ObjectOnDrag -= ElementContainerControl_ObjectOnDrag;
                            }
                            else if (PanelElement.Controls[i] is ElementIotControl)
                            {
                                (PanelElement.Controls[i] as ElementIotControl).ObjectSelected -= ElementContainerControl_ObjectSelected;
                                (PanelElement.Controls[i] as ElementIotControl).OnIOPortState -= ElementContainerControl_OnIOPortState;
                            }
                            else if (PanelElement.Controls[i] is ElementAlarmGeomapControl)
                            {
                                (PanelElement.Controls[i] as ElementAlarmGeomapControl).ObjectSelected -= ElementContainerControl_ObjectSelected;
                                (PanelElement.Controls[i] as ElementAlarmGeomapControl).ObjectOnClick -= ElementContainerControl_ObjectOnClick;
                                (PanelElement.Controls[i] as ElementAlarmGeomapControl).ObjectOnDrag -= ElementContainerControl_ObjectOnDrag;
                            }
                            else if (PanelElement.Controls[i] is ElementGeomapControl)
                            {
                                (PanelElement.Controls[i] as ElementGeomapControl).ObjectSelected -= ElementContainerControl_ObjectSelected;
                                (PanelElement.Controls[i] as ElementGeomapControl).ObjectOnClick -= ElementContainerControl_ObjectOnClick;
                                (PanelElement.Controls[i] as ElementGeomapControl).ObjectOnDrag -= ElementContainerControl_ObjectOnDrag;
                            }
                            else if (PanelElement.Controls[i] is ElementAlarmGeolocationControl)
                            {
                                (PanelElement.Controls[i] as ElementAlarmGeolocationControl).ObjectSelected -= ElementContainerControl_ObjectSelected;
                                (PanelElement.Controls[i] as ElementAlarmGeolocationControl).ObjectOnClick -= ElementContainerControl_ObjectOnClick;
                                (PanelElement.Controls[i] as ElementAlarmGeolocationControl).ObjectOnDrag -= ElementContainerControl_ObjectOnDrag;
                                (PanelElement.Controls[i] as ElementAlarmGeolocationControl).Dispose();
                            }
                        }

                        var cc = PanelElement.Controls[PanelElement.Controls.Count - 1];
                        var driver = cc.Controls.OfType<IDriverLive>().ToList();
                        if (driver.Count > 0)
                        {
                            driver[0].Dispose();
                        }

                        cc.Dispose();
                    }
                    PanelElement.Controls.Clear();
                    LoadElement(element, element.Data, _profile);
                    await Task.Delay(5000);
                    ClearPanelElement(controlsToRemove);
                    act_carousel++;
                }
                else if (carousels.Count == 0 && originCarousels.Count > 0)
                {
                    ClearPanelElement(controlsToRemove, true);
                    PanelElement.Controls.Add(new Label
                    {
                        Name = "LabelError",
                        Text = Resources.CarouselText,
                        Visible = true,
                        AutoSize = false,
                        ForeColor = Color.White,
                        Size = PanelElement.Size,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right
                    });
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (aTimerExist)
                {
                    aTimer.Start();
                }
                //act_carousel++;
                //if (act_carousel > (carousels != null ? carousels.Count : 0) - 1)
                if (carousels != null && act_carousel > carousels.Count - 1)
                {
                    act_carousel = 0;
                }
            }
        }

        private void ClearPanelElement(List<Control> ControlsRemove, bool clearAll = false)
        {
            foreach (var itemRemove in ControlsRemove)
            {
                var ctl = PanelElement.Controls.Cast<Control>().FirstOrDefault(control => control.Equals(itemRemove));
                if (ctl == null)
                {
                    return;
                }

                switch (ctl)
                {
                    case BunifuImageButton bunifuButton:
                        bunifuButton.Click -= ButtonBack_Click;
                        break;

                    case ElementCameraControl cameraControl:
                        cameraControl.ObjectSelected -= ElementContainerControl_ObjectSelected;
                        cameraControl.ObjectSelectedDoubleClick -= ElementContainerControl_ObjectSelectedDoubleClick;
                        cameraControl.OnVideo -= ElementContainerControl_OnVideo;
                        cameraControl.PressedButtons -= ElementContainerControlPressedButtons;
                        cameraControl.OnSequencingClick -= ElementContainerSequencingClick;
                        cameraControl.OnAddExtraProfiles -= ElementContainerControl_OnAddExtraProfiles;
                        break;

                    case ElementBlueprintControl blueprintControl:
                        blueprintControl.ObjectSelected -= ElementContainerControl_ObjectSelected;
                        blueprintControl.ObjectOnClick -= ElementContainerControl_ObjectOnClick;
                        blueprintControl.ObjectOnDrag -= ElementContainerControl_ObjectOnDrag;
                        break;

                    case ElementLprControl lprControl:
                        lprControl.ObjectSelected -= ElementContainerControl_ObjectSelected;
                        break;

                    case ElementFaceControl faceControl:
                        faceControl.ObjectSelected -= ElementContainerControl_ObjectSelected;
                        break;

                    case ElementKPIControl kpiControl:
                        kpiControl.ObjectSelected -= ElementContainerControl_ObjectSelected;
                        kpiControl.ObjectOnDrag -= ElementContainerControl_ObjectOnDrag;
                        break;

                    case ElementIotControl iotControl:
                        iotControl.ObjectSelected -= ElementContainerControl_ObjectSelected;
                        iotControl.OnIOPortState -= ElementContainerControl_OnIOPortState;
                        break;

                    case ElementAlarmGeomapControl alarmGeomapControl:
                        alarmGeomapControl.ObjectSelected -= ElementContainerControl_ObjectSelected;
                        alarmGeomapControl.ObjectOnClick -= ElementContainerControl_ObjectOnClick;
                        alarmGeomapControl.ObjectOnDrag -= ElementContainerControl_ObjectOnDrag;
                        break;

                    case ElementGeomapControl geomapControl:
                        geomapControl.ObjectSelected -= ElementContainerControl_ObjectSelected;
                        geomapControl.ObjectOnClick -= ElementContainerControl_ObjectOnClick;
                        geomapControl.ObjectOnDrag -= ElementContainerControl_ObjectOnDrag;
                        break;

                    case ElementAlarmGeolocationControl alarmGeolocationControl:
                        alarmGeolocationControl.ObjectSelected -= ElementContainerControl_ObjectSelected;
                        alarmGeolocationControl.ObjectOnClick -= ElementContainerControl_ObjectOnClick;
                        alarmGeolocationControl.ObjectOnDrag -= ElementContainerControl_ObjectOnDrag;
                        break;
                }

                ctl.Dispose();
                PanelElement.Controls.Remove(ctl);
            }

            ControlsRemove.Clear();
            if (clearAll)
            {
                PanelElement.Controls.Clear();
            }
        }

        public void ToogleCarousel()
        {
            if (ActiveCarousel)
            {
                aTimer.Stop();
            }
            else
            {
                aTimer.Start();
            }

            ActiveCarousel = !ActiveCarousel;
        }

        private void ElementContainerControl_OnVideo(bool video, object parent)
        {
            OnVideo?.Invoke(video, parent);
        }

        private void ElementContainerControl_OnIOPortState(object sender, SidebarElementDTO element, IOPortState state)
        {
            OnIOPortState(sender, element, state);
        }

        private void ButtonBack_Click(object sender, EventArgs e)
        {
            OnClickBackButton?.Invoke(this);
        }

        private void ButtonDrag_Click(object sender, EventArgs e)
        {
            PanelElement.Paint -= Draw_Border;
            PanelElement.Paint += Remove_Border;

            PanelElement.Paint -= Remove_Border;
            Cursor.Current = Cursors.Default;

            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND);
            UnSelected();
            this.DoDragDrop(PanelElement, DragDropEffects.Move);
        }

        private void ElementContainerControl_ObjectOnClick(object sender, SidebarElementDTO element, object data)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    ElementContainerControl_ObjectOnClick(sender, element, data);
                });
                return;
            }

            ObjectChange?.Invoke(this, element, data);
        }

        private void ElementContainerControl_ObjectSelected(object sender, SidebarElementDTO element)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    ElementContainerControl_ObjectSelected(sender, element);
                });
                return;
            }

            var panelElement = PanelElement as Control;
            if (panelElement != null)
            {
                var swapButtonArray = panelElement.Controls.Find("swap", false);
                if (swapButtonArray.Length > 0)
                {
                    var swapButton = swapButtonArray[0] as BunifuImageButton;
                    if (swapButton != null && !swapButton.Visible)
                    {
                        swapButton.Visible = true;
                        swapButton.Location = new Point(this.Width - 25, 1);
                    }
                }
            }

            if (ObjectSelected != null)
            {
                if (carousels.Count > 0)
                {
                    ObjectSelected(this, originElement);
                }
                else
                {
                    ObjectSelected(this, element);
                }
            }
        }

        private void ElementContainerControl_ObjectSelectedDoubleClick(object sender)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    ElementContainerControl_ObjectSelectedDoubleClick(sender);
                });
                return;
            }

            var panelElement = PanelElement as Control;
            if (panelElement != null)
            {
                var swapButtonArray = panelElement.Controls.Find("swap", false);
                if (swapButtonArray.Length > 0)
                {
                    var swapButton = swapButtonArray[0] as BunifuImageButton;
                    if (swapButton != null)
                    {
                        swapButton.Visible = true;
                        swapButton.Location = new Point(this.Width - 25, 1);
                    }
                }
            }

            if (ObjectDoubleClickSelected != null)
            {
                if (carousels.Count > 0)
                {
                    ObjectDoubleClickSelected(this);
                }
                else
                {
                    ObjectDoubleClickSelected(this);
                }
            }
        }

        private void ElementContainerControl_ObjectOnDrag(object sender, string namePanel)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    ElementContainerControl_ObjectOnDrag(sender, namePanel);
                });
                return;
            }

            ObjectOnDrag?.Invoke(this, namePanel);
            //((PanelElement as Control).Controls.Find("swap", false)[0] as BunifuImageButton).Visible = true;
            //((PanelElement as Control).Controls.Find("swap", false)[0] as BunifuImageButton).Location = new Point(this.Width - 25, 1);

            //if (carousels.Count > 0)
            //    ObjectSelected(this, originElement);
            //else
            //    ObjectSelected(this, element);
        }

        public void UnSelected()
        {
            if ((PanelElement as Control).Controls.Find("swap", false).Count() > 0)
            {
                ((PanelElement as Control).Controls.Find("swap", false)[0] as BunifuImageButton).Visible = false;
            }
        }

        public void SwapButtonVisible()
        {
            if ((PanelElement as Control).Controls.Find("swap", false).Count() > 0)
            {
                ((PanelElement as Control).Controls.Find("swap", false)[0] as BunifuImageButton).Visible = true;
            }
        }

        private void AddButtonSwapContainer(string name, Image image = null)
        {
            if (image != null)
            {
                BunifuImageButton buttonBack = new BunifuImageButton
                {
                    BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_SELECTED),
                    Location = new Point(this.Width - 25, 1),
                    Size = new Size(24, 24),
                    Image = image,
                    Cursor = Cursors.Hand,
                    Name = name,
                    Visible = _showContextbar,
                    Anchor = (AnchorStyles.Top | AnchorStyles.Right)
                };
                buttonBack.MouseDown += ButtonDrag_Click;
                this.PanelElement.Controls.Add(buttonBack);
            }
        }

        private void AddButtonMasterCamara()
        {
            BunifuImageButton buttonBack = new BunifuImageButton
            {
                BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_SELECTED),
                Location = new Point(this.Width - 26, this.Height - 26),
                Size = new Size(24, 24),
                Image = FileResources.elipse,
                Cursor = Cursors.Hand,
                Name = "MasterCamara",
                Visible = false,
                Anchor = (AnchorStyles.Top | AnchorStyles.Right)
            };
            this.PanelElement.Controls.Add(buttonBack);
        }

        public void SetMasterCamaraVisivility(bool visivility)
        {
            if ((PanelElement as Control).Controls.Find("MasterCamara", false).Count() > 0)
            {
                ((PanelElement as Control).Controls.Find("MasterCamara", false)[0] as BunifuImageButton).Visible = visivility;
            }
        }

        private void Draw_Border(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            ControlPaint.DrawBorder(e.Graphics, panel.ClientRectangle, Color.Red, ButtonBorderStyle.Dashed);
        }

        private void Remove_Border(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;
            ControlPaint.DrawBorder(e.Graphics, panel.ClientRectangle, Color.Transparent, ButtonBorderStyle.Dashed);
        }

        private bool _sequencingMode;
        public bool IsSequencingMode
        {
            get
            {
                var cc = PanelElement.Controls[PanelElement.Controls.Count - 1];
                var driver = cc.Controls.OfType<IDriverLive>().FirstOrDefault();
                if (driver != null && driver.SequencingStatus != _sequencingMode)
                {
                    driver.ToogleSequencing(_sequencingMode);
                }
                return _sequencingMode;
            }
            set
            {
                _sequencingMode = value;
                var cc = PanelElement.Controls[PanelElement.Controls.Count - 1];
                var driver = cc.Controls.OfType<IDriverLive>().FirstOrDefault();
                if (driver != null)
                {
                    driver.ToogleSequencing(_sequencingMode);
                }
            }
        }
        public void SwapSequencingCamera(SidebarElementDTO sidebarElementDTO, object dto)
        {

            if (PanelElement.Controls.Count > 0)
            {
                var cc = PanelElement.Controls[PanelElement.Controls.Count - 1];
                var driver = cc.Controls.OfType<IDriverLive>().ToList();
                if (driver.Count > 0)
                {
                    driver[0].Dispose();
                }

                cc.Dispose();
            }

            LoadElement(sidebarElementDTO, dto as CameraDTO, _profile);

        }
    }
}
