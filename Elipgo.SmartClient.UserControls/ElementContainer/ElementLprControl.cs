using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.SignalR.Connection;
using Elipgo.SmartClient.UserControls.LprObject;
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
    public partial class ElementLprControl : UserControl
    {
        public event ObjectSelectedEventHandler ObjectSelected;
        public event ObjectDoubleClickSelectedEventHandler ObjectSelectedDoubleClick;

        private int _lprHeight = 107;
        private const int _lprWidth = 234;

        private bool _loaded = false;
        private bool _painted = false;

        private readonly string _token = null;
        private readonly LiveViewModel _viewModel;
        private readonly VmonitoringManagerConnection _signal;

        public SidebarElementDTO _element { get; set; }
        public List<LprAlarmDTO> _dtoElement { get; set; }

        public ElementLprControl(SidebarElementDTO element, List<LprAlarmDTO> dto, LiveViewModel liveView)
        {
            _token = liveView.MainView.UserToken;
            _viewModel = liveView;
            _element = element;
            _dtoElement = dto;

            _signal = liveView.MainView.Signal;

            InitializeComponent();

            this.Paint += ElementLprControl_Paint;

            this.Click += ElementLprControl_Click;
            this._panelPicture.Click += ElementLprControl_Click;
            this._panelList.Click += ElementLprControl_Click;

            this.DoubleClick += PanelList_DoubleClick;
            this._panelPicture.DoubleClick += PanelList_DoubleClick;
            this._panelList.DoubleClick += PanelList_DoubleClick;

            this.Resize += ElementLprControl_Resize;
            Disposed += ElementLprControl_Disposed;
        }

        private void PanelList_DoubleClick(object sender, EventArgs e)
        {
            ObjectSelectedDoubleClick?.Invoke(this);
        }

        private void ElementLprControl_Disposed(object sender, EventArgs e)
        {
            _signal.LPREventAction -= LprEvent;
        }

        private void ElementLprControl_Resize(object sender, EventArgs e)
        {
            if (!_loaded && !_painted) return;

            SetStyles();
            _panelList.Controls.Clear();
            _loaded = false;
            LoadLPR();
        }

        private void ElementLprControl_Click(object sender, EventArgs e)
        {
            ObjectSelected(this, _element);
        }

        private void ElementLprControl_Paint(object sender, PaintEventArgs e)
        {
            if (_painted) return;

            var thread = new Thread(async () =>
            {
                try
                {
                    SetStyles();

                    _dtoElement = await _viewModel.GetLprElements(_element.ElementId);
                    if (_dtoElement == null) _dtoElement = new List<LprAlarmDTO>();

                    LoadLPR();
                }
                finally
                {
                    this._painted = true;
                    if (_panelList.Controls.Count > 0)
                        (_panelList.Controls[0] as LprElement).SetSelected();

                    _signal.LPREventAction += LprEvent;
                }
            });
            thread.Start();
        }

        private void LprEvent(dynamic d)
        {
            LprAlarmDTO card = JsonConvert.DeserializeObject<LprAlarmDTO>(d.ToString());
            if (card.VideoContentAnalyticId == _element.ElementId)
            {
                _dtoElement.Insert(0, card);
                if (_dtoElement.Count > 1)
                    _dtoElement.RemoveAt(_dtoElement.Count - 2);

                _panelList.Controls.Clear();
                _loaded = false;
                LoadLPR();
                if (_panelList.Controls.Count > 0)
                    (_panelList.Controls[0] as LprElement).SetSelected();
            }
        }

        private void SetStyles()
        {
            if (this.Width > 1000)
            {
                _panelPicture.Width = (int)((this.Width * 58.25) / 100);
                _panelList.Width = this.Width - _panelPicture.Width;

                if (_panelList.Width > _lprWidth)
                {
                    _panelList.Width = _panelList.Width + 100;
                    _panelPicture.Width = _panelPicture.Width;
                }
            }
            else
            {
                _panelPicture.Width = (int)((this.Width * 71.25) / 100);
                _panelList.Width = this.Width - _panelPicture.Width;

                if (_panelList.Width > _lprWidth)
                {
                    _panelList.Width = _lprWidth;
                    _panelPicture.Width = this.Width - _lprWidth;
                }
            }


            _panelPicture.Height = this.Height;
            _panelList.Height = this.Height;

            _panelList.Left = _panelPicture.Width;
        }

        private void LoadLPR()
        {
            if (_loaded) return;

            if (_panelList.InvokeRequired)
            {
                _panelList.Invoke((MethodInvoker)delegate
                {
                    LoadLPR();
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
                var pHeight = 107;
                pHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0990M), 2));


                if (main.Height == 768 && _panelList.Height < 100)
                    pHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0598M), 2));
                if (_panelList.Height < 300 && _panelList.Height > 100)
                {
                    pHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0648M), 2));
                }
                _lprHeight = pHeight;
            }

            int x = _panelList.Width / _lprWidth;
            int y = _panelList.Height / _lprHeight;

            int width = _panelList.Width > _lprWidth ? _panelList.Width / x : _panelList.Width;
            int height = _panelList.Height > _lprHeight ? _panelList.Height / y : _panelList.Height;

            if (y > 0)
            {
                int totalHeight = _panelList.Height;
                _lprHeight = totalHeight / y;
            }

            int t = x == 0 ? y : y * x;

            for (int i = 0; i < t; i++)
            {
                var item = new LprAlarmDTO();
                // Validamos no quedar fuera de rango
                if (_dtoElement.Any() && i < t)
                {
                    item = _dtoElement[i];

                    LprElement lpr = new LprElement(item, _panelList.Width - 2, _lprHeight)
                    {
                        Name = Guid.NewGuid().ToString(),
                        Visible = true,
                        Size = new Size(_panelList.Width - 2, _lprHeight),
                    };
                    _panelList.Controls.Add(lpr);
                    lpr.Selected += Lpr_Selected;
                    lpr.BorderStyle = BorderStyle.FixedSingle;
                }
                else
                {
                    // Agregamos un elemento Vacio
                    LprElement lprSinDatos = new LprElement(new LprAlarmDTO() { Number = "SIN DATOS", LprLists = new List<string>() }, width, _lprHeight)
                    {
                        Name = Guid.NewGuid().ToString(),
                        Visible = true,
                        Size = new Size(width - 1, height - 1),
                    };
                    _panelList.Controls.Add(lprSinDatos);
                }

                if (i == 0) LoadImage(item.Id);
            }
        }

        private void LoadImage(int id)
        {
            Task.Run(async () =>
            {
                if (id > 0)
                {
                    var i = await _viewModel.GetLprSnapshot(id);
                    ViewImage(i.Data);
                }
                else
                {
                    var img = ImageHelper.ImageToArray(FileResources.NotImage);
                    this.ViewImage(img);
                }
            });
        }

        private void Lpr_Selected(object sender, LprAlarmDTO e)
        {
            ObjectSelected(this, _element);

            if (e != null)
            {
                var element = sender as LprElement;
                foreach (var el in this._panelList.Controls.OfType<LprElement>())
                {
                    if (el != element) el.SetUnselected();
                }
                element.SetSelected();

                LoadImage(e.Id);
            }
        }

        private void ViewImage(byte[] i)
        {
            using (var ms = new MemoryStream(i, 0, i.Length))
            {
                var image = Image.FromStream(ms, true);
                _panelPicture.BackgroundImage = image;
                _panelPicture.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }
    }
}
