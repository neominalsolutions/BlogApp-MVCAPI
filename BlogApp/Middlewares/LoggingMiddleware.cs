namespace BlogApp.Middlewares
{
  public class LoggingMiddleware : IMiddleware
  {
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
      // tüm hataları merkezi olarak yakalar.
      try
      {
        // bütün uygulama istekleri geneline try catch koyduk
        await next(context);
      }
      catch (Exception  ex)
      {
        await context.Response.WriteAsync("<h1>Uygulamada beklenmedik bir hata meydana geldi lütfen admin ile iletişeme geçin</h1>");

        string logText = $"Hata Mesajı: {ex.Message} from {context.Request.Method} {context.Request.Path.Value} => {context.Response.StatusCode}{Environment.NewLine}";

        
        File.AppendAllText("request-log.txt",logText);

      }
     
    }
  }
}
