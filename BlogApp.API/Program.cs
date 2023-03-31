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
// sadece api oldu�u i�in AddControllers service eklemek.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region IdentityDbContext

builder.Services.AddDbContext<AppIdentityDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("BlogContextConn")));

#endregion


#region SwaggerAuthenticationModule

builder.Services.AddSwaggerGen(opt =>
{
  // optional parametre ayar�
  opt.OperationFilter<OptionalRouteParameterOperationFilter>();
  opt.EnableAnnotations();

  // authentication test ayar�
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

// Api uygulamas�n�n JWT Token generate etmesini sa�l�yaca��z.

#region JWT

var key = Encoding.ASCII.GetBytes(JWTSettings.SecretKey);
// uygulamadan bir JWT bazl� kimlik do�rulamas� yap�lacak.
builder.Services.AddAuthentication(x =>
{
  // Bu y�nteme  Authorization Bearer
  // Login oluca��m�z zaman Scheme dedi�imiz farkl� �emalar ile birden fazla farkl� alt yap� ile login olabiliriz.
  x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
            .AddJwtBearer(x =>
            {
              // JWT ayarlar�


              x.RequireHttpsMetadata = false;
              x.SaveToken = true; // Access Token In-Memory uygulam�n saklamas� i�in
              x.TokenValidationParameters = new TokenValidationParameters
              {
                // JWT Token ne gibi durumlara g�re valid say�lcak
                ValidateIssuerSigningKey = true, //JWTSettings.SecretKey �nemli
                IssuerSigningKey = new SymmetricSecurityKey(key), 
                ValidateIssuer = false, // JWT olu�turan servis (identityservice)
                ValidateAudience = false, // (JWT t�keticek servic (resource1))
                // Kat� kurallar ile JWT validayonlar� uygulamak i�in Audience ve Issuer de�erini kullanabiliriz.
                ValidateLifetime = true, // Expire olma s�resi �nemli
              };

            });


// interface �zerinden JwtTokenService ile s�recemizi y�netece�iz.
builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();


#endregion


#region Identity


builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
{
  opt.SignIn.RequireConfirmedEmail = false; // email confirm etmeden login etsin
  opt.User.RequireUniqueEmail = true; // email unique olmal�
  //opt.Password.RequireDigit = false;
  //opt.Password.RequireLowercase = false;
  opt.Password.RequireDigit = true; // parolada numeric alan olaml� gibi ayarlar� yapabiliyoruz.

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
