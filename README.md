# RemotePC

Bu uygulama ne yapıyor?

Aynı ağda bulunan bilgisayarınızın Android telefonunuz üzerinden ses açma, kapatma, bilgisayar kapatma ve url açma(YouTube, Spotify vs) gibi basit işleri yapıyor. 

Uygulamayı .NET MAUI ile yaklaşık 20 dakika gibi kısa bir sürede yazdım, bu yüzden kapsamı sınırlı. 

Sesi tamamen kapatma, tarayıcı kapama gibi ek işlemi ihtiyaçlarınıza göre uyarlayabilirsiniz. UI tarafını da dilediğiniz gibi düzenleyebilirsiniz.

Güncelleme
UI Tarafında basit düzeltmeler ve dokunmatik ekran üzerinden yapılan sürükleme hareketlerini yakalayıp, arka planda bir TCP bağlantısı aracılığıyla ilgili komutu masaüstüne gönderiyor. Aynı zamanda kullanıcı etkileşimlerini görselleştirmek için ekranda kaybolan bir iz (trail effect) animasyonu kullanıyorum. 

Temel Özellikler:
Dokunmatik hareketi algılama (drag detection)
Ekranda iz bırakma (fade-out animation ile)
TCP üzerinden komut gönderimi (mouse_move, mouse_click)
Sağ ve sol tıklama butonları
Gerçek zamanlı ve performanslı çizim
