using Elipgo.SmartClient.Common.Helpers;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.GenericForm
{
    public partial class GenericFormPagination : UserControl
    {
        private int Take = 3;//Convert.ToInt32(Common.Properties.Settings.Default["CountByPage"].ToString());
        public int PropTake { get => Take; set => Take = value; }
        public int PropTotalPage { get => TotalPage; set => TotalPage = value; }
        private int Page = 1;
        private int TotalPage = 1;

        public Action<int> OnClickNextPage;
        public Action<int> OnClickBackPage;
        public Action<int> OnClickStartPage;
        public Action<int> OnClickEndPage;
        public GenericFormPagination(int SeleccionPage)
        {
            var _config = SmartClientEnvironmentUtils.GetConfiguration();
            Take = (_config.AppSettings.Settings["CountByPageGrid"] != null) ? int.Parse(_config.AppSettings.Settings["CountByPageGrid"].Value) : 3;
            InitializeComponent();
            Page = SeleccionPage;
            resize();
        }

        public void UpdatePage(int itemTotal, int SeleccionPage, bool add = false)
        {
            TotalPage = ((int)Math.Ceiling((double)itemTotal / this.Take));
            if (add)
                SeleccionPage = TotalPage;

            if (TotalPage > 1)
            {
                labelNextPage.Visible = true;
                labelEndPage.Visible = true;
                var pagNext = (SeleccionPage + 1);
                labelPag.Text = pagNext.ToString();
                if (SeleccionPage == 1)
                {
                    labelStartPage.Visible = false;
                    labelbackPage.Visible = false;
                    labelPagB.Visible = false;
                }
                labelPag.Visible = (pagNext > TotalPage ? false : true);
            }
            else
            {
                labelPag.Visible = false;
                labelStartPage.Visible = false;
                labelbackPage.Visible = false;
                labelNextPage.Visible = false;
                labelEndPage.Visible = false;
                labelPagB.Visible = false;
                labelN.Visible = false;
            }
            labelActual.Text = SeleccionPage.ToString();
            Page = 1;
        }

        private void resize()
        {
            labelActual.Size = new Size(15, 15);
            labelPag.Size = new Size(15, 15);
        }

        private void labelTotal_Click(object sender, EventArgs e)
        {

        }

        private void panelPage_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            NextPage();
        }

        private void labelNextPage_Click(object sender, EventArgs e)
        {
            NextPage();
        }

        private void labelbackPage_Click(object sender, EventArgs e)
        {
            BackPage();
        }

        private void labelStartPage_Click(object sender, EventArgs e)
        {
            Page = 1;
            labelStartPage.Visible = false;
            labelbackPage.Visible = false;
            labelPagB.Visible = false;
            labelNextPage.Visible = true;
            labelEndPage.Visible = true;
            labelPag.Visible = true;
            labelN.Visible = true;
            labelActual.Text = Page.ToString();
            labelPag.Text = (Page + 1).ToString();
            OnClickNextPage?.Invoke(int.Parse(labelActual.Text));
        }

        private void labelEndPage_Click(object sender, EventArgs e)
        {
            endPage();
        }

        private void labelPagB_Click(object sender, EventArgs e)
        {
            BackPage();
        }

        private void BackPage()
        {
            Page -= 1;

            if (Page == 1)
            {
                labelStartPage.Visible = false;
                labelbackPage.Visible = false;
                labelN.Visible = true;
            }
            labelNextPage.Visible = true;
            labelEndPage.Visible = true;
            labelActual.Text = Page.ToString();

            var pagNext = (Page + 1);
            labelPag.Text = pagNext.ToString();
            labelPag.Visible = (pagNext > TotalPage ? false : true);
            var pagBack = (Page - 1);
            labelPagB.Text = pagBack.ToString();
            labelPagB.Visible = (pagBack < 1 ? false : true);

            OnClickBackPage?.Invoke(int.Parse(labelActual.Text));
        }

        private void NextPage()
        {
            Page += 1;

            if (TotalPage == Page)
            {
                labelNextPage.Visible = false;
                labelEndPage.Visible = false;
                labelPag.Visible = false;
                labelN.Visible = false;
            }


            labelStartPage.Visible = true;
            labelbackPage.Visible = true;
            labelActual.Text = Page.ToString();
            var pagNext = (Page + 1);
            labelPag.Text = pagNext.ToString();
            labelPag.Visible = (pagNext > TotalPage ? false : true);
            var pagBack = (Page - 1);
            labelPagB.Text = pagBack.ToString();
            labelPagB.Visible = (pagNext < 1 ? false : true);

            labelN.Visible = (int.Parse(labelPag.Text) >= TotalPage ? false : true);
            OnClickNextPage?.Invoke(int.Parse(labelActual.Text));

        }

        /// <summary>
        /// Metodo que se seleccionar la ultima pagina.
        /// </summary>
        public void endPage()
        {
            Page = TotalPage;
            labelStartPage.Visible = true;
            labelbackPage.Visible = true;
            labelPagB.Visible = true;
            labelNextPage.Visible = false;
            labelEndPage.Visible = false;
            labelPag.Visible = false;
            labelN.Visible = false;
            labelActual.Text = Page.ToString();
            labelPagB.Text = (Page - 1).ToString();
            OnClickEndPage?.Invoke(int.Parse(labelActual.Text));
        }


    }
}
