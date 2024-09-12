namespace REH_Presentacion.TalentoHumano.Transacciones
{
    partial class frmTrSolicitudPrestamo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTrSolicitudPrestamo));
            this.group = new System.Windows.Forms.GroupBox();
            this.lblTipo = new System.Windows.Forms.Label();
            this.cmbTipo = new DevExpress.XtraEditors.LookUpEdit();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnOrdenar = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddFila = new DevExpress.XtraEditors.SimpleButton();
            this.gcDatos = new DevExpress.XtraGrid.GridControl();
            this.dgvDatos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtCapacidadDeuda = new DevExpress.XtraEditors.TextEdit();
            this.label13 = new System.Windows.Forms.Label();
            this.txtEgresos = new DevExpress.XtraEditors.TextEdit();
            this.label12 = new System.Windows.Forms.Label();
            this.txtIngresos = new DevExpress.XtraEditors.TextEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdbCuotaFija = new System.Windows.Forms.RadioButton();
            this.rdbPlazo = new System.Windows.Forms.RadioButton();
            this.label16 = new System.Windows.Forms.Label();
            this.txtValorCuotaFija = new DevExpress.XtraEditors.TextEdit();
            this.label15 = new System.Windows.Forms.Label();
            this.dtpFechaInicioPago = new System.Windows.Forms.DateTimePicker();
            this.btnCalcular = new DevExpress.XtraEditors.SimpleButton();
            this.txtPlazo = new DevExpress.XtraEditors.TextEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.txtObservacion = new DevExpress.XtraEditors.TextEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.txtValor = new DevExpress.XtraEditors.TextEdit();
            this.lblValorPrestamo = new System.Windows.Forms.Label();
            this.lblMotivo = new System.Windows.Forms.Label();
            this.cmbMotivo = new DevExpress.XtraEditors.LookUpEdit();
            this.lblEstado = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpFechaSolicitud = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCiudad = new DevExpress.XtraEditors.TextEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpFechaIngreso = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbEmpleado = new DevExpress.XtraEditors.LookUpEdit();
            this.txtNo = new DevExpress.XtraEditors.TextEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.group.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipo.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCapacidadDeuda.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEgresos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIngresos.Properties)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorCuotaFija.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlazo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtObservacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMotivo.Properties)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCiudad.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmpleado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // group
            // 
            this.group.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.group.Controls.Add(this.lblTipo);
            this.group.Controls.Add(this.cmbTipo);
            this.group.Controls.Add(this.groupBox2);
            this.group.Controls.Add(this.groupBox4);
            this.group.Controls.Add(this.groupBox3);
            this.group.Controls.Add(this.lblEstado);
            this.group.Controls.Add(this.label11);
            this.group.Controls.Add(this.label3);
            this.group.Controls.Add(this.dtpFechaSolicitud);
            this.group.Controls.Add(this.groupBox1);
            this.group.Controls.Add(this.txtNo);
            this.group.Controls.Add(this.label8);
            this.group.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.group.Location = new System.Drawing.Point(12, 41);
            this.group.Name = "group";
            this.group.Size = new System.Drawing.Size(911, 565);
            this.group.TabIndex = 18;
            this.group.TabStop = false;
            this.group.Text = "Solicitud de Préstamo - Anticipo - Descuento";
            // 
            // lblTipo
            // 
            this.lblTipo.AutoSize = true;
            this.lblTipo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipo.Location = new System.Drawing.Point(118, 24);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(31, 13);
            this.lblTipo.TabIndex = 52;
            this.lblTipo.Text = "Tipo:";
            // 
            // cmbTipo
            // 
            this.cmbTipo.Location = new System.Drawing.Point(162, 21);
            this.cmbTipo.Name = "cmbTipo";
            this.cmbTipo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbTipo.Properties.NullText = "";
            this.cmbTipo.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbTipo.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbTipo.Properties.PopupWidth = 10;
            this.cmbTipo.Properties.ShowFooter = false;
            this.cmbTipo.Properties.ShowHeader = false;
            this.cmbTipo.Size = new System.Drawing.Size(344, 20);
            this.cmbTipo.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnOrdenar);
            this.groupBox2.Controls.Add(this.btnAddFila);
            this.groupBox2.Controls.Add(this.gcDatos);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(6, 238);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(899, 321);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tabla de Amortización";
            // 
            // btnOrdenar
            // 
            this.btnOrdenar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOrdenar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnOrdenar.ImageOptions.Image")));
            this.btnOrdenar.Location = new System.Drawing.Point(718, 19);
            this.btnOrdenar.Name = "btnOrdenar";
            this.btnOrdenar.Size = new System.Drawing.Size(76, 23);
            this.btnOrdenar.TabIndex = 0;
            this.btnOrdenar.Text = "Ordenar";
            this.btnOrdenar.Click += new System.EventHandler(this.btnOrdenar_Click);
            // 
            // btnAddFila
            // 
            this.btnAddFila.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFila.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddFila.ImageOptions.Image")));
            this.btnAddFila.Location = new System.Drawing.Point(800, 19);
            this.btnAddFila.Name = "btnAddFila";
            this.btnAddFila.Size = new System.Drawing.Size(93, 23);
            this.btnAddFila.TabIndex = 1;
            this.btnAddFila.Text = "Agregar Fila";
            this.btnAddFila.Click += new System.EventHandler(this.btnAddFila_Click);
            // 
            // gcDatos
            // 
            this.gcDatos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcDatos.Location = new System.Drawing.Point(6, 45);
            this.gcDatos.MainView = this.dgvDatos;
            this.gcDatos.Name = "gcDatos";
            this.gcDatos.Size = new System.Drawing.Size(887, 270);
            this.gcDatos.TabIndex = 2;
            this.gcDatos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvDatos});
            // 
            // dgvDatos
            // 
            this.dgvDatos.GridControl = this.gcDatos;
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvDatos.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.dgvDatos.OptionsCustomization.AllowColumnMoving = false;
            this.dgvDatos.OptionsView.ColumnAutoWidth = false;
            this.dgvDatos.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.dgvDatos.OptionsView.ShowAutoFilterRow = true;
            this.dgvDatos.OptionsView.ShowFooter = true;
            this.dgvDatos.OptionsView.ShowGroupPanel = false;
            this.dgvDatos.CustomRowCellEditForEditing += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.dgvDatos_CustomRowCellEditForEditing);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.txtCapacidadDeuda);
            this.groupBox4.Controls.Add(this.label13);
            this.groupBox4.Controls.Add(this.txtEgresos);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.txtIngresos);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(6, 97);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(899, 46);
            this.groupBox4.TabIndex = 48;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Uso exclusivo de RR. HH.";
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(648, 12);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(227, 35);
            this.label14.TabIndex = 49;
            this.label14.Text = "(valores considerados de acuerdo al promedio de los últimos 3 meses)";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCapacidadDeuda
            // 
            this.txtCapacidadDeuda.Location = new System.Drawing.Point(538, 19);
            this.txtCapacidadDeuda.Name = "txtCapacidadDeuda";
            this.txtCapacidadDeuda.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCapacidadDeuda.Properties.MaxLength = 100;
            this.txtCapacidadDeuda.Properties.ReadOnly = true;
            this.txtCapacidadDeuda.Size = new System.Drawing.Size(85, 20);
            this.txtCapacidadDeuda.TabIndex = 2;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(379, 23);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(153, 13);
            this.label13.TabIndex = 47;
            this.label13.Text = "Capacidad de Endeudamiento:";
            // 
            // txtEgresos
            // 
            this.txtEgresos.Location = new System.Drawing.Point(248, 19);
            this.txtEgresos.Name = "txtEgresos";
            this.txtEgresos.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtEgresos.Properties.MaxLength = 100;
            this.txtEgresos.Properties.ReadOnly = true;
            this.txtEgresos.Size = new System.Drawing.Size(85, 20);
            this.txtEgresos.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(192, 22);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(48, 13);
            this.label12.TabIndex = 45;
            this.label12.Text = "Egresos:";
            // 
            // txtIngresos
            // 
            this.txtIngresos.Location = new System.Drawing.Point(69, 19);
            this.txtIngresos.Name = "txtIngresos";
            this.txtIngresos.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtIngresos.Properties.MaxLength = 100;
            this.txtIngresos.Properties.ReadOnly = true;
            this.txtIngresos.Size = new System.Drawing.Size(85, 20);
            this.txtIngresos.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(13, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(50, 13);
            this.label10.TabIndex = 43;
            this.label10.Text = "Ingresos:";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.rdbCuotaFija);
            this.groupBox3.Controls.Add(this.rdbPlazo);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.txtValorCuotaFija);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.dtpFechaInicioPago);
            this.groupBox3.Controls.Add(this.btnCalcular);
            this.groupBox3.Controls.Add(this.txtPlazo);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.txtObservacion);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtValor);
            this.groupBox3.Controls.Add(this.lblValorPrestamo);
            this.groupBox3.Controls.Add(this.lblMotivo);
            this.groupBox3.Controls.Add(this.cmbMotivo);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(6, 149);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(899, 83);
            this.groupBox3.TabIndex = 45;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Detalle de Solicitud";
            // 
            // rdbCuotaFija
            // 
            this.rdbCuotaFija.AutoSize = true;
            this.rdbCuotaFija.Location = new System.Drawing.Point(107, 53);
            this.rdbCuotaFija.Name = "rdbCuotaFija";
            this.rdbCuotaFija.Size = new System.Drawing.Size(105, 17);
            this.rdbCuotaFija.TabIndex = 3;
            this.rdbCuotaFija.TabStop = true;
            this.rdbCuotaFija.Text = "Por Cuota Fija";
            this.rdbCuotaFija.UseVisualStyleBackColor = true;
            this.rdbCuotaFija.CheckedChanged += new System.EventHandler(this.rdbPlazo_CheckedChanged);
            // 
            // rdbPlazo
            // 
            this.rdbPlazo.AutoSize = true;
            this.rdbPlazo.Location = new System.Drawing.Point(22, 53);
            this.rdbPlazo.Name = "rdbPlazo";
            this.rdbPlazo.Size = new System.Drawing.Size(79, 17);
            this.rdbPlazo.TabIndex = 3;
            this.rdbPlazo.TabStop = true;
            this.rdbPlazo.Text = "Por Plazo";
            this.rdbPlazo.UseVisualStyleBackColor = true;
            this.rdbPlazo.CheckedChanged += new System.EventHandler(this.rdbPlazo_CheckedChanged);
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(359, 49);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 30);
            this.label16.TabIndex = 6;
            this.label16.Text = "Valor de Cuota Fija:";
            // 
            // txtValorCuotaFija
            // 
            this.txtValorCuotaFija.Location = new System.Drawing.Point(424, 52);
            this.txtValorCuotaFija.Name = "txtValorCuotaFija";
            this.txtValorCuotaFija.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtValorCuotaFija.Properties.MaxLength = 100;
            this.txtValorCuotaFija.Size = new System.Drawing.Size(78, 20);
            this.txtValorCuotaFija.TabIndex = 7;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(524, 55);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(110, 13);
            this.label15.TabIndex = 8;
            this.label15.Text = "Fecha Inicio de pago:";
            // 
            // dtpFechaInicioPago
            // 
            this.dtpFechaInicioPago.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaInicioPago.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaInicioPago.Location = new System.Drawing.Point(640, 52);
            this.dtpFechaInicioPago.Name = "dtpFechaInicioPago";
            this.dtpFechaInicioPago.Size = new System.Drawing.Size(83, 20);
            this.dtpFechaInicioPago.TabIndex = 9;
            // 
            // btnCalcular
            // 
            this.btnCalcular.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCalcular.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCalcular.ImageOptions.Image")));
            this.btnCalcular.Location = new System.Drawing.Point(729, 51);
            this.btnCalcular.Name = "btnCalcular";
            this.btnCalcular.Size = new System.Drawing.Size(85, 23);
            this.btnCalcular.TabIndex = 10;
            this.btnCalcular.Text = "Calcular";
            this.btnCalcular.Click += new System.EventHandler(this.btnCalcular_Click);
            // 
            // txtPlazo
            // 
            this.txtPlazo.Location = new System.Drawing.Point(272, 52);
            this.txtPlazo.Name = "txtPlazo";
            this.txtPlazo.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPlazo.Properties.MaxLength = 2;
            this.txtPlazo.Size = new System.Drawing.Size(61, 20);
            this.txtPlazo.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(230, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Plazo:";
            // 
            // txtObservacion
            // 
            this.txtObservacion.Location = new System.Drawing.Point(516, 19);
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Properties.MaxLength = 100;
            this.txtObservacion.Size = new System.Drawing.Size(377, 20);
            this.txtObservacion.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(432, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 13);
            this.label6.TabIndex = 45;
            this.label6.Text = "Observación:";
            // 
            // txtValor
            // 
            this.txtValor.Location = new System.Drawing.Point(89, 19);
            this.txtValor.Name = "txtValor";
            this.txtValor.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtValor.Properties.MaxLength = 100;
            this.txtValor.Size = new System.Drawing.Size(61, 20);
            this.txtValor.TabIndex = 0;
            // 
            // lblValorPrestamo
            // 
            this.lblValorPrestamo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValorPrestamo.Location = new System.Drawing.Point(19, 16);
            this.lblValorPrestamo.Name = "lblValorPrestamo";
            this.lblValorPrestamo.Size = new System.Drawing.Size(57, 30);
            this.lblValorPrestamo.TabIndex = 43;
            this.lblValorPrestamo.Text = "Valor de  Préstamo :";
            // 
            // lblMotivo
            // 
            this.lblMotivo.AutoSize = true;
            this.lblMotivo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMotivo.Location = new System.Drawing.Point(156, 22);
            this.lblMotivo.Name = "lblMotivo";
            this.lblMotivo.Size = new System.Drawing.Size(42, 13);
            this.lblMotivo.TabIndex = 25;
            this.lblMotivo.Text = "Motivo:";
            // 
            // cmbMotivo
            // 
            this.cmbMotivo.Location = new System.Drawing.Point(204, 19);
            this.cmbMotivo.Name = "cmbMotivo";
            this.cmbMotivo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbMotivo.Properties.NullText = "";
            this.cmbMotivo.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbMotivo.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbMotivo.Properties.PopupWidth = 10;
            this.cmbMotivo.Properties.ShowFooter = false;
            this.cmbMotivo.Properties.ShowHeader = false;
            this.cmbMotivo.Size = new System.Drawing.Size(209, 20);
            this.cmbMotivo.TabIndex = 1;
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblEstado.Location = new System.Drawing.Point(790, 24);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(10, 13);
            this.lblEstado.TabIndex = 47;
            this.lblEstado.Text = ":";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(736, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 13);
            this.label11.TabIndex = 46;
            this.label11.Text = "Estado:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(528, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 45;
            this.label3.Text = "Fecha de Solicitud:";
            // 
            // dtpFechaSolicitud
            // 
            this.dtpFechaSolicitud.Enabled = false;
            this.dtpFechaSolicitud.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaSolicitud.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaSolicitud.Location = new System.Drawing.Point(632, 21);
            this.dtpFechaSolicitud.Name = "dtpFechaSolicitud";
            this.dtpFechaSolicitud.Size = new System.Drawing.Size(78, 20);
            this.dtpFechaSolicitud.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtCiudad);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.dtpFechaIngreso);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbEmpleado);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(6, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(899, 44);
            this.groupBox1.TabIndex = 43;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos Personales";
            // 
            // txtCiudad
            // 
            this.txtCiudad.Location = new System.Drawing.Point(701, 19);
            this.txtCiudad.Name = "txtCiudad";
            this.txtCiudad.Properties.MaxLength = 30;
            this.txtCiudad.Size = new System.Drawing.Size(192, 20);
            this.txtCiudad.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(648, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 43;
            this.label4.Text = "Ciudad:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(437, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "Fecha de Ingreso:";
            // 
            // dtpFechaIngreso
            // 
            this.dtpFechaIngreso.Enabled = false;
            this.dtpFechaIngreso.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFechaIngreso.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaIngreso.Location = new System.Drawing.Point(537, 19);
            this.dtpFechaIngreso.Name = "dtpFechaIngreso";
            this.dtpFechaIngreso.Size = new System.Drawing.Size(83, 20);
            this.dtpFechaIngreso.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Empleado:";
            // 
            // cmbEmpleado
            // 
            this.cmbEmpleado.Location = new System.Drawing.Point(69, 19);
            this.cmbEmpleado.Name = "cmbEmpleado";
            this.cmbEmpleado.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbEmpleado.Properties.NullText = "";
            this.cmbEmpleado.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbEmpleado.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbEmpleado.Properties.PopupWidth = 10;
            this.cmbEmpleado.Properties.ShowFooter = false;
            this.cmbEmpleado.Properties.ShowHeader = false;
            this.cmbEmpleado.Size = new System.Drawing.Size(344, 20);
            this.cmbEmpleado.TabIndex = 0;
            this.cmbEmpleado.EditValueChanged += new System.EventHandler(this.cmbEmpleado_EditValueChanged);
            // 
            // txtNo
            // 
            this.txtNo.Enabled = false;
            this.txtNo.Location = new System.Drawing.Point(36, 21);
            this.txtNo.Name = "txtNo";
            this.txtNo.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNo.Properties.MaxLength = 100;
            this.txtNo.Size = new System.Drawing.Size(57, 20);
            this.txtNo.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 13);
            this.label8.TabIndex = 41;
            this.label8.Text = "No.";
            // 
            // frmTrSolicitudPrestamo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 607);
            this.Controls.Add(this.group);
            this.KeyPreview = true;
            this.Name = "frmTrSolicitudPrestamo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTrSolicitudPrestamo";
            this.Load += new System.EventHandler(this.frmTrSolicitudPrestamo_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTrSolicitudPrestamo_KeyDown);
            this.Controls.SetChildIndex(this.group, 0);
            this.group.ResumeLayout(false);
            this.group.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTipo.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCapacidadDeuda.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEgresos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIngresos.Properties)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtValorCuotaFija.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlazo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtObservacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbMotivo.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCiudad.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmpleado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNo.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox group;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.TextEdit txtNo;
        protected System.Windows.Forms.Label label8;
        protected System.Windows.Forms.Label label1;
        public DevExpress.XtraEditors.LookUpEdit cmbEmpleado;
        protected System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFechaSolicitud;
        protected System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpFechaIngreso;
        protected System.Windows.Forms.Label lblEstado;
        protected System.Windows.Forms.Label label11;
        private DevExpress.XtraEditors.TextEdit txtCiudad;
        protected System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevExpress.XtraEditors.TextEdit txtValor;
        protected System.Windows.Forms.Label lblValorPrestamo;
        protected System.Windows.Forms.Label lblMotivo;
        public DevExpress.XtraEditors.LookUpEdit cmbMotivo;
        private DevExpress.XtraEditors.TextEdit txtPlazo;
        protected System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.TextEdit txtObservacion;
        protected System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox4;
        private DevExpress.XtraEditors.TextEdit txtIngresos;
        protected System.Windows.Forms.Label label10;
        private DevExpress.XtraEditors.SimpleButton btnCalcular;
        private DevExpress.XtraEditors.TextEdit txtEgresos;
        protected System.Windows.Forms.Label label12;
        protected System.Windows.Forms.Label label14;
        private DevExpress.XtraEditors.TextEdit txtCapacidadDeuda;
        protected System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraGrid.GridControl gcDatos;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvDatos;
        protected System.Windows.Forms.Label label15;
        private System.Windows.Forms.DateTimePicker dtpFechaInicioPago;
        private DevExpress.XtraEditors.SimpleButton btnAddFila;
        protected System.Windows.Forms.Label label16;
        private DevExpress.XtraEditors.TextEdit txtValorCuotaFija;
        private System.Windows.Forms.RadioButton rdbCuotaFija;
        private System.Windows.Forms.RadioButton rdbPlazo;
        private DevExpress.XtraEditors.SimpleButton btnOrdenar;
        protected System.Windows.Forms.Label lblTipo;
        public DevExpress.XtraEditors.LookUpEdit cmbTipo;
    }
}