using BlogApp.API.Jwt;
using BlogApp.API.SwaggerFilters;
using BlogApp.Persistance.EF.Contexts;
using BlogApp.Persistance.EF.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// sadece api olduðu için AddControllers service eklemek.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region IdentityDbContext

builder.Services.AddDbContext<AppIdentityDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("BlogContextConn")));

#endregion


#region SwaggerAuthenticationModule

builder.Services.AddSwaggerGen(opt =>
{
  // optional parametre ayarý
  opt.OperationFilter<OptionalRouteParameterOperationFilter>();
  opt.EnableAnnotations();

  // authentication test ayarý
  var securityScheme = new OpenApiSecurityScheme()
  {
    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.Http,
    Scheme = "Bearer",
    BearerFormat = "JWT" // Optional
  };

  var securityRequirement = new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "bearerAuth"
            }
        },
        new string[] {}
    }
};

  opt.AddSecurityDefinition("bearerAuth", securityScheme);
  opt.AddSecurityRequirement(securityRequirement);
});

#endregion

// Api uygulamasýnýn JWT Token generate etmesini saðlýyacaðýz.

#region JWT

var key = Encoding.ASCII.GetBytes(JWTSettings.SecretKey);
// uygulamadan bir JWT bazlý kimlik doðrulamasý yapýlacak.
builder.Services.AddAuthentication(x =>
{
  // Bu yönteme  Authorization Bearer
  // Login olucaðýmýz zaman Scheme dediðimiz farklý þemalar ile birden fazla farklý alt yapý ile login olabiliriz.
  x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
            .AddJwtBearer(x =>
            {
              // JWT ayarlarý


              x.RequireHttpsMetadata = false;
              x.SaveToken = true; // Access Token In-Memory uygulamýn saklamasý için
              x.TokenValidationParameters = new TokenValidationParameters
              {
                // JWT Token ne gibi durumlara göre valid sayýlcak
                ValidateIssuerSigningKey = true, //JWTSettings.SecretKey önemli
                IssuerSigningKey = new SymmetricSecurityKey(key), 
                ValidateIssuer = false, // JWT oluþturan servis (identityservice)
                ValidateAudience = false, // (JWT tüketicek servic (resource1))
                // Katý kurallar ile JWT validayonlarý uygulamak için Audience ve Issuer deðerini kullanabiliriz.
                ValidateLifetime = true, // Expire olma süresi önemli
              };

            });


// interface üzerinden JwtTokenService ile sürecemizi yöneteceðiz.
builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();


#endregion


#region Identity


builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
{
  opt.SignIn.RequireConfirmedEmail = false; // email confirm etmeden login etsin
  opt.User.RequireUniqueEmail = true; // email unique olmalý
  //opt.Password.RequireDigit = false;
  //opt.Password.RequireLowercase = false;
  opt.Password.RequireDigit = true; // parolada numeric alan olamlý gibi ayarlarý yapabiliyoruz.

}).AddEntityFrameworkStores<AppIdentityDbContext>();


#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Authentication Middleware aktif hale getirdik.
app.UseAuthorization();



// gelen endpoint route isteklerini controllera maple.
app.MapControllers();

app.Run();
