# 📝 Akıllı Not Defteri (Enhanced Notepad)

**Görsel Programlama** dersi kapsamında geliştirilen, Windows Forms tabanlı gelişmiş bir metin düzenleme uygulaması. Klasik Not Defteri'nin ötesinde, dosya şifreleme, yazım denetimi, HTML dönüştürme ve daha birçok akıllı özellik sunar.

---

## 🚀 Özellikler

### 📄 Dosya İşlemleri
- **Yeni / Aç / Kaydet / Farklı Kaydet** — Temel dosya yönetimi (`Ctrl+N`, `Ctrl+O`, `Ctrl+S`)
- **Son Açılanlar** — En son açılan 5 dosyayı hatırlar ve hızlı erişim sunar
- **Sürükle-Bırak** — Dosyaları doğrudan editöre sürükleyerek açabilirsiniz
- **RTF Desteği** — `.txt` dosyalarının yanı sıra `.rtf` (Rich Text Format) dosyalarını okur ve yazar
- **Otomatik Kaydetme** — Belirlenen süre aralığıyla arka planda otomatik kayıt (dakika olarak ayarlanabilir)

### 🔒 Güvenlik & Şifreleme
- **Şifreli Kaydet** — Metinleri XOR tabanlı şifreleme + Base64 kodlamayla `.sifre` uzantısında korur
- **Şifreli Aç** — Şifreli dosyaları parola ile çözümler

### ✏️ Düzenleme Araçları
- **Kes / Kopyala / Yapıştır / Geri Al / Yinele** — Standart düzenleme işlemleri
- **Bul ve Değiştir** — Ayrı pencerede açılan gelişmiş arama; büyük/küçük harf duyarlılığı ve tam kelime eşleştirme seçenekleri
- **Satıra Git** — Belirli bir satır numarasına hızlı atlama
- **Tarih/Saat Ekle** — İmleç konumuna anlık zaman damgası ekler
- **Tümünü Seç** (`Ctrl+A`)
- **Büyük/Küçük Harf Dönüştür** — Seçili metni tek tıkla büyük ↔ küçük harfe çevirir

### 🎨 Biçimlendirme
- **Yazı Tipi & Renk** — Font ve renk diyalogları ile özelleştirme
- **Metin Hizalama** — Sola, ortaya ve sağa hizalama
- **Madde İşaretleri** — Seçili metni madde listesine çevirir
- **Girinti Ayarları** — Sol / sağ / ilk satır girintisi kontrolü
- **Yazı Boyutu Kısayolları** — `Ctrl+Shift+>` ve `Ctrl+Shift+<` ile anlık büyültme/küçültme

### 👁️ Görünüm
- **Karanlık Mod** — Araç çubuğundaki hızlı ayar butonu ile tek tıkla açılır/kapanır
- **Tema Seçimi** — Arka plan rengini istediğiniz renge ayarlayabilirsiniz
- **Yakınlaştır / Uzaklaştır** — Zoom kontrolü
- **Tam Ekran Modu** — Odaklanmak için kenarsız tam ekran
- **Araç Çubuğu & Durum Çubuğu Görünürlüğü** — İsteğe bağlı olarak gizlenebilir
- **Kelime Kaydırma** — Açılıp kapatılabilir

### 📊 İstatistik & Bilgi
- **Satır Numaraları** — Editörün sol tarafında dinamik satır numarası paneli
- **Durum Çubuğu** — Anlık olarak satır/sütun pozisyonu, karakter sayısı, kodlama (UTF-8) ve işlem durumunu gösterir
- **Dosya İstatistikleri** — Karakter, kelime, satır sayısı ve dosya bilgileri (boyut, oluşturma/değiştirme tarihi)

### ✔️ Yazım Denetimi
- `kelimeler.txt` sözlük dosyası ile çalışan gerçek zamanlı yazım denetimi
- Sözlükte olmayan kelimeler otomatik olarak **kırmızı** renk ile işaretlenir

### 🌐 Web / HTML Araçları
- **Metni HTML'e Çevir** — Düz metni standart HTML iskeleti içine dönüştürür
- **HTML'i Metne Çevir** — HTML etiketlerini temizleyerek saf metin çıkarır

### 🖨️ Yazdırma
- **Yazdır** (`Ctrl+P`) — Bağlı yazıcıya doğrudan çıktı
- **Yazdırma Önizleme** — Baskı öncesi görsel kontrol
- **Sayfa Yapısı** — Kenar boşlukları ve sayfa düzeni ayarları

### 🧮 Ek Araçlar
- **Hesap Makinesi** — Windows Hesap Makinesi'ni uygulama içinden başlatır

---

## 🛠️ Teknolojiler

| Bileşen | Teknoloji |
|---|---|
| **Platform** | .NET 8.0 (Windows) |
| **UI Framework** | Windows Forms (WinForms) |
| **Dil** | C# |
| **IDE** | Visual Studio 2022 (v17.14+) |

---

## 📁 Proje Yapısı

```
NotDefteri/
├── NotDefteri.sln                 # Visual Studio çözüm dosyası
├── NotDefteri.csproj              # Proje yapılandırma dosyası
├── Program.cs                     # Uygulama giriş noktası
│
├── Form1.cs                       # Ana form - tüm özellikler burada
├── Form1.Designer.cs              # Ana formun görsel tasarım kodu
├── Form1.resx                     # Ana form kaynakları (ikonlar vb.)
│
├── BulDegistirForm.cs             # Bul ve Değiştir penceresi
├── BulDegistirForm.Designer.cs    # Bul/Değiştir formunun tasarım kodu
├── BulDegistirForm.resx           # Bul/Değiştir form kaynakları
│
├── Properties/
│   ├── Resources.Designer.cs      # Otomatik üretilen kaynak dosyası
│   └── Resources.resx             # Proje kaynakları
│
├── kelimeler.txt                  # (Opsiyonel) Yazım denetimi sözlüğü
└── README.md                      # Bu dosya
```

---

## ⚙️ Kurulum ve Çalıştırma

### Gereksinimler
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) veya üzeri
- Windows işletim sistemi

### Komut Satırından
```bash
# Projeyi klonlayın
git clone https://github.com/<kullanici-adi>/NotDefteri.git
cd NotDefteri

# Derleyin ve çalıştırın
dotnet run
```

### Visual Studio ile
1. `NotDefteri.sln` dosyasını Visual Studio 2022 ile açın
2. **F5** tuşuna basarak Debug modda çalıştırın

---

## ⌨️ Klavye Kısayolları

| Kısayol | İşlev |
|---|---|
| `Ctrl + N` | Yeni dosya |
| `Ctrl + O` | Dosya aç |
| `Ctrl + S` | Kaydet |
| `Ctrl + P` | Yazdır |
| `Ctrl + A` | Tümünü seç |
| `Ctrl + X` | Kes |
| `Ctrl + C` | Kopyala |
| `Ctrl + V` | Yapıştır |
| `Ctrl + Z` | Geri al |
| `Ctrl + Y` | Yinele |
| `Ctrl + Shift + >` | Yazı boyutunu büyüt |
| `Ctrl + Shift + <` | Yazı boyutunu küçült |

---

## 👨‍💻 Geliştirici Ekip — ÇÖZÜM EKİBİ

| İsim |
|---|
| Emre Deniz |
| Serhat Yoldaş |
| Rustam Asadli |

---

## 📄 Lisans

© 2026 — Tüm Hakları Saklıdır.

> Bu proje **Görsel Programlama** dersi için geliştirilmiştir.
