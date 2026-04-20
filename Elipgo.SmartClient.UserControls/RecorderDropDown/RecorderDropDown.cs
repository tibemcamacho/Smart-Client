using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Services;
using Elipgo.SmartClient.UserControls.Shared;
using Elipgo.SmartClient.ViewModels;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.RecorderDropDown
{
    public partial class RecorderDropDown : UserControl
    {
        public event EventHandler<RecorderDTOSmall> SelectedValueChanged;

        public MainViewModel mainView = Locator.Current.GetService<IScreen>() as MainViewModel;

        private readonly PoperContainer _poper;
        private readonly DropDownContainer _content;
        private readonly List<CameraStatesDTO> _recorderStates = new List<CameraStatesDTO>();
        private RecorderDTOSmall _currentRecorder;
        private readonly bool _verifyStatus;

        public RecorderDropDown(int cameraId, List<RecorderDTOSmall> recorders, RecorderDTOSmall currentRecorder, bool verifyStatus = false)
        {
            InitializeComponent();

            _content = new DropDownContainer();
            _poper = new PoperContainer(_content);
            _verifyStatus = verifyStatus;
            _currentRecorder = currentRecorder;

            SetOptions(cameraId, recorders);

            if (mainView.Signal != null)
            {
                mainView.Signal.CameraStateEventAction += CameraStateEvent;
            }
        }

        private void SetOptions(int cameraId, List<RecorderDTOSmall> recorders)
        {
            foreach (var recorder in recorders)
            {
                var option = new DropDownOption
                {
                    Name = Guid.NewGuid().ToString(),
                    Key = cameraId + "-" + recorder.Id + "-" + recorder.Driver,
                    Dock = DockStyle.Bottom,
                    Item = recorder
                };
                option.ShowImage(_verifyStatus);
                option.SetText(recorder.Name);
                option.ButtonOptionClicked += Option_ButtonOptionClicked;
                _content.Controls.Add(option);
                _content.Height += option.Height;

                if (_verifyStatus)
                {
                    _recorderStates.Add(new CameraStatesDTO()
                    {
                        Apps = "Playback",
                        Id = cameraId,
                        RecorderDriver = recorder.RecorderType == RecorderType.EDGE ? null : recorder.Driver,
                        RecorderId = recorder.RecorderType == RecorderType.EDGE ? 0 : recorder.Id,
                        RecorderType = recorder.RecorderType.ToString()
                    });
                }
            }


            var value = recorders.Find(x => x.Id == _currentRecorder.Id && x.Driver == _currentRecorder.Driver && x.RecorderType == _currentRecorder.RecorderType);
            if (value != null)
            {
                this.ButtonOptionSelected.Text = string.IsNullOrEmpty(_currentRecorder.Name) ? value.Name : _currentRecorder.Name;
            }
        }

        private void Option_ButtonOptionClicked(object sender, OptionObjectDTO option)
        {
            var recorder = (RecorderDTOSmall)option.Item;
            if (recorder.Id != _currentRecorder.Id && recorder.RecorderType != _currentRecorder.RecorderType)
            {
                this.ButtonOptionSelected.Text = recorder.Name;
                SelectedValueChanged?.Invoke(sender, recorder);
            }
            _poper.Hide();
        }

        private void ButtonOptionSelected_Click(object sender, EventArgs e)
        {
            _poper.Show(this);
            if (_verifyStatus)
            {
                Task.Run(() => Vmon5Service.postCameraStatusRecordTest(_recorderStates, mainView.UserToken));
            }
        }

        private void CameraStateEvent(dynamic data)
        {
            if (data != null)
            {
                CameraStateDTO cameraState = Newtonsoft.Json.JsonConvert.DeserializeObject<CameraStateDTO>(data.ToString());

                var key = cameraState.Id + "-" + cameraState.RecorderId + "-" + cameraState.RecorderDriver;
                foreach (var it in _content.Controls)
                {
                    if (it is DropDownOption)
                    {
                        if ((it as DropDownOption).Key == key)
                        {
                            (it as DropDownOption).SetImage(cameraState.State, true);
                        }
                    }
                }
            }
        }

    }
}
