using Bunifu.Framework.UI;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.UserControls.Outputs;
using Elipgo.SmartClient.UserControls.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.MainBar
{
    public class ObjectSelectedArgs : EventArgs
    {
        public LiveBarItemDTO LiveBarItemDTO { get; }

        public ObjectSelectedArgs(LiveBarItemDTO element)
        {
            LiveBarItemDTO = element;
        }
    }

    public abstract class MainBarBaseControl : UserControl
    {
        public EventHandler<ObjectSelectedArgs> ObjectSelected;
        public EventHandler<bool> ObjectSelectedTalk;
        public EventHandler<bool> ObjectSelectedZigitalZoom;
        public List<Bunifu.Framework.UI.BunifuImageButton> Buttons = new List<Bunifu.Framework.UI.BunifuImageButton>();
        private bool _resizeLoad = false;
        public abstract void LoadButtons();
        public abstract void SetImageButtons();
        public abstract void ShowButtons();
        public abstract void SetTooltips();

        public virtual void SetPositionButtons()
        {
            System.Drawing.Point locationReference = new System.Drawing.Point(1154, 24);
            _resizeLoad = true;
            int shiftX = Convert.ToInt32(Elipgo.SmartClient.Common.Properties.VariableResources.shift_location_x);
            int shiftY = Convert.ToInt32(Elipgo.SmartClient.Common.Properties.VariableResources.shift_location_y);

            bool isFirstButton = true;

            foreach (var liveButton in Buttons)
            {
                if (liveButton.Visible)
                {
                    if (isFirstButton)
                    {
                        liveButton.Location = locationReference;
                        isFirstButton = false;
                    }
                    else
                    {
                        locationReference.X += shiftX;
                        locationReference.Y += shiftY;

                        liveButton.Location = locationReference;
                    }
                }
            }
        }

        protected virtual void OnObjectSelectedChanged(object sender, ObjectSelectedArgs args)
        {
            ObjectSelected?.Invoke(sender, args);
        }

        protected virtual void OnObjectSelectedTalkChanged(object sender, bool e)
        {
            ObjectSelectedTalk?.Invoke(sender, e);
        }

        protected virtual void OnObjectSelectedDigitalZoomChanged(object sender, bool e)
        {
            ObjectSelectedZigitalZoom?.Invoke(sender, e);
        }

        public virtual void SetCursorButtons()
        {
            Buttons.ForEach(b => b.Cursor = Cursors.Hand);
        }

        public void BuildBar()
        {
            LoadButtons();
            SetImageButtons();
            SetTooltips();
            SetCursorButtons();
            ShowButtons();
            SetPositionButtons();
        }

        public void ResizeControls()
        {
            if (!Screen.AllScreens.Any(s => s.WorkingArea.Contains(Cursor.Position)))
                return;
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var workingArea = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                Screen primaryScreen = Screen.PrimaryScreen;
                Screen currentScreen = Screen.FromPoint(Cursor.Position);
                int height = 0, top = 0;
                if (primaryScreen != null && primaryScreen.Primary && primaryScreen.DeviceName != Screen.AllScreens[0].DeviceName)
                {
                    if (!currentScreen.DeviceName.Contains("DISPLAY1") && currentScreen == Screen.PrimaryScreen)
                    {
                        height = this.Height - 60;
                        top = 30;

                        var size = new Size(height, height);
                        int separator = 0;

                        foreach (Control c in this.Controls)
                        {
                            if (!c.Visible) continue;

                            switch (c)
                            {
                                case Bunifu.UI.WinForms.BunifuDropdown control:
                                    control.Size = new Size(220, height);
                                    control.Top = top;
                                    control.Left = this.Width - 220 - separator;
                                    separator += height + 220;
                                    break;
                                case BunifuiOSSwitch control:
                                    control.Size = size;
                                    control.Top = top;
                                    control.Left = this.Width - height - separator;
                                    separator += height * 2;
                                    break;
                                case BunifuImageButton control:
                                    control.Size = size;
                                    control.Top = top;
                                    control.Left = this.Width - height - separator;
                                    separator += height * 2;
                                    break;
                                case OutputToggleButton control:
                                    control.Size = size;
                                    control.Top = top;
                                    control.Left = this.Width - height - separator;
                                    separator += height * 2;
                                    break;
                                case ButtonCalendarControl control:
                                    break;
                                case Label control:
                                    break;
                                default:
                                    //c.Size = size;
                                    c.Height = height;
                                    c.Top = top;
                                    c.Left = this.Width - c.Width - separator;
                                    separator += height + c.Width;
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    if (workingArea.Width > 1400 && workingArea.Width < 2000)
                    {
                        height = this.Height - 40;
                        top = 20;
                    }
                    else if (workingArea.Width < 1400)
                    {
                        height = this.Height - 28;
                        top = 14;
                    }
                    else if (workingArea.Width >= 2000 && workingArea.Width < 2560)
                    {
                        height = this.Height - 60;
                        top = 30;
                    }
                    else if (workingArea.Width >= 2560 && workingArea.Width <= 3440)
                    {
                        height = this.Height - 70;
                        top = 40;
                    }

                    var size = new Size(height, height);
                    int separator = 0;

                    foreach (Control c in this.Controls)
                    {
                        if (!c.Visible) continue;

                        switch (c)
                        {
                            case Bunifu.UI.WinForms.BunifuDropdown control:
                                control.Size = new Size(220, height);
                                control.Top = top;
                                control.Left = this.Width - 220 - separator;
                                separator += height + 220;
                                break;
                            case BunifuiOSSwitch control:
                                control.Size = size;
                                control.Top = top;
                                control.Left = this.Width - height - separator;
                                separator += height * 2;
                                break;
                            case BunifuImageButton control:
                                control.Size = size;
                                control.Top = top;
                                control.Left = this.Width - height - separator;
                                separator += height * 2;
                                break;
                            case OutputToggleButton control:
                                control.Size = size;
                                control.Top = top;
                                control.Left = this.Width - height - separator;
                                separator += height * 2;
                                break;
                            case ButtonCalendarControl control:
                                break;
                            case Label control:
                                break;
                            default:
                                //c.Size = size;
                                c.Height = height;
                                c.Top = top;
                                c.Left = this.Width - c.Width - separator;
                                separator += height + c.Width;
                                break;
                        }
                    }
                }


            }
            _resizeLoad = false;
        }
    }
}
