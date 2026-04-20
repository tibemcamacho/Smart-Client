using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services.Services.Interface;
using Elipgo.SmartClient.UserControls.Sidebar;
using Splat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.GenericForm
{

    public partial class GenericFormControl : UserControl
    {
        private readonly int Take = Convert.ToInt32(Common.Properties.Settings.Default["CountByPage"].ToString());

        private int Page = 1;

        private readonly int MAX_ITEMS = 8;

        public Action<ContentFormDTO, GenericFormItemCard> OnDoubleClickCard;

        public Action<ContentFormDTO, bool> AddedElement;

        public Action<ContentFormDTO, bool> EditedElement;

        public Action<ContentFormDTO, bool> DeletedElement;

        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();

        private IAuditService _auditService = Locator.Current.GetService<IAuditService>();
        private readonly ISmartNotification _notification = Locator.Current.GetService<ISmartNotification>();

        private bool _resizeLoad = false;
        public UserControl Grid { get; set; }

        public ContentFormDTO ItemSelected { get; set; }

        public GenericFormItemCard CardSelected { get; set; }

        public ConfigGenericForm Configuration
        {
            get; set;
        }

        public GenericContentComponent Control { get; set; }

        public GenericContentComponent enabledButtonAccept { get; set; }

        private bool NewOrGrill = false;

        public GenericFormControl()
        {
            InitializeComponent();
            _resizeLoad = true;
            this.ButtonCancel.Location = ButtonOK.Location;
            this.ClientSizeChanged += GenericFormControl_ClientSizeChanged;
            this.LocationChanged += GenericFormControl_LocationChanged;

            dropdownItems1.OnClickIconDropdownItems += OnClickIconDropdownItems;

            //Resize();
            LocationContentBody(false);
        }

        private void GenericFormControl_ClientSizeChanged(object sender, EventArgs e)
        {
            //Resize();
        }

        private void GenericFormControl_LocationChanged(object sender, EventArgs e)
        {
            //Resize();
        }

        public List<ContentFormDTO> DataSource
        {
            get;
            set;
        }

        public List<ContentFormDTO> DataSourceAllElements
        {
            get;
            set;
        }

        private static Bunifu.UI.WinForms.BunifuButton.BunifuButton EnabledButtonOk
        {
            get => ButtonOK;
            set => ButtonOK = value;
        }
        public static void EnabledButtonAccep(bool status)
        {
            ButtonOK.Enabled = status;
        }
        public static void EnabledButtonOkEvent(bool status)
        {
            EnabledButtonOk.Enabled = status;
        }
        private void RenderItems(List<ContentFormDTO> data)
        {
            ContentList.Hide();


            if (Configuration?.ObjectBarSelected == LiveBarButtom.guard)
            {
                dropdownItems1.optionState = true;
            }
            else
            {
                dropdownItems1.optionState = false;
            }

            dropdownItems1.DataSourceItems(data);

            this.editBtn.Enabled = true;
            this.IndexEntity.Visible = false;
            this.ButtonNew.Enabled = true;
            spinner.Hide();
        }

        private void NoItems(string ControlName, bool hasContent)
        {
            if (!hasContent)
            {
                ContentList.Hide();

                var control = new GenericFormNoData(ControlName);
                control.Name = "FormNoData";
                ContentList.Controls.Add(control);
                ContentList.Show();
                CalculateScrollVisbility();
                this.ButtonNew.Enabled = this.editBtn.Enabled = true;
                spinner.Hide();
                if (Configuration?.ObjectBarSelected == LiveBarButtom.guard)
                {
                    Name = ((ViewModels.GuardViewModel)Control.ViewModel).Driver.Camera.Name;
                    _notification.Show(String.Format(Resources.NoGuard, Name), null);
                }
            }
            else
            {
                var searchResult = ContentList.Controls.Find("FormNoData", true);

                if (searchResult != null && searchResult.Any())
                {
                    ContentList.Controls.Remove(searchResult[0]);
                }
            }
        }

        public async Task OnActiveChange(bool value, ContentFormDTO item)
        {
            bool result = await Control.SwitchChange(value, item);
            if (result)
            {
                await this._auditService.Add(new AuditDTO()
                {
                    CodeAction = (value ? AuditAction.START_GUARD : AuditAction.STOP_GUARD).ToString(),
                    Param01 = item.Id.ToString()
                }, ((ViewModels.GuardViewModel)Control.ViewModel).Token);
                DataSource = Control.GetDataSource(null).Result;
                ContentList.Controls.Clear();
                RenderItems(DataSource);
            }
        }

        private void OnDoublreClickCard(ContentFormDTO item, GenericFormItemCard card)
        {
            //OnSelectCard(item, card);
            OnSelectOption(item);
            ParentForm.Hide();
            Control.DobleClick(item);
            OnDoubleClickCard?.Invoke(item, card);
            ParentForm.DialogResult = DialogResult.OK;
            ParentForm.Close();
        }
        private void setVisivility()
        {
            bool AllowShowAsGrid = false;
            bool AllowNew = false;
            bool AllowEdit = false;
            bool AllowDelete = false;
            if (Configuration?.ObjectBarSelected == LiveBarButtom.groups)
            {
                if (Control.ParentApps == Common.Enum.Apps.Live)
                {
                    AllowShowAsGrid = appAuthorization.Exist(ButtonsContextBar.Grid.GetAttribute<PermissionLive>().PermissionKey);
                    AllowNew = appAuthorization.Exist(ButtonsContextBar.CreateGroup.GetAttribute<PermissionLive>().PermissionKey);
                    AllowEdit = appAuthorization.Exist(ButtonsContextBar.EditGroup.GetAttribute<PermissionLive>().PermissionKey);
                    AllowDelete = appAuthorization.Exist(ButtonsContextBar.DeleteGroup.GetAttribute<PermissionLive>().PermissionKey);
                }
                else if (Control.ParentApps == Common.Enum.Apps.Playback)
                {
                    AllowShowAsGrid = appAuthorization.Exist(ButtonsContextBar.Grid.GetAttribute<PermissionLive>().PermissionKey);
                    AllowNew = appAuthorization.Exist(ButtonsContextBar.CreateGroup.GetAttribute<PermissionPlayback>().PermissionKey);
                    AllowEdit = appAuthorization.Exist(ButtonsContextBar.EditGroup.GetAttribute<PermissionPlayback>().PermissionKey);
                    AllowDelete = appAuthorization.Exist(ButtonsContextBar.DeleteGroup.GetAttribute<PermissionPlayback>().PermissionKey);
                }
                ButtonNew.Visible = AllowNew && Configuration.ShowAddButton;
                editBtn.Visible = AllowNew;
                sidebarCheckListControl.LoadElement(new List<CheckElementDTO>
                {
                    // Cuando es grupo, agrego una nueva opción.
                    new CheckElementDTO
                    {
                         Key = "showgrid",
                         Name = Resources.showgrid,
                         State = false,
                         Visible = AllowShowAsGrid
                    },
                    new CheckElementDTO
                    {
                         Key = "edit",
                         Name = Resources.edit,
                         State = false,
                         Visible = AllowEdit
                    },
                    new CheckElementDTO
                    {
                         Key = "delete",
                         Name = Resources.delete,
                         State = false,
                         Visible = AllowDelete
                    }
                });
            }
            else if (Configuration?.ObjectBarSelected == LiveBarButtom.scenes)
            {
                if (Control.ParentApps == Common.Enum.Apps.Live)
                {
                    AllowNew = appAuthorization.Exist(ButtonsContextBar.CreateScene.GetAttribute<PermissionLive>().PermissionKey);
                    AllowEdit = appAuthorization.Exist(ButtonsContextBar.EditScene.GetAttribute<PermissionLive>().PermissionKey);
                    AllowDelete = appAuthorization.Exist(ButtonsContextBar.DeleteScene.GetAttribute<PermissionLive>().PermissionKey);
                }
                else if (Control.ParentApps == Common.Enum.Apps.Playback)
                {
                    AllowNew = appAuthorization.Exist(ButtonsContextBar.CreateScene.GetAttribute<PermissionPlayback>().PermissionKey);
                    AllowEdit = appAuthorization.Exist(ButtonsContextBar.EditScene.GetAttribute<PermissionPlayback>().PermissionKey);
                    AllowDelete = appAuthorization.Exist(ButtonsContextBar.DeleteScene.GetAttribute<PermissionPlayback>().PermissionKey);
                }
                ButtonNew.Visible = AllowNew && Configuration.ShowAddButton;
                editBtn.Visible = true;
                //160;
                if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
                {
                    var workingArea = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                    sidebarCheckListControl.Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.1481M), 2));

                }
                sidebarCheckListControl.LoadElement(new List<CheckElementDTO>
                {
                    new CheckElementDTO
                    {
                         Key = "edit",
                         Name = Resources.edit,
                         State = false,
                         Visible = AllowEdit
                    },
                    new CheckElementDTO
                    {
                         Key = "delete",
                         Name = Resources.delete,
                         State = false,
                         Visible = AllowDelete
                    },
                    new CheckElementDTO
                    {
                         Key = "exec",
                         Name = Resources.Exec,
                         State = false
                    }
                });
            }
            else if (Configuration?.ObjectBarSelected == LiveBarButtom.carousel)
            {
                if (Control.ParentApps == Common.Enum.Apps.Live)
                {
                    AllowNew = appAuthorization.Exist(ButtonsContextBar.CreateCarousel.GetAttribute<PermissionLive>().PermissionKey);
                    AllowEdit = appAuthorization.Exist(ButtonsContextBar.EditCarousel.GetAttribute<PermissionLive>().PermissionKey);
                    AllowDelete = appAuthorization.Exist(ButtonsContextBar.DeleteCarousel.GetAttribute<PermissionLive>().PermissionKey);
                    ButtonOK.Enabled = false;
                }
                else if (Control.ParentApps == Common.Enum.Apps.Playback)
                {
                    AllowNew = appAuthorization.Exist(ButtonsContextBar.CreateCarousel.GetAttribute<PermissionPlayback>().PermissionKey);
                    AllowEdit = appAuthorization.Exist(ButtonsContextBar.EditCarousel.GetAttribute<PermissionPlayback>().PermissionKey);
                    AllowDelete = appAuthorization.Exist(ButtonsContextBar.DeleteCarousel.GetAttribute<PermissionPlayback>().PermissionKey);
                }
                ButtonNew.Visible = AllowNew && Configuration.ShowAddButton;
                editBtn.Visible = AllowNew;
                sidebarCheckListControl.LoadElement(new List<CheckElementDTO>
                {
                    new CheckElementDTO
                    {
                         Key = "edit",
                         Name = Resources.edit,
                         Visible = AllowEdit,
                         State = false
                    },
                    new CheckElementDTO
                    {
                         Key = "delete",
                         Name = Resources.delete,
                         State = false,
                         Visible = AllowDelete
                    }
                });
            }
            else if (Configuration?.ObjectBarSelected == LiveBarButtom.preset)
            {
                if (Control.ParentApps == Common.Enum.Apps.Live)
                {
                    AllowNew = appAuthorization.Exist(ButtonsContextBar.CreatePreset.GetAttribute<PermissionLive>().PermissionKey);
                    AllowEdit = appAuthorization.Exist(ButtonsContextBar.ModifyPresets.GetAttribute<PermissionLive>().PermissionKey);
                    AllowDelete = appAuthorization.Exist(ButtonsContextBar.DeletePresets.GetAttribute<PermissionLive>().PermissionKey);
                }
                else if (Control.ParentApps == Common.Enum.Apps.Playback)
                {
                    AllowNew = appAuthorization.Exist(ButtonsContextBar.CreatePreset.GetAttribute<PermissionPlayback>().PermissionKey);
                    AllowEdit = appAuthorization.Exist(ButtonsContextBar.ModifyPresets.GetAttribute<PermissionPlayback>().PermissionKey);
                    AllowDelete = appAuthorization.Exist(ButtonsContextBar.DeletePresets.GetAttribute<PermissionPlayback>().PermissionKey);
                }
                else if (Control.ParentApps == Common.Enum.Apps.None)
                {
                    AllowNew = appAuthorization.Exist(ButtonsContextBar.CreatePreset.GetAttribute<PermissionFullScreen>().PermissionKey);
                    AllowEdit = appAuthorization.Exist(ButtonsContextBar.ModifyPresets.GetAttribute<PermissionFullScreen>().PermissionKey);
                    AllowDelete = appAuthorization.Exist(ButtonsContextBar.DeletePresets.GetAttribute<PermissionFullScreen>().PermissionKey);

                }
                ButtonNew.Visible = AllowNew && Configuration.ShowAddButton;
                editBtn.Visible = false;
                ButtonNew.Visible = Configuration.ShowAddButton;
                FindButton.Visible = Configuration.ShowFilterControls;
                //FilterTextbox.Visible = Configuration.ShowFilterControls;
                playButton.Visible = Configuration.ShowPlay;
                destroyElement.Visible = Configuration.CanDelete;
                lockElement.Visible = Configuration.CanPrivate;
                ButtonNew.Visible = Configuration.CanEditOrCreate;
                ButtonOK.Enabled = true;
                sidebarCheckListControl.LoadElement(new List<CheckElementDTO>
                {
                    new CheckElementDTO
                    {
                         Key = "edit",
                         Name = Resources.edit,
                         Visible = AllowEdit,
                         State = false
                    },
                    new CheckElementDTO
                    {
                         Key = "delete",
                         Name = Resources.delete,
                         State = false,
                         Visible = AllowDelete
                    }
                });

            }
            else if (Configuration?.ObjectBarSelected == LiveBarButtom.guard)
            {
                if (Control.ParentApps == Common.Enum.Apps.Live)
                {
                    AllowNew = appAuthorization.Exist(ButtonsContextBar.CreateGuard.GetAttribute<PermissionLive>().PermissionKey);
                    AllowEdit = appAuthorization.Exist(ButtonsContextBar.EditGuard.GetAttribute<PermissionLive>().PermissionKey);
                    AllowDelete = appAuthorization.Exist(ButtonsContextBar.DeleteGuard.GetAttribute<PermissionLive>().PermissionKey);
                }
                else if (Control.ParentApps == Common.Enum.Apps.Playback)
                {
                    AllowNew = appAuthorization.Exist(ButtonsContextBar.CreateGuard.GetAttribute<PermissionPlayback>().PermissionKey);
                    AllowEdit = appAuthorization.Exist(ButtonsContextBar.EditGuard.GetAttribute<PermissionPlayback>().PermissionKey);
                    AllowDelete = appAuthorization.Exist(ButtonsContextBar.DeleteGuard.GetAttribute<PermissionPlayback>().PermissionKey);
                }
                else
                {
                    AllowNew = appAuthorization.Exist(ButtonsContextBar.CreateGuard.GetAttribute<PermissionLive>().PermissionKey);
                    AllowEdit = appAuthorization.Exist(ButtonsContextBar.EditGuard.GetAttribute<PermissionLive>().PermissionKey);
                    AllowDelete = appAuthorization.Exist(ButtonsContextBar.DeleteGuard.GetAttribute<PermissionLive>().PermissionKey);
                }
                ButtonNew.Visible = AllowNew && Configuration.ShowAddButton;
                editBtn.Visible = false;
                ButtonNew.Visible = Configuration.ShowAddButton;
                FindButton.Visible = Configuration.ShowFilterControls;
                //FilterTextbox.Visible = Configuration.ShowFilterControls;
                playButton.Visible = Configuration.ShowPlay;
                destroyElement.Visible = Configuration.CanDelete;
                lockElement.Visible = Configuration.CanPrivate;
                ButtonNew.Visible = Configuration.CanEditOrCreate;
                sidebarCheckListControl.LoadElement(new List<CheckElementDTO>
                {
                    new CheckElementDTO
                    {
                         Key = "active",
                         Name = Resources.Active,
                         Visible = AllowEdit,
                         State = false
                    },
                    new CheckElementDTO
                    {
                         Key = "Inactive",
                         Name = Resources.Inactive,
                         Visible = AllowEdit,
                         State = false
                    },
                    new CheckElementDTO
                    {
                         Key = "edit",
                         Name = Resources.edit,
                         Visible = AllowEdit,
                         State = false
                    },
                    new CheckElementDTO
                    {
                         Key = "delete",
                         Name = Resources.delete,
                         State = false,
                         Visible = AllowDelete
                    }
                });
            }
            else
            {
                editBtn.Visible = false;
                ButtonNew.Visible = Configuration.ShowAddButton;
                FindButton.Visible = Configuration.ShowFilterControls;
                //FilterTextbox.Visible = Configuration.ShowFilterControls;
                playButton.Visible = Configuration.ShowPlay;
                destroyElement.Visible = Configuration.CanDelete;
                lockElement.Visible = Configuration.CanPrivate;
                ButtonNew.Visible = Configuration.CanEditOrCreate;
                sidebarCheckListControl.LoadElement(new List<CheckElementDTO>
                {
                    new CheckElementDTO
                    {
                         Key = "edit",
                         Name = Resources.edit,
                         State = false
                    },
                    new CheckElementDTO
                    {
                         Key = "delete",
                         Name = Resources.delete,
                         State = false
                    }
                });
            }
        }

        public void InitializeCustomComponent()
        {

            lblTitle.Text = Configuration?.NameEntity;
            //IndexEntity.Text = string.Format(Resources.listOf, Configuration?.NameEntity);

            ScrollBar.ThumbLength = 220;
            ScrollBar.BindingContainer = ContentList;
            ScrollBar.BackgroundColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);

            ButtonOK.Text = Resources.ButtonOK;
            //ButtonNew.Text = Resources.ButtonNew;
            ButtonCancel.Text = Resources.cancel;
            ButtonAcept.Text = Resources.ButtonOK;
            //editBtn.Text = Resources.buttonEditSelected;
            lblTitle.ForeColor = ColorTranslator.FromHtml(VariableResources.COLOR_TITLE_FONT);
            BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
            PanelHeader.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_HEADER_BACKGROUND);

            pictureBoxGeneric.Image = Configuration.IconEntity;

            sidebarCheckListControl.ShowCheckBox = false;
            sidebarCheckListControl.SelectItemEvent += SidebarCheckListControl_SelectItemEvent;
            setVisivility();
            //ButtonCancel.Location = ButtonOK.Location;
        }

        private async void SidebarCheckListControl_SelectItemEvent(string name, bool state)
        {
            switch (name)
            {
                // Mostrar grilla, cuando es "Grupos".
                case "showgrid":
                    ShowGridButton_Click(null, EventArgs.Empty);
                    break;
                case "edit":
                    EditButton_Click(null, EventArgs.Empty);
                    break;
                case "exec":
                    ExecButton_Click(null, EventArgs.Empty);
                    break;
                case "active":
                    await OnActiveChange(true, ItemSelected);
                    break;
                case "Inactive":
                    await OnActiveChange(false, ItemSelected);
                    break;
                default:
                    PapeleraButton_Click(null, EventArgs.Empty);
                    break; ;
            }

            sidebarCheckListControl.Hide();
        }

        private async void ButtonOK_Click(object sender, EventArgs e)
        {
            var obj = await Control.SaveOrUpdate();
            if (obj == null)
            {
                return;
            }

            var objDS = DataSource.FirstOrDefault(p => p.Id == obj.Id);

            DataSource.Add(obj);
            if (objDS != null)
            {
                DataSource.Remove(objDS);
                var key = obj.Id.ToString();
                RenderItems(DataSource);
                //if (ContentList.Controls.ContainsKey(key))
                //{
                //    GenericFormItemCard item = ContentList.Controls[key] as GenericFormItemCard;
                //    item.UpdateItem(obj);
                //    EditedElement?.Invoke(obj, true);
                //}
            }
            else
            {
                RenderItems(DataSource);
            }


            Control.Hide();
            ContentList.Show();
            ScrollBar.Show();
            ShowFilterControls(true);
            ShowMessageNoItems();
            LocationContentBody(false);
            //ButtonCancel.Location = ButtonOK.Location;
            ResizaPreajuste();
            dropdownItems1.Visible = true;
            ButtonNew.Visible = true;
            ButtonAcept.Visible = true;
        }

        private void OnSelectCard(ContentFormDTO item, GenericFormItemCard card)
        {
            if (CardSelected != null && item.Id != ItemSelected.Id)
            {
                sidebarCheckListControl.Hide();
                CardSelected.Uncheck();
            }

            CardSelected = card;
            ItemSelected = item;
            Control.SelectedItem = CardSelected;
        }

        private void OnSelectOption(ContentFormDTO item)
        {
            if (CardSelected != null && item.Id != ItemSelected.Id)
            {
                sidebarCheckListControl.Hide();
                CardSelected.Uncheck();
            }

            //CardSelected = card;
            ItemSelected = item;
            Control.SelectedItemOption = ItemSelected;
        }

        private void OnClickIconContextMenu(ContentFormDTO item, GenericFormItemCard card, Point point)
        {
            bool isCardRight = false;
            if (ContentList.Controls.Count > 4)
            {
                var totalCards = ContentList.Controls.Count;
                var CRight = Math.Round(Convert.ToDecimal(ContentList.Controls.Count / 4));
                for (int i = 1; i <= CRight; i++)
                {
                    if (ContentList.Controls[(i * 4) - 1].Name == item.Id.ToString())
                    {
                        isCardRight = true;
                    }
                }
            }


            if (sidebarCheckListControl.Visible && item.Id == ItemSelected.Id)
            {
                sidebarCheckListControl.Hide();
            }
            else
            {
                if (!sidebarCheckListControl.IsEmpty())
                {
                    if (isCardRight)
                    {
                        point = new Point(point.X - sidebarCheckListControl.Width, point.Y);
                    }

                    sidebarCheckListControl.Location = sidebarCheckListControl.ParentForm.PointToClient(point);
                    sidebarCheckListControl.Show();
                }
            }
            OnSelectCard(item, card);
        }

        private void OnClickIconDropdownItems(ContentFormDTO item, Point point)
        {
            if (!String.IsNullOrEmpty(dropdownItems1.SelectValue))
            {
                item = DataSource.SingleOrDefault(s => s.Id == int.Parse(dropdownItems1.SelectValue));

                if (sidebarCheckListControl.Visible) //&& item.Id == ItemSelected.Id
                {
                    sidebarCheckListControl.Hide();
                }
                else
                {
                    if (!sidebarCheckListControl.IsEmpty())
                    {
                        point = new Point(point.X - (sidebarCheckListControl.Width + 10), point.Y - 40);

                        if (Configuration?.ObjectBarSelected == LiveBarButtom.guard)
                        {
                            if (item.Switch == true)
                            {
                                sidebarCheckListControl.HideElement("Active");
                                sidebarCheckListControl.ShowElement("Inactive");
                            }
                            else
                            {
                                sidebarCheckListControl.HideElement("Inactive");
                                sidebarCheckListControl.ShowElement("Active");
                            }
                        }

                        sidebarCheckListControl.Location = sidebarCheckListControl.ParentForm.PointToClient(point);
                        sidebarCheckListControl.Show();
                        sidebarCheckListControl.BringToFront();


                    }
                }

                OnSelectOption(item);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            if (this.ParentForm != null)
            {
                this.ParentForm.DialogResult = DialogResult.Cancel;
                this.ParentForm.Close();
                SetCancelAction();

            }
        }

        private void ButtonGoBack_Click(object sender, EventArgs e)
        {
            Control.GoBack();
            Control.Hide();
            ContentList.Show();
            ScrollBar.Show();
            ShowFilterControls(true);
            ShowMessageNoItems();
            SetCancelAction();
        }

        private void ButtonNew_Click(object sender, EventArgs e)
        {
            panelPage.Visible = false;
            ShowForm();
            ShowFilterControls(false);
            SetGoBackAction();
            LocationContentBody(true);
            NewOrGrill = true;
            dropdownItems1.Visible = false;
            ResizaPreajuste();
            ButtonNew.Visible = false;
            editBtn.Visible = false;
            ButtonAcept.Visible = false;
        }

        public void ShowForm()
        {
            if (Control != null)
            {
                Control.Clear();
                ContentList.Hide();
                ScrollBar.Hide();
                Control.Show();
                ShowFilterControls(true);
            }
        }

        private void SetGoBackAction()
        {
            var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
            ButtonCancel.Left = ButtonOK.Left - Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.09259M), 2));
            ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            ButtonCancel.Click += new System.EventHandler(this.ButtonGoBack_Click);
        }

        private void SetCancelAction()
        {
            ButtonCancel.Click -= new System.EventHandler(this.ButtonGoBack_Click);
            ButtonCancel.Click -= new System.EventHandler(this.ButtonCancel_Click);
            //ButtonCancel.Location = ButtonOK.Location;
        }

        public void ShowFilterControls(bool visible)
        {
            if (Configuration?.ObjectBarSelected == LiveBarButtom.groups)
            {
                FindButton.Visible = visible;
                //FilterTextbox.Visible = visible;
                clearTextImage.Visible = visible;
            }
        }

        private void IndexEntity_Click(object sender, EventArgs e)
        {
            Control.Hide();
            Control.Clear();
            ContentList.Show();
            CalculateScrollVisbility();
            //ButtonCancel.Location = ButtonOK.Location;
            ShowFilterControls(true);
            NewOrGrill = false;
            LocationContentBody(false);
            ResizaPreajuste();
            ButtonNew.Visible = true;
            dropdownItems1.Visible = true;
            ButtonAcept.Visible = true;
            RenderItems(DataSource);
            if (Configuration?.ObjectBarSelected == LiveBarButtom.groups || Configuration?.ObjectBarSelected == LiveBarButtom.carousel)
            {
                editBtn.Visible = true;
            }
        }

        private void CalculateScrollVisbility()
        {
            if (ContentList.Controls.Count > MAX_ITEMS)
            {
                ScrollBar.Show();
            }
            else
            {
                ScrollBar.Hide();
            }
        }

        private async void GenericFormControl_Load(object sender, EventArgs e)
        {
            ButtonOK.Enabled = false;
            if (!DesignMode)
            {
                InitializeCustomComponent();

                if (Control != null)
                {
                    Control.Hide();
                    ContentBody.Controls.Add(Control);
                    Control.VisibleChanged += Control_VisibleChanged;
                }

                switch (Configuration?.NameEntity)
                {
                    case "Guardias":
                    case "guard":
                    case "Preset":
                    case "Preajustes":
                        DataSourceAllElements = await Control.GetDataSource(null);
                        if (DataSourceAllElements.Count > 0)
                        {

                            labelTotal.Text = ((int)Math.Ceiling((double)DataSourceAllElements.Count() / this.Take)).ToString();
                            TextNumberPage.Text = this.Page.ToString();
                            panelPage.Visible = true;
                            labelbackPage.Visible = false;
                            labelStartPage.Visible = false;
                        }
                        else
                        {
                            panelPage.Visible = false;
                            ShowMessageNoItems();
                        }

                        break;
                    default:
                        panelPage.Visible = false;
                        DataSource = await Control.GetDataSource(null);
                        if (DataSource.Count == 0)
                        {
                            ShowMessageNoItems();
                        }
                        else
                        {
                            RenderItems(DataSource);
                        }
                        break;
                }
            }
        }

        private async void ShowMessageNoItems()
        {
            DataSource = await Control.GetDataSource(null);
            NoItems(this.lblTitle.Text, !(DataSource.Count == 0));
        }

        private void Control_VisibleChanged(object sender, EventArgs e)
        {
            if (Control.Visible)
            {
                ButtonOK.Show();
            }
            else
            {
                ButtonOK.Hide();
            }
        }

        private async void EditButton_Click(object sender, EventArgs e)
        {
            if (Control != null)
            {
                Control.SelectedItemOption = DataSource.FirstOrDefault(t => t.Id == int.Parse(dropdownItems1.SelectValue));
                if (await Control.Edit())
                {
                    panelPage.Visible = false;
                    ContentList.Hide();
                    ScrollBar.Hide();
                    Control.Show();
                    ShowFilterControls(false);
                    SetGoBackAction();
                    LocationContentBody(true);
                    NewOrGrill = true;
                    dropdownItems1.Visible = false;
                    ButtonAcept.Visible = false;
                }
                if (Configuration?.ObjectBarSelected == LiveBarButtom.preset)
                {
                    await this._auditService.Add(new AuditDTO()
                    {
                        CodeAction = AuditAction.SAVE_PRESET.ToString(),
                        Param01 = this.CardSelected.Name
                    }, ((ViewModels.PresetViewModel)Control.ViewModel).Token);
                }
                ResizaPreajuste();
            }
        }

        private async void ExecButton_Click(object sender, EventArgs e)
        {
            if (await Control.Execute())
            {
                if (Configuration?.ObjectBarSelected == LiveBarButtom.scenes)
                {
                    await this._auditService.Add(new AuditDTO()
                    {
                        CodeAction = AuditAction.EXEC_SCENE.ToString(),
                        Param01 = this.ItemSelected.Label1
                    }, ((Scenes.ScenesControl)Control).ScenesViewModel.Token);
                }

                MessageBox.Show(string.Format(Resources.ExecuteElement), Resources.ExecuteTitle, MessageBoxButtons.OK);
                sidebarCheckListControl.Hide();
            }
        }

        // Al seleccionar una opción, se muestra la grilla desde el grupo.
        private void ShowGridButton_Click(object sender, EventArgs e)
        {
            // Cuando el módulo es "Grupos", y el elemento de la grilla es seleccionado...
            if (!string.IsNullOrEmpty(dropdownItems1.SelectValue))
            {
                OnDoublreClickCard(ItemSelected, CardSelected);
            }
        }

        private async void PapeleraButton_Click(object sender, EventArgs e)
        {
            if (ItemSelected != null)
            {
                if (MessageBox.Show(string.Format(Resources.deleteElementQuestion, ItemSelected.Label1),
                                                    Resources.delete,
                                                    MessageBoxButtons.OKCancel,
                                                    MessageBoxIcon.Question
                                    ) == DialogResult.OK
                )
                {
                    if (await Control.Delete())
                    {
                        var objDS = DataSource.FirstOrDefault(p => p.Id == ItemSelected.Id);

                        if (objDS != null)
                        {
                            DataSource.Remove(objDS);
                            var key = ItemSelected.Id.ToString();

                            if (ContentList.Controls.ContainsKey(key))
                            {
                                ContentList.Controls.RemoveByKey(key);
                            }

                            ItemSelected.DeviceType = Common.Enum.ElementType.Carousel;
                            DeletedElement?.Invoke(ItemSelected, true);

                            ItemSelected = null;
                            CardSelected = null;
                            Control.SelectedItem = null;
                            dropdownItems1.DataSourceItems(DataSource, true);
                        }

                        ShowMessageNoItems();
                    }
                }
            }
        }

        private void FindButton_Click(object sender, EventArgs e)
        {
            var filter = FilterTextbox.Text.Trim().ToLower();

            if (ContentList.Controls.Count == DataSource.Count && filter == string.Empty)
            {
                return;
            }

            ContentList.Controls.Clear();
            if (DataSourceAllElements == null)
            {
                ShowMessageNoItems();
                return;
            }

            //var filterData = DataSource.Where(p => (p.Label1?.ToLower().Contains(filter) == true) || (p.Label2?.ToLower().Contains(filter) == true));
            var filterData = DataSource.Where(x => x.Label1 != null ? x.Label1.ToLower().Contains(filter) : (true != true && x.Label2 == null) || x.Label2.ToLower().Contains(filter)).ToList();

            RenderItems(filterData.ToList());
        }

        private void ClearTextImage_Click(object sender, EventArgs e)
        {
            FilterTextbox.Text = string.Empty;

            if (DataSourceAllElements == null)
            {
                RenderItems(DataSource);
                return;
            }

            if (ContentList.Controls.Count != DataSource.Count)
            {
                FindButton_Click(sender, e);
            }
        }

        private void FilterTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (FilterTextbox.Text.Trim() != string.Empty)
            {
                clearTextImage.Show();
            }
            else
            {
                clearTextImage.Hide();
            }


            if (e.KeyCode == Keys.Enter)
            {
                FindButton_Click(sender, e);
            }
        }

        private async void editBtn_Click(object sender, EventArgs e)
        {
            if (Control != null)
            {
                if (await Control.EditSelected())
                {
                    ContentList.Hide();
                    ScrollBar.Hide();
                    Control.Show();
                    //EnabledButtonOk.Enabled = true;
                    ShowFilterControls(false);
                    SetGoBackAction();
                    panelPage.Visible = false;
                    NewOrGrill = true;
                    LocationContentBody(true);
                    dropdownItems1.Visible = false;
                    ResizaPreajuste();
                    ButtonNew.Visible = false;
                    editBtn.Visible = false;
                    ButtonAcept.Visible = false;

                }
            }
        }

        private void labelEndPage_Click(object sender, EventArgs e)
        {
            this.Page = Convert.ToInt32(labelTotal.Text);
            this.TextNumberPage.Text = labelTotal.Text;

            labelEndPage.Visible = false;
            labelNextPage.Visible = false;
            labelbackPage.Visible = true;
            labelStartPage.Visible = true;
        }

        private void labelNextPage_Click(object sender, EventArgs e)
        {
            this.Page++;
            TextNumberPage.Text = this.Page.ToString();
            visibilityPages();
        }

        private void TextNumberPage_TextChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(TextNumberPage.Text) || Convert.ToInt32(labelTotal.Text) == 0)
            {
                return;
            }

            if (Convert.ToInt32(TextNumberPage.Text) < 1)
            {
                this.Page = 1;
                TextNumberPage.Text = "1";
            }
            else if (this.Page > Convert.ToInt32(labelTotal.Text))
            {
                this.Page = Convert.ToInt32(labelTotal.Text);
                TextNumberPage.Text = labelTotal.Text;
            }

            this.Page = Convert.ToInt32(TextNumberPage.Text);
            FilterDataSource();
            visibilityPages();

        }

        private void labelbackPage_Click(object sender, EventArgs e)
        {

            this.Page--;
            this.TextNumberPage.Text = this.Page.ToString();
            visibilityPages();
        }

        private void labelStartPage_Click(object sender, EventArgs e)
        {
            this.Page = 1;
            this.TextNumberPage.Text = this.Page.ToString();

            labelEndPage.Visible = true;
            labelNextPage.Visible = true;
            labelbackPage.Visible = false;
            labelStartPage.Visible = false;
        }

        private void FilterDataSource()
        {
            DataSource = DataSourceAllElements.Skip(this.Take * (Convert.ToInt32(TextNumberPage.Text) - 1))
                              .Take(this.Take).ToList();

            if (DataSource.Count == 0)
            {
                ShowMessageNoItems();
            }
            else
            {
                while (ContentList.Controls.Count > 0)
                {
                    ((GenericFormItemCard)ContentList.Controls[0]).Dispose();
                }

                ContentList.Controls.Clear();
                RenderItems(DataSource);
            }
        }

        private void visibilityPages()
        {
            if (this.Page < Convert.ToInt32(labelTotal.Text))
            {
                labelEndPage.Visible = true;
                labelNextPage.Visible = true;
            }
            else
            {
                labelEndPage.Visible = false;
                labelNextPage.Visible = false;
            }

            if (this.Page > 1)
            {
                labelbackPage.Visible = true;
                labelStartPage.Visible = true;
            }
            else
            {
                labelbackPage.Visible = false;
                labelStartPage.Visible = false;
            }
        }

        private void TextNumberPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }

        public new void Resize()
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                int sidebarCheckListControlWidth = 0;
                #region Estilos

                System.Threading.Tasks.Task.Run(() =>
                {
                    foreach (Control ctl in ContentList.Controls)
                    {
                        if (ctl is GenericFormItemCard)
                        {

                            (ctl as GenericFormItemCard).ResizeControls();
                        }
                    }
                });

                if (main.Width > 1400 && main.Width < 2000)
                {
                    //lblTitle.Font = FontHelper.Get(FontSizes.Medium_3, FontName.ROBOTO_REGULAR);
                    lblTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Bold, GraphicsUnit.Pixel);
                    IndexEntity.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    IndexEntity.IconPadding = 10;
                    //GroupName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                    ButtonNew.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Regular, GraphicsUnit.Pixel);
                    editBtn.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonOK.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAcept.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonCancel.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Regular, GraphicsUnit.Pixel);
                    sidebarCheckListControl.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonOK.IdleBorderRadius = 30;
                    ButtonOK.OnIdleState.BorderRadius = 30;
                    ButtonCancel.IdleBorderRadius = 30;
                    ButtonCancel.OnIdleState.BorderRadius = 30;
                    ButtonNew.IdleBorderRadius = 36;
                    ButtonNew.OnIdleState.BorderRadius = 36;
                    ButtonAcept.IdleBorderRadius = 30;
                    ButtonAcept.OnIdleState.BorderRadius = 30;
                    editBtn.IdleBorderRadius = 36;
                    editBtn.OnIdleState.BorderRadius = 36;

                    //187
                    sidebarCheckListControlWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0976M), 2));
                }
                else if (main.Width >= 1366 && main.Width < 1400)
                {
                    lblTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    IndexEntity.Font = FontHelper.GetRobotoRegular(FontSizes.Small_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    IndexEntity.IconPadding = 5;
                    ButtonNew.Font = FontHelper.GetRobotoRegular(FontSizes.Small_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    editBtn.Font = FontHelper.GetRobotoRegular(FontSizes.Small_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonOK.Font = FontHelper.GetRobotoRegular(FontSizes.Small_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonCancel.Font = FontHelper.GetRobotoRegular(FontSizes.Small_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAcept.Font = FontHelper.GetRobotoRegular(FontSizes.Small_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    FilterTextbox.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    ScrollBar.Font = FontHelper.GetRobotoRegular(FontSizes.Small_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    sidebarCheckListControl.Font = FontHelper.GetRobotoRegular(FontSizes.Small_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonOK.IdleBorderRadius = 20;
                    ButtonOK.OnIdleState.BorderRadius = 20;
                    ButtonCancel.IdleBorderRadius = 20;
                    ButtonCancel.OnIdleState.BorderRadius = 20;
                    ButtonNew.IdleBorderRadius = 26;
                    ButtonNew.OnIdleState.BorderRadius = 26;
                    ButtonAcept.IdleBorderRadius = 20;
                    ButtonAcept.OnIdleState.BorderRadius = 20;
                    editBtn.IdleBorderRadius = 26;
                    editBtn.OnIdleState.BorderRadius = 26;
                    FilterTextbox.BorderRadius = 18;

                    //145
                    sidebarCheckListControlWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0755M), 2));
                }
                else if (main.Width == 1024 && main.Height == 768)
                {
                    lblTitle.Font = FontHelper.Get(FontSizes.Small_6, FontName.ROBOTO_REGULAR);

                    IndexEntity.Font = FontHelper.GetRobotoRegular(FontSizes.Small_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    IndexEntity.IconPadding = 2;

                    ButtonNew.Font = FontHelper.GetRobotoRegular(FontSizes.Small_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonNew.IdleBorderRadius = 20;
                    ButtonNew.OnIdleState.BorderRadius = 20;

                    editBtn.Font = FontHelper.GetRobotoRegular(FontSizes.Small_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    editBtn.IdleBorderRadius = 20;
                    editBtn.OnIdleState.BorderRadius = 20;

                    ButtonAcept.Font = FontHelper.GetRobotoRegular(FontSizes.Small_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAcept.IdleBorderRadius = 20;
                    ButtonAcept.OnIdleState.BorderRadius = 20;

                    ButtonOK.Font = FontHelper.GetRobotoRegular(FontSizes.Small_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonOK.IdleBorderRadius = 20;
                    ButtonOK.OnIdleState.BorderRadius = 20;

                    ButtonCancel.Font = FontHelper.GetRobotoRegular(FontSizes.Small_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonCancel.IdleBorderRadius = 20;
                    ButtonCancel.OnIdleState.BorderRadius = 20;

                    sidebarCheckListControl.Font = FontHelper.GetRobotoRegular(FontSizes.Small_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    sidebarCheckListControlWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0755M), 2));
                }
                else if (main.Width < 1366)
                {
                    lblTitle.Font = FontHelper.Get(FontSizes.Small_6, FontName.ROBOTO_REGULAR);
                    //GroupName.Font = FontHelper.Get(FontSizes.Small_1, FontName.ROBOTO_REGULAR);

                    //145
                    sidebarCheckListControlWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0755M), 2));

                }
                else if (main.Width >= 2000 && main.Width < 2560)
                {
                    lblTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Large_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    IndexEntity.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    IndexEntity.IconPadding = 10;
                    ButtonNew.Font = FontHelper.GetRobotoRegular(FontSizes.Small_9, FontStyle.Regular, GraphicsUnit.Pixel);
                    editBtn.Font = FontHelper.GetRobotoRegular(FontSizes.Small_9, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonOK.Font = FontHelper.GetRobotoRegular(FontSizes.Small_9, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAcept.Font = FontHelper.GetRobotoRegular(FontSizes.Small_9, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonCancel.Font = FontHelper.GetRobotoRegular(FontSizes.Small_9, FontStyle.Regular, GraphicsUnit.Pixel);
                    sidebarCheckListControl.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonOK.IdleBorderRadius = 30;
                    ButtonOK.OnIdleState.BorderRadius = 30;
                    ButtonCancel.IdleBorderRadius = 30;
                    ButtonCancel.OnIdleState.BorderRadius = 30;
                    ButtonNew.IdleBorderRadius = 36;
                    ButtonNew.OnIdleState.BorderRadius = 36;
                    ButtonAcept.IdleBorderRadius = 30;
                    ButtonAcept.OnIdleState.BorderRadius = 30;
                    editBtn.IdleBorderRadius = 36;
                    editBtn.OnIdleState.BorderRadius = 36;
                }
                else if (main.Width >= 2560 && main.Width <= 3440)
                {
                    lblTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Large_2, FontStyle.Bold, GraphicsUnit.Pixel);
                    IndexEntity.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_5, FontStyle.Regular, GraphicsUnit.Pixel);
                    IndexEntity.IconPadding = 10;
                    ButtonNew.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    editBtn.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonOK.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonAcept.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonCancel.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    sidebarCheckListControl.Font = FontHelper.GetRobotoRegular(FontSizes.Small_9, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonOK.IdleBorderRadius = 30;
                    ButtonOK.OnIdleState.BorderRadius = 30;
                    ButtonCancel.IdleBorderRadius = 30;
                    ButtonCancel.OnIdleState.BorderRadius = 30;
                    ButtonNew.IdleBorderRadius = 36;
                    ButtonNew.OnIdleState.BorderRadius = 36;
                    ButtonAcept.IdleBorderRadius = 30;
                    ButtonAcept.OnIdleState.BorderRadius = 30;
                    editBtn.IdleBorderRadius = 36;
                    editBtn.OnIdleState.BorderRadius = 36;
                }

                #endregion

                //30, 30
                var pictureBoxGenericWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0156M), 2));
                var pictureBoxGenericHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0277M), 2));

                //52
                var sidebarControlHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0481M), 2));
                var sidebarCheckListControlHeight = (sidebarCheckListControl.Controls[0].Controls.Count > 0 ? (sidebarCheckListControl.Controls[0].Controls.Count * sidebarControlHeight) : Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1481M), 2)));

                //740, 372
                var spinnerWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3854M), 2));
                var spinnerHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.3444M), 2));

                //2,2
                var spinnerX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0010M), 2));
                var spinnerY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0018M), 2));

                //spinner

                //97,28
                var lblTitleWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0572M), 2));
                var lblTitleHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0259M), 2));

                //215, 32*
                var IconNavPanelWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.112M), 2));
                var IconNavPanelHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

                //915, 41*
                var IconNavPanelX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.766M), 2));
                var IconNavPanelY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.038M), 2));

                //40, 40
                var ButtonNewWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0208M), 2));
                var ButtonNewHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0370M), 2));

                //294, 40 
                var ButtonNewX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1531M), 2));
                var ButtonNewY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0370M), 2));

                //40, 40
                var ButtonGrillaWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0208M), 2));
                var ButtonGrillaHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0370M), 2));

                //248, 40 
                var ButtonGrillaX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1291M), 2));
                var ButtonGrillaY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0370M), 2));

                //340, 80
                var dropdownItemsWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1770M), 2));
                var dropdownItemsHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0740M), 2));

                //6, 92
                var dropdownItemsX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0031M), 2));
                var dropdownItemsY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0851M), 2));

                if (main.Width == 1024 && main.Height == 768)
                {
                    var pictureBoxGenericX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0051M), 2));
                    var pictureBoxGenericY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0400M), 2));
                    pictureBoxGeneric.Location = new Point(pictureBoxGenericX, pictureBoxGenericY);
                    lblTitle.Location = new Point(pictureBoxGenericX + 15, pictureBoxGenericY + 2);

                    //28, 28
                    ButtonNewWidth = 28;
                    ButtonNewHeight = 28;

                    //294, 40 
                    ButtonNewX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1571M), 2));
                    ButtonNewY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0400M), 2));

                    //28, 28
                    ButtonGrillaWidth = 28;
                    ButtonGrillaHeight = 28;

                    //248, 40 
                    ButtonGrillaX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1291M), 2));
                    ButtonGrillaY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0400M), 2));

                    dropdownItemsWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2570M), 2));

                    var sidebarCheckListControlX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0031M), 2)) + dropdownItemsWidth + 5;
                    var sidebarCheckListControlY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0851M), 2));
                    sidebarCheckListControl.Location = new Point(sidebarCheckListControlX, sidebarCheckListControlY);

                }
                else if ((main.Width > 2020 && main.Width < 2100) && (main.Height > 1200 && main.Height < 1300))
                {
                    lblTitleWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0572M), 2)) + 40;
                }

                pictureBoxGeneric.Size = new Size(pictureBoxGenericWidth, pictureBoxGenericHeight);

                sidebarCheckListControl.Size = new Size(sidebarCheckListControlWidth, sidebarCheckListControlHeight);

                spinner.Size = new Size(spinnerWidth, spinnerHeight);
                spinner.Location = new Point(spinnerX, spinnerY);

                lblTitle.Size = new Size(lblTitleWidth, lblTitleHeight);

                IconNavPanel.Size = new Size(IconNavPanelWidth, IconNavPanelHeight);
                IconNavPanel.Location = new Point(IconNavPanelX, IconNavPanelY);

                ButtonNew.Size = new Size(ButtonNewWidth, ButtonNewHeight);
                ButtonNew.Location = new Point(ButtonNewX, ButtonNewY);

                editBtn.Size = new Size(ButtonGrillaWidth, ButtonGrillaHeight);
                editBtn.Location = new Point(ButtonGrillaX, ButtonGrillaY);

                dropdownItems1.Size = new Size(dropdownItemsWidth, dropdownItemsHeight);
                dropdownItems1.Location = new Point(dropdownItemsX, dropdownItemsY);

                //23, 32 26, 36 
                var IndexEntityWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0119M), 2));
                var IndexEntityHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0296M), 2));
                //6, 41
                var IndexEntityX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0031M), 2));
                var IndexEntityY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0379M), 2));
                //2560 x 1600  125 %
                if ((main.Width > 2020 && main.Width < 2100) && (main.Height > 1200 && main.Height < 1300))
                {
                    IndexEntity.Size = new Size(40, IndexEntityHeight);
                    IndexEntity.Location = new Point(3, 34);
                }
                else if (main.Width == 1707 && main.Height == 1067)
                {
                    IndexEntity.Size = new Size(30, IndexEntityHeight);
                    IndexEntity.Location = new Point(3, 34);
                }
                else if (main.Width == 1024 && main.Height == 768)
                {
                    IndexEntity.Size = new Size(18, 18);
                    IndexEntity.Location = new Point(-2, 34);
                }
                else
                {
                    IndexEntity.Size = new Size(IndexEntityWidth, IndexEntityHeight);
                    IndexEntity.Location = new Point(IndexEntityX, IndexEntityY);
                }

                //184, 37*
                var papeleraButtonWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.096M), 2));
                var papeleraButtonHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.034M), 2));
                papeleraButton.Size = new Size(papeleraButtonWidth, papeleraButtonHeight);

                var helpButtonWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.096M), 2));
                var helpButtonHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.034M), 2));
                helpButton.Size = new Size(helpButtonWidth, helpButtonHeight);

                sidebarCheckListControl.SidebarCheckListControlResize();

                if (!NewOrGrill)
                {
                    //ContentBody 750,423
                    var ContentBodyWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3906M), 2));
                    var ContentBodyHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.3925M), 2));

                    if (main.Width == 1024 && main.Height == 768)
                    {
                        ContentBodyWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.4906M), 2));
                        ContentBodyHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.4925M), 2));
                    }
                    ContentBody.Size = new Size(ContentBodyWidth, ContentBodyHeight);

                    //3, 107
                    var ContentBodyX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0015M), 2));
                    var ContentBodyY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0990M), 2));
                    ContentBody.Location = new Point(ContentBodyX, ContentBodyY);
                    //ContentList 716, 372
                    var ContentListWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3729M), 2));
                    var ContentListHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.3925M), 2));
                    ContentList.Size = new Size(ContentListWidth, ContentListHeight);
                }
                else
                {
                    LocationContentBody(true);
                }

                //3, 3
                var ContentListX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0015M), 2));
                var ContentListY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0027M), 2));
                ContentList.Location = new Point(ContentListX, ContentListY);

                //ScrollBar 13, 468*
                var ScrollBarWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0068M), 2));
                var ScrollBarHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.4334M), 2));
                ScrollBar.Size = new Size(ScrollBarWidth, ScrollBarHeight);
                //726, 4
                var ScrollBarX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.375M), 2));
                var ScrollBarY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0038M), 2));
                ScrollBar.Location = new Point(ScrollBarX, ScrollBarY);
                //ScrollBar.BringToFront();

                //ButtonOK 100, 37
                var ButtonOKWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0520M), 2));
                var ButtonOKHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.034M), 2));
                ButtonOK.Size = new Size(ButtonOKWidth, ButtonOKHeight);
                ButtonAcept.Size = new Size(ButtonOKWidth, ButtonOKHeight);
                //230, 212
                var ButtonOKX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1197M), 2));
                var ButtonOKY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1962M), 2));
                ButtonOK.Location = new Point(ButtonOKX, ButtonOKY);
                ButtonOK.BringToFront();
                ButtonAcept.Location = new Point(ButtonOKX, ButtonOKY);
                ButtonAcept.BringToFront();

                //ButtonCancel 100, 37
                var ButtonCancelWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0520M), 2));
                var ButtonCancelHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.034M), 2));
                ButtonCancel.Size = new Size(ButtonCancelWidth, ButtonCancelHeight);

                //123, 212
                var ButtonCancelX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.01138M), 2));
                var ButtonCancelY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1962M), 2));
                ButtonCancel.Location = new Point(ButtonCancelX, ButtonCancelY);
                ButtonCancel.BringToFront();

                //panelPage 200, 52*
                var panelPageWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1042M), 2));
                var panelPageHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.048M), 2));
                panelPage.Size = new Size(panelPageWidth, panelPageHeight);
                //270, 489
                var panelPageX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1406M), 2));
                var panelPageY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.4527M), 2));
                panelPage.Location = new Point(panelPageX, panelPageY);

                ResizaPreajuste();
                dropdownItems1.resize();
                sidebarCheckListControl.Hide();
            }
        }

        private void LocationContentBody(bool blocation)
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                if (blocation)
                {

                    //ContentBody 750, 502
                    var ContentBodyWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3906M), 2));
                    var ContentBodyHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.4648M), 2));

                    //3, 45
                    var ContentBodyX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0015M), 2));
                    var ContentBodyY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.02314M), 2));

                    if (main.Width == 1024 && main.Height == 768)
                    {
                        ContentBodyWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.4906M), 2));
                        ContentBodyHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.5648M), 2));
                    }
                    ContentBody.Size = new Size(ContentBodyWidth, ContentBodyHeight);
                    ContentBody.Location = new Point(ContentBodyX, ContentBodyY);

                    //ContentList 744, 502
                    var ContentListWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3875M), 2));
                    var ContentListHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.4648M), 2));
                    ContentList.Size = new Size(ContentListWidth, ContentListHeight);

                    this.IndexEntity.Visible = true;
                    clearTextImage.Visible = false;
                    //ButtonNew.Visible = false;
                }
                else
                {

                    //ContentBody 750, 372
                    var ContentBodyWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3906M), 2));
                    var ContentBodyHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.3925M), 2));

                    //3, 107
                    var ContentBodyX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0015M), 2));
                    var ContentBodyY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0990M), 2));

                    if (main.Width == 1024 && main.Height == 768)
                    {
                        ContentBodyWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.4906M), 2));
                        ContentBodyHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.5648M), 2));
                    }
                    ContentBody.Size = new Size(ContentBodyWidth, ContentBodyHeight);
                    ContentBody.Location = new Point(ContentBodyX, ContentBodyY);

                    //ContentList 744, 372
                    var ContentListWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3875M), 2));
                    var ContentListHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.3925M), 2));
                    ContentList.Size = new Size(ContentListWidth, ContentListHeight);

                    this.ContentBody.Visible = false;
                    this.IndexEntity.Visible = false;
                    clearTextImage.Visible = true;
                    //ButtonNew.Visible = true;
                }
            }
        }

        public void ResizaPreajuste()
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                if (ButtonOK.Visible && (Configuration?.ObjectBarSelected == LiveBarButtom.preset))
                {
                    //ContentList 750,150 
                    var ContentBodyWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3906M), 2));
                    var ContentBodyHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1388M), 2));
                    
                    ////752, 250
                    var GenericWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3916M), 2));
                    var GenericHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2314M), 2));

                    var fromWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3880M), 2));
                    var fromHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2314M), 2));

                    //593, 180
                    var ButtonOKX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3088M), 2));
                    var ButtonOKY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1666M), 2));

                    //466, 180
                    var ButtonCancelX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2427M), 2));
                    var ButtonCancelY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1666M), 2));

                    if (main.Width == 1024 && main.Height == 768)
                    {
                        ContentBodyWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.4906M), 2));
                        ContentBodyHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2388M), 2));
                        
                        GenericWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.4916M), 2));
                        GenericHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.3314M), 2));

                        if (Configuration?.NameEntity == "Preajustes")
                        {
                            ContentBodyWidth = 391;
                            ContentBodyHeight = 179;

                            fromWidth = 397;
                            fromHeight = 183;

                            ButtonOKY = 128;
                            ButtonCancelY = 128;

                            GenericWidth = 391;
                            GenericHeight = 179;
                        }
                    }

                    ContentBody.Size = new Size(ContentBodyWidth, ContentBodyHeight);
                    ContentList.Size = new Size(ContentBodyWidth, ContentBodyHeight);

                    this.Size = new Size(GenericWidth, GenericHeight);
                    ButtonOK.Location = new Point(ButtonOKX, ButtonOKY);
                    ButtonCancel.Location = new Point(ButtonCancelX, ButtonCancelY);
                    //ResizesPreset?.Invoke(true);

                    this.Parent.Size = new Size(fromWidth, fromHeight);
                    //this.Parent.Refresh();
                }
                else if ((ButtonNew.Visible && !dropdownItems1.Visible) || ButtonOK.Visible) //&& (Configuration?.NameEntity == "Guard" || Configuration?.NameEntity == "Carruseles" || Configuration?.NameEntity == "Grupos")
                {
                    //ContentList 752, 604
                    var ContentBodyWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3916M), 2));
                    var ContentBodyHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.5592M), 2));
                    
                    var fromWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3880M), 2));
                    var fromHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.5601M), 2));

                    //593, 535
                    var ButtonOKX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3088M), 2));
                    var ButtonOKY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.4953M), 2));

                    //466, 535
                    var ButtonCancelX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.2427M), 2));
                    var ButtonCancelY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.4953M), 2));

                    if (main.Width == 1024 && main.Height == 768)
                    {
                        ContentBodyWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3916M), 2)) - 10;
                        ContentBodyHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.5592M), 2)) - 5;

                        fromWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.3880M), 2));
                        fromHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.5601M), 2));

                        if (Configuration?.NameEntity == "Preajustes")
                        {
                            ContentBodyWidth = 391;
                            ContentBodyHeight = 179;

                            fromWidth = 397;
                            fromHeight = 183;

                            ButtonOKY = 128;
                            ButtonCancelY = 128;
                        }
                    }

                    ButtonOK.Location = new Point(ButtonOKX, ButtonOKY);
                    ButtonCancel.Location = new Point(ButtonCancelX, ButtonCancelY);

                    this.Size = new Size(ContentBodyWidth, ContentBodyHeight);
                    if (this.Parent != null)
                        this.Parent.Size = new Size(fromWidth, fromHeight);

                    ContentBody.Visible = true;
                    ButtonNew.Visible = false;
                }
                else
                {
                    //ContentList 404,280
                    var ContentBodyWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1875M), 2));
                    var ContentBodyHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2592M), 2));
                    
                    var fromWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1875M), 2));
                    var fromHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2592M), 2));

                    //230, 212
                    var ButtonOKX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1197M), 2));
                    var ButtonOKY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1962M), 2));

                    //123, 212
                    var ButtonCancelX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0640M), 2));
                    var ButtonCancelY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1962M), 2));

                    if (main.Width == 1024 && main.Height == 768)
                    {
                        // 294, 367
                        ContentBodyWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1975M), 2));
                        ContentBodyHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2496M), 2));

                        fromWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1998M), 2));
                        fromHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2541M), 2));

                        //225, 150
                        ButtonOKX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1367M), 2));
                        ButtonOKY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1962M), 2));

                        //167, 150
                        ButtonCancelX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0690M), 2));
                        ButtonCancelY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1962M), 2));
                    }

                    this.Size = new Size(ContentBodyWidth, ContentBodyHeight);
                    this.Parent.Size = new Size(fromWidth, fromHeight);

                    ButtonAcept.Location = new Point(ButtonOKX, ButtonOKY);
                    ButtonCancel.Location = new Point(ButtonCancelX, ButtonCancelY);

                    if (dropdownItems1.Visible)
                    {
                        ButtonNew.Visible = true;
                    }
                    //this.Parent.Refresh();
                    //ResizesPreset?.Invoke(false);
                    _resizeLoad = true;
                }
            }
        }


        private void ButtonAcept_Click(object sender, EventArgs e)
        {
            // Cuando el módulo es "Grupos", y el elemento de la grilla es seleccionado...
            if ((dropdownItems1.optionCount > 0 && !dropdownItems1.txtBusquedaVisible)
               || (Configuration?.ObjectBarSelected == LiveBarButtom.guard && dropdownItems1.comboBoxCount > 0 && !dropdownItems1.txtBusquedaVisible))
            {
                var item = DataSource.SingleOrDefault(s => s.Id == int.Parse(dropdownItems1.SelectValue));
                OnSelectOption(item);
                OnDoublreClickCard(ItemSelected, CardSelected);
            }
            else
            {
                MessageBox.Show(Resources.SelectElement, Resources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);
                RenderItems(DataSource);
            }
        }
    }
}
