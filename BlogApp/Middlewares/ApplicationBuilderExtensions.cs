namespace BlogApp.Middlewares
{
  public static class ApplicationBuilderExtensions
  {
    // static tanımlanırlar
    // uygulamadaki bir tipi extende edip yeni özellikler kazanmasını sağlar.
    // this keyword arkasına gelen değeri IApplicationBuilder extend edilecek yani yeni özellikler eklenecek şekilde tanımladık.
    public static IApplicationBuilder UseClientCrendential(this IApplicationBuilder app)
    {
      return app.UseMiddleware<ClientCredentialRequestMiddleware>();
    }

    public static IApplicationBuilder UseLogging(this IApplicationBuilder app)
    {
      return app.UseMiddleware<LoggingMiddleware>();
    }
  }
}
