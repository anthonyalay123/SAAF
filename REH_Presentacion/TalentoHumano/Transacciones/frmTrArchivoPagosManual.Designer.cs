namespace REH_Presentacion.TalentoHumano.Transacciones
{
    partial class frmTrArchivoPagosManual
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtNombre = new DevExpress.XtraEditors.TextEdit();
            this.txtNumeroIdentificacion = new DevExpress.XtraEditors.TextEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.txtValor = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNumeroCuenta = new DevExpress.XtraEditors.TextEdit();
            this.label61 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbTipoCuenta = new DevExpress.XtraEditors.LookUpEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbFormaPago = new DevExpress.XtraEditors.LookUpEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbBanco = new DevExpress.XtraEditors.LookUpEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbTipoIdentificacion = new DevExpress.XtraEditors.LookUpEdit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumeroIdentificacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumeroCuenta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoCuenta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFormaPago.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBanco.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoIdentificacion.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cmbTipoIdentificacion);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtNombre);
            this.groupBox1.Controls.Add(this.txtNumeroIdentificacion);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtValor);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtNumeroCuenta);
            this.groupBox1.Controls.Add(this.label61);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbTipoCuenta);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbFormaPago);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbBanco);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(619, 164);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(283, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 13);
            this.label6.TabIndex = 110;
            this.label6.Text = "Nombre:";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(336, 17);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNombre.Properties.MaxLength = 30;
            this.txtNombre.Size = new System.Drawing.Size(257, 20);
            this.txtNombre.TabIndex = 1;
            // 
            // txtNumeroIdentificacion
            // 
            this.txtNumeroIdentificacion.Location = new System.Drawing.Point(106, 17);
            this.txtNumeroIdentificacion.Name = "txtNumeroIdentificacion";
            this.txtNumeroIdentificacion.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNumeroIdentificacion.Properties.MaxLength = 13;
            this.txtNumeroIdentificacion.Size = new System.Drawing.Size(137, 20);
            this.txtNumeroIdentificacion.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 107;
            this.label5.Text = "Identificación:";
            // 
            // txtValor
            // 
            this.txtValor.Location = new System.Drawing.Point(336, 127);
            this.txtValor.Name = "txtValor";
            this.txtValor.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtValor.Properties.MaxLength = 15;
            this.txtValor.Size = new System.Drawing.Size(93, 20);
            this.txtValor.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label4.Location = new System.Drawing.Point(295, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 106;
            this.label4.Text = "Valor:";
            // 
            // txtNumeroCuenta
            // 
            this.txtNumeroCuenta.Location = new System.Drawing.Point(106, 127);
            this.txtNumeroCuenta.Name = "txtNumeroCuenta";
            this.txtNumeroCuenta.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNumeroCuenta.Properties.MaxLength = 15;
            this.txtNumeroCuenta.Size = new System.Drawing.Size(137, 20);
            this.txtNumeroCuenta.TabIndex = 6;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label61.Location = new System.Drawing.Point(7, 130);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(57, 13);
            this.label61.TabIndex = 104;
            this.label61.Text = "# Cuenta:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(262, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Tipo Cuenta:";
            // 
            // cmbTipoCuenta
            // 
            this.cmbTipoCuenta.Location = new System.Drawing.Point(336, 91);
            this.cmbTipoCuenta.Name = "cmbTipoCuenta";
            this.cmbTipoCuenta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoCuenta.Properties.NullText = "";
            this.cmbTipoCuenta.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbTipoCuenta.Properties.PopupWidth = 10;
            this.cmbTipoCuenta.Properties.ShowFooter = false;
            this.cmbTipoCuenta.Properties.ShowHeader = false;
            this.cmbTipoCuenta.Size = new System.Drawing.Size(257, 20);
            this.cmbTipoCuenta.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Forma Pago:";
            // 
            // cmbFormaPago
            // 
            this.cmbFormaPago.Location = new System.Drawing.Point(106, 91);
            this.cmbFormaPago.Name = "cmbFormaPago";
            this.cmbFormaPago.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbFormaPago.Properties.NullText = "";
            this.cmbFormaPago.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbFormaPago.Properties.PopupWidth = 10;
            this.cmbFormaPago.Properties.ShowFooter = false;
            this.cmbFormaPago.Properties.ShowHeader = false;
            this.cmbFormaPago.Size = new System.Drawing.Size(137, 20);
            this.cmbFormaPago.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(289, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Banco:";
            // 
            // cmbBanco
            // 
            this.cmbBanco.Location = new System.Drawing.Point(336, 54);
            this.cmbBanco.Name = "cmbBanco";
            this.cmbBanco.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBanco.Properties.NullText = "";
            this.cmbBanco.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbBanco.Properties.PopupWidth = 10;
            this.cmbBanco.Properties.ShowFooter = false;
            this.cmbBanco.Properties.ShowHeader = false;
            this.cmbBanco.Size = new System.Drawing.Size(257, 20);
            this.cmbBanco.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 13);
            this.label7.TabIndex = 112;
            this.label7.Text = "Tipo Identificación:";
            // 
            // cmbTipoIdentificacion
            // 
            this.cmbTipoIdentificacion.Location = new System.Drawing.Point(106, 54);
            this.cmbTipoIdentificacion.Name = "cmbTipoIdentificacion";
            this.cmbTipoIdentificacion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoIdentificacion.Properties.NullText = "";
            this.cmbTipoIdentificacion.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbTipoIdentificacion.Properties.PopupWidth = 10;
            this.cmbTipoIdentificacion.Properties.ShowFooter = false;
            this.cmbTipoIdentificacion.Properties.ShowHeader = false;
            this.cmbTipoIdentificacion.Size = new System.Drawing.Size(137, 20);
            this.cmbTipoIdentificacion.TabIndex = 2;
            // 
            // frmTrArchivoPagosManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 211);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmTrArchivoPagosManual";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTrArchivoPagos";
            this.Load += new System.EventHandler(this.frmTrArchivoPagos_Load);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNombre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumeroIdentificacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumeroCuenta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoCuenta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFormaPago.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBanco.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoIdentificacion.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        protected System.Windows.Forms.Label label3;
        public DevExpress.XtraEditors.LookUpEdit cmbTipoCuenta;
        protected System.Windows.Forms.Label label2;
        public DevExpress.XtraEditors.LookUpEdit cmbFormaPago;
        protected System.Windows.Forms.Label label1;
        public DevExpress.XtraEditors.LookUpEdit cmbBanco;
        private DevExpress.XtraEditors.TextEdit txtNumeroCuenta;
        private System.Windows.Forms.Label label61;
        private DevExpress.XtraEditors.TextEdit txtValor;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.TextEdit txtNumeroIdentificacion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.TextEdit txtNombre;
        protected System.Windows.Forms.Label label7;
        public DevExpress.XtraEditors.LookUpEdit cmbTipoIdentificacion;
    }
}