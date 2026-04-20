using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services.Services.Interface;
using Splat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    //public delegate void AlarmDiagnosticElementEventHandler(object sender, CardDto item);
    //public delegate void AlarmCancelElementEventHandler(object sender, CardDto item);
    public delegate void AlarmGeoLocationElementEventHandler(object sender, CardDto item);
    public delegate void AlarmShowGroupLiveElementEventHandler(object sender, CardDto item);

    public partial class ItemAlarmHeader : UserControl
    {
        public event AlarmDiagnosticElementEventHandler AlarmDiagnostic;
        public event AlarmGeoLocationElementEventHandler AlarmGeoLocation;
        public event AlarmCancelElementEventHandler AlarmCancelar;
        public event AlarmShowGroupLiveElementEventHandler AlarmShowGroupLive;

        private CardDto Item { set; get; }
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        public Bunifu.Framework.UI.BunifuCustomLabel Message { get => this.lblMessage; set => lblMessage = value; }
        private bool _shouldDrawBorder;

        public ItemAlarmHeader(CardDto item)
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
            string panicButtonSubtype = string.Empty;


            this.lblFechaHora.Text = item.Time.ToString();
            this.lblDeviceName.Text = item.DeviceName;
            this.bttpItemAlarm.SetToolTip(this.lblDeviceName, this.lblDeviceName.Text);

            if ("LPR".Equals(item.Type) && item.Message != null)
            {
                if (!String.IsNullOrEmpty(item.Message))
                {
                    string[] tempMessage = item.Message.Split(',');
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
            //this.lblModelo.Text = ((string.IsNullOrEmpty(item.DeviceName) ? Resources.CameraNoName.Trim() + " (" + item.IdAlarm + ")" : item.DeviceName.Trim())) + panicButtonSubtype;
            var btnDiagnosticar = ButtonsContextBar.DiagnoseAlarm.GetAttribute<PermissionAlarm>();
            var btnDescartar = ButtonsContextBar.DiscardAlarm.GetAttribute<PermissionAlarm>();
            if (btnDiagnosticar != null && appAuthorization.Exist(btnDiagnosticar.PermissionKey) == true)
            {
                this.lblViewAlarm.Visible = true;
                this.lblViewAlarm.Text = ci.Name.Contains("es") ? ButtonsContextBar.DiagnoseAlarm.GetDescription() : ButtonsContextBar.DiagnoseAlarm.GetAttribute<DescriptionEN>().Descripcion;
            }
            if (btnDescartar != null && appAuthorization.Exist(btnDiagnosticar.PermissionKey) == true)
            {
                this.lblDiscard.Visible = true;
                this.lblDiscard.Click += btnDescartar_Click;
                this.lblViewAlarm.Click += BtnDiagnosticar_Click;
                this.lblDiscard.Text = ci.Name.Contains("es") ? ButtonsContextBar.DiscardAlarm.GetDescription() : ButtonsContextBar.DiscardAlarm.GetAttribute<DescriptionEN>().Descripcion;
                this.lblViewAlarm.Text = Resources.ShowAlarm;
            }
            this.picAlarmGeolocation.Click += PicAlarmGeolocation_Click;
            this.imgCamara.Paint += ImgCamara_Paint;
            this.imgCamara.DoubleClick += ImgCamara_DoubleClick;
        }

        private void ImgCamara_DoubleClick(object sender, EventArgs e)
        {
            if (Item.ObjectGroupId > 0)
            {
                AlarmShowGroupLive?.Invoke(sender, this.Item);
            }
        }

        private void ImgCamara_Paint(object sender, PaintEventArgs e)
        {
            if (_shouldDrawBorder && Item.ObjectGroupId > 0)
            {
                ControlPaint.DrawBorder(e.Graphics, imgCamara.ClientRectangle, ColorTranslator.FromHtml(VariableResources.COLOR_CONTAINER_SELECTED), ButtonBorderStyle.Solid);
            }
        }

        private void setSyleComponent()
        {
            this.lblAcion.Font = FontHelper.Get(FontSizes.Medium_3, FontName.ROBOTO_MEDIUM);
            this.lblMessage.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_MEDIUM);
            this.lblDeviceName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_MEDIUM, FontStyle.Bold);
            this.lblFechaHora.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
            this.lblFechaHora.ForeColor = ColorTranslator.FromHtml("#D3D3D3");

        }

        public void SetImage(string image)
        {
            this.imgCamara.SizeMode = PictureBoxSizeMode.Zoom;
            this.imgCamara.Image = ImageHelper.Base64ToImage(image);
        }

        public void DrawAlarmAssignedObjectGroup(List<int> allGroupsId)
        {
            if (Item.ObjectGroupId > 0 && allGroupsId.Contains(Item.ObjectGroupId))
            {
                imgCamara.Cursor = Cursors.Hand;
                _shouldDrawBorder = true;
                imgCamara.Invalidate();
            }
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

        private void PicAlarmGeolocation_Click(object sender, EventArgs e)
        {
            this.Item.Snapshot = ImageHelper.ImageToBase64(this.imgCamara.Image);
            AlarmGeoLocation?.Invoke(sender, this.Item);
        }

        public void CustomDispose()
        {
            this.lblDiscard.Click -= btnDescartar_Click;
            this.lblViewAlarm.Click -= BtnDiagnosticar_Click;
            this.Dispose();
        }

        /// <summary>
        /// Establece el tooltip del mensaje usando el BunifuToolTip existente.
        /// </summary>
        public void SetMessageTooltip(string text)
        {
            this.bttpItemAlarm.SetToolTip(this.lblMessage, text);
        }
    }
}
