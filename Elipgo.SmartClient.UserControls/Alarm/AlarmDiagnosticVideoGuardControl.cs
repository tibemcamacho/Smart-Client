using Bunifu.Framework.UI;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Drivers;
using Elipgo.SmartClient.UserControls.ElementContainer;
using Elipgo.SmartClient.ViewModels;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    public partial class AlarmDiagnosticVideoGuardControl : AlarmDiagnosticBase
    {
        private string nameTab = string.Empty;
        public AlarmDiagnosticVideoGuardControl(AlarmViewModel viewModel, CardDto card) : base(viewModel, card)
        {
            InitializeComponent();
            this.nameTab = viewModel.MainView.ApplicationTitle;
            //CreatePanel()
        }

        protected override async void AddElementToPanel(Control control)
        {
            // Esperar a que _alarmDTO esté inicializado desde la clase base
            await AlarmDTOInitializationTask;

            if (_alarmDTO == null)
            {
                Common.Logger.Log("AlarmDiagnosticVideoGuardControl.AddElementToPanel: _alarmDTO es null", Common.LogPriority.Warning);
                return;
            }

            // Buscamos la Camra de Ayuda y lo dejamos en una Task
            // para levantar el resultado luego, y doblar un poco
            // el tiempo
            //var rspVideoHelp = Task.Run(() =>
            //{
            //    //var alarmHelp = this._viewModel.GetAlarm(_alarmDTO.HelperId);
            //    var dtoHelp = _viewModel.GetCamera(_alarmDTO.HelperId);

            //    return dtoHelp;
            //});

            // Creamos un panel para contener el maps de Chromium
            Panel panelChromium = new Panel() { Size = new Size(375, 512) };
            panelChromium.Controls.Add(control);

            // Creamos un panel para contener la llamada en vivo
            Panel panelVideoCall = new Panel() { Size = new Size(375, 302), Top = panelChromium.Height };
            var controlVideo = await GetContentVideoCall();
            panelVideoCall.Controls.Add(controlVideo);

            // Creamos un button para el Mic
            BunifuImageButton btnMic = new BunifuImageButton()
            {
                BackColor = Color.Red,
                Location = new Point(150, 830),
                Size = new System.Drawing.Size(30, 30),
                Image = FileResources.icon_micr_on,
                Cursor = Cursors.Hand,
                Name = "MicBonifu",
                Visible = true,
            };

            // Esperamos que termine de traer los datos del dto 
            //Task.WhenAll(rspVideoHelp);

            // Creamos el Panel para el Video vivo de la camara secundaria 
            Panel panelVideoHelp = new Panel() { Size = new Size(1013, 725), Left = panelChromium.Width };
            var dtoHelp = await _viewModel.GetCamera(_alarmDTO.HelperId);
            var controlVideoHelp = GetContentVideoHelp(dtoHelp);
            controlVideoHelp.Visible = false;
            panelVideoHelp.Controls.Add(controlVideoHelp);

            // Agregamos ambos en un solo elemento para pasarlo al contenedor general
            Panel panelGeneral = new Panel() { Size = new Size(1388, 731) };
            panelGeneral.Controls.Add(panelChromium);
            panelGeneral.Controls.Add(panelVideoCall);
            panelGeneral.Controls.Add(panelVideoHelp);

            base.AddElementToPanel(panelGeneral);

            this.Controls.Add(btnMic);
            btnMic.BringToFront();
            controlVideoHelp.Visible = true;
        }

        private async Task<Control> GetContentVideoCall()
        {
            // Buscamos los datos de la Alarma
            var dto = await _viewModel.GetCamera(_alarmDTO.ObjectId);

            // Creamos el Control
            var control = new ElementCameraControl(null, dto, Profile.MainStream, false, nameTab);
            control.Name = "videoVivo";

            // Seteamos los stylos
            //control.Size = new Size(1013, 731);
            control.Dock = DockStyle.Fill;

            control.OnVideo += Control_OnVideo;

            // Retornamos el control con el Video en vivo de la Camara
            return control;
        }

        private Control GetContentVideoHelp(CameraDTO dto)
        {
            // Creamos el Control
            var control = new ElementCameraControl(null, dto, Profile.MainStream, false, nameTab);
            control.Name = "videoAyuda";

            // Seteamos los stylos
            //control.Size = new Size(1013, 731);
            control.Dock = DockStyle.Fill;

            //control.OnVideo += Control_OnVideo;

            // Retornamos el control con el Video en vivo de la Camara
            return control;
        }

        private void Control_OnVideo(bool video, object parent)
        {
            if (video)
            {
                var controlMic = this.Controls.Find("MicBonifu", true)?.FirstOrDefault() as Bunifu.Framework.UI.BunifuImageButton;

                var controlVideo = this.Controls.Find("videoVivo", true)?.FirstOrDefault() as ElementCameraControl;

                var statusAudio = controlVideo.Controls.OfType<IDriverLive>().First().ListenStatus;

                if (!statusAudio)
                {
                    controlVideo.Controls.OfType<IDriverLive>().First().ToggleListen(statusAudio);
                }

                controlMic.Click += Control_Click;

                controlMic.Image = controlVideo.Controls.OfType<IDriverLive>().First().TalkStatus ? FileResources.icon_micr_on : FileResources.icon_micr_off;
            }
        }

        private void Control_Click(object sender, EventArgs e)
        {
            var control = this.Controls.Find("videoVivo", true)?.FirstOrDefault() as ElementCameraControl;

            var controlMic = this.Controls.Find("MicBonifu", true)?.FirstOrDefault() as Bunifu.Framework.UI.BunifuImageButton;

            var status = control.Controls.OfType<IDriverLive>().First().ToggleTalk();

            controlMic.Image = control.Controls.OfType<IDriverLive>().First().TalkStatus ? FileResources.icon_micr_on : FileResources.icon_micr_off;
        }

        public override void ButtonCancel_Click(object sender, EventArgs e)
        {
            var control = this.Controls.Find("videoVivo", true)?.FirstOrDefault() as ElementCameraControl;

            control.Dispose();

            base.ButtonCancel_Click(sender, e);
        }

        public override void ButtonOK_Click(object sender, EventArgs e)
        {
            var control = this.Controls.Find("videoVivo", true)?.FirstOrDefault() as ElementCameraControl;

            control.Dispose();

            base.ButtonOK_Click(sender, e);
        }

        private void AlarmDiagnosticVideoGuardControl_Load(object sender, EventArgs e)
        {
            this.LoadStep();
        }
    }
}
