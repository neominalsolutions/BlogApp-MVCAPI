using BlogApp.Application.Services;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Repositories;
using BlogApp.Domain.Services;
using BlogApp.Infra.Persistance.EF.Repositories;
using BlogApp.Middlewares;
using BlogApp.Persistance.EF.Contexts;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BlogAppContext>(opt => opt.UseSqlServer( builder.Configuration.GetConnectionString("BlogContextConn")));


// IoC container yani merkezi olarak uygulama kullanýlacak olan tüm nesnelerin instance bu dosya üzerinden IServiceColletion yapýsý ile yönetiliyor.

// service register iþlemi yapýyoruz.
// scoped service web programlarý için tanýmlanmýþ web request bazlý instance oluþturlmasýna olanak saðlayan bir service lifetime scope hizmet.
// buradaki sýnýflar ne kadarlýk bir zaman aralýðýnda uygulamada instancelarý kullanýlacak.
// unmanagement resource api call, db call, upload gibi operasyonlarda 
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ICreatePostRequestService, CreatePostRequestService>();
builder.Services.AddScoped<IPostDomainService, PostDomainService>();

// Login Middleware uygulama ayaða kalktýðýnda çalýþtýrmak için IMiddeware yönteminde bu þekilde tanýmlýyoruz.
builder.Services.AddTransient<LoggingMiddleware>();

// Post isteði yaptýðým herhangi bir yerde constructor üzerinden post'un boþ instance halini alabilirim
// builder.Services.AddTransient<Post>(); interface olmadan sadece class yazýlarak da kullanýlýyor.

// not eðer servislerini repostory service ile birlikte kullanýrsanýz bu durumda scoped service tercih ediniz

// her bir çaðýrýmda instance almamýz gereken sýnýflarýmýz varsa bu durumda tercih ederiz. Factory pattern kullanan sýnýflarda kullanýlabilir, Yada Session, Valiation gibi her bir instance birbirinden farklý olmasý gereken durumlada tercih edilir.
//builder.Services.AddTransient<>();

// Uygulama açýlýdýðý an itibari ile single instance çalýþýr. Performans amaçlý kullanýlan bir teknik. Singleton Design Pattern kullanýr.
// Utils, Helper servicler single instance tanýmlanmalýdýr, NotificationManager.notify(),DateTimeHelper.getPrettyDate(); Math.Min();
//builder.Services.AddSingleton();


#region APIConnectionHTTPClient

builder.Services.AddHttpClient("apiClient", opt =>
{
  // Api hangi domainden ayaða kalkacak ise buraya o ismi yazýrz
  // www.a.api.com
  opt.BaseAddress =  new Uri("https://localhost:5001");
});

#endregion

#region CookieBasedAuthentication

builder.Services.AddAuthentication(x =>
{
  // Cookies denlen bir þema ile 
  x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
  x.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(x => {

  x.LoginPath = "/Account/Login";
  x.LogoutPath = "/Account/LogOut";
  x.SlidingExpiration = true;
  x.ExpireTimeSpan = TimeSpan.FromDays(1);
  
  
  });

#endregion



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error"); // canlýda hata sayfasýna yönlendirir.
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();// gelen bir http isteðini https'e yönlendiren middleware
app.UseStaticFiles(); // www root altýndali dosyalarý public yapan middleware, wwwroot altýnda olmayan bir dosya çalýþma esnasýnda wwwroot dosyasý aldýndaymýþ gibi virtual directory açýlabilir.

app.UseRouting(); // {Controller/Action/id} formatýnda gelen isteklerin yönelndirimesini saðlar.
app.UseAuthentication(); // MVC cookie based auth.
app.UseAuthorization(); // [Authorize] attribute kullanýcý yetki kontrolün cookie üzerindeki claimler ile yönetir.

app.MapControllerRoute( // sayfa ilk açlýþýnda sayfanýn Home/Index yönlendirilmesini saðlar.
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// useMiddleware => use ile baþlayan middlewarelere süreci bir sonraki adým devreder bunýu next methodu ile yapar.
// runMiddleware var (operasyonu yapar ve artýk client response döner.) Next ile süreci bir sonraki adýma geçirmez.


// kendi custom middleware yapýmýzý sürece dahil ettik.
//app.UseMiddleware<ClientCredentialRequestMiddleware>();

// 2. hali.
app.UseClientCrendential();
app.UseLogging();


// her bir istek de burasý defalarca kez kontrol edilecek.
// ara bir yazýlým geliþtiriken useMiddleware tercih ederiz.
//app.Use(async (context, next) =>
//{



//  string controllerName = context.Request.RouteValues["Controller"].ToString();
//  string actionName = context.Request.RouteValues["Action"].ToString();



//  if (controllerName == "Home" && actionName == "Privacy")
//  {
//    // süreci bir sonraki adýma taþýr
//    // context HttpContext
//    // next requestDelegate

//    // header'a tr deðeri ekleme
//    context.Request.Headers.Append("lang", "tr-TR");


//    //
//    if(context.Request.Method == HttpMethod.Get.Method)
//    {
//      // sadece Get isteklerinde ara gir
//    }

//    //await next();
//    await context.Response.WriteAsync("<h1>Privacy Page</h1>");

//  }


//  await next();

//});


// en sona yazýlmalý buradan sonra gibi middleware çalýþmaz
app.Run(); // uygulamayý çalýþtýr.

// use middlewareler run altýna yazýlmaz
//app.Use(async (context, next) =>
//{

//  await next();
//});
