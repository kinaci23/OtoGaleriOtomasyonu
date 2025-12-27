namespace OtoGaleri.WinForms
{
    partial class FrmAylikDetay
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
            this.dgvDetay = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetay)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvDetay
            // 
            this.dgvDetay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetay.Location = new System.Drawing.Point(0, 0);
            this.dgvDetay.Name = "dgvDetay";
            this.dgvDetay.Size = new System.Drawing.Size(800, 450);
            this.dgvDetay.TabIndex = 0;
            this.dgvDetay.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDetay_CellDoubleClick);
            // 
            // FrmAylikDetay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvDetay);
            this.Name = "FrmAylikDetay";
            this.Text = "Aylık Detay";
            this.Load += new System.EventHandler(this.FrmAylikDetay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvDetay;
    }
}