using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.UserControls.Shared;
using Elipgo.SmartClient.UserControls.Sidebar;
using Elipgo.SmartClient.UserControls.UserProfile;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.PlaybackControls
{
    public partial class ZoomTimeLinePlaybackControl : UserControl
    {
        private PoperContainer _poper;
        private ItemsZoom _content;
        public event ObjectSelectEventHandler ItemSelectedClicked;
        private bool _isVault = false;

        private string _intervalX1;
        private string _intervalX1_2;
        private string _intervalX1_3;
        private string _intervalX1_6;
        public ZoomTimeLinePlaybackControl()
        {
            InitializeComponent();
            _content = new ItemsZoom();
            _poper = new PoperContainer(_content);
            _content.ItemSelectedClicked += Content_ItemSelectedClicked;
            LoadZoomOptions();

        }
        private void LoadZoomOptions()
        {
            var list = _isVault ? GetVaultZoomOptions() : GetNormalZoomOptions();
            if (_isVault)
            {
                SelectButton.Text = $"x1       -    ( {_intervalX1} )";
            }
            else
            {
                SelectButton.Text = Resources.TextZoomNormal;
            }
            _content.LoadSource(list);
        }

        private List<CheckElementDTO> GetNormalZoomOptions()
        {
            return new List<CheckElementDTO>()
        {
            new CheckElementDTO
            {
                Key = Constants.ZOOM_TIMELINE_NORMAL,
                Name = Resources.TextZoomNormal,
                Icon = FileResources.icon_check,
                Visible = true
            },
            new CheckElementDTO
            {
                Key = Constants.ZOOM_TIMELINE_15,
                Name = Resources.TextZoom15Min,
                Icon = null,
                Visible = true
            },
            new CheckElementDTO
            {
                Key = Constants.ZOOM_TIMELINE_10,
                Name = Resources.TextZoom10Min,
                Icon = null,
                Visible = true
            },
            new CheckElementDTO
            {
                Key = Constants.ZOOM_TIMELINE_5,
                Name = Resources.TextZoom5Min,
                Icon = null,
                Visible = true
            }
        };
        }

        private List<CheckElementDTO> GetVaultZoomOptions()
        {
            return new List<CheckElementDTO>()
        {
            new CheckElementDTO
            {
                Key = "x1",
                Name = $"x1       -    ( {_intervalX1} )",
                Icon = null,
                Visible = true
            },
            new CheckElementDTO
            {
                Key = "x1/2",
                Name = $"x1/2    -   ( {_intervalX1_2} )",
                Icon = null,
                Visible = true
            },
            new CheckElementDTO
            {
                Key = "x1/3",
                Name = $"x1/3    -   ( {_intervalX1_3} )",
                Icon = null,
                Visible = true
            },
            new CheckElementDTO
            {
                Key = "x1/6",
                Name = $"x1/6    -   ( {_intervalX1_6} )",
                Icon = null,
                Visible = true
            }
        };
        }


        public void UpdateZoomOptionsBasedOnDuration(bool isVault, string intervalX1, string intervalX1_2, string interval1_3, string interval1_6)
        {
            SelectButton.Text = isVault ? $"x1      -    ( {intervalX1} )" : Resources.TextZoomNormal;

            _intervalX1 = intervalX1;
            _intervalX1_2 = intervalX1_2;
            _intervalX1_3 = interval1_3;
            _intervalX1_6 = interval1_6;

            _isVault = isVault;
            LoadZoomOptions();
        }

        private void CustomDispose()
        {
            _poper.Dispose();
            _content.Dispose();
        }

        private void Content_ItemSelectedClicked(string name, bool state)
        {
            ItemSelected(name, state);
            ItemSelectedClicked?.Invoke(name, state);
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            this._poper.Show(this);
        }

        private void ItemSelected(string key, bool state)
        {
            for (int i = 0; i < _content.Controls[0].Controls.Count; i++)
            {
                if (((ItemControl)_content.Controls[0].Controls[i]).Item.Key == key)
                {
                    ((ItemControl)_content.Controls[0].Controls[i]).Icon = FileResources.icon_check;
                    SelectButton.Text = ((ItemControl)_content.Controls[0].Controls[i]).Item.Name;
                }
                else
                {
                    ((ItemControl)_content.Controls[0].Controls[i]).Icon = null;
                } ((ItemControl)_content.Controls[0].Controls[i]).Refresh();
            }

            ItemSelectedClicked?.Invoke(key, state);
        }

        public void SelecteItem(string key)
        {
            ItemSelected(key, true);
        }

        public void EnabledTimelineScale(bool enabled)
        {
            SelectButton.Enabled = enabled;
        }

        public void VisibleTimelineScale(bool visible)
        {
            SelectButton.Visible = visible;
        }
    }
}
