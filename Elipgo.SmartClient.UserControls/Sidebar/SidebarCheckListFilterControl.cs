using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Sidebar
{
    public delegate void SelectEventHandler(string name, bool state);
    public delegate void SelectItemEventHandler(string name, bool state);
    public delegate void ChangeEventHandler(List<CheckElementDTO> list);
    public delegate void AddSideBarOptionCheckBoxCallBack(List<CheckElementDTO> options);
    public delegate void HideSideBarOptionCheckBoxCallBack(string name);
    public delegate void ShowSideBarOptionCheckBoxCallBack(string name);
    public delegate void EnableSideBarOptionCheckBoxCallBack(string name, bool enable);

    public partial class SidebarCheckListControl : UserControl
    {
        public string path = AppDomain.CurrentDomain.BaseDirectory;

        public List<ObjectStateFilter> OptionsState = new List<ObjectStateFilter>();

        public ObjectStateFilter SelectedOptionsState = new ObjectStateFilter();

        public event ChangeEventHandler ChangeCheckFilter;

        public event SelectItemEventHandler SelectItemEvent;
        private bool _resizeLoad = false;
        public bool ShowCheckBox { get; set; } = true;
        public int ListOptionHeight { get; set; } = 0;

        private AddSideBarOptionCheckBoxCallBack addSideBarOptionCheckBoxDelegate;
        private HideSideBarOptionCheckBoxCallBack hideSideBarOptionCheckBoxDelegate;
        private ShowSideBarOptionCheckBoxCallBack showSideBarOptionCheckBoxDelegate;
        private EnableSideBarOptionCheckBoxCallBack enableSideBarOptionCheckBoxDelegate;

        public SidebarCheckListControl()
        {
            addSideBarOptionCheckBoxDelegate = new AddSideBarOptionCheckBoxCallBack(LoadElement);
            hideSideBarOptionCheckBoxDelegate = new HideSideBarOptionCheckBoxCallBack(HideElement);
            showSideBarOptionCheckBoxDelegate = new ShowSideBarOptionCheckBoxCallBack(ShowElement);
            enableSideBarOptionCheckBoxDelegate = new EnableSideBarOptionCheckBoxCallBack(EnableElement);

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();
            _resizeLoad = true;
            this.Resize += SidebarCheckListControl_Resize;
            this.Disposed += SidebarCheckListControl_Disposed;
        }

        private void SidebarCheckListControl_Resize(object sender, EventArgs e)
        {
            SidebarCheckListControlResize();
        }

        private void G_CheckFilterSelectedClicked(string name, bool state)
        {
            bool all = false;
            var flagAll = false;
            bool notAll = false;

            if (name == "All" && state)
                all = true;
            else if (name == "All" && !state)
                notAll = true;
            else if (name != "All" && !state)
                flagAll = true;

            int options = SelectedOptionsState.options.Count;
            int check = 0;
            foreach (var item in SelectedOptionsState.options)
            {
                if (item.Key == name)
                    item.State = state;
                else if (all)
                    item.State = true;
                else if (flagAll)
                    if (item.Key == "All")
                        item.State = false;
            }
            foreach (var item in SelectedOptionsState.options)
            {
                if (item.Key != "All" && item.State)
                    check++;
            }
            if (notAll)
            {
                foreach (var item in SelectedOptionsState.options)
                {
                    item.State = false;
                }
            }
            else if (options == check + 1)
            {
                var s = SelectedOptionsState.options.Find(x => x.Key == "All");
                s.State = true;
            }
            LoadElement(SelectedOptionsState.options);
            ChangeCheckFilter?.Invoke(SelectedOptionsState.options);
        }

        private void G_ItemSelectedClicked(string name, bool state)
        {
            SelectItemEvent?.Invoke(name, state);
        }

        public void LoadData(List<ObjectStateFilter> filter)
        {
            OptionsState = filter;
        }

        public void LoadElement(List<CheckElementDTO> options)
        {
            if (panCheckListOption.InvokeRequired)
                this.Invoke(addSideBarOptionCheckBoxDelegate, new object[] { options });
            else
            {
                panCheckListOption.Height = 0;
                ReleaseElements();
                foreach (var item in options.Where(x => x.Visible == true).ToList())
                {
                    if (item.Separator)
                    {
                        var line = new SidebaseSeparatorControl();
                        panCheckListOption.Controls.Add(line);
                        panCheckListOption.Height += line.Height;
                        continue;
                    }

                    var g = new SidebarCheckBoxControl
                    {
                        Name = "check" + item.Key,
                        Size = new Size(277, 52),
                        Visible = true,
                        Margin = new Padding(0),
                        ShowCheckBox = ShowCheckBox
                    };
                    g.SetOption(item.Name, item.Key, item.State);
                    g.CheckFilterSelectedClicked += G_CheckFilterSelectedClicked;
                    g.ItemSelectedClicked += G_ItemSelectedClicked;
                    panCheckListOption.Height += g.Height;
                    ListOptionHeight = panCheckListOption.Height;
                    panCheckListOption.Controls.Add(g);
                }
                this.Height = panCheckListOption.Height + 6;
            }
        }

        public bool IsEmpty()
        {
            return panCheckListOption.Controls.Count == 0;
        }
        private void ReleaseElements()
        {
            foreach (var it in panCheckListOption.Controls)
            {
                if (it is SidebaseSeparatorControl)
                {
                    (it as SidebaseSeparatorControl).Dispose();
                }
                if (it is SidebarCheckBoxControl)
                {
                    (it as SidebarCheckBoxControl).CheckFilterSelectedClicked -= G_CheckFilterSelectedClicked;
                    (it as SidebarCheckBoxControl).ItemSelectedClicked -= G_ItemSelectedClicked;
                    (it as SidebarCheckBoxControl).Dispose();
                }
            }
            panCheckListOption.Controls.Clear();
        }
        public void LoadElement(ElementSidebar element)
        {
            ReleaseElements();
            panCheckListOption.Height = 0;
            SelectedOptionsState = OptionsState.Find(o => o.Type == element);

            foreach (var item in SelectedOptionsState.options)
            {
                if (item.Separator)
                {
                    var line = new SidebaseSeparatorControl();
                    panCheckListOption.Controls.Add(line);
                    panCheckListOption.Height += line.Height;
                    continue;
                }

                var g = new SidebarCheckBoxControl
                {
                    Name = "check" + item.Key,
                    Size = new Size(277, 52),
                    Visible = true,
                    Margin = new Padding(0),
                    ShowCheckBox = ShowCheckBox

                };
                g.SetOption(item.Name, item.Key, item.State);
                g.CheckFilterSelectedClicked += G_CheckFilterSelectedClicked;
                g.ItemSelectedClicked += G_ItemSelectedClicked;
                panCheckListOption.Height += g.Height;
                ListOptionHeight = panCheckListOption.Height;
                panCheckListOption.Controls.Add(g);
            }
            SidebarCheckListControlResize();
        }

        public void HideElement(string name)
        {
            Control[] sideBarCheckBoxControls = panCheckListOption.Controls.Find(name, true);

            if (sideBarCheckBoxControls != null && sideBarCheckBoxControls.Any())
            {
                foreach (Control sideBarCheckBox in sideBarCheckBoxControls)
                {
                    if (sideBarCheckBox.InvokeRequired)
                        this.Invoke(hideSideBarOptionCheckBoxDelegate, new object[] { name });
                    else
                        sideBarCheckBox.Visible = false;
                }
            }
        }

        public void EnableElement(string name, bool enable)
        {
            Control[] sideBarCheckBoxControls = panCheckListOption.Controls.Find(name, true);

            if (sideBarCheckBoxControls != null && sideBarCheckBoxControls.Any())
            {
                foreach (Control sideBarCheckBox in sideBarCheckBoxControls)
                {
                    if (sideBarCheckBox.InvokeRequired)
                        this.Invoke(enableSideBarOptionCheckBoxDelegate, new object[] { name, enable });
                    else
                        sideBarCheckBox.Enabled = enable;
                }
            }
        }

        public void ShowElement(string name)
        {
            Control[] sideBarCheckBoxControls = panCheckListOption.Controls.Find(name, true);

            if (sideBarCheckBoxControls != null && sideBarCheckBoxControls.Any())
            {
                foreach (Control sideBarCheckBox in sideBarCheckBoxControls)
                {
                    if (sideBarCheckBox.InvokeRequired)
                        this.Invoke(showSideBarOptionCheckBoxDelegate, new object[] { name });
                    else
                        sideBarCheckBox.Visible = true;
                }
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            this.Hide();
        }

        private void SidebarCheckListControl_VisibleChanged(object sender, EventArgs e)
        {
            panCheckListOption.Focus();
        }

        public void SidebarCheckListControlResize()
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var workingArea = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                foreach (var sidebarCheckBoxControl in panCheckListOption.Controls)
                {
                    if (sidebarCheckBoxControl is SidebarCheckBoxControl)
                    {
                        var sidebarCheckBoxControlWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.1445M), 2));
                        var sidebarCheckBoxControlHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.048M), 2));
                        (sidebarCheckBoxControl as SidebarCheckBoxControl).Size = new Size(sidebarCheckBoxControlWidth, sidebarCheckBoxControlHeight);

                        foreach (var sidebarCheckBoxCtl in (sidebarCheckBoxControl as SidebarCheckBoxControl).Controls)
                        {
                            if (workingArea.Width > 1400 && workingArea.Width < 2000)
                            {
                                if (sidebarCheckBoxCtl is System.Windows.Forms.Label)
                                    (sidebarCheckBoxCtl as System.Windows.Forms.Label).Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Regular, GraphicsUnit.Pixel);

                                if (sidebarCheckBoxCtl is Bunifu.UI.WinForms.BunifuCheckBox)
                                    (sidebarCheckBoxCtl as Bunifu.UI.WinForms.BunifuCheckBox).Font = FontHelper.GetRobotoRegular(FontSizes.Small_2, FontStyle.Regular, GraphicsUnit.Pixel);
                            }
                            else if (workingArea.Width > 1366 && workingArea.Width <= 1400)
                            {
                            }
                            else if (workingArea.Width <= 1366)
                            {
                                if (sidebarCheckBoxCtl is System.Windows.Forms.Label)
                                    (sidebarCheckBoxCtl as System.Windows.Forms.Label).Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);

                                if (sidebarCheckBoxCtl is Bunifu.UI.WinForms.BunifuCheckBox)
                                    (sidebarCheckBoxCtl as Bunifu.UI.WinForms.BunifuCheckBox).Font = FontHelper.GetRobotoRegular(FontSizes.Small_0, FontStyle.Regular, GraphicsUnit.Pixel);

                                if (sidebarCheckBoxCtl is System.Windows.Forms.Panel)
                                {
                                    foreach (var ctrl in ((System.Windows.Forms.Control)sidebarCheckBoxCtl).Controls)
                                    {
                                        if (ctrl is System.Windows.Forms.Label)
                                        {
                                            (ctrl as System.Windows.Forms.Label).Font = FontHelper.GetRobotoRegular(FontSizes.Small_2, FontStyle.Regular, GraphicsUnit.Pixel);
                                        }
                                    }
                                }

                            }
                            else if (workingArea.Width >= 2000 && workingArea.Width < 2560)
                            {
                                if (sidebarCheckBoxCtl is System.Windows.Forms.Label)
                                    (sidebarCheckBoxCtl as System.Windows.Forms.Label).Font = FontHelper.GetRobotoRegular(FontSizes.Small_8, FontStyle.Regular, GraphicsUnit.Pixel);

                                if (sidebarCheckBoxCtl is Bunifu.UI.WinForms.BunifuCheckBox)
                                    (sidebarCheckBoxCtl as Bunifu.UI.WinForms.BunifuCheckBox).Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Regular, GraphicsUnit.Pixel);
                            }
                            else if (workingArea.Width >= 2560 && workingArea.Width <= 3440)
                            {
                                if (sidebarCheckBoxCtl is System.Windows.Forms.Label)
                                    (sidebarCheckBoxCtl as System.Windows.Forms.Label).Font = FontHelper.GetRobotoRegular(FontSizes.Small_9, FontStyle.Regular, GraphicsUnit.Pixel);

                                if (sidebarCheckBoxCtl is Bunifu.UI.WinForms.BunifuCheckBox)
                                    (sidebarCheckBoxCtl as Bunifu.UI.WinForms.BunifuCheckBox).Font = FontHelper.GetRobotoRegular(FontSizes.Small_8, FontStyle.Regular, GraphicsUnit.Pixel);

                            }

                            if (sidebarCheckBoxCtl is System.Windows.Forms.Panel)
                            {
                                foreach (var ctrl in ((System.Windows.Forms.Control)sidebarCheckBoxCtl).Controls)
                                {
                                    if (ctrl is System.Windows.Forms.Label)
                                    {
                                        var LabelWith = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0526M), 2));
                                        var LabelHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0351M), 2));
                                        (ctrl as System.Windows.Forms.Label).Size = new Size(LabelWith, LabelHeight);
                                    }

                                    if (ctrl is Bunifu.UI.WinForms.BunifuCheckBox)
                                    {
                                        var BunifuCheckBoxWith = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0078M), 2));
                                        var BunifuCheckBoxHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0138M), 2));
                                        (ctrl as Bunifu.UI.WinForms.BunifuCheckBox).Size = new Size(BunifuCheckBoxWith, BunifuCheckBoxHeight);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            _resizeLoad = true;
        }

        private void SidebarCheckListControl_Disposed(object sender, EventArgs e)
        {
            if (panCheckListOption.Controls.Count > 0)
            {
                foreach (Control it in panCheckListOption.Controls)
                {
                    if (it is SidebarCheckBoxControl)
                    {
                        (it as SidebarCheckBoxControl).CheckFilterSelectedClicked -= G_CheckFilterSelectedClicked; ;
                        (it as SidebarCheckBoxControl).ItemSelectedClicked -= G_ItemSelectedClicked;
                        panCheckListOption.Height -= it.Height;
                        (it as SidebarCheckBoxControl).Dispose();
                    }
                    it.Dispose();
                }
            }
            this.Resize -= SidebarCheckListControl_Resize;
            this.Disposed -= SidebarCheckListControl_Disposed;
        }

    }
}
