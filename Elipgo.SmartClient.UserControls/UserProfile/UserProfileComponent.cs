using Bunifu.ToggleSwitch;
using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services.Services.Interface;
using Elipgo.SmartClient.UserControls.Shared;
using Elipgo.SmartClient.UserControls.Sidebar;
using Splat;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.UserProfile
{
    public partial class UserProfileComponent : UserControl
    {
        public event ObjectSelectEventHandler ItemSelectedClicked;

        private PoperContainer _poper;
        private ItemsProfile _content;
        private IAppAuthorization _appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        private readonly string RELEASE_VERSION;
        private int _accessLevel;

        public void SetProfileImage(string user, System.Drawing.Image image)
        {
            if (image != null)
            {
                this._bunifuImageButtonProfile.Dock = DockStyle.Fill;
                this._bunifuImageButtonProfile.Image = null;
                this._bunifuImageButtonProfile.BackgroundImage = image;
                this._bunifuImageButtonProfile.BackgroundImageLayout = ImageLayout.Zoom;
                this._bunifuImageButtonProfile.SizeMode = PictureBoxSizeMode.Zoom;
            }

            this._bunToolTipCurrentUser.SetToolTip(this._bunifuImageButtonProfile, user);
        }

        public void ReleaseImage()
        {
            this._bunifuImageButtonProfile.BackgroundImage = null;
        }

        public UserProfileComponent(bool showJoystick)
        {
            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();
            InitializeComponent();

            RELEASE_VERSION = config.AppSettings.Settings["ReleaseVersion"].Value;

            this.Resize += UserProfileComponent_Resize;
        }

        private int GetUserAccessLevel(string userToken)
        {
            try
            {
                var parts = userToken.Split('.');
                if (parts.Length != 3)
                    return 5;

                string payload = parts[1];
                payload = payload.PadRight((payload.Length + 3) / 4 * 4, '=');

                string decodedPayload = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(payload.Replace('-', '+').Replace('_', '/')));
                int startIndex = decodedPayload.IndexOf("\"accessLevel\":\"") + 15;
                int endIndex = decodedPayload.IndexOf("\"", startIndex);

                return int.TryParse(decodedPayload.Substring(startIndex, endIndex - startIndex), out int accessLevel) ? accessLevel : 5;
            }
            catch
            {
                return 5;
            }
        }

        public void LoadListItemUser(bool showJoystick, string token = null)
        {
            if (token == null)
            {
                return;
            }

            _accessLevel = GetUserAccessLevel(token);

            _content = new ItemsProfile();
            _poper = new PoperContainer(_content);
            _content.ItemSelectedClicked += Content_ItemSelectedClicked;

            var list = new System.Collections.Generic.List<CheckElementDTO>() {
                new CheckElementDTO
                {
                    Key = "ChangePassword",
                    Name = Resources.ChangePassword,
                    Icon = FileResources.icon_lock,
                    Visible =  _appAuthorization != null && _appAuthorization.Exist(ButtonsContextBar.ChangePassword.GetAttribute<Permission>().PermissionKey)
                },
                new CheckElementDTO
                {
                     Key = "MyDownloads",
                     Name = Resources.MyDownloads,
                     Icon = FileResources.icon_view_list,
                     Visible = true
                }
            };

            if (showJoystick)
            {
                list.Add(new CheckElementDTO
                {
                    Key = "JoystickSetting",
                    Name = Resources.JoystickSetting,
                    Icon = FileResources.icon_tool_setting,
                    Visible = true
                });
            }

            list.Add(new CheckElementDTO
            {
                Key = "ResourceMonitor",
                Name = Resources.ResourceMonitor,
                Icon = FileResources.icon_system_monitor,
                Visible = true
            });

            list.AddRange(new System.Collections.Generic.List<CheckElementDTO>()
            {
                new CheckElementDTO
                {
                    Key="DownloadLogs",
                    Name=Resources.DownloadLogs,
                    Icon = FileResources.icon_outline_file_download,
                    Visible = true
                },
                new CheckElementDTO
                {
                     Key = "FolderBrowser",
                     Name = Resources.FolderBrowser,
                     Icon = FileResources.icon_folder,
                     Visible = true
                },
                new CheckElementDTO
                {
                     Key = "DeviceStatus",
                     Name = Resources.DeviceStatus,
                     Control = new Bunifu.ToggleSwitch.BunifuToggleSwitch(),
                     Icon = FileResources.icon_videocam,
                     Visible = true
                },
                new CheckElementDTO
                {
                     Key = "CustomSettings",
                     Name = Resources.CustomSettings,
                     Icon = FileResources.icon_tool_setting,
                     Visible = true
                },
                new CheckElementDTO
                {
                     Key = "BitRate",
                     Name = Resources.BitRate,
                     Control = new Bunifu.ToggleSwitch.BunifuToggleSwitch(),
                     Icon = FileResources.icon_videocam,
                     Visible = true
                },new CheckElementDTO
                {
                     Key = "levelAccess",
                     Name = Resources.AccessLevel,
                     Icon = _accessLevel == 1 ? FileResources.icon_accessLevel_1 :
                            _accessLevel == 2 ? FileResources.icon_accessLevel_2 :
                            _accessLevel == 3 ? FileResources.icon_accessLevel_3 :
                            _accessLevel == 4 ? FileResources.icon_accessLevel_4 :
                            FileResources.icon_accessLevel_5,
                     Visible = true
                }, new CheckElementDTO
                {
                     Key = "LogOut",
                     Name = Resources.LogOut,
                     Icon = FileResources.icon_logout,
                     Visible = true
                },new CheckElementDTO
                {
                    Key = "SEPARATOR",
                    Separator = true
                }, new CheckElementDTO
                {
                     Key = "Version",
                     Name = RELEASE_VERSION,
                     Icon = FileResources.icon_help,
                     Visible = true
                }

            });
            _content.LoadSource(list);
        }

        private void UserProfileComponent_Resize(object sender, System.EventArgs e)
        {
        }

        private void CustomDispose()
        {
            if (_poper != null)
            {
                _poper.Dispose();
                _content.Dispose();
            }
        }

        private void Content_ItemSelectedClicked(string name, bool state)
        {
            ItemSelectedClicked?.Invoke(name, state);
            this._poper.Close();
        }

        private void ButtonProfile_Click(object sender, System.EventArgs e)
        {
            try
            {
                this._poper.Show(this);
                UpdateDeviceState();
            }
            catch (Exception ex)
            {
                Logger.Log("ButtonProfile_Click --> "+ ex.StackTrace,LogPriority.Fatal);
            }
        }

        private void UpdateDeviceState()
        {
            foreach (var item in _content.Controls[0].Controls)
            {
                if (((System.Windows.Forms.Control)item).Name == "chkDeviceStatus" || ((System.Windows.Forms.Control)item).Name == "chkBitRate")
                {
                    var verifyStatus = bool.TryParse(WorkFolderUtils.GetUserSettings((((System.Windows.Forms.Control)item).Name == "chkDeviceStatus" ?
                                                                                        "VerifyStatus" : "BitRate"), true), out bool preResult) && preResult;
                    var bunu = ((ItemControl)item);
                    (bunu.Controls[0].Controls[0] as BunifuToggleSwitch).Checked = verifyStatus;
                }

            }
        }

    }
}
