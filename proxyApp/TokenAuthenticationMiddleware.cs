using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

public class TokenAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _expectedToken;

    public TokenAuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _expectedToken = configuration.GetValue<string>("Token"); // تأكد من إضافة هذا المفتاح في appsettings.json
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Exclude the base route ("/") from token authentication
        if (context.Request.Path == "/" || context.Request.Path.StartsWithSegments("/home"))
        {
            await _next(context);
            return;
        }

        // إذا كان الطلب OPTIONS، نتعامل معه كطلب "Preflight" ونسمح له بالمرور
        if (context.Request.Method == HttpMethods.Options)
        {
            // إضافة رؤوس CORS المناسبة
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");

            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("OK");
            return; // لا نكمل الفحص
        }

        // التحقق من وجود header التفويض
        if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Authorization header was not provided.");
            return;
        }

        // استخراج التوكن من header (نفترض شكل "Bearer {token}")
        var token = authHeader.ToString().Split(' ').Last();

        // مقارنة التوكن مع القيمة المتوقعة
        if (token != _expectedToken)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized client.");
            return;
        }

        // إذا كان التوكن صحيحاً، استمر في معالجة الطلب
        await _next(context);
    }
}
