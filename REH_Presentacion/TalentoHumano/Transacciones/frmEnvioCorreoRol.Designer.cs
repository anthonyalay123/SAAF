namespace EmpleadoNomina
{
    partial class frmTrEnvioCorreoRol
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEnvioCorreoRol));
            this.cmbTipoRol = new System.Windows.Forms.ComboBox();
            this.label37 = new System.Windows.Forms.Label();
            this.cmbFechaRol = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPreliminar = new System.Windows.Forms.Button();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.rdbEmpleado = new System.Windows.Forms.RadioButton();
            this.rdbTodos = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBitacora = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbTipoRol
            // 
            this.cmbTipoRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoRol.FormattingEnabled = true;
            this.cmbTipoRol.Location = new System.Drawing.Point(84, 19);
            this.cmbTipoRol.Name = "cmbTipoRol";
            this.cmbTipoRol.Size = new System.Drawing.Size(171, 21);
            this.cmbTipoRol.TabIndex = 157;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(12, 22);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(65, 13);
            this.label37.TabIndex = 158;
            this.label37.Text = "Tipo de Rol:";
            // 
            // cmbFechaRol
            // 
            this.cmbFechaRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFechaRol.FormattingEnabled = true;
            this.cmbFechaRol.Location = new System.Drawing.Point(84, 51);
            this.cmbFechaRol.Name = "cmbFechaRol";
            this.cmbFechaRol.Size = new System.Drawing.Size(171, 21);
            this.cmbFechaRol.TabIndex = 159;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 160;
            this.label1.Text = "Fecha Rol:";
            // 
            // btnPreliminar
            // 
            this.btnPreliminar.Image = ((System.Drawing.Image)(resources.GetObject("btnPreliminar.Image")));
            this.btnPreliminar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPreliminar.Location = new System.Drawing.Point(273, 13);
            this.btnPreliminar.Name = "btnPreliminar";
            this.btnPreliminar.Size = new System.Drawing.Size(74, 31);
            this.btnPreliminar.TabIndex = 162;
            this.btnPreliminar.Text = "Preliminar";
            this.btnPreliminar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPreliminar.UseVisualStyleBackColor = true;
            this.btnPreliminar.Click += new System.EventHandler(this.btnPreliminar_Click);
            // 
            // btnNuevo
            // 
            this.btnNuevo.Image = ((System.Drawing.Image)(resources.GetObject("btnNuevo.Image")));
            this.btnNuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNuevo.Location = new System.Drawing.Point(146, 91);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(96, 62);
            this.btnNuevo.TabIndex = 163;
            this.btnNuevo.Text = "Enviar Rol";
            this.btnNuevo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // rdbEmpleado
            // 
            this.rdbEmpleado.AutoSize = true;
            this.rdbEmpleado.Location = new System.Drawing.Point(6, 20);
            this.rdbEmpleado.Name = "rdbEmpleado";
            this.rdbEmpleado.Size = new System.Drawing.Size(90, 17);
            this.rdbEmpleado.TabIndex = 164;
            this.rdbEmpleado.TabStop = true;
            this.rdbEmpleado.Text = "Seleccionado";
            this.rdbEmpleado.UseVisualStyleBackColor = true;
            // 
            // rdbTodos
            // 
            this.rdbTodos.AutoSize = true;
            this.rdbTodos.Location = new System.Drawing.Point(6, 43);
            this.rdbTodos.Name = "rdbTodos";
            this.rdbTodos.Size = new System.Drawing.Size(55, 17);
            this.rdbTodos.TabIndex = 165;
            this.rdbTodos.TabStop = true;
            this.rdbTodos.Text = "Todos";
            this.rdbTodos.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbEmpleado);
            this.groupBox1.Controls.Add(this.rdbTodos);
            this.groupBox1.Location = new System.Drawing.Point(12, 83);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(117, 70);
            this.groupBox1.TabIndex = 166;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Empleados";
            // 
            // btnBitacora
            // 
            this.btnBitacora.Image = ((System.Drawing.Image)(resources.GetObject("btnBitacora.Image")));
            this.btnBitacora.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBitacora.Location = new System.Drawing.Point(273, 47);
            this.btnBitacora.Name = "btnBitacora";
            this.btnBitacora.Size = new System.Drawing.Size(74, 31);
            this.btnBitacora.TabIndex = 167;
            this.btnBitacora.Text = "Bitácora";
            this.btnBitacora.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnBitacora.UseVisualStyleBackColor = true;
            this.btnBitacora.Click += new System.EventHandler(this.btnBitacora_Click);
            // 
            // frmEnvioCorreoRol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 168);
            this.Controls.Add(this.btnBitacora);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnNuevo);
            this.Controls.Add(this.btnPreliminar);
            this.Controls.Add(this.cmbFechaRol);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbTipoRol);
            this.Controls.Add(this.label37);
            this.Name = "frmEnvioCorreoRol";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Envío por Correo de Rol de Pago";
            this.Load += new System.EventHandler(this.frmEnvioCorreoRol_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbTipoRol;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.ComboBox cmbFechaRol;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button btnPreliminar;
        public System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.RadioButton rdbEmpleado;
        private System.Windows.Forms.RadioButton rdbTodos;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Button btnBitacora;
    }
}