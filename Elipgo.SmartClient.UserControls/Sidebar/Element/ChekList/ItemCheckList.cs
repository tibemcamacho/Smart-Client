using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.UserControls.Shared;
using Elipgo.SmartClient.UserControls.Sidebar.Element.ChekList;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Sidebar.Element
{
    public partial class ItemCheckList : UserControl
    {

        private PoperContainer poper;

        private PanelCheckList content;

        public ItemCheckList()
        {
            InitializeComponent();

            content = new PanelCheckList();
            poper = new PoperContainer(content);
        }

        public void loadSource(List<CheckElementDTO> listElement)
        {
            this.content.loadSource(listElement);
        }

        public List<CheckElementDTO> GetItemChecked()
        {
            return this.content.GetStateElement();
        }

        private void G_CheckFilterSelectedClicked(string name, bool state)
        {

        }

        private void fBtnCheck_Click(object sender, EventArgs e)
        {

        }

        private void fBtnCheck_MouseHover(object sender, EventArgs e)
        {

        }

        private void fBtnCheck_MouseMove(object sender, MouseEventArgs e)
        {
            //System.Console.WriteLine(e.Location.X + ":" + e.Location.Y);
        }

        private void fBtnCheck_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.poper.Show(this);
            }
        }


    }
}
