namespace REH_Presentacion.Ventas.Parametrizadores
{
    partial class frmPaPresupuestoVentas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPaPresupuestoVentas));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnEliminarSeleccionados = new DevExpress.XtraEditors.SimpleButton();
            this.btnSeleccionarTodos = new DevExpress.XtraEditors.SimpleButton();
            this.gcParametrizaciones = new DevExpress.XtraGrid.GridControl();
            this.dgvParametrizaciones = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtMes = new DevExpress.XtraEditors.TextEdit();
            this.label17 = new System.Windows.Forms.Label();
            this.txtTotal = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTipoProducto = new DevExpress.XtraEditors.TextEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.btnAddFila = new DevExpress.XtraEditors.SimpleButton();
            this.txtMedidaConversion = new DevExpress.XtraEditors.TextEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.txtValor = new DevExpress.XtraEditors.TextEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPrecio = new DevExpress.XtraEditors.TextEdit();
            this.label7 = new System.Windows.Forms.Label();
            this.txtUnidades = new DevExpress.XtraEditors.TextEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbZona = new DevExpress.XtraEditors.LookUpEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.txtAnio = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbProducto = new DevExpress.XtraEditors.LookUpEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcParametrizaciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParametrizaciones)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTipoProducto.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMedidaConversion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrecio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnidades.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbZona.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProducto.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btnEliminarSeleccionados);
            this.groupBox1.Controls.Add(this.btnSeleccionarTodos);
            this.groupBox1.Controls.Add(this.gcParametrizaciones);
            this.groupBox1.Location = new System.Drawing.Point(12, 177);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(765, 296);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parametrizaciones";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(125, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(370, 13);
            this.label5.TabIndex = 50;
            this.label5.Text = "Si se desea seleccionar por mes, Ingrese el mes, caso contrario tomara todos";
            // 
            // btnEliminarSeleccionados
            // 
            this.btnEliminarSeleccionados.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEliminarSeleccionados.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnEliminarSeleccionados.ImageOptions.Image")));
            this.btnEliminarSeleccionados.Location = new System.Drawing.Point(616, 16);
            this.btnEliminarSeleccionados.Name = "btnEliminarSeleccionados";
            this.btnEliminarSeleccionados.Size = new System.Drawing.Size(143, 23);
            this.btnEliminarSeleccionados.TabIndex = 49;
            this.btnEliminarSeleccionados.Text = "Eliminar Seleccionados";
            this.btnEliminarSeleccionados.Click += new System.EventHandler(this.btnEliminarSeleccionados_Click);
            // 
            // btnSeleccionarTodos
            // 
            this.btnSeleccionarTodos.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSeleccionarTodos.ImageOptions.Image")));
            this.btnSeleccionarTodos.Location = new System.Drawing.Point(9, 19);
            this.btnSeleccionarTodos.Name = "btnSeleccionarTodos";
            this.btnSeleccionarTodos.Size = new System.Drawing.Size(113, 23);
            this.btnSeleccionarTodos.TabIndex = 48;
            this.btnSeleccionarTodos.Text = "Seleccionar todos";
            this.btnSeleccionarTodos.Click += new System.EventHandler(this.btnSeleccionarTodos_Click);
            // 
            // gcParametrizaciones
            // 
            this.gcParametrizaciones.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcParametrizaciones.Location = new System.Drawing.Point(9, 48);
            this.gcParametrizaciones.MainView = this.dgvParametrizaciones;
            this.gcParametrizaciones.Name = "gcParametrizaciones";
            this.gcParametrizaciones.Size = new System.Drawing.Size(750, 243);
            this.gcParametrizaciones.TabIndex = 0;
            this.gcParametrizaciones.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvParametrizaciones});
            // 
            // dgvParametrizaciones
            // 
            this.dgvParametrizaciones.GridControl = this.gcParametrizaciones;
            this.dgvParametrizaciones.Name = "dgvParametrizaciones";
            this.dgvParametrizaciones.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvParametrizaciones.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.dgvParametrizaciones.OptionsBehavior.Editable = false;
            this.dgvParametrizaciones.OptionsCustomization.AllowColumnMoving = false;
            this.dgvParametrizaciones.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.dgvParametrizaciones.OptionsView.ShowAutoFilterRow = true;
            this.dgvParametrizaciones.OptionsView.ShowFooter = true;
            this.dgvParametrizaciones.OptionsView.ShowGroupPanel = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtMes);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.txtTotal);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtTipoProducto);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.btnAddFila);
            this.groupBox2.Controls.Add(this.txtMedidaConversion);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtValor);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtPrecio);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtUnidades);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cmbZona);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtAnio);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cmbProducto);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(12, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(765, 141);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            // 
            // txtMes
            // 
            this.txtMes.Location = new System.Drawing.Point(400, 13);
            this.txtMes.Name = "txtMes";
            this.txtMes.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMes.Properties.MaxLength = 2;
            this.txtMes.Size = new System.Drawing.Size(55, 20);
            this.txtMes.TabIndex = 2;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(370, 16);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(30, 13);
            this.label17.TabIndex = 67;
            this.label17.Text = "Mes:";
            // 
            // txtTotal
            // 
            this.txtTotal.Enabled = false;
            this.txtTotal.Location = new System.Drawing.Point(528, 107);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTotal.Properties.MaxLength = 18;
            this.txtTotal.Size = new System.Drawing.Size(84, 20);
            this.txtTotal.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(525, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 51;
            this.label1.Text = "Total:";
            // 
            // txtTipoProducto
            // 
            this.txtTipoProducto.Location = new System.Drawing.Point(109, 68);
            this.txtTipoProducto.Name = "txtTipoProducto";
            this.txtTipoProducto.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtTipoProducto.Properties.MaxLength = 100;
            this.txtTipoProducto.Size = new System.Drawing.Size(639, 20);
            this.txtTipoProducto.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 68);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 13);
            this.label10.TabIndex = 48;
            this.label10.Text = "Tipo de Producto:";
            // 
            // btnAddFila
            // 
            this.btnAddFila.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddFila.ImageOptions.Image")));
            this.btnAddFila.Location = new System.Drawing.Point(666, 104);
            this.btnAddFila.Name = "btnAddFila";
            this.btnAddFila.Size = new System.Drawing.Size(63, 23);
            this.btnAddFila.TabIndex = 11;
            this.btnAddFila.Text = "Añadir";
            this.btnAddFila.Click += new System.EventHandler(this.btnAddFila_Click);
            // 
            // txtMedidaConversion
            // 
            this.txtMedidaConversion.Location = new System.Drawing.Point(396, 107);
            this.txtMedidaConversion.Name = "txtMedidaConversion";
            this.txtMedidaConversion.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMedidaConversion.Properties.MaxLength = 18;
            this.txtMedidaConversion.Size = new System.Drawing.Size(77, 20);
            this.txtMedidaConversion.TabIndex = 9;
            this.txtMedidaConversion.EditValueChanged += new System.EventHandler(this.txtUnidades_EditValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(393, 91);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 13);
            this.label9.TabIndex = 46;
            this.label9.Text = "M. Conversión:";
            // 
            // txtValor
            // 
            this.txtValor.Enabled = false;
            this.txtValor.Location = new System.Drawing.Point(264, 107);
            this.txtValor.Name = "txtValor";
            this.txtValor.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtValor.Properties.MaxLength = 18;
            this.txtValor.Size = new System.Drawing.Size(77, 20);
            this.txtValor.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(261, 91);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 44;
            this.label8.Text = "Valor:";
            // 
            // txtPrecio
            // 
            this.txtPrecio.Location = new System.Drawing.Point(138, 107);
            this.txtPrecio.Name = "txtPrecio";
            this.txtPrecio.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPrecio.Properties.Mask.EditMask = "n";
            this.txtPrecio.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtPrecio.Properties.MaxLength = 18;
            this.txtPrecio.Size = new System.Drawing.Size(74, 20);
            this.txtPrecio.TabIndex = 7;
            this.txtPrecio.EditValueChanged += new System.EventHandler(this.txtUnidades_EditValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(135, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 42;
            this.label7.Text = "Precio:";
            // 
            // txtUnidades
            // 
            this.txtUnidades.Location = new System.Drawing.Point(9, 107);
            this.txtUnidades.Name = "txtUnidades";
            this.txtUnidades.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtUnidades.Properties.MaxLength = 18;
            this.txtUnidades.Size = new System.Drawing.Size(68, 20);
            this.txtUnidades.TabIndex = 6;
            this.txtUnidades.EditValueChanged += new System.EventHandler(this.txtUnidades_EditValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 40;
            this.label6.Text = "Unidades:";
            // 
            // cmbZona
            // 
            this.cmbZona.Location = new System.Drawing.Point(109, 39);
            this.cmbZona.Name = "cmbZona";
            this.cmbZona.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbZona.Properties.NullText = "";
            this.cmbZona.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbZona.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbZona.Properties.PopupWidth = 10;
            this.cmbZona.Properties.ShowFooter = false;
            this.cmbZona.Properties.ShowHeader = false;
            this.cmbZona.Size = new System.Drawing.Size(232, 20);
            this.cmbZona.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "Zona:";
            // 
            // txtAnio
            // 
            this.txtAnio.Location = new System.Drawing.Point(109, 13);
            this.txtAnio.Name = "txtAnio";
            this.txtAnio.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAnio.Properties.MaxLength = 4;
            this.txtAnio.Size = new System.Drawing.Size(66, 20);
            this.txtAnio.TabIndex = 1;
            this.txtAnio.Leave += new System.EventHandler(this.txtAnio_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Año:";
            // 
            // cmbProducto
            // 
            this.cmbProducto.Location = new System.Drawing.Point(400, 39);
            this.cmbProducto.Name = "cmbProducto";
            this.cmbProducto.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbProducto.Properties.NullText = "";
            this.cmbProducto.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbProducto.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbProducto.Properties.PopupWidth = 10;
            this.cmbProducto.Properties.ShowFooter = false;
            this.cmbProducto.Properties.ShowHeader = false;
            this.cmbProducto.Size = new System.Drawing.Size(348, 20);
            this.cmbProducto.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(347, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Producto:";
            // 
            // frmPaPresupuestoVentas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 480);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmPaPresupuestoVentas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Presupuesto Ventas";
            this.Load += new System.EventHandler(this.frmPaPresupuestoVentasRebate_Load);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcParametrizaciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParametrizaciones)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTipoProducto.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMedidaConversion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPrecio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnidades.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbZona.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAnio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProducto.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraGrid.GridControl gcParametrizaciones;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvParametrizaciones;
        private System.Windows.Forms.GroupBox groupBox2;
        public DevExpress.XtraEditors.LookUpEdit cmbProducto;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit txtAnio;
        private System.Windows.Forms.Label label2;
        public DevExpress.XtraEditors.LookUpEdit cmbZona;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.SimpleButton btnAddFila;
        private DevExpress.XtraEditors.TextEdit txtTipoProducto;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.TextEdit txtMes;
        private System.Windows.Forms.Label label17;
        private DevExpress.XtraEditors.TextEdit txtTotal;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtMedidaConversion;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.TextEdit txtValor;
        private System.Windows.Forms.Label label8;
        private DevExpress.XtraEditors.TextEdit txtPrecio;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.TextEdit txtUnidades;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.SimpleButton btnEliminarSeleccionados;
        private DevExpress.XtraEditors.SimpleButton btnSeleccionarTodos;
    }
}