using System.Windows.Forms;

namespace Elipgo.SmartClient.Common
{
    public class DBFlowLayoutPanel : FlowLayoutPanel
    {
        public DBFlowLayoutPanel()
        {
            this.DoubleBuffered = true;
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            this.Invalidate();
            base.OnScroll(se);
        }
    }
}