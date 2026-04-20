using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Common.Reflections;
using Elipgo.SmartClient.Drivers;
using Elipgo.SmartClient.ViewModels;
using Splat;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Outputs
{
    public partial class OutputPanelControl : UserControl
    {
        private readonly IDriverFactory DriverFactory = Locator.Current.GetService<IDriverFactory>();
        public LiveViewModel ViewModel { get; set; }

        public OutputPanelControl(LiveViewModel viewModel)
        {
            InitializeComponent();
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);

            ViewModel = viewModel;

        }

        private void Element_ItemChangeState(int outputId, IOPortState state)
        {
            Task.Run(async () =>
            {
                CameraDTO device =await ViewModel.GetCamera(outputId);
                if (device != null)
                {
                    IManufactureUri driver = DriverFactory.GetDriverApiCgi(device);
                    driver.OuputPortChangeState(state);
                    ViewModel.SendNotificationIot(new IotDTO { active = state == IOPortState.Active });
                }
            });
        }

        private Task GetOutputState(int outputId)
        {
            return Task.Run(async () =>
            {
                CameraDTO device = await ViewModel.GetCamera(outputId);
                if (device != null)
                {
                    IManufactureUri driver = DriverFactory.GetDriverApiCgi(device);
                    SetOutputState(outputId, driver.OuputPortState());
                }
            });
        }

        public void SetOutputState(int outputId, IOPortState state)
        {
            foreach (var control in FlowLayoutPanel.Controls)
            {
                if (control is OutputItems && ((OutputItems)control).OutputId == outputId)
                {
                    ((OutputItems)control).SetState(state);
                }
            }
        }

        public void SetContent(List<CatalogIot> iots)
        {
            iots.ForEach(item =>
            {
                var element = new OutputItems(item.ObjectId, item.Name);
                element.Name = "OutputToggleItem_" + item.ObjectId;
                element.ItemChangeState += Element_ItemChangeState;
                GetOutputState(item.ObjectId);
                FlowLayoutPanel.Controls.Add(element);
            });
        }
    }
}
