
namespace Elipgo.SmartClient.UserControls.UserProfile
{
    partial class ResourceMonitor
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResourceMonitor));
            this.panel1 = new System.Windows.Forms.Panel();
            this.chrMonitorDisk = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblDiskSc = new System.Windows.Forms.Label();
            this.lblDisk = new System.Windows.Forms.Label();
            this.chrMonitorRAM = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chrMonitorCPU = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblRamSc = new System.Windows.Forms.Label();
            this.lblCpuSc = new System.Windows.Forms.Label();
            this.lblRAM = new System.Windows.Forms.Label();
            this.lblCPU = new System.Windows.Forms.Label();
            this.PanelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.ButtonClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.tmrMonitor = new System.Windows.Forms.Timer(this.components);
            this.pfcCPU = new System.Diagnostics.PerformanceCounter();
            this.pfCPUSC = new System.Diagnostics.PerformanceCounter();
            this.pfcDisk = new System.Diagnostics.PerformanceCounter();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chrMonitorDisk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrMonitorRAM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrMonitorCPU)).BeginInit();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pfcCPU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pfCPUSC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pfcDisk)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(26)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.chrMonitorDisk);
            this.panel1.Controls.Add(this.lblDiskSc);
            this.panel1.Controls.Add(this.lblDisk);
            this.panel1.Controls.Add(this.chrMonitorRAM);
            this.panel1.Controls.Add(this.chrMonitorCPU);
            this.panel1.Controls.Add(this.lblRamSc);
            this.panel1.Controls.Add(this.lblCpuSc);
            this.panel1.Controls.Add(this.lblRAM);
            this.panel1.Controls.Add(this.lblCPU);
            this.panel1.Controls.Add(this.PanelHeader);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(433, 608);
            this.panel1.TabIndex = 1;
            // 
            // chrMonitorDisk
            // 
            this.chrMonitorDisk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(26)))));
            chartArea4.AxisX.Interval = 1D;
            chartArea4.AxisX.LabelStyle.ForeColor = System.Drawing.Color.Silver;
            chartArea4.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea4.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea4.AxisX.Maximum = 10D;
            chartArea4.AxisX.Minimum = 0D;
            chartArea4.AxisY.Interval = 50D;
            chartArea4.AxisY.LabelStyle.ForeColor = System.Drawing.Color.Silver;
            chartArea4.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea4.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea4.AxisY.Maximum = 100D;
            chartArea4.AxisY.Minimum = 0D;
            chartArea4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(26)))));
            chartArea4.BorderColor = System.Drawing.Color.Empty;
            chartArea4.Name = "ChartArea1";
            this.chrMonitorDisk.ChartAreas.Add(chartArea4);
            this.chrMonitorDisk.Location = new System.Drawing.Point(-19, 435);
            this.chrMonitorDisk.Name = "chrMonitorDisk";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series4.IsVisibleInLegend = false;
            series4.Name = "DISK";
            series4.SmartLabelStyle.Enabled = false;
            series4.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            this.chrMonitorDisk.Series.Add(series4);
            this.chrMonitorDisk.Size = new System.Drawing.Size(450, 159);
            this.chrMonitorDisk.TabIndex = 52;
            this.chrMonitorDisk.Text = "chart1";
            // 
            // lblDiskSc
            // 
            this.lblDiskSc.AutoSize = true;
            this.lblDiskSc.ForeColor = System.Drawing.Color.Silver;
            this.lblDiskSc.Location = new System.Drawing.Point(116, 419);
            this.lblDiskSc.Name = "lblDiskSc";
            this.lblDiskSc.Size = new System.Drawing.Size(37, 13);
            this.lblDiskSc.TabIndex = 51;
            this.lblDiskSc.Text = "0 MBs";
            // 
            // lblDisk
            // 
            this.lblDisk.AutoSize = true;
            this.lblDisk.ForeColor = System.Drawing.Color.Silver;
            this.lblDisk.Location = new System.Drawing.Point(36, 419);
            this.lblDisk.Name = "lblDisk";
            this.lblDisk.Size = new System.Drawing.Size(74, 13);
            this.lblDisk.TabIndex = 50;
            this.lblDisk.Text = "Uso de Disco:";
            // 
            // chrMonitorRAM
            // 
            this.chrMonitorRAM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(26)))));
            chartArea5.AxisX.Interval = 1D;
            chartArea5.AxisX.LabelStyle.ForeColor = System.Drawing.Color.Silver;
            chartArea5.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea5.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea5.AxisX.Maximum = 10D;
            chartArea5.AxisX.Minimum = 0D;
            chartArea5.AxisY.LabelStyle.ForeColor = System.Drawing.Color.Silver;
            chartArea5.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea5.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea5.AxisY.Minimum = 0D;
            chartArea5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(26)))));
            chartArea5.BorderColor = System.Drawing.Color.Empty;
            chartArea5.Name = "ChartArea1";
            this.chrMonitorRAM.ChartAreas.Add(chartArea5);
            this.chrMonitorRAM.Location = new System.Drawing.Point(-19, 251);
            this.chrMonitorRAM.Name = "chrMonitorRAM";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series5.IsVisibleInLegend = false;
            series5.Name = "RAM";
            series5.SmartLabelStyle.Enabled = false;
            series5.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            this.chrMonitorRAM.Series.Add(series5);
            this.chrMonitorRAM.Size = new System.Drawing.Size(450, 159);
            this.chrMonitorRAM.TabIndex = 49;
            this.chrMonitorRAM.Text = "chart1";
            // 
            // chrMonitorCPU
            // 
            this.chrMonitorCPU.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(26)))));
            chartArea6.AxisX.Interval = 1D;
            chartArea6.AxisX.LabelStyle.ForeColor = System.Drawing.Color.Silver;
            chartArea6.AxisX.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea6.AxisX.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea6.AxisX.Maximum = 10D;
            chartArea6.AxisX.Minimum = 0D;
            chartArea6.AxisY.Interval = 50D;
            chartArea6.AxisY.LabelStyle.ForeColor = System.Drawing.Color.Silver;
            chartArea6.AxisY.MajorGrid.LineColor = System.Drawing.Color.Gray;
            chartArea6.AxisY.MajorGrid.LineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
            chartArea6.AxisY.Maximum = 100D;
            chartArea6.AxisY.Minimum = 0D;
            chartArea6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(25)))), ((int)(((byte)(26)))));
            chartArea6.BorderColor = System.Drawing.Color.Empty;
            chartArea6.Name = "ChartArea1";
            this.chrMonitorCPU.ChartAreas.Add(chartArea6);
            this.chrMonitorCPU.Location = new System.Drawing.Point(-19, 67);
            this.chrMonitorCPU.Name = "chrMonitorCPU";
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series6.IsVisibleInLegend = false;
            series6.Name = "CPU";
            series6.SmartLabelStyle.Enabled = false;
            series6.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            this.chrMonitorCPU.Series.Add(series6);
            this.chrMonitorCPU.Size = new System.Drawing.Size(450, 159);
            this.chrMonitorCPU.TabIndex = 48;
            this.chrMonitorCPU.Text = "chart1";
            // 
            // lblRamSc
            // 
            this.lblRamSc.AutoSize = true;
            this.lblRamSc.ForeColor = System.Drawing.Color.Silver;
            this.lblRamSc.Location = new System.Drawing.Point(146, 235);
            this.lblRamSc.Name = "lblRamSc";
            this.lblRamSc.Size = new System.Drawing.Size(37, 13);
            this.lblRamSc.TabIndex = 43;
            this.lblRamSc.Text = "0 MBs";
            // 
            // lblCpuSc
            // 
            this.lblCpuSc.AutoSize = true;
            this.lblCpuSc.ForeColor = System.Drawing.Color.Silver;
            this.lblCpuSc.Location = new System.Drawing.Point(114, 51);
            this.lblCpuSc.Name = "lblCpuSc";
            this.lblCpuSc.Size = new System.Drawing.Size(24, 13);
            this.lblCpuSc.TabIndex = 37;
            this.lblCpuSc.Text = "0 %";
            // 
            // lblRAM
            // 
            this.lblRAM.AutoSize = true;
            this.lblRAM.ForeColor = System.Drawing.Color.Silver;
            this.lblRAM.Location = new System.Drawing.Point(36, 235);
            this.lblRAM.Name = "lblRAM";
            this.lblRAM.Size = new System.Drawing.Size(104, 13);
            this.lblRAM.TabIndex = 31;
            this.lblRAM.Text = "Asignación de RAM:";
            // 
            // lblCPU
            // 
            this.lblCPU.AutoSize = true;
            this.lblCPU.ForeColor = System.Drawing.Color.Silver;
            this.lblCPU.Location = new System.Drawing.Point(36, 51);
            this.lblCPU.Name = "lblCPU";
            this.lblCPU.Size = new System.Drawing.Size(72, 13);
            this.lblCPU.TabIndex = 30;
            this.lblCPU.Text = "Uso de CPU: ";
            // 
            // PanelHeader
            // 
            this.PanelHeader.BackColor = System.Drawing.Color.Black;
            this.PanelHeader.Controls.Add(this.lblTitle);
            this.PanelHeader.Controls.Add(this.ButtonClose);
            this.PanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelHeader.Location = new System.Drawing.Point(0, 0);
            this.PanelHeader.Name = "PanelHeader";
            this.PanelHeader.Size = new System.Drawing.Size(429, 32);
            this.PanelHeader.TabIndex = 28;
            this.PanelHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelHeader_MouseDown);
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblTitle.ForeColor = System.Drawing.Color.Silver;
            this.lblTitle.Location = new System.Drawing.Point(23, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(297, 24);
            this.lblTitle.TabIndex = 30;
            this.lblTitle.Text = "title";
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseDown);
            // 
            // ButtonClose
            // 
            this.ButtonClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonClose.Image = ((System.Drawing.Image)(resources.GetObject("ButtonClose.Image")));
            this.ButtonClose.ImageActive = null;
            this.ButtonClose.Location = new System.Drawing.Point(402, 5);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(24, 24);
            this.ButtonClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonClose.TabIndex = 2;
            this.ButtonClose.TabStop = false;
            this.ButtonClose.Zoom = 10;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // tmrMonitor
            // 
            this.tmrMonitor.Enabled = true;
            this.tmrMonitor.Interval = 1000;
            this.tmrMonitor.Tick += new System.EventHandler(this.tmrMonitor_Tick);
            // 
            // pfcCPU
            // 
            this.pfcCPU.CategoryName = "Processor";
            this.pfcCPU.CounterName = "% Processor Time";
            this.pfcCPU.InstanceName = "_Total";
            // 
            // pfCPUSC
            // 
            this.pfCPUSC.CategoryName = "Process";
            this.pfCPUSC.CounterName = "% Processor Time";
            this.pfCPUSC.InstanceName = "SmartClient";
            // 
            // pfcDisk
            // 
            this.pfcDisk.CategoryName = "PhysicalDisk";
            this.pfcDisk.CounterName = "% Disk Time";
            this.pfcDisk.InstanceName = "_Total";
            // 
            // ResourceMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "ResourceMonitor";
            this.Size = new System.Drawing.Size(433, 608);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chrMonitorDisk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrMonitorRAM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrMonitorCPU)).EndInit();
            this.PanelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pfcCPU)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pfCPUSC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pfcDisk)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel PanelHeader;
        private System.Windows.Forms.Label lblTitle;
        private Bunifu.Framework.UI.BunifuImageButton ButtonClose;
        private System.Windows.Forms.Label lblRAM;
        private System.Windows.Forms.Label lblCPU;
        private System.Windows.Forms.Timer tmrMonitor;
        private System.Windows.Forms.Label lblCpuSc;
        private System.Diagnostics.PerformanceCounter pfcCPU;
        private System.Diagnostics.PerformanceCounter pfCPUSC;
        private System.Windows.Forms.Label lblRamSc;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrMonitorCPU;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrMonitorRAM;
        private System.Windows.Forms.DataVisualization.Charting.Chart chrMonitorDisk;
        private System.Windows.Forms.Label lblDiskSc;
        private System.Windows.Forms.Label lblDisk;
        private System.Diagnostics.PerformanceCounter pfcDisk;
    }
}
