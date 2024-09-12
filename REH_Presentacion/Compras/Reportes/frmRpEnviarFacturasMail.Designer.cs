namespace REH_Presentacion.Compras.Reportes
{
    partial class frmRpEnviarFacturasMail
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.pnc1 = new DevExpress.XtraEditors.PanelControl();
            this.cmbGrupoPago = new DevExpress.XtraEditors.LookUpEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.lblPar4 = new System.Windows.Forms.Label();
            this.txtDestinatarios = new DevExpress.XtraEditors.TextEdit();
            this.txtAsunto = new DevExpress.XtraEditors.TextEdit();
            this.lblPar3 = new System.Windows.Forms.Label();
            this.lblPar2 = new System.Windows.Forms.Label();
            this.lblPar1 = new System.Windows.Forms.Label();
            this.txtTexto = new DevExpress.XtraEditors.MemoEdit();
            this.txtFirma = new DevExpress.XtraEditors.MemoEdit();
            this.gcDatos = new DevExpress.XtraGrid.GridControl();
            this.dgvDatos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.bsDatos = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnc1)).BeginInit();
            this.pnc1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGrupoPago.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinatarios.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAsunto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTexto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFirma.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Controls.Add(this.pnc1);
            this.panelControl1.Controls.Add(this.gcDatos);
            this.panelControl1.Location = new System.Drawing.Point(0, 41);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(905, 481);
            this.panelControl1.TabIndex = 31;
            // 
            // pnc1
            // 
            this.pnc1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnc1.Controls.Add(this.cmbGrupoPago);
            this.pnc1.Controls.Add(this.label2);
            this.pnc1.Controls.Add(this.lblPar4);
            this.pnc1.Controls.Add(this.txtDestinatarios);
            this.pnc1.Controls.Add(this.txtAsunto);
            this.pnc1.Controls.Add(this.lblPar3);
            this.pnc1.Controls.Add(this.lblPar2);
            this.pnc1.Controls.Add(this.lblPar1);
            this.pnc1.Controls.Add(this.txtTexto);
            this.pnc1.Controls.Add(this.txtFirma);
            this.pnc1.Location = new System.Drawing.Point(5, 5);
            this.pnc1.Name = "pnc1";
            this.pnc1.Size = new System.Drawing.Size(895, 198);
            this.pnc1.TabIndex = 32;
            // 
            // cmbGrupoPago
            // 
            this.cmbGrupoPago.Location = new System.Drawing.Point(91, 5);
            this.cmbGrupoPago.Name = "cmbGrupoPago";
            this.cmbGrupoPago.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbGrupoPago.Properties.NullText = "";
            this.cmbGrupoPago.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbGrupoPago.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbGrupoPago.Properties.PopupWidth = 10;
            this.cmbGrupoPago.Properties.ShowFooter = false;
            this.cmbGrupoPago.Properties.ShowHeader = false;
            this.cmbGrupoPago.Properties.UseReadOnlyAppearance = false;
            this.cmbGrupoPago.Size = new System.Drawing.Size(359, 20);
            this.cmbGrupoPago.TabIndex = 54;
            this.cmbGrupoPago.EditValueChanged += new System.EventHandler(this.cmbGrupoPago_EditValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 53;
            this.label2.Text = "Grupo de Pago:";
            // 
            // lblPar4
            // 
            this.lblPar4.Location = new System.Drawing.Point(24, 139);
            this.lblPar4.Name = "lblPar4";
            this.lblPar4.Size = new System.Drawing.Size(60, 27);
            this.lblPar4.TabIndex = 41;
            this.lblPar4.Text = "Firma:";
            this.lblPar4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDestinatarios
            // 
            this.txtDestinatarios.Location = new System.Drawing.Point(544, 29);
            this.txtDestinatarios.Name = "txtDestinatarios";
            this.txtDestinatarios.Properties.MaxLength = 100;
            this.txtDestinatarios.Size = new System.Drawing.Size(346, 20);
            this.txtDestinatarios.TabIndex = 1;
            this.txtDestinatarios.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPar1_KeyDown);
            // 
            // txtAsunto
            // 
            this.txtAsunto.Location = new System.Drawing.Point(91, 29);
            this.txtAsunto.Name = "txtAsunto";
            this.txtAsunto.Properties.MaxLength = 100;
            this.txtAsunto.Size = new System.Drawing.Size(359, 20);
            this.txtAsunto.TabIndex = 0;
            this.txtAsunto.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPar1_KeyDown);
            // 
            // lblPar3
            // 
            this.lblPar3.Location = new System.Drawing.Point(456, 22);
            this.lblPar3.Name = "lblPar3";
            this.lblPar3.Size = new System.Drawing.Size(82, 31);
            this.lblPar3.TabIndex = 37;
            this.lblPar3.Text = "Destinatarios:";
            this.lblPar3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPar2
            // 
            this.lblPar2.Location = new System.Drawing.Point(8, 57);
            this.lblPar2.Name = "lblPar2";
            this.lblPar2.Size = new System.Drawing.Size(76, 13);
            this.lblPar2.TabIndex = 35;
            this.lblPar2.Text = "Texto:";
            this.lblPar2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPar1
            // 
            this.lblPar1.Location = new System.Drawing.Point(6, 30);
            this.lblPar1.Name = "lblPar1";
            this.lblPar1.Size = new System.Drawing.Size(79, 15);
            this.lblPar1.TabIndex = 33;
            this.lblPar1.Text = "Asunto:";
            this.lblPar1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtTexto
            // 
            this.txtTexto.Location = new System.Drawing.Point(91, 56);
            this.txtTexto.Name = "txtTexto";
            this.txtTexto.Properties.MaxLength = 800;
            this.txtTexto.Size = new System.Drawing.Size(799, 77);
            this.txtTexto.TabIndex = 2;
            // 
            // txtFirma
            // 
            this.txtFirma.Location = new System.Drawing.Point(91, 143);
            this.txtFirma.Name = "txtFirma";
            this.txtFirma.Properties.MaxLength = 400;
            this.txtFirma.Size = new System.Drawing.Size(799, 49);
            this.txtFirma.TabIndex = 3;
            // 
            // gcDatos
            // 
            this.gcDatos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDatos.Location = new System.Drawing.Point(5, 209);
            this.gcDatos.MainView = this.dgvDatos;
            this.gcDatos.Name = "gcDatos";
            this.gcDatos.Size = new System.Drawing.Size(900, 267);
            this.gcDatos.TabIndex = 0;
            this.gcDatos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvDatos});
            // 
            // dgvDatos
            // 
            this.dgvDatos.GridControl = this.gcDatos;
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvDatos.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.dgvDatos.OptionsBehavior.Editable = false;
            this.dgvDatos.OptionsCustomization.AllowColumnMoving = false;
            this.dgvDatos.OptionsView.ColumnAutoWidth = false;
            this.dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.dgvDatos.OptionsView.ShowAutoFilterRow = true;
            this.dgvDatos.OptionsView.ShowFooter = true;
            this.dgvDatos.OptionsView.ShowGroupPanel = false;
            // 
            // frmRpEnviarFacturasMail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 530);
            this.Controls.Add(this.panelControl1);
            this.Name = "frmRpEnviarFacturasMail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestor de Consultas";
            this.Load += new System.EventHandler(this.frmTrNomina_Load);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnc1)).EndInit();
            this.pnc1.ResumeLayout(false);
            this.pnc1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGrupoPago.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinatarios.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAsunto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTexto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFirma.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDatos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gcDatos;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvDatos;
        private System.Windows.Forms.BindingSource bsDatos;
        private DevExpress.XtraEditors.PanelControl pnc1;
        protected System.Windows.Forms.Label lblPar2;
        protected System.Windows.Forms.Label lblPar1;
        private DevExpress.XtraEditors.TextEdit txtDestinatarios;
        private DevExpress.XtraEditors.TextEdit txtAsunto;
        protected System.Windows.Forms.Label lblPar3;
        protected System.Windows.Forms.Label lblPar4;
        private DevExpress.XtraEditors.MemoEdit txtTexto;
        private DevExpress.XtraEditors.MemoEdit txtFirma;
        public DevExpress.XtraEditors.LookUpEdit cmbGrupoPago;
        private System.Windows.Forms.Label label2;
    }
}