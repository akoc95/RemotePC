# RemotePC

Bu uygulama ne yapıyor?

Aynı ağda bulunan bilgisayarınızın Android telefonunuz üzerinden ses açma, kapatma, bilgisayar kapatma ve youtube ile url açma gibi basit işleri yapıyor. 
Uygulamayı yaklaşık 20 dakika gibi kısa bir sürede yazdım, bu yüzden kapsamı sınırlıdır.
Sesi tamamen kapatma, tarayıcı kapama gibi ek işlemi ihtiyaçlarınıza göre uyarlayabilirsiniz. UI tarafını da dilediğiniz gibi düzenleyebilirsiniz.

Nasıl çalışır?
TCPListener uygulamasını ilgili cihazda çalıştırın.
Android uygulamasında cihazın(bilgisayar) IP adresini ve açık portu giriniz. Girilen portun açık olduğundan emin olun.
TCPListener açık değilse, komutları yakalayamaz.

Not: Bilgisayar ve Cep telefonunuz aynı ağda olmalı.

İsteğe bağlı olarak PR gönderebilirsiniz.

Ayrıca, TCPListener yerine cihazınıza uzaktan erişim açarsanız ara yazılıma gerek kalmadan doğrudan komutları gönderebilirsiniz. Güvenlik sebepleriyle o şekilde yazmadım.
