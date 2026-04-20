using System.Windows.Forms;

namespace Elipgo.SmartClient.Drivers
{
    /// <summary>
    /// 17/Jun/2021 * vmon-4308 * ddvl  
    /// </summary>
    public class Reconnecting
    {
        public Reconnecting() { }

        public static void DisplayLogo(double pictureWidth, double pictureHeight, ref Panel panel, ref PictureBox panelFondo)
        {
            // factor de proporción obtenido de medidas del contenedor en Grilla 9 / medidas del logo a 200 x 40
            double _width = (pictureWidth / ((521 / 200) + 0.5));
            double _height = (pictureHeight / ((297 / 40) + 0.5));

            // Ajustando el tamaño en base al contenedor y centrando el panelconnection con la imagen png de "Reconectando..."
            panel.Size = new System.Drawing.Size((int)_width, (int)_height);
            panel.Top = (int)(pictureHeight / 2) - (panel.Height / 2);
            panel.Left = (int)(pictureWidth / 2) - (panel.Width / 2);

            /// 01/Jul/2021 * vmon-4362 * ddvl   * Al redimensionar la Grilla el cartel de Reconectando no quedaba centrado
            panelFondo.Visible = false;
            panelFondo.Size = new System.Drawing.Size((int)pictureWidth, (int)pictureHeight);
            panelFondo.Top = 0;
            panelFondo.Left = 0;
            //esta linea se comento por que inpedia recibir eventos al elementcontrol y no se puede remover la camara ni nada favor de no desconetar  
            // panelFondo.BringToFront();
            //panel.BringToFront();
        }

    }
}
