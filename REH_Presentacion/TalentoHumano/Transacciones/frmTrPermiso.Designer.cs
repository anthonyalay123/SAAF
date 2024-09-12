namespace REH_Presentacion.Transacciones
{
    partial class frmTrPermiso
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.txtObservacion = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpFechaFinPeriodoLactancia = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbEmpleadoCubre = new DevExpress.XtraEditors.LookUpEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbTipo = new DevExpress.XtraEditors.LookUpEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpFechaInicio = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbEmpleado = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtObservacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmpleadoCubre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmpleado.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Controls.Add(this.txtObservacion);
            this.panelControl1.Controls.Add(this.label7);
            this.panelControl1.Controls.Add(this.label6);
            this.panelControl1.Controls.Add(this.dtpFechaFinPeriodoLactancia);
            this.panelControl1.Controls.Add(this.label5);
            this.panelControl1.Controls.Add(this.cmbEmpleadoCubre);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Controls.Add(this.cmbTipo);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.dtpFechaFin);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.dtpFechaInicio);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.cmbEmpleado);
            this.panelControl1.Location = new System.Drawing.Point(12, 41);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(421, 198);
            this.panelControl1.TabIndex = 31;
            // 
            // txtObservacion
            // 
            this.txtObservacion.EditValue = "";
            this.txtObservacion.Location = new System.Drawing.Point(88, 98);
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtObservacion.Properties.MaxLength = 100;
            this.txtObservacion.Size = new System.Drawing.Size(312, 20);
            this.txtObservacion.TabIndex = 41;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 101);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "Observación:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(132, 173);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(144, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "Fecha Fin Periodo Lactancia:";
            // 
            // dtpFechaFinPeriodoLactancia
            // 
            this.dtpFechaFinPeriodoLactancia.Checked = false;
            this.dtpFechaFinPeriodoLactancia.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaFinPeriodoLactancia.Location = new System.Drawing.Point(293, 167);
            this.dtpFechaFinPeriodoLactancia.Name = "dtpFechaFinPeriodoLactancia";
            this.dtpFechaFinPeriodoLactancia.Size = new System.Drawing.Size(107, 21);
            this.dtpFechaFinPeriodoLactancia.TabIndex = 32;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(14, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 29);
            this.label5.TabIndex = 31;
            this.label5.Text = "Empleado Cubre:";
            // 
            // cmbEmpleadoCubre
            // 
            this.cmbEmpleadoCubre.Location = new System.Drawing.Point(88, 137);
            this.cmbEmpleadoCubre.Name = "cmbEmpleadoCubre";
            this.cmbEmpleadoCubre.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbEmpleadoCubre.Properties.NullText = "";
            this.cmbEmpleadoCubre.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbEmpleadoCubre.Properties.PopupWidth = 10;
            this.cmbEmpleadoCubre.Properties.ShowFooter = false;
            this.cmbEmpleadoCubre.Properties.ShowHeader = false;
            this.cmbEmpleadoCubre.Size = new System.Drawing.Size(312, 20);
            this.cmbEmpleadoCubre.TabIndex = 30;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Tipo:";
            // 
            // cmbTipo
            // 
            this.cmbTipo.Location = new System.Drawing.Point(88, 38);
            this.cmbTipo.Name = "cmbTipo";
            this.cmbTipo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipo.Properties.NullText = "";
            this.cmbTipo.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbTipo.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbTipo.Properties.PopupWidth = 10;
            this.cmbTipo.Properties.ShowFooter = false;
            this.cmbTipo.Properties.ShowHeader = false;
            this.cmbTipo.Size = new System.Drawing.Size(312, 20);
            this.cmbTipo.TabIndex = 28;
            this.cmbTipo.EditValueChanged += new System.EventHandler(this.cmbTipo_EditValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(219, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Fecha Fin:";
            // 
            // dtpFechaFin
            // 
            this.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaFin.Location = new System.Drawing.Point(293, 64);
            this.dtpFechaFin.Name = "dtpFechaFin";
            this.dtpFechaFin.Size = new System.Drawing.Size(107, 21);
            this.dtpFechaFin.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Fecha Inicio:";
            // 
            // dtpFechaInicio
            // 
            this.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaInicio.Location = new System.Drawing.Point(88, 64);
            this.dtpFechaInicio.Name = "dtpFechaInicio";
            this.dtpFechaInicio.Size = new System.Drawing.Size(107, 21);
            this.dtpFechaInicio.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Empleado:";
            // 
            // cmbEmpleado
            // 
            this.cmbEmpleado.Location = new System.Drawing.Point(88, 12);
            this.cmbEmpleado.Name = "cmbEmpleado";
            this.cmbEmpleado.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbEmpleado.Properties.NullText = "";
            this.cmbEmpleado.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbEmpleado.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbEmpleado.Properties.PopupWidth = 10;
            this.cmbEmpleado.Properties.ShowFooter = false;
            this.cmbEmpleado.Properties.ShowHeader = false;
            this.cmbEmpleado.Size = new System.Drawing.Size(312, 20);
            this.cmbEmpleado.TabIndex = 1;
            // 
            // frmTrPermiso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 251);
            this.Controls.Add(this.panelControl1);
            this.Name = "frmTrPermiso";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmPaRubroTipoRol";
            this.Load += new System.EventHandler(this.frmPaRubroTipoRol_Load);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtObservacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmpleadoCubre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmpleado.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl1;
        protected System.Windows.Forms.Label label1;
        public DevExpress.XtraEditors.LookUpEdit cmbEmpleado;
        protected System.Windows.Forms.Label label4;
        public DevExpress.XtraEditors.LookUpEdit cmbTipo;
        protected System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFechaFin;
        protected System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpFechaInicio;
        protected System.Windows.Forms.Label label5;
        public DevExpress.XtraEditors.LookUpEdit cmbEmpleadoCubre;
        protected System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpFechaFinPeriodoLactancia;
        protected System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.TextEdit txtObservacion;
    }
}