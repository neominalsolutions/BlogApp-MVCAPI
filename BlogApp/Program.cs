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


// IoC container yani merkezi olarak uygulama kullan�lacak olan t�m nesnelerin instance bu dosya �zerinden IServiceColletion yap�s� ile y�netiliyor.

// service register i�lemi yap�yoruz.
// scoped service web programlar� i�in tan�mlanm�� web request bazl� instance olu�turlmas�na olanak sa�layan bir service lifetime scope hizmet.
// buradaki s�n�flar ne kadarl�k bir zaman aral���nda uygulamada instancelar� kullan�lacak.
// unmanagement resource api call, db call, upload gibi operasyonlarda 
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ICreatePostRequestService, CreatePostRequestService>();
builder.Services.AddScoped<IPostDomainService, PostDomainService>();

// Login Middleware uygulama aya�a kalkt���nda �al��t�rmak i�in IMiddeware y�nteminde bu �ekilde tan�ml�yoruz.
builder.Services.AddTransient<LoggingMiddleware>();

// Post iste�i yapt���m herhangi bir yerde constructor �zerinden post'un bo� instance halini alabilirim
// builder.Services.AddTransient<Post>(); interface olmadan sadece class yaz�larak da kullan�l�yor.

// not e�er servislerini repostory service ile birlikte kullan�rsan�z bu durumda scoped service tercih ediniz

// her bir �a��r�mda instance almam�z gereken s�n�flar�m�z varsa bu durumda tercih ederiz. Factory pattern kullanan s�n�flarda kullan�labilir, Yada Session, Valiation gibi her bir instance birbirinden farkl� olmas� gereken durumlada tercih edilir.
//builder.Services.AddTransient<>();

// Uygulama a��l�d��� an itibari ile single instance �al���r. Performans ama�l� kullan�lan bir teknik. Singleton Design Pattern kullan�r.
// Utils, Helper servicler single instance tan�mlanmal�d�r, NotificationManager.notify(),DateTimeHelper.getPrettyDate(); Math.Min();
//builder.Services.AddSingleton();


#region APIConnectionHTTPClient

builder.Services.AddHttpClient("apiClient", opt =>
{
  // Api hangi domainden aya�a kalkacak ise buraya o ismi yaz�rz
  // www.a.api.com
  opt.BaseAddress =  new Uri("https://localhost:5001");
});

#endregion

#region CookieBasedAuthentication

builder.Services.AddAuthentication(x =>
{
  // Cookies denlen bir �ema ile 
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
  app.UseExceptionHandler("/Home/Error"); // canl�da hata sayfas�na y�nlendirir.
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();// gelen bir http iste�ini https'e y�nlendiren middleware
app.UseStaticFiles(); // www root alt�ndali dosyalar� public yapan middleware, wwwroot alt�nda olmayan bir dosya �al��ma esnas�nda wwwroot dosyas� ald�ndaym�� gibi virtual directory a��labilir.

app.UseRouting(); // {Controller/Action/id} format�nda gelen isteklerin y�nelndirimesini sa�lar.
app.UseAuthentication(); // MVC cookie based auth.
app.UseAuthorization(); // [Authorize] attribute kullan�c� yetki kontrol�n cookie �zerindeki claimler ile y�netir.

app.MapControllerRoute( // sayfa ilk a�l���nda sayfan�n Home/Index y�nlendirilmesini sa�lar.
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// useMiddleware => use ile ba�layan middlewarelere s�reci bir sonraki ad�m devreder bun�u next methodu ile yapar.
// runMiddleware var (operasyonu yapar ve art�k client response d�ner.) Next ile s�reci bir sonraki ad�ma ge�irmez.


// kendi custom middleware yap�m�z� s�rece dahil ettik.
//app.UseMiddleware<ClientCredentialRequestMiddleware>();

// 2. hali.
app.UseClientCrendential();
app.UseLogging();


// her bir istek de buras� defalarca kez kontrol edilecek.
// ara bir yaz�l�m geli�tiriken useMiddleware tercih ederiz.
//app.Use(async (context, next) =>
//{



//  string controllerName = context.Request.RouteValues["Controller"].ToString();
//  string actionName = context.Request.RouteValues["Action"].ToString();



//  if (controllerName == "Home" && actionName == "Privacy")
//  {
//    // s�reci bir sonraki ad�ma ta��r
//    // context HttpContext
//    // next requestDelegate

//    // header'a tr de�eri ekleme
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


// en sona yaz�lmal� buradan sonra gibi middleware �al��maz
app.Run(); // uygulamay� �al��t�r.

// use middlewareler run alt�na yaz�lmaz
//app.Use(async (context, next) =>
//{

//  await next();
//});
