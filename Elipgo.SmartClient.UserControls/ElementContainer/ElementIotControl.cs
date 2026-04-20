using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Drivers;
using Splat;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.ElementContainer
{
    public partial class ElementIotControl : UserControl
    {
        // * Uniview *
        //private TreeNodeInfo m_CurSelectTreeNodeInfo = new TreeNodeInfo();
        //private List<DeviceInfo> m_deviceInfoList = null;
        //private IntPtr m_lpDevHandle = IntPtr.Zero;

        private string path = AppDomain.CurrentDomain.BaseDirectory;
        private readonly IDriverFactory DriverFactory = Locator.Current.GetService<IDriverFactory>();
        public event ObjectSelectedEventHandler ObjectSelected;
        public event IOPortStateEventHandler OnIOPortState;
        public event ObjectDoubleClickSelectedEventHandler ObjectSelectedDoubleClick;
        private SidebarElementDTO Element { get; set; }
        private CameraDTO Device { get; set; }
        public IOPortState State { get; set; }
        public SidebarElementDTO _element;
        public CameraDTO _dtoElement;
        public ElementIotControl(SidebarElementDTO element, CameraDTO device)
        {
            Element = element;
            Device = device;
            _element = element;
            _dtoElement = device;
            //TODO: ELIMINAR LA SIGUIENTE LINEA CUANDO SE ENCUENTRE EL NUEVO DEPLOY
            Device.ChannelType = element.DeviceType == ElementType.Iot_In ? ChannelType.DI : ChannelType.DO;

            InitializeComponent();
            this.PictureBox.Location = new Point((this.Width / 2) - (this.PictureBox.Width / 2), (this.Height / 2) - (this.PictureBox.Height / 2));
            this.PictureBox.BackgroundImage = element.DeviceType == ElementType.Iot_In ? FileResources.input : FileResources.output;
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND);

            this.Click += ElementIotControl_Click;
            this.DoubleClick += ElementIotControl_DoubleClick;
        }

        private void ElementIotControl_DoubleClick(object sender, EventArgs e)
        {
            ObjectSelectedDoubleClick?.Invoke(this);
        }

        private void ElementIotControl_Click(object sender, EventArgs e)
        {
            Console.WriteLine("IOT State");
            ObjectSelected(this, Element);
            GetState();
        }

        private void ElementIotControl_Resize(object sender, EventArgs e)
        {
            this.PictureBox.Location = new Point((this.Width / 2) - (this.PictureBox.Width / 2), (this.Height / 2) - (this.PictureBox.Height / 2));
        }

        public void GetState()
        {
            IOPortState state = IOPortState.Offline;
            //IManufactureUri driver = DriverFactory.GetDriverApiCgi(Device);

            // para este IDriverLive requiere un profile en el constructor, podes utilizar main o sub stream indistintamente
            IDriverLive driverLive = DriverFactory.GetDriverLive(Device);

            if (Device.ChannelType == ChannelType.DI)
            {
                state = driverLive.InputPortState();
            }
            else
            {
                state = driverLive.OuputPortState();
            }
            State = state;
            OnIOPortState(this, Element, state);
        }

        public bool ToogleOutput()
        {
            if (State == IOPortState.Offline)
            {
                return false;
            }

            var state = State == IOPortState.Active ? IOPortState.Inactive : IOPortState.Active;
            IDriverLive driverLive = DriverFactory.GetDriverLive(Device);
            driverLive.OuputPortChangeState(state);

            GetState();
            return true;
        }

    }
}
