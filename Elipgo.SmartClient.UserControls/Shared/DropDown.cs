using Elipgo.SmartClient.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Shared
{
    public delegate void ShowDDLEnventHandler(bool state);
    public partial class Dropdown : UserControl
    {
        public event ShowDDLEnventHandler ShowDDLBtn;
        public event EventHandler<OptionObjectDTO> OptionSelected;


        private bool expand = false;
        public int SelectedIndex { get; set; }
        public Dropdown()
        {
            InitializeComponent();
            this.ResizeDropdown();
            this.Disposed += Dropdown_Disposed;
        }

        private void Dropdown_Disposed(object sender, EventArgs e)
        {
            this.ddlTimer.Tick -= ddlTimer_Tick;
            this.SelectButton.Click -= SelectButton_Click;
            this.Disposed -= Dropdown_Disposed;

            var options = this.DropdownContainer.Controls;
            foreach (Control item in options)
            {
                item.Dispose();
                this.DropdownContainer.Controls.Remove(item);
            }
        }

        private void ResizeDropdown()
        {
            var sumHeigth = 0;
            foreach (var item in DropdownContainer.Controls)
            {
                var optionBtn = item as Control;
                sumHeigth += optionBtn.Height;

                optionBtn.Width = this.Width;
            }
            this.MaximumSize = new Size(this.Width, sumHeigth);
        }

        public int GetMaxHeigth()
        {
            var sumHeigth = 0;
            foreach (var item in DropdownContainer.Controls)
            {
                var optionBtn = item as Control;
                sumHeigth += optionBtn.Height;

                optionBtn.Width = this.Width;
            }
            return sumHeigth;
        }

        public void SetOptions(List<OptionObjectDTO> options)
        {
            foreach (var option in options)
            {
                var optionBtn = new OptionDropDown();
                optionBtn.Name = option.Key;
                optionBtn.SetOption(option.Name);
                optionBtn.SetIconOption(option.Key);
                optionBtn.OptionButtonClicked += SelectedOptionDDL_Click;
                optionBtn.Dock = DockStyle.Bottom;
                this.DropdownContainer.Controls.Add(optionBtn);
            }
            var firstOption = options.FirstOrDefault();
            if (firstOption != null)
            {
                this.SelectButton.Text = firstOption.Name;
                OptionSelected.Invoke(null, firstOption);
                expand = false;
            }
            this.ResizeDropdown();
        }

        public void SetOptionsWhitImage(List<OptionObjectDTO> options)
        {
            foreach (var option in options)
            {
                var optionBtn = new OptionDropDown();
                optionBtn.Name = option.Key;
                optionBtn.SetOption(option.Name);
                optionBtn.SetIconOption(option.Item as Image);
                optionBtn.OptionButtonClicked += SelectedOptionDDL_Click;
                optionBtn.Dock = DockStyle.Bottom;
                this.DropdownContainer.Controls.Add(optionBtn);
            }
            var firstOption = options.FirstOrDefault();
            if (firstOption != null)
            {
                this.SelectButton.Text = firstOption.Name;
                OptionSelected.Invoke(null, firstOption);
                expand = false;
            }
            this.ResizeDropdown();
        }

        public void RemoveOptions(List<OptionObjectDTO> options)
        {
            foreach (var option in options)
            {
                OptionDropDown optionBtn = this.DropdownContainer.Controls.Find(option.Key, true).FirstOrDefault() as OptionDropDown;
                this.DropdownContainer.Controls.Remove(optionBtn);
                optionBtn.Dispose();
            }
            this.ResizeDropdown();
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            ShowDDLBtn?.Invoke(expand);
            ddlTimer.Start();
        }

        private void ddlTimer_Tick(object sender, EventArgs e)
        {
            if (!expand)
            {
                this.Height += 15;
                if (this.Height >= this.MaximumSize.Height)
                {
                    ddlTimer.Stop();
                    expand = true;
                }
            }
            else
            {
                this.Height -= 15;
                if (this.Height <= this.MinimumSize.Height)
                {
                    ddlTimer.Stop();
                    expand = false;
                }
            }

        }

        private void SelectedOptionDDL_Click(object sender, OptionObjectDTO e)
        {
            this.SelectButton.Text = e.Name;
            OptionSelected.Invoke(sender, e);
            ShowDDLBtn?.Invoke(expand);
            ddlTimer.Start();
        }
    }
}
