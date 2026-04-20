using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services.Services.Interface;
using Splat;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    public delegate void AlarmDiagnosticElementEventHandler(object sender, CardDto item);
    public delegate void AlarmCancelElementEventHandler(object sender, CardDto item);
    public delegate void AlarmRetrieveElementEventHandler(object sender, CardDto item);
    public delegate void AlarmShowDetailElementEventHandler(object sender, CardDto item);

    public partial class ItemCard : UserControl
    {
        public event AlarmDiagnosticElementEventHandler AlarmDiagnostic;
        public event AlarmCancelElementEventHandler AlarmCancelar;
        public event AlarmRetrieveElementEventHandler AlarmRetrieve;
        public event AlarmShowDetailElementEventHandler AlarmShowDetail;
        private CardDto Item { set; get; }
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();

        public ItemCard(CardDto item)
        {
            InitializeComponent();

            this.Item = item;
            string message = null;

            this.setSyleComponent();
            CultureInfo ci = CultureInfo.InstalledUICulture;
            if (Enum.TryParse<AlarmType>(item.Type, out var enumValue))
            {
                this.lblAcion.Text = EnumExtension.GetTranslation(enumValue);
            }
            this.lblPais.Text = item.Site;
            /* Anterior... */
            /* this.lblModelo.Text = item.IdAlarm + " - " + item.DeviceName.Trim(); */
            string panicButtonSubtype = string.Empty;


            this.lblFechaHora.Text = item.Time.ToString();
            if ("LPR".Equals(item.Type))
            {
                if (!String.IsNullOrEmpty(item.Message))
                {
                    string[] tempMessage = String.IsNullOrEmpty(item.Message) ? null : item.Message.Split(',');
                    if (tempMessage != null)
                    {
                        string[] tempPlate = tempMessage.Length > 0 ? tempMessage[0].Split(':') : new string[] { item.Message };
                        string[] tempList = tempMessage.Length > 1 ? tempMessage[1].Split(':') : null;
                        if (tempList != null)
                        {
                            if (" ".Equals(tempList[0]))
                            {
                                message = tempPlate[0] + " : " + tempPlate[1];
                                this.lblMessage.Text = string.IsNullOrEmpty(item.Message) ? (string.IsNullOrEmpty(item.Personalized_Message) ? "" : item.Personalized_Message) : message;
                            }
                            else
                            {
                                string list = tempList[1].Replace('[', ' ');
                                list = list.Replace(']', ' ');
                                list = list.Replace('.', ' ');
                                message = tempPlate[0] + " : " + tempPlate[1] + " - " + tempList[0] + " : " + list;
                                this.lblMessage.Text = string.IsNullOrEmpty(item.Message) ? (string.IsNullOrEmpty(item.Personalized_Message) ? "" : item.Personalized_Message) : message;
                            }
                        }
                        else
                        {
                            this.lblMessage.Text = string.IsNullOrEmpty(item.Message) ? (string.IsNullOrEmpty(item.Personalized_Message) ? "" : item.Personalized_Message) : item.Message;
                        }
                    }

                    if (tempMessage == null && !string.IsNullOrEmpty(item.Personalized_Message))
                    {
                        tempMessage = item.Personalized_Message.Split('|');
                        var plateNumberObj = Array.FindAll(tempMessage, s => s.Contains("Plate Number"));
                        plateNumberObj = plateNumberObj.Length > 0 ? plateNumberObj[0].Split(':') : null;
                        var plateNumberStr = String.Concat(ci.Name.Contains("es") ? LprTags.PlateNumber.GetDescription() : LprTags.PlateNumber.GetAttribute<DescriptionEN>().Descripcion, ": ", plateNumberObj.Length > 1 ? plateNumberObj[1] : ci.Name.Contains("es") ? LprTags.NoPlate.GetDescription() : LprTags.NoPlate.GetAttribute<DescriptionEN>().Descripcion);

                        this.lblMessage.Text = plateNumberStr;
                    }
                }
                else
                {
                    this.lblMessage.Text = String.Empty;
                }
            }
            else
            {
                this.lblMessage.Text = string.IsNullOrEmpty(item.Message) ? (string.IsNullOrEmpty(item.Personalized_Message) ? "" : item.Personalized_Message) : item.Message;
            }
            if (item.Type == AlarmType.PANIC_BUTTON.ToString())
            {
                switch (item.SubType)
                {
                    case "F":
                        panicButtonSubtype = " " + PanicButtonType.F.GetDescription();
                        break;
                    case "V":
                        panicButtonSubtype = " " + PanicButtonType.V.GetDescription();
                        break;
                }
                if (!string.IsNullOrEmpty(item.Cad_Invoice))
                    this.lblMessage.Text += (!string.IsNullOrEmpty(this.lblMessage.Text) ? " Folio " : "Folio ") + item.Cad_Invoice;
            }
            this.lblModelo.Text = ((string.IsNullOrEmpty(item.DeviceName) ? Common.Properties.Resources.CameraNoName.Trim() + " (" + item.IdAlarm + ")" : item.DeviceName.Trim())) + panicButtonSubtype;
            var btnDiagnosticar = ButtonsContextBar.DiagnoseAlarm.GetAttribute<PermissionAlarm>();
            var btnDescartar = ButtonsContextBar.DiscardAlarm.GetAttribute<PermissionAlarm>();
            if (btnDiagnosticar != null && appAuthorization.Exist(btnDiagnosticar.PermissionKey) == true)
            {
                this.btnDiagnosticar.Visible = true;
                this.btnDiagnosticar.Text = ci.Name.Contains("es") ? ButtonsContextBar.DiagnoseAlarm.GetDescription() : ButtonsContextBar.DiagnoseAlarm.GetAttribute<DescriptionEN>().Descripcion;
            }
            if (btnDescartar != null && appAuthorization.Exist(btnDiagnosticar.PermissionKey) == true)
            {
                this.btnDescartar.Visible = true;
                this.btnDescartar.Click += btnDescartar_Click;
                this.btnDescartar.Text = ci.Name.Contains("es") ? ButtonsContextBar.DiscardAlarm.GetDescription() : ButtonsContextBar.DiscardAlarm.GetAttribute<DescriptionEN>().Descripcion;
            }
            this.btnDescartar.TabStop = false;
            ValidateHitoryButtons();
        }

        private void setSyleComponent()
        {
            this.IconElement.Image = FileResources.icon_alarm_card;
            this.lblAcion.Font = FontHelper.Get(FontSizes.Medium_3, FontName.ROBOTO_MEDIUM);
            this.lblPais.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_LIGHT);
            this.lblPais.ForeColor = ColorTranslator.FromHtml("#D3D3D3");

            this.lblModelo.Font = FontHelper.Get(FontSizes.Medium_3, FontName.ROBOTO_MEDIUM);
            this.lblMessage.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_MEDIUM);
            this.lblFechaHora.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
            this.lblFechaHora.ForeColor = ColorTranslator.FromHtml("#D3D3D3");

            this.btnDescartar.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_MEDIUM);
            this.btnDiagnosticar.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_MEDIUM);
        }

        public void SetImage(string image)
        {
            this.imgCamara.SizeMode = PictureBoxSizeMode.Zoom;
            this.imgCamara.Image = ImageHelper.Base64ToImage(image);
        }

        private void BtnDiagnosticar_Click(object sender, EventArgs e)
        {
            this.Item.Snapshot = ImageHelper.ImageToBase64(this.imgCamara.Image);
            AlarmDiagnostic?.Invoke(sender, this.Item);
        }

        private void btnDescartar_Click(object sender, EventArgs e)
        {
            AlarmCancelar?.Invoke(sender, this.Item);
        }

        private void ValidateHitoryButtons()
        {
            if (Item.AlarmLevel == (int)AlarmLevels.CriticalChecked || Item.AlarmLevel == (int)AlarmLevels.CriticalDiscard)
            {

                this.btnDescartar.Visible = false;
                this.btnDescartar.Enabled = false;
                this.btnDescartar.SendToBack();

                this.btnDiagnosticar.Visible = false;
                this.btnDiagnosticar.Enabled = false;
                this.btnDiagnosticar.SendToBack();

                if (Item.AlarmLevel == (int)AlarmLevels.CriticalDiscard)
                {
                    this.btnRetrieve.Visible = true;
                    this.btnRetrieve.Enabled = true;
                    this.btnRetrieve.BringToFront();
                    this.btnRetrieve.Click += BtnRetrieve_Click;
                }
                else if (Item.AlarmLevel == (int)AlarmLevels.CriticalChecked)
                {
                    this.btnDetail.Visible = true;
                    this.btnDetail.Enabled = true;
                    this.btnDetail.BringToFront();
                    this.btnDetail.Click += BtnDetail_Click; ;
                }
            }
        }

        private void BtnDetail_Click(object sender, EventArgs e)
        {
            AlarmShowDetail?.Invoke(sender, this.Item);
        }

        private void BtnRetrieve_Click(object sender, EventArgs e)
        {
            AlarmRetrieve?.Invoke(sender, this.Item);
        }
    }
}
