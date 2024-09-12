namespace REH_Presentacion.TalentoHumano.Transacciones
{
    partial class frmTrArchivoPagos
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
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.cmbPersona = new DevExpress.XtraEditors.LookUpEdit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumeroCuenta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoCuenta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFormaPago.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBanco.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPersona.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.groupBox1.Controls.Add(this.lblPeriodo);
            this.groupBox1.Controls.Add(this.cmbPersona);
            this.groupBox1.Location = new System.Drawing.Point(3, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(477, 164);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // txtValor
            // 
            this.txtValor.Location = new System.Drawing.Point(197, 127);
            this.txtValor.Name = "txtValor";
            this.txtValor.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtValor.Properties.MaxLength = 15;
            this.txtValor.Size = new System.Drawing.Size(93, 20);
            this.txtValor.TabIndex = 105;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label4.Location = new System.Drawing.Point(156, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 106;
            this.label4.Text = "Valor:";
            // 
            // txtNumeroCuenta
            // 
            this.txtNumeroCuenta.Location = new System.Drawing.Point(296, 90);
            this.txtNumeroCuenta.Name = "txtNumeroCuenta";
            this.txtNumeroCuenta.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNumeroCuenta.Properties.MaxLength = 15;
            this.txtNumeroCuenta.Size = new System.Drawing.Size(137, 20);
            this.txtNumeroCuenta.TabIndex = 103;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.label61.Location = new System.Drawing.Point(223, 93);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(57, 13);
            this.label61.TabIndex = 104;
            this.label61.Text = "# Cuenta:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-1, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Tipo Cuenta:";
            // 
            // cmbTipoCuenta
            // 
            this.cmbTipoCuenta.Location = new System.Drawing.Point(73, 90);
            this.cmbTipoCuenta.Name = "cmbTipoCuenta";
            this.cmbTipoCuenta.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipoCuenta.Properties.NullText = "";
            this.cmbTipoCuenta.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbTipoCuenta.Properties.PopupWidth = 10;
            this.cmbTipoCuenta.Properties.ShowFooter = false;
            this.cmbTipoCuenta.Properties.ShowHeader = false;
            this.cmbTipoCuenta.Size = new System.Drawing.Size(137, 20);
            this.cmbTipoCuenta.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(223, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Forma Pago:";
            // 
            // cmbFormaPago
            // 
            this.cmbFormaPago.Location = new System.Drawing.Point(296, 54);
            this.cmbFormaPago.Name = "cmbFormaPago";
            this.cmbFormaPago.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbFormaPago.Properties.NullText = "";
            this.cmbFormaPago.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbFormaPago.Properties.PopupWidth = 10;
            this.cmbFormaPago.Properties.ShowFooter = false;
            this.cmbFormaPago.Properties.ShowHeader = false;
            this.cmbFormaPago.Size = new System.Drawing.Size(137, 20);
            this.cmbFormaPago.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Banco:";
            // 
            // cmbBanco
            // 
            this.cmbBanco.Location = new System.Drawing.Point(73, 54);
            this.cmbBanco.Name = "cmbBanco";
            this.cmbBanco.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBanco.Properties.NullText = "";
            this.cmbBanco.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbBanco.Properties.PopupWidth = 10;
            this.cmbBanco.Properties.ShowFooter = false;
            this.cmbBanco.Properties.ShowHeader = false;
            this.cmbBanco.Size = new System.Drawing.Size(137, 20);
            this.cmbBanco.TabIndex = 24;
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Location = new System.Drawing.Point(10, 20);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(57, 13);
            this.lblPeriodo.TabIndex = 23;
            this.lblPeriodo.Text = "Empleado:";
            // 
            // cmbPersona
            // 
            this.cmbPersona.Location = new System.Drawing.Point(73, 17);
            this.cmbPersona.Name = "cmbPersona";
            this.cmbPersona.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbPersona.Properties.NullText = "";
            this.cmbPersona.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbPersona.Properties.PopupWidth = 10;
            this.cmbPersona.Properties.ShowFooter = false;
            this.cmbPersona.Properties.ShowHeader = false;
            this.cmbPersona.Size = new System.Drawing.Size(360, 20);
            this.cmbPersona.TabIndex = 22;
            this.cmbPersona.EditValueChanged += new System.EventHandler(this.cmbPersona_EditValueChanged);
            // 
            // frmTrArchivoPagos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 226);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmTrArchivoPagos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTrArchivoPagos";
            this.Load += new System.EventHandler(this.frmTrArchivoPagos_Load);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumeroCuenta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipoCuenta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFormaPago.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBanco.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPersona.Properties)).EndInit();
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
        protected System.Windows.Forms.Label lblPeriodo;
        public DevExpress.XtraEditors.LookUpEdit cmbPersona;
        private DevExpress.XtraEditors.TextEdit txtNumeroCuenta;
        private System.Windows.Forms.Label label61;
        private DevExpress.XtraEditors.TextEdit txtValor;
        private System.Windows.Forms.Label label4;
    }
}