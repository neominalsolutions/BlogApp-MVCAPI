namespace BlogApp.Middlewares
{
  public class ClientCredentialRequestMiddleware
  {
    // süreci bir sonraki işleme aktarmamızı sağlayan delegate sınıfı
    private readonly RequestDelegate _next;

    public ClientCredentialRequestMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {

      // 2 makina peer to peer habeleşeceğiz zaman header üzerinden bazı bilgileri göndererek, apida ilgili client_id ve client_secret bilgisi var.

      var controllerName = context.Request.RouteValues["Controller"];


      if(controllerName != null && controllerName.ToString() == "ClientCredential")
      {
        var clientId = context.Request.Headers.Keys.Contains("client_id");
        var clientSecret = context.Request.Headers.Keys.Contains("client_secret");
        var grantType = context.Request.Headers.Keys.Contains("grant_types");

        if (!clientId || !clientSecret || !grantType)
        {
          context.Response.StatusCode = 400; // bad request
          await context.Response.WriteAsync("Client Credentials bilgileri eksik!");
          return;
        }
      }


     
    
      await _next(context);
    }


  }
}
