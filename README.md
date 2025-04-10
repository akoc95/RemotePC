<h1>RemotePC</h1>

<h2>📱 Uygulama Nedir?</h2>
<p>
RemotePC, Android telefonunuzdan aynı ağda bulunan bilgisayarınıza basit uzaktan komutlar gönderebilmenizi sağlar.
Ses açma/kapatma, bilgisayar kapatma ve URL açma (örneğin YouTube, Spotify gibi) gibi işlemleri kolaylıkla gerçekleştirebilirsiniz.
</p>

<h2>⚙️ Teknoloji ve Geliştirme Notları</h2>
<p>
Bu uygulamayı <strong>.NET MAUI</strong> kullanarak yaklaşık <strong>20 dakika</strong> gibi kısa bir sürede geliştirdim.
Dolayısıyla kapsamı sınırlıdır; ancak kaynak kod açık olduğundan dilediğiniz gibi geliştirip özelleştirebilirsiniz.
</p>
<ul>
  <li>Sesi tamamen kapatma veya tarayıcı kapatma gibi işlemleri kolayca entegre edebilirsiniz.</li>
  <li>Kullanıcı arayüzünü (UI) kendi zevkinize göre değiştirebilirsiniz.</li>
</ul>

<h2>🆕 Güncelleme</h2>
<p>
UI tarafında bazı basit iyileştirmeler yapıldı. Ayrıca dokunmatik ekran üzerinden sürükleme hareketleri algılanarak,
arka plandaki TCP bağlantısı üzerinden ilgili komutlar masaüstü uygulamasına iletiliyor.
</p>
<p>
Kullanıcı etkileşimlerini görselleştirmek adına, dokunma sırasında ekranda iz bırakma (trail effect) animasyonu eklendi.
</p>

<h2>🚀 Temel Özellikler</h2>
<ul>
  <li>Dokunmatik hareket algılama (drag detection)</li>
  <li>Ekranda kaybolan iz efekti (fade-out animation)</li>
  <li>TCP üzerinden komut gönderimi (örnek: <code>mouse_move</code>, <code>mouse_click</code>)</li>
  <li>Sol ve sağ tıklama butonları</li>
  <li>Gerçek zamanlı ve performanslı çizim</li>
</ul>
