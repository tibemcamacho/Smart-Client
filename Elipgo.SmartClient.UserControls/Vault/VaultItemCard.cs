using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Elipgo.SmartClient.UserControls.Vault
{
    public delegate void OnItemSelected(object sender, VaultItemCardDTO item);
    public delegate void OnContextMenuClick(object sender, VaultItemCardDTO item, Point point);
    public delegate void OnItemSelectedClick(object sender, VaultItemCardDTO item);


    public partial class VaultItemCard : UserControl
    {
        public event OnItemSelected ItemSelected;
        public event OnContextMenuClick ContextMenuClick;
        public event OnItemSelectedClick ItemSelectedClick;

        public VaultItemCardDTO Item { get; set; }

        public string Label1Text { get => Label1.Text; set => Label1.Text = value; }
        public string Label2Text { get => Label2.Text; set => Label2.Text = value; }
        public List<int> bookmarDownloadkList = new List<int>();

        public int Progress
        {
            set =>
                //if (this.ProgressBar.InvokeRequired)
                //{
                //    this.ProgressBar.Invoke((MethodInvoker)delegate
                //    {
                //        this.ProgressBar.Value = value;
                //    });
                //}
                //else 
                //{
                //    this.ProgressBar.Value = value;
                //}
                Item.Progress = value;
        }

        public VaultItemCardState State
        {
            set
            {
                try
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => ChangeStateProgressBar(value)));
                    }
                    else
                    {
                        ChangeStateProgressBar(value);
                    }
                }
                catch (ObjectDisposedException) { }
            }
        }

        private DispatcherTimer progressbarTimer = new DispatcherTimer();

        private Color _backColor;

        public VaultItemCard()
        {
            InitializeComponent();
        }

        public VaultItemCard(VaultItemCardDTO item)
        {
            InitializeComponent();

            Label1.Text = StringHelper.Truncate(item.Label1, Convert.ToInt32(VariableResources.TRUNCATE), "...");
            Label1.Text = !string.IsNullOrEmpty(Label1.Text) ? Label1.Text.ToLower() : Label1.Text;
            Label2.Text = item.Label2;
            //Label3.Text = Resources.generating;
            if (String.IsNullOrEmpty(Label1.Text)) Label1.Text = "BookMark " + item.Id.ToString();

            this.Name = item.Id.ToString();

            this.IconPanel.BackgroundImage = FileResources.icon_bookmarks;
            this.ProgressBar.Value = 0;
            this.ProgressBar.Visible = false;

            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND);
            _backColor = this.BackColor;

            Item = item;

            progressbarTimer.Interval = TimeSpan.FromMilliseconds(500);
            progressbarTimer.Tick += ProgressBarTimer_Tick;

            this.Disposed += VaultItemCard_Disposed;
        }

        private void VaultItemCard_Disposed(object sender, EventArgs e)
        {
            progressbarTimer.Stop();
            progressbarTimer.Tick -= ProgressBarTimer_Tick;
        }

        public void UnSelected()
        {
            this.BackColor = _backColor;
        }

        private void VaultItemCard_Click(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
            if (Form.ModifierKeys == Keys.Control)
            {
                ItemSelectedClick?.Invoke(this, Item);
            }
            else
            {
                ItemSelected?.Invoke(this, Item);
            }
        }

        private void VaultItemCard_DoubleClick(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);

            ItemSelected?.Invoke(this, Item);
        }

        private void ButtonContexMenu_Click(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);

            ContextMenuClick?.Invoke(this, Item, Cursor.Position);
        }

        private void ChangeStateProgressBar(VaultItemCardState value)
        {
            switch (value)
            {
                case VaultItemCardState.Begin:
                    this.Label3.Text = Resources.starting;
                    this.Label3.Visible = true;
                    this.ProgressBar.Visible = true;
                    break;
                case VaultItemCardState.InProgress:
                    this.Label2.Visible = false;
                    this.Label3.Text = Resources.Generating;
                    this.ProgressBar.Visible = this.Label3.Visible = (value == VaultItemCardState.InProgress);
                    if (!progressbarTimer.IsEnabled)
                    {
                        progressbarTimer.Start();
                    }

                    break;
                case VaultItemCardState.End:
                    this.Label2.Visible = false;
                    this.ProgressBar.Visible = this.Label3.Visible = false;
                    progressbarTimer.Stop();
                    break;
                case VaultItemCardState.Waiting:
                    this.Label3.Text = Resources.Waiting;
                    this.Label3.Visible = true;
                    break;
            }
        }

        private void ProgressBarTimer_Tick(object sender, EventArgs e)
        {
            this.ProgressBar.Value += 10;

            if (this.ProgressBar.Value == 100)
            {
                this.ProgressBar.Value = 10;
            }
        }
    }
}
