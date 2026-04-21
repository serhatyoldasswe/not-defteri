namespace Notdefteri
{
    partial class BulDegistirForm
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
            label1 = new Label();
            label2 = new Label();
            txtBul = new TextBox();
            txtDegistir = new TextBox();
            chkBuyukKucuk = new CheckBox();
            chkTamKelime = new CheckBox();
            btnBulSonraki = new Button();
            btnDegistir = new Button();
            btnTumunuDegistir = new Button();
            btnKapat = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(30, 31);
            label1.Name = "label1";
            label1.Size = new Size(33, 20);
            label1.TabIndex = 0;
            label1.Text = "Bul:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(30, 83);
            label2.Name = "label2";
            label2.Size = new Size(64, 20);
            label2.TabIndex = 1;
            label2.Text = "Değiştir:";
            // 
            // txtBul
            // 
            txtBul.Location = new Point(151, 31);
            txtBul.Name = "txtBul";
            txtBul.Size = new Size(125, 27);
            txtBul.TabIndex = 2;
            // 
            // txtDegistir
            // 
            txtDegistir.Location = new Point(151, 76);
            txtDegistir.Name = "txtDegistir";
            txtDegistir.Size = new Size(125, 27);
            txtDegistir.TabIndex = 3;
            // 
            // chkBuyukKucuk
            // 
            chkBuyukKucuk.AutoSize = true;
            chkBuyukKucuk.Location = new Point(30, 154);
            chkBuyukKucuk.Name = "chkBuyukKucuk";
            chkBuyukKucuk.Size = new Size(199, 24);
            chkBuyukKucuk.TabIndex = 4;
            chkBuyukKucuk.Text = "Büyük/Küçük Harf Duyarlı";
            chkBuyukKucuk.UseVisualStyleBackColor = true;
            // 
            // chkTamKelime
            // 
            chkTamKelime.AutoSize = true;
            chkTamKelime.Location = new Point(257, 154);
            chkTamKelime.Name = "chkTamKelime";
            chkTamKelime.Size = new Size(158, 24);
            chkTamKelime.TabIndex = 5;
            chkTamKelime.Text = "Tam Kelime Eşleştir";
            chkTamKelime.UseVisualStyleBackColor = true;
            // 
            // btnBulSonraki
            // 
            btnBulSonraki.Location = new Point(30, 216);
            btnBulSonraki.Name = "btnBulSonraki";
            btnBulSonraki.Size = new Size(94, 29);
            btnBulSonraki.TabIndex = 6;
            btnBulSonraki.Text = "Bul Sonraki";
            btnBulSonraki.UseVisualStyleBackColor = true;
            // 
            // btnDegistir
            // 
            btnDegistir.Location = new Point(164, 216);
            btnDegistir.Name = "btnDegistir";
            btnDegistir.Size = new Size(94, 29);
            btnDegistir.TabIndex = 7;
            btnDegistir.Text = "Değiştir";
            btnDegistir.UseVisualStyleBackColor = true;
            // 
            // btnTumunuDegistir
            // 
            btnTumunuDegistir.Location = new Point(299, 216);
            btnTumunuDegistir.Name = "btnTumunuDegistir";
            btnTumunuDegistir.Size = new Size(139, 29);
            btnTumunuDegistir.TabIndex = 8;
            btnTumunuDegistir.Text = "Tümünü Değiştir";
            btnTumunuDegistir.UseVisualStyleBackColor = true;
            // 
            // btnKapat
            // 
            btnKapat.Location = new Point(30, 305);
            btnKapat.Name = "btnKapat";
            btnKapat.Size = new Size(94, 29);
            btnKapat.TabIndex = 9;
            btnKapat.Text = "Kapat";
            btnKapat.UseVisualStyleBackColor = true;
            // 
            // BulDegistirForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(496, 345);
            Controls.Add(btnKapat);
            Controls.Add(btnTumunuDegistir);
            Controls.Add(btnDegistir);
            Controls.Add(btnBulSonraki);
            Controls.Add(chkTamKelime);
            Controls.Add(chkBuyukKucuk);
            Controls.Add(txtDegistir);
            Controls.Add(txtBul);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "BulDegistirForm";
            Text = "BulDegistirForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private TextBox txtBul;
        private TextBox txtDegistir;
        private CheckBox chkBuyukKucuk;
        private CheckBox chkTamKelime;
        private Button btnBulSonraki;
        private Button btnDegistir;
        private Button btnTumunuDegistir;
        private Button btnKapat;
    }
}