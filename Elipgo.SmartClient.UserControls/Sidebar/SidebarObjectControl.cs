using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.ViewModels;
using ReactiveUI;
using Splat;
using System;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Sidebar
{
    public delegate void ObjectClickEventHandler(object sender, SidebarElementDTO element);
    public delegate void ObjectMouseUpEventHandler(object sender, SidebarElementDTO element);
    public delegate void ObjectDoubleClickEventHandler(object sender, SidebarElementDTO element);

    public partial class SidebarObjectControl : UserControl
    {
        public event ObjectClickEventHandler ObjectClicked;
        public event ObjectMouseUpEventHandler ObjectMouseUp;
        public event ObjectDoubleClickEventHandler ObjectDoubleClick;
        public Guid Key = Guid.Empty;
        public SidebarElementDTO Element = null;
        //private readonly string path = AppDomain.CurrentDomain.BaseDirectory;
        public MainViewModel mainView = Locator.Current.GetService<IScreen>() as MainViewModel;
        private readonly Configuration _config;
        private bool _resizeLoad = false;
        public SidebarObjectControl()
        {
            InitializeComponent();
            _resizeLoad = true;
            this.BackColorChanged += SidebarObjectControl_BackColorChanged;

            //PanelStatus.Visible = true;

            this.MouseDown += SidebarObject_MouseDown;
            //this.PanelButton.MouseDown += SidebarObject_MouseDown;
            this.DoubleClick += SidebarObjectControl_DoubleClick;

            this.LabelName.MouseDown += SidebarObject_MouseDown;
            this.LabelSubName.MouseDown += SidebarObject_MouseDown;
            this.PanelIcon.MouseDown += SidebarObject_MouseDown;

            this.MouseUp += SidebarObject_MouseUp;
            //this.PanelButton.MouseUp += SidebarObject_MouseUp;
            this.LabelName.MouseUp += SidebarObject_MouseUp;
            this.LabelSubName.MouseUp += SidebarObject_MouseUp;
            this.PanelIcon.MouseUp += SidebarObject_MouseUp;

            this.MouseDoubleClick += SidebarObjectControl_DoubleClick;
            //this.PanelButton.MouseDoubleClick += SidebarObjectControl_DoubleClick;
            this.LabelName.MouseDoubleClick += SidebarObjectControl_DoubleClick;
            this.LabelSubName.MouseDoubleClick += SidebarObjectControl_DoubleClick;
            //this.PanelStatus.MouseDoubleClick += SidebarObjectControl_DoubleClick;
            this.PanelIcon.MouseDoubleClick += SidebarObjectControl_DoubleClick;

            this.DragDrop += SidebarObjectControl_DragDrop;

            PanelIcon.BackgroundImageLayout = ImageLayout.Stretch;

            //PanelButton.BringToFront();
            this.Resize += SidebarObjectControl_Resize;

            _config = SmartClientEnvironmentUtils.GetConfiguration();

        }

        private void SidebarObjectControl_Resize(object sender, EventArgs e)
        {
            ResizeForm();
        }

        private void SidebarObjectControl_DragDrop(object sender, DragEventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND);
        }

        private void SidebarObject_MouseUp(object sender, MouseEventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND);
        }

        private void SidebarObjectControl_DoubleClick(object sender, EventArgs e)
        {
            ObjectDoubleClick(this, Element);
        }

        private void SidebarObject_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks != 1)
            {
                return;
            }

            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);

            Cursor.Current = Cursors.SizeAll;
            ObjectMouseUp(this, Element);
            this.DoDragDrop(Element, DragDropEffects.Move);

            Cursor.Current = Cursors.Default;

            Threads.RunInOtherThread(new Action[]
            {
                () => Thread.Sleep(500)
            }, () => this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND));

        }

        private void SidebarObjectControl_BackColorChanged(object sender, EventArgs e)
        {
            this.BackColor = this.BackColor;
            //PanelButton.BackColor = this.BackColor;
        }

        public void SetValues(SidebarElementDTO element)
        {
            Element = element;
            Key = element.Key;
            LabelName.Text = element.Name;

            switch (element.DeviceType)
            {
                case ElementType.Camera:
                    if (PanelIcon.BackgroundImage == null)
                    {
                        PanelIcon.BackgroundImage = FileResources.siderbar_icon_camara;
                    }
                    break;
                case ElementType.Iot_In:
                    PanelIcon.BackgroundImage = FileResources.siderbar_icon_iot_input;
                    break;
                case ElementType.Iot_Out:
                    PanelIcon.BackgroundImage = FileResources.siderbar_icon_iot_output;
                    break;
                case ElementType.Carousel:
                    PanelIcon.BackgroundImage = FileResources.siderbar_icon_carrusel;
                    break;
                case ElementType.Kpi:
                    PanelIcon.BackgroundImage = FileResources.siderbar_icon_kpi;
                    break;
                case ElementType.Blueprint:
                case ElementType.Location:
                    PanelIcon.BackgroundImage = FileResources.siderbar_icon_map;
                    break;
                case ElementType.AlarmsMap:
                case ElementType.Geomap:
                    PanelIcon.BackgroundImage = FileResources.siderbar_icon_location;
                    break;
                case ElementType.Face:
                    PanelIcon.BackgroundImage = FileResources.siderbar_icon_kpi;
                    break;
                case ElementType.Lpr:
                    PanelIcon.BackgroundImage = FileResources.siderbar_icon_kpi;
                    break;
                default:
                    break;
            }
            ShowSubLabel(element);
        }

        private void ShowSubLabel(SidebarElementDTO element)
        {
            ResizeForm();
            if (element.DeviceType == ElementType.Camera && !string.IsNullOrEmpty(element.RecorderName))
            {
                LabelSubName.Text = element.RecorderName;
                LabelSubName.Visible = true;
            }
        }

        private void ResizeForm()
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                if (main.Width > 1400 && main.Width < 2000)
                {
                    LabelName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    LabelSubName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width >= 1366 && main.Width <= 1400)
                {
                    LabelName.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    LabelSubName.Font = FontHelper.GetRobotoRegular(FontSizes.Small_3, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width < 1366)
                {
                    LabelName.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Regular, GraphicsUnit.Pixel);
                    LabelSubName.Font = FontHelper.GetRobotoRegular(FontSizes.Small_1, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width >= 2000 && main.Width <= 2560)
                {
                    LabelName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    LabelSubName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width > 2560 && main.Width <= 3440)
                {
                    LabelName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    LabelSubName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                }

                var LabelNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.033M), 2));
                var LabelNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.013M), 2));
                LabelName.Location = new Point(LabelNameX, LabelNameY);

                var LabelSubNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.033M), 2));
                var LabelSubNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.019M), 2));
                LabelSubName.Location = new Point(LabelSubNameX, LabelSubNameY);

                var PanelIconSize = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0210M), 2));
                var PanelIconX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.01M), 2)) + 1;
                var PanelIconY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014M), 2));

                PanelIcon.Size = new Size(PanelIconSize, PanelIconSize);
                PanelIcon.Location = new Point(PanelIconX, PanelIconY);
                _resizeLoad = false;
            }
        }
    }
}