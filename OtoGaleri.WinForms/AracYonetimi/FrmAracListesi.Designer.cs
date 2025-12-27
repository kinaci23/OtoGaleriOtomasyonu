namespace OtoGaleri.WinForms.AracYonetimi
{
    partial class FrmAracListesi
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
            this.dgvAraclar = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnYeniArac = new System.Windows.Forms.Button();
            this.btnSil = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAraclar)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvAraclar
            // 
            this.dgvAraclar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAraclar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvAraclar.Location = new System.Drawing.Point(0, 84);
            this.dgvAraclar.Name = "dgvAraclar";
            this.dgvAraclar.Size = new System.Drawing.Size(754, 577);
            this.dgvAraclar.TabIndex = 0;
            this.dgvAraclar.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAraclar_CellDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSil);
            this.panel1.Controls.Add(this.btnYeniArac);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(754, 78);
            this.panel1.TabIndex = 1;
            // 
            // btnYeniArac
            // 
            this.btnYeniArac.BackColor = System.Drawing.Color.GreenYellow;
            this.btnYeniArac.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnYeniArac.Location = new System.Drawing.Point(12, 12);
            this.btnYeniArac.Name = "btnYeniArac";
            this.btnYeniArac.Size = new System.Drawing.Size(161, 44);
            this.btnYeniArac.TabIndex = 0;
            this.btnYeniArac.Text = "+ YENİ ARAÇ EKLE";
            this.btnYeniArac.UseVisualStyleBackColor = false;
            this.btnYeniArac.Click += new System.EventHandler(this.btnYeniArac_Click);
            // 
            // btnSil
            // 
            this.btnSil.BackColor = System.Drawing.Color.IndianRed;
            this.btnSil.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSil.ForeColor = System.Drawing.Color.White;
            this.btnSil.Location = new System.Drawing.Point(564, 12);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(161, 44);
            this.btnSil.TabIndex = 1;
            this.btnSil.Text = "SEÇİLİ ARACI SİL";
            this.btnSil.UseVisualStyleBackColor = false;
            this.btnSil.Click += new System.EventHandler(this.btnSil_Click);
            // 
            // FrmAracListesi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(754, 661);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvAraclar);
            this.MaximizeBox = false;
            this.Name = "FrmAracListesi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Araç Listesi";
            this.Load += new System.EventHandler(this.FrmAracListesi_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAraclar)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvAraclar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnYeniArac;
        private System.Windows.Forms.Button btnSil;
    }
}