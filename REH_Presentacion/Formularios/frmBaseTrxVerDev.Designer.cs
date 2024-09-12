namespace REH_Presentacion.Formularios
{
    partial class frmBaseTrxVerDev
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
            this.tstBotones = new System.Windows.Forms.ToolStrip();
            this.SuspendLayout();
            // 
            // tstBotones
            // 
            this.tstBotones.AutoSize = false;
            this.tstBotones.Dock = System.Windows.Forms.DockStyle.Left;
            this.tstBotones.Location = new System.Drawing.Point(0, 0);
            this.tstBotones.Name = "tstBotones";
            this.tstBotones.Size = new System.Drawing.Size(130, 454);
            this.tstBotones.TabIndex = 16;
            this.tstBotones.Text = "toolStrip1";
            // 
            // frmBaseTrxVerDev
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 454);
            this.Controls.Add(this.tstBotones);
            this.Name = "frmBaseTrxVerDev";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmBaseTrxDev";
            this.Load += new System.EventHandler(this.frmBaseTrxVerDev_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ToolStrip tstBotones;
    }
}