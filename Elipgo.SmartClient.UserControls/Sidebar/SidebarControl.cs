using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services;
using Elipgo.SmartClient.ViewModels;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Sidebar
{
    public delegate void ObjectSelectedEventHandler(object sender, SidebarElementDTO element);
    public delegate void ObjectSelectedMouseUpEventHandler(object sender, SidebarElementDTO element);
    public delegate void ObjectSelectedDoubleClickEventHandler(object sender, SidebarElementDTO element);
    public delegate void ObjectSelectedGroupDoubleClickEventHandler(object sender, List<SidebarElementDTO> element);

    public partial class SidebarControl : UserControl
    {
        public event ObjectSelectedEventHandler ObjectSelected;
        public event ObjectSelectedMouseUpEventHandler ObjectMouseUp;
        public event ObjectSelectedDoubleClickEventHandler ObjectDoubleClick;
        public event ObjectSelectedGroupDoubleClickEventHandler ObjectGroupDoubleClick;

        public List<SidebarGroupElementDTO> sidebarElements = new List<SidebarGroupElementDTO>();
        public List<SidebarGroupElementDTO> sidebarElementsFiltered = new List<SidebarGroupElementDTO>();
        public List<SidebarGroupElementDTO> sidebarElementsFilteredText = new List<SidebarGroupElementDTO>();
        public List<CheckElementDTO> filterChecked = new List<CheckElementDTO>();
        private readonly string path = AppDomain.CurrentDomain.BaseDirectory;
        public MainViewModel mainView = Locator.Current.GetService<IScreen>() as MainViewModel;
        private readonly Configuration _config;

        public int WIDTH_SIDEBAR = 270;
        public int HEIGHT_BUTTON = 72;
        private Size clearTextImageSize;
        private int camerasCount = 0;
        private bool _resizeLoad = false;

        public SidebarControl()
        {
            InitializeComponent();
            _resizeLoad = true;
            WIDTH_SIDEBAR = this.Width - 20;
            HEIGHT_BUTTON = (this.Height * 8) / 100;

            CheckForIllegalCrossThreadCalls = false;

            ScrollBar.Maximum = PanSidebar.VerticalScroll.Maximum;
            ScrollBar.ThumbLength = 40;
            ScrollBar.BindingContainer = PanSidebar;
            ScrollBar.Width = 15;

            //PanSidebar.Width = WIDTH_SIDEBAR; // + 20;

            FilterTextbox.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);

            FindButton.Click += FindButton_Click;
            PanSidebar.Scroll += PanSidebar_Scroll;
            // PanSidebar.MouseWheel += PanSidebar_MouseWheel;
            FilterTextbox.PlaceholderText = Resources.search;
            bunifuToolTip1.SetToolTip(this.FindButton, Resources.search);

            if (!DesignMode)
            {
                try
                {
                    FindButton.BackgroundImage = FileResources.icon_search_input;
                    picBoxSitios.BackgroundImage = FileResources.icon_pin;
                    picBoxCameras.BackgroundImage = FileResources.icon_videocam;
                }
                catch (Exception) { }


                this.PanSidebar.Location = new System.Drawing.Point(0, 41);
                panCountSearch.Visible = false;
            }

            FilterTextbox.TextChange += FilterTextbox_TextChange;
            this.Resize += SidebarControl_Resize;
            _config = SmartClientEnvironmentUtils.GetConfiguration();

        }

        private void SidebarControl_Resize(object sender, EventArgs e)
        {
            if (ParentForm != null && ParentForm.ParentForm.WindowState == FormWindowState.Maximized)
                ResizeForm();
        }
        private void CustonDispose()
        {
            this.ReleaseElements();
            this.sidebarElements.Clear();
            sidebarElementsFiltered.Clear();
            filterChecked.Clear();
            sidebarElementsFilteredText.Clear();
            FindButton.Click -= FindButton_Click;
            PanSidebar.Scroll -= PanSidebar_Scroll;
            FilterTextbox.TextChange -= FilterTextbox_TextChange;
            this.Resize -= SidebarControl_Resize;

        }
        private void FilterTextbox_TextChange(object sender, EventArgs e)
        {
            if (FilterTextbox.Text.Trim().Length > 0)
                clearTextImage.Visible = true;
            else
                clearTextImage.Visible = false;
            this.Select();
        }

        private void PanSidebar_MouseWheel(object sender, MouseEventArgs e)
        {
            Application.DoEvents();
        }

        private void PanSidebar_Scroll(object sender, ScrollEventArgs e)
        {
            Application.DoEvents();
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            if (FilterTextbox.Text.Trim() == string.Empty)
            {
                lblSitios.Visible = false;
                lblCameras.Visible = false;
                LoadAllElements(sidebarElements);
                return;
            }
            FilterElements();
        }

        private void FilterElements()
        {
            camerasCount = 0;
            int siteCount = 0;
            var filter = FilterTextbox.Text.Trim().ToLower();
            if (filter == string.Empty)
            {
                LoadAllElements(sidebarElementsFiltered);
                return;
            }
            lblSitios.Visible = true;
            lblCameras.Visible = true;
            var sidebarOrigin = CloneClass.Copy(sidebarElementsFiltered);
            sidebarElementsFilteredText = sidebarOrigin.Where(a => a.Name.ToLower().Contains(filter) || (a.SidebarElements == null || a.SidebarElements.Any(b => b.Name.ToLower().Contains(filter)))).ToList();
            if (sidebarElementsFilteredText.All(a => a.SidebarElements == null))
            {
                sidebarElementsFilteredText = sidebarElementsFilteredText.Where(a => a.Name.ToLower().Contains(filter)).ToList();
            }
            foreach (var item in sidebarElementsFilteredText)
            {
                if (!item.Name.ToLower().Contains(filter))
                {
                    item.SidebarElements = item.SidebarElements.Where(a => a.Name.ToLower().Contains(filter)).ToList();
                    camerasCount = item.SidebarElements.Count;
                }
            }
            LoadAllElements(sidebarElementsFilteredText);
            siteCount = sidebarElementsFilteredText.Count;

            lblSitios.Text = "(" + siteCount.ToString() + ") " + (siteCount == 1 ? Resources.SiteText : Resources.sitesText);
            lblCameras.Text = "(" + camerasCount.ToString() + ") " + (camerasCount == 1 ? Resources.cameraText : Resources.camerasText);
            this.PanSidebar.Location = new System.Drawing.Point(0, 64);
            panCountSearch.Visible = true;
        }
        private void ReleaseElements()
        {
            foreach (var it in PanSidebar.Controls)
            {
                if (it is FlowLayoutPanel)
                {
                    foreach (var i in (it as FlowLayoutPanel).Controls)
                    {
                        if (i is SidebarGroupObjectControl)
                        {
                            (i as SidebarGroupObjectControl).ObjectClicked -= sidebarGroup_ObjectClicked;
                            (i as SidebarGroupObjectControl).ObjectGroupDoubleClick -= sidebarObjectGroup_DoubleClick;
                            (i as SidebarGroupObjectControl).Dispose();
                        }
                        if (i is SidebarObjectControl)
                        {
                            (i as SidebarObjectControl).ObjectClicked -= sidebarObject_ObjectClicked;
                            (i as SidebarObjectControl).ObjectMouseUp -= sidebarObject_MouseUp;
                            (i as SidebarObjectControl).ObjectDoubleClick -= sidebarObject_DoubleClick;
                            (i as SidebarObjectControl).Dispose();
                        }
                    }
                    (it as FlowLayoutPanel).Dispose();
                }

            }
            PanSidebar.Controls.Clear();
        }
        public void LoadAllElements(List<SidebarGroupElementDTO> datasource = null)
        {
            if (PanSidebar.InvokeRequired)
            {
                PanSidebar.Invoke((MethodInvoker)delegate
                {
                    LoadAllElements(datasource);
                });
                return;
            }

            if (datasource == null)
            {
                datasource = sidebarElements;
                FilterTextbox.Text = "";
                sidebarElementsFiltered = CloneClass.Copy(datasource);
            }

            ReleaseElements();

            datasource = datasource.OrderBy(x => x.Name).ToList();
            for (int x = 0; x < datasource.Count; x++)
            {
                if (datasource[x].SidebarElements != null)
                {
                    if (datasource[x].SidebarElements.Count != 0)

                    {
                        var g = new SidebarGroupObjectControl
                        {
                            Name = "sidebarGroup" + x,
                            Size = new Size(WIDTH_SIDEBAR, HEIGHT_BUTTON),
                            Visible = true,
                            Margin = new Padding(0),
                            ElementId = datasource[x].ElementId,
                            Elements = datasource[x].SidebarElements,
                            Anchor = AnchorStyles.Left | AnchorStyles.Right
                        };
                        g.ObjectClicked += sidebarGroup_ObjectClicked;
                        g.ObjectGroupDoubleClick += sidebarObjectGroup_DoubleClick;
                        g.SetValues(datasource[x].Name);

                        if (!string.IsNullOrEmpty(FilterTextbox.Text))
                        {
                            g.Opened = true;
                            g.ChangeBackgroundImage();
                        }
                        else
                            g.Opened = false;


                        var pg = new FlowLayoutPanel
                        {
                            Name = "panel" + x,
                            Visible = true,
                            Margin = new Padding(0),
                            Width = WIDTH_SIDEBAR,
                            Anchor = AnchorStyles.Left | AnchorStyles.Right
                        };
                        pg.Controls.Add(g);
                        pg.Height = g.Height;
                        PanSidebar.Controls.Add(pg);
                        if (!string.IsNullOrEmpty(FilterTextbox.Text))
                            Task.Run(() => ExpandGroup(g, true));
                    }
                    //else
                    //{
                    //    var elements = new List<SidebarObjectControl>();
                    //    var so = new SidebarObjectControl
                    //    {
                    //        Name = "sidebarObject" + x,
                    //        Size = new Size(WIDTH_SIDEBAR, HEIGHT_BUTTON),
                    //        Visible = true,
                    //        Anchor = AnchorStyles.Left | AnchorStyles.Right,
                    //        Margin = new Padding(0)
                    //    };
                    //    so.ObjectClicked += sidebarObject_ObjectClicked;
                    //    so.ObjectMouseUp += sidebarObject_MouseUp;
                    //    var element = new SidebarElementDTO()
                    //    {
                    //        ElementId = datasource[x].ElementId,
                    //        Name = datasource[x].Name,
                    //        DeviceType = datasource[x].DeviceType,
                    //        Status = null,
                    //        Key = datasource[x].Key,
                    //        DeviceTypeStr = datasource[x].DeviceTypeStr
                    //    };
                    //    so.SetValues(element);
                    //    var pg = new FlowLayoutPanel
                    //    {
                    //        Name = "panel" + x,
                    //        Visible = true,
                    //        Margin = new Padding(0),
                    //        Anchor = AnchorStyles.Left | AnchorStyles.Right,
                    //        Width = WIDTH_SIDEBAR
                    //    };
                    //    pg.Controls.Add(so);
                    //    PanSidebar.Controls.Add(pg);
                    //}
                }
                else
                {
                    var elements = new List<SidebarObjectControl>();
                    var so = new SidebarObjectControl
                    {
                        Name = "sidebarObject" + x,
                        Size = new Size(WIDTH_SIDEBAR, HEIGHT_BUTTON),
                        Visible = true,
                        Anchor = AnchorStyles.Left | AnchorStyles.Right,
                        Margin = new Padding(0)
                    };
                    so.ObjectClicked += sidebarObject_ObjectClicked;
                    so.ObjectDoubleClick += sidebarObject_DoubleClick;
                    so.ObjectMouseUp += sidebarObject_MouseUp;
                    var element = new SidebarElementDTO()
                    {
                        ElementId = datasource[x].ElementId,
                        Name = datasource[x].Name,
                        DeviceType = datasource[x].DeviceType,
                        Status = null,
                        Key = datasource[x].Key,
                        DeviceTypeStr = datasource[x].DeviceTypeStr
                    };
                    so.SetValues(element);
                    var pg = new FlowLayoutPanel
                    {
                        Name = "panel" + x,
                        Visible = true,
                        Margin = new Padding(0),
                        Anchor = AnchorStyles.Left | AnchorStyles.Right,
                        Width = WIDTH_SIDEBAR
                    };
                    pg.Controls.Add(so);
                    PanSidebar.Controls.Add(pg);
                }

            }
        }

        public void AddElementToSidebar(SidebarGroupElementDTO sidebarGroup)
        {
            var g = new SidebarGroupObjectControl
            {
                Name = "sidebarGroup" + PanSidebar.Controls.OfType<Panel>().Count(),
                Size = new Size(WIDTH_SIDEBAR, HEIGHT_BUTTON),
                Visible = true,
                Margin = new Padding(0)
            };
            g.ObjectClicked += sidebarGroup_ObjectClicked;
            g.SetValues(sidebarGroup.Name);
            g.Opened = false;

            var elements = new List<SidebarObjectControl>();
            sidebarGroup.SidebarElements.ForEach(se =>
            {
                var so = new SidebarObjectControl
                {
                    Name = "sidebarObject" + PanSidebar.Controls.OfType<Panel>().Count(),
                    Size = new Size(WIDTH_SIDEBAR, HEIGHT_BUTTON),
                    Visible = true,
                    Margin = new Padding(0)
                };
                so.ObjectClicked += sidebarObject_ObjectClicked;
                so.ObjectMouseUp += sidebarObject_MouseUp;
                so.ObjectDoubleClick += sidebarObject_DoubleClick;
                so.SetValues(se);

                elements.Add(so);
            });
            var pg = new FlowLayoutPanel
            {
                Name = "panel" + PanSidebar.Controls.OfType<Panel>().Count(),
                Visible = true,
                Margin = new Padding(0)
            };
            pg.Controls.Add(g);
            pg.Controls.AddRange(elements.ToArray());
            pg.Height = g.Height;

            PanSidebar.Controls.Add(pg);
        }

        private void sidebarObjectGroup_DoubleClick(object sender, List<SidebarElementDTO> Elements)
        {
            ObjectGroupDoubleClick(sender, Elements);
        }

        private void sidebarObject_DoubleClick(object sender, SidebarElementDTO element)
        {
            ObjectDoubleClick(sender, element);
        }

        private void sidebarObject_ObjectClicked(object sender, SidebarElementDTO element)
        {
            ObjectSelected(sender, element);
        }

        private void sidebarObject_MouseUp(object sender, SidebarElementDTO element)
        {
            ObjectMouseUp(sender, element);
        }

        public void SetSidebarElements(List<SidebarGroupElementDTO> sidebarGroups)
        {
            sidebarElements = sidebarGroups;
            LoadAllElements();
        }

        public void SetSidebarElementsFiltered(List<CheckElementDTO> list)
        {
            FilterTextbox.Text = "";
            filterChecked = list;
            var sidebarOrigin = CloneClass.Copy(sidebarElements);
            sidebarElementsFiltered = sidebarOrigin;
            foreach (var item in list)
            {
                if (item.Key == "All")
                {
                    if (item.State)
                    {
                        LoadAllElements(sidebarElementsFiltered);
                        return;
                    }
                    continue;
                }
                if (!item.State)
                    foreach (var a in sidebarElementsFiltered)
                    {
                        // Este fix verifica si alguna Ubicacion es del tipo "MAP".
                        // Se tiene que cambiar desde la definición de la clase el valor de la enumeración.
                        if (item.Key == "Map")
                        {
                            a.SidebarElements.RemoveAll(b => Enum.GetName(typeof(ElementType), b.DeviceType) == "Location");
                        }
                        else
                        {
                            a.SidebarElements.RemoveAll(b => Enum.GetName(typeof(ElementType), b.DeviceType) == item.Key);
                        }

                    }

            }
            LoadAllElements(sidebarElementsFiltered);
        }

        public void SetSidebarElementsRecorderFiltered(List<CheckElementDTO> list)
        {
            FilterTextbox.Text = "";
            filterChecked = list;
            var sidebarOrigin = CloneClass.Copy(sidebarElements);
            sidebarElementsFiltered = sidebarOrigin;
            foreach (var item in list)
            {
                if (item.Key == "All")
                {
                    if (item.State)
                    {
                        LoadAllElements(sidebarElementsFiltered);
                        return;
                    }
                    continue;
                }
                if (!item.State)
                    foreach (var a in sidebarElementsFiltered)
                        a.SidebarElements.RemoveAll(b => Enum.GetName(typeof(RecorderType), b.RecorderType) == item.Key);
            }
            LoadAllElements(sidebarElementsFiltered);
        }

        public void AddSidebarElement(SidebarGroupElementDTO sidebarGroup)
        {
            sidebarElements.Add(sidebarGroup);
            AddElementToSidebar(sidebarGroup);
        }

        private void sidebarGroup_ObjectClicked(object sender, bool state)
        {
            if (state == true)
            {// si state == true entonces cierro todos los sidebarcontrol
                foreach (var pg in PanSidebar.Controls.OfType<FlowLayoutPanel>())
                {
                    if (pg.Controls[0] is SidebarGroupObjectControl
                        && (pg.Controls[0] as SidebarGroupObjectControl) != (sender as SidebarGroupObjectControl)
                        && (pg.Controls[0] as SidebarGroupObjectControl).Opened)
                    {
                        Task.Run(() => ExpandGroup((pg.Controls[0] as SidebarGroupObjectControl), false));
                        (pg.Controls[0] as SidebarGroupObjectControl).Opened = false;
                        (pg.Controls[0] as SidebarGroupObjectControl).ChangeBackgroundImage();
                    }
                }
            }
            Task.Run(() => ExpandGroup(sender, state));
        }

        private void FilterTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                FilterElements();
            }
        }

        public SidebarElementDTO GetSidebarElementDTO(int elementId, ElementType[] type)
        {
            var flPanels = PanSidebar.Controls.OfType<FlowLayoutPanel>();
            var flPanel = flPanels?.FirstOrDefault(gp => (gp.Controls.OfType<SidebarObjectControl>()?.Any(g => g.Element.ElementId == elementId && type.Contains(g.Element.DeviceType)) ?? false));
            return flPanel?.Controls.OfType<SidebarObjectControl>().FirstOrDefault(p => p.Element.ElementId == elementId)?.Element;
        }

        public void UpdateSidebarElementDTO(SidebarElementDTO element)
        {
            var flPanels = PanSidebar.Controls.OfType<FlowLayoutPanel>();
            var flPanel = flPanels?.FirstOrDefault(gp => (gp.Controls.OfType<SidebarObjectControl>()?.Any(g => g.Element.ElementId == element.ElementId && g.Element.DeviceType == element.DeviceType) ?? false));
            var item = flPanel?.Controls.OfType<SidebarObjectControl>().FirstOrDefault(p => p.Element.ElementId == element.ElementId);
            if (item != null)
                item.SetValues(element);
        }

        public void AddSidebarElementDTO(SidebarElementDTO element)
        {
            var flPanels = PanSidebar.Controls.OfType<FlowLayoutPanel>();
            var flPanel = flPanels?.LastOrDefault(gp => (gp.Controls.OfType<SidebarObjectControl>()?.Any(g => g.Element.DeviceType == element.DeviceType) ?? false));
            if (flPanel != null)
            {
                var so = new SidebarObjectControl
                {
                    Name = "sidebarObject" + (element.ElementId * 10),
                    Size = new Size(WIDTH_SIDEBAR, HEIGHT_BUTTON),
                    Visible = true,
                    Margin = new Padding(0)
                };
                so.ObjectClicked += sidebarObject_ObjectClicked;
                so.ObjectMouseUp += sidebarObject_MouseUp;
                so.ObjectDoubleClick += sidebarObject_DoubleClick;
                so.SetValues(element);
                flPanel.Controls.Add(so);
                flPanel.Height += so.Height;
            }
        }

        public void UpdateSidebarElements(ElementType type, List<SidebarGroupElementDTO> datasource = null)
        {
            var flPanels = PanSidebar.Controls.OfType<FlowLayoutPanel>();
            var flPanel = flPanels?.LastOrDefault(gp => (gp.Controls.OfType<SidebarObjectControl>()?.Any(g => g.Element.DeviceType == type) ?? false));
            if (flPanel != null)
            {
                LoadAllElements(datasource);
            }
        }

        public IEnumerable<SidebarElementDTO> GetAllSidebarElementDTOActives()
        {
            var flPanels = PanSidebar.Controls.OfType<FlowLayoutPanel>();
            return flPanels.SelectMany(panel => panel.Controls.OfType<SidebarObjectControl>()?.Select(p => p.Element));
        }

        private void ClearTextImage_Click(object sender, EventArgs e)
        {
            FilterTextbox.Text = "";
            lblSitios.Visible = false;
            lblCameras.Visible = false;
            LoadAllElements(sidebarElements);
        }

        private async Task ExpandGroup(object sender, bool state)
        {
            if (this.InvokeRequired)
            {
                await this.InvokeAsync(() => ExpandGroup(sender, state));
                return;
            }

            var o = sender as SidebarGroupObjectControl;

            if (state)
            {
                /////////////////////////////////////////////
                // CARGAR ELEMENTOS DEL GRUPO BAJO DEMANDA //
                /////////////////////////////////////////////

                var listElements = o.Elements.Where(q => q.DeviceType != ElementType.Iot_In && q.DeviceType != ElementType.Iot_Out).ToList();

                var objects = new List<SidebarObjectControl>(listElements.Count);
                listElements.ForEach(se =>
                {
                    var so = new SidebarObjectControl
                    {
                        Name = "sidebarObject" + o.Name.Replace("sidebarGroup", ""),
                        Size = new Size(WIDTH_SIDEBAR, HEIGHT_BUTTON),
                        Visible = true,
                        Margin = new Padding(0),
                        Anchor = AnchorStyles.Left | AnchorStyles.Right
                    };
                    so.ObjectClicked += sidebarObject_ObjectClicked;
                    so.ObjectMouseUp += sidebarObject_MouseUp;
                    so.ObjectDoubleClick += sidebarObject_DoubleClick;
                    so.SetValues(se);
                    so.Visible = false;
                    objects.Add(so);
                });
                var pg = o.Parent as FlowLayoutPanel; //new FlowLayoutPanel
                o.BringToFront();
                pg.Controls.AddRange(objects.ToArray());

                objects.ForEach(x => x.Visible = true);
                /////////////////////////////////////////////
                // CARGAR ELEMENTOS DEL GRUPO BAJO DEMANDA //
                /////////////////////////////////////////////

                var elements = (o.Parent as Panel).Controls.OfType<SidebarObjectControl>();
                camerasCount = elements.Count();
                if (elements.Count() > 0)
                    (o.Parent as Panel).Height = o.Height + (elements.Count() * (elements.ToArray()[0] as SidebarObjectControl).Height);

                var verifyStatus = bool.TryParse(WorkFolderUtils.GetUserSettings("VerifyStatus", true), out bool preResult) ? preResult : false;
                if (verifyStatus)
                {
                    await Task.Run(async () =>
                    {
                        Enum.TryParse(mainView.NameModule, out Apps nameModule);
                        switch (nameModule)
                        {
                            case Apps.Live:
                                var listArray = listElements.Select(t => new CameraStatesLiveDTO { Id = t.ElementId, Apps = "Live" }).ToList();
                                TestDeviceFeatureAvailableDTO status = await Vmon5Service.postDeviceStatusTest(listArray, mainView.UserToken);
                                break;
                            case Apps.Playback:
                                //var listPlayback = listElements.Select(t => new CameraStatesDTO { Id = t.ElementId, RecorderDriver = t.RecorderDriver, RecorderId = t.RecorderId, RecorderType = t.RecorderType.ToString(), Apps = "Playback" }).ToList();
                                //var RecState = Vmon5Service.postCameraStatusRecordTest(listPlayback, mainView.UserToken);
                                break;
                        }
                    });
                }
            }
            else
            {
                (o.Parent as Panel).Height = o.Height;
                var els = (o.Parent as Panel).Controls.OfType<SidebarObjectControl>().ToList();
                foreach (var el in els)
                {
                    (o.Parent as Panel).Controls.Remove(el);
                    el.Dispose();
                }
                GC.Collect();
            }
        }

        public void ResizeForm()
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                HEIGHT_BUTTON = (this.Height * 9) / 100;
                if (HEIGHT_BUTTON > 0)
                {
                    WIDTH_SIDEBAR = this.Width - 20;

                    var workingArea = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                    clearTextImageSize = clearTextImageSize.Width > 0 ? clearTextImageSize : clearTextImage.Size;

                    PanSidebar.Width = this.Width;
                    foreach (Control c in PanSidebar.Controls)
                    {
                        c.Width = WIDTH_SIDEBAR;
                        foreach (Control d in c.Controls)
                        {
                            d.Width = WIDTH_SIDEBAR;
                            d.Height = HEIGHT_BUTTON;
                        }
                    }
                    FilterTextbox.Location = workingArea.Width < 1400 ? new System.Drawing.Point(20, 15) : new System.Drawing.Point(20, 5);
                    FindButton.Location = workingArea.Width < 1400 ? new System.Drawing.Point(247, 23) : new System.Drawing.Point(247, 8);
                    clearTextImage.Location = workingArea.Width < 1400 ? new System.Drawing.Point(245, 22) : new System.Drawing.Point(210, 8);

                    var PsearchWith = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.1354M), 2));
                    var PsearchHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0222), 2));
                    panCountSearch.Size = new Size(PsearchWith, PsearchHeight);

                    var sitioX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0125M), 2));
                    var sitioY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0074), 2));
                    lblSitios.Location = new Point(sitioX, sitioY);

                    var cameraX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0890M), 2));
                    var cameraY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0055), 2));
                    lblCameras.Location = new Point(cameraX, cameraY);

                    var FindButtonWith = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0125M), 2));
                    var FindButtonHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0222), 2));
                    FindButton.Size = new Size(FindButtonWith, FindButtonHeight);

                    var FilterTextboxWith = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.1281M), 2));
                    var FilterTextboxHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0259), 2));
                    FilterTextbox.Size = new Size(FilterTextboxWith, FilterTextboxHeight);

                    if (workingArea.Width >= 2000)
                    {
                        FilterTextbox.Font = FontHelper.GetRobotoRegular(FontSizes.Large_2, FontStyle.Regular, GraphicsUnit.Pixel);
                        var widthClear = (clearTextImageSize.Width * workingArea.Width) / 1920;
                        var heightClear = (clearTextImageSize.Height * workingArea.Height) / 1080;
                        clearTextImage.Size = new Size(widthClear, heightClear);
                        FindButton.Size = new Size(widthClear, heightClear);
                        FindButton.Location = new Point(clearTextImage.Location.X, clearTextImage.Location.Y);
                    }
                    else if (workingArea.Width > 1400 && workingArea.Width < 2000)
                    {
                        lblSitios.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                        lblCameras.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                        FilterTextbox.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                        //PanSidebar.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                        var widthClear = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0104M), 2));
                        var heightClear = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0175), 2));
                        clearTextImage.Size = new Size(widthClear, heightClear);
                        var FindButtonX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.1213M), 2));
                        var FindButtonY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0074), 2));
                        FindButton.Location = new Point(FindButtonX, FindButtonY);

                    }
                    else if (workingArea.Width <= 1366)
                    {

                        lblSitios.Font = FontHelper.GetRobotoRegular(FontSizes.Small_3, FontStyle.Regular, GraphicsUnit.Pixel);
                        lblCameras.Font = FontHelper.GetRobotoRegular(FontSizes.Small_3, FontStyle.Regular, GraphicsUnit.Pixel);
                        FilterTextbox.Font = FontHelper.GetRobotoRegular(FontSizes.Small_5, FontStyle.Regular, GraphicsUnit.Pixel);
                        //PanSidebar.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                        var widthClear = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0104M), 2));
                        var heightClear = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0175), 2));
                        clearTextImage.Size = new Size(widthClear, heightClear);
                        var FindButtonX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.1213M), 2));
                        var FindButtonY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0225), 2));
                        FindButton.Location = new Point(FindButtonX, FindButtonY);

                    }
                }
            }
            _resizeLoad = false;
        }
    }
}
