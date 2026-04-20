using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using StreamCoders;
using System.Threading;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace Elipgo.SmartClient.Drivers.Rtsp.RtspPlayerControl
{
    public enum UIModeControl
    {
        None,
        DigitalZoom,
        PTZ
    }

    [GuidAttribute("A3E3F7BC-A44F-45C8-AA73-35F55F28E394")]
    [ComVisible(true)]
    [ProgId("RtspPlayerControl.RtspPlayerControl")]
    [ComSourceInterfaces(typeof(IRtspPlayerControlEvents))]
    public partial class RtspPlayerControl : UserControl
    {
        private RtspCameraCapture rtspCapture;
        private bool running = false;
        private PictureMediaBuffer currentFrame;

        private bool isFullscreen = false;
        private FullScreenForm fullScreenForm = new FullScreenForm();

        private bool saveToFile = false;
        private StreamCoders.Wave.WaveOutput audioDevice;

        private int zoom = 1;
        private Point currentOffsetPoint = new Point();
        private UIModeControl uiMode = UIModeControl.None;
        private System.Timers.Timer checkConnectionTimer;
        private long displayedVideoFrames = 0;
        private DateTime displayedVideoFramesTime = DateTime.UtcNow;
        private long receivedVideoFrames = 0;
        private DateTime receivedVideoFramesTime = DateTime.UtcNow;
        private long droppedVideoFrames = 0;

        private ConcurrentQueue<PictureMediaBuffer> frameQueue = new ConcurrentQueue<PictureMediaBuffer>();
        private Thread frameDequeuerThread;
        private readonly int frameQueueCapacity = 90;
        private bool isRunningFrameDequeuer = false;
        private string MediaProxyHost { get; set; }
        private string MediaProxyUsername { get; set; }
        private string MediaProxyPassword { get; set; }
        private bool MediaProxy { get; set; } = false;

        public RtspPlayerControl()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            // Set the control style to double buffer.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            zoom = 1;
            currentOffsetPoint = Point.Empty;
            NetworkTimeout = 15000;

            checkConnectionTimer = new System.Timers.Timer
            {
                Interval = 100
            };
            checkConnectionTimer.Elapsed += CheckConnectionTimer_Elapsed;
        }

        private void CheckConnectionTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (EnableReconnect)
            {
                if (rtspCapture.GetStatus() == CameraCaptureStatus.Disconnected || rtspCapture.GetStatus() == CameraCaptureStatus.Error)
                {
                    checkConnectionTimer.Stop();
                    this.Invoke(new Action(() => DoStart()));
                }
            }
        }

        #region RegisterCOM & UnregisterCOM
        [ComRegisterFunction()]
        public static void RegisterClass(string key)
        {
            // Strip off HKEY_CLASSES_ROOT\ from the passed key as I don't need it
            StringBuilder sb = new StringBuilder(key);
            sb.Replace(@"HKEY_CLASSES_ROOT\", "");

            // Open the CLSID\{guid} key for write access
            RegistryKey k = Registry.ClassesRoot.OpenSubKey(sb.ToString(), true);

            RegistryKey ctrl = k.CreateSubKey("Control");
            ctrl.Close();

            // Next create the CodeBase entry - needed if not string named and GACced.
            RegistryKey inprocServer32 = k.OpenSubKey("InprocServer32", true);
            inprocServer32.SetValue("CodeBase", Assembly.GetExecutingAssembly().CodeBase);
            inprocServer32.Close();

            // Finally close the main key
            k.Close();
        }

        [ComUnregisterFunction()]
        public static void UnregisterClass(string key)
        {
            StringBuilder sb = new StringBuilder(key);
            sb.Replace(@"HKEY_CLASSES_ROOT\", "");

            // Open HKCR\CLSID\{guid} for write access
            RegistryKey k = Registry.ClassesRoot.OpenSubKey(sb.ToString(), true);

            // Delete the 'Control' key, but don't throw an exception if it does not exist
            k.DeleteSubKey("Control", false);

            // Next open up InprocServer32
            RegistryKey inprocServer32 = k.OpenSubKey("InprocServer32", true);

            // And delete the CodeBase key, again not throwing if missing 
            k.DeleteSubKey("CodeBase", false);

            // Finally close the main key 
            k.Close();
        }
        #endregion

        [ComVisible(false)]
        public delegate void OnNewImageDelegate();
        private event OnNewImageDelegate OnNewImage;

        [ComVisible(false)]
        public delegate void OnErrorDelegate(int errorCode, string errorInfo);
        private event OnErrorDelegate OnError;

        [ComVisible(false)]
        public delegate void OnClickDelegate(int x, int y);
        public event OnClickDelegate OnClick;

        [ComVisible(false)]
        public delegate void OnKeyDownDelegate(int keyCode, long flags, bool handled);
        private event OnKeyDownDelegate OnKeyDown;

        [ComVisible(false)]
        public delegate void OnDoubleClickDelegate(int nButton, int nShiftState, int fX, int fY);
        private event OnDoubleClickDelegate OnDoubleClick;

        [ComVisible(false)]
        public delegate void OnMouseDownDelegate(int nButton, int nShiftState, int fX, int fY);
        private event OnMouseDownDelegate OnMouseDown;

        [ComVisible(false)]
        public delegate void OnMouseMoveDelegate(int nButton, int nShiftState, int fX, int fY);
        private event OnMouseMoveDelegate OnMouseMove;

        [ComVisible(false)]
        public delegate void OnMouseUpDelegate(int nButton, int nShiftState, int fX, int fY);
        private event OnMouseUpDelegate OnMouseUp;

        [ComVisible(false)]
        public delegate void OnMouseWheelDelegate(int nShiftState, int zDelta, int fX, int fY);
        private event OnMouseWheelDelegate OnMouseWheel;

        [ComVisible(true)]
        public bool EnableReconnect { get; set; }

        [ComVisible(true)]
        public long NetworkTimeout { get; set; }

        [ComVisible(true)]
        public string MediaURL { get; set; }

        [ComVisible(true)]
        public string MediaUsername { get; set; }

        [ComVisible(true)]
        public string MediaPassword { get; set; }

        [ComVisible(true)]
        public bool Mute { get; set; } = true;

        [ComVisible(true)]
        public string Version
        {
            get
            {
                AssemblyName name = Assembly.GetExecutingAssembly().GetName();
                Version ver = name.Version;
                return ver.Major + "," + ver.Minor + "," + ver.Build + "," + ver.Revision;
            }
        }

        [ComVisible(true)]
        public string UIMode
        {
            get
            {
                switch (uiMode)
                {
                    case UIModeControl.DigitalZoom:
                        return "digital-zoom";
                    case UIModeControl.PTZ:
                        return "ptz";
                    default:
                        return "none";
                }
            }
            set
            {
                if (value == "digital-zoom")
                {
                    this.Panel.Cursor = Cursors.Cross;
                    uiMode = UIModeControl.DigitalZoom;
                }
                else
                {
                    this.Panel.Cursor = Cursors.Default;
                    uiMode = UIModeControl.None;
                }
                currentOffsetPoint = Point.Empty;
                zoom = 1;
            }
        }

        [ComVisible(true)]
        public void SetProxy(string host, string username, string password)
        {
            MediaProxyHost = host;
            MediaProxyUsername = username;
            MediaProxyPassword = password;
            MediaProxy = true;
        }

        [ComVisible(true)]
        public void Play()
        {
            lock (this)
            {
                if (running)
                    return;
                DoStart();
                running = true;
            }
        }

        [ComVisible(true)]
        public void Stop()
        {
            lock (this)
            {
                DoStop();

                running = false;
            }
        }

        [ComVisible(true)]
        public bool SaveCurrentImage(int format, string path)
        {
            try
            {
                if (currentFrame == null)
                    return false;

                using (var bmp = StreamCoders.Imaging.ImageTools.Rgb24ToBitmap(currentFrame))
                {
                    if (format == 0)
                    {
                        bmp.Save(path, ImageFormat.Jpeg);
                    }
                    else
                    {
                        bmp.Save(path);
                    }
                    bmp.Dispose();
                }
                return true;
            }
            catch (Exception e)
            {
                OnErrorSend(e.Message);
                return false;
            }
        }

        [ComVisible(true)]
        public void FullScreen()
        {
            if (currentFrame == null)
                return;

            if (!isFullscreen)
            {
                if (fullScreenForm == null || fullScreenForm.IsDisposed)
                {
                    fullScreenForm = new FullScreenForm();
                }

                fullScreenForm.FormClosed += new FormClosedEventHandler(FullscreenFormClosed);
                fullScreenForm.Show();
                fullScreenForm.Activate();
                isFullscreen = true;
            }
            else
            {
                if (fullScreenForm != null && !fullScreenForm.IsDisposed)
                    fullScreenForm.Close();
                isFullscreen = false;
            }
        }

        [ComVisible(true)]
        public void StartRecordMedia(string fullPath)
        {
            if (saveToFile)
                return;

            if (rtspCapture != null)
                saveToFile = rtspCapture.StartRecordMedia(fullPath);
        }

        [ComVisible(true)]
        public void StopRecordMedia()
        {
            if (rtspCapture != null)
                rtspCapture.StopRecordMedia();
            saveToFile = false;
        }

        private void OnErrorSend(string message)
        {
            //OnError?.Invoke(0, message);

        }

        private void DoStart()
        {
            try
            {
                displayedVideoFrames = 0;

                rtspCapture = new RtspCameraCapture();
                rtspCapture.Init(Guid.NewGuid().ToString(), MediaURL, MediaUsername, MediaPassword, MediaProxy, MediaProxyHost, true, true);
                rtspCapture.NewVideoFrame += RtspCapture_NewVideoFrame;
                rtspCapture.NewAudioFrame += RtspCapture_NewAudioFrame;
                rtspCapture.Error += RtspCapture_Error;
                rtspCapture.Start();

                checkConnectionTimer.Start();
            }
            catch (Exception e)
            {
                OnErrorSend(e.Message);
            }
        }

        private void RtspCapture_Error(string error)
        {
            OnErrorSend(error);
        }

        private void DoStop()
        {
            try
            {
                if (fullScreenForm != null && !fullScreenForm.IsDisposed)
                    fullScreenForm.Close();

                checkConnectionTimer.Stop();

                if (rtspCapture != null)
                {
                    rtspCapture.Stop();
                    rtspCapture.NewVideoFrame -= RtspCapture_NewVideoFrame;
                    rtspCapture.NewAudioFrame -= RtspCapture_NewAudioFrame;
                    rtspCapture = null;
                }

                StopRecordMedia();

                if (currentFrame != null)
                    currentFrame = null;

                if (audioDevice != null)
                {
                    audioDevice.ResetDevice();
                    audioDevice.CloseDevice();
                    audioDevice.Dispose();
                    audioDevice = null;
                }

                isRunningFrameDequeuer = false;
                if (frameDequeuerThread != null) {
                    frameDequeuerThread.Join();
                    frameDequeuerThread = null;
                }
                while (!frameQueue.IsEmpty)
                    frameQueue.TryDequeue(out PictureMediaBuffer item);
            }
            catch (Exception e)
            {
                OnErrorSend(e.Message);
            }
        }

        public string GetStatus()
        {
            try
            {
                if (rtspCapture != null)
                    return rtspCapture.GetStatus().ToString();
                else
                    return CameraCaptureStatus.Disconnected.ToString();
            }
            catch
            {
                return CameraCaptureStatus.Disconnected.ToString();
            }
        }

        private void RenderVideoFrame(Panel panel, PictureMediaBuffer frame, bool isFullscreen)
        {
            if (panel.IsDisposed || !panel.IsHandleCreated)
                return;

            //panel.BeginInvoke(new Action(() => {
                using (Graphics g = Graphics.FromHwnd(panel.Handle))
                {
                    using (var pictureFrame = StreamCoders.Imaging.ImageTools.ToBitmap(frame))
                    {
                        using (var bmp = new Bitmap(pictureFrame, new Size(panel.Width, panel.Height)))
                        {
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            if (isFullscreen || uiMode != UIModeControl.DigitalZoom)
                            {
                                g.DrawImageUnscaled(bmp, 0, 0);
                            }
                            else
                            {
                                g.ScaleTransform(zoom, zoom);
                                g.DrawImageUnscaled(bmp, currentOffsetPoint);
                            }
                        }
                    }
                }
            //}));
        }

        private void RtspCapture_NewVideoFrame(PictureMediaBuffer frame)
        {
            if (frame == null)
                return;

            currentFrame = frame;
            if (isFullscreen)
            {
                RenderVideoFrame(this.fullScreenForm.Panel, frame, true);
            }
            else
            {
                RenderVideoFrame(this.Panel, frame, false);
            }
            return;

            if (frameDequeuerThread == null)
            {
                frameDequeuerThread = new Thread(FrameDequeuer);
                frameDequeuerThread.Start();
                isRunningFrameDequeuer = true;
            }

            if (frameQueue.Count > frameQueueCapacity)
            {
                while (frameQueue.Count > 2)
                    frameQueue.TryDequeue(out PictureMediaBuffer item);
            }
            frameQueue.Enqueue(frame);
        }

        private void RtspCapture_NewAudioFrame(AudioMediaBuffer frame)
        {
            if (Mute)
                return;

            if (audioDevice == null)
            {
                audioDevice = new StreamCoders.Wave.WaveOutput
                {
                    BitsPerSample = 16,
                    Channels = frame.Channels,
                    SampleRate = frame.Samplerate
                };
                if (audioDevice.Init() == false)
                {
                    return;
                }
                if (audioDevice.OpenDevice() == false)
                {
                    return;
                }
            }

            if (audioDevice.QueuedSamplesInSeconds > 1)
            {
                Thread.Sleep(200);
            }

            audioDevice.Enqueue(frame);
            audioDevice.UnprepareBuffers();
        }
        private void FrameDequeuer()
        {
            while (isRunningFrameDequeuer)
            {
                Thread.Sleep(30);
                try
                {
                    if (!frameQueue.TryDequeue(out PictureMediaBuffer frame)) continue;

                    currentFrame = frame;

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    if (isFullscreen)
                    {
                        RenderVideoFrame(this.fullScreenForm.Panel, frame, true);
                    }
                    else
                    {
                        RenderVideoFrame(this.Panel, frame, false);
                    }
                    stopwatch.Stop();

                    if (displayedVideoFrames == 0)
                    {
                        displayedVideoFramesTime = DateTime.UtcNow;
                    }

                    ++displayedVideoFrames;
                    double fps = (displayedVideoFrames) / (DateTime.UtcNow - displayedVideoFramesTime).TotalSeconds;

                    if (displayedVideoFrames < long.MaxValue)
                    {
                        /*
                        if (!fpsLabel.IsHandleCreated)
                            return;
                        fpsLabel.BeginInvoke(new Action(() => {
                            fpsLabel.Text = string.Format("Displayed {0}\n{1:0.00} fps {2}x{3}\nElapsed time {4:00}.{5:0000}\nQueue count {6}", 
                                displayedVideoFrames, fps, currentFrame.Width, currentFrame.Height, stopwatch.Elapsed.Seconds, stopwatch.Elapsed.TotalMilliseconds, frameQueue.Count);
                        }));
                        */
                    }
                    else
                        displayedVideoFrames = 0;

                }
                catch (Exception) { }
            }
        }

        private void FullscreenFormClosed(object sender, FormClosedEventArgs e)
        {
            isFullscreen = false;
        }

        private void PanelMouseClick(object sender, MouseEventArgs e)
        {
            OnClick?.Invoke(e.X, e.Y);
        }

        private void PanelMouseDoubleClick(object sender, MouseEventArgs e)
        {
            OnDoubleClick?.Invoke(0, 0, e.X, e.Y);
        }

        private void PanelMouseMove(object sender, MouseEventArgs e)
        {
            if (uiMode == UIModeControl.PTZ)
            {
                OnMouseMove?.Invoke(0, 0, e.X, e.Y);
            }
        }

        private void PanelMouseWheel(object sender, MouseEventArgs e)
        {
            Point mouseLocation = e.Location;
            if (uiMode == UIModeControl.DigitalZoom)
            {
                float oldZoom = zoom;
                if (e.Delta > 0)
                {
                    zoom++;
                    if (zoom >= 8) zoom = 8;
                }
                else if (e.Delta < 0)
                {
                    zoom--;
                    if (zoom <= 1) zoom = 1;
                }

                int x = mouseLocation.X - this.Panel.Location.X;
                int y = mouseLocation.Y - this.Panel.Location.Y;
                int oldImageX = (int)(x / oldZoom);
                int oldImageY = (int)(y / oldZoom);
                int newImageX = (int)(x / zoom);
                int newImageY = (int)(y / zoom);

                currentOffsetPoint.X = newImageX - oldImageX + currentOffsetPoint.X;
                currentOffsetPoint.Y = newImageY - oldImageY + currentOffsetPoint.Y;
            }
            else if (uiMode == UIModeControl.PTZ)
            {
                OnMouseWheel?.Invoke(0, 0, e.X, e.Y);
            }
        }

        private void PanelMouseDown(object sender, MouseEventArgs e)
        {
            if (uiMode == UIModeControl.PTZ)
            {
                int nButton = 0;
                int nShiftState = 0;
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        nButton = 1;
                        break;
                    case MouseButtons.Right:
                        nButton = 2;
                        break;
                    case MouseButtons.Middle:
                        nButton = 16;
                        break;
                    default:
                        break;
                }

                OnMouseDown?.Invoke(nButton, nShiftState, e.X, e.Y);
            }
        }

        private void PanelMouseUp(object sender, MouseEventArgs e)
        {
            if (uiMode == UIModeControl.PTZ)
            {
                OnMouseUp?.Invoke(0, 0, e.X, e.Y);
            }
        }

        private void PanelKeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown?.Invoke(e.KeyValue, 0, false);
        }


        private void PanelClick(object sender, EventArgs e)
        {
            OnClick?.Invoke(MousePosition.X, MousePosition.Y);
        }
    }
}
