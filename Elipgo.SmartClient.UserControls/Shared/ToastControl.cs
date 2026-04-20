using Elipgo.SmartClient.Common.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Shared
{
    public partial class ToastControl : Form
    {
        private System.Windows.Forms.Timer _timer;

        public ToastControl(string message)
        {
            InitializeComponent();

            Label lbl = new Label()
            {
                Text = message,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lbl);

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 8000;
            _timer.Tick += (s, e) => { this.Close(); };
            this.Shown += ToastControl_Shown;
        }

        private void ToastControl_Shown(object sender, EventArgs e)
        {
            _timer.Start();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // 1. Obtenemos el área de trabajo (excluyendo la barra de tareas)
            var workingArea = Screen.PrimaryScreen.WorkingArea;

            // 2. Calculamos X: (Ancho de pantalla / 2) - (Ancho del formulario / 2)
            int x = workingArea.Left + (workingArea.Width / 2) - (this.Width / 2);

            // 3. Calculamos Y: El borde superior del área de trabajo + un pequeño margen (ej. 10px)
            int y = workingArea.Top + 10;

            this.Location = new Point(x, y);
        }

        private void ButtonClose_Click(object sender, System.EventArgs e)
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
            this.Close();
        }
    }
}
