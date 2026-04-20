using Elipgo.SmartClient.UserControls.Sidebar;
using System;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    public partial class ToolBarAlarm : UserControl
    {
        public ToolBarAlarm()
        {
            InitializeComponent();
        }

        private void createComponentSidebarFIlterControl()
        {
            SidebarFilterControl sdbFilter = new SidebarFilterControl();
            sdbFilter.ButtonFilterClicked += SdbFilter_ButtonFilterClicked;
        }

        private void SdbFilter_ButtonFilterClicked(object sender, object e)
        {
            throw new NotImplementedException();
        }
    }
}
