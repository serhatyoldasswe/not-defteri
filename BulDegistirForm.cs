using System;
using System.Windows.Forms;

namespace Notdefteri
{
    public partial class BulDegistirForm : Form
    {
        // Ana formdaki RichTextBox'a buradan ulaşmak için bir değişken
        RichTextBox rtb;

        public BulDegistirForm(RichTextBox anaKutu)
        {
            InitializeComponent();
            rtb = anaKutu; // Ana formdaki dökümanı buraya bağladık

            // ---------------------------------------------------------
            // KRİTİK NOKTA: Butonların tıklama olaylarını (Click) zorla 
            // kodlara bağlıyoruz ki tasarımcıda kopukluk olsa bile çalışsın.
            // ---------------------------------------------------------
            this.Load += (s, e) =>
            {
                if (btnBulSonraki != null) btnBulSonraki.Click += btnBulSonraki_Click;
                if (btnDegistir != null) btnDegistir.Click += btnDegistir_Click;
                if (btnTumunuDegistir != null) btnTumunuDegistir.Click += btnTumunuDegistir_Click;
                if (btnKapat != null) btnKapat.Click += btnKapat_Click;

                // Form her açıldığında her zaman en üstte kalsın (Notepad gibi)
                this.TopMost = true;
            };
        }

        private void btnBulSonraki_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBul.Text)) return;

            RichTextBoxFinds secenekler = RichTextBoxFinds.None;
            if (chkBuyukKucuk != null && chkBuyukKucuk.Checked) secenekler |= RichTextBoxFinds.MatchCase;
            if (chkTamKelime != null && chkTamKelime.Checked) secenekler |= RichTextBoxFinds.WholeWord;

            // Aramaya mevcut seçimin bittiği yerden başla
            int baslangicNoktasi = rtb.SelectionStart + rtb.SelectionLength;
            int sonuc = rtb.Find(txtBul.Text, baslangicNoktasi, secenekler);

            if (sonuc == -1) // Sayfa sonuna geldiyse baştan ara
            {
                sonuc = rtb.Find(txtBul.Text, 0, secenekler);
            }

            if (sonuc != -1)
            {
                rtb.Select(sonuc, txtBul.Text.Length);
                rtb.Focus(); // Bulduğunda metni seçili göstersin diye odaklan
            }
            else
            {
                MessageBox.Show($"'{txtBul.Text}' bulunamadı.", "Not Defteri", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDegistir_Click(object sender, EventArgs e)
        {
            if (rtb.SelectionLength > 0)
            {
                // Büyük/küçük harf durumuna göre seçili metnin bizim aradığımız metin olup olmadığını kontrol et
                bool eslesiyor = (chkBuyukKucuk != null && chkBuyukKucuk.Checked) ?
                    rtb.SelectedText == txtBul.Text :
                    rtb.SelectedText.Equals(txtBul.Text, StringComparison.CurrentCultureIgnoreCase);

                if (eslesiyor)
                {
                    rtb.SelectedText = txtDegistir.Text;
                }
            }

            // Değiştirdikten sonra otomatik olarak bir sonrakini bul
            btnBulSonraki_Click(null, null);
        }

        private void btnTumunuDegistir_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBul.Text)) return;

            // İmlecin mevcut yerini kaybetmemek için kaydediyoruz
            int mevcutKonum = rtb.SelectionStart;

            // Metindeki her şeyi değiştirir (RichTextBox.Text kullandığımız için formattlar plain text kalır)
            // Replace büyük/küçük harf duyarlıdır, o yüzden basit bir değiştirme yapar.
            rtb.Text = rtb.Text.Replace(txtBul.Text, txtDegistir.Text);

            // İmleci eski yerine koymaya çalış
            if (mevcutKonum <= rtb.Text.Length) rtb.SelectionStart = mevcutKonum;

            MessageBox.Show("Tüm eşleşmeler değiştirildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}