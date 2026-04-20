using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.UserControls.Shared;
using Elipgo.SmartClient.UserControls.Sidebar;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.UserProfile
{
    public partial class ItemsProfile : PopedCotainer
    {
        public event ObjectSelectEventHandler ItemSelectedClicked;
        public event ObjectSelectEventHandler SwichChecked;

        private readonly Configuration _config;

        public ItemsProfile()
        {
            InitializeComponent();
            _config = SmartClientEnvironmentUtils.GetConfiguration();
            this.dbFlowLayoutPanel1.Size = new Size(240, 52);
            this.dbFlowLayoutPanel1.Location = new Point(0, 0);
        }

        public void LoadSource(List<CheckElementDTO> listElement)
        {
            // Asegurar que toda la creación y manipulación de controles se ejecute en el hilo de UI
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(() => LoadSource(listElement)));
                return;
            }

            // Disponer y limpiar controles anteriores de forma segura
            foreach (var it in this.dbFlowLayoutPanel1.Controls.OfType<ItemControl>().ToList())
            {
                it.ItemSelectedClicked -= Element_ItemSelectedClicked;
                it.Dispose();
            }
            this.dbFlowLayoutPanel1.Controls.Clear();

            this.Size = new Size(240, listElement.Any() ? 0 : 52);

            listElement.Where(x => x.Visible == true).ToList().ForEach(item =>
            {
                var element = new ItemControl();
                element.Name = "chk" + item.Key;
                element.Size = new Size(240, 36);
                element.Visible = item.Visible;
                element.Margin = new Padding(0);
                element.Label = item.Name;
                if (item.Key == "DeviceStatus" || item.Key == "BitRate")
                {
                    var verifyStatus = bool.TryParse(WorkFolderUtils.GetUserSettings((item.Key == "DeviceStatus" ? "VerifyStatus" : "BitRate"), true), out bool preResult) ? preResult : false;
                    element.SwichCheckedEvent(verifyStatus);
                    element.VisibleSwichEvent(true);
                    element.IconSizeEvent(40, 20);
                }
                else if (item.Key == "levelAccess")
                {
                    element.IconSizeEvent(120, 20);
                    element.Size = new Size(240, 56);
                    var labelControl = element.Controls.OfType<Label>().FirstOrDefault();
                    if (labelControl != null)
                    {
                        labelControl.Location = new Point(130, 10);
                    }
                }
                else
                {
                    element.VisibleSwichEvent(false);
                }

                element.Icon = item.Icon;
                element.Item = item;

                element.ItemSelectedClicked += Element_ItemSelectedClicked;
                dbFlowLayoutPanel1.Height += element.Height;
                Height += element.Height;
                dbFlowLayoutPanel1.Controls.Add(element);
            });
        }

        private void Element_ItemSelectedClicked(string name, bool state)
        {
            ItemSelectedClicked?.Invoke(name, state);
        }

        public void CustomDispose()
        {
            // Garantizar ejecutar en el hilo de UI de este control contenedor
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { CustomDispose(); });
                return;
            }

            // Disponer hijos con seguridad de hilo; si algún hijo fue creado en otro hilo, dispóngalo en su propio hilo
            foreach (var ctrl in dbFlowLayoutPanel1.Controls.OfType<ItemControl>().ToList())
            {
                try
                {
                    ctrl.ItemSelectedClicked -= Element_ItemSelectedClicked;

                    if (ctrl.InvokeRequired)
                    {
                        // Disponer en el hilo creador del control hijo para evitar cross-thread
                        ctrl.Invoke((MethodInvoker)(() => ctrl.Dispose()));
                    }
                    else
                    {
                        Height -= ctrl.Height;
                        dbFlowLayoutPanel1.Height -= ctrl.Height;
                        ctrl.Dispose();
                    }
                }
                catch { /* Ignorar disposición redundante u objetos ya eliminados */ }
            }

            dbFlowLayoutPanel1.Controls.Clear();
        }
    }
}
