using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ForwardingController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly string _ngrokUrl;

    // حقن HttpClient وإعدادات ngrok من التكوين
    public ForwardingController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        //_httpClient = httpClientFactory.CreateClient();
        _httpClient = httpClientFactory.CreateClient("NoSSLValidation");
        _ngrokUrl = configuration.GetSection("TargetSettings")["NgrokUrl"];
    }

    /// <summary>
    /// يعيد توجيه طلبات POST إلى عنوان ngrok المحدد.
    /// </summary>
    [HttpPost("{**catchAll}")]
    public async Task<IActionResult> PostForward()
    {
        // بناء عنوان الهدف بدمج ngrok مع بقية المسار ومعاملات الاستعلام إن وجدت.
        var targetUrl = _ngrokUrl + "/" + Request.Path.Value?.Substring("/api/forwarding".Length).TrimStart('/');
        if (Request.QueryString.HasValue)
        {
            targetUrl += Request.QueryString.Value;
        }

        // إنشاء رسالة الطلب الموجهة
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, targetUrl)
        {
            Content = new StreamContent(Request.Body)
        };

        // تمرير الرؤوس الواردة للطلب الأصلي
        foreach (var header in Request.Headers)
        {
            // استثناء رأس Host
            if (header.Key.Equals("Host", StringComparison.OrdinalIgnoreCase))
                continue;
            if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
            {
                requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }

        // إرسال الطلب وإعادة الرد كما هو
        var responseMessage = await _httpClient.SendAsync(requestMessage);

        // نسخ رؤوس الاستجابة من responseMessage إلى HttpContext.Response
        foreach (var header in responseMessage.Headers)
        {
            // تجاهل الرؤوس التي قد تسبب تضارباً
            if (header.Key.Equals("Transfer-Encoding", StringComparison.OrdinalIgnoreCase) ||
                header.Key.Equals("Content-Length", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            HttpContext.Response.Headers[header.Key] = string.Join(",", header.Value);
        }
        foreach (var header in responseMessage.Content.Headers)
        {
            // تجاهل الرؤوس التي قد تسبب تضارباً
            if (header.Key.Equals("Transfer-Encoding", StringComparison.OrdinalIgnoreCase) ||
                header.Key.Equals("Content-Length", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            HttpContext.Response.Headers[header.Key] = string.Join(",", header.Value);
        }

        var responseContent = await responseMessage.Content.ReadAsStringAsync();

        return new ContentResult
        {
            Content = responseContent,
            StatusCode = (int)responseMessage.StatusCode,
            ContentType = responseMessage.Content.Headers.ContentType?.ToString() ?? "application/json"
        };
    }

    /// <summary>
    /// يعيد توجيه طلبات GET إلى عنوان ngrok المحدد.
    /// </summary>
    [HttpGet("{**catchAll}")]
    public async Task<IActionResult> GetForward()
    {
        var targetUrl = _ngrokUrl + "/" + Request.Path.Value?.Substring("/api/forwarding".Length).TrimStart('/');
        if (Request.QueryString.HasValue)
        {
            targetUrl += Request.QueryString.Value;
        }

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, targetUrl);

        foreach (var header in Request.Headers)
        {
            // استثناء رأس Host
            if (header.Key.Equals("Host", StringComparison.OrdinalIgnoreCase))
                continue;
            requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
        }

        var responseMessage = await _httpClient.SendAsync(requestMessage);

        // نسخ رؤوس الاستجابة من responseMessage إلى HttpContext.Response
        foreach (var header in responseMessage.Headers)
        {
            // تجاهل الرؤوس التي قد تسبب تضارباً
            if (header.Key.Equals("Transfer-Encoding", StringComparison.OrdinalIgnoreCase) ||
                header.Key.Equals("Content-Length", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            HttpContext.Response.Headers[header.Key] = string.Join(",", header.Value);
        }
        foreach (var header in responseMessage.Content.Headers)
        {
            // تجاهل الرؤوس التي قد تسبب تضارباً
            if (header.Key.Equals("Transfer-Encoding", StringComparison.OrdinalIgnoreCase) ||
                header.Key.Equals("Content-Length", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            HttpContext.Response.Headers[header.Key] = string.Join(",", header.Value);
        }

        var responseContent = await responseMessage.Content.ReadAsStringAsync();

        return new ContentResult
        {
            Content = responseContent,
            StatusCode = (int)responseMessage.StatusCode,
            ContentType = responseMessage.Content.Headers.ContentType?.ToString() ?? "application/json"
        };
    }
}
