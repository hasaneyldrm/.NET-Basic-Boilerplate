# .NET Basic Boilerplate

**Bu boilerplate kodunda neler var?**

Hepimiz yeni bir projeye başlarken çok basit bir boilerplate koduna ihtiyaç duyuyoruz. Bu repoda; JWT Token ile auth sistemi, MySQL connectionu, basic CRUD yapısını içeriyor. Ek olarak IP kısıtlaması, genel güvenlik önlemleri ve basit ve kullanışlı bir mimarı kurulu. 

Yeni projeye başlarken Token, Auth, DB connectionu ve Temel CRUD işlemleri ile uğraşmamanızı sağlamasını umuyorum.

Geliştirebileceğimiz yerleri TODO veya summary olarak yazılı. Altyapı hazır olduğu için düzenlemek çok zaman almayacaktır.

![image](https://github.com/hasaneyldrm/.NET-Basic-Boilerplate/assets/28005450/ffc29d79-05f5-4b80-b808-1db78d26a694)



**Nasıl kullanılır? - Konfigürasyon ayarları**

Öncelikle Appsettings altındaki değerleri dolduruyoruz.

**MySQLConnection**: MySQL Database'ine bağlanmamız için gerekli id pass ve temel bilgileri dinamik olarak yönettiğimiz alan oluyor kendileri.

**AppSettings Secret Keyi:** Token alışverişi yaparken kullandığımız secret keydir. Herhangi bir salt verilebilir.

![image](https://github.com/hasaneyldrm/.NET-Basic-Boilerplate/assets/28005450/a93c7c9f-a5fd-4b1a-a951-2910e9783b92)


**HashId Nedir? - Niye kullanırız?**

HashId sistemi, verdiğiniz key'e göre sizlere unique code üreten ufak bir kütüphanedir. Bu sayede JWT Token'larda, apileriniz arasında konuşurken, vereceğiniz herhangi bir veride gönlünüzün rahat olmasını sağlar.
Eğer telefon kameralarını bantlayan güvenlik aşığı bir paranoyaksanız saltları sürekli değiştirerek çözülmesi imkansız sonuçlar oluşturabilirsiniz veya Bora kaşmer'in "JWT Token güvenli değildir" söylemini aşabilirsiniz.

![image](https://github.com/hasaneyldrm/.NET-Basic-Boilerplate/assets/28005450/e5762bd3-ae53-4f69-887e-b864172c9db9)

Detaylı bilgi: https://github.com/hashids 


[Uploading boilerPlate.sql…]()

**Nasıl konfigüre ederiz?**

Kodun içine ufak ve tatlı "yoursalt" alanları bıraktım. Folderlardaki Utilities -> HashCode alanında yourSalt alanına istediğiniz unique değeri verebilirsiniz. Verdiğiniz salt'a göre sistem kendi hashidlerini oluşturur.

![image](https://github.com/hasaneyldrm/.NET-Basic-Boilerplate/assets/28005450/2606f9ce-db98-40f2-bfa9-2c84bb505380)


İşinize yaraması için son olarak da Postman collectionu ve user tablosunun sql dump'ını atıyorum.

MySQL Dumpı: https://drive.google.com/file/d/14g8yFb_oDfBTwvrYl7BhbarUaL1Ev4QU/view?usp=sharing
Postman Collectionu: https://drive.google.com/file/d/1Sv1Z58xS-Oc8sbTePso-ChUdLWiiE9zR/view?usp=sharing

Sevgilerle,
