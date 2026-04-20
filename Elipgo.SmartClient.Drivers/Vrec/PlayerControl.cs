using DVRPlay;
using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Elipgo.SmartClient.Drivers.Vrec.Player
{
    /// <summary>
    /// Modo de zoom de imagen.
    /// </summary>
    public enum ZoomModeEnum
    {
        /// <summary>
        /// Indica que la imagen se ajustará al tamaño del control.
        /// </summary>
        Automatic,
        /// <summary>
        /// Determina un factor fijo de zoom (modificable con propiedad <c>Zoom</c>.
        /// </summary>
        Fixed
    };

    /// <summary>
    /// Control de reproducción de video via servicio web.
    /// </summary>
    public partial class PlayerControl4 : System.Windows.Forms.UserControl
    {

        #region Attributes

        private VideoClient client;
        private string cameraId = "";
        public delegate void PlayerErrorHandler(string errorMsg);

        // NOTA: currFrame se actualiza constantemente por un thread aparte los accesos a la instancia deben estar sincronizados
        private Bitmap currFrame = null;    // cuadro de video visualizado
        private Rectangle frameRect;    // rectángulo dentro del control correspondiente al cuadro de video actual
        private bool tracking = false;
        private bool error = false;
        private string errorMsg = "";
        private ZoomModeEnum zoomMode = ZoomModeEnum.Automatic;

        private int zoom = 100;   // porcentaje (100 = tamaño original)
        private int zoomX = 0;
        private int zoomY = 0;
        private int zoomOverX = 0;
        private int zoomOverY = 0;
        private int zoomStepX = 25;
        private int zoomStepY = 25;
        public bool ZoomStatus { get; set; }

        private bool showTitleBar = false;
        private bool showStatusBar = false;
        private bool showTrackBar = false;
        private bool showConnectionStatus = false;
        private bool showLabel = false;
        private bool showSmartSearchOverlay = false;
        private bool showControlStatusBar = false;

        private enum WindowArea { TitleBar, Frame, StatusBar, TrackBar };

        private const int titleBarHeight = 16;
        private const int statusBarHeight = 16;
        private const int trackBarHeight = 16;
        private const int iconWidth = 16;

        private const int timelineCodeCount = 3;
        private byte[] timeline = null;
        private Brush[] timelineBrush;
        private DateTime lastTimestamp = new DateTime(1970, 1, 1);      // para actualización de linea de tiempo

        private bool smartSearchMode = false;
        private Rectangle smartSearchArea;
        private int smartSearchThDelta;
        private int smartSearchThVol;
        private Color smartSearchOverlayColor;
        private Color smartSearchAreaColor;
        private bool _autoDetectProxy = false;

        private H264Decoder.Decoder_H264 vdec;

        public event EventHandler PlayerSelected;
        public event PlayerErrorHandler ErrorHandler;
        private struct RoiEditStatus
        {
            public bool left;
            public bool top;
            public bool right;
            public bool bottom;
        }

        private RoiEditStatus roiEditStatus;

        private bool flagsZeroFrame = false;
        private bool flagsTooManyUser = false;
        private Common.Enum.VRecLastOperationResult lastError;



        /// <summary>
        /// Puerto Alternativo de transmicion
        /// </summary>
        private int portTransfer = -1;

        #endregion

        #region Constructors

        /// <summary>
        /// Instancia un nuevo control de reproducción de video.
        /// </summary>
        /// 
        public PlayerControl4()
        {
            try
            {
                ActivateWS();
                SetTimelineBrush();
                SetSmartSearchConfigure();
                SetStyle(ControlStyles.Selectable | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
                UpdateStyles();
                this.Click += PlayerControl4_Click;
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
          
        }

        private void PlayerControl4_Click(object sender, EventArgs e)
        {
            try
            {
                PlayerSelected?.Invoke(sender, e);

                if (ZoomStatus)
                {
                    if (frameRect.Location.X < 0)
                    {
                        var mouse = new Point(Cursor.Position.X - this.PointToScreen(Point.Empty).X, Cursor.Position.Y - this.PointToScreen(Point.Empty).Y);
                        var mp = new Point((100 * mouse.X) / this.Width, (100 * mouse.Y) / this.Height);
                        var p = new Point((frameRect.Width * mp.X) / 100, (frameRect.Height * mp.Y) / 100);
                        var picPosition = new Point((mouse.X - p.X), (mouse.Y - p.Y));
                        frameRect.Location = picPosition;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
            
        }

        #endregion

        #region Implements

        /// <summary>
        /// Cierra la conexión con el servidor de video al liberar los recursos.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    this.Close();
                }

                base.Dispose(disposing);
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }

        }

        /// <summary>
        /// Procesa el evento <c>Paint</c> (dibuja el control).
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            try
            {
                base.OnPaint(pe);
                pe.Graphics.FillRectangle(Brushes.Black, 0, 0, this.Width, this.Height);
                Draw(pe.Graphics);
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
                throw new Exception(Resources.VRecOnPaintError);
            }
        }

        /// <summary>
        /// Procesa el evento <c>Resize</c> (cambia tamaño del control).
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            try
            {
                UpdateDestinationRectangle();
                base.OnResize(e);
            }
            catch
            { }
        }

        /// <summary>
        /// Procesa el evento <c>MouseDown</c>.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            try
            {
                this.Focus();

                if (e.Button == MouseButtons.Right)
                {
                    return;
                }

                WindowArea area = MouseToArea(e);

                if (e.Button == MouseButtons.Middle)
                {
                    if (area == WindowArea.Frame && zoomMode == ZoomModeEnum.Fixed)
                    {
                        zoom = 100;
                        UpdateDestinationRectangle();
                        this.Invalidate();
                    }
                    else if (area == WindowArea.StatusBar && e.X >= 5 * iconWidth && e.X < 7 * iconWidth)
                    {
                        this.SetSkipRate(Math.Sign(client.GetSkipRate()));
                    }

                    return;
                }

                if (e.Button == MouseButtons.Left)
                {
                    if ((area == WindowArea.StatusBar) && this.ShowControlStatusBar)
                    {
                        int skipRate = client.GetSkipRate();
                        if (e.X < iconWidth)
                        {
                            this.Start();
                        }
                        else if (e.X < 2 * iconWidth)
                        {
                            this.Pause();
                        }
                        else if (e.X < 3 * iconWidth)
                        {
                            this.Stop();
                        }
                        else if (e.X >= 4 * iconWidth && e.X < 5 * iconWidth)
                        {
                            this.SetSkipRate(skipRate == +1 ? -1 : skipRate - 1);
                        }
                        else if (e.X >= 5 * iconWidth && e.X < 7 * iconWidth)
                        {
                            this.SetSkipRate(-skipRate);
                        }
                        else if (e.X >= 7 * iconWidth && e.X < 8 * iconWidth)
                        {
                            this.SetSkipRate(skipRate == -1 ? +1 : skipRate + 1);
                        }
                        // TODO : mensaje de error deshabilitado
                        /*
						else if( error )
							MessageBox.Show( errorMsg );
						*/
                    }
                    else if (area == WindowArea.TrackBar)
                    {
                        this.SetTimeOffset((e.X * 3600) / this.Width);
                        tracking = true;
                    }
                    else if (smartSearchMode && area == WindowArea.Frame && frameRect.Contains(e.X, e.Y))
                    {
                        Rectangle r = SmartSearchAreaToControlRect(smartSearchArea);
                        if (Math.Abs(r.Left - e.X) < 5)
                        {
                            roiEditStatus.left = true;
                        }
                        else if (Math.Abs(r.Right - e.X) < 5)
                        {
                            roiEditStatus.right = true;
                        }

                        if (Math.Abs(r.Top - e.Y) < 5)
                        {
                            roiEditStatus.top = true;
                        }
                        else if (Math.Abs(r.Bottom - e.Y) < 5)
                        {
                            roiEditStatus.bottom = true;
                        }

                        if (!roiEditStatus.left && !roiEditStatus.right && !roiEditStatus.top && !roiEditStatus.bottom)
                        {
                            smartSearchArea = ControlRectToSmartSearchArea(new Rectangle(e.X, e.Y, 0, 0));
                            roiEditStatus.left = false;
                            roiEditStatus.bottom = false;
                            roiEditStatus.right = true;
                            roiEditStatus.bottom = true;
                        }
                    }

                    if (area == WindowArea.Frame && zoomMode == ZoomModeEnum.Fixed)
                    {
                        this.ZoomView(e);
                        UpdateDestinationRectangle();
                    }
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Procesa el evento <c>MouseUp</c>.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            try
            {
                tracking = false;

                if (e.Button == MouseButtons.Left && smartSearchMode)
                {
                    roiEditStatus.left = false;
                    roiEditStatus.top = false;
                    roiEditStatus.right = false;
                    roiEditStatus.bottom = false;

                    // cambiar la zona de busqueda inteligente en el servidor
                    client.SetSmartSearchArea(smartSearchArea.Left, smartSearchArea.Top, smartSearchArea.Width, smartSearchArea.Height);
                }

            }
            catch
            { }
        }

        /// <summary>
        /// Procesa el evento <c>MouseMove</c>.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            try
            {
                if (tracking)
                {
                    double seconds = (e.X * 3600) / this.Width;
                    if (seconds >= 0 && seconds < 3600)
                    {
                        this.SetTimeOffset(seconds);
                    }
                }
                else if (smartSearchMode)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        lock (this)
                        {
                            if (currFrame != null)
                            {
                                int x = ClipValue(e.X, frameRect.Left, frameRect.Right);
                                int y = ClipValue(e.Y, frameRect.Top, frameRect.Bottom);
                                if (roiEditStatus.left)
                                {
                                    int left = ControlToSmartSearchX(x);
                                    int right = smartSearchArea.Right;
                                    // ajustar para caso negativo
                                    if (left > right)
                                    {
                                        int t = left;
                                        left = right;
                                        right = t;
                                        roiEditStatus.left = false;
                                        roiEditStatus.right = true;
                                    }
                                    smartSearchArea.X = left;
                                    smartSearchArea.Width = right - left;
                                }
                                else if (roiEditStatus.right)
                                {
                                    int left = smartSearchArea.Left;
                                    int right = ControlToSmartSearchX(x);
                                    // ajustar para caso negativo
                                    if (left > right)
                                    {
                                        int t = left;
                                        left = right;
                                        right = t;
                                        roiEditStatus.left = true;
                                        roiEditStatus.right = false;
                                    }
                                    smartSearchArea.X = left;
                                    smartSearchArea.Width = right - left;
                                }
                                if (roiEditStatus.top)
                                {
                                    int top = ControlToSmartSearchY(y);
                                    int bottom = smartSearchArea.Bottom;
                                    // ajustar para caso negativo
                                    if (top > bottom)
                                    {
                                        int t = top;
                                        top = bottom;
                                        bottom = t;
                                        roiEditStatus.top = false;
                                        roiEditStatus.bottom = true;
                                    }
                                    smartSearchArea.Y = top;
                                    smartSearchArea.Height = bottom - top;
                                }
                                else if (roiEditStatus.bottom)
                                {
                                    int top = smartSearchArea.Top;
                                    int bottom = ControlToSmartSearchY(y);
                                    // ajustar para caso negativo
                                    if (top > bottom)
                                    {
                                        int t = top;
                                        top = bottom;
                                        bottom = t;
                                        roiEditStatus.top = true;
                                        roiEditStatus.bottom = false;
                                    }
                                    smartSearchArea.Y = top;
                                    smartSearchArea.Height = bottom - top;
                                }
                                Graphics g = this.CreateGraphics();
                                Draw(g);
                                g.Dispose();
                            }
                        }
                    }
                    else if (e.Button == MouseButtons.None)
                    {
                        Rectangle r = SmartSearchAreaToControlRect(smartSearchArea);
                        bool left = false;
                        bool top = false;
                        bool right = false;
                        bool bottom = false;
                        if (e.Y >= r.Top - 5 && e.Y <= r.Bottom + 5)
                        {
                            if (Math.Abs(r.Left - e.X) < 5)
                            {
                                left = true;
                            }
                            else if (Math.Abs(r.Right - e.X) < 5)
                            {
                                right = true;
                            }
                        }
                        if (e.X >= r.Left - 5 && e.X <= r.Right + 5)
                        {
                            if (Math.Abs(r.Top - e.Y) < 5)
                            {
                                top = true;
                            }
                            else if (Math.Abs(r.Bottom - e.Y) < 5)
                            {
                                bottom = true;
                            }
                        }
                        if (left)
                        {
                            if (top)
                            {
                                this.Cursor = Cursors.SizeNWSE;
                            }
                            else if (bottom)
                            {
                                this.Cursor = Cursors.SizeNESW;
                            }
                            else
                            {
                                this.Cursor = Cursors.SizeWE;
                            }
                        }
                        else if (right)
                        {
                            if (top)
                            {
                                this.Cursor = Cursors.SizeNESW;
                            }
                            else if (bottom)
                            {
                                this.Cursor = Cursors.SizeNWSE;
                            }
                            else
                            {
                                this.Cursor = Cursors.SizeWE;
                            }
                        }
                        else if (top || bottom)
                        {
                            this.Cursor = Cursors.SizeNS;
                        }
                        else
                        {
                            this.Cursor = Cursors.Default;
                        }
                    }
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Procesa el evento <c>MouseWheel</c>.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            try
            {
                WindowArea area = MouseToArea(e);
                if (area == WindowArea.TrackBar)
                {
                    this.SetTimeOffset(client.GetTimeOffset() + 15 * e.Delta / 120);
                }
                else if (area == WindowArea.StatusBar && e.X >= 4 * iconWidth && e.X < 8 * iconWidth)
                {
                    int delta = e.Delta / 120;
                    int skipRate = client.GetSkipRate();
                    if (delta < 0)
                    {
                        this.SetSkipRate(skipRate == +1 ? -1 : skipRate - 1);
                    }
                    else if (delta > 0)
                    {
                        this.SetSkipRate(skipRate == -1 ? +1 : skipRate + 1);
                    }
                }
                else if (area == WindowArea.Frame && zoomMode == ZoomModeEnum.Fixed)
                {
                    zoom = (int)(zoom * Math.Exp(0.1 * (e.Delta / 120)));
                    if (zoom < 10)
                    {
                        zoom = 10;
                    }
                    else if (zoom > 1000)
                    {
                        zoom = 1000;
                    }

                    this.ZoomView(e);

                    UpdateDestinationRectangle();
                    this.Invalidate();
                }
                else if (ZoomStatus)
                {
                    zoom = (int)(zoom * Math.Exp(0.1 * (e.Delta / 120)));
                    if (zoom < 10)
                    {
                        zoom = 10;
                    }
                    else if (zoom > 1000)
                    {
                        zoom = 1000;
                    }

                    this.ZoomView(e);

                    if (currFrame == null)
                    {
                        return;
                    }

                    int frameWidth = this.Width;
                    int frameHeight = GetFrameHeight();

                    Rectangle dest = new Rectangle(Point.Empty, currFrame.Size);
                    int top = GetBarTop(WindowArea.Frame);


                    var dW = (dest.Width * zoom) / 100;
                    var dH = (dest.Height * zoom) / 100;

                    dest.Width = (dW <= this.Width || dH <= this.Height ? this.Width : dW);
                    dest.Height = (dH <= this.Height || dest.Width == this.Width ? this.Height : dH);

                    var dX = (frameWidth - dest.Width) / 2 - zoomX;
                    var dY = top + (frameHeight - dest.Height) / 2 - zoomY;

                    dest.X = (dX > 0 ? 0 : dX);
                    dest.Y = (dY > 0 || dX == 0 ? 0 : dY);
                    //dest.X = dX - zoomX;
                    //dest.Y = dY - zoomY;

                    var zOX = (frameWidth - dest.Width) / 2;
                    var zOY = (frameHeight - dest.Height) / 2;

                    this.zoomOverX = (e.Delta < 0 ? 0 : zOX);
                    this.zoomOverY = (e.Delta < 0 ? 0 : zOY);
                    //this.zoomOverX = zOX;
                    //this.zoomOverY = zOY;

                    frameRect = dest;
                    this.Invalidate();
                }
            }
            catch
            { }
        }

        #endregion

        #region Private Methods

        private void SetTimelineBrush()
        {
            try
            {
                timelineBrush = new Brush[timelineCodeCount];
                timelineBrush[0] = new SolidBrush(Color.FromArgb(32, 32, 32));
                timelineBrush[1] = new SolidBrush(Color.FromArgb(0, 192, 0));
                timelineBrush[2] = new SolidBrush(Color.FromArgb(255, 255, 0));
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
            
        }

        private void SetSmartSearchConfigure()
        {
            try
            {
                smartSearchOverlayColor = Color.FromArgb(255, 0, 0);
                smartSearchAreaColor = Color.FromArgb(0, 255, 0);
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
           
        }

        private int GetStringWidth(string input)
        {
            try
            {
                Graphics g = CreateGraphics();
                Font font = new Font(FontFamily.GenericSansSerif, 8, FontStyle.Regular);
                SizeF size = g.MeasureString(input, font);
                g.Dispose();
                return (int)size.Width;
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
                return 0;
            }
            
        }

        private void CreatePath(ref GraphicsPath gp, ref Rectangle rec)
        {
            try
            {
                if (client.GetCameraName() != string.Empty)
                {
                    int ancho = GetStringWidth(this.cameraId + " - " + client.GetCameraName());
                    int alto = 15;
                    int left = (int)((this.Width / 2) - (ancho / 2));

                    rec = new Rectangle(left, 0, ancho, alto);

                    //Construye la region paralelogramo para el titulo
                    Point[] pts = new Point[4];
                    pts[0] = new Point(left - 7, 0);
                    pts[1] = new Point(ancho + left + 8, 0);
                    pts[2] = new Point(ancho + left, alto);
                    pts[3] = new Point(left, alto);
                    byte[] types = new byte[4];
                    types[0] = (byte)PathPointType.Line;
                    types[1] = (byte)PathPointType.Line;
                    types[2] = (byte)PathPointType.Line;
                    types[3] = (byte)PathPointType.Line;

                    gp = new GraphicsPath(pts, types, FillMode.Winding);
                    gp.CloseFigure();
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
         
        }

        private void ClipGraphic(Graphics g)
        {
            try
            {
                GraphicsPath gp = null;
                Rectangle rect = new Rectangle();
                CreatePath(ref gp, ref rect);
                g.SetClip(gp, CombineMode.Exclude);
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
          
        }

        private void Draw(Graphics g)
        {
            try
            {
                lock (this)
                {
                    if (this.Width == 0 || this.Height == 0)
                    {
                        return;
                    }

                    int frameWidth = this.Width;
                    int frameHeight = GetFrameHeight();

                    ClientStatus status = client.GetStatus();

                    int skipRate = client.GetSkipRate();
                    int currTimeOffset = client.GetTimeOffset();

                    //Si hay que pintar el titulo, recorto del grafico, la supercie del titulo
                    if ((this.showLabel) && (client.GetCameraName() != string.Empty))
                    {
                        ClipGraphic(g);
                    }

                    #region Draw Image

                    // dibujar cuadro de video
                    if (currFrame != null && frameWidth > 0 && frameHeight > 0)
                    {
                        //int top = getBarTop(WindowArea.Frame);
                        //g.SetClip( new Rectangle( 0, top, frameWidth, frameHeight ) );
                        g.DrawImage(currFrame, frameRect);
                        // dibujar area de búsqueda inteligente dentro del cuadro
                        if (smartSearchMode)
                        {
                            Rectangle rect = SmartSearchAreaToControlRect(smartSearchArea);
                            g.DrawRectangle(new Pen(smartSearchAreaColor), rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
                        }
                        g.ResetClip();
                    }

                    #endregion

                    #region MAX Users
                    // Dibuja el max de users
                    if (flagsTooManyUser == true)
                    {
                        Font font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);

                        g.DrawString("Too Many Users.", font, Brushes.Red, frameWidth / 2 - 65, frameHeight / 2);
                    }
                    #endregion

                    //Si hay que pintar el titulo, restablezco el grafico
                    if ((this.showLabel) && (client.GetCameraName() != string.Empty))
                    {
                        g.ResetClip();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
           
        }

        /// <summary>
        /// Genera el offset
        /// </summary>
        /// <param name="e">Argumentos de cordenadas</param>
        private void ZoomView(MouseEventArgs e)
        {
            try
            {
                int x = e.X;
                int restX = this.Width / 2 - x;
                if (restX < 0)
                {
                    this.zoomX = this.zoomX + zoomStepX;
                }
                else
                {
                    this.zoomX = this.zoomX - zoomStepX;
                }

                int y = e.Y;
                int restY = y - this.Height / 2;
                if (restY < 0)
                {
                    this.zoomY = this.zoomY - zoomStepY;
                }
                else
                {
                    this.zoomY = this.zoomY + zoomStepY;
                }

                this.zoomOverX = Math.Abs(this.zoomOverX);
                if (Math.Abs(this.zoomX) > this.zoomOverX)
                {
                    this.zoomX = Math.Sign(this.zoomX) * this.zoomOverX;
                }

                this.zoomOverY = Math.Abs(this.zoomOverY);
                if (Math.Abs(this.zoomY) > this.zoomOverY)
                {
                    this.zoomY = Math.Sign(this.zoomY) * this.zoomOverY;
                }

                this.Invalidate();
            }
            catch
            { }
        }

        private void ActivateWS()
        {
            try
            {
                if (client == null)
                {
                    client = new VideoClient();
                    client.OnNewFrame += new newFrameHandler(ClientNewFrame);
                    client.OnReloadTimeline += new reloadTimelineHandler(ClientReloadTimeline);
                    client.OnStop += new stopHandler(ClientStop);
                    client.OnError += new errorHandler(ClientError);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
        }

        void ClientReloadTimeline()
        {
            try
            {
                this.timeline = this.client.GetTimelineReload();
            }
            catch (Exception ex10)
            {
                Logger.Log($"Message {ex10.Message} StackTrace {ex10.StackTrace}");
            }
            
        }

        void ClientStop()
        {
            try
            {
                this.Invalidate();
            }
            catch (Exception ex30)
            {

                Logger.Log($"Message 30 {ex30.Message} StackTrace {ex30.StackTrace}");
            }
            
        }

        private void UpdateDestinationRectangle()
        {
            // recalcula el rectángulo correspondiente a la imagen actual dentro del 
            // control, teniendo en cuenta la modalidad y factor de zoom
            try
            {
                lock (this)
                {
                    if (currFrame == null)
                    {
                        return;
                    }

                    int frameWidth = this.Width;
                    int frameHeight = GetFrameHeight();

                    Rectangle dest = new Rectangle(Point.Empty, currFrame.Size);

                    if (zoomMode == ZoomModeEnum.Automatic)
                    {
                        if (currFrame.Width > 0 && currFrame.Height > 0)
                        {
                            double aspect = (double)currFrame.Height / (double)currFrame.Width;

                            dest.Width = frameWidth;
                            //dest.Height = (int)(frameWidth * aspect);
                            dest.Height = frameHeight;
                            if (dest.Height > frameHeight)
                            {
                                dest.Height = frameHeight;
                                //dest.Width = (int)(frameHeight / aspect);
                            }

                            dest.X = (frameWidth - dest.Width) / 2;
                            dest.Y = GetBarTop(WindowArea.Frame) + (frameHeight - dest.Height) / 2;
                        }
                    }
                    else
                    {
                        // notar que en este caso el area destino puede ser superior al area
                        // del control; el gráfico debe utilizar clipping
                        //					int top = getBarTop(WindowArea.Frame);
                        //					dest.Width = ( dest.Width * zoom ) / 100;
                        //					dest.Height = ( dest.Height * zoom ) / 100;
                        //					dest.X = (frameWidth - dest.Width) / 2; 
                        //					dest.Y = top + (frameHeight - dest.Height) / 2;


                        // notar que en este caso el area destino puede ser superior al area
                        // del control; el gráfico debe utilizar clipping
                        int top = GetBarTop(WindowArea.Frame);

                        dest.Width = (dest.Width * zoom) / 100;
                        dest.Height = (dest.Height * zoom) / 100;

                        //dest.X = (frameWidth - dest.Width) / 2; 
                        //dest.Y = top + (frameHeight - dest.Height) / 2;

                        dest.X = (frameWidth - dest.Width) / 2 - zoomX;
                        dest.Y = top + (frameHeight - dest.Height) / 2 - zoomY;

                        this.zoomOverX = (frameWidth - dest.Width) / 2;
                        this.zoomOverY = (frameHeight - dest.Height) / 2;
                    }

                    frameRect = dest;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
            
        }

        private int GetBarTop(WindowArea area)
        {
            try
            {
                // retorna el inicio de un area determinada, considerando las areas 
                // visibles e invisibles
                // el orden de visualización (de arriba hacia abajo) es fijo:
                // 1) barra de titulo, 2) cuadro de imagen, 3) barra de estado, 4) barra de desplazamiento
                // las barras son de altura fija, en tanto que la altura del cuadro de imagen se 
                // adapta de acuerdo a las barras visibles
                switch (area)
                {
                    case WindowArea.TitleBar:
                        return 0;
                    case WindowArea.Frame:
                        if (!showTitleBar)
                        {
                            return 0;
                        }
                        else
                        {
                            return titleBarHeight;
                        }

                    case WindowArea.StatusBar:
                        if (!showTrackBar)
                        {
                            return this.Height - statusBarHeight;
                        }
                        else
                        {
                            return this.Height - trackBarHeight - statusBarHeight;
                        }

                    case WindowArea.TrackBar:
                        return this.Height - trackBarHeight;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
            
            return 0;
        }

        private int GetFrameHeight()
        {
            // retorna la altura en pixels del cuadro de imagen, considerando las 
            // barras adicionales que se muestren
            int frameHeight = this.Height;
            try
            {
                if (showTitleBar)
                {
                    frameHeight -= titleBarHeight;
                }

                if (showStatusBar)
                {
                    frameHeight -= statusBarHeight;
                }

                if (showTrackBar)
                {
                    frameHeight -= trackBarHeight;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
          

            return frameHeight;
        }

        private WindowArea MouseToArea(MouseEventArgs e)
        {
            try
            {  // determina el area de la ventana correspondiente a un evento del mouse
                if (showTitleBar)
                {
                    int y = e.Y - GetBarTop(WindowArea.TitleBar);
                    if (y >= 0 && y < titleBarHeight)
                    {
                        return WindowArea.TitleBar;
                    }
                }
                if (showStatusBar)
                {
                    int y = e.Y - GetBarTop(WindowArea.StatusBar);
                    if (y >= 0 && y < statusBarHeight)
                    {
                        return WindowArea.StatusBar;
                    }
                }
                if (showTrackBar)
                {
                    int y = e.Y - GetBarTop(WindowArea.TrackBar);
                    if (y >= 0 && y < trackBarHeight)
                    {
                        return WindowArea.TrackBar;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
          
            return WindowArea.Frame;
        }

        private int ClipValue(int x, int x1, int x2)
        {
            try
            {
                if (x < x1)
                {
                    return x1;
                }

                if (x > x2)
                {
                    return x2;
                }
            }
            catch (Exception ex)
            {

                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
           

            return x;
        }

        private void RenderVideo(byte[] data)
        {
            try
            {
                // este método se invoca desde otro thread
                lock (this)
                {

                    // verificar cambio de hora para actualizar la linea de tiempo
                    DateTime currTimestamp = client.GetTimestamp();
                    if (currTimestamp.Year != lastTimestamp.Year ||
                        currTimestamp.Month != lastTimestamp.Month ||
                        currTimestamp.Day != lastTimestamp.Day ||
                        currTimestamp.Hour != lastTimestamp.Hour)
                    {
                        timeline = client.GetTimelineReload();
                    }

                    int w = 0;
                    int h = 0;
                    if (currFrame != null)
                    {
                        w = currFrame.Width;
                        h = currFrame.Height;
                        currFrame.Dispose();
                        currFrame = null;
                    }

                    Bitmap bmpTemp = null;
                    bool imageOk = true;

                    try
                    {
                        if (vdec == null) // FIXME: 
                        {
                            return;
                        }

                        MemoryStream msIn = new MemoryStream();
                        msIn.Write(data, 0, data.Length);
                        msIn.Position = 0;
                        Frame framein = new Frame(msIn, VideoCodecType.H264.ToString(), true);
                        Frame frameout = vdec.process(framein);
                        if (frameout == null)
                        {
                            throw new Exception("Null Decode Frame");
                        }

                        MemoryStream msOut = frameout.dataStream;
                        byte[] rawData = new byte[msOut.Length];
                        msOut.Read(rawData, 0, rawData.Length);

                        bmpTemp = CreateBitmapFromRawBytes(rawData, frameout.width, frameout.height);

                    }
                    catch (Exception)
                    {
                        imageOk = false;
                    }

                    if (imageOk)
                    {
                        if (bmpTemp == null)
                        {
                            return;
                        }

                        //Copio la imagen nueva  a currentFrame
                        currFrame = (Bitmap)bmpTemp.Clone();
                        bmpTemp.Dispose();

                        // si cambió el tamaño del cuadro repintar todo el control, sino
                        // solamente dibujar la imagen
                        if (currFrame.Width != w || currFrame.Height != h)
                        {
                            UpdateDestinationRectangle();
                            this.Invalidate();
                        }
                        else
                        {
                            Graphics g = this.CreateGraphics();
                            Draw(g);
                            g.Dispose();
                        }
                    }
                    lastTimestamp = currTimestamp;
                }
            }
            catch (Exception ex)
            {

                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
            
        }

        private Rectangle SmartSearchAreaToControlRect(Rectangle area)
        {
            // retorna el rectángulo dentro del control correspondiente al area de busqueda
            // inteligente (coordenadas entre 0 y 100, porcentaje respecto del tamaño
            // del cuadro actual)
            int x = frameRect.X + (frameRect.Width * area.X) / 100;
            int y = frameRect.Y + (frameRect.Height * area.Y) / 100;
            int w = (frameRect.Width * area.Width) / 100;
            int h = (frameRect.Height * area.Height) / 100;
            return new Rectangle(x, y, w, h);
        }

        private Rectangle ControlRectToSmartSearchArea(Rectangle rect)
        {
            // retorna el area de busqueda inteligente (coordenadas entre 0 y 100)
            // correspondiente a un rectángulo dibujado en el control
            int x = (100 * (rect.X - frameRect.X)) / frameRect.Width;
            int y = (100 * (rect.Y - frameRect.Y)) / frameRect.Height;
            int w = (rect.Width * 100) / frameRect.Width;
            int h = (rect.Height * 100) / frameRect.Height;
            return new Rectangle(x, y, w, h);
        }

        private int ControlToSmartSearchX(int x)
        {
            // convierte una coordenada X dentro del control a un porcentaje (0 a 100)
            // para el area de busqueda inteligente
            return (100 * (x - frameRect.X)) / frameRect.Width;
        }

        private int ControlToSmartSearchY(int y)
        {
            // convierte una coordenada Y dentro del control a un porcentaje (0 a 100)
            // para el area de busqueda inteligente
            return (100 * (y - frameRect.Y)) / frameRect.Height;
        }

        private Bitmap CreateBitmapFromRawBytes(byte[] pixelValues, int width, int height)
        {
            if (pixelValues.Length == 0)
            {
                return null;
            }

            //Create an image that will hold the image data
            Bitmap pic = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            try
            {
                //Get a reference to the images pixel data
                Rectangle dimension = new Rectangle(0, 0, pic.Width, pic.Height);
                BitmapData picData = pic.LockBits(dimension, ImageLockMode.ReadWrite, pic.PixelFormat);
                IntPtr pixelStartAddress = picData.Scan0;

                //Copy the pixel data into the bitmap structure
                Marshal.Copy(pixelValues, 0, pixelStartAddress, pixelValues.Length);

                pic.UnlockBits(picData);
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
            

            return pic;
        }

        private byte[] EscapeHash(byte[] hash)
        {
            try
            {
                for (int i = 0; i < hash.Length - 1; i++)
                {
                    if ((hash[i] != 0xFF) || (hash[i + 1] != 0xC0))
                    {
                        continue;
                    }
                    else
                    {
                        hash[i] = 0x58;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
           
            return hash;
        }

        private void ClientNewFrame(byte[] data, int length)
        {
            try
            {
                RenderVideo(data);
            }
            catch (ThreadAbortException)
            {
                // entra aqui por cierre externo (se dispara desde el close)
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Version del ensamblado DLL
        /// </summary>
        /// <returns></returns>
        [ComVisible(true)]
        public string AxAssemblyVersion
        {
            get
            {
                AssemblyName aname = Assembly.GetExecutingAssembly().GetName();
                Version ver = aname.Version;
                return ver.Major + "," + ver.Minor + "," + ver.Build + "," + ver.Revision;
            }
        }

        /// <summary>
        /// URL del servidor de video.
        /// </summary>
        [ComVisible(true)]
        public string ServerUrl
        {
            get => client.GetServerUrl();
            set => client.SetServerUrl(value);
        }

        /// <summary>
        /// Cambia el puerto de transmision.
        /// </summary>
        /// <param name="port">Puerto servidor de video.</param>
        [ComVisible(true)]
        public void SetPortTransfer(int port)
        {
            if (port > 0 && port < 65535)
            {
                this.portTransfer = port;
            }
            else
            {
                this.portTransfer = -1;
            }
        }

        /// <summary>
        /// Factor de zoom (porcentaje, 100 = tamaño original). 
        /// Válido cuando ZoomMode es 'Fixed'.
        /// </summary>
        [ComVisible(true)]
        public int Zoom
        {
            get => zoom;
            set
            {
                if (value > 0)
                {
                    zoom = value;
                    UpdateDestinationRectangle();
                    this.Invalidate();
                }
            }
        }

        /// <summary>
        /// Modo de zoom de imagen.
        /// </summary>
        [ComVisible(true)]
        public ZoomModeEnum ZoomMode
        {
            get => zoomMode;
            set
            {
                zoomMode = value;
                UpdateDestinationRectangle();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Modo de zoom de imagen ToString.
        /// </summary>
        [ComVisible(true)]
        public string ZoomModeToString => zoomMode.ToString();

        /// <summary>
        /// Mostrar barra de título.
        /// </summary>
        [ComVisible(true)]
        public bool ShowTitleBar
        {
            get => showTitleBar;
            set
            {
                showTitleBar = value;
                UpdateDestinationRectangle();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Mostrar barra de estado.
        /// </summary>
        [ComVisible(true)]
        public bool ShowStatusBar
        {
            get => showStatusBar;
            set
            {
                showStatusBar = value;
                UpdateDestinationRectangle();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Mostrar barra de estado.
        /// </summary>
        [ComVisible(true)]
        public bool ShowControlStatusBar
        {
            get => showControlStatusBar;
            set
            {
                showControlStatusBar = value;
                UpdateDestinationRectangle();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Mostrar barra de desplazamiento.
        /// </summary>
        [ComVisible(true)]
        public bool ShowTrackBar
        {
            get => showTrackBar;
            set
            {
                showTrackBar = value;
                UpdateDestinationRectangle();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Mostrar estado de la conexión.
        /// </summary>
        [ComVisible(true)]
        public bool ShowConnectionStatus
        {
            get => showConnectionStatus;
            set
            {
                showConnectionStatus = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Mostrar etiqueta del cuadro.
        /// </summary>
        [ComVisible(true)]
        public bool ShowLabel
        {
            get => showLabel;
            set
            {
                showLabel = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Habilitar/deshabilitar modo de "búsqueda inteligente" (detección de movimiento).
        /// La reproducción saltea cuadros hasta encontrar uno con detección de movimiento, 
        /// y queda en pausa.
        /// </summary>
        [ComVisible(true)]
        public bool SmartSearchMode
        {
            get => smartSearchMode;
            set
            {
                smartSearchMode = value;
                client.SetSmartSearchMode(smartSearchMode);
                this.Invalidate();
            }
        }

        /// <summary>
        /// ShowSmartSearchOverlay
        /// </summary>
        [ComVisible(true)]
        public bool ShowSmartSearchOverlay
        {
            get => ShowSmartSearchOverlay;
            set
            {
                showSmartSearchOverlay = value;
                client.SetSmartSearchOverlay(showSmartSearchOverlay, smartSearchAreaColor.R, smartSearchAreaColor.G, smartSearchAreaColor.B);
                this.Invalidate();
            }
        }

        /// <summary>
        /// autoDetectProxy
        /// </summary>
        [ComVisible(true)]
        public bool AutoDetectProxy
        {
            get => _autoDetectProxy;
            set
            {
                _autoDetectProxy = value;
                client.SetAutoDetectProxy(_autoDetectProxy);
            }
        }

        public VRecLastOperationResult LastError => lastError;

        /// <summary>
        /// Cambiar tamaño del cuadro de video.
        /// </summary>
        /// <remarks>Este método determina el tamaño que debe ocupar la imagen, sin 
        /// incluir las barras adicionales. Para cambiar el tamaño 
        /// del control usar las propiedades 'Width' y 'Height'.</remarks>
        /// <param name="width">Ancho del cuadro en pixels.</param>
        /// <param name="height">Alto del cuadro en pixels.</param>
        [ComVisible(true)]
        public void SetFrameSize(int width, int height)
        {
            this.Width = width;
            this.Height = height + titleBarHeight + statusBarHeight + trackBarHeight;
        }

        /// <summary>
        /// Cambia el timeout de la conexión.
        /// </summary>
        /// <param name="timeout">Período de espera en milisegundos.</param>
        [ComVisible(true)]
        public void SetConnectionTimeout(int timeout)
        {
            client.SetConnectionTimeout(timeout);
        }

        /// <summary>
        /// Estado de la conexión.
        /// </summary>
        /// <returns>True si está conectado al servidor, false en caso contrario.</returns>
        [ComVisible(true)]
        public bool IsConnected()
        {
            return client.GetStatus() != ClientStatus.Close;
        }

        /// <summary>
        /// Abre la conexión al servidor de reproducción de video, solicitando la 
        /// reproducción de una cámara y fecha/hora en particular.
        /// </summary>
        /// <param name="cameraId">Identificador de cámara</param>
        /// <param name="videoDateTime">Fecha/hora del video requerido, en formato 'YYYYMMDDHHMMSS'</param>
        /// <returns>True si la conexión  se abrió exitosamente, false en caso contrario.</returns>
        //[ComVisible(true)]
        public bool Open(string cameraId, string videoDateTime)
        {  if (vdec != null)
            {
                vdec = null;
            }

            vdec = new H264Decoder.Decoder_H264();

            Cursor.Current = Cursors.WaitCursor;
            if (client.GetStatus() != ClientStatus.Close)
            {
                client.Close();
            }

            currFrame = null;
            timeline = null;

            flagsZeroFrame = false;

            smartSearchMode = false;
            smartSearchArea = new Rectangle(0, 0, 100, 100);

            this.Invalidate();

            if (this.portTransfer > 0)
            {
                client.SetPortTransfer(this.portTransfer);
            }
            var success = false;
            try
            {
                success = client.Open(cameraId, videoDateTime);
                if (success)
                {
                    error = false;
                    this.cameraId = cameraId;
                    timeline = client.GetTimeline();
                    this.lastError = VRecLastOperationResult.Success;
                    //client.start();
                }
                else if (this.errorMsg == Resources.VRecVideoEmpty)
                {
                    Graphics g = this.CreateGraphics();
                    flagsZeroFrame = true;
                    Draw(g);
                    this.lastError = VRecLastOperationResult.VRecVideoEmpty;
                }
                else if (this.errorMsg == Resources.VRecVideoNotExists)
                {
                    Graphics g = this.CreateGraphics();
                    flagsZeroFrame = true;
                    Draw(g);
                    this.lastError = VRecLastOperationResult.VRecVideoNotExists;
                }
                else if (this.errorMsg == Resources.VRecMaximumClientNumbers)
                {
                    Graphics g = this.CreateGraphics();
                    flagsTooManyUser = true;
                    Draw(g);
                    this.lastError = VRecLastOperationResult.VRecMaximumClientNumbers;
                }
                else
                {
                    this.lastError = VRecLastOperationResult.VRecErrorInexperado;
                }

                Cursor.Current = Cursors.Default;
                this.Invalidate();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
            return success;
        }

        public bool Open(CameraDTO camera, string videoDateTime)
        {
            if (vdec != null)
            {
                vdec = null;
            }

            vdec = new H264Decoder.Decoder_H264();

            Cursor.Current = Cursors.WaitCursor;
            if (client.GetStatus() != ClientStatus.Close)
            {
                client.Close();
            }

            currFrame = null;
            timeline = null;

            flagsZeroFrame = false;

            smartSearchMode = false;
            smartSearchArea = new Rectangle(0, 0, 100, 100);

            this.Invalidate();

            if (this.portTransfer > 0)
            {
                client.SetPortTransfer(this.portTransfer);
            }
            var success = false;
            try
            {
                success = client.Open(camera.Id.ToString(), videoDateTime);
                if (success)
                {
                    error = false;
                    this.cameraId = camera.Id.ToString();
                    timeline = client.GetTimeline();
                    this.lastError = VRecLastOperationResult.Success;
                }
                else if (this.errorMsg == Resources.VRecVideoEmpty)
                {
                    flagsZeroFrame = true;
                    this.lastError = VRecLastOperationResult.VRecVideoEmpty;
                }
                else if (this.errorMsg == Resources.VRecVideoNotExists)
                {
                    flagsZeroFrame = true;
                    this.lastError = VRecLastOperationResult.VRecVideoNotExists;
                }
                else if (this.errorMsg == Resources.VRecMaximumClientNumbers)
                {
                    flagsTooManyUser = true;
                    this.lastError = VRecLastOperationResult.VRecMaximumClientNumbers;
                }
                else
                {
                    this.lastError = VRecLastOperationResult.VRecErrorInexperado;
                }

                Cursor.Current = Cursors.Default;
                this.Invalidate();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
            
            return success;
        }

        private void ClientError(string errorMsg)
        {
            try
            {
                // este método se invoca desde otro thread
                error = true;
                this.errorMsg = errorMsg;
                this.Invalidate();
                if (errorMsg != Resources.VRecMaximumClientNumbers
                    && errorMsg != Resources.VRecVideoNotExists
                    && errorMsg != Resources.VRecVideoEmpty
                    && errorMsg != Resources.VRecError)
                {// si es disintos a estos errores entonces no fue un error al momento de conectar al vrec y le aviso al smart client
                    this.lastError = VRecLastOperationResult.VRecErrorInexperado;
                    ErrorHandler?.Invoke(errorMsg);
                }
            }
            catch (Exception ex20)
            {
                Logger.Log($"Message 20 {ex20.Message} StackTrace {ex20.StackTrace}");
            }
            
        }

        /// <summary>
        /// Inicia el proceso de reproducción.
        /// </summary>
        [ComVisible(true)]
        public void Start()
        {
            try
            {
                client.Start();
                this.Invalidate();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
           
        }

        /// <summary>
        /// Detiene el proceso de reproducción y retorna a la posición inicial.
        /// </summary>
        [ComVisible(true)]
        public void Stop()
        {
            try
            {
                client.Stop();
                lastTimestamp = new DateTime(1970, 1, 1);
                this.Invalidate();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
           
        }

        /// <summary>
        /// Congela la reproducción en la posición actual.
        /// </summary>
        [ComVisible(true)]
        public void Pause()
        {
            try
            {
                client.Pause();
                this.Invalidate();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
            
        }

        /// <summary>
        /// Finaliza la conexión con el servidor de reproducción de video.
        /// </summary>
        [ComVisible(true)]
        public void Close()
        {
            try
            {
                if (vdec != null)
                {
                    vdec = null;
                }

                client.Stop();
                client.Close();
                lastTimestamp = new DateTime(1970, 1, 1);
                timeline = null;
                error = false;
                flagsZeroFrame = false;
                flagsTooManyUser = false;
                currFrame = null;
                this.Invalidate();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
            
        }

        /// <summary>
        /// Obtiene el identificador de cámara del video actual en reproducción.
        /// </summary>
        /// <returns>Identificador de cámara.</returns>
        [ComVisible(true)]
        public string GetCameraId()
        {
            return cameraId;
        }

        /// <summary>
        /// Setea el identificador de cámara del video actual en reproducción.
        /// </summary>		
        /// <param name="id">Identificador de cámara</param>
        [ComVisible(true)]
        public void SetCameraId(string id)
        {
            this.cameraId = id;
        }

        /// <summary>
        /// Obtiene la fecha y hora inicial del video actual en reproducción.
        /// </summary>
        /// <returns>Fecha/hora en formato "YYYYMMDDHHMMSS".</returns>
        [ComVisible(true)]
        public string GetVideoDateTime()
        {
            return client.GetTimestamp().ToString("yyyyMMddHH" + "0000");
        }

        /// <summary>
        /// Retorna la posición de reproducción actual (en tiempo).
        /// </summary>
        /// <returns>Posición de reproducción (en segundos respecto de hora de reproducción actual).</returns>
        [ComVisible(true)]
        public int GetTimeOffset()
        {
            return client.GetTimeOffset();
        }

        /// <summary>
        /// Cambiar posición de reproducción actual (en tiempo).
        /// </summary>
        /// <param name="seconds">Desplazamiento en segundos respecto de hora de reproducción actual.</param>
        [ComVisible(true)]
        public void SetTimeOffset(double seconds)
        {
            client.SetTimeOffset(seconds);
            this.Invalidate();
        }

        /// <summary>
        /// Saltar a una posición absoluta de reproducción (en tiempo).
        /// </summary>
        /// <param name="seekDateTime">Fecha/hora de reproducción buscada, en formato 'YYYYMMDDHHMMSS'</param>
        [ComVisible(true)]
        public void Seek(string seekDateTime)
        {
            DateTime dateTime = DateTime.ParseExact(seekDateTime, "yyyyMMddHHmmss", DateTimeFormatInfo.CurrentInfo);
            client.Seek(dateTime);
            this.Invalidate();
        }

        /// <summary>
        /// Saltear cuadros en la reproducción.
        /// </summary>
        /// <param name="count">Cantidad de cuadros a saltear (negativo o positivo).</param>
        [ComVisible(true)]
        public void FrameSkip(int count)
        {
            client.SetPosition(client.GetPosition() + count);
            this.Invalidate();
        }

        /// <summary>
        /// Obtiene el factor de skip (salto de cuadros) de reproducción.
        /// </summary>
        /// <returns>Factor de skip.</returns>
        [ComVisible(true)]
        public int GetSkipRate()
        {
            return client.GetSkipRate();
        }

        /// <summary>
        /// Cambia el factor de skip (salto de cuadros) de reproducción. 
        /// </summary>
        /// <param name="rate">Factor de skip. Es la cantidad de cuadros que deben
        /// saltearse durante la reproducción. Puede ser un número positivo (siendo 1 el
        /// factor de reproducción normal) o un número negativo (reproducción inversa).</param>
        [ComVisible(true)]
        public void SetSkipRate(int rate)
        {
            client.SetSkipRate(rate);
            this.Invalidate();
        }

        /// <summary>
        /// Obtiene la cantidad de cuadros por segundo a la que fue grabado el
        /// cuadro actualmente en reproducción.
        /// El mínimo fps informado es 1.
        /// </summary>
        /// <returns>FPS de cuadro actual.</returns>
        [ComVisible(true)]
        public float GetFrameRate()
        {
            return client.GetFrameRate() > 0 ? client.GetFrameRate() : 1f;
        }

        /// <summary>
        /// Obtiene la fecha/hora correspondiente al cuadro actualmente en 
        /// reproducción.
        /// </summary>
        /// <returns>Fecha/hora de cuadro actual.</returns>
        [ComVisible(true)]
        public DateTime GetTimestamp()
        {
            return client.GetTimestamp();
        }

        /// <summary>
        /// Obtiene la etiqueta correspondiente al cuadro actualmente en 
        /// reproducción.
        /// </summary>
        /// <returns>Etiqueta del cuadro actual.</returns>
        [ComVisible(true)]
        public string GetLabel()
        {
            return client.GetLabel();
        }

        /// <summary>
        /// Obtiene el estado del proceso de reproducción.
        /// </summary>
        /// <returns>Estado del proceso de reproducción.</returns>
        [ComVisible(true)]
        public ClientStatus GetStatus()
        {
            return client.GetStatus();
        }

        /// <summary>
        /// Obtiene el estado del proceso de reproducción en un string.
        /// </summary>
        /// <returns>Estado del proceso de reproducción.</returns>
        [ComVisible(true)]
        public string GetStatusToString()
        {
            return client.GetStatus().ToString();
        }

        /// <summary>
        /// Obtiene el último mensaje de error producido.
        /// </summary>
        /// <returns>Mensaje de error (o string vacío si no hubo errores).</returns>
        [ComVisible(true)]
        public string GetErrorMsg()
        {
            return error ? errorMsg : "";
        }

        /// <summary>
        /// Asocia colores a la linea de tiempo indicadora de zonas de video. Los colores
        /// se identifican por nombre (de System.Drawing.Color).
        /// </summary>
        /// <param name="code">Código de zona de video. Los códigos reconocidos son: 0 (no hay video), 1 (hay video sin eventos), 2 (hay video con eventos).</param>
        /// <param name="colorName">Nombre de color (de System.Drawing.Color).</param>
        [ComVisible(true)]
        public void SetTimelineColorName(int code, string colorName)
        {
            if (code >= 0 && code < timelineCodeCount)
            {
                timelineBrush[code] = new SolidBrush(Color.FromName(colorName));
            }
        }

        /// <summary>
        /// Asocia colores a la linea de tiempo indicadora de zonas de video. Los colores
        /// se identifican por su valor (R,G,B).
        /// </summary>
        /// <param name="code">Código de zona de video. Los códigos reconocidos son: 0 (no hay video), 1 (hay video sin eventos), 2 (hay video con eventos).</param>
        /// <param name="red">Valor de rojo (0 a 255).</param>
        /// <param name="green">Valor de verde (0 a 255).</param>
        /// <param name="blue">Valor de azul (0 a 255).</param>
        [ComVisible(true)]
        public void SetTimelineColorRGB(int code, int red, int green, int blue)
        {
            if (code >= 0 && code < timelineCodeCount)
            {
                timelineBrush[code] = new SolidBrush(Color.FromArgb(red, green, blue));
            }
        }

        /// <summary>
        /// Determina los umbrales para búsqueda inteligente.
        /// </summary>
        /// <param name="delta">Umbral de diferencia de nivel por pixel.</param>
        /// <param name="vol">Umbral de volumen de movimiento global.</param>
        [ComVisible(true)]
        public void SetSmartSearchThresholds(int delta, int vol)
        {
            if (delta >= 0 && delta <= 100)
            {
                smartSearchThDelta = delta;
            }

            if (vol >= 0 && vol <= 100)
            {
                smartSearchThVol = vol;
            }

            client.SetSmartSearchThresholds(smartSearchThDelta, smartSearchThVol);
            this.Invalidate();
        }

        /// <summary>
        /// Determina si debe mostrarse el overlay de movimiento y el color del mismo.
        /// </summary>
        /// <param name="red">Nivel de rojo (0 a 255).</param>
        /// <param name="green">Nivel de verde (0 a 255).</param>
        /// <param name="blue">Nivel de azul (0 a 255).</param>
        [ComVisible(true)]
        public void SetSmartSearchOverlayColorRGB(int red, int green, int blue)
        {
            client.SetSmartSearchOverlay(showSmartSearchOverlay, red, green, blue);
            smartSearchOverlayColor = Color.FromArgb(red, green, blue);
            this.Invalidate();
        }

        /// <summary>
        /// Determina si debe mostrarse el overlay de movimiento y el color del mismo.
        /// </summary>
        /// <param name="red">Nivel de rojo (0 a 255).</param>
        /// <param name="green">Nivel de verde (0 a 255).</param>
        /// <param name="blue">Nivel de azul (0 a 255).</param>
        [ComVisible(true)]
        public void SetSmartSearchAreaColorRGB(int red, int green, int blue)
        {
            smartSearchAreaColor = Color.FromArgb(red, green, blue);
            this.Invalidate();
        }

        /// <summary>
        /// Realiza una búsqueda inteligente (detección de movimiento) offline sobre todos 
        /// los archivos de video dentro del rango horario dado.
        /// </summary>
        /// <remarks>Los parámetros para la búsqueda (umbrales y area) son los mismos que 
        /// usa el modo online.</remarks>
        /// <param name="start">Fecha/hora inicial, en formato 'YYYYMMDDHHMMSS'</param>
        /// <param name="end">Fecha/hora final, en formato 'YYYYMMDDHHMMSS'</param>
        /// <param name="deltaSeconds">Intervalo de búsqueda en segundos.</param>
        [ComVisible(true)]
        public void StartOfflineSmartSearch(string start, string end, int deltaSeconds)
        {
            DateTime startDateTime = DateTime.ParseExact(start, "yyyyMMddHHmmss", DateTimeFormatInfo.CurrentInfo);
            DateTime endDateTime = DateTime.ParseExact(end, "yyyyMMddHHmmss", DateTimeFormatInfo.CurrentInfo);
            client.StartOfflineSmartSearch(cameraId, startDateTime, endDateTime, deltaSeconds,
                smartSearchArea.X, smartSearchArea.Y, smartSearchArea.Width, smartSearchArea.Height,
                smartSearchThDelta, smartSearchThVol);
        }

        /// <summary>
        /// Finalizar la búsqueda offline.
        /// </summary>
        [ComVisible(true)]
        public void AbortOfflineSmartSearch()
        {
            client.AbortOfflineSmartSearch();
        }

        /// <summary>
        /// Obtiene los resultados parciales de la búsqueda offline.
        /// </summary>
        /// <returns>Lista de fecha/hora indicando los instantes donde se detectó movimiento, en formato 'YYYYMMDDHHMMSS'.</returns>
        [ComVisible(true)]
        public string[] GetOfflineSmartSearchResults()
        {
            DateTime[] dateTimeResults = client.GetOfflineSmartSearchResults();
            string[] results = new string[dateTimeResults.Length];
            for (int i = 0; i < dateTimeResults.Length; i++)
            {
                results[i] = dateTimeResults[i].ToString("yyyyMMddHHmmss");
            }

            return results;
        }

        /// <summary>
        /// Obtiene los resultados parciales de la búsqueda offline (Lista separada por ";").
        /// </summary>
        /// <returns>Lista de fecha/hora indicando los instantes donde se detectó movimiento, en formato 'YYYYMMDDHHMMSS;'.</returns>
        [ComVisible(true)]
        public string GetOfflineSmartSearchResultsToString()
        {
            DateTime[] dateTimeResults = client.GetOfflineSmartSearchResults();
            string results = "";
            for (int i = 0; i < dateTimeResults.Length; i++)
            {
                results += dateTimeResults[i].ToString("yyyyMMddHHmmss") + ";";
            }

            return results;
        }

        /// <summary>
        /// Indica si la búsqueda offline se completó.
        /// </summary>
        /// <returns>True si la búsqueda está completa, false en caso contrario.</returns>
        [ComVisible(true)]
        public bool IsOfflineSmartSearchComplete()
        {
            return client.IsOfflineSmartSearchComplete();
        }

        /// <summary>
        /// Obtiene el porcentaje de progreso de la búsqueda offline.
        /// </summary>
        /// <returns>Porcentaje de progreso de la búsqueda (valor de 0 a 100).</returns>
        [ComVisible(true)]
        public int GetOfflineSmartSearchProgress()
        {
            return client.GetOfflineSmartSearchProgress();
        }

        /// <summary>
        /// Muestra una ventana de diálogo para guardar la imagen actual.
        /// </summary>
        [ComVisible(true)]
        public void Snapshot(string path)
        {
            Bitmap bmp;
            lock (this)
            {
                if (currFrame == null)
                {
                    return;
                }

                bmp = new Bitmap(currFrame);
            }
            string filename = string.Empty;

            try
            {
                string camera_name = client.GetCameraName();
                if ((camera_name != string.Empty) && (!Regex.IsMatch(camera_name, @"[\\|'/'|':'|'*'|'?'|'<'|'>'|'|']")))
                {
                    filename += camera_name + "_";
                }
                else
                {
                    if ((this.cameraId != string.Empty) && (this.cameraId != null))
                    {
                        filename += this.cameraId + "_";
                    }
                }
            }
            catch
            { }

            filename += client.GetTimestamp().ToString("yyyyMMddHHmmss");

            ImageFormat fmt = ImageFormat.Jpeg;
            PropertyItem hashPropertyItem = (PropertyItem)Activator.CreateInstance(typeof(PropertyItem), true);

            //Calculo del hash SHA1 con clave privada de la imagen
            byte[] key = { 0x6C, 0x69, 0x70, 0x73, 0x69, 0x72, 0x6E, 0x66, 0x69, 0x75, 0x62, 0x61 };
            HMACSHA1 provider = new HMACSHA1(key);

            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, fmt);
            ms.Position = 0;
            byte[] buffer = new byte[ms.Length];
            ms.Read(buffer, 0, (int)ms.Length);

            int i;
            for (i = 0; i < buffer.Length - 1; i++)
            {
                if ((buffer[i] == 0xFF) && (buffer[i + 1] == 0xC0))
                {
                    break;
                }
            }

            byte[] hash = provider.ComputeHash(buffer, i, buffer.Length - i);
            hash = EscapeHash(hash);

            //Copyright ID
            hashPropertyItem.Id = 33432;
            hashPropertyItem.Len = hash.Length;
            hashPropertyItem.Type = 2;
            hashPropertyItem.Value = hash;

            try
            {
                bmp.SetPropertyItem(hashPropertyItem);
                bmp.Save(path, fmt);
            }
            catch
            {
            }
        }

        [ComVisible(true)]
        public bool OpenSound()
        {
            return client.OpenSound();
        }

        [ComVisible(true)]
        public bool CloseSound()
        {
            return client.CloseSound();
        }

        #endregion
    }
}
