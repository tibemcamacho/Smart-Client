using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.UserControls.Shared;
using Elipgo.SmartClient.UserControls.Sidebar;
using Elipgo.SmartClient.UserControls.UserProfile;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.PlaybackControls
{
    public partial class ItemsZoom : PopedCotainer
    {
        public event ObjectSelectEventHandler ItemSelectedClicked;
        public ItemsZoom()
        {
            InitializeComponent();
            this.dbFlowLayoutPanel1.Size = new Size(240, 52);
            this.dbFlowLayoutPanel1.Location = new Point(0, 0);
        }

        public void LoadSource(List<CheckElementDTO> listElement)
        {
            foreach (var it in this.dbFlowLayoutPanel1.Controls)
            {
                if (it is ItemControl)
                {
                    (it as ItemControl).ItemSelectedClicked -= Element_ItemSelectedClicked;
                    (it as ItemControl).Dispose();
                }
            }
            this.dbFlowLayoutPanel1.Controls.Clear();

            this.Size = new Size(240, listElement.Any() ? 0 : 52);

            listElement.Where(x => x.Visible == true).ToList().ForEach(item =>
            {
                var element = new ItemControl();
                element.Name = "item" + item.Key;
                element.Size = new Size(240, 36);
                element.Visible = item.Visible;
                element.Margin = new Padding(0);
                element.Label = item.Name;
                element.Icon = item.Icon;
                element.Item = item;
                element.ItemSelectedClicked += Element_ItemSelectedClicked;
                dbFlowLayoutPanel1.Height += element.Height;
                Height += element.Height;
                dbFlowLayoutPanel1.Controls.Add(element);
            });
        }

        private void Element_ItemSelectedClicked(string name, bool state)
        {
            ItemSelectedClicked?.Invoke(name, state);
        }

        public void CustomDispose()
        {
            foreach (var item in dbFlowLayoutPanel1.Controls)
            {
                if (item is ItemControl)
                {
                    (item as ItemControl).ItemSelectedClicked -= Element_ItemSelectedClicked;
                    Height -= (item as ItemControl).Height;
                    dbFlowLayoutPanel1.Height -= (item as ItemControl).Height;
                    (item as ItemControl).Dispose();
                }

            }
            dbFlowLayoutPanel1.Controls.Clear();
        }
    }
}
