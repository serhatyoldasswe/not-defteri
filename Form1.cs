#nullable disable
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace Notdefteri
{
    public partial class Form1 : Form
    {
        string dosyaYolu = "";
        bool degisiklikVar = false;
        Panel panelSatir;
        List<string> sonAcilanDosyalar = new List<string>();

        // yazım denetimi (sözlük) için oluşturduğum değişkenler
        HashSet<string> sozluk = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        bool denetleniyor = false;

        public Form1()
        {
            InitializeComponent();
            this.Load -= Form1_Load;
            this.Load += Form1_Load;
            this.FormClosing -= Form1_FormClosing;
            this.FormClosing += Form1_FormClosing;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // kelimeler.txt dosyası varsa satır satır okuyup sözlük listesine atıyorum ki hatalı kelimeleri bulabilelim
            if (File.Exists("kelimeler.txt"))
            {
                string[] kelimeler = File.ReadAllLines("kelimeler.txt");
                foreach (var k in kelimeler) sozluk.Add(k.Trim());
            }

            panelSatir = new Panel();
            panelSatir.Width = 35;
            panelSatir.Dock = DockStyle.Left;
            panelSatir.BackColor = Color.WhiteSmoke;
            panelSatir.Paint += panelSatir_Paint;
            this.Controls.Add(panelSatir);

            if (richTextBox1 != null)
            {
                richTextBox1.AllowDrop = true;
                richTextBox1.DragEnter += Form1_DragEnter;
                richTextBox1.DragDrop += Form1_DragDrop;

                richTextBox1.Dock = DockStyle.Fill;
                richTextBox1.BringToFront();
                richTextBox1.TextChanged += richTextBox1_TextChanged;
                richTextBox1.SelectionChanged += richTextBox1_SelectionChanged;
                richTextBox1.KeyDown += richTextBox1_KeyDown;
                richTextBox1.VScroll += (s, ev) => panelSatir.Invalidate();
                richTextBox1.Resize += (s, ev) => panelSatir.Invalidate();
                richTextBox1.FontChanged += (s, ev) => panelSatir.Invalidate();
            }

            if (durumSatirSutun != null) durumSatirSutun.Spring = false;
            if (durumKarakterSayisi != null) durumKarakterSayisi.Spring = false;
            if (durumKodlama != null) durumKodlama.Spring = false;
            if (durumIslemBilgisi != null) durumIslemBilgisi.Spring = false;

            if (durumSatirSutun != null) durumSatirSutun.Text = "Satır: 1, Sütun: 1  |  ";
            if (durumKarakterSayisi != null) durumKarakterSayisi.Text = "0 karakter  |  ";
            if (durumKodlama != null) durumKodlama.Text = "UTF-8  |  ";
            if (durumIslemBilgisi != null) durumIslemBilgisi.Text = "Hazır";

            if (sayfaYapısıToolStripMenuItem != null) sayfaYapısıToolStripMenuItem.Click += sayfaYapısıToolStripMenuItem_Click;
            if (çıkışToolStripMenuItem != null) çıkışToolStripMenuItem.Click += çıkışToolStripMenuItem_Click;

            SonAcilanlariYukle();
        }

        // bu fonksiyon metni kelime kelime böler ve sözlükte olmayan kelimelerin rengini kırmızı yapar
        private void YazimDenetimiYap()
        {
            if (sozluk.Count == 0) return;
            denetleniyor = true;

            int imlec = richTextBox1.SelectionStart;
            string[] kelimeler = richTextBox1.Text.Split(new char[] { ' ', '\n', '\r', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string kelime in kelimeler)
            {
                if (kelime.Length <= 1) continue;

                if (!sozluk.Contains(kelime))
                {
                    int index = 0;
                    while ((index = richTextBox1.Find(kelime, index, RichTextBoxFinds.WholeWord)) != -1)
                    {
                        richTextBox1.Select(index, kelime.Length);
                        richTextBox1.SelectionColor = Color.Red;
                        index += kelime.Length;
                    }
                }
            }

            richTextBox1.SelectionStart = imlec;
            richTextBox1.SelectionLength = 0;
            richTextBox1.SelectionColor = richTextBox1.ForeColor;
            denetleniyor = false;
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] dosyalar = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (dosyalar.Length > 0)
            {
                string yol = dosyalar[0];
                if (KaydetmeyiKontrolEt())
                {
                    try
                    {
                        if (yol.EndsWith(".rtf")) richTextBox1.LoadFile(yol);
                        else richTextBox1.Text = File.ReadAllText(yol);

                        dosyaYolu = yol;
                        this.Text = yol;
                        degisiklikVar = false;
                        SonAcilanlariGuncelle(yol);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Dosya açılırken hata: " + ex.Message);
                    }
                }
            }
        }

        private void YazıBoyutunuDegistir(float miktar)
        {
            if (richTextBox1.SelectionFont != null)
            {
                Font mevcutFont = richTextBox1.SelectionFont;
                float yeniBoyut = mevcutFont.Size + miktar;
                if (yeniBoyut < 2) yeniBoyut = 2;
                richTextBox1.SelectionFont = new Font(mevcutFont.FontFamily, yeniBoyut, mevcutFont.Style);
            }
        }

        private void solGirintiArtirToolStripMenuItem_Click(object sender, EventArgs e) => richTextBox1.SelectionIndent += 10;
        private void solGirintiAzaltToolStripMenuItem_Click(object sender, EventArgs e) => richTextBox1.SelectionIndent = Math.Max(0, richTextBox1.SelectionIndent - 10);
        private void sagGirintiArtirToolStripMenuItem_Click(object sender, EventArgs e) => richTextBox1.SelectionRightIndent += 10;
        private void ilkSatirGirintiToolStripMenuItem_Click(object sender, EventArgs e) => richTextBox1.SelectionHangingIndent = -20;

        private void SonAcilanlariKaydet()
        {
            try { File.WriteAllLines("son_dosyalar.txt", sonAcilanDosyalar); } catch { }
        }

        private void SonAcilanlariYukle()
        {
            try
            {
                if (File.Exists("son_dosyalar.txt"))
                {
                    string[] yollar = File.ReadAllLines("son_dosyalar.txt");
                    foreach (string yol in yollar) { if (File.Exists(yol)) sonAcilanDosyalar.Add(yol); }
                    SonAcilanlariGuncelle("");
                }
            }
            catch { }
        }

        private void SonAcilanlariGuncelle(string yol)
        {
            if (!string.IsNullOrEmpty(yol))
            {
                if (sonAcilanDosyalar.Contains(yol)) sonAcilanDosyalar.Remove(yol);
                sonAcilanDosyalar.Insert(0, yol);
                if (sonAcilanDosyalar.Count > 5) sonAcilanDosyalar.RemoveAt(5);
            }

            if (sonAçılanlarToolStripMenuItem != null)
            {
                sonAçılanlarToolStripMenuItem.DropDownItems.Clear();
                foreach (string s in sonAcilanDosyalar)
                {
                    ToolStripMenuItem item = new ToolStripMenuItem(Path.GetFileName(s));
                    item.ToolTipText = s;
                    item.Click += (sender, e) =>
                    {
                        if (KaydetmeyiKontrolEt())
                        {
                            if (s.EndsWith(".rtf")) richTextBox1.LoadFile(s);
                            else richTextBox1.Text = File.ReadAllText(s);
                            dosyaYolu = s; this.Text = s; SonAcilanlariGuncelle(s);
                            degisiklikVar = false;
                        }
                    };
                    sonAçılanlarToolStripMenuItem.DropDownItems.Add(item);
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (denetleniyor) return;

            degisiklikVar = true;
            if (!this.Text.EndsWith("*")) this.Text += "*";
            if (durumIslemBilgisi != null) durumIslemBilgisi.Text = "Düzenleniyor...";
            if (panelSatir != null) panelSatir.Invalidate();

            // kullanıcı boşluk tuşuna bastığında kelime bittiği için yazım denetimini tetikliyoruz
            if (richTextBox1.Text.EndsWith(" "))
            {
                YazimDenetimiYap();
            }
        }

        private bool KaydetmeyiKontrolEt()
        {
            if (!degisiklikVar) return true;
            DialogResult cevap = MessageBox.Show("Kaydedilmemiş değişiklikleriniz var. Kaydetmek ister misiniz?", "Uyarı", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (cevap == DialogResult.Yes) { kaydetToolStripMenuItem_Click(null, null); return !degisiklikVar; }
            else if (cevap == DialogResult.No) { degisiklikVar = false; return true; }
            return false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SonAcilanlariKaydet();
            if (!KaydetmeyiKontrolEt()) e.Cancel = true;
        }

        private void panelSatir_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(panelSatir.BackColor);
            e.Graphics.DrawLine(Pens.Gray, panelSatir.Width - 1, 0, panelSatir.Width - 1, panelSatir.Height);
            if (richTextBox1 == null) return;
            int ilkKarakter = richTextBox1.GetCharIndexFromPosition(new Point(0, 0));
            int ilkSatir = richTextBox1.GetLineFromCharIndex(ilkKarakter);
            int sonKarakter = richTextBox1.GetCharIndexFromPosition(new Point(0, richTextBox1.Height));
            int sonSatir = richTextBox1.GetLineFromCharIndex(sonKarakter);
            for (int i = ilkSatir; i <= sonSatir; i++)
            {
                int index = richTextBox1.GetFirstCharIndexFromLine(i);
                if (index >= 0)
                {
                    Point rtbPos = richTextBox1.GetPositionFromCharIndex(index);
                    Point ekranPos = richTextBox1.PointToScreen(rtbPos);
                    Point panelPos = panelSatir.PointToClient(ekranPos);
                    e.Graphics.DrawString((i + 1).ToString(), richTextBox1.Font, Brushes.DimGray, new PointF(5, panelPos.Y + 2));
                }
            }
        }

        private void yeniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!KaydetmeyiKontrolEt()) return;
            richTextBox1.Clear();
            this.Text = "Yeni Dosya - Enhanced Notepad";
            dosyaYolu = ""; degisiklikVar = false;
        }

        private void açToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!KaydetmeyiKontrolEt()) return;
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Metin Dosyaları|*.txt|RTF Dosyaları|*.rtf|Tüm Dosyalar|*.*" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.FileName.EndsWith(".rtf")) richTextBox1.LoadFile(ofd.FileName);
                else richTextBox1.Text = File.ReadAllText(ofd.FileName);
                this.Text = ofd.FileName; dosyaYolu = ofd.FileName; degisiklikVar = false;
                SonAcilanlariGuncelle(ofd.FileName);
            }
        }

        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(dosyaYolu))
            {
                SaveFileDialog sfd = new SaveFileDialog { Filter = "Metin Dosyası|*.txt" };
                if (sfd.ShowDialog() == DialogResult.OK) dosyaYolu = sfd.FileName;
                else return;
            }
            File.WriteAllText(dosyaYolu, richTextBox1.Text);
            degisiklikVar = false; this.Text = this.Text.TrimEnd('*');
            SonAcilanlariGuncelle(dosyaYolu);
        }

        private void farklıKaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "Metin Dosyası|*.txt|RTF Dosyası|*.rtf" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (sfd.FileName.EndsWith(".rtf")) richTextBox1.SaveFile(sfd.FileName);
                else File.WriteAllText(sfd.FileName, richTextBox1.Text);
                dosyaYolu = sfd.FileName; degisiklikVar = false; this.Text = this.Text.TrimEnd('*');
                SonAcilanlariGuncelle(sfd.FileName);
            }
        }

        private void sayfaYapısıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { PageSetupDialog ps = new PageSetupDialog { Document = printDocument1 }; ps.ShowDialog(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void yazdırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog { Document = printDocument1 };
            if (pd.ShowDialog() == DialogResult.OK) printDocument1.Print();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(richTextBox1.Text, richTextBox1.Font, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top);
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e) { printPreviewDialog1.Document = printDocument1; }

        private void yazdırÖnizlemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { printPreviewDialog1.Document = printDocument1; printPreviewDialog1.ShowDialog(); }
            catch { }
        }

        private void toolStripButton1_Click(object sender, EventArgs e) { yeniToolStripMenuItem_Click(sender, e); }
        private void toolStripButton2_Click(object sender, EventArgs e) { açToolStripMenuItem_Click(sender, e); }
        private void toolStripButton3_Click(object sender, EventArgs e) { kaydetToolStripMenuItem_Click(sender, e); }
        private void toolStripButton4_Click(object sender, EventArgs e) { yazdırToolStripMenuItem_Click(sender, e); }
        private void toolStripButton9_Click(object sender, EventArgs e) { richTextBox1.Cut(); }
        private void toolStripButton5_Click(object sender, EventArgs e) { richTextBox1.Copy(); }
        private void toolStripButton11_Click(object sender, EventArgs e) { richTextBox1.Paste(); }
        private void toolStripButton6_Click(object sender, EventArgs e) { if (richTextBox1.CanUndo) richTextBox1.Undo(); }
        private void toolStripButton7_Click(object sender, EventArgs e) { if (richTextBox1.CanRedo) richTextBox1.Redo(); }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            if (richTextBox1.BackColor == Color.White) { richTextBox1.BackColor = Color.FromArgb(30, 30, 30); richTextBox1.ForeColor = Color.White; }
            else { richTextBox1.BackColor = Color.White; richTextBox1.ForeColor = Color.Black; }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            string aranan = Microsoft.VisualBasic.Interaction.InputBox("Aranacak kelimeyi girin:", "Metin Bul", "");
            if (!string.IsNullOrEmpty(aranan))
            {
                int index = richTextBox1.Find(aranan);
                if (index > -1) { richTextBox1.Select(index, aranan.Length); richTextBox1.Focus(); }
                else MessageBox.Show("Bulunamadı.");
            }
        }

        private void solaYaslaToolStripMenuItem_Click(object sender, EventArgs e) { richTextBox1.SelectionAlignment = HorizontalAlignment.Left; }
        private void ortayaYaslaToolStripMenuItem_Click(object sender, EventArgs e) { richTextBox1.SelectionAlignment = HorizontalAlignment.Center; }
        private void sağaHizalaToolStripMenuItem_Click(object sender, EventArgs e) { richTextBox1.SelectionAlignment = HorizontalAlignment.Right; }
        private void solaHizalaToolStripMenuItem_Click(object sender, EventArgs e) { richTextBox1.SelectionAlignment = HorizontalAlignment.Left; }
        private void ortalaToolStripMenuItem_Click(object sender, EventArgs e) { richTextBox1.SelectionAlignment = HorizontalAlignment.Center; }
        private void toolStripMenuItem14_Click(object sender, EventArgs e) { richTextBox1.SelectionAlignment = HorizontalAlignment.Center; }
        private void kesToolStripMenuItem_Click(object sender, EventArgs e) { richTextBox1.Cut(); }
        private void kopyalaToolStripMenuItem_Click(object sender, EventArgs e) { richTextBox1.Copy(); }
        private void yapıştırToolStripMenuItem_Click(object sender, EventArgs e) { richTextBox1.Paste(); }
        private void geriAlToolStripMenuItem_Click(object sender, EventArgs e) { richTextBox1.Undo(); }
        private void yenileToolStripMenuItem_Click(object sender, EventArgs e) { richTextBox1.Redo(); }
        private void tarihSaatEkleToolStripMenuItem_Click(object sender, EventArgs e) { richTextBox1.AppendText("\n" + DateTime.Now.ToString()); }
        private void yazıTipiToolStripMenuItem_Click(object sender, EventArgs e) { if (fontDialog1.ShowDialog() == DialogResult.OK) richTextBox1.SelectionFont = fontDialog1.Font; }
        private void yazıRengiToolStripMenuItem_Click(object sender, EventArgs e) { if (colorDialog1.ShowDialog() == DialogResult.OK) richTextBox1.SelectionColor = colorDialog1.Color; }
        private void yakınlaştırUzaklaştırToolStripMenuItem_Click(object sender, EventArgs e) { richTextBox1.ZoomFactor += 0.1f; }
        private void uzaklaştırToolStripMenuItem_Click(object sender, EventArgs e) { if (richTextBox1.ZoomFactor > 0.5f) richTextBox1.ZoomFactor -= 0.1f; }
        private void kelimeKaydırmaToolStripMenuItem_Click(object sender, EventArgs e) { richTextBox1.WordWrap = !richTextBox1.WordWrap; }
        private void dosyaBilgileriToolStripMenuItem_Click(object sender, EventArgs e) { istatistiklerToolStripMenuItem_Click(sender, e); }

        private void istatistiklerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int karakterSayisi = richTextBox1.Text.Length;
            int kelimeSayisi = richTextBox1.Text.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
            int satirSayisi = richTextBox1.Lines.Length;

            string mesaj = $"--- Metin İstatistikleri ---\n" +
                   $"Karakter Sayısı: {karakterSayisi}\n" +
                   $"Kelime Sayısı: {kelimeSayisi}\n" +
                   $"Satır Sayısı: {satirSayisi}\n\n";

            if (!string.IsNullOrEmpty(dosyaYolu) && File.Exists(dosyaYolu))
            {
                FileInfo fi = new FileInfo(dosyaYolu);
                double boyutKB = fi.Length / 1024.0;

                mesaj += $"--- Dosya Bilgileri (FileInfo) ---\n" +
                     $"Dosya Adı: {fi.Name}\n" +
                     $"Boyut: {boyutKB:N2} KB\n" +
                     $"Oluşturulma: {fi.CreationTime}\n" +
                     $"Son Değiştirme: {fi.LastWriteTime}\n" +
                     $"Tam Yol: {fi.FullName}";
            }
            else
            {
                mesaj += "--- Dosya Bilgileri ---\nDosya henüz kaydedilmediği için sistem bilgileri mevcut değil.";
            }

            MessageBox.Show(mesaj, "Dosya ve Metin İstatistikleri", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            if (richTextBox1 == null) return;
            int i = richTextBox1.SelectionStart;
            int l = richTextBox1.GetLineFromCharIndex(i) + 1;
            int c = i - richTextBox1.GetFirstCharIndexOfCurrentLine() + 1;
            if (durumSatirSutun != null) durumSatirSutun.Text = $"Satır: {l}, Sütun: {c}  |  ";
            if (durumKarakterSayisi != null) durumKarakterSayisi.Text = $"{richTextBox1.Text.Length} karakter  |  ";
        }

        private void durumKarakterSayisi_Click(object sender, EventArgs e) { }
        private void timer1_Tick(object sender, EventArgs e) { kaydetToolStripMenuItem.PerformClick(); }
        private void Form1_Load_1(object sender, EventArgs e) { }
        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e) { this.Close(); }
        private void tümünüSeçToolStripMenuItem_Click(object sender, EventArgs e) { richTextBox1.SelectAll(); }

        private void gitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = Microsoft.VisualBasic.Interaction.InputBox("Satır:", "Git", "1");
            if (int.TryParse(s, out int n) && n > 0 && n <= richTextBox1.Lines.Length)
            {
                int idx = richTextBox1.GetFirstCharIndexFromLine(n - 1);
                richTextBox1.Select(idx, 0); richTextBox1.ScrollToCaret(); richTextBox1.Focus();
            }
        }

        private void maddeİşaretleriToolStripMenuItem_Click(object sender, EventArgs e) { richTextBox1.SelectionBullet = !richTextBox1.SelectionBullet; }

        private void büyükKüçükHarfDönüştürToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(richTextBox1.SelectedText))
            {
                if (richTextBox1.SelectedText == richTextBox1.SelectedText.ToUpper()) richTextBox1.SelectedText = richTextBox1.SelectedText.ToLower();
                else richTextBox1.SelectedText = richTextBox1.SelectedText.ToUpper();
            }
        }

        private void durumÇubuğuToolStripMenuItem_Click(object sender, EventArgs e) { statusStrip2.Visible = !statusStrip2.Visible; }
        private void araçÇubuğuToolStripMenuItem_Click(object sender, EventArgs e) { toolStrip1.Visible = !toolStrip1.Visible; }

        private void tamEkranModuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.FormBorderStyle == FormBorderStyle.None) { this.FormBorderStyle = FormBorderStyle.Sizable; this.WindowState = FormWindowState.Normal; }
            else { this.FormBorderStyle = FormBorderStyle.None; this.WindowState = FormWindowState.Maximized; }
        }

        private void hesapMakinesiToolStripMenuItem_Click(object sender, EventArgs e) { System.Diagnostics.Process.Start("calc.exe"); }

        private void temaSeçimiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog c = new ColorDialog();
            if (c.ShowDialog() == DialogResult.OK) richTextBox1.BackColor = c.Color;
        }

        private void otomatikKaydetmeAyarıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = Microsoft.VisualBasic.Interaction.InputBox("Dakika (Kapatmak için 0):", "Oto Kayıt", "5");
            if (int.TryParse(s, out int d))
            {
                if (d > 0) { timer1.Interval = d * 60000; timer1.Start(); }
                else timer1.Stop();
            }
        }

        private void nasılKullanılırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string kullanimRehberi =
                "📖 AKILLI NOT DEFTERİ - KULLANIM REHBERİ\n\n" +
                "📝 Temel İşlemler:\n" +
                "• Dosya menüsünden yeni notlar oluşturabilir, açabilir ve kaydedebilirsiniz.\n" +
                "• Sürükle-bırak özelliği ile bilgisayarınızdaki metin dosyalarını direkt ekrana taşıyabilirsiniz.\n\n" +
                "🔒 Güvenlik ve Şifreleme:\n" +
                "• Özel notlarınızı 'Şifreli Kaydet' özelliği ile belirlediğiniz bir parola ile kilitleyebilirsiniz.\n" +
                "• Bu dosyaları okumak için 'Şifreli Aç' seçeneğiyle doğru parolayı girmeniz gerekir.\n\n" +
                "🌐 Web / HTML Araçları:\n" +
                "• Metinlerinizi tek tıkla HTML etiketlerine dönüştürebilir veya karmaşık HTML kodlarını temizleyerek saf metne çevirebilirsiniz.\n\n" +
                "✔️ Otomatik Yazım Denetimi:\n" +
                "• Siz yazarken sistem kelimeleri denetler, sözlükte bulunmayan hatalı kelimelerin altını kırmızı ile çizer.\n\n" +
                "📊 İstatistik ve Görünüm:\n" +
                "• 'İstatistikler' menüsünden karakter/kelime sayısını ve dosya boyutunu görebilirsiniz.\n" +
                "• Karanlık mod (Dark Mode) ve tema seçenekleriyle gözünüzü yormadan çalışabilirsiniz.";

            MessageBox.Show(kullanimRehberi, "Nasıl Kullanılır?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string hakkindaMetni =
                "📌 PROJE BİLGİLERİ\n\n" +
                "Ders: Görsel Programlama\n" +
                "Proje Konusu: Akıllı Not Defteri Tasarımı ve Geliştirmesi\n\n" +
                "👨‍💻 Geliştirici Ekip: ÇÖZÜM EKİBİ\n" +
                "• Eme Deniz\n" +
                "• Serhat Yoldaş\n" +
                "• Rustam Asadli\n\n" +
                "Gelişmiş özelliklerle donatılmış bu proje, metin düzenleme ve dosya şifreleme gibi işlemleri daha güvenli ve pratik hale getirmek amacıyla tasarlanmıştır.\n\n" +
                "© 2026 - Tüm Hakları Saklıdır.";

            MessageBox.Show(hakkindaMetni, "Akıllı Not Defteri Hakkında", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void sonAçılanlarToolStripMenuItem_Click(object sender, EventArgs e) { }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift)
            {
                if (e.KeyValue == 190) { YazıBoyutunuDegistir(1); e.SuppressKeyPress = true; }
                else if (e.KeyValue == 188) { YazıBoyutunuDegistir(-1); e.SuppressKeyPress = true; }
            }
        }

        private void metniHTMLeÇevirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(richTextBox1.Text)) return;

            string htmlMetin = richTextBox1.Text;

            // HTML kodlarının bozulmaması için &, < ve > işaretlerini HTML'in anladığı formatlara çeviriyoruz
            htmlMetin = htmlMetin.Replace("&", "&amp;")
                                 .Replace("<", "&lt;")
                                 .Replace(">", "&gt;")
                                 .Replace("\n", "<br/>\n");

            // standart bir web sayfası iskeleti oluşturup dönüştürdüğümüz metni arasına koyuyoruz
            richTextBox1.Text = "<html>\n<body>\n" + htmlMetin + "\n</body>\n</html>";
        }

        private void hTMLiMetneÇevirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(richTextBox1.Text)) return;

            string metin = richTextBox1.Text;

            // HTML içindeki < > parantezlerini ve arasındaki her şeyi (kodları) silerek saf yazıyı buluyoruz
            while (metin.Contains("<") && metin.Contains(">"))
            {
                int baslangic = metin.IndexOf("<");
                int bitis = metin.IndexOf(">");

                // program sonsuz döngüye girip kilitlenmesin diye ufak bir güvenlik kontrolü
                if (bitis > baslangic)
                {
                    metin = metin.Remove(baslangic, bitis - baslangic + 1);
                }
                else
                {
                    break;
                }
            }

            // silme işleminden sonra kalan &lt; gibi HTML özel karakterlerini tekrar normal hallerine döndürüyoruz
            metin = metin.Replace("&lt;", "<")
                         .Replace("&gt;", ">")
                         .Replace("&amp;", "&")
                         .Replace("&nbsp;", " ");

            // Trim() ile yazının başında veya sonunda kalan fazla boşlukları atıp ekrana basıyoruz
            richTextBox1.Text = metin.Trim();
        }

        // ==========================================
        // şifreleme ve çözme işlemleri için metodlar
        // ==========================================

        private string SifreleCoz(string metin, string sifre)
        {
            if (string.IsNullOrEmpty(metin) || string.IsNullOrEmpty(sifre)) return metin;

            System.Text.StringBuilder sonuc = new System.Text.StringBuilder();

            // metnin içindeki her harfi, belirlediğimiz şifreyle XOR (^) işlemine sokup karmakarışık hale getiriyoruz
            for (int i = 0; i < metin.Length; i++)
            {
                sonuc.Append((char)(metin[i] ^ sifre[i % sifre.Length]));
            }

            return sonuc.ToString();
        }

        private void şifreliKaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // dosyayı kaydederken InputBox aracılığıyla bir parola istiyoruz
            string sifre = Microsoft.VisualBasic.Interaction.InputBox("Dosyayı korumak için bir şifre belirleyin:", "Şifreli Kaydet", "");

            if (!string.IsNullOrEmpty(sifre))
            {
                SaveFileDialog sfd = new SaveFileDialog { Filter = "Şifreli Dosya|*.sifre" };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // yazdığımız metni parolayla beraber SifreleCoz fonksiyonuna gönderip şifreliyoruz
                    string sifreliMetin = SifreleCoz(richTextBox1.Text, sifre);

                    // notepad gibi programlar garip karakterleri okurken dosyayı bozmasın diye veriyi Base64 dediğimiz güvenli bir formata çeviriyoruz
                    string kaydedilecek = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sifreliMetin));

                    File.WriteAllText(sfd.FileName, kaydedilecek);
                    MessageBox.Show("Dosyanız başarıyla şifrelendi ve kaydedildi!", "Güvenlik", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void şifreliAçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Şifreli Dosya|*.sifre" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // kilidi açmak için kaydederken belirlediğimiz parolayı soruyoruz
                string sifre = Microsoft.VisualBasic.Interaction.InputBox("Bu dosya şifreli! Lütfen şifreyi girin:", "Şifreli Aç", "");

                if (!string.IsNullOrEmpty(sifre))
                {
                    try
                    {
                        // kaydederken Base64'e çevirdiğimiz güvenli metni okuyup tekrar şifreli metne dönüştürüyoruz
                        string okunanBase64 = File.ReadAllText(ofd.FileName);
                        string sifreliMetin = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(okunanBase64));

                        // SifreleCoz fonksiyonu bu sefer şifreyi çözmek için çalışıyor ve okuduğumuz metni ekrana basıyor
                        richTextBox1.Text = SifreleCoz(sifreliMetin, sifre);
                        MessageBox.Show("Kilidi başarıyla açtınız!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        // try-catch kullandık ki adam yanlış şifre girerse program "hata verip kapanmasın", uyarı versin
                        MessageBox.Show("Hatalı şifre girdiniz veya dosya bozuk!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void şifreliKaydetToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // dosyayı kaydederken InputBox aracılığıyla bir parola istiyoruz
            string sifre = Microsoft.VisualBasic.Interaction.InputBox("Dosyayı korumak için bir şifre belirleyin:", "Şifreli Kaydet", "");

            if (!string.IsNullOrEmpty(sifre))
            {
                SaveFileDialog sfd = new SaveFileDialog { Filter = "Şifreli Dosya|*.sifre" };
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // yazdığımız metni parolayla beraber SifreleCoz fonksiyonuna gönderip şifreliyoruz
                    string sifreliMetin = SifreleCoz(richTextBox1.Text, sifre);

                    // notepad gibi programlar garip karakterleri okurken dosyayı bozmasın diye veriyi Base64 dediğimiz güvenli bir formata çeviriyoruz
                    string kaydedilecek = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sifreliMetin));

                    File.WriteAllText(sfd.FileName, kaydedilecek);
                    MessageBox.Show("Dosyanız başarıyla şifrelendi ve kaydedildi!", "Güvenlik", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void şifreliAçToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Şifreli Dosya|*.sifre" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // kilidi açmak için kaydederken belirlediğimiz parolayı soruyoruz
                string sifre = Microsoft.VisualBasic.Interaction.InputBox("Bu dosya şifreli! Lütfen şifreyi girin:", "Şifreli Aç", "");

                if (!string.IsNullOrEmpty(sifre))
                {
                    try
                    {
                        // kaydederken Base64'e çevirdiğimiz güvenli metni okuyup tekrar şifreli metne dönüştürüyoruz
                        string okunanBase64 = File.ReadAllText(ofd.FileName);
                        string sifreliMetin = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(okunanBase64));

                        // SifreleCoz fonksiyonu bu sefer şifreyi çözmek için çalışıyor ve okuduğumuz metni ekrana basıyor
                        richTextBox1.Text = SifreleCoz(sifreliMetin, sifre);
                        MessageBox.Show("Kilidi başarıyla açtınız!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        // try-catch kullandık ki adam yanlış şifre girerse program "hata verip kapanmasın", uyarı versin
                        MessageBox.Show("Hatalı şifre girdiniz veya dosya bozuk!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void bulVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Parantez içine ana formundaki yazı yazdığın alanın adını ekliyoruz.
            // Genellikle standart adı richTextBox1'dir.
            BulDegistirForm bulFormu = new BulDegistirForm(richTextBox1);
            bulFormu.Show();
        }
    }
}