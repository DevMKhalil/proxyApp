using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// إضافة الخدمات الأساسية وإعداد HttpClient والميدلوير
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// تسجيل الميدلوير المخصص للتحقق من API Key
builder.Services.AddSingleton<ApiKeyMiddleware>();

var app = builder.Build();

// تمكين Swagger في بيئة التطوير للمساعدة في التوثيق والاختبار
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// استخدام ميدلوير التحقق من API Key
app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();
app.MapControllers();
app.Run();
