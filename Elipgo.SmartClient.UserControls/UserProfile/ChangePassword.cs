using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services.Services.Interface;
using Newtonsoft.Json;
using Splat;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.UserProfile
{
    public partial class ChangePassword : UserControl
    {
        bool verPassActual;
        bool verPassNueva;
        string user;
        string token;
        private readonly string VMON_URL;
        private readonly ISmartNotification _notification = Locator.Current.GetService<ISmartNotification>();
        private IAuditService _auditService = Locator.Current.GetService<IAuditService>();

        public ChangePassword(string user, string token)
        {
            InitializeComponent();
            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();
            VMON_URL = config.AppSettings.Settings["VMON5_URL"].Value;
            verPassActual = verPassNueva = false;
            lblOldPass.Text = Resources.CurrentPassword;
            lblNewPass.Text = Resources.NewPassword;
            lblReEnterPass.Text = Resources.ConfirmPassword;
            bunifuButtonCancelar.Text = Resources.ButtonCancel;
            bunifuButtonGuardar.Text = Resources.ButtonOK;
            tilteForm.Text = Resources.ChangePassword;
            btnPassVisibleNueva.Image = FileResources.icon_visibility_off;
            btnPassVisibleNueva.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            btnPassVisibleActual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            btnPassVisibleActual.Image = FileResources.icon_visibility_off;
            this.user = user;
            this.token = token;
            txtOldPass.PasswordChar = '*';
            txtNewPass.PasswordChar = '*';
            txtReEnterPass.PasswordChar = '*';
        }

        private async void bunifuButtonGuardar_Click(object sender, EventArgs e)
        {
            if (txtNewPass.Text != txtReEnterPass.Text)
            {
                this.errorProvider1.Clear();
                this.errorProvider1.SetError(btnPassVisibleNueva, Resources.NoMatchPassword);
                this.errorProvider1.SetError(txtReEnterPass, Resources.NoMatchPassword);
                return;
            }
            await SendChangePassword();
        }

        private void bunifuButtonCancelar_Click(object sender, EventArgs e)
        {
            ((Form)this.TopLevelControl).Close();

        }

        private void btnPassVisibleActual_Click(object sender, EventArgs e)
        {
            if (verPassActual)
            {
                verPassActual = false;
                txtOldPass.PasswordChar = '*';
                btnPassVisibleActual.Image = FileResources.icon_visibility_off;
            }
            else
            {
                btnPassVisibleActual.Image = FileResources.icon_visibility_on;
                verPassActual = true;
                txtOldPass.PasswordChar = '\0';
            }
        }

        private void btnPassVisibleNueva_Click(object sender, EventArgs e)
        {
            if (verPassNueva)
            {
                verPassNueva = false;
                txtNewPass.PasswordChar = '*';
                txtReEnterPass.PasswordChar = '*';
                btnPassVisibleNueva.Image = FileResources.icon_visibility_off;
            }
            else
            {
                btnPassVisibleNueva.Image = FileResources.icon_visibility_on;
                verPassNueva = true;
                txtNewPass.PasswordChar = '\0';
                txtReEnterPass.PasswordChar = '\0';
            }
        }

        private async Task<bool> SendChangePassword()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {//Users/Password
                    UserChangePasswordDTO pass = new UserChangePasswordDTO
                    {
                        CurrentPassword = txtOldPass.Text,
                        NewPassword = txtNewPass.Text,
                        Email = this.user
                    };
                    //client.Timeout = TimeSpan.FromMilliseconds(30000);
                    string jsonString = JsonConvert.SerializeObject(pass);
                    var requestUrl = new Uri(VMON_URL + "/v1/User/Password");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    using (HttpContent httpContent = new StringContent(jsonString))
                    {
                        httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        var response = client.PutAsync(requestUrl, httpContent).Result;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            ((Form)this.TopLevelControl).Close();
                            _notification.Show(Resources.PassWordChangedSuccessfully, null);

                            await _auditService.Add(new AuditDTO()
                            {
                                CodeAction = AuditAction.CHANGE_PASSWORD.ToString(),
                                Param01 = this.user
                            }, "Bearer " + token);
                            return true;
                        }
                        else
                        {
                            //((Form)this.TopLevelControl).Close();

                            this.errorProvider1.Clear();
                            var msg = JsonConvert.DeserializeObject<IdentityResultDTO>(response.Headers.GetValues("X-ErrorPassword").ToArray()[0]);

                            if (msg.Errors.ToArray()[0].Code == "PasswordAlreadyUsed")
                            {
                                _notification.Show(Resources.PasswordAlreadyUsed, null);
                            }
                            else
                            {
                                _notification.Show(msg.Errors.ToArray()[0].Description, null);
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void btnPassVisibleActual_MouseLeave(object sender, EventArgs e)
        {
            btnPassVisibleActual.BackgroundImage = FileResources.icon_visibility_off;
            txtOldPass.PasswordChar = '*';
        }

        private void btnPassVisibleNueva_MouseLeave(object sender, EventArgs e)
        {
            btnPassVisibleNueva.BackgroundImage = FileResources.icon_visibility_off;
            txtNewPass.PasswordChar = '*';
            txtReEnterPass.PasswordChar = '*';
        }

        private void btnPassVisibleNueva_MouseEnter(object sender, EventArgs e)
        {
            txtNewPass.PasswordChar = '\0';
            txtReEnterPass.PasswordChar = '\0';
        }

        private void btnPassVisibleActual_MouseEnter(object sender, EventArgs e)
        {
            txtOldPass.PasswordChar = '\0';
        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            ((Form)this.TopLevelControl).Close();
        }
    }
}
