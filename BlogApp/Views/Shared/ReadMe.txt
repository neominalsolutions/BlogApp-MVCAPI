Layout'a dinamik veri çekerken Layout dosyasının herhangi bir backend'i olmadığından zaten eski mvc yönteminde RenderPartial şimdi ise ViewComponent denen yapıları çalıştırmamız gerekiyor.

UI kodunu parçalara ayırarak UI daha efektif yönetilmesini sağlıyoruz.

Veri ile alakalı gereksinimleri bir arayüz içerisinde göstermek için, bu işlemin viewden izole çalışması için viewComponent tercih ederiz.

// BlogListViewComponent
// BlogCommentsViewComponent
// AddBlogFormViewComponent
// PostCategoryViewComponent

Yeni verisyonda performans amaçlı partial with modal yerine burada ViewComponent



Arayüzden daha atomik olan yapıları, element düzeyinde uygulama içerisinde birçok yerde kullanabilmek için ise Tag Helper dediğimiz yapıları geliştirir. ButtonTagHelper, AlertTagHelper, SummaryTagHelper


Not: TagHelper ile çalışırken yada viewComponentleri Tag Helper olarak kullanmak istediğimizde viewİmports dosyasını güncelleyelim ve projeyi build edelim. 