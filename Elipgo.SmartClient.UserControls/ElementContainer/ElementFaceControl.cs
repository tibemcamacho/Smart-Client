using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.SignalR.Connection;
using Elipgo.SmartClient.UserControls.FaceObject;
using Elipgo.SmartClient.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.ElementContainer
{
    public partial class ElementFaceControl : UserControl, IDisposable
    {
        private List<FaceAlarmsDTO> faceAlarms = new List<FaceAlarmsDTO>();
        private SidebarElementDTO Element;
        public event ObjectSelectedEventHandler ObjectSelected;
        public event ObjectDoubleClickSelectedEventHandler ObjectSelectedDoubleClick;

        private int _faceHeight = 142;
        private int _faceWidth = 234;

        private bool _loaded = false;
        private bool _painted = false;

        private readonly string _token = null;
        private readonly LiveViewModel viewModel;
        private readonly VmonitoringManagerConnection Signal;
        public SidebarElementDTO _element;
        public List<FaceAlarmsDTO> _dtoElement;

        public ElementFaceControl(SidebarElementDTO element, List<FaceAlarmsDTO> dto, LiveViewModel liveView)
        {
            _token = liveView.MainView.UserToken;
            viewModel = liveView;
            faceAlarms = dto;
            Element = element;
            _element = element;
            _dtoElement = dto;

            Signal = liveView.MainView.Signal;

            InitializeComponent();

            this.Paint += ElementFaceControl_Paint;
            this.Click += ElementFaceControl_Click;
            this.PanelPicture.Click += ElementFaceControl_Click;
            this.PanelList.Click += ElementFaceControl_Click;

            this.DoubleClick += ElementFaceControl_DoubleClick; ;
            this.PanelPicture.DoubleClick += ElementFaceControl_DoubleClick;
            this.PanelList.DoubleClick += ElementFaceControl_DoubleClick;

            this.Resize += ElementFaceControl_Resize;
            Disposed += ElementFaceControl_Disposed;
        }

        private void ElementFaceControl_DoubleClick(object sender, EventArgs e)
        {
            ObjectSelectedDoubleClick?.Invoke(this);
        }

        private void ElementFaceControl_Disposed(object sender, EventArgs e)
        {
            Signal.FaceRecognitionEventAction -= FaceEvent;
        }

        private void ElementFaceControl_Resize(object sender, EventArgs e)
        {
            if (!_loaded && !_painted) return;

            SetStyles();
            PanelList.Controls.Clear();
            _loaded = false;
            LoadFaces();
        }

        private void ElementFaceControl_Click(object sender, EventArgs e)
        {
            ObjectSelected(this, Element);
        }

        private void ElementFaceControl_Paint(object sender, PaintEventArgs e)
        {
            var thread = new Thread(async () =>
            {
                try
                {
                    SetStyles();

                    faceAlarms = await viewModel.GetFaceElements(Element.ElementId);
                    if (faceAlarms == null) faceAlarms = new List<FaceAlarmsDTO>();

                    LoadFaces();
                }
                finally
                {
                    this._painted = true;
                    Signal.FaceRecognitionEventAction += FaceEvent;
                }
            });
            thread.Start();
        }

        private void SetStyles()
        {
            PanelPicture.Width = (int)((this.Width * 71.25) / 100);
            PanelList.Width = this.Width - PanelPicture.Width;

            if (PanelList.Width > _faceWidth)
            {
                PanelList.Width = _faceWidth;
                PanelPicture.Width = this.Width - _faceWidth;
            }

            PanelPicture.Height = this.Height;
            PanelList.Height = this.Height;

            PanelList.Left = PanelPicture.Width;
        }

        private void FaceEvent(dynamic d)
        {
            FaceAlarmsDTO card = JsonConvert.DeserializeObject<FaceAlarmsDTO>(d.ToString());
            if (card.DeviceFeatureId == Element.ElementId)
            {
                faceAlarms.Insert(0, card);
                if (faceAlarms.Count > 1)
                    faceAlarms.RemoveAt(faceAlarms.Count - 2);

                PanelList.Controls.Clear();
                _loaded = false;
                LoadFaces();
            }
        }

        private void LoadFaces()
        {
            if (_loaded) return;

            if (PanelList.InvokeRequired)
            {
                PanelList.Invoke((MethodInvoker)delegate
                {
                    LoadFaces();
                });
                return;
            }

            this.LoadPanel();

        }

        private void LoadPanel()
        {

            if (Screen.AllScreens.Any(m => m.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                //142;
                var pHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1314M), 2));
                var pWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1218M), 2));


                if (main.Height == 768 && PanelList.Height < 100)
                    pHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0598M), 2));
                if (PanelList.Height < 300 && PanelList.Height > 100)
                {
                    pHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0648M), 2));
                }
                _faceHeight = pHeight;
                //_faceWidth = pWidth;
            }

            int x = PanelList.Width / _faceWidth;
            int y = PanelList.Height / _faceHeight;

            int width = PanelList.Width > _faceWidth ? PanelList.Width / x : PanelList.Width;
            int height = PanelList.Height > _faceHeight ? PanelList.Height / y : PanelList.Height;

            int t = x == 0 ? y : y * x;

            for (int i = 0; i < t; i++)
            {
                var item = new FaceAlarmsDTO();
                // Validamos no quedar fuera de rango
                if (faceAlarms != null && faceAlarms.Any() && i < t && i < faceAlarms.Count)
                {
                    item = faceAlarms[i];

                    if (i == 0) LoadImage(item.Id);

                    FaceElement face = new FaceElement(item, width, height)
                    {
                        Name = Guid.NewGuid().ToString(),
                        Visible = true,
                        Size = new Size(width - 1, height - 1),
                    };
                    PanelList.Controls.Add(face);
                    face.Selected += Face_Selected;
                    face.BorderStyle = BorderStyle.FixedSingle;
                }
                else
                {
                    // Agregamos un elemento Vacio
                    FaceElementEmpty faceSinDatos = new FaceElementEmpty(width, height)
                    {
                        Name = Guid.NewGuid().ToString(),
                        Visible = true,
                        Size = new Size(width - 1, height - 1),
                    };

                    PanelList.Controls.Add(faceSinDatos);
                }

                if (i == 0) LoadImage(item.Id);
            }

            _loaded = true;
        }

        private void Face_Selected(object sender, FaceAlarmsDTO e)
        {
            if (e != null)
            {
                this.ViewImage(e.SubjectFaceImage);
            }
            ObjectSelected(this, Element);
        }

        private void LoadImage(int id)
        {
            Task.Run(async () =>
            {
                if (id > 0)
                {
                    var i = await viewModel.GetLprSnapshot(id);

                    ViewImage(i.Data);
                }
                else
                {
                    var img = ImageHelper.ImageToArray(FileResources.NotImage);
                    this.ViewImage(img);
                }
            });
        }

        private void ViewImage(byte[] i)
        {
            using (var ms = new MemoryStream(i, 0, i.Length))
            {
                var image = Image.FromStream(ms, true);
                PanelPicture.BackgroundImage = image;
                PanelPicture.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        public new void Dispose()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    Dispose();
                });
                return;
            }
            base.Dispose(true);
        }

    }
}
