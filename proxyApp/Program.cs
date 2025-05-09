using proxyApp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<INgrokUrlService, NgrokUrlService>();
// إضافة الخدمات الأساسية وإعداد HttpClient
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("NoSSLValidation")
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
    });


var app = builder.Build();

// تمكين Swagger في بيئة التطوير للمساعدة في التوثيق والاختبار
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// استخدام ميدلوير التحقق من API Key
//app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<TokenAuthenticationMiddleware>();

app.UseAuthorization();
app.MapControllers();
app.Run();
