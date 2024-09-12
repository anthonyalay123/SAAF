namespace REH_Presentacion.Formularios
{
    partial class frmBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBase));
            this.tabContenedor = new System.Windows.Forms.TabControl();
            this.tabPrincipal = new System.Windows.Forms.TabPage();
            this.pnlEstado = new System.Windows.Forms.Panel();
            this.lblEstado = new System.Windows.Forms.Label();
            this.rdoInactivo = new System.Windows.Forms.RadioButton();
            this.rdoActivo = new System.Windows.Forms.RadioButton();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.txtCodigo = new System.Windows.Forms.TextBox();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.tabAuditoria = new System.Windows.Forms.TabPage();
            this.txtTerminalModificacion = new System.Windows.Forms.TextBox();
            this.txtUsuarioModificacion = new System.Windows.Forms.TextBox();
            this.txtFechaHoraModificacion = new System.Windows.Forms.TextBox();
            this.txtTerminalIngreso = new System.Windows.Forms.TextBox();
            this.txtUsuarioIngreso = new System.Windows.Forms.TextBox();
            this.txtFechaHoraIngreso = new System.Windows.Forms.TextBox();
            this.lblTerminalModificacion = new System.Windows.Forms.Label();
            this.lblUsuarioModificacion = new System.Windows.Forms.Label();
            this.lblFechaHoraModificacion = new System.Windows.Forms.Label();
            this.lblTerminalIngreso = new System.Windows.Forms.Label();
            this.lblUsuarioIngreso = new System.Windows.Forms.Label();
            this.lblFechaHoraIngreso = new System.Windows.Forms.Label();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnGrabar = new System.Windows.Forms.Button();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.tabContenedor.SuspendLayout();
            this.tabPrincipal.SuspendLayout();
            this.pnlEstado.SuspendLayout();
            this.tabAuditoria.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabContenedor
            // 
            this.tabContenedor.Controls.Add(this.tabPrincipal);
            this.tabContenedor.Controls.Add(this.tabAuditoria);
            this.tabContenedor.Location = new System.Drawing.Point(12, 1);
            this.tabContenedor.Name = "tabContenedor";
            this.tabContenedor.SelectedIndex = 0;
            this.tabContenedor.Size = new System.Drawing.Size(387, 232);
            this.tabContenedor.TabIndex = 0;
            // 
            // tabPrincipal
            // 
            this.tabPrincipal.Controls.Add(this.pnlEstado);
            this.tabPrincipal.Controls.Add(this.txtDescripcion);
            this.tabPrincipal.Controls.Add(this.txtCodigo);
            this.tabPrincipal.Controls.Add(this.lblDescripcion);
            this.tabPrincipal.Controls.Add(this.lblCodigo);
            this.tabPrincipal.Location = new System.Drawing.Point(4, 22);
            this.tabPrincipal.Name = "tabPrincipal";
            this.tabPrincipal.Padding = new System.Windows.Forms.Padding(3);
            this.tabPrincipal.Size = new System.Drawing.Size(379, 206);
            this.tabPrincipal.TabIndex = 0;
            this.tabPrincipal.Text = "Principal";
            this.tabPrincipal.UseVisualStyleBackColor = true;
            // 
            // pnlEstado
            // 
            this.pnlEstado.Controls.Add(this.lblEstado);
            this.pnlEstado.Controls.Add(this.rdoInactivo);
            this.pnlEstado.Controls.Add(this.rdoActivo);
            this.pnlEstado.Location = new System.Drawing.Point(54, 116);
            this.pnlEstado.Name = "pnlEstado";
            this.pnlEstado.Size = new System.Drawing.Size(281, 36);
            this.pnlEstado.TabIndex = 4;
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(23, 13);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(43, 13);
            this.lblEstado.TabIndex = 0;
            this.lblEstado.Text = "Estado:";
            // 
            // rdoInactivo
            // 
            this.rdoInactivo.AutoSize = true;
            this.rdoInactivo.Location = new System.Drawing.Point(199, 11);
            this.rdoInactivo.Name = "rdoInactivo";
            this.rdoInactivo.Size = new System.Drawing.Size(63, 17);
            this.rdoInactivo.TabIndex = 2;
            this.rdoInactivo.TabStop = true;
            this.rdoInactivo.Text = "Inactivo";
            this.rdoInactivo.UseVisualStyleBackColor = true;
            // 
            // rdoActivo
            // 
            this.rdoActivo.AutoSize = true;
            this.rdoActivo.Location = new System.Drawing.Point(91, 11);
            this.rdoActivo.Name = "rdoActivo";
            this.rdoActivo.Size = new System.Drawing.Size(55, 17);
            this.rdoActivo.TabIndex = 1;
            this.rdoActivo.TabStop = true;
            this.rdoActivo.Text = "Activo";
            this.rdoActivo.UseVisualStyleBackColor = true;
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Location = new System.Drawing.Point(117, 72);
            this.txtDescripcion.MaxLength = 50;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(192, 20);
            this.txtDescripcion.TabIndex = 3;
            // 
            // txtCodigo
            // 
            this.txtCodigo.Enabled = false;
            this.txtCodigo.Location = new System.Drawing.Point(117, 38);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Size = new System.Drawing.Size(61, 20);
            this.txtCodigo.TabIndex = 1;
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Location = new System.Drawing.Point(32, 72);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Size = new System.Drawing.Size(66, 13);
            this.lblDescripcion.TabIndex = 2;
            this.lblDescripcion.Text = "Descripción:";
            // 
            // lblCodigo
            // 
            this.lblCodigo.AutoSize = true;
            this.lblCodigo.Location = new System.Drawing.Point(55, 41);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(43, 13);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código:";
            // 
            // tabAuditoria
            // 
            this.tabAuditoria.Controls.Add(this.txtTerminalModificacion);
            this.tabAuditoria.Controls.Add(this.txtUsuarioModificacion);
            this.tabAuditoria.Controls.Add(this.txtFechaHoraModificacion);
            this.tabAuditoria.Controls.Add(this.txtTerminalIngreso);
            this.tabAuditoria.Controls.Add(this.txtUsuarioIngreso);
            this.tabAuditoria.Controls.Add(this.txtFechaHoraIngreso);
            this.tabAuditoria.Controls.Add(this.lblTerminalModificacion);
            this.tabAuditoria.Controls.Add(this.lblUsuarioModificacion);
            this.tabAuditoria.Controls.Add(this.lblFechaHoraModificacion);
            this.tabAuditoria.Controls.Add(this.lblTerminalIngreso);
            this.tabAuditoria.Controls.Add(this.lblUsuarioIngreso);
            this.tabAuditoria.Controls.Add(this.lblFechaHoraIngreso);
            this.tabAuditoria.Location = new System.Drawing.Point(4, 22);
            this.tabAuditoria.Name = "tabAuditoria";
            this.tabAuditoria.Padding = new System.Windows.Forms.Padding(3);
            this.tabAuditoria.Size = new System.Drawing.Size(379, 206);
            this.tabAuditoria.TabIndex = 1;
            this.tabAuditoria.Text = "Auditoría";
            this.tabAuditoria.UseVisualStyleBackColor = true;
            // 
            // txtTerminalModificacion
            // 
            this.txtTerminalModificacion.Location = new System.Drawing.Point(153, 158);
            this.txtTerminalModificacion.Name = "txtTerminalModificacion";
            this.txtTerminalModificacion.ReadOnly = true;
            this.txtTerminalModificacion.Size = new System.Drawing.Size(195, 20);
            this.txtTerminalModificacion.TabIndex = 11;
            // 
            // txtUsuarioModificacion
            // 
            this.txtUsuarioModificacion.Location = new System.Drawing.Point(153, 130);
            this.txtUsuarioModificacion.Name = "txtUsuarioModificacion";
            this.txtUsuarioModificacion.ReadOnly = true;
            this.txtUsuarioModificacion.Size = new System.Drawing.Size(195, 20);
            this.txtUsuarioModificacion.TabIndex = 10;
            // 
            // txtFechaHoraModificacion
            // 
            this.txtFechaHoraModificacion.Location = new System.Drawing.Point(153, 104);
            this.txtFechaHoraModificacion.Name = "txtFechaHoraModificacion";
            this.txtFechaHoraModificacion.ReadOnly = true;
            this.txtFechaHoraModificacion.Size = new System.Drawing.Size(195, 20);
            this.txtFechaHoraModificacion.TabIndex = 9;
            // 
            // txtTerminalIngreso
            // 
            this.txtTerminalIngreso.Location = new System.Drawing.Point(153, 80);
            this.txtTerminalIngreso.Name = "txtTerminalIngreso";
            this.txtTerminalIngreso.ReadOnly = true;
            this.txtTerminalIngreso.Size = new System.Drawing.Size(195, 20);
            this.txtTerminalIngreso.TabIndex = 8;
            // 
            // txtUsuarioIngreso
            // 
            this.txtUsuarioIngreso.Location = new System.Drawing.Point(153, 48);
            this.txtUsuarioIngreso.Name = "txtUsuarioIngreso";
            this.txtUsuarioIngreso.ReadOnly = true;
            this.txtUsuarioIngreso.Size = new System.Drawing.Size(195, 20);
            this.txtUsuarioIngreso.TabIndex = 7;
            // 
            // txtFechaHoraIngreso
            // 
            this.txtFechaHoraIngreso.Location = new System.Drawing.Point(153, 22);
            this.txtFechaHoraIngreso.Name = "txtFechaHoraIngreso";
            this.txtFechaHoraIngreso.ReadOnly = true;
            this.txtFechaHoraIngreso.Size = new System.Drawing.Size(195, 20);
            this.txtFechaHoraIngreso.TabIndex = 6;
            // 
            // lblTerminalModificacion
            // 
            this.lblTerminalModificacion.AutoSize = true;
            this.lblTerminalModificacion.Location = new System.Drawing.Point(34, 165);
            this.lblTerminalModificacion.Name = "lblTerminalModificacion";
            this.lblTerminalModificacion.Size = new System.Drawing.Size(113, 13);
            this.lblTerminalModificacion.TabIndex = 5;
            this.lblTerminalModificacion.Text = "Terminal Modificación:";
            // 
            // lblUsuarioModificacion
            // 
            this.lblUsuarioModificacion.AutoSize = true;
            this.lblUsuarioModificacion.Location = new System.Drawing.Point(41, 137);
            this.lblUsuarioModificacion.Name = "lblUsuarioModificacion";
            this.lblUsuarioModificacion.Size = new System.Drawing.Size(109, 13);
            this.lblUsuarioModificacion.TabIndex = 4;
            this.lblUsuarioModificacion.Text = "Usuario Modificación:";
            // 
            // lblFechaHoraModificacion
            // 
            this.lblFechaHoraModificacion.AutoSize = true;
            this.lblFechaHoraModificacion.Location = new System.Drawing.Point(19, 107);
            this.lblFechaHoraModificacion.Name = "lblFechaHoraModificacion";
            this.lblFechaHoraModificacion.Size = new System.Drawing.Size(131, 13);
            this.lblFechaHoraModificacion.TabIndex = 3;
            this.lblFechaHoraModificacion.Text = "Fecha/Hora Modificación:";
            // 
            // lblTerminalIngreso
            // 
            this.lblTerminalIngreso.AutoSize = true;
            this.lblTerminalIngreso.Location = new System.Drawing.Point(59, 80);
            this.lblTerminalIngreso.Name = "lblTerminalIngreso";
            this.lblTerminalIngreso.Size = new System.Drawing.Size(88, 13);
            this.lblTerminalIngreso.TabIndex = 2;
            this.lblTerminalIngreso.Text = "Terminal Ingreso:";
            // 
            // lblUsuarioIngreso
            // 
            this.lblUsuarioIngreso.AutoSize = true;
            this.lblUsuarioIngreso.Location = new System.Drawing.Point(63, 51);
            this.lblUsuarioIngreso.Name = "lblUsuarioIngreso";
            this.lblUsuarioIngreso.Size = new System.Drawing.Size(84, 13);
            this.lblUsuarioIngreso.TabIndex = 1;
            this.lblUsuarioIngreso.Text = "Usuario Ingreso:";
            // 
            // lblFechaHoraIngreso
            // 
            this.lblFechaHoraIngreso.AutoSize = true;
            this.lblFechaHoraIngreso.Location = new System.Drawing.Point(41, 22);
            this.lblFechaHoraIngreso.Name = "lblFechaHoraIngreso";
            this.lblFechaHoraIngreso.Size = new System.Drawing.Size(106, 13);
            this.lblFechaHoraIngreso.TabIndex = 0;
            this.lblFechaHoraIngreso.Text = "Fecha/Hora Ingreso:";
            // 
            // btnNuevo
            // 
            this.btnNuevo.Image = ((System.Drawing.Image)(resources.GetObject("btnNuevo.Image")));
            this.btnNuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNuevo.Location = new System.Drawing.Point(405, 23);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(68, 32);
            this.btnNuevo.TabIndex = 12;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // btnGrabar
            // 
            this.btnGrabar.Image = ((System.Drawing.Image)(resources.GetObject("btnGrabar.Image")));
            this.btnGrabar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGrabar.Location = new System.Drawing.Point(405, 99);
            this.btnGrabar.Name = "btnGrabar";
            this.btnGrabar.Size = new System.Drawing.Size(68, 32);
            this.btnGrabar.TabIndex = 14;
            this.btnGrabar.Text = "Grabar";
            this.btnGrabar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGrabar.UseVisualStyleBackColor = true;
            // 
            // btnBuscar
            // 
            this.btnBuscar.Image = ((System.Drawing.Image)(resources.GetObject("btnBuscar.Image")));
            this.btnBuscar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuscar.Location = new System.Drawing.Point(405, 61);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(68, 32);
            this.btnBuscar.TabIndex = 13;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBuscar.UseVisualStyleBackColor = true;
            // 
            // frmBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 234);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.btnGrabar);
            this.Controls.Add(this.btnNuevo);
            this.Controls.Add(this.tabContenedor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmBase";
            this.Load += new System.EventHandler(this.frmBase_Load);
            this.tabContenedor.ResumeLayout(false);
            this.tabPrincipal.ResumeLayout(false);
            this.tabPrincipal.PerformLayout();
            this.pnlEstado.ResumeLayout(false);
            this.pnlEstado.PerformLayout();
            this.tabAuditoria.ResumeLayout(false);
            this.tabAuditoria.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.TabControl tabContenedor;
        protected System.Windows.Forms.TabPage tabPrincipal;
        protected System.Windows.Forms.Panel pnlEstado;
        protected System.Windows.Forms.Label lblEstado;
        protected System.Windows.Forms.RadioButton rdoInactivo;
        protected System.Windows.Forms.RadioButton rdoActivo;
        protected System.Windows.Forms.TextBox txtDescripcion;
        protected System.Windows.Forms.TextBox txtCodigo;
        protected System.Windows.Forms.Label lblDescripcion;
        protected System.Windows.Forms.Label lblCodigo;
        protected System.Windows.Forms.TabPage tabAuditoria;
        protected System.Windows.Forms.TextBox txtTerminalModificacion;
        protected System.Windows.Forms.TextBox txtUsuarioModificacion;
        protected System.Windows.Forms.TextBox txtFechaHoraModificacion;
        protected System.Windows.Forms.TextBox txtTerminalIngreso;
        protected System.Windows.Forms.TextBox txtUsuarioIngreso;
        protected System.Windows.Forms.TextBox txtFechaHoraIngreso;
        private System.Windows.Forms.Label lblTerminalModificacion;
        private System.Windows.Forms.Label lblUsuarioModificacion;
        private System.Windows.Forms.Label lblFechaHoraModificacion;
        private System.Windows.Forms.Label lblTerminalIngreso;
        private System.Windows.Forms.Label lblUsuarioIngreso;
        private System.Windows.Forms.Label lblFechaHoraIngreso;
        public System.Windows.Forms.Button btnNuevo;
        public System.Windows.Forms.Button btnGrabar;
        public System.Windows.Forms.Button btnBuscar;
    }
}