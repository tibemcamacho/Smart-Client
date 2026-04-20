using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services;
using Elipgo.SmartClient.Services.Services.Interface;
using Elipgo.SmartClient.ViewModels;
using Splat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Vault
{
    public delegate void OnInstantPlayerOpenHandler(CameraDTO camera, List<RecorderDTO> recorder, DateTime StartTime, DateTime EndTime);
    public delegate void OnInstantPlayerOpenMP4Handler(string pathFileMP4, CameraDTO camera, DateTime StartTime, DateTime EndTime);
    public partial class VaultDialogControl : UserControl
    {
        public event OnInstantPlayerOpenHandler OnInstantPlayerOpen;
        public event OnInstantPlayerOpenMP4Handler OnInstantPlayerOpenMP4;
        public VaultViewModel ViewModel;
        private bool editMode;
        private List<DataGridThreeValuesBookmarkGroup> listValues = new List<DataGridThreeValuesBookmarkGroup>();
        private readonly IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        private VaultItemCardDTO itemSelected;
        public VaultItemCardDTO ItemSelected
        {
            get => itemSelected;
            set
            {
                itemSelected = value;
                _textBoxName.Text = _labelNameData.Text = itemSelected.Name;
                _labelFromData.Text = itemSelected.BookmarkGroup.Elements == null ? "" : Convert.ToDateTime(itemSelected.BookmarkGroup.Elements[0].TimeStart).ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                _labelToData.Text = itemSelected.BookmarkGroup.Elements == null ? "" : Convert.ToDateTime(itemSelected.BookmarkGroup.Elements[0].TimeEnd).ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                List<BookmarkGroupElementDTO> bookMarkGroupElements = itemSelected.BookmarkGroup.Elements == null ? new List<BookmarkGroupElementDTO>() : itemSelected.BookmarkGroup.Elements.Select(x => x).ToList();
                InitializerAsync(bookMarkGroupElements);
                if (editMode == false)
                {
                    _bunifuImageButtonFolder.Visible = true;
                }
            }
        }

        private async void InitializerAsync(List<BookmarkGroupElementDTO> bookMarkGroupElements)
        {
            foreach (BookmarkGroupElementDTO element in bookMarkGroupElements)
            {
                ViewModel._dvfid = element.DeviceFeatureId;
                foreach (var site in (await ViewModel.GetCatalogAsync()).Sites)
                {
                    var camera = site.Cameras.Where(x => x.ObjectId == element.DeviceFeatureId).FirstOrDefault();
                    if (camera != null)
                    {
                        var recorder = camera.Recorders.Where(x => x.Id == element.NvrId).FirstOrDefault();
                        listValues.Add(new DataGridThreeValuesBookmarkGroup()
                        {
                            Site = site.Name,
                            DeviceName = camera.Name,
                            RecorderName = recorder == null ? camera.DeviceName : recorder.Name,
                            DeviceFeatureId = camera.ObjectId,
                            RecorderId = recorder?.Id,
                            TimeStart = element.TimeStart,
                            TimeEnd = element.TimeEnd
                        });
                        break;
                    }
                }

            }
            _dataGridView.DataSource = (from it in listValues
                                        select new
                                        {
                                            it.Site,
                                            it.DeviceName,
                                            it.RecorderName,
                                            it.DeviceFeatureId,
                                            it.RecorderId
                                        }).ToList();
        }
        public bool EditMode
        {
            get => editMode;
            set
            {
                editMode = value;
                if (editMode)
                {
                    _textBoxName.ReadOnly = false;
                    _buttonCancel.Visible = true;
                    _buttonCancel.Text = Resources.ButtonCancel;
                    _buttonOK.Visible = true;
                    _buttonOK.Text = Resources.ButtonOK;
                    _labelNameData.Visible = false;
                    _textBoxName.Visible = true;
                }
                else
                {
                    _textBoxName.ReadOnly = true;
                    _buttonOK.Visible = true;
                    _buttonOK.Text = Resources.ButtonOK;
                    _buttonCancel.Visible = false;
                    _labelNameData.Visible = true;
                    _textBoxName.Visible = true;
                }
            }
        }
        public VaultDialogControl()
        {
            InitializeComponent();

            _textBoxName.TextChanged += (s, e) =>
            {
                if (_errorProvider.ContainerControl != null)
                {
                    _errorProvider.SetError(_textBoxName, string.Empty);
                }
            };

            _dataGridView.AutoGenerateColumns = false;
            _dataGridView.RowHeadersVisible = false;
            _dataGridView.RowTemplate.Height = 28;
            _dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold);
            _labelTitleForm.Text = Resources.NewBookmark;
            List<BookmarkGroupElementDTO> dataView = new List<BookmarkGroupElementDTO>();
            _dataGridView.DataSource = dataView;
            _dataGridColumnSite.HeaderText = Resources.SiteName;
            _dataGridColumnDevice.HeaderText = Resources.DeviceName;
            _dataGridColumnRecorder.HeaderText = Resources.RecorderName;

            if (!appAuthorization.Exist("auth.app.apps.playback.showPlayBack"))
            {
                _dataGridView.Columns[3].Visible = false;
            }

            _dataGridView.CellFormatting += DataGridView_CellFormatting;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.ParentForm.DialogResult = DialogResult.Cancel;
            this.ParentForm.Close();
        }

        private void TextBookmarkName_KeyPress(object sender, KeyPressEventArgs e)
        {
            string invalid = new string(Path.GetInvalidFileNameChars());

            if (!(invalid.IndexOf(e.KeyChar.ToString()) > -1) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == (char)Keys.Space))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private async void ButtonOK_Click(object sender, EventArgs e)
        {
            if (editMode)
            {
                if (!ValidateName())
                    return;

                itemSelected.BookmarkGroup.Description = itemSelected.BookmarkGroup.FileName = _textBoxName.Text;
                await Vmon5Service.UpdateBookmarkGroup(itemSelected.BookmarkGroup, this.ViewModel.MainView.UserToken);
                this.ParentForm.DialogResult = DialogResult.OK;
            }
            else
            {
                this.ParentForm.DialogResult = DialogResult.Cancel;
            }
            this.ParentForm.Close();
        }

        private void bunifuImageButtonFolder_Click(object sender, EventArgs e)
        {
            string DefaultFolderExportPath = Common.Properties.Settings.Default["DefaultLocation"].ToString() + "\\Export";
            string directory = Path.Combine(DefaultFolderExportPath, itemSelected.BookmarkGroup.FileName);
            if (Directory.Exists(directory))
            {
                Process.Start(directory);
            }
            else
            {
                Process.Start(DefaultFolderExportPath);
            }

        }


        private void DataGridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1 && e.ColumnIndex == 3)
                {
                    _dataGridView.Cursor = Cursors.Hand;
                }
                else
                {
                    _dataGridView.Cursor = Cursors.Default;
                }
            }
            catch { }
        }

        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex == 3)
            {
                _dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = Resources.Play;
            }
        }

        private async void DataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex == 3)
            {
                DataGridThreeValuesBookmarkGroup data = listValues.ToArray()[e.RowIndex];

                if (data != null)
                {
                    string DefaultFolderExportPath = Common.Properties.Settings.Default["DefaultLocation"].ToString() + "\\Export";
                    string cameraName = data.DeviceName.Replace(" ", "").Replace("/", "-");
                    string dateStart = data.TimeStart.Split('T')[0];
                    string timeStart = data.TimeStart.Split('T')[1].Replace(":", "-");
                    string bookmarkName = $"{cameraName}_{dateStart}_H{timeStart}.mp4";
                    string filePath = Path.Combine(DefaultFolderExportPath, itemSelected.BookmarkGroup.FileName, bookmarkName);

                    CameraDTO camera = await this.ViewModel.GetCamera(data.DeviceFeatureId);
                    var recorder = camera.Recorders.Where(r => r.Id == data.RecorderId).FirstOrDefault();
                    if (recorder != null)
                    {
                        camera.RecorderId = recorder.Id;
                    }

                    if (File.Exists(filePath))
                    {
                        OnInstantPlayerOpenMP4?.Invoke(filePath, camera, Convert.ToDateTime(data.TimeStart), Convert.ToDateTime(data.TimeEnd));
                    }
                    else
                    {
                        OnInstantPlayerOpen?.Invoke(camera, camera.Recorders, Convert.ToDateTime(data.TimeStart), Convert.ToDateTime(data.TimeEnd));
                    }
                }
            }
        }

        private bool ValidateName()
        {
            if (string.IsNullOrWhiteSpace(_textBoxName.Text))
            {
                _errorProvider.SetError(_textBoxName, "El nombre es obligatorio");
                return false;
            }

            _errorProvider.SetError(_textBoxName, string.Empty);
            return true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var form = this.FindForm();
            if (form != null)
            {
                _errorProvider.ContainerControl = form;

                _textBoxName.TextChanged += (s, ev) =>
                {
                    _errorProvider.SetError(_textBoxName, string.Empty);
                };
            }
        }
    }
}
