namespace REH_Presentacion.Compras.Parametrizadores
{
    partial class frmPaParametro
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
            this.tbcPrincipal = new DevExpress.XtraTab.XtraTabControl();
            this.tbpGeneral = new DevExpress.XtraTab.XtraTabPage();
            this.txtCodigo = new DevExpress.XtraEditors.TextEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.tbpNomina = new DevExpress.XtraTab.XtraTabPage();
            this.txtSemana = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCodigoIntitucionFinancieraMultiCash = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCuentaBancariaEmpresa = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCodigoEmpresaMultiChash = new DevExpress.XtraEditors.TextEdit();
            this.lblCodigoSri = new System.Windows.Forms.Label();
            this.dtpFechaConsultaGuiaDesde = new DevExpress.XtraEditors.DateEdit();
            this.labelControl22 = new DevExpress.XtraEditors.LabelControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbPermitirSeleccionarDiferentesBodegasEnGuias = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbcPrincipal)).BeginInit();
            this.tbcPrincipal.SuspendLayout();
            this.tbpGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            this.tbpNomina.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSemana.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigoIntitucionFinancieraMultiCash.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCuentaBancariaEmpresa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigoEmpresaMultiChash.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaConsultaGuiaDesde.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaConsultaGuiaDesde.Properties.CalendarTimeProperties)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chbPermitirSeleccionarDiferentesBodegasEnGuias.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.tbcPrincipal);
            this.panelControl1.Location = new System.Drawing.Point(12, 41);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(521, 306);
            this.panelControl1.TabIndex = 0;
            // 
            // tbcPrincipal
            // 
            this.tbcPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcPrincipal.Location = new System.Drawing.Point(2, 2);
            this.tbcPrincipal.Name = "tbcPrincipal";
            this.tbcPrincipal.SelectedTabPage = this.tbpGeneral;
            this.tbcPrincipal.Size = new System.Drawing.Size(517, 302);
            this.tbcPrincipal.TabIndex = 0;
            this.tbcPrincipal.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tbpGeneral,
            this.tbpNomina});
            // 
            // tbpGeneral
            // 
            this.tbpGeneral.Controls.Add(this.groupBox1);
            this.tbpGeneral.Controls.Add(this.txtCodigo);
            this.tbpGeneral.Controls.Add(this.label9);
            this.tbpGeneral.Name = "tbpGeneral";
            this.tbpGeneral.Size = new System.Drawing.Size(515, 279);
            this.tbpGeneral.Text = "Compras";
            // 
            // txtCodigo
            // 
            this.txtCodigo.Enabled = false;
            this.txtCodigo.Location = new System.Drawing.Point(128, 12);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.MaxLength = 20;
            this.txtCodigo.Size = new System.Drawing.Size(58, 20);
            this.txtCodigo.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Código:";
            // 
            // tbpNomina
            // 
            this.tbpNomina.Controls.Add(this.txtSemana);
            this.tbpNomina.Controls.Add(this.label3);
            this.tbpNomina.Controls.Add(this.txtCodigoIntitucionFinancieraMultiCash);
            this.tbpNomina.Controls.Add(this.label2);
            this.tbpNomina.Controls.Add(this.txtCuentaBancariaEmpresa);
            this.tbpNomina.Controls.Add(this.label1);
            this.tbpNomina.Controls.Add(this.txtCodigoEmpresaMultiChash);
            this.tbpNomina.Controls.Add(this.lblCodigoSri);
            this.tbpNomina.Name = "tbpNomina";
            this.tbpNomina.Size = new System.Drawing.Size(515, 279);
            this.tbpNomina.Text = "Orden de Pago";
            // 
            // txtSemana
            // 
            this.txtSemana.Location = new System.Drawing.Point(116, 107);
            this.txtSemana.Name = "txtSemana";
            this.txtSemana.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtSemana.Properties.MaxLength = 100;
            this.txtSemana.Size = new System.Drawing.Size(375, 20);
            this.txtSemana.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Semana:";
            // 
            // txtCodigoIntitucionFinancieraMultiCash
            // 
            this.txtCodigoIntitucionFinancieraMultiCash.Location = new System.Drawing.Point(391, 22);
            this.txtCodigoIntitucionFinancieraMultiCash.Name = "txtCodigoIntitucionFinancieraMultiCash";
            this.txtCodigoIntitucionFinancieraMultiCash.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCodigoIntitucionFinancieraMultiCash.Properties.MaxLength = 20;
            this.txtCodigoIntitucionFinancieraMultiCash.Size = new System.Drawing.Size(100, 20);
            this.txtCodigoIntitucionFinancieraMultiCash.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(283, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 41);
            this.label2.TabIndex = 4;
            this.label2.Text = "Código Institución Financiera MultiCash:";
            // 
            // txtCuentaBancariaEmpresa
            // 
            this.txtCuentaBancariaEmpresa.Location = new System.Drawing.Point(116, 66);
            this.txtCuentaBancariaEmpresa.Name = "txtCuentaBancariaEmpresa";
            this.txtCuentaBancariaEmpresa.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCuentaBancariaEmpresa.Properties.MaxLength = 20;
            this.txtCuentaBancariaEmpresa.Size = new System.Drawing.Size(127, 20);
            this.txtCuentaBancariaEmpresa.TabIndex = 3;
            this.txtCuentaBancariaEmpresa.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SoloNumeros);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 37);
            this.label1.TabIndex = 2;
            this.label1.Text = "Cuenta Bancaria Empresa:";
            // 
            // txtCodigoEmpresaMultiChash
            // 
            this.txtCodigoEmpresaMultiChash.Location = new System.Drawing.Point(116, 22);
            this.txtCodigoEmpresaMultiChash.Name = "txtCodigoEmpresaMultiChash";
            this.txtCodigoEmpresaMultiChash.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCodigoEmpresaMultiChash.Properties.MaxLength = 20;
            this.txtCodigoEmpresaMultiChash.Size = new System.Drawing.Size(127, 20);
            this.txtCodigoEmpresaMultiChash.TabIndex = 1;
            this.txtCodigoEmpresaMultiChash.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SoloNumeros);
            // 
            // lblCodigoSri
            // 
            this.lblCodigoSri.Location = new System.Drawing.Point(12, 11);
            this.lblCodigoSri.Name = "lblCodigoSri";
            this.lblCodigoSri.Size = new System.Drawing.Size(72, 41);
            this.lblCodigoSri.TabIndex = 0;
            this.lblCodigoSri.Text = "Código Empresa MultiCash:";
            // 
            // dtpFechaConsultaGuiaDesde
            // 
            this.dtpFechaConsultaGuiaDesde.EditValue = null;
            this.dtpFechaConsultaGuiaDesde.Location = new System.Drawing.Point(166, 21);
            this.dtpFechaConsultaGuiaDesde.Name = "dtpFechaConsultaGuiaDesde";
            this.dtpFechaConsultaGuiaDesde.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpFechaConsultaGuiaDesde.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpFechaConsultaGuiaDesde.Properties.DisplayFormat.FormatString = "";
            this.dtpFechaConsultaGuiaDesde.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtpFechaConsultaGuiaDesde.Properties.EditFormat.FormatString = "";
            this.dtpFechaConsultaGuiaDesde.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtpFechaConsultaGuiaDesde.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dtpFechaConsultaGuiaDesde.Size = new System.Drawing.Size(124, 20);
            this.dtpFechaConsultaGuiaDesde.TabIndex = 96;
            // 
            // labelControl22
            // 
            this.labelControl22.Location = new System.Drawing.Point(6, 24);
            this.labelControl22.Name = "labelControl22";
            this.labelControl22.Size = new System.Drawing.Size(140, 13);
            this.labelControl22.TabIndex = 97;
            this.labelControl22.Text = "Fecha Consulta Guias Desde:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbPermitirSeleccionarDiferentesBodegasEnGuias);
            this.groupBox1.Controls.Add(this.labelControl22);
            this.groupBox1.Controls.Add(this.dtpFechaConsultaGuiaDesde);
            this.groupBox1.Location = new System.Drawing.Point(17, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(482, 91);
            this.groupBox1.TabIndex = 98;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Guia de Remisión";
            // 
            // chbPermitirSeleccionarDiferentesBodegasEnGuias
            // 
            this.chbPermitirSeleccionarDiferentesBodegasEnGuias.Location = new System.Drawing.Point(6, 53);
            this.chbPermitirSeleccionarDiferentesBodegasEnGuias.Name = "chbPermitirSeleccionarDiferentesBodegasEnGuias";
            this.chbPermitirSeleccionarDiferentesBodegasEnGuias.Properties.Caption = "Permitir Seleccionar Diferentes Bodegas En Guias";
            this.chbPermitirSeleccionarDiferentesBodegasEnGuias.Size = new System.Drawing.Size(284, 18);
            this.chbPermitirSeleccionarDiferentesBodegasEnGuias.TabIndex = 98;
            // 
            // frmPaParametro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 349);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPaParametro";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmParametros";
            this.Load += new System.EventHandler(this.frmParametro_Load);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbcPrincipal)).EndInit();
            this.tbcPrincipal.ResumeLayout(false);
            this.tbpGeneral.ResumeLayout(false);
            this.tbpGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            this.tbpNomina.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSemana.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigoIntitucionFinancieraMultiCash.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCuentaBancariaEmpresa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigoEmpresaMultiChash.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaConsultaGuiaDesde.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaConsultaGuiaDesde.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chbPermitirSeleccionarDiferentesBodegasEnGuias.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraTab.XtraTabControl tbcPrincipal;
        private DevExpress.XtraTab.XtraTabPage tbpGeneral;
        private DevExpress.XtraTab.XtraTabPage tbpNomina;
        private DevExpress.XtraEditors.TextEdit txtCuentaBancariaEmpresa;
        protected System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtCodigoEmpresaMultiChash;
        protected System.Windows.Forms.Label lblCodigoSri;
        private DevExpress.XtraEditors.TextEdit txtCodigo;
        protected System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.TextEdit txtCodigoIntitucionFinancieraMultiCash;
        protected System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txtSemana;
        protected System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.LabelControl labelControl22;
        private DevExpress.XtraEditors.DateEdit dtpFechaConsultaGuiaDesde;
        private DevExpress.XtraEditors.CheckEdit chbPermitirSeleccionarDiferentesBodegasEnGuias;
    }
}