namespace REH_Presentacion.Login
{
    partial class frmCambioContrasena
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCambioContrasena));
            this.lblClave = new System.Windows.Forms.Label();
            this.btnSalir = new DevExpress.XtraEditors.SimpleButton();
            this.btnAceptar = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRepetirClave = new System.Windows.Forms.TextBox();
            this.txtClave = new System.Windows.Forms.TextBox();
            this.txtAnterior = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblClave
            // 
            this.lblClave.AutoSize = true;
            this.lblClave.Location = new System.Drawing.Point(10, 57);
            this.lblClave.Name = "lblClave";
            this.lblClave.Size = new System.Drawing.Size(99, 13);
            this.lblClave.TabIndex = 0;
            this.lblClave.Text = "Nueva Contraseña:";
            this.lblClave.Click += new System.EventHandler(this.lblClave_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSalir.ImageOptions.Image")));
            this.btnSalir.Location = new System.Drawing.Point(151, 117);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(75, 23);
            this.btnSalir.TabIndex = 4;
            this.btnSalir.Text = "Salir";
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnAceptar
            // 
            this.btnAceptar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAceptar.ImageOptions.Image")));
            this.btnAceptar.Location = new System.Drawing.Point(48, 117);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 23);
            this.btnAceptar.TabIndex = 3;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Repetir Contraseña:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtRepetirClave
            // 
            this.txtRepetirClave.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtRepetirClave.Location = new System.Drawing.Point(115, 80);
            this.txtRepetirClave.MaxLength = 20;
            this.txtRepetirClave.Name = "txtRepetirClave";
            this.txtRepetirClave.PasswordChar = '*';
            this.txtRepetirClave.Size = new System.Drawing.Size(159, 20);
            this.txtRepetirClave.TabIndex = 2;
            this.txtRepetirClave.TextChanged += new System.EventHandler(this.txtRepetirClave_TextChanged);
            this.txtRepetirClave.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EnterEqualTab);
            // 
            // txtClave
            // 
            this.txtClave.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtClave.Location = new System.Drawing.Point(115, 54);
            this.txtClave.MaxLength = 20;
            this.txtClave.Name = "txtClave";
            this.txtClave.PasswordChar = '*';
            this.txtClave.Size = new System.Drawing.Size(159, 20);
            this.txtClave.TabIndex = 1;
            this.txtClave.TextChanged += new System.EventHandler(this.txtClave_TextChanged);
            this.txtClave.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EnterEqualTab);
            // 
            // txtAnterior
            // 
            this.txtAnterior.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAnterior.Location = new System.Drawing.Point(115, 28);
            this.txtAnterior.MaxLength = 20;
            this.txtAnterior.Name = "txtAnterior";
            this.txtAnterior.PasswordChar = '*';
            this.txtAnterior.Size = new System.Drawing.Size(159, 20);
            this.txtAnterior.TabIndex = 0;
            this.txtAnterior.TextChanged += new System.EventHandler(this.txtAnterior_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Contraseña anterior:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // frmCambioContrasena
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 167);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAnterior);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRepetirClave);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.lblClave);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.txtClave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCambioContrasena";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cambio de Contraseña";
            this.Load += new System.EventHandler(this.frmCambioContrasena_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnSalir;
        private System.Windows.Forms.Label lblClave;
        private DevExpress.XtraEditors.SimpleButton btnAceptar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRepetirClave;
        private System.Windows.Forms.TextBox txtClave;
        private System.Windows.Forms.TextBox txtAnterior;
        private System.Windows.Forms.Label label2;
    }
}