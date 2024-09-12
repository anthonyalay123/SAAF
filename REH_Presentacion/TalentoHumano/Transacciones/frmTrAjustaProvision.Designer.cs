namespace REH_Presentacion.Transacciones
{
    partial class frmTrAjustaProvision
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
            this.components = new System.ComponentModel.Container();
            this.txtAnio = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbEmpleado = new DevExpress.XtraEditors.LookUpEdit();
            this.txtMes = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtProvisionFondoReserva = new DevExpress.XtraEditors.TextEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.txtProvisionDecimoTercero = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.txtProvisionVacaciones = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTotalIngresos = new DevExpress.XtraEditors.TextEdit();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chbAjustarLiquidado = new DevExpress.XtraEditors.CheckEdit();
            this.chbAjustarManualmente = new DevExpress.XtraEditors.CheckEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.txtProvisionFondoReservaAjuste = new DevExpress.XtraEditors.TextEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.txtProvisionDecimoTerceroAjuste = new DevExpress.XtraEditors.TextEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.txtProvisionVacacionesAjuste = new DevExpress.XtraEditors.TextEdit();
            this.label11 = new System.Windows.Forms.Label();
            this.txtTotalIngresosAjuste = new DevExpress.XtraEditors.TextEdit();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gcDatos = new DevExpress.XtraGrid.GridControl();
            this.dgvDatos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.bsDatos = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtAnio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmpleado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMes.Properties)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtProvisionFondoReserva.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProvisionDecimoTercero.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProvisionVacaciones.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalIngresos.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chbAjustarLiquidado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbAjustarManualmente.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProvisionFondoReservaAjuste.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProvisionDecimoTerceroAjuste.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProvisionVacacionesAjuste.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalIngresosAjuste.Properties)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).BeginInit();
            this.SuspendLayout();
            // 
            // txtAnio
            // 
            this.txtAnio.Location = new System.Drawing.Point(527, 41);
            this.txtAnio.Name = "txtAnio";
            this.txtAnio.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAnio.Properties.MaxLength = 4;
            this.txtAnio.Size = new System.Drawing.Size(66, 20);
            this.txtAnio.TabIndex = 1;
            this.txtAnio.Leave += new System.EventHandler(this.cmbEmpleado_EditValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(453, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Año:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Empleado:";
            // 
            // cmbEmpleado
            // 
            this.cmbEmpleado.Location = new System.Drawing.Point(87, 41);
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
            this.cmbEmpleado.TabIndex = 0;
            this.cmbEmpleado.EditValueChanged += new System.EventHandler(this.cmbEmpleado_EditValueChanged);
            // 
            // txtMes
            // 
            this.txtMes.Location = new System.Drawing.Point(706, 41);
            this.txtMes.Name = "txtMes";
            this.txtMes.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMes.Properties.MaxLength = 4;
            this.txtMes.Size = new System.Drawing.Size(66, 20);
            this.txtMes.TabIndex = 2;
            this.txtMes.Leave += new System.EventHandler(this.cmbEmpleado_EditValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(660, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Mes:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtProvisionFondoReserva);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtProvisionDecimoTercero);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtProvisionVacaciones);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtTotalIngresos);
            this.groupBox1.Location = new System.Drawing.Point(12, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(399, 100);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos en Nómina";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(305, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 27);
            this.label7.TabIndex = 37;
            this.label7.Text = "Provisión   Fondo Reserva:";
            // 
            // txtProvisionFondoReserva
            // 
            this.txtProvisionFondoReserva.Location = new System.Drawing.Point(308, 46);
            this.txtProvisionFondoReserva.Name = "txtProvisionFondoReserva";
            this.txtProvisionFondoReserva.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtProvisionFondoReserva.Properties.MaxLength = 4;
            this.txtProvisionFondoReserva.Properties.ReadOnly = true;
            this.txtProvisionFondoReserva.Size = new System.Drawing.Size(74, 20);
            this.txtProvisionFondoReserva.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(205, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 27);
            this.label6.TabIndex = 35;
            this.label6.Text = "Provisión Décimo Tercero:";
            // 
            // txtProvisionDecimoTercero
            // 
            this.txtProvisionDecimoTercero.Location = new System.Drawing.Point(208, 46);
            this.txtProvisionDecimoTercero.Name = "txtProvisionDecimoTercero";
            this.txtProvisionDecimoTercero.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtProvisionDecimoTercero.Properties.MaxLength = 4;
            this.txtProvisionDecimoTercero.Properties.ReadOnly = true;
            this.txtProvisionDecimoTercero.Size = new System.Drawing.Size(74, 20);
            this.txtProvisionDecimoTercero.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(104, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 27);
            this.label5.TabIndex = 33;
            this.label5.Text = "Provisión Vacaciones:";
            // 
            // txtProvisionVacaciones
            // 
            this.txtProvisionVacaciones.Location = new System.Drawing.Point(107, 46);
            this.txtProvisionVacaciones.Name = "txtProvisionVacaciones";
            this.txtProvisionVacaciones.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtProvisionVacaciones.Properties.MaxLength = 4;
            this.txtProvisionVacaciones.Properties.ReadOnly = true;
            this.txtProvisionVacaciones.Size = new System.Drawing.Size(74, 20);
            this.txtProvisionVacaciones.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 27);
            this.label4.TabIndex = 31;
            this.label4.Text = "Total Ingresos:";
            // 
            // txtTotalIngresos
            // 
            this.txtTotalIngresos.Location = new System.Drawing.Point(9, 46);
            this.txtTotalIngresos.Name = "txtTotalIngresos";
            this.txtTotalIngresos.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTotalIngresos.Properties.MaxLength = 4;
            this.txtTotalIngresos.Properties.ReadOnly = true;
            this.txtTotalIngresos.Size = new System.Drawing.Size(74, 20);
            this.txtTotalIngresos.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chbAjustarLiquidado);
            this.groupBox2.Controls.Add(this.chbAjustarManualmente);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtProvisionFondoReservaAjuste);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtProvisionDecimoTerceroAjuste);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtProvisionVacacionesAjuste);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.txtTotalIngresosAjuste);
            this.groupBox2.Location = new System.Drawing.Point(438, 76);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(399, 100);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Datos a Ajustar";
            // 
            // chbAjustarLiquidado
            // 
            this.chbAjustarLiquidado.Location = new System.Drawing.Point(260, 72);
            this.chbAjustarLiquidado.Name = "chbAjustarLiquidado";
            this.chbAjustarLiquidado.Properties.Caption = "Ajustar Liquidados";
            this.chbAjustarLiquidado.Size = new System.Drawing.Size(122, 19);
            this.chbAjustarLiquidado.TabIndex = 39;
            this.chbAjustarLiquidado.Visible = false;
            // 
            // chbAjustarManualmente
            // 
            this.chbAjustarManualmente.Location = new System.Drawing.Point(9, 72);
            this.chbAjustarManualmente.Name = "chbAjustarManualmente";
            this.chbAjustarManualmente.Properties.Caption = "Ajustar Manualmente";
            this.chbAjustarManualmente.Size = new System.Drawing.Size(122, 19);
            this.chbAjustarManualmente.TabIndex = 38;
            this.chbAjustarManualmente.CheckedChanged += new System.EventHandler(this.chbAjustarManualmente_CheckedChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(305, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(87, 27);
            this.label8.TabIndex = 37;
            this.label8.Text = "Provisión   Fondo Reserva:";
            // 
            // txtProvisionFondoReservaAjuste
            // 
            this.txtProvisionFondoReservaAjuste.Location = new System.Drawing.Point(308, 46);
            this.txtProvisionFondoReservaAjuste.Name = "txtProvisionFondoReservaAjuste";
            this.txtProvisionFondoReservaAjuste.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtProvisionFondoReservaAjuste.Properties.MaxLength = 4;
            this.txtProvisionFondoReservaAjuste.Properties.ReadOnly = true;
            this.txtProvisionFondoReservaAjuste.Size = new System.Drawing.Size(74, 20);
            this.txtProvisionFondoReservaAjuste.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(205, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 27);
            this.label9.TabIndex = 35;
            this.label9.Text = "Provisión Décimo Tercero:";
            // 
            // txtProvisionDecimoTerceroAjuste
            // 
            this.txtProvisionDecimoTerceroAjuste.Location = new System.Drawing.Point(208, 46);
            this.txtProvisionDecimoTerceroAjuste.Name = "txtProvisionDecimoTerceroAjuste";
            this.txtProvisionDecimoTerceroAjuste.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtProvisionDecimoTerceroAjuste.Properties.MaxLength = 4;
            this.txtProvisionDecimoTerceroAjuste.Properties.ReadOnly = true;
            this.txtProvisionDecimoTerceroAjuste.Size = new System.Drawing.Size(74, 20);
            this.txtProvisionDecimoTerceroAjuste.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(104, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 27);
            this.label10.TabIndex = 33;
            this.label10.Text = "Provisión Vacaciones:";
            // 
            // txtProvisionVacacionesAjuste
            // 
            this.txtProvisionVacacionesAjuste.Location = new System.Drawing.Point(107, 46);
            this.txtProvisionVacacionesAjuste.Name = "txtProvisionVacacionesAjuste";
            this.txtProvisionVacacionesAjuste.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(247)))));
            this.txtProvisionVacacionesAjuste.Properties.Appearance.Options.UseBackColor = true;
            this.txtProvisionVacacionesAjuste.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtProvisionVacacionesAjuste.Properties.MaxLength = 4;
            this.txtProvisionVacacionesAjuste.Properties.ReadOnly = true;
            this.txtProvisionVacacionesAjuste.Size = new System.Drawing.Size(74, 20);
            this.txtProvisionVacacionesAjuste.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(6, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 27);
            this.label11.TabIndex = 31;
            this.label11.Text = "Total Ingresos:";
            // 
            // txtTotalIngresosAjuste
            // 
            this.txtTotalIngresosAjuste.Location = new System.Drawing.Point(9, 46);
            this.txtTotalIngresosAjuste.Name = "txtTotalIngresosAjuste";
            this.txtTotalIngresosAjuste.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTotalIngresosAjuste.Properties.MaxLength = 8;
            this.txtTotalIngresosAjuste.Size = new System.Drawing.Size(74, 20);
            this.txtTotalIngresosAjuste.TabIndex = 0;
            this.txtTotalIngresosAjuste.Leave += new System.EventHandler(this.txtTotalIngresosAjuste_Leave);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.gcDatos);
            this.groupBox3.Location = new System.Drawing.Point(12, 182);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(825, 320);
            this.groupBox3.TabIndex = 39;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Datos Preliminares a Ajustar";
            // 
            // gcDatos
            // 
            this.gcDatos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDatos.Location = new System.Drawing.Point(9, 19);
            this.gcDatos.MainView = this.dgvDatos;
            this.gcDatos.Name = "gcDatos";
            this.gcDatos.Size = new System.Drawing.Size(816, 295);
            this.gcDatos.TabIndex = 1;
            this.gcDatos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvDatos});
            // 
            // dgvDatos
            // 
            this.dgvDatos.GridControl = this.gcDatos;
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.dgvDatos.OptionsCustomization.AllowColumnMoving = false;
            this.dgvDatos.OptionsView.ShowAutoFilterRow = true;
            this.dgvDatos.OptionsView.ShowFooter = true;
            this.dgvDatos.OptionsView.ShowGroupPanel = false;
            // 
            // frmTrAjustaProvision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 505);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtMes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbEmpleado);
            this.Controls.Add(this.txtAnio);
            this.Controls.Add(this.label2);
            this.Name = "frmTrAjustaProvision";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTrAjustaProvision";
            this.Load += new System.EventHandler(this.frmTrAjustaProvision_Load);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtAnio, 0);
            this.Controls.SetChildIndex(this.cmbEmpleado, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtMes, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.groupBox3, 0);
            ((System.ComponentModel.ISupportInitialize)(this.txtAnio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmpleado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMes.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtProvisionFondoReserva.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProvisionDecimoTercero.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProvisionVacaciones.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalIngresos.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chbAjustarLiquidado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbAjustarManualmente.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProvisionFondoReservaAjuste.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProvisionDecimoTerceroAjuste.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProvisionVacacionesAjuste.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotalIngresosAjuste.Properties)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtAnio;
        private System.Windows.Forms.Label label2;
        protected System.Windows.Forms.Label label1;
        public DevExpress.XtraEditors.LookUpEdit cmbEmpleado;
        private DevExpress.XtraEditors.TextEdit txtMes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.TextEdit txtProvisionFondoReserva;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit txtProvisionDecimoTercero;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.TextEdit txtProvisionVacaciones;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txtTotalIngresos;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.TextEdit txtProvisionFondoReservaAjuste;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.TextEdit txtProvisionDecimoTerceroAjuste;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.TextEdit txtProvisionVacacionesAjuste;
        private System.Windows.Forms.Label label11;
        private DevExpress.XtraEditors.TextEdit txtTotalIngresosAjuste;
        private DevExpress.XtraEditors.CheckEdit chbAjustarManualmente;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevExpress.XtraGrid.GridControl gcDatos;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvDatos;
        private System.Windows.Forms.BindingSource bsDatos;
        private DevExpress.XtraEditors.CheckEdit chbAjustarLiquidado;
    }
}