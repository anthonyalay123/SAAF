namespace REH_Presentacion.Compras.Transaccion
{
    partial class frmTrCotizacion
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTrCotizacion));
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode3 = new DevExpress.XtraGrid.GridLevelNode();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gcBandejaSolicitud = new DevExpress.XtraGrid.GridControl();
            this.dgvBandejaSolicitud = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAddFila = new DevExpress.XtraEditors.SimpleButton();
            this.gcProveedor = new DevExpress.XtraGrid.GridControl();
            this.dgvProveedor = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.AddFilaDetalle = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.gcCotizacionDetalle = new DevExpress.XtraGrid.GridControl();
            this.dgvCotizacionDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.gcAdjunto = new DevExpress.XtraGrid.GridControl();
            this.dgvAdjunto = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ofdArchicoAdjunto = new System.Windows.Forms.OpenFileDialog();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblFecha = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.txtObservacion = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.chbCompletado = new DevExpress.XtraEditors.CheckEdit();
            this.cmbEstado = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtDescripcion = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtNo = new DevExpress.XtraEditors.TextEdit();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaSolicitud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaSolicitud)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcProveedor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProveedor)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCotizacionDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCotizacionDetalle)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcAdjunto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdjunto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtObservacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbCompletado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.gcBandejaSolicitud);
            this.groupBox1.Location = new System.Drawing.Point(13, 135);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1108, 87);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Solicitud de compra";
            // 
            // gcBandejaSolicitud
            // 
            this.gcBandejaSolicitud.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode1.RelationName = "Level1";
            this.gcBandejaSolicitud.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gcBandejaSolicitud.Location = new System.Drawing.Point(6, 19);
            this.gcBandejaSolicitud.MainView = this.dgvBandejaSolicitud;
            this.gcBandejaSolicitud.Name = "gcBandejaSolicitud";
            this.gcBandejaSolicitud.Size = new System.Drawing.Size(1096, 62);
            this.gcBandejaSolicitud.TabIndex = 21;
            this.gcBandejaSolicitud.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvBandejaSolicitud});
            // 
            // dgvBandejaSolicitud
            // 
            this.dgvBandejaSolicitud.ActiveFilterEnabled = false;
            this.dgvBandejaSolicitud.GridControl = this.gcBandejaSolicitud;
            this.dgvBandejaSolicitud.Name = "dgvBandejaSolicitud";
            this.dgvBandejaSolicitud.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvBandejaSolicitud.OptionsCustomization.AllowFilter = false;
            this.dgvBandejaSolicitud.OptionsCustomization.AllowGroup = false;
            this.dgvBandejaSolicitud.OptionsCustomization.AllowSort = false;
            this.dgvBandejaSolicitud.OptionsPrint.AutoWidth = false;
            this.dgvBandejaSolicitud.OptionsView.ShowGroupPanel = false;
            this.dgvBandejaSolicitud.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.GridView1_PopupMenuShowing);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnAddFila);
            this.groupBox2.Controls.Add(this.gcProveedor);
            this.groupBox2.Location = new System.Drawing.Point(13, 228);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1108, 167);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Proveedor";
            // 
            // btnAddFila
            // 
            this.btnAddFila.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFila.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddFila.ImageOptions.Image")));
            this.btnAddFila.Location = new System.Drawing.Point(1021, 9);
            this.btnAddFila.Name = "btnAddFila";
            this.btnAddFila.Size = new System.Drawing.Size(75, 23);
            this.btnAddFila.TabIndex = 44;
            this.btnAddFila.Text = "Añadir";
            this.btnAddFila.Click += new System.EventHandler(this.btnAddFila_Click);
            // 
            // gcProveedor
            // 
            this.gcProveedor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode2.RelationName = "Level1";
            this.gcProveedor.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode2});
            this.gcProveedor.Location = new System.Drawing.Point(7, 38);
            this.gcProveedor.MainView = this.dgvProveedor;
            this.gcProveedor.Name = "gcProveedor";
            this.gcProveedor.Size = new System.Drawing.Size(1095, 123);
            this.gcProveedor.TabIndex = 21;
            this.gcProveedor.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvProveedor});
            // 
            // dgvProveedor
            // 
            this.dgvProveedor.GridControl = this.gcProveedor;
            this.dgvProveedor.Name = "dgvProveedor";
            this.dgvProveedor.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvProveedor.OptionsCustomization.AllowColumnMoving = false;
            this.dgvProveedor.OptionsPrint.AutoWidth = false;
            this.dgvProveedor.OptionsView.ShowGroupPanel = false;
            this.dgvProveedor.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.GridView1_PopupMenuShowing);
            this.dgvProveedor.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.dgvProveedor_FocusedRowChanged);
            this.dgvProveedor.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.dgvProveedor_CellValueChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.AddFilaDetalle);
            this.groupBox3.Controls.Add(this.xtraTabControl1);
            this.groupBox3.Location = new System.Drawing.Point(13, 398);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1109, 233);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Detalle";
            // 
            // AddFilaDetalle
            // 
            this.AddFilaDetalle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddFilaDetalle.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("AddFilaDetalle.ImageOptions.Image")));
            this.AddFilaDetalle.Location = new System.Drawing.Point(1021, 11);
            this.AddFilaDetalle.Name = "AddFilaDetalle";
            this.AddFilaDetalle.Size = new System.Drawing.Size(75, 23);
            this.AddFilaDetalle.TabIndex = 46;
            this.AddFilaDetalle.Text = "Añadir";
            this.AddFilaDetalle.Visible = false;
            this.AddFilaDetalle.Click += new System.EventHandler(this.AddFilaDetalle_Click);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xtraTabControl1.Location = new System.Drawing.Point(4, 19);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(1098, 214);
            this.xtraTabControl1.TabIndex = 46;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            this.xtraTabControl1.Click += new System.EventHandler(this.xtraTabControl1_Click);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.gcCotizacionDetalle);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1092, 186);
            this.xtraTabPage1.Text = "Detalle";
            // 
            // gcCotizacionDetalle
            // 
            this.gcCotizacionDetalle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcCotizacionDetalle.Location = new System.Drawing.Point(-1, 0);
            this.gcCotizacionDetalle.MainView = this.dgvCotizacionDetalle;
            this.gcCotizacionDetalle.Name = "gcCotizacionDetalle";
            this.gcCotizacionDetalle.Size = new System.Drawing.Size(1093, 183);
            this.gcCotizacionDetalle.TabIndex = 23;
            this.gcCotizacionDetalle.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvCotizacionDetalle});
            // 
            // dgvCotizacionDetalle
            // 
            this.dgvCotizacionDetalle.GridControl = this.gcCotizacionDetalle;
            this.dgvCotizacionDetalle.Name = "dgvCotizacionDetalle";
            this.dgvCotizacionDetalle.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvCotizacionDetalle.OptionsCustomization.AllowColumnMoving = false;
            this.dgvCotizacionDetalle.OptionsPrint.AutoWidth = false;
            this.dgvCotizacionDetalle.OptionsView.ShowFooter = true;
            this.dgvCotizacionDetalle.OptionsView.ShowGroupPanel = false;
            this.dgvCotizacionDetalle.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.GridView1_PopupMenuShowing);
            this.dgvCotizacionDetalle.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.dgvCotizacionDetalle_CellValueChanged);
            this.dgvCotizacionDetalle.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.dgvCotizacionDetalle_CellValueChanging);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.gcAdjunto);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1092, 186);
            this.xtraTabPage2.Text = "Archivos Adjuntos";
            // 
            // gcAdjunto
            // 
            this.gcAdjunto.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode3.RelationName = "Level1";
            this.gcAdjunto.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode3});
            this.gcAdjunto.Location = new System.Drawing.Point(-6, 0);
            this.gcAdjunto.MainView = this.dgvAdjunto;
            this.gcAdjunto.Name = "gcAdjunto";
            this.gcAdjunto.Size = new System.Drawing.Size(1097, 188);
            this.gcAdjunto.TabIndex = 23;
            this.gcAdjunto.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvAdjunto,
            this.gridView1});
            // 
            // dgvAdjunto
            // 
            this.dgvAdjunto.GridControl = this.gcAdjunto;
            this.dgvAdjunto.Name = "dgvAdjunto";
            this.dgvAdjunto.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvAdjunto.OptionsPrint.AutoWidth = false;
            this.dgvAdjunto.OptionsView.ShowGroupPanel = false;
            this.dgvAdjunto.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.GridView1_PopupMenuShowing);
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gcAdjunto;
            this.gridView1.Name = "gridView1";
            // 
            // ofdArchicoAdjunto
            // 
            this.ofdArchicoAdjunto.FileName = "openFileDialog1";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.lblFecha);
            this.groupBox4.Controls.Add(this.labelControl7);
            this.groupBox4.Controls.Add(this.txtObservacion);
            this.groupBox4.Controls.Add(this.labelControl3);
            this.groupBox4.Controls.Add(this.chbCompletado);
            this.groupBox4.Controls.Add(this.cmbEstado);
            this.groupBox4.Controls.Add(this.labelControl2);
            this.groupBox4.Controls.Add(this.txtDescripcion);
            this.groupBox4.Controls.Add(this.labelControl5);
            this.groupBox4.Controls.Add(this.labelControl1);
            this.groupBox4.Controls.Add(this.txtNo);
            this.groupBox4.Location = new System.Drawing.Point(12, 41);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1109, 89);
            this.groupBox4.TabIndex = 56;
            this.groupBox4.TabStop = false;
            // 
            // lblFecha
            // 
            this.lblFecha.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblFecha.Appearance.Options.UseFont = true;
            this.lblFecha.Location = new System.Drawing.Point(416, 15);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(0, 13);
            this.lblFecha.TabIndex = 74;
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Location = new System.Drawing.Point(312, 15);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(97, 13);
            this.labelControl7.TabIndex = 73;
            this.labelControl7.Text = "Fecha Cotización:";
            // 
            // txtObservacion
            // 
            this.txtObservacion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObservacion.Location = new System.Drawing.Point(87, 64);
            this.txtObservacion.Name = "txtObservacion";
            this.txtObservacion.Properties.MaxLength = 100;
            this.txtObservacion.Size = new System.Drawing.Size(1016, 20);
            this.txtObservacion.TabIndex = 63;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(11, 67);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(64, 13);
            this.labelControl3.TabIndex = 64;
            this.labelControl3.Text = "Observación:";
            // 
            // chbCompletado
            // 
            this.chbCompletado.Location = new System.Drawing.Point(832, 12);
            this.chbCompletado.Name = "chbCompletado";
            this.chbCompletado.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.chbCompletado.Properties.Appearance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chbCompletado.Properties.Appearance.Options.UseBackColor = true;
            this.chbCompletado.Properties.Appearance.Options.UseForeColor = true;
            this.chbCompletado.Properties.Caption = "Completada";
            this.chbCompletado.Size = new System.Drawing.Size(87, 19);
            this.chbCompletado.TabIndex = 62;
            this.chbCompletado.CheckedChanged += new System.EventHandler(this.chbCompletado_CheckedChanged);
            // 
            // cmbEstado
            // 
            this.cmbEstado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbEstado.Location = new System.Drawing.Point(973, 12);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbEstado.Properties.NullText = "";
            this.cmbEstado.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbEstado.Properties.PopupWidth = 10;
            this.cmbEstado.Properties.ReadOnly = true;
            this.cmbEstado.Properties.ShowFooter = false;
            this.cmbEstado.Properties.ShowHeader = false;
            this.cmbEstado.Properties.UseReadOnlyAppearance = false;
            this.cmbEstado.Size = new System.Drawing.Size(130, 20);
            this.cmbEstado.TabIndex = 60;
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl2.Location = new System.Drawing.Point(930, 15);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(37, 13);
            this.labelControl2.TabIndex = 61;
            this.labelControl2.Text = "Estado:";
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescripcion.Location = new System.Drawing.Point(87, 38);
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Properties.MaxLength = 100;
            this.txtDescripcion.Size = new System.Drawing.Size(1016, 20);
            this.txtDescripcion.TabIndex = 57;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(11, 41);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(58, 13);
            this.labelControl5.TabIndex = 59;
            this.labelControl5.Text = "Descripcion:";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(11, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(17, 13);
            this.labelControl1.TabIndex = 58;
            this.labelControl1.Text = "No:";
            // 
            // txtNo
            // 
            this.txtNo.Enabled = false;
            this.txtNo.Location = new System.Drawing.Point(87, 12);
            this.txtNo.Name = "txtNo";
            this.txtNo.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtNo.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtNo.Properties.ReadOnly = true;
            this.txtNo.Properties.UseReadOnlyAppearance = false;
            this.txtNo.Size = new System.Drawing.Size(61, 20);
            this.txtNo.TabIndex = 56;
            // 
            // gridView2
            // 
            this.gridView2.Name = "gridView2";
            // 
            // frmTrCotizacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1133, 634);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmTrCotizacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTrCotizacion";
            this.Load += new System.EventHandler(this.frmTrCotizacion_Load);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.groupBox3, 0);
            this.Controls.SetChildIndex(this.groupBox4, 0);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcBandejaSolicitud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBandejaSolicitud)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcProveedor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProveedor)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcCotizacionDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCotizacionDetalle)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcAdjunto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdjunto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtObservacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbCompletado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescripcion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraGrid.GridControl gcBandejaSolicitud;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvBandejaSolicitud;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraGrid.GridControl gcProveedor;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvProveedor;
        private System.Windows.Forms.GroupBox groupBox3;
        private DevExpress.XtraEditors.SimpleButton btnAddFila;
        private DevExpress.XtraEditors.SimpleButton AddFilaDetalle;
        private System.Windows.Forms.OpenFileDialog ofdArchicoAdjunto;
        private System.Windows.Forms.GroupBox groupBox4;
        private DevExpress.XtraEditors.TextEdit txtDescripcion;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtNo;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraGrid.GridControl gcCotizacionDetalle;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvCotizacionDetalle;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraGrid.GridControl gcAdjunto;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvAdjunto;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        public DevExpress.XtraEditors.LookUpEdit cmbEstado;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckEdit chbCompletado;
        private DevExpress.XtraEditors.TextEdit txtObservacion;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl lblFecha;
        private DevExpress.XtraEditors.LabelControl labelControl7;
    }
}