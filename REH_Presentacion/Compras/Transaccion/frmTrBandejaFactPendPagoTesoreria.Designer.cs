namespace REH_Presentacion.Compras.Transaccion
{
    partial class frmTrBandejaFactPendPagoTesoreria
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTrBandejaFactPendPagoTesoreria));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbcDocumentosPagar = new DevExpress.XtraTab.XtraTabControl();
            this.tbpDocPagar = new DevExpress.XtraTab.XtraTabPage();
            this.cmbGrupoPago = new DevExpress.XtraEditors.LookUpEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.gcRebate = new DevExpress.XtraGrid.GridControl();
            this.dgvRebate = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddManualmente = new DevExpress.XtraEditors.SimpleButton();
            this.tbpHistorial = new DevExpress.XtraTab.XtraTabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnEliminarAdjunto = new DevExpress.XtraEditors.SimpleButton();
            this.btnVisualizar = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdjuntar = new DevExpress.XtraEditors.SimpleButton();
            this.cmbGrupoPagoHistorico = new DevExpress.XtraEditors.LookUpEdit();
            this.txtNo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.gcHistorial = new DevExpress.XtraGrid.GridControl();
            this.dgvHistorial = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnBuscarPorFechas = new DevExpress.XtraEditors.SimpleButton();
            this.dtpFechaFin = new DevExpress.XtraEditors.DateEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.dtpFechaInicio = new DevExpress.XtraEditors.DateEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.ofdArchivo = new System.Windows.Forms.OpenFileDialog();
            this.ofdArchivoAdjunto = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbcDocumentosPagar)).BeginInit();
            this.tbcDocumentosPagar.SuspendLayout();
            this.tbpDocPagar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGrupoPago.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcRebate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRebate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.tbpHistorial.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGrupoPagoHistorico.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNo.Properties)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcHistorial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaFin.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaFin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaInicio.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaInicio.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.tbcDocumentosPagar);
            this.groupBox3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(5, 41);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1135, 555);
            this.groupBox3.TabIndex = 51;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Documentos a Pagar";
            // 
            // tbcDocumentosPagar
            // 
            this.tbcDocumentosPagar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbcDocumentosPagar.Location = new System.Drawing.Point(6, 20);
            this.tbcDocumentosPagar.Name = "tbcDocumentosPagar";
            this.tbcDocumentosPagar.SelectedTabPage = this.tbpDocPagar;
            this.tbcDocumentosPagar.Size = new System.Drawing.Size(1129, 535);
            this.tbcDocumentosPagar.TabIndex = 50;
            this.tbcDocumentosPagar.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tbpDocPagar,
            this.tbpHistorial});
            this.tbcDocumentosPagar.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tbcDocumentosPagar_SelectedPageChanged);
            // 
            // tbpDocPagar
            // 
            this.tbpDocPagar.Controls.Add(this.cmbGrupoPago);
            this.tbpDocPagar.Controls.Add(this.label2);
            this.tbpDocPagar.Controls.Add(this.lblTotal);
            this.tbpDocPagar.Controls.Add(this.gcRebate);
            this.tbpDocPagar.Controls.Add(this.label1);
            this.tbpDocPagar.Controls.Add(this.btnAddManualmente);
            this.tbpDocPagar.Name = "tbpDocPagar";
            this.tbpDocPagar.Size = new System.Drawing.Size(1123, 507);
            this.tbpDocPagar.Text = "Generar Archivo MultiCash";
            // 
            // cmbGrupoPago
            // 
            this.cmbGrupoPago.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbGrupoPago.Location = new System.Drawing.Point(109, 12);
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
            this.cmbGrupoPago.Size = new System.Drawing.Size(491, 20);
            this.cmbGrupoPago.TabIndex = 52;
            this.cmbGrupoPago.EditValueChanged += new System.EventHandler(this.cmbGrupoPago_EditValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 51;
            this.label2.Text = "Grupo de Pago";
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(1021, 19);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(0, 13);
            this.lblTotal.TabIndex = 48;
            this.lblTotal.Visible = false;
            // 
            // gcRebate
            // 
            this.gcRebate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcRebate.Location = new System.Drawing.Point(7, 43);
            this.gcRebate.MainView = this.dgvRebate;
            this.gcRebate.Name = "gcRebate";
            this.gcRebate.Size = new System.Drawing.Size(1113, 452);
            this.gcRebate.TabIndex = 0;
            this.gcRebate.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvRebate,
            this.gridView2});
            // 
            // dgvRebate
            // 
            this.dgvRebate.GridControl = this.gcRebate;
            this.dgvRebate.Name = "dgvRebate";
            this.dgvRebate.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvRebate.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.dgvRebate.OptionsBehavior.Editable = false;
            this.dgvRebate.OptionsCustomization.AllowColumnMoving = false;
            this.dgvRebate.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.dgvRebate.OptionsView.ShowAutoFilterRow = true;
            this.dgvRebate.OptionsView.ShowFooter = true;
            this.dgvRebate.OptionsView.ShowGroupPanel = false;
            this.dgvRebate.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.dgvRebate_CustomDrawCell);
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gcRebate;
            this.gridView2.Name = "gridView2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(979, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 47;
            this.label1.Text = "Total";
            this.label1.Visible = false;
            // 
            // btnAddManualmente
            // 
            this.btnAddManualmente.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAddManualmente.ImageOptions.Image")));
            this.btnAddManualmente.Location = new System.Drawing.Point(799, 14);
            this.btnAddManualmente.Name = "btnAddManualmente";
            this.btnAddManualmente.Size = new System.Drawing.Size(113, 23);
            this.btnAddManualmente.TabIndex = 44;
            this.btnAddManualmente.Text = "Seleccionar todos";
            this.btnAddManualmente.Visible = false;
            this.btnAddManualmente.Click += new System.EventHandler(this.btnAddManualmente_Click);
            // 
            // tbpHistorial
            // 
            this.tbpHistorial.Controls.Add(this.groupBox2);
            this.tbpHistorial.Controls.Add(this.groupBox4);
            this.tbpHistorial.Name = "tbpHistorial";
            this.tbpHistorial.Size = new System.Drawing.Size(1123, 507);
            this.tbpHistorial.Text = "Historial";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnEliminarAdjunto);
            this.groupBox2.Controls.Add(this.btnVisualizar);
            this.groupBox2.Controls.Add(this.btnAdjuntar);
            this.groupBox2.Controls.Add(this.cmbGrupoPagoHistorico);
            this.groupBox2.Controls.Add(this.txtNo);
            this.groupBox2.Controls.Add(this.labelControl5);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(3, 14);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1112, 56);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Historial de Grupo de Pago";
            // 
            // btnEliminarAdjunto
            // 
            this.btnEliminarAdjunto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEliminarAdjunto.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnEliminarAdjunto.ImageOptions.Image")));
            this.btnEliminarAdjunto.Location = new System.Drawing.Point(1052, 21);
            this.btnEliminarAdjunto.Name = "btnEliminarAdjunto";
            this.btnEliminarAdjunto.Size = new System.Drawing.Size(24, 23);
            this.btnEliminarAdjunto.TabIndex = 71;
            this.btnEliminarAdjunto.Click += new System.EventHandler(this.btnEliminarAdjunto_Click);
            // 
            // btnVisualizar
            // 
            this.btnVisualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVisualizar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnVisualizar.ImageOptions.Image")));
            this.btnVisualizar.Location = new System.Drawing.Point(1082, 21);
            this.btnVisualizar.Name = "btnVisualizar";
            this.btnVisualizar.Size = new System.Drawing.Size(24, 23);
            this.btnVisualizar.TabIndex = 70;
            this.btnVisualizar.Click += new System.EventHandler(this.btnVisualizar_Click);
            // 
            // btnAdjuntar
            // 
            this.btnAdjuntar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnAdjuntar.ImageOptions.Image")));
            this.btnAdjuntar.Location = new System.Drawing.Point(799, 21);
            this.btnAdjuntar.Name = "btnAdjuntar";
            this.btnAdjuntar.Size = new System.Drawing.Size(24, 23);
            this.btnAdjuntar.TabIndex = 69;
            this.btnAdjuntar.Click += new System.EventHandler(this.btnAdjuntar_Click);
            // 
            // cmbGrupoPagoHistorico
            // 
            this.cmbGrupoPagoHistorico.Location = new System.Drawing.Point(7, 23);
            this.cmbGrupoPagoHistorico.Name = "cmbGrupoPagoHistorico";
            this.cmbGrupoPagoHistorico.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbGrupoPagoHistorico.Properties.NullText = "";
            this.cmbGrupoPagoHistorico.Properties.PopupFilterMode = DevExpress.XtraEditors.PopupFilterMode.Contains;
            this.cmbGrupoPagoHistorico.Properties.PopupFormMinSize = new System.Drawing.Size(10, 0);
            this.cmbGrupoPagoHistorico.Properties.PopupWidth = 10;
            this.cmbGrupoPagoHistorico.Properties.ShowFooter = false;
            this.cmbGrupoPagoHistorico.Properties.ShowHeader = false;
            this.cmbGrupoPagoHistorico.Properties.UseReadOnlyAppearance = false;
            this.cmbGrupoPagoHistorico.Size = new System.Drawing.Size(601, 20);
            this.cmbGrupoPagoHistorico.TabIndex = 66;
            this.cmbGrupoPagoHistorico.EditValueChanged += new System.EventHandler(this.cmbSemana_EditValueChanged);
            // 
            // txtNo
            // 
            this.txtNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNo.Enabled = false;
            this.txtNo.Location = new System.Drawing.Point(829, 23);
            this.txtNo.Name = "txtNo";
            this.txtNo.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtNo.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtNo.Properties.ReadOnly = true;
            this.txtNo.Properties.UseReadOnlyAppearance = false;
            this.txtNo.Size = new System.Drawing.Size(217, 20);
            this.txtNo.TabIndex = 65;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(709, 26);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(84, 13);
            this.labelControl5.TabIndex = 64;
            this.labelControl5.Text = "Adjuntar archivo:";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.gcHistorial);
            this.groupBox4.Controls.Add(this.btnBuscarPorFechas);
            this.groupBox4.Controls.Add(this.dtpFechaFin);
            this.groupBox4.Controls.Add(this.labelControl6);
            this.groupBox4.Controls.Add(this.dtpFechaInicio);
            this.groupBox4.Controls.Add(this.labelControl7);
            this.groupBox4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(3, 118);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1117, 385);
            this.groupBox4.TabIndex = 37;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Detalle de pagos por rango de fechas";
            // 
            // gcHistorial
            // 
            this.gcHistorial.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcHistorial.Location = new System.Drawing.Point(6, 43);
            this.gcHistorial.MainView = this.dgvHistorial;
            this.gcHistorial.Name = "gcHistorial";
            this.gcHistorial.Size = new System.Drawing.Size(1105, 336);
            this.gcHistorial.TabIndex = 38;
            this.gcHistorial.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dgvHistorial,
            this.gridView1});
            // 
            // dgvHistorial
            // 
            this.dgvHistorial.GridControl = this.gcHistorial;
            this.dgvHistorial.Name = "dgvHistorial";
            this.dgvHistorial.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.dgvHistorial.OptionsBehavior.AllowFixedGroups = DevExpress.Utils.DefaultBoolean.False;
            this.dgvHistorial.OptionsBehavior.Editable = false;
            this.dgvHistorial.OptionsCustomization.AllowColumnMoving = false;
            this.dgvHistorial.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
            this.dgvHistorial.OptionsView.ShowAutoFilterRow = true;
            this.dgvHistorial.OptionsView.ShowFooter = true;
            this.dgvHistorial.OptionsView.ShowGroupPanel = false;
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gcHistorial;
            this.gridView1.Name = "gridView1";
            // 
            // btnBuscarPorFechas
            // 
            this.btnBuscarPorFechas.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnBuscarPorFechas.ImageOptions.Image")));
            this.btnBuscarPorFechas.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.TopCenter;
            this.btnBuscarPorFechas.Location = new System.Drawing.Point(470, 15);
            this.btnBuscarPorFechas.Name = "btnBuscarPorFechas";
            this.btnBuscarPorFechas.Size = new System.Drawing.Size(31, 22);
            this.btnBuscarPorFechas.TabIndex = 2;
            this.btnBuscarPorFechas.Click += new System.EventHandler(this.btnBuscarPorFechas_Click);
            // 
            // dtpFechaFin
            // 
            this.dtpFechaFin.EditValue = null;
            this.dtpFechaFin.Location = new System.Drawing.Point(366, 17);
            this.dtpFechaFin.Name = "dtpFechaFin";
            this.dtpFechaFin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpFechaFin.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpFechaFin.Size = new System.Drawing.Size(97, 20);
            this.dtpFechaFin.TabIndex = 1;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(245, 20);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(115, 13);
            this.labelControl6.TabIndex = 52;
            this.labelControl6.Text = "Fecha Generación Final:";
            // 
            // dtpFechaInicio
            // 
            this.dtpFechaInicio.EditValue = null;
            this.dtpFechaInicio.Location = new System.Drawing.Point(133, 17);
            this.dtpFechaInicio.Name = "dtpFechaInicio";
            this.dtpFechaInicio.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpFechaInicio.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpFechaInicio.Size = new System.Drawing.Size(97, 20);
            this.dtpFechaInicio.TabIndex = 0;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(7, 20);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(120, 13);
            this.labelControl7.TabIndex = 50;
            this.labelControl7.Text = "Fecha Generación Inicial:";
            // 
            // ofdArchivoAdjunto
            // 
            this.ofdArchivoAdjunto.FileName = "ofdArchicoAdjunto";
            // 
            // frmTrBandejaFactPendPagoTesoreria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1146, 599);
            this.Controls.Add(this.groupBox3);
            this.Name = "frmTrBandejaFactPendPagoTesoreria";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTrBandejaFactPendPagoPorAprobar";
            this.Load += new System.EventHandler(this.frmTrRebate_Load);
            this.Controls.SetChildIndex(this.groupBox3, 0);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tbcDocumentosPagar)).EndInit();
            this.tbcDocumentosPagar.ResumeLayout(false);
            this.tbpDocPagar.ResumeLayout(false);
            this.tbpDocPagar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGrupoPago.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcRebate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRebate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.tbpHistorial.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGrupoPagoHistorico.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNo.Properties)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcHistorial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaFin.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaFin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaInicio.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaInicio.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.OpenFileDialog ofdArchivo;
        private System.Windows.Forms.OpenFileDialog ofdArchivoAdjunto;
        private DevExpress.XtraTab.XtraTabControl tbcDocumentosPagar;
        private DevExpress.XtraTab.XtraTabPage tbpDocPagar;
        private System.Windows.Forms.Label lblTotal;
        private DevExpress.XtraGrid.GridControl gcRebate;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvRebate;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton btnAddManualmente;
        private DevExpress.XtraTab.XtraTabPage tbpHistorial;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.SimpleButton btnEliminarAdjunto;
        private DevExpress.XtraEditors.SimpleButton btnVisualizar;
        private DevExpress.XtraEditors.SimpleButton btnAdjuntar;
        public DevExpress.XtraEditors.LookUpEdit cmbGrupoPagoHistorico;
        private DevExpress.XtraEditors.TextEdit txtNo;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private System.Windows.Forms.GroupBox groupBox4;
        private DevExpress.XtraGrid.GridControl gcHistorial;
        private DevExpress.XtraGrid.Views.Grid.GridView dgvHistorial;
        private DevExpress.XtraEditors.SimpleButton btnBuscarPorFechas;
        private DevExpress.XtraEditors.DateEdit dtpFechaFin;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.DateEdit dtpFechaInicio;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        public DevExpress.XtraEditors.LookUpEdit cmbGrupoPago;
        private System.Windows.Forms.Label label2;
    }
}