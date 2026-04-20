using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.UserControls.Shared;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Sidebar.Element.ChekList
{
    public partial class PanelCheckList : PopedCotainer
    {
        public PanelCheckList()
        {
            InitializeComponent();
            this.dbFlowLayoutPanel1.Size = new Size(240, 52);
            this.dbFlowLayoutPanel1.Location = new Point(0, 0);
        }

        public void loadSource(List<CheckElementDTO> listElement)
        {
            this.dbFlowLayoutPanel1.Controls.Clear();

            this.Size = new Size(240, listElement.Any() ? 0 : 52);

            // Agregamos Items para deselcionar todos
            listElement.ForEach(item =>
            {
                if (dbFlowLayoutPanel1.Controls.Count == 0)
                {
                    this.CreateItemAll();
                }

                var element = new SidebarCheckBoxControl
                {
                    Name = "chk" + item.Key,
                    Size = new Size(240, 52),
                    Visible = true,
                    Margin = new Padding(0),
                };
                element.SetOption(item.Name, item.Key, item.State);
                if (listElement.Count < 12)
                {
                    dbFlowLayoutPanel1.Height += element.Height;
                    this.Height += element.Height;
                }

                element.ShowCheckBox = true;
                //element.ItemChangeState += Element_ItemChangeState;
                dbFlowLayoutPanel1.Controls.Add(element);
            });


            if (listElement.Count > 12)
            {
                this.dbFlowLayoutPanel1.Size = new Size(245, 624);
                this.Height = 624;
                this.Width = 246;
                this.dbFlowLayoutPanel1.HorizontalScroll.Maximum = 0;
                this.dbFlowLayoutPanel1.AutoScroll = false;
                this.dbFlowLayoutPanel1.HorizontalScroll.Visible = false;
                this.dbFlowLayoutPanel1.AutoScroll = true;
            }
        }

        private void Element_ItemChangeState(CheckElementDTO control)
        {
            if (control.Key.Equals("chkAll"))
            {
                //ar lst = dbFlowLayoutPanel1.Controls
                foreach (var item in dbFlowLayoutPanel1.Controls)
                {
                    if (item is SidebarCheckBoxControl && ((SidebarCheckBoxControl)item) is var scbc && !scbc.Item.Key.Equals("chkAll"))
                    {
                        var chk = control.State;
                        scbc.CheckedItem = chk;
                    }
                }
            }
        }

        public List<CheckElementDTO> GetStateElement()
        {
            var lst = new List<CheckElementDTO>();
            foreach (var control in dbFlowLayoutPanel1.Controls)
            {
                if (control is SidebarCheckBoxControl && ((SidebarCheckBoxControl)control).Item is var scbc && scbc.State)
                {
                    if (!scbc.Key.Equals("chkAll"))
                        lst.Add(scbc);
                }
            }
            return lst;
        }

        private void CreateItemAll()
        {
            var element = new SidebarCheckBoxControl
            {
                Name = "chkAll",
                Size = new Size(240, 52),
                Visible = true,
                Margin = new Padding(0),
            };
            element.SetOption("All Element", "chkAll", true);
            dbFlowLayoutPanel1.Height += element.Height;
            this.Height += element.Height;
            element.ShowCheckBox = true;
            element.ItemChangeState += Element_ItemChangeState;
            dbFlowLayoutPanel1.Controls.Add(element);

        }
    }
}
